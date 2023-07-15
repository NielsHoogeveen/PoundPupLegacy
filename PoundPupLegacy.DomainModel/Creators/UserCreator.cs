using PoundPupLegacy.DomainModel;

namespace PoundPupLegacy.DomainModel.Creators;

internal sealed class UserCreatorFactory(
    IDatabaseInserterFactory<PrincipalToCreate> principalInserterFactory,
    IDatabaseInserterFactory<PublisherToCreate> publisherInserterFactory,
    IDatabaseInserterFactory<User.ToCreate> userInserterFactory,
    IDatabaseInserterFactory<UserRoleUser> userGroupUserRoleUserInserterFactory
) : IEntityCreatorFactory<User.ToCreate>
{
    public async Task<IEntityCreator<User.ToCreate>> CreateAsync(IDbConnection connection) =>
        new UserCreator(
            new() {
                await principalInserterFactory.CreateAsync(connection),
                await publisherInserterFactory.CreateAsync(connection),
                await userInserterFactory.CreateAsync(connection)
            },
            await userGroupUserRoleUserInserterFactory.CreateAsync(connection)
        );
}
internal sealed class UserCreator(
    List<IDatabaseInserter<User.ToCreate>> inserters,
    IDatabaseInserter<UserRoleUser> userGroupUserRoleUserInserter
) : InsertingEntityCreator<User.ToCreate>(inserters)
{
    public override async Task ProcessAsync(User.ToCreate element)
    {
        await base.ProcessAsync(element);
        foreach (var role in element.UserRoleIds) {
            await userGroupUserRoleUserInserter.InsertAsync(new UserRoleUser {
                UserRoleId = role,
                UserId = element.Identification.Id!.Value
            });
        }
    }

    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await userGroupUserRoleUserInserter.DisposeAsync();
    }
}