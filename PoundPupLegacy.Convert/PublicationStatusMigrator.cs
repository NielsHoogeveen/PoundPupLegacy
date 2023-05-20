namespace PoundPupLegacy.Convert;

internal sealed class PublicationStatusMigrator(
    IDatabaseConnections databaseConnections,
    IEntityCreator<PublicationStatus> publicationStatusCreator
) : MigratorPPL(databaseConnections)
{
    protected override string Name => "publication statuses";
    private static async IAsyncEnumerable<PublicationStatus> GetNodeStatuses()
    {
        await Task.CompletedTask;
        yield return new PublicationStatus {
            Id = 0,
            Name = "Not Published",
        };
        yield return new PublicationStatus {
            Id = 1,
            Name = "Published publically",
        };
        yield return new PublicationStatus {
            Id = 2,
            Name = "Published privately",
        };
    }

    protected override async Task MigrateImpl()
    {
        await publicationStatusCreator.CreateAsync(GetNodeStatuses(), _postgresConnection);
    }
}
