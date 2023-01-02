namespace PoundPupLegacy.Db.Writers;

internal class TermWriter : DatabaseWriter<Term>, IDatabaseWriter<Term>
{
    private const string VOCABULARY_ID = "vocabulary_id";
    private const string NAME = "name";
    private const string NAMEABLE_ID = "nameable_id";
    public static DatabaseWriter<Term> Create(NpgsqlConnection connection)
    {
        var command = CreateIdentityInsertStatement(
            connection,
            "vocabulary",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = NAME,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = VOCABULARY_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = NAMEABLE_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new TermWriter(command);

    }

    internal TermWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override void Write(Term term)
    {
        WriteValue(term.VocabularyId, VOCABULARY_ID);
        WriteValue(term.Name, NAME);
        WriteValue(term.NameableId, NAMEABLE_ID);
        term.Id = _command.ExecuteScalar() switch
        {
            int i => i,
            _ => throw new Exception("Id could not be set for term")
        };
    }
}
