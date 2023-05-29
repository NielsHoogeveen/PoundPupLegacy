namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class HagueStatusInserterFactory : SingleIdInserterFactory<HagueStatus.HagueStatusToCreate>
{
    protected override string TableName => "hague_status";

    protected override bool AutoGenerateIdentity => false;

}
