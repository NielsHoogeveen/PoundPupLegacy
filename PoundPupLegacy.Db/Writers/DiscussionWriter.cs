namespace PoundPupLegacy.Db.Writers;

internal class DiscussionWriter : DatabaseWriter<Discussion>, IDatabaseWriter<Discussion>
{
    private const string ID = "id";
    private const string TEXT = "text";
    public static DatabaseWriter<Discussion> Create(NpgsqlConnection connection)
    {
        var command = CreateInsertStatement(
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

    internal override void Write(Discussion discussion)
    {
        if (discussion.Id is null)
            throw new NullReferenceException();

        WriteValue(discussion.Id, ID);
        WriteValue(discussion.Text, TEXT);
        _command.ExecuteNonQuery();
    }
}
