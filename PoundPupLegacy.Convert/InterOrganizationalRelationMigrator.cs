using PoundPupLegacy.Db;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert;

internal sealed class InterOrganizationalRelationMigrator: PPLMigrator
{
    public InterOrganizationalRelationMigrator(MySqlToPostgresConverter mySqlToPostgresConverter) : base(mySqlToPostgresConverter)
    {
    }

    protected override string Name => "person organization relation";

    protected override async Task MigrateImpl()
    {
        await InterOrganizationalRelationCreator.CreateAsync(ReadInterOrganizationalRelations(), _postgresConnection);

    }

    private async IAsyncEnumerable<InterOrganizationalRelation> ReadInterOrganizationalRelations()
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
                    geographical_entity_id,
                    nameable_id,
                    vocabulary_id,
                    document_id_proof,
                    description,
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
                        case 
                            when n2.nid = 7760 then 8063
                            when n2.nid = 30638 then 14681
                            when n2.nid = 12700 then 52558
                            else n2.nid
                        end organization_id_from,
                        case 
                            when n3.nid = 7760 then 8063
                            when n2.nid = 30638 then 14681
                            when n3.nid = 12700 then 52558
                            else n3.nid
                        end organization_id_to,
                        STR_TO_DATE(REPLACE(p.field_date_from_value, '-00', '-01'),'%Y-%m-%d') start_date,
                    STR_TO_DATE(REPLACE(p.field_end_date_0_value,'-00', '-01'),'%Y-%m-%d') end_date,
                        n4.nid geographical_entity_id,
                        n5.nid nameable_id,
                        c.cnid vocabulary_id,
                        case 
                        when fp.field_proof_nid = 0 then null
                        ELSE fp.field_proof_nid 
                        end document_id_proof,
                        field_description_1_value description,
                        field_money_involved_value money_involved,
                        field_number_children_value number_of_children_involved
                    FROM node n
                    JOIN content_type_adopt_affiliation p ON p.nid = n.nid AND p.vid = n.vid
                    JOIN node n2 ON n2.nid = p.field_organisatie_from_nid 
                    JOIN node n3 ON n3.nid = p.field_organization_to_nid
                    LEFT JOIN node n4 ON n4.nid = p.field_country_2_nid
                    JOIN category_node cn ON cn.nid = n.nid
                    JOIN category c ON c.cid = cn.cid AND c.cnid = 12637
                	JOIN node n5 ON n5.nid = c.cid
                    JOIN category_node cn2 ON cn2.nid = n3.nid
                    JOIN category c2 ON c2.cid = cn2.cid AND c2.cnid = 12622
                    JOIN node n6 ON n6.nid = c2.cid AND n6.nid NOT IN (38518, 38308)
                    JOIN category_node cn3 ON cn3.nid = n2.nid
                    JOIN category c3 ON c3.cid = cn3.cid AND c3.cnid = 12622
                    JOIN node n7 ON n7.nid = c3.cid AND n7.nid NOT IN (38518, 38308)
                    LEFT JOIN (
                    SELECT
                    fp.field_proof_nid,
                    fp.nid,
                    fp.vid
                    FROM content_field_proof fp
                    JOIN node n ON n.nid = fp.field_proof_nid
                    ) fp ON fp.nid = n.nid AND fp.vid = n.vid
                ) x
                """;
        using var readCommand = _mysqlConnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;

        var reader = await readCommand.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {


            var id = reader.GetInt32("id");

            int organizationIdFrom = await _nodeIdReader.ReadAsync(Constants.PPL, reader.GetInt32("organization_id_from"));
            int organizationIdTo = await _nodeIdReader.ReadAsync(Constants.PPL, reader.GetInt32("organization_id_to"));
            int? geographicalEntityId = reader.IsDBNull("geographical_entity_id") ? null: await _nodeIdReader.ReadAsync(Constants.PPL, reader.GetInt32("geographical_entity_id"));
            int interOrganizationalRelationTypeId = await _nodeIdReader.ReadAsync(Constants.PPL, reader.GetInt32("nameable_id"));

            yield return new InterOrganizationalRelation
            {
                Id = null,
                PublisherId = reader.GetInt32("user_id"),
                CreatedDateTime = reader.GetDateTime("created_date_time"),
                ChangedDateTime = reader.GetDateTime("changed_date_time"),
                Title = reader.GetString("title"),
                OwnerId = Constants.PPL,
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
                    }
                },
                NodeTypeId = 47,
                OrganizationIdFrom = organizationIdFrom,
                OrganizationIdTo = organizationIdTo,
                GeographicalEntityId = geographicalEntityId,
                InterOrganizationalRelationTypeId = interOrganizationalRelationTypeId,
                DateRange = new DateTimeRange(reader.IsDBNull("start_date") ? null : reader.GetDateTime("start_date"), reader.IsDBNull("end_date") ? null: reader.GetDateTime("end_date")),
                DocumentIdProof = reader.IsDBNull("document_id_proof") ? null : await _nodeIdReader.ReadAsync(Constants.PPL, reader.GetInt32("document_id_proof")),
                Description = reader.IsDBNull("description")? null: reader.GetString("description"),
                MoneyInvolved = reader.IsDBNull("money_involved") ? null : reader.GetDecimal("money_involved"),
                NumberOfChildrenInvolved = reader.IsDBNull("number_of_children_involved") ? null : reader.GetInt32("number_of_children_involved"),
            };
        }
        await reader.CloseAsync();
    }

}
