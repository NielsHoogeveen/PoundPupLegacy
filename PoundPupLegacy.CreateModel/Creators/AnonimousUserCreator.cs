namespace PoundPupLegacy.CreateModel.Creators;

public class AnonimousUserCreator
{
    public async Task CreateAsync(IDbConnection connection)
    {

        await using var principalWriter = await PrincipalInserter.CreateAsync(connection);
        await using var publisherWriter = await PublisherInserter.CreateAsync(connection);

        var user = new AnonymousUser();
        await principalWriter.InsertAsync(user);
        await publisherWriter.InsertAsync(user);
    }
}
