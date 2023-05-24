namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class PollStatusCreatorFactory(
    IDatabaseInserterFactory<PollStatus> pollStatusInserterFactory
) : IInsertingEntityCreatorFactory<PollStatus>
{
    public async Task<InsertingEntityCreator<PollStatus>> CreateAsync(IDbConnection connection) => 
        new(new() {
            await pollStatusInserterFactory.CreateAsync(connection)
        });
}
