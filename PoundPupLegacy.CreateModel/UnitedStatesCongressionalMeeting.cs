namespace PoundPupLegacy.DomainModel;

public abstract record UnitedStatesCongressionalMeeting : Nameable, Documentable
{
    private UnitedStatesCongressionalMeeting() { }
    public required UnitedStatesCongressionalMeetingDetails UnitedStatesCongressionalMeetingDetails { get; init; }
    public sealed record ToCreate : UnitedStatesCongressionalMeeting, NameableToCreate, DocumentableToCreate
    {
        public required Identification.Possible Identification { get; init; }
        public required NodeDetails.ForCreate NodeDetails { get; init; }
        public required NameableDetails.ForCreate NameableDetails { get; init; }
    }
    public sealed record ToUpdate : UnitedStatesCongressionalMeeting, NameableToUpdate, DocumentableToUpdate
    {
        public required Identification.Certain Identification { get; init; }
        public required NodeDetails.ForUpdate NodeDetails { get; init; }
        public required NameableDetails.ForUpdate NameableDetails { get; init; }
    }
}

public sealed record UnitedStatesCongressionalMeetingDetails
{
    public required DateTimeRange DateRange { get; init; }
    public required int Number { get; init; }
}
