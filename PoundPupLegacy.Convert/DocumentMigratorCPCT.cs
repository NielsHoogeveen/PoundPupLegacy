﻿using PoundPupLegacy.Db;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert;

internal sealed class DocumentMigratorCPCT : CPCTMigrator
{
    public DocumentMigratorCPCT(MySqlToPostgresConverter mySqlToPostgresConverter) : base(mySqlToPostgresConverter)
    {
    }

    protected override string Name => "documents cpct";

    protected override async Task MigrateImpl()
    {
        await DocumentCreator.CreateAsync(ReadDocuments(), _postgresConnection);
    }

    private async IAsyncEnumerable<(int, int)> GetDocumentablesWithStatus(IEnumerable<int> documentableIds)
    {
        foreach (var urlId in documentableIds) {
            yield return await GetNodeId(urlId);
        }
    }

    private async IAsyncEnumerable<Document> ReadDocuments()
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
                r.field_report_date_value publication_date,
                case when r.field_web_address_url = '' then null else r.field_web_address_url end source_url,
                nr.body `text`,
                c.document_type_id,
                GROUP_CONCAT(dd.documentable_id) documentable_ids
            FROM node n
            JOIN content_type_adopt_ind_rep r ON r.nid = n.nid AND r.vid = n.vid
            JOIN node_revisions nr ON nr.nid = n.nid AND nr.vid = n.vid
            LEFT JOIN (
            select 
                c.nid,
                n2.nid document_type_id
            FROM category_node c 
            JOIN node n2 ON n2.nid = c.cid
            JOIN category cat ON cat.cid = c.cid AND cat.cnid = 42416 
            JOIN node n3 ON n3.nid = cat.cnid 
            ) c ON c.nid = n.nid 
            LEFT JOIN (
                SELECT
                n2.nid documentable_id,
                n.nid document_id
                FROM content_field_pers_org cfr 
                JOIN node n ON n.nid = cfr.nid AND n.vid = cfr.vid
                JOIN content_type_adopt_ind_rep r ON r.nid = n.nid AND r.vid = n.vid
                JOIN node n2 ON n2.nid = cfr.field_pers_org_nid
            ) dd ON dd.document_id = n.nid
            WHERE n.`type` = 'adopt_ind_rep'
            AND n.nid > 33162
            GROUP BY
                n.nid,
                n.uid,
                n.title,
                n.`status`,
                n.created, 
                n.`changed`,
                r.field_report_date_value,
                r.field_web_address_url,
                nr.body,
                c.document_type_id
            """;
        using var readCommand = MysqlConnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;

        var reader = await readCommand.ExecuteReaderAsync();


        while (await reader.ReadAsync()) {
            var publicationDate = StringToDateTimeRange(reader.IsDBNull("publication_date") ? null : reader.GetString("publication_date"));
            var id = reader.GetInt32("id");
            var text = reader.GetString("text");

            var documentableIds = reader.IsDBNull("documentable_ids") ?
                new List<int>() :
                reader
                .GetString("documentable_ids")
                .Split(',')
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(x => int.Parse(x))
                .Distinct()
                .ToList();

            var documentable = await GetDocumentablesWithStatus(documentableIds).ToListAsync();

            var tenantNodes = new List<TenantNode>
                {
                    new TenantNode
                    {
                        Id = null,
                        TenantId = Constants.CPCT,
                        PublicationStatusId = 2,
                        UrlPath = null,
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = id
                    }
                };

            if (documentable.All(x => x.Item2 == 1) && !text.ToLower().Contains("arun dohle") && !text.ToLower().Contains("roelie post") && !text.ToLower().Contains("againstchildtrafficking.org")) {
                tenantNodes.Add(new TenantNode {
                    Id = null,
                    TenantId = Constants.PPL,
                    PublicationStatusId = 1,
                    UrlPath = null,
                    NodeId = null,
                    SubgroupId = null,
                    UrlId = null
                });
            }

            yield return new Document {
                Id = null,
                PublisherId = reader.GetInt32("user_id"),
                CreatedDateTime = reader.GetDateTime("created"),
                ChangedDateTime = reader.GetDateTime("changed"),
                Title = reader.GetString("title"),
                OwnerId = Constants.OWNER_DOCUMENTATION,
                TenantNodes = tenantNodes,
                NodeTypeId = reader.GetInt16("node_type_id"),
                PublicationDate = publicationDate,
                SourceUrl = reader.IsDBNull("source_url") ? null : reader.GetString("source_url"),
                Text = TextToHtml(text),
                Teaser = TextToTeaser(text),
                DocumentTypeId = reader.IsDBNull("document_type_id") ? null : await _nodeIdReader.ReadAsync(Constants.PPL, reader.GetInt32("document_type_id")),
                Documentables = documentable.Select(x => x.Item1).ToList()
            };

        }
        await reader.CloseAsync();
    }
}
