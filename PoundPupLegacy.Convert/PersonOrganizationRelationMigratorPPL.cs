using PoundPupLegacy.Db;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert;

internal sealed class PersonOrganizationRelationMigratorPPL : PPLMigrator
{


    public PersonOrganizationRelationMigratorPPL(MySqlToPostgresConverter mySqlToPostgresConverter) : base(mySqlToPostgresConverter)
    {

    }

    protected override string Name => "person organization relation (ppl)";

    protected override async Task MigrateImpl()
    {
        await PersonOrganizationRelationCreator.CreateAsync(ReadPersonOrganizationRelationsPPL(), _postgresConnection);

    }

    private async IAsyncEnumerable<PersonOrganizationRelation> ReadPersonOrganizationRelationsPPL()
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
                    geographical_entity_id,
                    nameable_id,
                    vocabulary_id,
                    document_id_proof,
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
                        n4.nid geographical_entity_id,
                        n5.nid nameable_id,
                        c.cnid vocabulary_id,
                        case 
                        when fp.field_proof_nid = 0 then null
                        ELSE fp.field_proof_nid 
                        end document_id_proof,
                        field_description_2_value description
                    FROM node n
                    JOIN content_type_adopt_positions p ON p.nid = n.nid AND p.vid = n.vid
                    JOIN node n2 ON n2.nid = p.field_industrialist_nid AND n2.nid not in (74250)
                    JOIN node n3 ON n3.nid = p.field_industrial_organisation_nid AND n3.nid not in (11108, 38419, 38421)
                    LEFT JOIN node n4 ON n4.nid = p.field_country_3_nid
                    JOIN category_node cn ON cn.nid = n.nid
                    JOIN category c ON c.cid = cn.cid AND c.cnid = 12663
                		JOIN node n5 ON n5.nid = c.cid
                    JOIN category_node cn2 ON cn2.nid = n3.nid
                    JOIN category c2 ON c2.cid = cn2.cid AND c2.cnid = 12622
                    JOIN node n6 ON n6.nid = c2.cid AND n6.nid NOT IN (38308)

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
        using var readCommand = MysqlConnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;

        var reader = await readCommand.ExecuteReaderAsync();

        while (await reader.ReadAsync()) {


            var id = reader.GetInt32("id");

            int personId = await _nodeIdReader.ReadAsync(new Db.Readers.NodeIdReaderByUrlId.NodeIdReaderByUrlIdRequest 
            { 
                UrlId = reader.GetInt32("person_id"),
                TenantId = Constants.PPL,
            });
            int organizationId = await _nodeIdReader.ReadAsync(new Db.Readers.NodeIdReaderByUrlId.NodeIdReaderByUrlIdRequest 
            { 
                UrlId = reader.GetInt32("organization_id"),
                TenantId = Constants.PPL,
            });
            int? geographicalEntityId = reader.IsDBNull("geographical_entity_id") 
                    ? null 
                    : await _nodeIdReader.ReadAsync(new Db.Readers.NodeIdReaderByUrlId.NodeIdReaderByUrlIdRequest 
                    { 
                        UrlId = reader.GetInt32("geographical_entity_id"),
                        TenantId = Constants.PPL,
                    });
            int personOrganizationRelationTypeId = await _nodeIdReader.ReadAsync(new Db.Readers.NodeIdReaderByUrlId.NodeIdReaderByUrlIdRequest 
            { 
                UrlId = reader.GetInt32("nameable_id"),
                TenantId = Constants.PPL,
            });

            yield return new PersonOrganizationRelation {
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
                PersonId = personId,
                OrganizationId = organizationId,
                GeographicalEntityId = geographicalEntityId,
                PersonOrganizationRelationTypeId = personOrganizationRelationTypeId,
                DateRange = new DateTimeRange(reader.IsDBNull("start_date") ? null : reader.GetDateTime("start_date"), reader.IsDBNull("end_date") ? null : reader.GetDateTime("end_date")),

                DocumentIdProof = reader.IsDBNull("document_id_proof") 
                    ? null 
                    : await _nodeIdReader.ReadAsync(new Db.Readers.NodeIdReaderByUrlId.NodeIdReaderByUrlIdRequest 
                    { 
                        TenantId = Constants.PPL,
                        UrlId = reader.GetInt32("document_id_proof")
                    }),
                Description = reader.IsDBNull("description") ? null : reader.GetString("description"),
            };
        }
        await reader.CloseAsync();
    }

}
