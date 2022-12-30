using Npgsql;
using NpgsqlTypes;
using PoundPupLegacy.Model;

namespace PoundPupLegacy.Db.Writers;

internal class TermWriter : DatabaseWriter<Term>, IDatabaseWriter<Term>
{
    private const string ID = "id";
    private const string NAME = "name";
    private const string VOCABULARY_ID = "vocabulary_id";
    public static DatabaseWriter<Term> Create(NpgsqlConnection connection)
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
                    Name = VOCABULARY_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new TermWriter(command);

    }

    internal TermWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override void Write(Term abuseCase)
    {
        try
        {
            WriteValue(abuseCase.Id, ID);
            WriteValue(abuseCase.Name, NAME);
            WriteValue(abuseCase.VocabularyId, VOCABULARY_ID);
            _command.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
