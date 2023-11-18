namespace PoundPupLegacy.DomainModel;

public abstract record UnitedStatesCity : Nameable
{
    private UnitedStatesCity() { }
    public required int UnitedStatesCountyId { get; init; }
    public required decimal Latitude { get; init; }
    public required decimal Longitude { get; init; }
    public required int Population { get; init; }
    public required double Density { get; init; }
    public required bool Military { get; init; }
    public required bool Incorporated { get; init; }
    public required string Timezone { get; init; }
    public required string SimpleName { get; init; }
    public sealed record ToCreate : UnitedStatesCity, NameableToCreate
    {
        public required Identification.Possible Identification { get; init; }
        public required NodeDetails.ForCreate NodeDetails { get; init; }
        public required NameableDetails.ForCreate NameableDetails { get; init; }
    }
    public sealed record ToUpdate : UnitedStatesCity, NameableToUpdate
    {
        public required Identification.Certain Identification { get; init; }
        public required NodeDetails.ForUpdate NodeDetails { get; init; }
        public required NameableDetails.ForUpdate NameableDetails { get; init; }
    }
}
