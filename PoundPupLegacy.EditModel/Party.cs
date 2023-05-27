namespace PoundPupLegacy.EditModel;

public abstract record NewPartyBase : NewLocatableBase, Party
{
}
public abstract record ExistingPartyBase : ExistingLocatableBase, Party
{
}
public interface Party : Locatable
{
}
