namespace PoundPupLegacy.CreateModel;

public sealed record NewBindingCountry : NewTopLevelCountryBase, EventuallyIdentifiableTopLevelCountry
{
}
public sealed record ExistingBindingCountry : ExistingTopLevelCountryBase, ImmediatelyIdentifiableTopLevelCountry
{
}




