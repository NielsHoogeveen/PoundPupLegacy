using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("PoundPupLegacy.Db.Test")]
namespace PoundPupLegacy.Db.Writers;

internal class AbuseCaseWriter : DatabaseWriter<AbuseCase>, IDatabaseWriter<AbuseCase>
{
    private const string ID = "id";
    private const string CHILD_PLACEMENT_TYPE_ID = "child_placement_type_id";
    private const string FAMILY_SIZE_ID = "family_size_id";
    private const string HOME_SCHOOLING_INVOLVED = "home_schooling_involved";
    private const string FUNDAMENTAL_FAITH_INVOLVED = "fundamental_faith_involved";
    private const string DISABILITIES_INVOLVED = "disabilities_involved";
    public static DatabaseWriter<AbuseCase> Create(NpgsqlConnection connection)
    {
        var command = CreateInsertStatement(
            connection,
            "abuse_case",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = CHILD_PLACEMENT_TYPE_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = FAMILY_SIZE_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = HOME_SCHOOLING_INVOLVED,
                    NpgsqlDbType = NpgsqlDbType.Boolean
                },
                new ColumnDefinition{
                    Name = FUNDAMENTAL_FAITH_INVOLVED,
                    NpgsqlDbType = NpgsqlDbType.Boolean
                },
                new ColumnDefinition{
                    Name = DISABILITIES_INVOLVED,
                    NpgsqlDbType = NpgsqlDbType.Boolean
                },
            }
        );
        return new AbuseCaseWriter(command);

    }

    internal AbuseCaseWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override void Write(AbuseCase abuseCase)
    {
        try
        {
            WriteValue(abuseCase.Id, ID);
            WriteValue(abuseCase.ChildPlacementTypeId, CHILD_PLACEMENT_TYPE_ID);
            WriteNullableValue(abuseCase.FamilySizeId, FAMILY_SIZE_ID);
            WriteNullableValue(abuseCase.HomeschoolingInvolved, HOME_SCHOOLING_INVOLVED);
            WriteNullableValue(abuseCase.FundamentalFaithInvolved, FUNDAMENTAL_FAITH_INVOLVED);
            WriteNullableValue(abuseCase.DisabilitiesInvolved, DISABILITIES_INVOLVED);
            _command.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
