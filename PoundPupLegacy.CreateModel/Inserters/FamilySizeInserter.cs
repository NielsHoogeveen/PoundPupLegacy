namespace PoundPupLegacy.DomainModel.Inserters;

internal sealed class FamilySizeInserterFactory : SingleIdInserterFactory<FamilySize.ToCreate>
{
    protected override string TableName => "family_size";

    protected override bool AutoGenerateIdentity => false;

}
