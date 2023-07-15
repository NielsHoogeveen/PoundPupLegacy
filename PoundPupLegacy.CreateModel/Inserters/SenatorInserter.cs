namespace PoundPupLegacy.DomainModel.Inserters;

internal sealed class SenatorInserterFactory : SingleIdInserterFactory<Senator.ToCreateForExistingPerson>
{
    protected override string TableName => "senator";

    protected override bool AutoGenerateIdentity => false;

}
