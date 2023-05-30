namespace PoundPupLegacy.Convert;

internal sealed class PollStatusMigrator(
    IDatabaseConnections databaseConnections,
    IEntityCreatorFactory<PollStatus> pollStatusCreatorFactory
) : MigratorPPL(databaseConnections)
{
    protected override string Name => "poll statuses";

    private static async IAsyncEnumerable<PollStatus> GetNodeStatuses()
    {
        await Task.CompletedTask;
        yield return new PollStatus {
            IdentificationForCreate = new Identification.Possible {
                Id = 0,
            },
            Name = "Closed",
        };
        yield return new PollStatus {
            IdentificationForCreate = new Identification.Possible {
                Id = 1,
            },
            Name = "Open",
        };
    }

    protected override async Task MigrateImpl()
    {
        await using var pollStatusCreator = await pollStatusCreatorFactory.CreateAsync(_postgresConnection);
        await pollStatusCreator.CreateAsync(GetNodeStatuses());
    }
}
