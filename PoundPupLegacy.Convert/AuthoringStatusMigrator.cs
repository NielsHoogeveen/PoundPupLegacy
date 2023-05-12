namespace PoundPupLegacy.Convert;

internal sealed class AuthoringStatusMigrator : MigratorPPL
{
    protected override string Name => "authoring statuses";
    private readonly IEntityCreator<AuthoringStatus> _authoringStatusCreator;
    public AuthoringStatusMigrator(
        IDatabaseConnections databaseConnections,
        IEntityCreator<AuthoringStatus> authoringStatusCreator
    ) : base(databaseConnections)
    {
        _authoringStatusCreator = authoringStatusCreator;
    }

    private static async IAsyncEnumerable<AuthoringStatus> GetNodeStatuses()
    {
        await Task.CompletedTask;
        yield return new AuthoringStatus {
            Id = 1,
            Name = "Authored",
        };
        yield return new AuthoringStatus {
            Id = 2,
            Name = "Retracted",
        };
    }

    protected override async Task MigrateImpl()
    {
        await _authoringStatusCreator.CreateAsync(GetNodeStatuses(), _postgresConnection);
    }
}
