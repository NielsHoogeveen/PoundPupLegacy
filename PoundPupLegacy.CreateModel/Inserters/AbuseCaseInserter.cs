namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class AbuseCaseInserterFactory : DatabaseInserterFactory<AbuseCase, AbuseCaseInserter>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableIntegerDatabaseParameter ChildPlacementTypeId = new() { Name = "child_placement_type_id" };
    internal static NullableIntegerDatabaseParameter FamilySizeId = new() { Name = "family_size_id" };
    internal static NullableBooleanDatabaseParameter HomeSchoolingInvolved = new() { Name = "home_schooling_involved" };
    internal static NullableBooleanDatabaseParameter FundamentalFaithInvolved = new() { Name = "fundamental_faith_involved" };
    internal static NullableBooleanDatabaseParameter DisabilitiesInvolved = new() { Name = "disabilities_involved" };
    public override string TableName => "abuse_case";
}
internal sealed class AbuseCaseInserter : DatabaseInserter<AbuseCase>
{

    public AbuseCaseInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetParameterValues(AbuseCase abuseCase)
    {
        if (abuseCase.Id is null)
            throw new NullReferenceException();

        return new ParameterValue[] {
            ParameterValue.Create(AbuseCaseInserterFactory.Id, abuseCase.Id.Value),
            ParameterValue.Create(AbuseCaseInserterFactory.ChildPlacementTypeId, abuseCase.ChildPlacementTypeId),
            ParameterValue.Create(AbuseCaseInserterFactory.FamilySizeId, abuseCase.FamilySizeId),
            ParameterValue.Create(AbuseCaseInserterFactory.HomeSchoolingInvolved, abuseCase.HomeschoolingInvolved),
            ParameterValue.Create(AbuseCaseInserterFactory.FundamentalFaithInvolved, abuseCase.FundamentalFaithInvolved),
            ParameterValue.Create(AbuseCaseInserterFactory.DisabilitiesInvolved, abuseCase.DisabilitiesInvolved),
        };
    }
}
