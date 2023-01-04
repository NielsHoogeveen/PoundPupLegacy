﻿using MySqlConnector;
using Npgsql;
using PoundPupLegacy.Db;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert
{
    internal partial class Program
    {

        private static async Task MigrateProfessions(MySqlConnection mysqlconnection, NpgsqlConnection connection)
        {
            await ProfessionCreator.CreateAsync(ReadProfessions(mysqlconnection), connection);
        }
        private static async IAsyncEnumerable<Profession> ReadProfessions(MySqlConnection mysqlconnection)
        {

            var sql = $"""
                    SELECT
                       n.nid id,
                       n.uid access_role_id,
                       n.title,
                       n.`status` node_status_id,
                       FROM_UNIXTIME(n.created) created_date_time, 
                       FROM_UNIXTIME(n.changed) changed_date_time,
                       case 
                    		when n2.body is NULL then ''
                    		ELSE n2.body
                    	END description,
                    	case 
                    		when n2.field_tile_image_fid = 0 then null
                    		ELSE n2.field_tile_image_fid
                    	END file_id_tile_image,
                       n2.title topic_name,
                       case
                       when n.nid IN (27214,27215) then true
                       ELSE false
                       END has_concrete_subtype
                    FROM node n
                    JOIN node_revisions nr ON nr.nid = n.nid AND nr.vid = n.vid
                    JOIN category c ON c.cid = n.nid AND c.cnid = 27213
                    LEFT JOIN (
                    SELECT 27214 nameable_id, 'lawyers' topic_name
                    UNION
                    SELECT 27215, 'therapists'
                    ) v ON v.nameable_id = n.nid
                    LEFT JOIN (
                    SELECT 
                    n2.nid,
                    n2.title,
                    nr2.body,
                    cc.field_tile_image_fid
                    FROM node n2 
                    JOIN category c ON c.cid = n2.nid AND c.cnid = 4126
                    JOIN content_type_category_cat cc ON cc.vid = n2.vid AND cc.nid = n2.nid
                    JOIN node_revisions nr2 ON nr2.nid = n2.nid AND nr2.vid = n2.vid
                    WHERE n2.`type` = 'category_cat'
                    ) n2 ON n2.title = v.topic_name
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
                var topicName = reader.IsDBNull("topic_name") ? null : reader.GetString("topic_name");

                var vocabularyNames = new List<VocabularyName>
                {
                    new VocabularyName
                    {
                        VocabularyId = 27213,
                        Name = name,
                        ParentNames = new List<string>(),
                    }
                };
                if (topicName != null)
                {
                    vocabularyNames.Add(new VocabularyName
                    {
                        VocabularyId = TOPICS,
                        Name = topicName,
                        ParentNames = new List<string>()
                    });
                }

                yield return new Profession
                {
                    Id = id,
                    AccessRoleId = reader.GetInt32("access_role_id"),
                    CreatedDateTime = reader.GetDateTime("created_date_time"),
                    ChangedDateTime = reader.GetDateTime("changed_date_time"),
                    Title = name,
                    NodeStatusId = reader.GetInt32("node_status_id"),
                    NodeTypeId = 6,
                    Description = reader.GetString("description"),
                    FileIdTileImage = reader.IsDBNull("file_id_tile_image") ? null : reader.GetInt32("file_id_tile_image"),
                    VocabularyNames = vocabularyNames,
                    HasConcreteSubtype = reader.GetBoolean("has_concrete_subtype"),
                };
            }
            reader.Close();
        }
    }
}
