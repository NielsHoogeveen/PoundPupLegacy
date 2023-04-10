namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class AbuseCaseInserterFactory : DatabaseInserterFactory<AbuseCase>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableIntegerDatabaseParameter ChildPlacementTypeId = new() { Name = "child_placement_type_id" };
    internal static NullableIntegerDatabaseParameter FamilySizeId = new() { Name = "family_size_id" };
    internal static NullableBooleanDatabaseParameter HomeSchoolingInvolved = new() { Name = "home_schooling_involved" };
    internal static NullableBooleanDatabaseParameter FundamentalFaithInvolved = new() { Name = "fundamental_faith_involved" };
    internal static NullableBooleanDatabaseParameter DisabilitiesInvolved = new() { Name = "disabilities_involved" };

    public override async Task<IDatabaseInserter<AbuseCase>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "abuse_case",
            new DatabaseParameter[] {
                Id, 
                ChildPlacementTypeId, 
                FamilySizeId, 
                HomeSchoolingInvolved, 
                FundamentalFaithInvolved, 
                DisabilitiesInvolved
            }
        );
        return new AbuseCaseInserter(command);

    }

}
internal sealed class AbuseCaseInserter : DatabaseInserter<AbuseCase>
{

    internal AbuseCaseInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(AbuseCase abuseCase)
    {
        if (abuseCase.Id is null)
            throw new NullReferenceException();
        Set(AbuseCaseInserterFactory.Id, abuseCase.Id.Value);
        Set(AbuseCaseInserterFactory.ChildPlacementTypeId,abuseCase.ChildPlacementTypeId);
        Set(AbuseCaseInserterFactory.FamilySizeId, abuseCase.FamilySizeId);
        Set(AbuseCaseInserterFactory.HomeSchoolingInvolved, abuseCase.HomeschoolingInvolved);
        Set(AbuseCaseInserterFactory.FundamentalFaithInvolved, abuseCase.FundamentalFaithInvolved);
        Set(AbuseCaseInserterFactory.DisabilitiesInvolved, abuseCase.DisabilitiesInvolved);
        await _command.ExecuteNonQueryAsync();
    }
}
