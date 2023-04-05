namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class CollectiveCreator : EntityCreator<Collective>
{
    private readonly IDatabaseInserterFactory<Principal> _principalInserterFactory;
    private readonly IDatabaseInserterFactory<AccessRole> _accessRoleInserterFactory;
    private readonly IDatabaseInserterFactory<Publisher> _publisherInserterFactory;
    private readonly IDatabaseInserterFactory<Collective> _collectiveInserterFactory;

    public CollectiveCreator(
        IDatabaseInserterFactory<Principal> principalInserterFactory,
        IDatabaseInserterFactory<AccessRole> accessRoleInserterFactory,
        IDatabaseInserterFactory<Publisher> publisherInserterFactory,
        IDatabaseInserterFactory<Collective> collectiveInserterFactory
    )
    {
        _principalInserterFactory = principalInserterFactory;
        _accessRoleInserterFactory = accessRoleInserterFactory;
        _publisherInserterFactory = publisherInserterFactory;
        _collectiveInserterFactory = collectiveInserterFactory;
    }
    public override async Task CreateAsync(IAsyncEnumerable<Collective> collectives, IDbConnection connection)
    {

        await using var principalWriter = await _principalInserterFactory.CreateAsync(connection);
        await using var accessRoleWriter = await _accessRoleInserterFactory.CreateAsync(connection);
        await using var publisherWriter = await _publisherInserterFactory.CreateAsync(connection);
        await using var userWriter = await _collectiveInserterFactory.CreateAsync(connection);

        await foreach (var collective in collectives) {
            await principalWriter.InsertAsync(collective);
            await publisherWriter.InsertAsync(collective);
            await userWriter.InsertAsync(collective);
        }
    }
}