namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class FamilySizeInserterFactory : SingleIdInserterFactory<FamilySize.FamilySizeToCreate>
{
    protected override string TableName => "family_size";

    protected override bool AutoGenerateIdentity => false;

}
