using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.Models;
using System;

namespace PoundPupLegacy.Services.Implementation;

public class ChatService(
    NpgsqlDataSource dataSource,
    ILogger<ChatService> logger
) : DatabaseService(dataSource, logger), IChatService
{
    private Dictionary<int, ChatUser> _notifiers = new();
    private class ChatUser
    {
        public event Func<Task>? OnNewMessage;

        public event Func<Task>? OnActivate;

        public async Task InvokeNewMessageAsync()
        {
            if (OnNewMessage is not null) {
                await OnNewMessage.Invoke();
            }
        }
        public async Task InvokeActivateAsync()
        {
            if (OnActivate is not null) {
                await OnActivate.Invoke();
            }
        }
    }
    public async Task Activate(Chat chat, Func<Task> callback)

    {
        await WithConnection(async (connection) => {
            using var command = connection.CreateCommand();
            command.CommandText = """ 
                UPDATE chat_participant
                SET timestamp_last_read = now()
                WHERE chat_id = @id AND publisher_id = @publisher_id
            """;
            command.Parameters.Add("id", NpgsqlTypes.NpgsqlDbType.Integer);
            command.Parameters.Add("publisher_id", NpgsqlTypes.NpgsqlDbType.Integer);
            await command.PrepareAsync();
            command.Parameters["id"].Value = chat.Id;
            command.Parameters["publisher_id"].Value = chat.Self.Id;
            await command.ExecuteNonQueryAsync();
            return Unit.Instance;
        });
        await callback();
        if (_notifiers.TryGetValue(chat.Self.Id, out var notifier)) {
            await notifier.InvokeActivateAsync();
        }
    }

    public async Task OnNewMessage(Chat chat)
    {
        foreach (var participant in chat.Participants) {
            if (_notifiers.TryGetValue(participant.Id, out var notifier)) {
                await notifier.InvokeNewMessageAsync();
            }
        }
    }

    private SemaphoreSlim userSemaphore = new SemaphoreSlim(1, 1);

    public async Task RegisterForActivation(int userId, Func<Task> notification)
    {
        await userSemaphore.WaitAsync();
        try {
            if (_notifiers.TryGetValue(userId, out var notifier)) {
                notifier.OnActivate += notification;
            }
            else {
                notifier = new ChatUser();
                notifier.OnActivate += notification;
                _notifiers.Add(userId, notifier);
            }
        }
        finally {
            userSemaphore.Release();
        }
    }

    public async Task RegisterForNewMessage(int userId, Func<Task> notification)
    {
        await userSemaphore.WaitAsync();
        try {
            if (_notifiers.TryGetValue(userId, out var notifier)) {
                notifier.OnNewMessage += notification;
            }
            else {
                notifier = new ChatUser();
                notifier.OnNewMessage += notification;
                _notifiers.Add(userId, notifier);
            }
        }
        finally {
            userSemaphore.Release();
        }
    }
    public async Task SendMessage(int userId, Chat chat, string message, Func<Task> func)
    {
        await AddMessage(chat.Id, userId, message);
        await func();
        await OnNewMessage(chat);
    }

    private async Task AddChat(int userId, string? name, List<int> participatnIds, Func<int, Task> func)
    {
        var id = await WithTransactedConnection<int>(async (connection) => {
            var command1 = connection.CreateCommand();
            command1.CommandText = """
            insert into chat(name) values(@name);
            select lastval();
            """;
            command1.Parameters.Add("name", NpgsqlTypes.NpgsqlDbType.Text);
            await command1.PrepareAsync();
            if (name is not null) {
                command1.Parameters["name"].Value = name;
            }
            else {
                command1.Parameters["name"].Value = DBNull.Value;
            }
            var result = await command1.ExecuteScalarAsync();
            var id = (int)(long)result!;
            var command2 = connection.CreateCommand();
            command2.CommandText = """
                insert into chat_participant(chat_id, publisher_id) values(@chat_id, @publisher_id);
                """;
            command2.Parameters.Add("chat_id", NpgsqlTypes.NpgsqlDbType.Integer);
            command2.Parameters.Add("publisher_id", NpgsqlTypes.NpgsqlDbType.Integer);
            await command2.PrepareAsync();
            command2.Parameters["chat_id"].Value = id;
            foreach (var participantId in participatnIds) {
                command2.Parameters["publisher_id"].Value = participantId;
                await command2.ExecuteNonQueryAsync();
            }
            return id;
        });
        await func(id);
        if (_notifiers.TryGetValue(userId, out var notifier)) {
            await notifier.InvokeActivateAsync();
        }

    }

    public async Task<int?> GetChat(int userId1, int userId2)
    {
        return await WithConnection<int?>(async (connection) => {
            using var command = connection.CreateCommand();
            command.CommandText = """
                SELECT 
                c.id
                FROM chat c
                JOIN chat_participant cp1 ON cp1.chat_id = c.id and cp1.publisher_id = @publisher_id1
                JOIN chat_participant cp2 ON cp2.chat_id = c.id and cp2.publisher_id = @publisher_id2
                """;
            command.Parameters.Add("publisher_id1", NpgsqlTypes.NpgsqlDbType.Integer);
            command.Parameters.Add("publisher_id2", NpgsqlTypes.NpgsqlDbType.Integer);
            await command.PrepareAsync();
            command.Parameters["publisher_id1"].Value = userId1;
            command.Parameters["publisher_id2"].Value = userId2;
            using var reader = await command.ExecuteReaderAsync();
            if(await reader.ReadAsync()) {
                return (int)reader.GetInt64(0);
            }
            return null;
        });
    }

    public async Task<List<User>> GetUsers(string searchString)
    {
        return await WithSequencedConnection(async (connection) => {
            var lst = new List<User>();
            var command = connection.CreateCommand();
            command.CommandText = """
                select
                p.id,
                p.name
                from publisher p
                join "user" u on u.id = p.id
                where p.name ilike @search_string
                order by p.name
                LIMIT 50
                """;
            command.Parameters.Add("search_string", NpgsqlTypes.NpgsqlDbType.Varchar);
            await command.PrepareAsync();
            command.Parameters["search_string"].Value = $"%{searchString}%";
            using var reader = await command.ExecuteReaderAsync();
            while (reader.Read()) { 
                lst.Add(new User {
                    Id = (int)reader.GetInt64(0),
                    Name = reader.GetString(1)
                });
            }
            return lst;
        });
    }
    private async Task AddMessage(int chatId, int userId, string message)
    {
        await WithSequencedConnection(async (connection) => {
            var lst = new List<User>();
            var command = connection.CreateCommand();
            command.CommandText = """
            INSERT INTO chat_message(
                chat_id, 
                publisher_id, 
                text, 
                timestamp) 
            values (
                @chat_id, 
                @publisher_id, 
                @text, 
                now());
            UPDATE chat_participant set timestamp_last_read = now() 
                where chat_id = @chat_id and publisher_id = @publisher_id;
            """;
            command.Parameters.Add("chat_id", NpgsqlTypes.NpgsqlDbType.Integer);
            command.Parameters.Add("publisher_id", NpgsqlTypes.NpgsqlDbType.Integer);
            command.Parameters.Add("text", NpgsqlTypes.NpgsqlDbType.Text);
            await command.PrepareAsync();
            command.Parameters["chat_id"].Value = chatId;
            command.Parameters["publisher_id"].Value = userId;
            command.Parameters["text"].Value = message;
            await command.ExecuteNonQueryAsync();
            return Unit.Instance;
        });
    }

    public async Task AddChat(int userId1, int userId2, Func<int, Task> func)
    {
        await AddChat(userId1, null, new List<int> { userId1, userId2 }, func);
    }
}
