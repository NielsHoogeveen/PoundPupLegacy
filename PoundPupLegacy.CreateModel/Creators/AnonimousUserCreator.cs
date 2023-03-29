namespace PoundPupLegacy.CreateModel.Creators;

public class AnonimousUserCreator
{
    public static async Task CreateAsync(NpgsqlConnection connection)
    {

        await using var principalWriter = await PrincipalWriter.CreateAsync(connection);
        await using var publisherWriter = await PublisherWriter.CreateAsync(connection);

        var user = new AnonymousUser();
        await principalWriter.WriteAsync(user);
        await publisherWriter.WriteAsync(user);
    }
}
