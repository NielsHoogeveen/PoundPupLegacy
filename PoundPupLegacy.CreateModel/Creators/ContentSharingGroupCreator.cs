namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class ContentSharingGroupCreator : EntityCreator<ContentSharingGroup>
{
    private readonly IDatabaseInserterFactory<UserGroup> _userGroupInserterFactory;
    private readonly IDatabaseInserterFactory<Owner> _ownerInserterFactory;
    private readonly IDatabaseInserterFactory<ContentSharingGroup> _contentSharingGroupInserterFactory;
    private readonly IDatabaseInserterFactory<Principal> _principalInserterFactory;
    private readonly IDatabaseInserterFactory<UserRole> _userRoleInserterFactory;
    private readonly IDatabaseInserterFactory<AccessRole> _accessRoleInserterFactory;
    private readonly IDatabaseInserterFactory<AdministratorRole> _administratorRoleInserterFactory;
    //Add constructor
    public ContentSharingGroupCreator(
        IDatabaseInserterFactory<UserGroup> userGroupInserterFactory,
        IDatabaseInserterFactory<Owner> ownerInserterFactory,
        IDatabaseInserterFactory<ContentSharingGroup> contentSharingGroupInserterFactory,
        IDatabaseInserterFactory<Principal> principalInserterFactory,
        IDatabaseInserterFactory<UserRole> userRoleInserterFactory,
        IDatabaseInserterFactory<AccessRole> accessRoleInserterFactory,
        IDatabaseInserterFactory<AdministratorRole> administratorRoleInserterFactory
    )
    {
        _userGroupInserterFactory = userGroupInserterFactory;
        _ownerInserterFactory = ownerInserterFactory;
        _contentSharingGroupInserterFactory = contentSharingGroupInserterFactory;
        _principalInserterFactory = principalInserterFactory;
        _userRoleInserterFactory = userRoleInserterFactory;
        _accessRoleInserterFactory = accessRoleInserterFactory;
        _administratorRoleInserterFactory = administratorRoleInserterFactory;
    }

    public override async Task CreateAsync(IAsyncEnumerable<ContentSharingGroup> contentSharingGroups, IDbConnection connection)
    {

        await using var userGroupWriter = await _userGroupInserterFactory.CreateAsync(connection);
        await using var ownerWriter = await _ownerInserterFactory.CreateAsync(connection);
        await using var contentSharingGroupWriter = await _contentSharingGroupInserterFactory.CreateAsync(connection);
        await using var principalWriter = await _principalInserterFactory.CreateAsync(connection);
        await using var userRoleWriter = await _userRoleInserterFactory.CreateAsync(connection);
        await using var accessRoleWriter = await _accessRoleInserterFactory.CreateAsync(connection);
        await using var administratorRoleWriter = await _administratorRoleInserterFactory.CreateAsync(connection);


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
