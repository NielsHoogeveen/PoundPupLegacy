namespace PoundPupLegacy.DomainModel.Inserters;

internal sealed class LocatableInserterFactory : SingleIdInserterFactory<LocatableToCreate>
{
    protected override string TableName => "locatable";

    protected override bool AutoGenerateIdentity => false;

}
