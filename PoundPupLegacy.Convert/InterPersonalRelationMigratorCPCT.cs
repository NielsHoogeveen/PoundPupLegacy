namespace PoundPupLegacy.Convert;

internal sealed class InterPersonalRelationMigratorCPCT(
    IDatabaseConnections databaseConnections,
    IMandatorySingleItemDatabaseReaderFactory<NodeIdReaderByUrlIdRequest, int> nodeIdReaderFactory,
    ISingleItemDatabaseReaderFactory<TenantNodeReaderByUrlIdRequest, TenantNode.ToCreateForExistingNode> tenantNodeReaderByUrlIdFactory,
    IEntityCreatorFactory<InterPersonalRelation.ToCreateForExistingParticipants> interPersonalRelationCreatorFactory
) : MigratorCPCT(
    databaseConnections, 
    nodeIdReaderFactory, 
    tenantNodeReaderByUrlIdFactory
)
{
    protected override string Name => "inter personal relation";

    protected override async Task MigrateImpl()
    {
        await using var nodeIdReader = await nodeIdReaderFactory.CreateAsync(_postgresConnection);
        await using var tenantNodeReaderByUrlId = await tenantNodeReaderByUrlIdFactory.CreateAsync(_postgresConnection);
        await using var interPersonalRelationCreator = await interPersonalRelationCreatorFactory.CreateAsync(_postgresConnection);
        await interPersonalRelationCreator.CreateAsync(ReadInterPersonalRelations(nodeIdReader, tenantNodeReaderByUrlId));

    }

    private async IAsyncEnumerable<InterPersonalRelation.ToCreateForExistingParticipants> ReadInterPersonalRelations(
        IMandatorySingleItemDatabaseReader<NodeIdReaderByUrlIdRequest, int> nodeIdReader,
        ISingleItemDatabaseReader<TenantNodeReaderByUrlIdRequest, TenantNode.ToCreateForExistingNode> tenantNodeReaderByUrlId
    )
    {

        var sql = $"""
                select
                    id,
                    user_id,
                    title,
                    status,
                    created_date_time, 
                    changed_date_time,
                    person_id_from,
                    person_id_to,
                    case 
                        when start_date > end_date then end_date
                        else start_date
                    end start_date,
                    case
                        when start_date = end_date then null
                        when start_date > end_date then start_date
                        else end_date
                    end end_date,
                    nameable_id,
                    vocabulary_id
                from(
                    SELECT
                        n.nid id,
                        n.uid user_id,
                        n.title,
                        n.`status` status,
                        FROM_UNIXTIME(n.created) created_date_time, 
                        FROM_UNIXTIME(n.changed) changed_date_time,
                        n2.nid person_id_from,
                        n3.nid person_id_to,
                        STR_TO_DATE(REPLACE(p.field_date_from_0_value, '-00', '-01'),'%Y-%m-%d') start_date,
                    STR_TO_DATE(REPLACE(p.field_date_to_value,'-00', '-01'),'%Y-%m-%d') end_date,
                        n5.nid nameable_id,
                        c.cnid vocabulary_id
                    FROM node n
                    JOIN content_type_adopt_ind_rel p ON p.nid = n.nid AND p.vid = n.vid
                    JOIN node n2 ON n2.nid = p.field_person1_nid
                    JOIN node n3 ON n3.nid = p.field_person2_nid
                    JOIN category_node cn ON cn.nid = n.nid
                    JOIN category c ON c.cid = cn.cid AND c.cnid = 16900
                    JOIN node n5 ON n5.nid = c.cid
                	WHERE n.nid > 33162
                ) x
                """;
        using var readCommand = _mySqlConnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;

        var reader = await readCommand.ExecuteReaderAsync();

        while (await reader.ReadAsync()) {


            var id = reader.GetInt32("id");

            var (personIdFrom, personFromPublicationStatusId) = await GetNodeId(reader.GetInt32("person_id_from"), nodeIdReader, tenantNodeReaderByUrlId);
            var (personIdTo, personToPublicationStatusId) = await GetNodeId(reader.GetInt32("person_id_to"), nodeIdReader, tenantNodeReaderByUrlId);
            int interPersonalRelationTypeId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                TenantId = Constants.CPCT,
                UrlId = reader.GetInt32("nameable_id")
            });

            var tenantNodes = new List<TenantNode.ToCreateForNewNode>
            {
                new TenantNode.ToCreateForNewNode
                {
                    IdentificationForCreate = new Identification.Possible {
                        Id = null
                    },
                    TenantId = Constants.CPCT,
                    PublicationStatusId = reader.GetInt32("status"),
                    UrlPath = null,
                    SubgroupId = null,
                    UrlId = id
                }
            };
            if (personFromPublicationStatusId == 1 && personToPublicationStatusId == 1) {
                tenantNodes.Add(new TenantNode.ToCreateForNewNode {
                    IdentificationForCreate = new Identification.Possible {
                        Id = null
                    },
                    TenantId = Constants.PPL,
                    PublicationStatusId = 1,
                    UrlPath = null,
                    SubgroupId = null,
                    UrlId = null
                });
            }
            yield return new InterPersonalRelation.ToCreateForExistingParticipants{
                IdentificationForCreate = new Identification.Possible {
                    Id = null
                },
                NodeDetailsForCreate = new NodeDetails.NodeDetailsForCreate {
                    PublisherId = reader.GetInt32("user_id"),
                    CreatedDateTime = reader.GetDateTime("created_date_time"),
                    ChangedDateTime = reader.GetDateTime("changed_date_time"),
                    Title = reader.GetString("title"),
                    OwnerId = Constants.CPCT,
                    AuthoringStatusId = 1,
                    TenantNodes = tenantNodes,
                    NodeTypeId = 47,
                    TermIds = new List<int>(),
                },
                PersonIdFrom = personIdFrom,
                PersonIdTo = personIdTo,
                InterPersonalRelationDetails = new InterPersonalRelationDetails {
                    InterPersonalRelationTypeId = interPersonalRelationTypeId,
                    DateRange = new DateTimeRange(reader.IsDBNull("start_date") ? null : reader.GetDateTime("start_date"), reader.IsDBNull("end_date") ? null : reader.GetDateTime("end_date")),
                    DocumentIdProof = null,
                    Description = null,
                },
            };
        }
        await reader.CloseAsync();
    }
}
