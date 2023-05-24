namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class BasicNameableInserterFactory : SingleIdInserterFactory<EventuallyIdentifiableBasicNameable>
{
    protected override string TableName => "basic_nameable";

    protected override bool AutoGenerateIdentity => false;

}
