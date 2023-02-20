using PoundPupLegacy.Db;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert;

internal sealed class PartyPoliticalEntityRelationMigratorPPL : PPLMigrator
{
    public PartyPoliticalEntityRelationMigratorPPL(MySqlToPostgresConverter mySqlToPostgresConverter) : base(mySqlToPostgresConverter)
    {
    }

    protected override string Name => "party political enitity relation";

    protected override async Task MigrateImpl()
    {
        await PartyPoliticalEntityRelationCreator.CreateAsync(ReadPartyPoliticalEntityRelations(), _postgresConnection);

    }

    private async IAsyncEnumerable<PartyPoliticalEntityRelation> ReadPartyPoliticalEntityRelations()
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
                    when id = 61872 then  STR_TO_DATE(REPLACE('1994-01-15', '-00', '-01'),'%Y-%m-%d')
                	when start_date > end_date then end_date
                	ELSE start_date
                end start_date,
                case 
                    when id = 30667 then null
                	when start_date > end_date then start_date
                	ELSE end_date
                end end_date,
                case 
                    when id = 61872 then 12654
                    else nameable_id
                end nameable_id,
                vocabulary_id,
                document_id_proof
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
                	  c.cnid vocabulary_id,
                	  case when fp.field_proof_nid = 0 then null
                	  ELSE fp.field_proof_nid 
                	  end document_id_proof
                	FROM node n
                	JOIN content_type_adopt_country_link p ON p.nid = n.nid AND p.vid = n.vid
                	JOIN node n2 ON n2.nid = p.field_person_or_organization_nid
                	JOIN node n3 ON n3.nid = p.field_country_nid
                	JOIN category_node cn ON cn.nid = n.nid
                	JOIN node n4 ON n4.nid = cn.cid
                	JOIN category c ON c.cid = n4.nid AND c.cnid = 12652
                	LEFT JOIN content_field_proof fp ON fp.nid = n.nid AND fp.vid = n.vid
                	WHERE n.nid not in (13792) and n2.nid not in (74250)
                	GROUP BY n2.nid,
                	n3.nid,
                	p.field_from_date_value,
                	p.field_to_date_0_value,
                	n4.nid,
                	c.cnid
                ) x
                """;
        using var readCommand = MysqlConnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;

        var reader = await readCommand.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {


            var id = reader.GetInt32("id");

            int partyId = await _nodeIdReader.ReadAsync(Constants.PPL, reader.GetInt32("party_id"));
            int politicalEntityId = await _nodeIdReader.ReadAsync(Constants.PPL, reader.GetInt32("political_entity_id"));
            int partyPpoliticalEntityTypeId = await _nodeIdReader.ReadAsync(Constants.PPL, reader.GetInt32("nameable_id"));

            yield return new PartyPoliticalEntityRelation
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
                        TenantId = Constants.PPL,
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
                NodeTypeId = 49,
                PartyId = partyId,
                PoliticalEntityId = politicalEntityId,
                PartyPoliticalEntityRelationTypeId = partyPpoliticalEntityTypeId,
                DateRange = new DateTimeRange(reader.IsDBNull("start_date") ? null : reader.GetDateTime("start_date"), reader.IsDBNull("end_date") ? null : reader.GetDateTime("end_date")),
                DocumentIdProof = reader.IsDBNull("document_id_proof") ? null : await _nodeIdReader.ReadAsync(Constants.PPL, reader.GetInt32("document_id_proof"))
            };
        }
        await reader.CloseAsync();
    }

}
