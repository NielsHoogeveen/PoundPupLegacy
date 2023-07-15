using PoundPupLegacy.DomainModel;

namespace PoundPupLegacy.DomainModel.Inserters;

internal sealed class CountryAndFirstLevelSubdivisionInserterFactory : SingleIdInserterFactory<CountryAndFirstLevelSubdivisionToCreate>
{
    protected override string TableName => "country_and_first_level_subdivision";

    protected override bool AutoGenerateIdentity => false;

}
