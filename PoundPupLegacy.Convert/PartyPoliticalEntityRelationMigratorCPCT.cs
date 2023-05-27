namespace PoundPupLegacy.Convert;

internal sealed class PartyPoliticalEntityRelationMigratorCPCT(
    IDatabaseConnections databaseConnections,
    IMandatorySingleItemDatabaseReaderFactory<NodeIdReaderByUrlIdRequest, int> nodeIdReaderFactory,
    ISingleItemDatabaseReaderFactory<TenantNodeReaderByUrlIdRequest, NewTenantNodeForExistingNode> tenantNodeReaderByUrlIdFactory,
    IEntityCreatorFactory<EventuallyIdentifiablePartyPoliticalEntityRelationForExistingParty> partyPoliticalEntityRelationCreatorFactory
) : MigratorCPCT(
    databaseConnections, 
    nodeIdReaderFactory, 
    tenantNodeReaderByUrlIdFactory
)
{
    protected override string Name => "party political enitity relation";

    protected override async Task MigrateImpl()
    {
        await using var nodeIdReader = await nodeIdReaderFactory.CreateAsync(_postgresConnection);
        await using var tenantNodeReader = await tenantNodeReaderByUrlIdFactory.CreateAsync(_postgresConnection);
        await using var partyPoliticalEntityRelationCreator = await partyPoliticalEntityRelationCreatorFactory.CreateAsync(_postgresConnection);
        await partyPoliticalEntityRelationCreator.CreateAsync(ReadPartyPoliticalEntityRelations(nodeIdReader, tenantNodeReader));
    }

    private async IAsyncEnumerable<NewPartyPoliticalEntityRelationForExistingParty> ReadPartyPoliticalEntityRelations(
        IMandatorySingleItemDatabaseReader<NodeIdReaderByUrlIdRequest, int> nodeIdReader,
        ISingleItemDatabaseReader<TenantNodeReaderByUrlIdRequest, NewTenantNodeForExistingNode> tenantNodeReader)
    {

        var sql = $"""
                SELECT
                id,
                user_id,
                title,
                `status`,
                created_date_time,
                changed_date_time,
                party_id,
                political_entity_id,
                case 
                    when start_date > end_date then end_date
                    ELSE start_date
                end start_date,
                case 
                    when id = 30667 then null
                    when start_date > end_date then start_date
                    ELSE end_date
                end end_date,
                nameable_id,
                vocabulary_id
                FROM(
                    SELECT
                        min(n.nid) id,
                        n.uid user_id,
                        n.title,
                        n.`status` status,
                        FROM_UNIXTIME(n.created) created_date_time, 
                        FROM_UNIXTIME(n.changed) changed_date_time,
                        case 
                        when n2.nid = 30638 then 14681
                        else n2.nid
                        end party_id,
                        n3.nid political_entity_id,
                        STR_TO_DATE(REPLACE(p.field_from_date_value, '-00', '-01'),'%Y-%m-%d') start_date,
                        STR_TO_DATE(REPLACE(p.field_to_date_0_value,'-00', '-01'),'%Y-%m-%d') end_date,
                        n4.nid nameable_id,
                        c.cnid vocabulary_id
                    FROM node n
                    JOIN content_type_adopt_country_link p ON p.nid = n.nid AND p.vid = n.vid
                    JOIN node n2 ON n2.nid = p.field_person_or_organization_nid
                    JOIN node n3 ON n3.nid = p.field_country_nid
                    JOIN category_node cn ON cn.nid = n.nid
                    JOIN node n4 ON n4.nid = cn.cid
                    JOIN category c ON c.cid = n4.nid AND c.cnid = 12652
                	WHERE n.nid > 33162 and n.nid not in (37168)
                    GROUP BY n2.nid,
                    n3.nid,
                    p.field_from_date_value,
                    p.field_to_date_0_value,
                    n4.nid,
                    c.cnid
                ) x
                """;
        using var readCommand = _mySqlConnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;

        var reader = await readCommand.ExecuteReaderAsync();

        while (await reader.ReadAsync()) {


            var id = reader.GetInt32("id");

            var (partyId, partyPublicationStatusId) = await GetNodeId(reader.GetInt32("party_id"), nodeIdReader, tenantNodeReader);
            int politicalEntityId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                UrlId = reader.GetInt32("political_entity_id"),
                TenantId = Constants.PPL
            });
            int partyPpoliticalEntityTypeId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                UrlId = reader.GetInt32("nameable_id"),
                TenantId = Constants.PPL
            });

            var tenantNodes = new List<NewTenantNodeForNewNode>
            {
                new NewTenantNodeForNewNode
                {
                    Id = null,
                    TenantId = Constants.CPCT,
                    PublicationStatusId = reader.GetInt32("status"),
                    UrlPath = null,
                    SubgroupId = null,
                    UrlId = id
                }
            };
            if (partyPublicationStatusId == 1) {
                tenantNodes.Add(new NewTenantNodeForNewNode {
                    Id = null,
                    TenantId = Constants.PPL,
                    PublicationStatusId = 1,
                    UrlPath = null,
                    SubgroupId = null,
                    UrlId = null
                });
            }
            yield return new NewPartyPoliticalEntityRelationForExistingParty {
                Id = null,
                PublisherId = reader.GetInt32("user_id"),
                CreatedDateTime = reader.GetDateTime("created_date_time"),
                ChangedDateTime = reader.GetDateTime("changed_date_time"),
                Title = reader.GetString("title"),
                OwnerId = Constants.OWNER_PARTIES,
                AuthoringStatusId = 1,
                TenantNodes = tenantNodes,
                NodeTypeId = 49,
                PartyId = partyId,
                PoliticalEntityId = politicalEntityId,
                PartyPoliticalEntityRelationTypeId = partyPpoliticalEntityTypeId,
                DateRange = new DateTimeRange(reader.IsDBNull("start_date") ? null : reader.GetDateTime("start_date"), reader.IsDBNull("end_date") ? null : reader.GetDateTime("end_date")),
                DocumentIdProof = null,
                TermIds = new List<int>(),
            };
        }
        await reader.CloseAsync();
    }

}
