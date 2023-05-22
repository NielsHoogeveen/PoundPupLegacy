namespace PoundPupLegacy.CreateModel;

public sealed record NewBasicCountry : NewTopLevelCountryBase, EventuallyIdentifiableBasicCountry
{
}
public sealed record ExistingBasicCountry : ExistingTopLevelCountryBase, ImmediatelyIdentifiableBasicCountry
{
}
public interface ImmediatelyIdentifiableBasicCountry : BasicCountry, ImmediatelyIdentifiableTopLevelCountry
{
}
public interface EventuallyIdentifiableBasicCountry : BasicCountry, EventuallyIdentifiableTopLevelCountry
{
}
public interface BasicCountry: TopLevelCountry
{
}


