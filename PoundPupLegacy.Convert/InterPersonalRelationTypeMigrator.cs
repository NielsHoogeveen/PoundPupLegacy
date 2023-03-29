﻿namespace PoundPupLegacy.Convert;

internal sealed class InterPersonalRelationTypeMigrator : PPLMigrator
{

    public InterPersonalRelationTypeMigrator(MySqlToPostgresConverter converter) : base(converter) { }

    protected override string Name => "inter-personal relation types";

    protected override async Task MigrateImpl()
    {
        await InterPersonalRelationTypeCreator.CreateAsync(ReadInterPersonalRelationTypes(), _postgresConnection);
    }
    private async IAsyncEnumerable<InterPersonalRelationType> ReadInterPersonalRelationTypes()
    {

        var sql = $"""
                SELECT
                    n.nid id,
                    n.uid access_role_id,
                    n.title,
                    n.`status` node_status_id,
                    FROM_UNIXTIME(n.created) created_date_time, 
                    FROM_UNIXTIME(n.changed) changed_date_time,
                    '' description,
                NULL file_id_tile_image,
                case 
                    when n.nid IN (16911, 16904, 16909, 16912, 16916, 35216) then true
                    ELSE false
                END is_symmetric,
                ua.dst url_path
                FROM node n
                LEFT JOIN url_alias ua ON cast(SUBSTRING(ua.src, 6) AS INT) = n.nid
                JOIN node_revisions nr ON nr.nid = n.nid AND nr.vid = n.vid
                JOIN category c ON c.cid = n.nid AND c.cnid = 16900
                """;

        using var readCommand = MysqlConnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;

        var reader = await readCommand.ExecuteReaderAsync();

        while (await reader.ReadAsync()) {
            var id = reader.GetInt32("id");
            var name = reader.GetString("title");

            var vocabularyNames = new List<VocabularyName>
            {
                new VocabularyName
                {
                    OwnerId = Constants.OWNER_PARTIES,
                    Name = Constants.VOCABULARY_INTERPERSONAL_RELATION_TYPE,
                    TermName = name,
                    ParentNames = new List<string>(),
                }
            };
            yield return new InterPersonalRelationType {
                Id = null,
                PublisherId = reader.GetInt32("access_role_id"),
                CreatedDateTime = reader.GetDateTime("created_date_time"),
                ChangedDateTime = reader.GetDateTime("changed_date_time"),
                Title = name,
                OwnerId = Constants.OWNER_PARTIES,
                TenantNodes = new List<TenantNode>
                {
                    new TenantNode
                    {
                        Id = null,
                        TenantId = Constants.PPL,
                        PublicationStatusId = reader.GetInt32("node_status_id"),
                        UrlPath = reader.IsDBNull("url_path") ? null : reader.GetString("url_path"),
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = id
                    },
                    new TenantNode
                    {
                        Id = null,
                        TenantId = Constants.CPCT,
                        PublicationStatusId = 2,
                        UrlPath = null,
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = id < 33163 ? id : null
                    }
                },
                NodeTypeId = 5,
                Description = reader.GetString("description"),
                FileIdTileImage = reader.IsDBNull("file_id_tile_image")
                    ? null
                        : await _fileIdReaderByTenantFileId.ReadAsync(new FileIdReaderByTenantFileId.FileIdReaderByTenantFileIdRequest {
                            TenantId = Constants.PPL,
                            TenantFileId = reader.GetInt32("file_id_tile_image")
                        }),
                VocabularyNames = vocabularyNames,
                IsSymmetric = reader.GetBoolean("is_symmetric"),
            };

        }
        await reader.CloseAsync();
    }
}
