namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class SenatorInserterFactory : SingleIdInserterFactory<EventuallyIdentifiableSenator>
{
    protected override string TableName => "senator";

    protected override bool AutoGenerateIdentity => false;

}
