namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(FathersRightsViolationCase.ExistingFathersRightsViolationCase))]
public partial class ExistingFathersRightsViolationCaseJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(FathersRightsViolationCase.NewFathersRightsViolationCase))]
public partial class NewFathersRightsViolationCaseJsonContext : JsonSerializerContext { }

public abstract record FathersRightsViolationCase : Case, ResolvedNode
{
    private FathersRightsViolationCase() { }
    public abstract T Match<T>(Func<ExistingFathersRightsViolationCase, T> existingItem, Func<NewFathersRightsViolationCase, T> newItem);
    public abstract void Match(Action<ExistingFathersRightsViolationCase> existingItem, Action<NewFathersRightsViolationCase> newItem);
    public required CaseDetails CaseDetails { get; init; }
    public required NameableDetails NameableDetails { get; init; }
    public required NodeDetails NodeDetails { get; init; }
    public abstract LocatableDetails LocatableDetails { get; }
    public abstract TenantNodeDetails TenantNodeDetails { get; }

    public sealed record ExistingFathersRightsViolationCase : FathersRightsViolationCase, ExistingNode
    {
        public override TenantNodeDetails TenantNodeDetails => ExistingTenantNodeDetails;
        public required TenantNodeDetails.ExistingTenantNodeDetails ExistingTenantNodeDetails { get; init; }
        public override LocatableDetails LocatableDetails => ExistingLocatableDetails;
        public required LocatableDetails.ExistingLocatableDetails ExistingLocatableDetails { get; init; }
        public required NodeIdentification NodeIdentification { get; init; }
        public override T Match<T>(Func<ExistingFathersRightsViolationCase, T> existingItem, Func<NewFathersRightsViolationCase, T> newItem)
        {
            return existingItem(this);
        }
        public override void Match(Action<ExistingFathersRightsViolationCase> existingItem, Action<NewFathersRightsViolationCase> newItem)
        {
            existingItem(this);
        }

    }
    public sealed record NewFathersRightsViolationCase : FathersRightsViolationCase, ResolvedNewNode
    {
        public override TenantNodeDetails TenantNodeDetails => NewTenantNodeDetails;
        public required TenantNodeDetails.NewTenantNodeDetails NewTenantNodeDetails { get; init; }
        public override LocatableDetails LocatableDetails => NewLocatableDetails;
        public required LocatableDetails.NewLocatableDetails NewLocatableDetails { get; init; }

        public override T Match<T>(Func<ExistingFathersRightsViolationCase, T> existingItem, Func<NewFathersRightsViolationCase, T> newItem)
        {
            return newItem(this);
        }
        public override void Match(Action<ExistingFathersRightsViolationCase> existingItem, Action<NewFathersRightsViolationCase> newItem)
        {
            newItem(this);
        }
    }
}
