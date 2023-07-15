using PoundPupLegacy.DomainModel;

namespace PoundPupLegacy.DomainModel.Inserters;

internal sealed class PartyInserterFactory : SingleIdInserterFactory<PartyToCreate>
{
    protected override string TableName => "party";

    protected override bool AutoGenerateIdentity => false;

}
