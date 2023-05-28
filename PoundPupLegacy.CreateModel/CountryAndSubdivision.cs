namespace PoundPupLegacy.CreateModel;
public interface CountryAndSubdivisionToUpdate : CountryAndSubdivision, TopLevelCountryToUpdate, ISOCodedSubdivisionToUpdate
{
}
public interface CountryAndSubdivisionToCreate : CountryAndSubdivision, TopLevelCountryToCreate, ISOCodedSubdivisionToCreate
{
}
public interface CountryAndSubdivision: TopLevelCountry, ISOCodedSubdivision
{
}

