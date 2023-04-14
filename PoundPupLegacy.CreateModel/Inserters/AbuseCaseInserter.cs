namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = AbuseCaseInserterFactory;
using Inserter = AbuseCaseInserter;
using Request = AbuseCase;

internal sealed class AbuseCaseInserterFactory : IdentifiableDatabaseInserterFactory<Request, Inserter>
{
    internal static NonNullableIntegerDatabaseParameter ChildPlacementTypeId = new() { Name = "child_placement_type_id" };
    internal static NullableIntegerDatabaseParameter FamilySizeId = new() { Name = "family_size_id" };
    internal static NullableBooleanDatabaseParameter HomeSchoolingInvolved = new() { Name = "home_schooling_involved" };
    internal static NullableBooleanDatabaseParameter FundamentalFaithInvolved = new() { Name = "fundamental_faith_involved" };
    internal static NullableBooleanDatabaseParameter DisabilitiesInvolved = new() { Name = "disabilities_involved" };
    public override string TableName => "abuse_case";
}
internal sealed class AbuseCaseInserter : IdentifiableDatabaseInserter<Request>
{

    public AbuseCaseInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.ChildPlacementTypeId, request.ChildPlacementTypeId),
            ParameterValue.Create(Factory.FamilySizeId, request.FamilySizeId),
            ParameterValue.Create(Factory.HomeSchoolingInvolved, request.HomeschoolingInvolved),
            ParameterValue.Create(Factory.FundamentalFaithInvolved, request.FundamentalFaithInvolved),
            ParameterValue.Create(Factory.DisabilitiesInvolved, request.DisabilitiesInvolved),
        };
    }
}
