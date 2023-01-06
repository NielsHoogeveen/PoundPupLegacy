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
                     field_report_date_0_value `date`
                FROM node n
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
                Id = reader.GetInt32("id"),
                AccessRoleId = reader.GetInt32("user_id"),
                CreatedDateTime = reader.GetDateTime("created"),
                ChangedDateTime = reader.GetDateTime("changed"),
                Title = reader.GetString("title"),
                NodeStatusId = reader.GetInt32("status"),
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
