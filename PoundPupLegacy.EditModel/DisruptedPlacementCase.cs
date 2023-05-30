namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(DisruptedPlacementCase.ToUpdate), TypeInfoPropertyName = "DisruptedPlacementCaseToUpdate")]
public partial class ExistingDisruptedPlacementCaseJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(DisruptedPlacementCase.ToCreate))]
public partial class NewDisruptedPlacementCaseJsonContext : JsonSerializerContext { }

public abstract record DisruptedPlacementCase : Case, ResolvedNode
{
    public required CaseDetails CaseDetails { get; init; }
    public required NameableDetails NameableDetails { get; init; }
    public abstract LocatableDetails LocatableDetails { get; }
    public abstract NodeDetails NodeDetails { get; }
    public sealed record ToUpdate : DisruptedPlacementCase, ExistingNode
    {
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public required NodeDetails.NodeDetailsForUpdate NodeDetailsForUpdate { get; init; }
        public override LocatableDetails LocatableDetails => ExistingLocatableDetails;
        public required LocatableDetails.ExistingLocatableDetails ExistingLocatableDetails { get; init; }
        public required NodeIdentification NodeIdentification { get; init; }

    }
    public sealed record ToCreate : DisruptedPlacementCase, ResolvedNewNode
    {
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public override LocatableDetails LocatableDetails => NewLocatableDetails;
        public required LocatableDetails.NewLocatableDetails NewLocatableDetails { get; init; }

    }
}
