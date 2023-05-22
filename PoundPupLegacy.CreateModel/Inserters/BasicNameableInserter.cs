namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class BasicNameableInserterFactory : SingleIdInserterFactory<NewBasicNameable>
{
    protected override string TableName => "basic_nameable";

    protected override bool AutoGenerateIdentity => false;

}
