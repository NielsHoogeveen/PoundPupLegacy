namespace PoundPupLegacy.DomainModel.Creators;

internal sealed class PollStatusCreatorFactory(
    IDatabaseInserterFactory<PollStatus> pollStatusInserterFactory
) : IEntityCreatorFactory<PollStatus>
{
    public async Task<IEntityCreator<PollStatus>> CreateAsync(IDbConnection connection) =>
        new InsertingEntityCreator<PollStatus>(new() {
            await pollStatusInserterFactory.CreateAsync(connection)
        });
}
