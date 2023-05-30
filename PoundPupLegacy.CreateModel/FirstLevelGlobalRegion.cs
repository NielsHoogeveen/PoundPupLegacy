namespace PoundPupLegacy.CreateModel;

public abstract record FirstLevelGlobalRegion : GlobalRegion
{
    private FirstLevelGlobalRegion() { }
    public required GlobalRegionDetails GlobalRegionDetails { get; init; }
    public abstract Identification Identification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public abstract NameableDetails NameableDetails { get; }
    public sealed record ToCreate : FirstLevelGlobalRegion, GlobalRegionToCreate
    {
        public override Identification Identification => IdentificationForCreate;
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public override NameableDetails NameableDetails => NameableDetailsForCreate;
        public required Identification.Possible IdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public required NameableDetails.NameableDetailsForCreate NameableDetailsForCreate { get; init; }
    }
    public sealed record ToUpdate : FirstLevelGlobalRegion, GlobalRegionToUpdate
    {
        public required Identification.Certain IdentificationCertain { get; init; }
        public required NodeDetails.ForUpdate NodeDetailsForUpdate { get; init; }
        public override NameableDetails NameableDetails => NameableDetailsForUpdate;
        public override Identification Identification => IdentificationCertain;
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public required NameableDetails.NameableDetailsForUpdate NameableDetailsForUpdate { get; init; }
    }
}
