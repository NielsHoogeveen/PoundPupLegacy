namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class CountryAndFirstLevelSubdivisionInserterFactory : SingleIdInserterFactory<CountryAndFirstLevelSubdivision>
{
    protected override string TableName => "country_and_first_level_subdivision";

    protected override bool AutoGenerateIdentity => false;

}
