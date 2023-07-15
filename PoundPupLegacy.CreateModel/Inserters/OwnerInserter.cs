namespace PoundPupLegacy.DomainModel.Inserters;

internal sealed class OwnerInserterFactory : SingleIdInserterFactory<Owner>
{
    protected override string TableName => "owner";

    protected override bool AutoGenerateIdentity => false;

}
