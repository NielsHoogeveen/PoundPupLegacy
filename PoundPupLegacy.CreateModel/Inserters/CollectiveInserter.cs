using PoundPupLegacy.DomainModel;

namespace PoundPupLegacy.DomainModel.Inserters;

internal sealed class CollectiveInserterFactory : SingleIdInserterFactory<Collective>
{
    protected override string TableName => "collective";

    protected override bool AutoGenerateIdentity => false;

}
