namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class ContentSharingGroupCreator(
    IDatabaseInserterFactory<UserGroup> userGroupInserterFactory,
    IDatabaseInserterFactory<Owner> ownerInserterFactory,
    IDatabaseInserterFactory<ContentSharingGroup> contentSharingGroupInserterFactory,
    IDatabaseInserterFactory<Principal> principalInserterFactory,
    IDatabaseInserterFactory<UserRole> userRoleInserterFactory,
    IDatabaseInserterFactory<AccessRole> accessRoleInserterFactory,
    IDatabaseInserterFactory<AdministratorRole> administratorRoleInserterFactory
) : EntityCreator<ContentSharingGroup>
{

    public override async Task CreateAsync(IAsyncEnumerable<ContentSharingGroup> contentSharingGroups, IDbConnection connection)
    {
        await using var userGroupWriter = await userGroupInserterFactory.CreateAsync(connection);
        await using var ownerWriter = await ownerInserterFactory.CreateAsync(connection);
        await using var contentSharingGroupWriter = await contentSharingGroupInserterFactory.CreateAsync(connection);
        await using var principalWriter = await principalInserterFactory.CreateAsync(connection);
        await using var userRoleWriter = await userRoleInserterFactory.CreateAsync(connection);
        await using var accessRoleWriter = await accessRoleInserterFactory.CreateAsync(connection);
        await using var administratorRoleWriter = await administratorRoleInserterFactory.CreateAsync(connection);

        await foreach (var contentSharingGroup in contentSharingGroups) {
            await userGroupWriter.InsertAsync(contentSharingGroup);
            await ownerWriter.InsertAsync(contentSharingGroup);
            await contentSharingGroupWriter.InsertAsync(contentSharingGroup);

            var administratorRole = contentSharingGroup.AdministratorRole;
            administratorRole.UserGroupId = contentSharingGroup.Id;
            await principalWriter.InsertAsync(administratorRole);
            await userRoleWriter.InsertAsync(administratorRole);
            await administratorRoleWriter.InsertAsync(administratorRole);
        }
    }
}
