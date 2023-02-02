using PoundPupLegacy.Db;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert;

internal sealed class NodeTermMigrator: PPLMigrator
{
    public NodeTermMigrator(MySqlToPostgresConverter mySqlToPostgresConverter) : base(mySqlToPostgresConverter)
    {
    }

    protected override string Name => "node terms";

    protected override async Task MigrateImpl()
    {
        
        await NodeTermCreator.CreateAsync(ReadNodeTerms(), _postgresConnection);
        
    }
    private async IAsyncEnumerable<NodeTerm> ReadNodeTerms()
    {

        var sql = $"""
                SELECT
                    DISTINCT
                    n.nid node_id,
                    case
                        when n4.node_id = 19316 then 39429
                        when n4.node_id = 4806 then 17310
                        when n4.node_id = 19326 then 35715
                        when n4.node_id = 19447 then 31716
                        when n4.node_id = 19597 then 12632
                        when n4.node_id = 20242 then 48309
                        when n4.node_id = 4816 then 12635
                        when n4.node_id = 27466 then 12635
                        when n4.node_id = 47130 then 12624
                        when n4.node_id = 74755 then 31586
                        when n4.node_id = 18644 then 14670
                        when n4.node_id = 20899 then 27215
                        when n4.node_id = 6167 then 27214
                        when n4.node_id = 4209 then 12625
                        when n4.node_id = 19256 then 6135
                        when n4.node_id = 22589 then 22591
                        when n4.node_id = 45656 then 41375
                        when n4.node_id = 74730 then 55660
                        when n3.node_id IS NULL then n4.node_id
                        ELSE n3.node_id
                    end nameable_id
                FROM node n
                JOIN category_node cn ON cn.nid = n.nid
                JOIN category c ON c.cid = cn.cid AND c.cnid = 4126
                LEFT JOIN(
                    SELECT 
                    cc.nid,
                    n.nid node_id
                    FROM content_type_category_cat cc 
                    JOIN node n3 ON n3.nid = cc.nid AND n3.vid = cc.vid
                    JOIN node n ON n.nid = cc.field_related_page_nid
                    WHERE n.`type` NOT IN ('group')
                ) n3 ON n3.nid = c.cid
                LEFT JOIN(
                    SELECT 
                    case 
                		when n2.nid is NULL then n.nid 
                		ELSE n2.nid
                	end node_id,
                    n.nid
                    FROM node n 
                    LEFT JOIN node n2 ON n2.title = n.title and n2.`type` in ('adopt_person','country_type', 'adopt_orgs', 'case', 'region_facts', 'coerced_adoption_cases', 'child_trafficking', 'child_trafficking_case')
                ) n4 ON n4.nid = c.cid
                WHERE  n.nid NOT IN (
                    22589,
                    54123
                )
                AND (n3.nid IS NOT NULL OR n4.nid IS NOT NULL)
                AND n.`type` NOT IN ('amazon_node', 'poll', 'video', 'amazon', 'website', 'image', 'award_poll', 'book_page', 'panel', 'viewnode')
                AND n.uid <> 0
                """;
        using var readCommand = MysqlConnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;


        var reader = await readCommand.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var nodeId = await _nodeIdReader.ReadAsync(Constants.PPL, reader.GetInt32("node_id"));
            var nameableId = await _nodeIdReader.ReadAsync(Constants.PPL, reader.GetInt32("nameable_id"));


            var term = await _termReaderByNameableId.ReadAsync(Constants.PPL, Constants.VOCABULARY_TOPICS, nameableId);

            yield return new NodeTerm
            {
                NodeId = nodeId,
                TermId = (int)term.Id!,
            };

        }
        await reader.CloseAsync();
    }
}
