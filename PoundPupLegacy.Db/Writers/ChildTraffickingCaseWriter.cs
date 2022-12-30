using Npgsql;
using NpgsqlTypes;
using PoundPupLegacy.Model;

namespace PoundPupLegacy.Db.Writers;

internal class ChildTraffickingCaseWriter : DatabaseWriter<ChildTraffickingCase>, IDatabaseWriter<ChildTraffickingCase>
{
    private const string ID = "id";
    private const string NUMBER_OF_CHILDREN_INVOLVED = "number_of_children_involved";
    private const string COUNTRY_ID_FROM = "country_id_from";
    public static DatabaseWriter<ChildTraffickingCase> Create(NpgsqlConnection connection)
    {
        var command = CreateInsertStatement(
            connection,
            "child_trafficking_case",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = NUMBER_OF_CHILDREN_INVOLVED,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = COUNTRY_ID_FROM,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },

            }
        );
        return new ChildTraffickingCaseWriter(command);

    }

    internal ChildTraffickingCaseWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override void Write(ChildTraffickingCase abuseCase)
    {
        try
        {
            WriteValue(abuseCase.Id, ID);
            WriteNullableValue(abuseCase.NumberOfChildrenInvolved, NUMBER_OF_CHILDREN_INVOLVED);
            WriteValue(abuseCase.CountryIdFrom, COUNTRY_ID_FROM);
            _command.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
