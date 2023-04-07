namespace PoundPupLegacy.CreateModel.Inserters;

public class PublisherInserterFactory : DatabaseInserterFactory<Publisher>
{
    public override async Task<IDatabaseInserter<Publisher>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "publisher",
            new ColumnDefinition[] {
            new ColumnDefinition
            {
                Name = PublisherInserter.ID,
                NpgsqlDbType = NpgsqlDbType.Integer
            },
            new ColumnDefinition
            {
                Name = PublisherInserter.NAME,
                NpgsqlDbType = NpgsqlDbType.Varchar
            },
        });
        return new PublisherInserter(command);
    }

}
public class PublisherInserter : DatabaseInserter<Publisher>
{
    internal const string ID = "id";
    internal const string NAME = "name";

    internal PublisherInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(Publisher publisher)
    {
        SetParameter(publisher.Id, ID);
        SetParameter(publisher.Name, NAME);
        await _command.ExecuteNonQueryAsync();
    }
}
