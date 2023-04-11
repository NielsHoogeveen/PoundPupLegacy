namespace PoundPupLegacy.Convert;

internal sealed class PublicationStatusMigrator : MigratorPPL
{
    protected override string Name => "publication statuses";
    private readonly IEntityCreator<PublicationStatus> _publicationStatusCreator;
    public PublicationStatusMigrator(
        IDatabaseConnections databaseConnections,
        IEntityCreator<PublicationStatus> publicationStatusCreator
    ) : base(databaseConnections)
    {
        _publicationStatusCreator = publicationStatusCreator;
    }

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
        await _publicationStatusCreator.CreateAsync(GetNodeStatuses(), _postgresConnection);
    }
}
