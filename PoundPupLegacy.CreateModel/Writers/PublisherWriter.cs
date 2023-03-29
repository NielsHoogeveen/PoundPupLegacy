namespace PoundPupLegacy.CreateModel.Writers;
public class PublisherWriter : DatabaseWriter<Publisher>, IDatabaseWriter<Publisher>
{
    private const string ID = "id";
    private const string NAME = "name";

    public static async Task<DatabaseWriter<Publisher>> CreateAsync(NpgsqlConnection connection)
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
        return new PublisherWriter(command);
    }
    private PublisherWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override async Task WriteAsync(Publisher publisher)
    {
        WriteValue(publisher.Id, ID);
        WriteValue(publisher.Name, NAME);
        await _command.ExecuteNonQueryAsync();
    }
}
