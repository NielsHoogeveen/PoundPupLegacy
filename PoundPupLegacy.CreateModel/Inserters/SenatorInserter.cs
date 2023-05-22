namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class SenatorInserterFactory : SingleIdInserterFactory<NewSenator>
{
    protected override string TableName => "senator";

    protected override bool AutoGenerateIdentity => false;

}
