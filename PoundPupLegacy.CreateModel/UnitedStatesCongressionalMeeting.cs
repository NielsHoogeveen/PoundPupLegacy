namespace PoundPupLegacy.CreateModel;

public abstract record UnitedStatesCongressionalMeeting : Nameable, Documentable
{
    private UnitedStatesCongressionalMeeting() { }
    public required UnitedStatesCongressionalMeetingDetails UnitedStatesCongressionalMeetingDetails { get; init; }
    public abstract Identification Identification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public abstract NameableDetails NameableDetails { get; }
    public abstract T Match<T>(Func<UnitedStatesCongressionalMeetingToCreate, T> create, Func<UnitedStatesCongressionalMeetingToUpdate, T> update);
    public abstract void Match(Action<UnitedStatesCongressionalMeetingToCreate> create, Action<UnitedStatesCongressionalMeetingToUpdate> update);

    public sealed record UnitedStatesCongressionalMeetingToCreate : UnitedStatesCongressionalMeeting, NameableToCreate, DocumentableToCreate
    {
        public override Identification Identification => IdentificationForCreate;
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public override NameableDetails NameableDetails => NameableDetailsForCreate;
        public required Identification.IdentificationForCreate IdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public required NameableDetails.NameableDetailsForCreate NameableDetailsForCreate { get; init; }
        public override T Match<T>(Func<UnitedStatesCongressionalMeetingToCreate, T> create, Func<UnitedStatesCongressionalMeetingToUpdate, T> update)
        {
            return create(this);
        }
        public override void Match(Action<UnitedStatesCongressionalMeetingToCreate> create, Action<UnitedStatesCongressionalMeetingToUpdate> update)
        {
            create(this);
        }
    }
    public sealed record UnitedStatesCongressionalMeetingToUpdate : UnitedStatesCongressionalMeeting, NameableToUpdate, DocumentableToUpdate
    {
        public required Identification.IdentificationForUpdate IdentificationForUpdate { get; init; }
        public required NodeDetails.NodeDetailsForUpdate NodeDetailsForUpdate { get; init; }
        public override NameableDetails NameableDetails => NameableDetailsForUpdate;
        public override Identification Identification => IdentificationForUpdate;
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public required NameableDetails.NameableDetailsForUpdate NameableDetailsForUpdate { get; init; }
        public override T Match<T>(Func<UnitedStatesCongressionalMeetingToCreate, T> create, Func<UnitedStatesCongressionalMeetingToUpdate, T> update)
        {
            return update(this);
        }
        public override void Match(Action<UnitedStatesCongressionalMeetingToCreate> create, Action<UnitedStatesCongressionalMeetingToUpdate> update)
        {
            update(this);
        }
    }
}

public sealed record UnitedStatesCongressionalMeetingDetails
{
    public required DateTimeRange DateRange { get; init; }
    public required int Number { get; init; }
}
