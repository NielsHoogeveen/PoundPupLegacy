﻿namespace PoundPupLegacy.CreateModel;
public interface CountryAndFirstLevelSubdivisionToUpdate : CountryAndFirstLevelSubdivision, ISOCodedFirstLevelSubdivisionToUpdate, TopLevelCountryToUpdate
{

}
public interface CountryAndFirstLevelSubdivisionToCreate : CountryAndFirstLevelSubdivision, ISOCodedFirstLevelSubdivisionToCreate, TopLevelCountryToCreate
{

}
public interface CountryAndFirstLevelSubdivision : ISOCodedFirstLevelSubdivision, TopLevelCountry
{
}
