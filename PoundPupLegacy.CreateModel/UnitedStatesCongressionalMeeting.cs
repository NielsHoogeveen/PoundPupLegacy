namespace PoundPupLegacy.CreateModel;

public sealed record NewUnitedStatesCongressionalMeeting : NewNameableBase, EventuallyIdentifiableUnitedStatesCongressionalMeeting
{
    public required DateTimeRange DateRange { get; init; }
    public required int Number { get; init; }
}
public sealed record ExistingUnitedStatesCongressionalMeeting : ExistingNameableBase, ImmediatelyIdentifiableUnitedStatesCongressionalMeeting
{
    public required DateTimeRange DateRange { get; init; }
    public required int Number { get; init; }
}
public interface ImmediatelyIdentifiableUnitedStatesCongressionalMeeting : UnitedStatesCongressionalMeeting, ImmediatelyIdentifiableNameable, ImmediatelyIdentifiableDocumentable
{
}
public interface EventuallyIdentifiableUnitedStatesCongressionalMeeting : UnitedStatesCongressionalMeeting, EventuallyIdentifiableNameable, EventuallyIdentifiableDocumentable
{
}
public interface UnitedStatesCongressionalMeeting : Nameable, Documentable
{
    DateTimeRange DateRange { get; }
    int Number { get; }
}
