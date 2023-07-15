namespace PoundPupLegacy.DomainModel.Inserters;

internal sealed class CountryAndIntermediateLevelSubdivisionInserterFactory : SingleIdInserterFactory<CountryAndIntermediateLevelSubdivision.ToCreate>
{
    protected override string TableName => "country_and_intermediate_level_subdivision";

    protected override bool AutoGenerateIdentity => false;

}
