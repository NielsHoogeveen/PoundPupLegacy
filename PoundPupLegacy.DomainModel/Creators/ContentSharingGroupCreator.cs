namespace PoundPupLegacy.DomainModel.Creators;

internal sealed class ContentSharingGroupCreatorFactory(
    IDatabaseInserterFactory<UserGroup> userGroupInserterFactory,
    IDatabaseInserterFactory<Owner> ownerInserterFactory,
    IDatabaseInserterFactory<ContentSharingGroup> contentSharingGroupInserterFactory,
    IDatabaseInserterFactory<PrincipalToCreate> principalInserterFactory,
    IDatabaseInserterFactory<UserRoleToCreate> userRoleInserterFactory,
    IDatabaseInserterFactory<AccessRole> accessRoleInserterFactory,
    IDatabaseInserterFactory<AdministratorRole> administratorRoleInserterFactory
) : IEntityCreatorFactory<ContentSharingGroup>
{
    public async Task<IEntityCreator<ContentSharingGroup>> CreateAsync(IDbConnection connection) =>
        new ContentSharingGroupCreator(
            new() {
                await userGroupInserterFactory.CreateAsync(connection),
                await ownerInserterFactory.CreateAsync(connection),
                await contentSharingGroupInserterFactory.CreateAsync(connection)
            },
            await principalInserterFactory.CreateAsync(connection),
            await userRoleInserterFactory.CreateAsync(connection),
            await accessRoleInserterFactory.CreateAsync(connection),
            await administratorRoleInserterFactory.CreateAsync(connection)
        );
}

public class ContentSharingGroupCreator(
    List<IDatabaseInserter<ContentSharingGroup>> inserters,
    IDatabaseInserter<PrincipalToCreate> principalInserter,
    IDatabaseInserter<UserRoleToCreate> userRoleInserter,
    IDatabaseInserter<AccessRole> accessRoleInserter,
    IDatabaseInserter<AdministratorRole> administratorRoleInserter
) : InsertingEntityCreator<ContentSharingGroup>(inserters)
{
    public override async Task ProcessAsync(ContentSharingGroup element)
    {
        await base.ProcessAsync(element);
        var administratorRole = element.AdministratorRole;
        administratorRole.UserGroupId = element.Identification.Id;
        await principalInserter.InsertAsync(administratorRole);
        await userRoleInserter.InsertAsync(administratorRole);
        await administratorRoleInserter.InsertAsync(administratorRole);

    }
    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await principalInserter.DisposeAsync();
        await userRoleInserter.DisposeAsync();
        await accessRoleInserter.DisposeAsync();
        await administratorRoleInserter.DisposeAsync();
    }
}