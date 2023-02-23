using PoundPupLegacy.Db;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert;

internal sealed class PersonMigratorCPCT : CPCTMigrator
{
    public PersonMigratorCPCT(MySqlToPostgresConverter mySqlToPostgresConverter) : base(mySqlToPostgresConverter)
    {
    }

    protected override string Name => "persons (cpct)";

    protected override async Task MigrateImpl()
    {
        await PersonCreator.CreateAsync(ReadPersons(), _postgresConnection);
    }
    private async IAsyncEnumerable<Person> ReadPersons()
    {

        var sql = $"""
                SELECT
                DISTINCT
                n.nid id,
                n.uid access_role_id,
                TRIM(n.title) title,
                n.`status` node_status_id,
                FROM_UNIXTIME(n.created) created_date_time, 
                FROM_UNIXTIME(n.changed) changed_date_time
                FROM node n
                WHERE n.`type` = 'adopt_person'
                AND n.nid > 33162
                
                """;
        using var readCommand = MysqlConnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;


        var reader = await readCommand.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var id = reader.GetInt32("id");
            var title = reader.GetString("title");


            yield return new Person
            {
                Id = null,
                PublisherId = reader.GetInt32("access_role_id"),
                CreatedDateTime = reader.GetDateTime("created_date_time"),
                ChangedDateTime = reader.GetDateTime("changed_date_time"),
                Title = title,
                OwnerId = Constants.OWNER_PARTIES,
                TenantNodes = new List<TenantNode>
                {
                    new TenantNode
                    {
                        Id = null,
                        TenantId = Constants.PPL,
                        PublicationStatusId = 1,
                        UrlPath = null,
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = null
                    },
                    new TenantNode
                    {
                        Id = null,
                        TenantId = Constants.CPCT,
                        PublicationStatusId = 2,
                        UrlPath = null,
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = id
                    }
                },
                NodeTypeId = 24,
                Description = "",
                FileIdTileImage = null,
                VocabularyNames = new List<VocabularyName>(),
                DateOfBirth = null,
                DateOfDeath = null,
                FileIdPortrait = null,
                FirstName = null,
                LastName = null,
                MiddleName = null,
                FullName = null,
                GovtrackId = null,
                Bioguide = null,
                Suffix = null,
                ProfessionalRoles = new List<ProfessionalRole>(),
            };

        }
        await reader.CloseAsync();
    }

}
