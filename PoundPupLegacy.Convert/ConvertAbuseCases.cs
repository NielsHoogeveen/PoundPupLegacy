﻿using MySqlConnector;
using Npgsql;
using PoundPupLegacy.Db;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert;

internal partial class Program
{


    private static async Task MigrateAbuseCases(MySqlConnection mysqlconnection, NpgsqlConnection connection)
    {
        await using var tx = await connection.BeginTransactionAsync();
        try
        {
            await AbuseCaseCreator.CreateAsync(ReadAbuseCases(mysqlconnection), connection);
            await tx.CommitAsync();
        }
        catch(Exception)
        {
            await tx.RollbackAsync();
            throw;
        }
    }
    private static async IAsyncEnumerable<AbuseCase> ReadAbuseCases(MySqlConnection mysqlconnection)
    {

        var sql = $"""
                SELECT
                n.nid id,
                n.uid access_role_id,
                n.title,
                n.`status` node_status_id,
                FROM_UNIXTIME(n.created) created_date_time, 
                FROM_UNIXTIME(n.changed) changed_date_time,
                26 node_type_id,
                		case 
                        when c2.title IS NOT NULL then c2.title
                        ELSE c3.title
                END topic_name,
                case 
                    when c2.topic_parent_names IS NOT NULL then c2.topic_parent_names
                    ELSE c3.topic_parent_names
                END topic_parent_names,
                		c.field_discovery_date_value `date`,
                c.field_body_0_value description,
                case 
                    when field_child_placement_type_value = 'Adoption' then {ADOPTION}
                    when field_child_placement_type_value = 'Foster care' then {FOSTER_CARE}
                    when field_child_placement_type_value = 'To be adopted' then {TO_BE_ADOPTED}
                    when field_child_placement_type_value = 'Legal Guardianship' then {LEGAL_GUARDIANSHIP}
                    when field_child_placement_type_value = 'Institution' then {INSTITUTION}
                END child_placement_type_id,
                case 
                    when c.field_family_size_value = '1 to 4' then {ONE_TO_FOUR}
                    when c.field_family_size_value = '4 to 8' then {FOUR_TO_EIGHT}
                    when c.field_family_size_value = '8 to 12' then {EIGHT_TO_TWELVE}
                    when c.field_family_size_value = 'more than 12' then {MORE_THAN_TWELVE}
                    when field_child_placement_type_value = '' then null
                    when field_child_placement_type_value = null then null
                END family_size_id,
                case when c.field_home_schooling_value = 'yes' then true else null END home_schooling_involved,
                case when field_fundamentalist_faith_value = 'yes' then TRUE ELSE NULL END fundamental_faith_involved,
                case when field_disabilities_value = 'yes' then TRUE ELSE NULL END disabilities_involved
                FROM node n
                JOIN content_type_case c ON c.nid = n.nid AND c.vid = n.vid
                LEFT JOIN content_type_category_cat cc ON cc.field_related_page_nid = n.nid AND cc.nid <> 44518
                LEFT JOIN node n2 ON n2.nid = cc.nid AND n2.vid = cc.vid
                LEFT JOIN (
                    select
                    n.nid,
                    n.title,
                    cc.field_tile_image_title,
                    cc.field_related_page_nid,
                    GROUP_CONCAT(p.title, ',') topic_parent_names
                    FROM node n
                    JOIN content_type_category_cat cc ON cc.nid = n.nid AND cc.vid = n.vid
                    LEFT JOIN (
                        SELECT
                        n.nid, 
                        n.title,
                        ch.cid
                        FROM node n
                        JOIN category_hierarchy ch ON ch.parent = n.nid
                        WHERE n.`type` = 'category_cat'
                    ) p ON p.cid = n.nid
                    WHERE n.nid NOT IN (44881)
                    GROUP BY
                    n.nid,
                    n.title,
                    cc.field_tile_image_title,
                    cc.field_related_page_nid
                ) c2 ON c2.field_related_page_nid = n.nid
                		LEFT JOIN (
                    select
                        n.nid,
                        n.title,
                        GROUP_CONCAT(p.title, ',') topic_parent_names
                    FROM node n
                    JOIN category c ON c.cid = n.nid AND c.cnid = 4126
                    LEFT JOIN (
                        SELECT
                            n.nid, 
                            n.title,
                            ch.cid
                        FROM node n
                        JOIN category_hierarchy ch ON ch.parent = n.nid
                        WHERE n.`type` = 'category_cat'
                    ) p ON p.cid = n.nid
                    GROUP BY 
                        n.nid,
                        n.title
                ) c3 ON c3.title = n.title                
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

            var vocabularyNames = new List<VocabularyName>();

            if (!reader.IsDBNull("topic_name"))
            {
                var topicName = reader.GetString("topic_name");
                var topicParentNames = reader.IsDBNull("topic_parent_names") ?
                    new List<string>() : reader.GetString("topic_parent_names")
                    .Split(',')
                    .Where(x => !string.IsNullOrEmpty(x))
                    .ToList();

                vocabularyNames.Add(new VocabularyName
                {
                    VocabularyId = TOPICS,
                    Name = topicName,
                    ParentNames = topicParentNames,
                });
            }


            var country = new AbuseCase
            {
                Id = id,
                AccessRoleId = reader.GetInt32("access_role_id"),
                CreatedDateTime = reader.GetDateTime("created_date_time"),
                ChangedDateTime = reader.GetDateTime("changed_date_time"),
                Title = name,
                NodeStatusId = reader.GetInt32("node_status_id"),
                NodeTypeId = reader.GetInt32("node_type_id"),
                Date = reader.IsDBNull("date") ? null : StringToDateTimeRange(reader.GetString("date")),
                Description = reader.GetString("description"),
                FileIdTileImage = null,
                ChildPlacementTypeId = reader.GetInt32("child_placement_type_id"),
                FamilySizeId = reader.IsDBNull("family_size_id") ? null : reader.GetInt32("family_size_id"),
                HomeschoolingInvolved = reader.IsDBNull("home_schooling_involved") ? null : reader.GetBoolean("home_schooling_involved"),
                FundamentalFaithInvolved = reader.IsDBNull("fundamental_faith_involved") ? null : reader.GetBoolean("fundamental_faith_involved"),
                DisabilitiesInvolved = reader.IsDBNull("disabilities_involved") ? null : reader.GetBoolean("disabilities_involved"),
                VocabularyNames = vocabularyNames,
            };
            yield return country;

        }
        await reader.CloseAsync();
    }
}
