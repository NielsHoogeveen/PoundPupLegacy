using Npgsql;
using PoundPupLegacy.Db;
using PoundPupLegacy.Model;

namespace PoundPupLegacy.Convert;

internal sealed class PublicationStatusMigrator : Migrator
{
    protected override string Name => "publication statuses";
    public PublicationStatusMigrator(MySqlToPostgresConverter converter): base(converter) { }

    private static async IAsyncEnumerable<PublicationStatus> GetNodeStatuses()
    {
        await Task.CompletedTask;
        yield return new PublicationStatus
        {
            Id = 0,
            Name = "Not Published",
        };
        yield return new PublicationStatus
        {
            Id = 1,
            Name = "Published publically",
        };
        yield return new PublicationStatus
        {
            Id = 2,
            Name = "Published privately",
        };
    }

    protected override async Task MigrateImpl()
    {
        await PublicationStatusCreator.CreateAsync(GetNodeStatuses(), _postgresConnection);
    }
}
