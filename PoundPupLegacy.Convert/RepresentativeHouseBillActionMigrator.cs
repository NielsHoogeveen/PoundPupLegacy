using PoundPupLegacy.Db;
using PoundPupLegacy.Db.Readers;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert;

internal sealed class RepresentativeHouseBillActionMigrator : PPLMigrator
{


    public RepresentativeHouseBillActionMigrator(MySqlToPostgresConverter mySqlToPostgresConverter) : base(mySqlToPostgresConverter)
    {

    }

    protected override string Name => "representative house bill action";

    protected override async Task MigrateImpl()
    {
        await RepresentativeHouseBillActionCreator.CreateAsync(ReadRepresentativeHouseBillActionsPPL(), _postgresConnection);

    }

    private async IAsyncEnumerable<RepresentativeHouseBillAction> ReadRepresentativeHouseBillActionsPPL()
    {

        await using var professionReader = await new ProfessionIdReaderFactory().CreateAsync(_postgresConnection);

        var sql = $"""
            SELECT
                n.nid id,
                n.uid user_id,
                n.title,
                n.`status` status,
                FROM_UNIXTIME(n.created) created_date_time, 
                FROM_UNIXTIME(n.changed) changed_date_time,
                n2.nid person_id,
                n3.nid bill_id,
                n5.nid nameable_id,
                STR_TO_DATE(REPLACE(p.field_start_date_value, '-00', '-01'),'%Y-%m-%d') date
            FROM node n
            JOIN content_type_adopt_positions p ON p.nid = n.nid AND p.vid = n.vid
            JOIN node n2 ON n2.nid = p.field_industrialist_nid 
            JOIN node n3 ON n3.nid = p.field_industrial_organisation_nid 
            LEFT JOIN node n4 ON n4.nid = p.field_country_3_nid
            JOIN category_node cn ON cn.nid = n.nid
            JOIN category c ON c.cid = cn.cid AND c.cnid = 12663
            JOIN node n5 ON n5.nid = c.cid
            WHERE n5.nid IN (38312, 38509)
            AND n3.title LIKE 'H%' 
            AND n.nid not in (65267, 65493)
            """;
        using var readCommand = MysqlConnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;

        var reader = await readCommand.ExecuteReaderAsync();

        while (await reader.ReadAsync()) {


            var id = reader.GetInt32("id");

            int representativeId = await professionReader.ReadAsync(new ProfessionIdReader.ProfessionIdReaderRequest 
            { 
                TenantId = Constants.PPL,
                ProfessionType = ProfessionIdReader.ProfessionType.Representative,
                UrlId = reader.GetInt32("person_id")
            });
            int billId = await _nodeIdReader.ReadAsync(new NodeIdReaderByUrlId.NodeIdReaderByUrlIdRequest() {
                TenantId = Constants.PPL,
                UrlId = reader.GetInt32("bill_id")
            });
            int billActionTypeId = await _nodeIdReader.ReadAsync(new NodeIdReaderByUrlId.NodeIdReaderByUrlIdRequest {
                TenantId = Constants.PPL,
                UrlId = reader.GetInt32("nameable_id")
            });

            yield return new RepresentativeHouseBillAction {
                Id = null,
                PublisherId = reader.GetInt32("user_id"),
                CreatedDateTime = reader.GetDateTime("created_date_time"),
                ChangedDateTime = reader.GetDateTime("changed_date_time"),
                Title = reader.GetString("title"),
                OwnerId = Constants.OWNER_PARTIES,
                TenantNodes = new List<TenantNode>
                {
                    new TenantNode
                    {
                        Id = null,
                        TenantId = 1,
                        PublicationStatusId = reader.GetInt32("status"),
                        UrlPath = null,
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = id
                    },
                    new TenantNode
                    {
                        Id = null,
                        TenantId = Constants.CPCT,
                        PublicationStatusId = 2,
                        UrlPath = null,
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = id < 33163 ? id : null
                    }
                },
                NodeTypeId = 48,
                RepresentativeId = representativeId,
                HouseBillId = billId,
                BillActionTypeId = billActionTypeId,
                Date = reader.GetDateTime("date"),
            };
        }
        await reader.CloseAsync();
    }

}
