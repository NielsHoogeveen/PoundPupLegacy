namespace PoundPupLegacy.Db.Writers;

internal class DiscussionWriter : DatabaseWriter<Discussion>, IDatabaseWriter<Discussion>
{
    private const string ID = "id";
    private const string TEXT = "text";
    public static async Task<DatabaseWriter<Discussion>> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateInsertStatementAsync(
            connection,
            "discussion",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = TEXT,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
            }
        );
        return new DiscussionWriter(command);

    }

    internal DiscussionWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override async Task WriteAsync(Discussion discussion)
    {
        if (discussion.Id is null)
            throw new NullReferenceException();

        WriteValue(discussion.Id, ID);
        WriteValue(discussion.Text, TEXT);
        await _command.ExecuteNonQueryAsync();
    }
}
