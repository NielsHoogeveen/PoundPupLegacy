using System.Collections.Immutable;

namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class FileInserterFactory: DatabaseInserterFactory<File>
{

    public override async Task<IDatabaseInserter<File>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var collumnDefinitions = new ColumnDefinition[]
        {
            new ColumnDefinition{
                Name = FileInserter.PATH,
                NpgsqlDbType = NpgsqlDbType.Varchar
            },
            new ColumnDefinition{
                Name = FileInserter.NAME,
                NpgsqlDbType = NpgsqlDbType.Varchar
            },
            new ColumnDefinition{
                Name = FileInserter.MIME_TYPE,
                NpgsqlDbType = NpgsqlDbType.Varchar
            },
            new ColumnDefinition{
                Name = FileInserter.SIZE,
                NpgsqlDbType = NpgsqlDbType.Integer
            },
        };

        var commandWithId = await CreateInsertStatementAsync(
            postgresConnection,
            "file",
            collumnDefinitions.ToImmutableList().Prepend(
                new ColumnDefinition {
                    Name = FileInserter.ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                })
        );
        var commandWithoutId = await CreateIdentityInsertStatementAsync(
            postgresConnection,
            "file",
            collumnDefinitions
        );
        return new FileInserter(commandWithId, commandWithoutId);

    }
}

internal sealed class FileInserter : DatabaseInserter<File>
{
    internal const string ID = "id";
    internal const string PATH = "path";
    internal const string NAME = "name";
    internal const string MIME_TYPE = "mime_type";
    internal const string SIZE = "size";

    private NpgsqlCommand _identityCommand;

    internal FileInserter(NpgsqlCommand command, NpgsqlCommand identityCommand) : base(command)
    {
        _identityCommand = identityCommand;
    }

    public override async Task InsertAsync(File file)
    {
        if (file.Id is null) {
            WriteValue(file.Path, PATH, _identityCommand);
            WriteValue(file.Name,   NAME, _identityCommand);
            WriteValue(file.MimeType, MIME_TYPE, _identityCommand);
            WriteValue(file.Size, SIZE, _identityCommand);
            file.Id = await _identityCommand.ExecuteScalarAsync() switch {
                long i => (int)i,
                _ => throw new Exception("No id has been assigned when adding a file"),
            };
        }
        else {
            WriteValue(file.Id, ID);
            WriteValue(file.Path, PATH);
            WriteValue(file.Name, NAME);
            WriteValue(file.MimeType, MIME_TYPE);
            WriteValue(file.Size, SIZE);
            await _command.ExecuteNonQueryAsync();
        }
    }

    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await _identityCommand.DisposeAsync();
    }
}
