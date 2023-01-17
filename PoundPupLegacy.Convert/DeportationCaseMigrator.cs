﻿using MySqlConnector;
using Npgsql;
using PoundPupLegacy.Db;
using PoundPupLegacy.Db.Readers;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert;

internal sealed class DeportationCaseMigrator: Migrator
{
    public DeportationCaseMigrator(MySqlToPostgresConverter mySqlToPostgresConverter) : base(mySqlToPostgresConverter)
    {
    }

    protected override string Name => "deportation cases";

    protected override async Task MigrateImpl()
    {
        await DeportationCaseCreator.CreateAsync(ReadDeportationCases(), _postgresConnection);
       
    }
    private async IAsyncEnumerable<DeportationCase> ReadDeportationCases()
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
                	END country_id_to,
                    ua.dst url_path
                FROM node n
                LEFT JOIN url_alias ua ON cast(SUBSTRING(ua.src, 6) AS INT) = n.nid
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
        using var readCommand = _mysqlConnection.CreateCommand();
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
                Id = null,
                PublisherId = reader.GetInt32("user_id"),
                CreatedDateTime = reader.GetDateTime("created"),
                ChangedDateTime = reader.GetDateTime("changed"),
                Title = name,
                OwnerId = Constants.OWNER_CASES,
                TenantNodes = new List<TenantNode>
                {
                    new TenantNode
                    {
                        Id = null,
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
                SubdivisionIdFrom = reader.IsDBNull("subdivision_id_from") ? null : await _nodeIdReader.ReadAsync(Constants.PPL, reader.GetInt32("subdivision_id_from")),
                CountryIdTo = reader.IsDBNull("country_id_to") ? null : await _nodeIdReader.ReadAsync(Constants.PPL, reader.GetInt32("country_id_to")),
                FileIdTileImage = null,
            };
            yield return country;

        }
        await reader.CloseAsync();
    }
}