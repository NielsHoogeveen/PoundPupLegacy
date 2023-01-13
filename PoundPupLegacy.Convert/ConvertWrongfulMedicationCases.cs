using MySqlConnector;
using Npgsql;
using PoundPupLegacy.Db;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert;

internal partial class Program
{


    private static async Task MigrateWrongfulMedicationCases(MySqlConnection mysqlconnection, NpgsqlConnection connection)
    {
        await using var tx = await connection.BeginTransactionAsync();
        try
        {
            await WrongfulMedicationCaseCreator.CreateAsync(ReadWrongfulMedicationCases(mysqlconnection), connection);
            await tx.CommitAsync();
        }
        catch (Exception)
        {
            await tx.RollbackAsync();
            throw;
        }
        
    }
    private static async IAsyncEnumerable<WrongfulMedicationCase> ReadWrongfulMedicationCases(MySqlConnection mysqlconnection)
    {

        var sql = $"""
                SELECT
                    n.nid id,
                    n.uid user_id,
                    n.title,
                    n.`status`,
                    FROM_UNIXTIME(n.created) created, 
                    FROM_UNIXTIME(n.changed) `changed`,
                    33 node_type_id,
                    field_long_description_1_value description,
                    field_report_date_0_value `date`,
                    ua.dst url_path
                FROM node n
                LEFT JOIN url_alias ua ON cast(SUBSTRING(ua.src, 6) AS INT) = n.nid
                JOIN content_type_wrongful_medication_case c ON c.nid = n.nid AND c.vid = n.vid
                """;
        using var readCommand = mysqlconnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;


        var reader = await readCommand.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var id = reader.GetInt32("id");
            var title = reader.GetString("title");
            var country = new WrongfulMedicationCase
            {
                Id = null,
                PublisherId = reader.GetInt32("user_id"),
                CreatedDateTime = reader.GetDateTime("created"),
                ChangedDateTime = reader.GetDateTime("changed"),
                Title = reader.GetString("title"),
                OwnerId = OWNER_CASES,
                TenantNodes = new List<TenantNode>
                {
                    new TenantNode
                    {
                        TenantId = 1,
                        PublicationStatusId = reader.GetInt32("status"),
                        UrlPath = reader.IsDBNull("url_path") ? null : reader.GetString("url_path"),
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = id
                    }
                },
                NodeTypeId = reader.GetInt32("node_type_id"),
                VocabularyNames = new List<VocabularyName>(),
                Date = reader.IsDBNull("date") ? null : StringToDateTimeRange(reader.GetString("date")),
                Description = reader.GetString("description"),
                FileIdTileImage = null,
            };
            yield return country;

        }
        await reader.CloseAsync();
    }
}
