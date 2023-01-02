namespace PoundPupLegacy.Db.Writers;

internal class VocabularyWriter : DatabaseWriter<Vocabulary>, IDatabaseWriter<Vocabulary>
{
    private const string ID = "id";
    private const string NAME = "name";
    private const string DESCRIPTION = "description";
    public static DatabaseWriter<Vocabulary> Create(NpgsqlConnection connection)
    {
        var command = CreateInsertStatement(
            connection,
            "vocabulary",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = NAME,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = DESCRIPTION,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
            }
        );
        return new VocabularyWriter(command);

    }

    internal VocabularyWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override void Write(Vocabulary vocabulary)
    {
        if (vocabulary.Id is null)
            throw new NullReferenceException();
        WriteValue(vocabulary.Id, ID);
        WriteValue(vocabulary.Name, NAME);
        WriteValue(vocabulary.Description, DESCRIPTION);
        _command.ExecuteNonQuery();
    }
}
