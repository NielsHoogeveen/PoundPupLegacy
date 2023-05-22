namespace PoundPupLegacy.CreateModel;

public sealed record NewHagueStatus : NewNameableBase, EventuallyIdentifiableHagueStatus
{
}
public sealed record ExistingHagueStatus : ExistingNameableBase, ImmediatelyIdentifiableHagueStatus
{
}
public interface ImmediatelyIdentifiableHagueStatus : HagueStatus, ImmediatelyIdentifiableNameable
{
}
public interface EventuallyIdentifiableHagueStatus : HagueStatus, EventuallyIdentifiableNameable
{
}

public interface HagueStatus : Nameable
{
}
