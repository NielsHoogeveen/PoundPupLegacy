using PoundPupLegacy.DomainModel;

namespace PoundPupLegacy.DomainModel.Inserters;

internal sealed class SecondLevelSubdivisionInserterFactory : SingleIdInserterFactory<SecondLevelSubdivisionToCreate>
{
    protected override string TableName => "second_level_subdivision";

    protected override bool AutoGenerateIdentity => false;

}
