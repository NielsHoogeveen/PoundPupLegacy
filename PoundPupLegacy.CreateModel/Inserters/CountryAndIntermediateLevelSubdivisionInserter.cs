namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class CountryAndIntermediateLevelSubdivisionInserterFactory : SingleIdInserterFactory<CountryAndIntermediateLevelSubdivision.CountryAndIntermediateLevelSubdivisionToCreate>
{
    protected override string TableName => "country_and_intermediate_level_subdivision";

    protected override bool AutoGenerateIdentity => false;

}
