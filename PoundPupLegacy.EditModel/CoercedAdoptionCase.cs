namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(CoercedAdoptionCase.ExistingCoercedAdoptionCase))]
public partial class ExistingCoercedAdoptionCaseJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(CoercedAdoptionCase.NewCoercedAdoptionCase))]
public partial class NewCoercedAdoptionCaseJsonContext : JsonSerializerContext { }

public abstract record CoercedAdoptionCase : Case, ResolvedNode
{
    private CoercedAdoptionCase() { }
    public abstract T Match<T>(Func<ExistingCoercedAdoptionCase, T> existingItem, Func<NewCoercedAdoptionCase, T> newItem);
    public abstract void Match(Action<ExistingCoercedAdoptionCase> existingItem, Action<NewCoercedAdoptionCase> newItem);
    public required CaseDetails CaseDetails { get; init; }
    public required NameableDetails NameableDetails { get; init; }
    public abstract LocatableDetails LocatableDetails { get; }
    public abstract NodeDetails NodeDetails { get; }
    public sealed record ExistingCoercedAdoptionCase : CoercedAdoptionCase, ExistingNode
    {
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public required NodeDetails.NodeDetailsForUpdate NodeDetailsForUpdate { get; init; }
        public override LocatableDetails LocatableDetails => ExistingLocatableDetails;
        public required LocatableDetails.ExistingLocatableDetails ExistingLocatableDetails { get; init; }
        public required NodeIdentification NodeIdentification { get; init; }
        public override T Match<T>(Func<ExistingCoercedAdoptionCase, T> existingItem, Func<NewCoercedAdoptionCase, T> newItem)
        {
            return existingItem(this);
        }
        public override void Match(Action<ExistingCoercedAdoptionCase> existingItem, Action<NewCoercedAdoptionCase> newItem)
        {
            existingItem(this);
        }

    }
    public sealed record NewCoercedAdoptionCase : CoercedAdoptionCase, ResolvedNewNode
    {
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public override LocatableDetails LocatableDetails => NewLocatableDetails;
        public required LocatableDetails.NewLocatableDetails NewLocatableDetails { get; init; }

        public override T Match<T>(Func<ExistingCoercedAdoptionCase, T> existingItem, Func<NewCoercedAdoptionCase, T> newItem)
        {
            return newItem(this);
        }
        public override void Match(Action<ExistingCoercedAdoptionCase> existingItem, Action<NewCoercedAdoptionCase> newItem)
        {
            newItem(this);
        }
    }
}
