namespace PoundPupLegacy.CreateModel.Creators;
internal sealed class SystemGroupCreator(
    IDatabaseInserterFactory<SystemGroup> systemGroupInserterFactory,
    IDatabaseInserterFactory<UserGroup> userGroupInserterFactory,
    IDatabaseInserterFactory<Owner> ownerInserterFactory,
    IDatabaseInserterFactory<Principal> principalInserterFactory,
    IDatabaseInserterFactory<UserRole> userRoleInserterFactory,
    IDatabaseInserterFactory<AccessRole> accessRoleInserterFactory,
    IDatabaseInserterFactory<AdministratorRole> administratorRoleInserterFactory,
    IEntityCreator<Vocabulary> vocabularyCreator
) : EntityCreator<SystemGroup>
{
    public override async Task CreateAsync(IAsyncEnumerable<SystemGroup> elements, IDbConnection connection)
    {
        await using var userGroupWriter = await userGroupInserterFactory.CreateAsync(connection);
        await using var ownerWriter = await ownerInserterFactory.CreateAsync(connection);
        await using var systemGroupWriter = await systemGroupInserterFactory.CreateAsync(connection);
        await using var principalWriter = await principalInserterFactory.CreateAsync(connection);
        await using var userRoleWriter = await userRoleInserterFactory.CreateAsync(connection);
        await using var accessRoleWriter = await accessRoleInserterFactory.CreateAsync(connection);
        await using var administratorRoleWriter = await administratorRoleInserterFactory.CreateAsync(connection);

        await foreach (var systemGroup in elements) {
            await vocabularyCreator.CreateAsync(systemGroup.VocabularyTagging, connection);

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
