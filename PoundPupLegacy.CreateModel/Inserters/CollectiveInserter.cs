namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class CollectiveInserterFactory : SingleIdInserterFactory<Collective>
{
    protected override string TableName => "collective";

    protected override bool AutoGenerateIdentity => false;

}
