﻿namespace PoundPupLegacy.Convert;

internal sealed class InterOrganizationalRelationTypeMigrator : MigratorPPL
{
    private readonly IDatabaseReaderFactory<FileIdReaderByTenantFileId> _fileIdReaderByTenantFileIdFactory;
    private readonly IEntityCreator<InterOrganizationalRelationType> _interOrganizationalRelationTypeCreator;
    public InterOrganizationalRelationTypeMigrator(
        IDatabaseConnections databaseConnections,
        IDatabaseReaderFactory<FileIdReaderByTenantFileId> fileIdReaderByTenantFileIdFactory,
        IEntityCreator<InterOrganizationalRelationType> interOrganizationalRelationTypeCreator
    ) : base(databaseConnections)
    {
        _fileIdReaderByTenantFileIdFactory = fileIdReaderByTenantFileIdFactory;
        _interOrganizationalRelationTypeCreator = interOrganizationalRelationTypeCreator;
    }

    protected override string Name => "inter-organization relation types";

    protected override async Task MigrateImpl()
    {
        await using var fileIdReaderByTenantFileId = await _fileIdReaderByTenantFileIdFactory.CreateAsync(_postgresConnection);
        await _interOrganizationalRelationTypeCreator.CreateAsync(ReadInterOrganizationalRelationTypes(fileIdReaderByTenantFileId), _postgresConnection);
    }
    private async IAsyncEnumerable<InterOrganizationalRelationType> ReadInterOrganizationalRelationTypes(FileIdReaderByTenantFileId fileIdReaderByTenantFileId)
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
                    	when n.nid IN (51470, 64420, 14977) then true
                    	ELSE false
                    END is_symmetric,
                    ua.dst url_path
                FROM node n
                LEFT JOIN url_alias ua ON cast(SUBSTRING(ua.src, 6) AS INT) = n.nid
                JOIN node_revisions nr ON nr.nid = n.nid AND nr.vid = n.vid
                JOIN category c ON c.cid = n.nid AND c.cnid = 12637
                """;

        using var readCommand = _mySqlConnection.CreateCommand();
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
                    Name = Constants.VOCABULARY_INTERORGANIZATIONAL_RELATION_TYPE,
                    TermName = name,
                    ParentNames = new List<string>(),
                }
            };
            yield return new InterOrganizationalRelationType {
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
                NodeTypeId = 2,
                Description = reader.GetString("description"),
                FileIdTileImage = reader.IsDBNull("file_id_tile_image")
                    ? null
                    : await fileIdReaderByTenantFileId.ReadAsync(new FileIdReaderByTenantFileId.Request {
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
