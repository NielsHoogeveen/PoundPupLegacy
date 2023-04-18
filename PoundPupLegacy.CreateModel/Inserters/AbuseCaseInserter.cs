namespace PoundPupLegacy.CreateModel.Inserters;

using Request = AbuseCase;

internal sealed class AbuseCaseInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    internal static NonNullableIntegerDatabaseParameter ChildPlacementTypeId = new() { Name = "child_placement_type_id" };
    internal static NullableIntegerDatabaseParameter FamilySizeId = new() { Name = "family_size_id" };
    internal static NullableBooleanDatabaseParameter HomeSchoolingInvolved = new() { Name = "home_schooling_involved" };
    internal static NullableBooleanDatabaseParameter FundamentalFaithInvolved = new() { Name = "fundamental_faith_involved" };
    internal static NullableBooleanDatabaseParameter DisabilitiesInvolved = new() { Name = "disabilities_involved" };
    public override string TableName => "abuse_case";

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(ChildPlacementTypeId, request.ChildPlacementTypeId),
            ParameterValue.Create(FamilySizeId, request.FamilySizeId),
            ParameterValue.Create(HomeSchoolingInvolved, request.HomeschoolingInvolved),
            ParameterValue.Create(FundamentalFaithInvolved, request.FundamentalFaithInvolved),
            ParameterValue.Create(DisabilitiesInvolved, request.DisabilitiesInvolved),
        };
    }
}
