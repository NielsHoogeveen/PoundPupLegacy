namespace PoundPupLegacy.Convert;

internal sealed class PollStatusMigrator : MigratorPPL
{
    protected override string Name => "poll statuses";

    private readonly IEntityCreator<PollStatus> _pollStatusCreator;
    public PollStatusMigrator(
        IDatabaseConnections databaseConnections,
        IEntityCreator<PollStatus> pollStatusCreator
    ) : base(databaseConnections)
    {
        _pollStatusCreator = pollStatusCreator;
    }

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
        await _pollStatusCreator.CreateAsync(GetNodeStatuses(), _postgresConnection);
    }
}
