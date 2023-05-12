namespace PoundPupLegacy.Convert;

internal sealed class InterOrganizationalRelationMigratorCPCT : MigratorCPCT
{
    private readonly IEntityCreator<InterOrganizationalRelation> _interOrganizationalRelationCreator;
    public InterOrganizationalRelationMigratorCPCT(
        IDatabaseConnections databaseConnections,
        IMandatorySingleItemDatabaseReaderFactory<NodeIdReaderByUrlIdRequest, int> nodeIdReaderFactory,
        ISingleItemDatabaseReaderFactory<TenantNodeReaderByUrlIdRequest, TenantNode> tenantNodeReaderByUrlIdFactory,
        IEntityCreator<InterOrganizationalRelation> interOrganizationalRelationCreator
    ) : base(databaseConnections, nodeIdReaderFactory, tenantNodeReaderByUrlIdFactory)
    {
        _interOrganizationalRelationCreator = interOrganizationalRelationCreator;
    }

    protected override string Name => "person organization relation (cpct)";

    protected override async Task MigrateImpl()
    {
        await using var nodeIdReader = await _nodeIdReaderFactory.CreateAsync(_postgresConnection);
        await using var tenantNodeReaderByUrlId = await _tenantNodeReaderByUrlIdFactory.CreateAsync(_postgresConnection);

        await _interOrganizationalRelationCreator.CreateAsync(ReadInterOrganizationalRelations(nodeIdReader, tenantNodeReaderByUrlId), _postgresConnection);

    }

    private async IAsyncEnumerable<InterOrganizationalRelation> ReadInterOrganizationalRelations(
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
                    organization_id_from,
                    organization_id_to,
                    case 
                        when start_date > end_date then end_date
                        else start_date
                    end start_date,
                    case
                        when start_date = end_date then null
                        when start_date > end_date then start_date
                        else end_date
                    end end_date,
                    description,
                    nameable_id, 
                    vocabulary_id,
                    money_involved,
                    number_of_children_involved
                from(
                    SELECT
                        n.nid id,
                        n.uid user_id,
                        n.title,
                        n.`status` status,
                        FROM_UNIXTIME(n.created) created_date_time, 
                        FROM_UNIXTIME(n.changed) changed_date_time,
                        n2.nid organization_id_to,
                        case 
                        when n3.nid = 30638 then 14681
                        else n3.nid
                        end organization_id_from,
                        STR_TO_DATE(REPLACE(p.field_date_from_value, '-00', '-01'),'%Y-%m-%d') start_date,
                				STR_TO_DATE(REPLACE(p.field_end_date_0_value,'-00', '-01'),'%Y-%m-%d') end_date,
                				c.cid nameable_id,
                        c.cnid vocabulary_id,
                        field_description_1_value description, 
                        field_money_involved_value money_involved,
                        field_number_children_value number_of_children_involved
                    FROM node n
                    JOIN content_type_adopt_affiliation p ON p.nid = n.nid AND p.vid = n.vid
                    JOIN node n2 ON n2.nid = p.field_organisatie_from_nid AND n2.nid not in (11108)
                    JOIN node n3 ON n3.nid = p.field_organization_to_nid AND n3.nid not in (11108)
                    JOIN category_node cn ON cn.nid = n.nid
                    JOIN category c ON c.cid = cn.cid AND c.cnid = 12637
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

            var (organizationIdFrom, organizationFromPublicationStatusId) = await GetNodeId(reader.GetInt32("organization_id_from"), nodeIdReader, tenantNodeReaderByUrlId);
            var (organizationIdTo, organizationToPublicationStatusId) = await GetNodeId(reader.GetInt32("organization_id_to"), nodeIdReader, tenantNodeReaderByUrlId);
            int interOrganizationalRelationTypeId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                UrlId = reader.GetInt32("nameable_id"),
                TenantId = Constants.CPCT
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
            if (organizationFromPublicationStatusId == 1 && organizationToPublicationStatusId == 1) {
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

            yield return new InterOrganizationalRelation {
                Id = null,
                PublisherId = reader.GetInt32("user_id"),
                CreatedDateTime = reader.GetDateTime("created_date_time"),
                ChangedDateTime = reader.GetDateTime("changed_date_time"),
                Title = reader.GetString("title"),
                OwnerId = Constants.PPL,
                AuthoringStatusId = 1,
                TenantNodes = tenantNodes,
                NodeTypeId = 47,
                OrganizationIdFrom = organizationIdFrom,
                OrganizationIdTo = organizationIdTo,
                GeographicalEntityId = null,
                InterOrganizationalRelationTypeId = interOrganizationalRelationTypeId,
                DateRange = new DateTimeRange(reader.IsDBNull("start_date") ? null : reader.GetDateTime("start_date"), reader.IsDBNull("end_date") ? null : reader.GetDateTime("end_date")),
                DocumentIdProof = null,
                Description = reader.IsDBNull("description") ? null : reader.GetString("description"),
                MoneyInvolved = reader.IsDBNull("money_involved") ? null : reader.GetDecimal("money_involved"),
                NumberOfChildrenInvolved = reader.IsDBNull("number_of_children_involved") ? null : reader.GetInt32("number_of_children_involved"),
            };
        }
        await reader.CloseAsync();
    }

}
