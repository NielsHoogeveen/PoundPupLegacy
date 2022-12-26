using Npgsql;
using NpgsqlTypes;
using PoundPupLegacy.Model;
namespace PoundPupLegacy.Db.Writers;

internal class BasicSecondLevelSubdivisionWriter : DatabaseWriter<BasicSecondLevelSubdivision>, IDatabaseWriter<BasicSecondLevelSubdivision>
{
    private const string ID = "id";
    private const string INTERMEDIATE_LEVEL_SUBDIVISION_ID = "intermediate_level_subdivision_id";

    public static DatabaseWriter<BasicSecondLevelSubdivision> Create(NpgsqlConnection connection)
    {
        var command = CreateInsertStatement(
            connection,
            "basic_second_level_subdivision",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = INTERMEDIATE_LEVEL_SUBDIVISION_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new BasicSecondLevelSubdivisionWriter(command);
    }
    private BasicSecondLevelSubdivisionWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override void Write(BasicSecondLevelSubdivision country)
    {
        WriteValue(country.Id, ID);
        WriteValue(country.IntermediateLevelSubdivisionId, INTERMEDIATE_LEVEL_SUBDIVISION_ID);
        _command.ExecuteNonQuery();
    }
}
