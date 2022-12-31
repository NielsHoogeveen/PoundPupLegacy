﻿using MySqlConnector;
using Npgsql;
using PoundPupLegacy.Db;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert;

internal partial class Program
{

    private static void MigrateDocuments(MySqlConnection mysqlconnection, NpgsqlConnection connection)
    {
        DocumentCreator.Create(ReadDocuments(mysqlconnection), connection);
    }

    private static string LastDayOfMonth(string month, int year)
    {
        return month switch
        {
            "01" => "31",
            "02" => year % 400 == 0 ? "29" : year % 100 == 0 ? "28" : year % 4 == 0 ? "29" : "28",
            "03" => "31",
            "04" => "30",
            "05" => "31",
            "06" => "30",
            "07" => "31",
            "08" => "31",
            "09" => "30",
            "10" => "31",
            "11" => "30",
            "12" => "31",
        };
    }

    private static DateTimeRange? StringToDateTimeRange(string? str)
    {
        if (str is null)
        {
            return null;
        }
        if (DateTime.TryParse(str, out var dt))
        {
            return new DateTimeRange(dt, dt);
        }
        else
        {
            if (str.Substring(5, 2) == "00")
            {
                var year = str.Substring(0, 4);
                var dateFrom = DateTime.Parse($"{year}-01-01");
                var dateTo = DateTime.Parse($"{year}-12-31");
                return new DateTimeRange(dateFrom, dateTo);
            }
            if (str.Substring(8, 2) == "00")
            {
                var year = str.Substring(0, 4);
                var month = str.Substring(5, 2);
                var dateFrom = DateTime.Parse($"{year}-{month}-01");
                var dateTo = DateTime.Parse($"{year}-{month}-{LastDayOfMonth(month, int.Parse(year))}");
                return new DateTimeRange(dateFrom, dateTo);
            }
            throw new NotSupportedException($"Cannot convert {str} to a date time range");

        }
    }
    private static IEnumerable<Document> ReadDocuments(MySqlConnection mysqlconnection)
    {

        var sql = $"""
                SELECT
                    n.nid id,
                    n.uid user_id,
                    n.title,
                    n.`status`,
                    FROM_UNIXTIME(n.created) created, 
                    FROM_UNIXTIME(n.changed) `changed`,
                    10 node_type_id,
                    FALSE is_topic,
                    r.field_report_date_value publication_date,
                    case when r.field_web_address_url = '' then null else r.field_web_address_url end source_url,
                    nr.body `text`,
                    n2.nid document_type_id
                FROM node n
                JOIN content_type_adopt_ind_rep r ON r.nid = n.nid AND r.vid = n.vid
                JOIN node_revisions nr ON nr.nid = n.nid AND nr.vid = n.vid
                LEFT JOIN category_node c ON c.nid = n.nid 
                LEFT JOIN node n2 ON n2.nid = c.cid
                LEFT JOIN category cat ON cat.cid = c.cid 
                LEFT JOIN node n3 ON n3.nid = cat.cnid 
                WHERE cat.cnid is null or cat.cnid = 42416 AND nr.body <> ''
                UNION
                SELECT
                    n.nid id,
                    n.uid user_id,
                    n.title,
                    n.`status`,
                    FROM_UNIXTIME(n.created) created, 
                    FROM_UNIXTIME(n.changed) `changed`,
                    10 node_type_id,
                    FALSE is_topic,
                    r.field_date_value publication_date,
                    case when r.field_source_url = '' then null else r.field_source_url end source_url,
                    r.field_body_value `text`,
                    n2.nid document_type_id
                FROM node n
                JOIN content_type_case_file r ON r.nid = n.nid AND r.vid = n.vid
                JOIN node_revisions nr ON nr.nid = n.nid AND nr.vid = n.vid
                LEFT JOIN category_node c ON c.nid = n.nid 
                LEFT JOIN node n2 ON n2.nid = c.cid
                LEFT JOIN category cat ON cat.cid = c.cid 
                LEFT JOIN node n3 ON n3.nid = cat.cnid 
                WHERE cat.cnid is null or cat.cnid = 42416 AND r.field_body_value <> ''
                """;
        using var readCommand = mysqlconnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;


        var reader = readCommand.ExecuteReader();

        while (reader.Read())
        {
            var publicationDate = StringToDateTimeRange(reader.IsDBNull("publication_date") ? null : reader.GetString("publication_date"));
            yield return new Document
            {
                Id = reader.GetInt32("id"),
                AccessRoleId = reader.GetInt32("user_id"),
                CreatedDateTime = reader.GetDateTime("created"),
                ChangedDateTime = reader.GetDateTime("changed"),
                Title = reader.GetString("title"),
                NodeStatusId = reader.GetInt32("status"),
                NodeTypeId = reader.GetInt16("node_type_id"),
                PublicationDate = publicationDate,
                SourceUrl = reader.IsDBNull("source_url") ? null : reader.GetString("source_url"),
                Text = reader.GetString("text"),
                DocumentTypeId = reader.IsDBNull("document_type_id") ? null : reader.GetInt32("document_type_id"),
            };

        }
        reader.Close();
    }
}