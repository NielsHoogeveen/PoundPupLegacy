namespace PoundPupLegacy.CreateModel.Inserters;

public class PublisherInserterFactory : DatabaseInserterFactory<Publisher>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableStringDatabaseParameter Name = new() { Name = "name" };

    public override async Task<IDatabaseInserter<Publisher>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "publisher",
            new DatabaseParameter[] {
                Id,
                Name
            }
        );
        return new PublisherInserter(command);
    }

}
public class PublisherInserter : DatabaseInserter<Publisher>
{
    internal PublisherInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(Publisher publisher)
    {
        if (publisher.Id is null)
            throw new NullReferenceException();
        Set(PublisherInserterFactory.Id, publisher.Id.Value);
        Set(PublisherInserterFactory.Name, publisher.Name);
        await _command.ExecuteNonQueryAsync();
    }
}
