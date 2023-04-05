namespace PoundPupLegacy.CreateModel.Creators;

public interface IAnonimousUserCreator
{
    Task CreateAsync(IDbConnection connection);
}
internal sealed class AnonimousUserCreator: IAnonimousUserCreator
{
    private readonly IDatabaseInserterFactory<Principal> _principalInserterFactory;
    private readonly IDatabaseInserterFactory<Publisher> _publisherInserterFactory;
    public AnonimousUserCreator(
        IDatabaseInserterFactory<Principal> principalInserterFactory,
        IDatabaseInserterFactory<Publisher> publisherInserterFactory
        )
    {
        _principalInserterFactory = principalInserterFactory;
        _publisherInserterFactory = publisherInserterFactory;
    }
    public async Task CreateAsync(IDbConnection connection)
    {

        await using var principalWriter = await _principalInserterFactory.CreateAsync(connection);
        await using var publisherWriter = await _publisherInserterFactory.CreateAsync(connection);

        var user = new AnonymousUser();
        await principalWriter.InsertAsync(user);
        await publisherWriter.InsertAsync(user);
    }
}
