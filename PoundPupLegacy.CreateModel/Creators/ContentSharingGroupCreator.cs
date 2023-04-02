namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class ContentSharingGroupCreator : IEntityCreator<ContentSharingGroup>
{
    public async Task CreateAsync(IAsyncEnumerable<ContentSharingGroup> contentSharingGroups, IDbConnection connection)
    {

        await using var userGroupWriter = await UserGroupInserter.CreateAsync(connection);
        await using var ownerWriter = await OwnerInserter.CreateAsync(connection);
        await using var contentSharingGroupWriter = await ContentSharingGroupInserter.CreateAsync(connection);
        await using var principalWriter = await PrincipalInserter.CreateAsync(connection);
        await using var userRoleWriter = await UserRoleInserter.CreateAsync(connection);
        await using var accessRoleWriter = await AccessRoleInserter.CreateAsync(connection);
        await using var administratorRoleWriter = await AdministratorRoleInserter.CreateAsync(connection);


        await foreach (var contentSharingGroup in contentSharingGroups) {
            await userGroupWriter.InsertAsync(contentSharingGroup);
            await ownerWriter.InsertAsync(contentSharingGroup);
            await contentSharingGroupWriter.InsertAsync(contentSharingGroup);

            var administratorRole = contentSharingGroup.AdministratorRole;
            administratorRole.UserGroupId = contentSharingGroup.Id.Value;
            await principalWriter.InsertAsync(administratorRole);
            await userRoleWriter.InsertAsync(administratorRole);
            await administratorRoleWriter.InsertAsync(administratorRole);

        }
    }
}
