﻿namespace PoundPupLegacy.Convert;

internal sealed class PersonOrganizationRelationMigratorCPCT : MigratorCPCT
{
    private readonly IEntityCreator<PersonOrganizationRelation> _personOrganizationRelationCreator;

    public PersonOrganizationRelationMigratorCPCT(
        IDatabaseConnections databaseConnections,
        IMandatorySingleItemDatabaseReaderFactory<NodeIdReaderByUrlIdRequest, int> nodeIdReaderFactory,
        ISingleItemDatabaseReaderFactory<TenantNodeReaderByUrlIdRequest, TenantNode> tenantNodeReaderByUrlIdFactory,
        IEntityCreator<PersonOrganizationRelation> personOrganizationRelationCreator
    ) : base(databaseConnections, nodeIdReaderFactory, tenantNodeReaderByUrlIdFactory)
    {
        _personOrganizationRelationCreator = personOrganizationRelationCreator;
    }

    protected override string Name => "person organization relation (cpct)";

    protected override async Task MigrateImpl()
    {
        await using var nodeIdReader = await _nodeIdReaderFactory.CreateAsync(_postgresConnection);
        await using var tenantNodeReaderByUrlId = await _tenantNodeReaderByUrlIdFactory.CreateAsync(_postgresConnection);
        await _personOrganizationRelationCreator.CreateAsync(ReadPersonOrganizationRelations(nodeIdReader, tenantNodeReaderByUrlId), _postgresConnection);
    }


    private async IAsyncEnumerable<PersonOrganizationRelation> ReadPersonOrganizationRelations(
        IMandatorySingleItemDatabaseReader<NodeIdReaderByUrlIdRequest, int> nodeIdReader,
        ISingleItemDatabaseReader<TenantNodeReaderByUrlIdRequest, TenantNode> tenantNodeReaderByUrlId
    )
    {

        var sql = $"""
                select
                    distinct
                    id,
                    user_id,
                    title,
                    status,
                    created_date_time, 
                    changed_date_time,
                    person_id,
                    organization_id,
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
                    vocabulary_id,
                    description
                from(
                    SELECT
                        n.nid id,
                        n.uid user_id,
                        n.title,
                        n.`status` status,
                        FROM_UNIXTIME(n.created) created_date_time, 
                        FROM_UNIXTIME(n.changed) changed_date_time,
                        n2.nid person_id,
                        case 
                        when n3.nid = 30638 then 14681
                        else n3.nid
                	    end organization_id,
                        STR_TO_DATE(REPLACE(p.field_start_date_value, '-00', '-01'),'%Y-%m-%d') start_date,
                        case 
                            when n.nid = 30588 then null
                            else STR_TO_DATE(REPLACE(p.field_to_date_value,'-00', '-01'),'%Y-%m-%d') 
                        end end_date,
                        n5.nid nameable_id,
                        c.cnid vocabulary_id,
                        field_description_2_value description
                    FROM node n
                    JOIN content_type_adopt_positions p ON p.nid = n.nid AND p.vid = n.vid
                    JOIN node n2 ON n2.nid = p.field_industrialist_nid AND n2.nid not in (74250)
                    JOIN node n3 ON n3.nid = p.field_industrial_organisation_nid AND n3.nid not in (11108)
                    JOIN category_node cn ON cn.nid = n.nid
                    JOIN category c ON c.cid = cn.cid AND c.cnid = 12663
                	JOIN node n5 ON n5.nid = c.cid
                    JOIN category_node cn2 ON cn2.nid = n3.nid
                    JOIN category c2 ON c2.cid = cn2.cid AND c2.cnid = 12622
                    JOIN node n6 ON n6.nid = c2.cid AND n6.nid NOT IN (38308)
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

            var (personId, personPublicationStatusId) = await GetNodeId(reader.GetInt32("person_id"), nodeIdReader, tenantNodeReaderByUrlId);
            var (organizationId, organizationPublicationStatusId) = await GetNodeId(reader.GetInt32("organization_id"), nodeIdReader, tenantNodeReaderByUrlId);
            int personOrganizationRelationTypeId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                UrlId = reader.GetInt32("nameable_id"),
                TenantId = Constants.PPL
            });

            var tenantNodes = new List<TenantNode>
                {
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
                };
            if (personPublicationStatusId == 1 && organizationPublicationStatusId == 1) {
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
            yield return new PersonOrganizationRelation {
                Id = null,
                PublisherId = reader.GetInt32("user_id"),
                CreatedDateTime = reader.GetDateTime("created_date_time"),
                ChangedDateTime = reader.GetDateTime("changed_date_time"),
                Title = reader.GetString("title"),
                OwnerId = Constants.OWNER_PARTIES,
                TenantNodes = tenantNodes,
                NodeTypeId = 48,
                PersonId = personId,
                OrganizationId = organizationId,
                GeographicalEntityId = null,
                PersonOrganizationRelationTypeId = personOrganizationRelationTypeId,
                DateRange = new DateTimeRange(reader.IsDBNull("start_date") ? null : reader.GetDateTime("start_date"), reader.IsDBNull("end_date") ? null : reader.GetDateTime("end_date")),
                DocumentIdProof = null,
                Description = reader.IsDBNull("description") ? null : reader.GetString("description"),
            };
        }
        await reader.CloseAsync();
    }

}
