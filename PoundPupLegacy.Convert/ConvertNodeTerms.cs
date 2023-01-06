using MySqlConnector;
using Npgsql;
using PoundPupLegacy.Db;
using PoundPupLegacy.Db.Readers;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert;

internal partial class Program
{

    private static async Task MigrateNodeTerms(MySqlConnection mysqlconnection, NpgsqlConnection connection)
    {
        await using var tx = await connection.BeginTransactionAsync();
        try
        {
            await NodeTermCreator.CreateAsync(ReadNodeTerms(mysqlconnection, connection), connection);
            await tx.CommitAsync();
        }
        catch (Exception)
        {
            await tx.RollbackAsync();
            throw;
        }
    }
    private static async IAsyncEnumerable<NodeTerm> ReadNodeTerms(MySqlConnection mysqlconnection, NpgsqlConnection connection)
    {

        await using var termReader = await TermReaderByName.CreateAsync(connection);

        var sql = $"""
                SELECT
                 cn.nid node_id,
                 n.title term_name
                FROM node n
                JOIN category c ON c.cid = n.nid AND c.cnid = 4126
                JOIN content_type_category_cat cc ON cc.nid = n.nid AND cc.vid = n.vid
                JOIN node_revisions nr ON nr.nid = n.nid AND nr.vid = n.vid
                LEFT JOIN node n2 ON n2.title = n.title AND n2.nid <> n.nid AND n2.`type` IN ('adopt_person','country_type', 'adopt_orgs', 'case', 'region_facts')
                LEFT JOIN node n3 ON n3.nid = cc.field_related_page_nid
                JOIN category_node cn ON cn.cid = n.nid
                WHERE  (n3.nid IS NULL OR n3.`type` = 'group')
                AND n.nid NOT IN (
                	22589
                )
                AND n2.nid IS  NULL
                """;
        using var readCommand = mysqlconnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;


        var reader = await readCommand.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var nodeId = reader.GetInt32("node_id");
            var termName = reader.GetString("term_name");

            var term = await termReader.ReadAsync(TOPICS, termName);

            yield return new NodeTerm
            {
                NodeId = nodeId,
                TermId = (int)term.Id!,
            };

        }
        await reader.CloseAsync();
    }
}
