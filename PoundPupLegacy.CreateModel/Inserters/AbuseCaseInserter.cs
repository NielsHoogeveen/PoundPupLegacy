namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class AbuseCaseInserterFactory : DatabaseInserterFactory<AbuseCase>
{
    public override async Task<IDatabaseInserter<AbuseCase>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "abuse_case",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = AbuseCaseInserter.ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = AbuseCaseInserter.CHILD_PLACEMENT_TYPE_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = AbuseCaseInserter.FAMILY_SIZE_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = AbuseCaseInserter.HOME_SCHOOLING_INVOLVED,
                    NpgsqlDbType = NpgsqlDbType.Boolean
                },
                new ColumnDefinition{
                    Name = AbuseCaseInserter.FUNDAMENTAL_FAITH_INVOLVED,
                    NpgsqlDbType = NpgsqlDbType.Boolean
                },
                new ColumnDefinition{
                    Name = AbuseCaseInserter.DISABILITIES_INVOLVED,
                    NpgsqlDbType = NpgsqlDbType.Boolean
                },
            }
        );
        return new AbuseCaseInserter(command);

    }

}
internal sealed class AbuseCaseInserter : DatabaseInserter<AbuseCase>
{
    internal const string ID = "id";
    internal const string CHILD_PLACEMENT_TYPE_ID = "child_placement_type_id";
    internal const string FAMILY_SIZE_ID = "family_size_id";
    internal const string HOME_SCHOOLING_INVOLVED = "home_schooling_involved";
    internal const string FUNDAMENTAL_FAITH_INVOLVED = "fundamental_faith_involved";
    internal const string DISABILITIES_INVOLVED = "disabilities_involved";

    internal AbuseCaseInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(AbuseCase abuseCase)
    {
        if (abuseCase.Id is null)
            throw new NullReferenceException();
        SetParameter(abuseCase.Id, ID);
        SetParameter(abuseCase.ChildPlacementTypeId, CHILD_PLACEMENT_TYPE_ID);
        SetNullableParameter(abuseCase.FamilySizeId, FAMILY_SIZE_ID);
        SetNullableParameter(abuseCase.HomeschoolingInvolved, HOME_SCHOOLING_INVOLVED);
        SetNullableParameter(abuseCase.FundamentalFaithInvolved, FUNDAMENTAL_FAITH_INVOLVED);
        SetNullableParameter(abuseCase.DisabilitiesInvolved, DISABILITIES_INVOLVED);
        await _command.ExecuteNonQueryAsync();
    }
}
