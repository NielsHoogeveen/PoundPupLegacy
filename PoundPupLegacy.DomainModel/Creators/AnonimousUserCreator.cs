namespace PoundPupLegacy.DomainModel.Creators;

public interface IAnonimousUserCreator
{
    Task CreateAsync(IDbConnection connection);
}
internal sealed class AnonimousUserCreator(
    IDatabaseInserterFactory<PrincipalToCreate> principalInserterFactory,
    IDatabaseInserterFactory<PublisherToCreate> publisherInserterFactory
) : IAnonimousUserCreator
{
    public async Task CreateAsync(IDbConnection connection)
    {
        await using var principalWriter = await principalInserterFactory.CreateAsync(connection);
        await using var publisherWriter = await publisherInserterFactory.CreateAsync(connection);

        var user = new AnonymousUser {
            Identification = new Identification.Possible {
                Id = 0
            },
            Name = "anonymous",
        };
        await principalWriter.InsertAsync(user);
        await publisherWriter.InsertAsync(user);
    }
}
