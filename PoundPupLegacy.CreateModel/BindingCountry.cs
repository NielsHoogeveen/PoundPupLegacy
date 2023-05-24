namespace PoundPupLegacy.CreateModel;

public sealed record NewBindingCountry : NewTopLevelCountryBase, EventuallyIdentifiableBindingCountry
{
}
public sealed record ExistingBindingCountry : ExistingTopLevelCountryBase, ImmediatelyIdentifiableBindingCountry
{
}
public interface ImmediatelyIdentifiableBindingCountry : BindingCountry, ImmediatelyIdentifiableTopLevelCountry
{

}
public interface EventuallyIdentifiableBindingCountry : BindingCountry, EventuallyIdentifiableTopLevelCountry
{

}
public interface BindingCountry: TopLevelCountry
{

}




