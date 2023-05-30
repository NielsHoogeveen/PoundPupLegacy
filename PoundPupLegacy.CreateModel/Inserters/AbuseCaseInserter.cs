namespace PoundPupLegacy.CreateModel.Inserters;

using Request = AbuseCase.ToCreate;

internal sealed class AbuseCaseInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    private static readonly NonNullableIntegerDatabaseParameter ChildPlacementTypeId = new() { Name = "child_placement_type_id" };
    private static readonly NullableIntegerDatabaseParameter FamilySizeId = new() { Name = "family_size_id" };
    private static readonly NullableBooleanDatabaseParameter HomeSchoolingInvolved = new() { Name = "home_schooling_involved" };
    private static readonly NullableBooleanDatabaseParameter FundamentalFaithInvolved = new() { Name = "fundamental_faith_involved" };
    private static readonly NullableBooleanDatabaseParameter DisabilitiesInvolved = new() { Name = "disabilities_involved" };
    public override string TableName => "abuse_case";

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(ChildPlacementTypeId, request.AbuseCaseDetails.ChildPlacementTypeId),
            ParameterValue.Create(FamilySizeId, request.AbuseCaseDetails.FamilySizeId),
            ParameterValue.Create(HomeSchoolingInvolved, request.AbuseCaseDetails.HomeschoolingInvolved),
            ParameterValue.Create(FundamentalFaithInvolved, request.AbuseCaseDetails.FundamentalFaithInvolved),
            ParameterValue.Create(DisabilitiesInvolved, request.AbuseCaseDetails.DisabilitiesInvolved),
        };
    }
}
