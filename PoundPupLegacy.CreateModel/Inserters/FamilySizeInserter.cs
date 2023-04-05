namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class FamilySizeInserterFactory : SingleIdInserterFactory<FamilySize>
{
    protected override string TableName => "family_size";

    protected override bool AutoGenerateIdentity => false;

}
