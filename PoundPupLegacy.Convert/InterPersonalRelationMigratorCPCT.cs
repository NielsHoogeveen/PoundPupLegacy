﻿namespace PoundPupLegacy.Convert;

internal sealed class InterPersonalRelationMigratorCPCT : MigratorCPCT
{
    private readonly IEntityCreator<InterPersonalRelation> _interPersonalRelationCreator;
    public InterPersonalRelationMigratorCPCT(
        IDatabaseConnections databaseConnections,
        IDatabaseReaderFactory<NodeIdReaderByUrlId> nodeIdReaderFactory,
        IDatabaseReaderFactory<TenantNodeReaderByUrlId> tenantNodeReaderByUrlIdFactory,
        IEntityCreator<InterPersonalRelation> interPersonalRelationCreator
    ) : base(databaseConnections, nodeIdReaderFactory, tenantNodeReaderByUrlIdFactory)
    {
        _interPersonalRelationCreator = interPersonalRelationCreator;
    }

    protected override string Name => "inter personal relation";

    protected override async Task MigrateImpl()
    {
        await using var nodeIdReader = await _nodeIdReaderFactory.CreateAsync(_postgresConnection);
        await using var tenantNodeReaderByUrlId = await _tenantNodeReaderByUrlIdFactory.CreateAsync(_postgresConnection);
        await _interPersonalRelationCreator.CreateAsync(ReadInterPersonalRelations(nodeIdReader, tenantNodeReaderByUrlId), _postgresConnection);

    }

    private async IAsyncEnumerable<InterPersonalRelation> ReadInterPersonalRelations(NodeIdReaderByUrlId nodeIdReader, TenantNodeReaderByUrlId tenantNodeReaderByUrlId)
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
            int interPersonalRelationTypeId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlId.Request {
                TenantId = Constants.CPCT,
                UrlId = reader.GetInt32("nameable_id")
            });

            var tenantNodes = new List<TenantNode>
            {
                new TenantNode
                {
                    Id = null,
                    TenantId = Constants.CPCT,
                    PublicationStatusId = reader.GetInt32("status"),
                    UrlPath = null,
                    NodeId = null,
                    SubgroupId = null,
                    UrlId = id
                }
            };
            if (personFromPublicationStatusId == 1 && personToPublicationStatusId == 1) {
                tenantNodes.Add(new TenantNode {
                    Id = null,
                    TenantId = Constants.PPL,
                    PublicationStatusId = 1,
                    UrlPath = null,
                    NodeId = null,
                    SubgroupId = null,
                    UrlId = null
                });
            }
            yield return new InterPersonalRelation {
                Id = null,
                PublisherId = reader.GetInt32("user_id"),
                CreatedDateTime = reader.GetDateTime("created_date_time"),
                ChangedDateTime = reader.GetDateTime("changed_date_time"),
                Title = reader.GetString("title"),
                OwnerId = Constants.CPCT,
                TenantNodes = tenantNodes,
                NodeTypeId = 47,
                PersonIdFrom = personIdFrom,
                PersonIdTo = personIdTo,
                InterPersonalRelationTypeId = interPersonalRelationTypeId,
                DateRange = new DateTimeRange(reader.IsDBNull("start_date") ? null : reader.GetDateTime("start_date"), reader.IsDBNull("end_date") ? null : reader.GetDateTime("end_date")),
                DocumentIdProof = null,
            };
        }
        await reader.CloseAsync();
    }

}
