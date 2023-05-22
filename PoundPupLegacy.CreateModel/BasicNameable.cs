namespace PoundPupLegacy.CreateModel;

public sealed record ExistingBasicNameable : ExistingNameableBase, ImmediatelyIdentifiableBasicNameable
{
}

public sealed record NewBasicNameable : NewNameableBase, EventuallyIdentifiableBasicNameable
{
}
public interface ImmediatelyIdentifiableBasicNameable : BasicNameable, ImmediatelyIdentifiableNameable
{
}
public interface EventuallyIdentifiableBasicNameable : BasicNameable, EventuallyIdentifiableNameable
{
}
public interface BasicNameable: Nameable
{
}
