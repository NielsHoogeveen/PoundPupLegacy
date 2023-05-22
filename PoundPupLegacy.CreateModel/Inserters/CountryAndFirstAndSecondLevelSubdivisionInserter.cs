namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class CountryAndFirstAndSecondLevelSubdivisionInserterFactory : SingleIdInserterFactory<NewCountryAndFirstAndSecondLevelSubdivision>
{
    protected override string TableName => "country_and_first_and_second_level_subdivision";

    protected override bool AutoGenerateIdentity => false;

}
