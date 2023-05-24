namespace PoundPupLegacy.Convert;

internal sealed class AuthoringStatusMigrator(
    IDatabaseConnections databaseConnections,
    IInsertingEntityCreatorFactory<AuthoringStatus> authoringStatusCreatorFactory
) : MigratorPPL(databaseConnections)
{
    protected override string Name => "authoring statuses";

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
        await using var authoringStatusCreator = await authoringStatusCreatorFactory.CreateAsync(_postgresConnection);
        await authoringStatusCreator.CreateAsync(GetNodeStatuses());
    }
}
