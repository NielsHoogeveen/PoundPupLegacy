namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class FirstLevelGlobalRegionInserterFactory : SingleIdInserterFactory<EventuallyIdentifiableFirstLevelGlobalRegion>
{
    protected override string TableName => "first_level_global_region";

    protected override bool AutoGenerateIdentity => false;

}
