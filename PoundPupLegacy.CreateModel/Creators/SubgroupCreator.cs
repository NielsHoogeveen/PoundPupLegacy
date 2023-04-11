namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class SubgroupCreator : EntityCreator<Subgroup>
{
    private readonly IDatabaseInserterFactory<Subgroup> _subgroupInserterFactory;
    private readonly IDatabaseInserterFactory<UserGroup> _userGroupInserterFactory;
    private readonly IDatabaseInserterFactory<PublishingUserGroup> _publishingUserGroupInserterFactory;
    private readonly IDatabaseInserterFactory<Principal> _principalInserterFactory;
    private readonly IDatabaseInserterFactory<UserRole> _userRoleInserterFactory;
    private readonly IDatabaseInserterFactory<AccessRole> _accessRoleInserterFactory;
    private readonly IDatabaseInserterFactory<AdministratorRole> _administratorRoleInserterFactory;
    public SubgroupCreator(
        IDatabaseInserterFactory<Subgroup> subgroupInserterFactory,
        IDatabaseInserterFactory<UserGroup> userGroupInserterFactory,
        IDatabaseInserterFactory<PublishingUserGroup> publishingUserGroupInserterFactory,
        IDatabaseInserterFactory<Principal> principalInserterFactory,
        IDatabaseInserterFactory<UserRole> userRoleInserterFactory,
        IDatabaseInserterFactory<AccessRole> accessRoleInserterFactory,
        IDatabaseInserterFactory<AdministratorRole> administratorRoleInserterFactory
    )
    {
        _subgroupInserterFactory = subgroupInserterFactory;
        _userGroupInserterFactory = userGroupInserterFactory;
        _publishingUserGroupInserterFactory = publishingUserGroupInserterFactory;
        _principalInserterFactory = principalInserterFactory;
        _userRoleInserterFactory = userRoleInserterFactory;
        _accessRoleInserterFactory = accessRoleInserterFactory;
        _administratorRoleInserterFactory = administratorRoleInserterFactory;
    }
    public override async Task CreateAsync(IAsyncEnumerable<Subgroup> subgroups, IDbConnection connection)
    {
        await using var userGroupWriter = await _userGroupInserterFactory.CreateAsync(connection);
        await using var publishingUserGroupWriter = await _publishingUserGroupInserterFactory.CreateAsync(connection);
        await using var subgroupWriter = await _subgroupInserterFactory.CreateAsync(connection);
        await using var principalWriter = await _principalInserterFactory.CreateAsync(connection);
        await using var userRoleWriter = await _userRoleInserterFactory.CreateAsync(connection);
        await using var accessRoleWriter = await _accessRoleInserterFactory.CreateAsync(connection);
        await using var administratorRoleWriter = await _administratorRoleInserterFactory.CreateAsync(connection);


        await foreach (var subgroup in subgroups) {
            await userGroupWriter.InsertAsync(subgroup);
            await publishingUserGroupWriter.InsertAsync(subgroup);
            await subgroupWriter.InsertAsync(subgroup);

            var administratorRole = subgroup.AdministratorRole;
            administratorRole.UserGroupId = subgroup.Id;
            await principalWriter.InsertAsync(administratorRole);
            await userRoleWriter.InsertAsync(administratorRole);
            await administratorRoleWriter.InsertAsync(administratorRole);
        }
    }
}
