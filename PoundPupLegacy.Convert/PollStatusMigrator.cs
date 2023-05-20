namespace PoundPupLegacy.Convert;

internal sealed class PollStatusMigrator(
    IDatabaseConnections databaseConnections,
    IEntityCreator<PollStatus> pollStatusCreator
) : MigratorPPL(databaseConnections)
{
    protected override string Name => "poll statuses";

    private static async IAsyncEnumerable<PollStatus> GetNodeStatuses()
    {
        await Task.CompletedTask;
        yield return new PollStatus {
            Id = 0,
            Name = "Closed",
        };
        yield return new PollStatus {
            Id = 1,
            Name = "Open",
        };
    }

    protected override async Task MigrateImpl()
    {
        await pollStatusCreator.CreateAsync(GetNodeStatuses(), _postgresConnection);
    }
}
