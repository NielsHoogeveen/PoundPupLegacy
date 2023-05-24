namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class CountryAndFirstAndSecondLevelSubdivisionInserterFactory : SingleIdInserterFactory<EventuallyIdentifiableCountryAndFirstAndSecondLevelSubdivision>
{
    protected override string TableName => "country_and_first_and_second_level_subdivision";

    protected override bool AutoGenerateIdentity => false;

}
