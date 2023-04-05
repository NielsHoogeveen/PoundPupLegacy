namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class ActionInserterFactory : SingleIdInserterFactory<Action>
{
    protected override string TableName => "action";

    protected override bool AutoGenerateIdentity => true;

}
