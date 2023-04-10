using System.Collections.Immutable;

namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class FileInserterFactory: DatabaseInserterFactory<File>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableStringDatabaseParameter Path = new() { Name = "path" };
    internal static NonNullableStringDatabaseParameter Name = new() { Name = "name" };
    internal static NonNullableStringDatabaseParameter MimeType = new() { Name = "mime_type" };
    internal static NonNullableIntegerDatabaseParameter Size = new() { Name = "size" };

    public override async Task<IDatabaseInserter<File>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var databaseParameters = new DatabaseParameter[]
        {
            Path,
            Name,
            MimeType,
            Size
        };

        var commandWithId = await CreateInsertStatementAsync(
            postgresConnection,
            "file",
            databaseParameters.ToImmutableList().Prepend(Id)
        );
        var commandWithoutId = await CreateIdentityInsertStatementAsync(
            postgresConnection,
            "file",
            databaseParameters
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
            Set(FileInserterFactory.Path, file.Path, _identityCommand);
            Set(FileInserterFactory.Name, file.Name, _identityCommand);
            Set(FileInserterFactory.MimeType, file.MimeType, _identityCommand);
            Set(FileInserterFactory.Size, file.Size, _identityCommand);
            file.Id = await _identityCommand.ExecuteScalarAsync() switch {
                long i => (int)i,
                _ => throw new Exception("No id has been assigned when adding a file"),
            };
        }
        else {
            Set(FileInserterFactory.Id, file.Id.Value);
            Set(FileInserterFactory.Path, file.Path);
            Set(FileInserterFactory.Name, file.Name);
            Set(FileInserterFactory.MimeType, file.MimeType);
            Set(FileInserterFactory.Size, file.Size);
            await _command.ExecuteNonQueryAsync();
        }
    }

    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await _identityCommand.DisposeAsync();
    }
}
