using PoundPupLegacy.DomainModel;

namespace PoundPupLegacy.DomainModel.Inserters;

internal sealed class CongressionalTermInserterFactory : SingleIdInserterFactory<CongressionalTermToCreate>
{
    protected override string TableName => "congressional_term";

    protected override bool AutoGenerateIdentity => false;

}
