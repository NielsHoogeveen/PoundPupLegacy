﻿using MySqlConnector;
using Npgsql;
using PoundPupLegacy.Db;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert;

internal partial class Program
{


    private static async Task MigrateDeportationCases(MySqlConnection mysqlconnection, NpgsqlConnection connection)
    {
        await using var tx = await connection.BeginTransactionAsync();
        try
        {
            await DeportationCaseCreator.CreateAsync(ReadDeportationCases(mysqlconnection), connection);
            await tx.CommitAsync();
        }
        catch (Exception)
        {
            await tx.RollbackAsync();
            throw;
        }
       
    }
    private static async IAsyncEnumerable<DeportationCase> ReadDeportationCases(MySqlConnection mysqlconnection)
    {

        var sql = $"""
                SELECT
                     n.nid id,
                     n.uid user_id,
                     n.title,
                     n.`status`,
                     FROM_UNIXTIME(n.created) created, 
                     FROM_UNIXTIME(n.changed) `changed`,
                     31 node_type_id,
                     field_description_6_value description,
                     MIN(cf.field_date_value) `date`,
                     case 
                	  		when field_state_nid = 0 then null 
                		  else field_state_nid
                	  end subdivision_id_from,
                	  case
                		when field_country_0_nid = 0 then null
                		ELSE field_country_0_nid
                	  END country_id_to
                FROM node n
                JOIN content_type_deportation_case c ON c.nid = n.nid AND c.vid = n.vid
                LEFT JOIN content_field_cases fc ON fc.field_cases_nid = n.nid
                LEFT JOIN node n3 ON fc.nid = n3.nid AND fc.vid = n3.vid
                LEFT JOIN content_type_case_file cf ON cf.nid = n3.nid AND cf.vid = n3.vid
                GROUP BY 
                     n.nid,
                     n.uid,
                     n.title,
                     n.`status`,
                     n.created, 
                     n.changed,
                     field_description_6_value
                """;
        using var readCommand = mysqlconnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;


        var reader = await readCommand.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var id = reader.GetInt32("id");
            var name = reader.GetString("title");
            var country = new DeportationCase
            {
                Id = id,
                AccessRoleId = reader.GetInt32("user_id"),
                CreatedDateTime = reader.GetDateTime("created"),
                ChangedDateTime = reader.GetDateTime("changed"),
                Title = name,
                NodeStatusId = reader.GetInt32("status"),
                NodeTypeId = reader.GetInt32("node_type_id"),
                VocabularyNames = new List<VocabularyName>(),
                Date = reader.IsDBNull("date") ? null : StringToDateTimeRange(reader.GetString("date")),
                Description = reader.GetString("description"),
                SubdivisionIdFrom = reader.IsDBNull("subdivision_id_from") ? null : reader.GetInt32("subdivision_id_from"),
                CountryIdTo = reader.IsDBNull("country_id_to") ? null : reader.GetInt32("country_id_to"),
                FileIdTileImage = null,
            };
            yield return country;

        }
        await reader.CloseAsync();
    }
}
