namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class SenatorInserterFactory : SingleIdInserterFactory<Senator.SenatorToCreate>
{
    protected override string TableName => "senator";

    protected override bool AutoGenerateIdentity => false;

}
