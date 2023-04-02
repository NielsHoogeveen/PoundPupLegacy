namespace PoundPupLegacy.Convert;

internal sealed class PollStatusMigrator : PPLMigrator
{
    protected override string Name => "poll statuses";
    public PollStatusMigrator(MySqlToPostgresConverter converter) : base(converter) { }

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
        await new PollStatusCreator().CreateAsync(GetNodeStatuses(), _postgresConnection);
    }
}
