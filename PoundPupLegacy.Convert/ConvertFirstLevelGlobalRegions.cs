﻿using MySqlConnector;
using Npgsql;
using PoundPupLegacy.Db;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert
{
    internal partial class Program
    {
        private static void MigrateFirstLevelGlobalRegions(MySqlConnection mysqlconnection, NpgsqlConnection connection)
        {
            FirstLevelGlobalRegionCreator.Create(ReadFirstLevelGlobalRegions(mysqlconnection), connection);
        }

        private static IEnumerable<FirstLevelGlobalRegion> ReadFirstLevelGlobalRegions(MySqlConnection mysqlconnection)
        {
            var sql = $"""
                SELECT n.nid id,
                n.uid user_id,
                n.title,
                n.`status`,
                FROM_UNIXTIME(n.created) created, 
                FROM_UNIXTIME(n.changed) `changed`
                FROM node n 
                JOIN category_hierarchy ch ON ch.cid = n.nid
                JOIN node n2 ON n2.nid = ch.parent
                WHERE n.`type` = 'region_facts'
                AND n.nid < 30000
                AND n2.`type` = 'category_cont'
                """;

            using var readCommand = mysqlconnection.CreateCommand();
            readCommand.CommandType = CommandType.Text;
            readCommand.CommandTimeout = 300;
            readCommand.CommandText = sql;

            var reader = readCommand.ExecuteReader();

            while (reader.Read())
            {
                yield return new FirstLevelGlobalRegion
                {
                    Id = reader.GetInt32("id"),
                    UserId = reader.GetInt32("user_id"),
                    Created = reader.GetDateTime("created"),
                    Changed = reader.GetDateTime("changed"),
                    Title = reader.GetString("title"),
                    Status = reader.GetInt32("status"),
                    NodeTypeId = 11,
                    IsTerm = true
                };
            }
            reader.Close();
        }
    }
}

