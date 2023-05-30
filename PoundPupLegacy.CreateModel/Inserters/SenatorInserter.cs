namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class SenatorInserterFactory : SingleIdInserterFactory<Senator.SenatorToCreateForExistingPerson>
{
    protected override string TableName => "senator";

    protected override bool AutoGenerateIdentity => false;

}
