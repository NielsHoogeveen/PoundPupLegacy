namespace PoundPupLegacy.CreateModel.Inserters;
public class PublisherInserter : DatabaseInserter<Publisher>, IDatabaseInserter<Publisher>
{
    private const string ID = "id";
    private const string NAME = "name";

    public static async Task<DatabaseInserter<Publisher>> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateInsertStatementAsync(
            connection,
            "publisher",
            new ColumnDefinition[] {
            new ColumnDefinition
            {
                Name = ID,
                NpgsqlDbType = NpgsqlDbType.Integer
            },
            new ColumnDefinition
            {
                Name = NAME,
                NpgsqlDbType = NpgsqlDbType.Varchar
            },
        });
        return new PublisherInserter(command);
    }
    private PublisherInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(Publisher publisher)
    {
        WriteValue(publisher.Id, ID);
        WriteValue(publisher.Name, NAME);
        await _command.ExecuteNonQueryAsync();
    }
}
