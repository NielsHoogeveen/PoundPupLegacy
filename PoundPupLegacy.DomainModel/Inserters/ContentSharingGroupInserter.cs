namespace PoundPupLegacy.DomainModel.Inserters;

internal sealed class ContentSharingGroupInserterFactory : SingleIdInserterFactory<ContentSharingGroup>
{
    protected override string TableName => "content_sharing_group";

    protected override bool AutoGenerateIdentity => false;

}
