using PoundPupLegacy.DomainModel;

namespace PoundPupLegacy.DomainModel.Inserters;

internal sealed class CountryAndFirstAndBottomLevelSubdivisionInserterFactory : SingleIdInserterFactory<CountryAndFirstAndBottomLevelSubdivision.ToCreate>
{
    protected override string TableName => "country_and_first_and_bottom_level_subdivision";

    protected override bool AutoGenerateIdentity => false;

}
