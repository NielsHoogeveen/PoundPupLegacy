namespace PoundPupLegacy.CreateModel.Creators;
internal sealed class SystemGroupCreator : EntityCreator<SystemGroup>
{
    private readonly IDatabaseInserterFactory<SystemGroup> _systemGroupInserterFactory;
    private readonly IDatabaseInserterFactory<UserGroup> _userGroupInserterFactory;
    private readonly IDatabaseInserterFactory<Owner> _ownerInserterFactory;
    private readonly IDatabaseInserterFactory<Principal> _principalInserterFactory;
    private readonly IDatabaseInserterFactory<UserRole> _userRoleInserterFactory;
    private readonly IDatabaseInserterFactory<AccessRole> _accessRoleInserterFactory;
    private readonly IDatabaseInserterFactory<AdministratorRole> _administratorRoleInserterFactory;
    private readonly IEntityCreator<Vocabulary> _vocabularyCreator;
    public SystemGroupCreator(
        IDatabaseInserterFactory<SystemGroup> systemGroupInserterFactory,
        IDatabaseInserterFactory<UserGroup> userGroupInserterFactory,
        IDatabaseInserterFactory<Owner> ownerInserterFactory,
        IDatabaseInserterFactory<Principal> principalInserterFactory,
        IDatabaseInserterFactory<UserRole> userRoleInserterFactory,
        IDatabaseInserterFactory<AccessRole> accessRoleInserterFactory,
        IDatabaseInserterFactory<AdministratorRole> administratorRoleInserterFactory,
        IEntityCreator<Vocabulary> vocabularyCreator
    )
    {
        _systemGroupInserterFactory = systemGroupInserterFactory;
        _userGroupInserterFactory = userGroupInserterFactory;
        _ownerInserterFactory = ownerInserterFactory;
        _principalInserterFactory = principalInserterFactory;
        _userRoleInserterFactory = userRoleInserterFactory;
        _accessRoleInserterFactory = accessRoleInserterFactory;
        _administratorRoleInserterFactory = administratorRoleInserterFactory;
        _vocabularyCreator = vocabularyCreator;
    }
    public override async Task CreateAsync(IAsyncEnumerable<SystemGroup> elements, IDbConnection connection)
    {
        await using var userGroupWriter = await _userGroupInserterFactory.CreateAsync(connection);
        await using var ownerWriter = await _ownerInserterFactory.CreateAsync(connection);
        await using var systemGroupWriter = await _systemGroupInserterFactory.CreateAsync(connection);
        await using var principalWriter = await _principalInserterFactory.CreateAsync(connection);
        await using var userRoleWriter = await _userRoleInserterFactory.CreateAsync(connection);
        await using var accessRoleWriter = await _accessRoleInserterFactory.CreateAsync(connection);
        await using var administratorRoleWriter = await _administratorRoleInserterFactory.CreateAsync(connection);

        await foreach (var systemGroup in elements) {
            await _vocabularyCreator.CreateAsync(systemGroup.VocabularyTagging, connection);

            await userGroupWriter.InsertAsync(systemGroup);
            await ownerWriter.InsertAsync(systemGroup);
            await systemGroupWriter.InsertAsync(systemGroup);

            var administratorRole = systemGroup.AdministratorRole;
            administratorRole.UserGroupId = systemGroup.Id!.Value;
            await principalWriter.InsertAsync(administratorRole);
            await userRoleWriter.InsertAsync(administratorRole);
            await administratorRoleWriter.InsertAsync(administratorRole);


        }
    }
}
