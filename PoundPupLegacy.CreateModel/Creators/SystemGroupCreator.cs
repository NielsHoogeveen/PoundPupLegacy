namespace PoundPupLegacy.CreateModel.Creators;
internal sealed class SystemGroupCreatorFactory(
    IDatabaseInserterFactory<SystemGroup> systemGroupInserterFactory,
    IDatabaseInserterFactory<UserGroup> userGroupInserterFactory,
    IDatabaseInserterFactory<Owner> ownerInserterFactory,
    IDatabaseInserterFactory<Principal> principalInserterFactory,
    IDatabaseInserterFactory<UserRole> userRoleInserterFactory,
    IDatabaseInserterFactory<AccessRole> accessRoleInserterFactory,
    IDatabaseInserterFactory<AdministratorRole> administratorRoleInserterFactory,
    IEntityCreatorFactory<Vocabulary.ToCreate> vocabularyCreatorFactory
) : IEntityCreatorFactory<SystemGroup>
{
    public async Task<IEntityCreator<SystemGroup>> CreateAsync(IDbConnection connection) =>
        new SystemGroupCreator(
            await userGroupInserterFactory.CreateAsync(connection),
            await ownerInserterFactory.CreateAsync(connection),
            await systemGroupInserterFactory.CreateAsync(connection),
            await principalInserterFactory.CreateAsync(connection),
            await userRoleInserterFactory.CreateAsync(connection),
            await accessRoleInserterFactory.CreateAsync(connection),
            await administratorRoleInserterFactory.CreateAsync(connection),
            await vocabularyCreatorFactory.CreateAsync(connection)
        );
}

internal class SystemGroupCreator(
    IDatabaseInserter<UserGroup> userGroupInserter,
    IDatabaseInserter<Owner> ownerInserter,
    IDatabaseInserter<SystemGroup> systemGroupInserter,
    IDatabaseInserter<Principal> principalInserter,
    IDatabaseInserter<UserRole> userRoleInserter,
    IDatabaseInserter<AccessRole> accessRoleInserter,
    IDatabaseInserter<AdministratorRole> administratorRoleInserter,
    IEntityCreator<Vocabulary.ToCreate> vocabularyCreator
) : EntityCreator<SystemGroup>()
{
    public override async Task ProcessAsync(SystemGroup element)
    {
        await base.ProcessAsync(element);
        await vocabularyCreator.CreateAsync(element.VocabularyTagging);

        await userGroupInserter.InsertAsync(element);
        await ownerInserter.InsertAsync(element);
        await systemGroupInserter.InsertAsync(element);

        var administratorRole = element.AdministratorRole;
        administratorRole.UserGroupId = element.Identification.Id!.Value;
        await principalInserter.InsertAsync(administratorRole);
        await userRoleInserter.InsertAsync(administratorRole);
        await administratorRoleInserter.InsertAsync(administratorRole);

    }
    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await systemGroupInserter.DisposeAsync();
        await userGroupInserter.DisposeAsync();
        await ownerInserter.DisposeAsync();
        await principalInserter.DisposeAsync();
        await userRoleInserter.DisposeAsync();
        await accessRoleInserter.DisposeAsync();
        await administratorRoleInserter.DisposeAsync();
        await vocabularyCreator.DisposeAsync();
    }
}