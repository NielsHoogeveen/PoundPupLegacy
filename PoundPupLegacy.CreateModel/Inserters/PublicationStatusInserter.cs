namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class PublicationStatusInserterFactory : DatabaseInserterFactory<PublicationStatus>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableStringDatabaseParameter Name = new() { Name = "name" };

    public override async Task<IDatabaseInserter<PublicationStatus>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "publication_status",
            new DatabaseParameter[] {
                Id,
                Name
            }
        );
        return new PublicationStatusInserter(command);
    }
}
internal sealed class PublicationStatusInserter : DatabaseInserter<PublicationStatus>
{

    internal PublicationStatusInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(PublicationStatus nodeStatus)
    {
        if (nodeStatus.Id is null)
            throw new NullReferenceException();

        Set(PublicationStatusInserterFactory.Id, nodeStatus.Id.Value);
        Set(PublicationStatusInserterFactory.Name, nodeStatus.Name);
        await _command.ExecuteNonQueryAsync();
    }
}
