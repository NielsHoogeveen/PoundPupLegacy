namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class LocatableInserterFactory : SingleIdInserterFactory<Locatable>
{
    protected override string TableName => "locatable";

    protected override bool AutoGenerateIdentity => false;

}
