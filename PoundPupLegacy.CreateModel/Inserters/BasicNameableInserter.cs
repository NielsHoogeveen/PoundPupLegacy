namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class BasicNameableInserterFactory : SingleIdInserterFactory<BasicNameable>
{
    protected override string TableName => "basic_nameable";

    protected override bool AutoGenerateIdentity => false;

}
