using Npgsql;
using NpgsqlTypes;
using PoundPupLegacy.Model;

namespace PoundPupLegacy.Db.Writers;

internal class CaseWriter : DatabaseWriter<Case>, IDatabaseWriter<Case>
{
    private const string ID = "id";
    private const string DESCRIPTION = "description";
    private const string DATE = "date";
    private const string DATERANGE = "date_range";
    public static DatabaseWriter<Case> Create(NpgsqlConnection connection)
    {
        var command = CreateInsertStatement(
            connection,
            "case",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = DESCRIPTION,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = DATE,
                    NpgsqlDbType = NpgsqlDbType.Unknown
                },
                new ColumnDefinition{
                    Name = DATERANGE,
                    NpgsqlDbType = NpgsqlDbType.Unknown
                },
            }
        );
        return new CaseWriter(command);

    }

    internal CaseWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override void Write(Case therapist)
    {
        WriteValue(therapist.Id, ID);
        WriteValue(therapist.Description, DESCRIPTION);
        WriteDateTimeRange(therapist.Date, DATE, DATERANGE);
        _command.ExecuteNonQuery();
    }
}
