namespace PoundPupLegacy.CreateModel.Creators;

public class AnonimousUserCreator
{
    public static async Task CreateAsync(NpgsqlConnection connection)
    {

        await using var principalWriter = await PrincipalInserter.CreateAsync(connection);
        await using var publisherWriter = await PublisherInserter.CreateAsync(connection);

        var user = new AnonymousUser();
        await principalWriter.WriteAsync(user);
        await publisherWriter.WriteAsync(user);
    }
}
