namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class CountryAndFirstAndBottomLevelSubdivisionInserterFactory : SingleIdInserterFactory<NewCountryAndFirstAndBottomLevelSubdivision>
{
    protected override string TableName => "country_and_first_and_bottom_level_subdivision";

    protected override bool AutoGenerateIdentity => false;

}
