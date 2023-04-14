using PoundPupLegacy.CreateModel.Updaters;

namespace PoundPupLegacy.Convert;

internal sealed class VocabularyMigrator : MigratorPPL
{
    private readonly IMandatorySingleItemDatabaseReaderFactory<NodeIdReaderByUrlIdRequest, int> _nodeIdReaderByUrlIdFactory;
    private readonly IEntityCreator<Vocabulary> _vocabularyCreator;
    private readonly IDatabaseUpdaterFactory<TenantUpdaterSetTaggingVocabularyRequest> _tenantUpdaterSetTaggingVocabularyFactory;

    protected override string Name => "vocabularies";
    public VocabularyMigrator(
        IDatabaseConnections databaseConnections,
        IMandatorySingleItemDatabaseReaderFactory<NodeIdReaderByUrlIdRequest, int> nodeIdReaderByUrlIdFactory,
        IEntityCreator<Vocabulary> vocabularyCreator,
        IDatabaseUpdaterFactory<TenantUpdaterSetTaggingVocabularyRequest> tenantUpdaterSetTaggingVocabularyFactory
    ) : base(databaseConnections)
    {
        _nodeIdReaderByUrlIdFactory = nodeIdReaderByUrlIdFactory;
        _vocabularyCreator = vocabularyCreator;
        _tenantUpdaterSetTaggingVocabularyFactory = tenantUpdaterSetTaggingVocabularyFactory;
    }

    private static async IAsyncEnumerable<Vocabulary> GetVocabularies()
    {
        await Task.CompletedTask;
        yield return new Vocabulary {
            Id = null,
            Name = Constants.VOCABULARY_CHILD_PLACEMENT_TYPE,
            PublisherId = 1,
            CreatedDateTime = DateTime.Now,
            ChangedDateTime = DateTime.Now,
            Title = Constants.VOCABULARY_CHILD_PLACEMENT_TYPE,
            OwnerId = Constants.OWNER_CASES,
            TenantNodes = new List<TenantNode>
                {
                    new TenantNode
                    {
                        Id = null,
                        TenantId = 1,
                        PublicationStatusId = 1,
                        UrlPath = null,
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = Constants.CHILD_PLACEMENT_TYPE
                    }
                },
            NodeTypeId = 36,
            Description = ""
        };
        yield return new Vocabulary {
            Id = null,
            Name = Constants.VOCABULARY_TYPE_OF_ABUSE,
            PublisherId = 1,
            CreatedDateTime = DateTime.Now,
            ChangedDateTime = DateTime.Now,
            Title = Constants.VOCABULARY_TYPE_OF_ABUSE,
            OwnerId = Constants.OWNER_CASES,
            TenantNodes = new List<TenantNode>
                {
                    new TenantNode
                    {
                        Id = null,
                        TenantId = 1,
                        PublicationStatusId = 1,
                        UrlPath = null,
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = Constants.TYPE_OF_ABUSE
                    }
                },
            NodeTypeId = 36,
            Description = ""
        };
        yield return new Vocabulary {
            Id = null,
            Name = Constants.VOCABULARY_TYPE_OF_ABUSER,
            PublisherId = 1,
            CreatedDateTime = DateTime.Now,
            ChangedDateTime = DateTime.Now,
            Title = Constants.VOCABULARY_TYPE_OF_ABUSER,
            OwnerId = Constants.OWNER_CASES,
            TenantNodes = new List<TenantNode>
                {
                    new TenantNode
                    {
                        Id = null,
                        TenantId = 1,
                        PublicationStatusId = 1,
                        UrlPath = null,
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = Constants.TYPE_OF_ABUSER
                    }
                },
            NodeTypeId = 36,
            Description = ""
        };
        yield return new Vocabulary {
            Id = null,
            Name = Constants.VOCABULARY_FAMILY_SIZE,
            PublisherId = 1,
            CreatedDateTime = DateTime.Now,
            ChangedDateTime = DateTime.Now,
            Title = Constants.VOCABULARY_FAMILY_SIZE,
            OwnerId = Constants.OWNER_CASES,
            TenantNodes = new List<TenantNode>
                {
                    new TenantNode
                    {
                        Id = null,
                        TenantId = 1,
                        PublicationStatusId = 1,
                        UrlPath = null,
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = Constants.FAMILY_SIZE
                    }
                },
            NodeTypeId = 36,
            Description = ""
        };
        yield return new Vocabulary {
            Id = null,
            Name = Constants.VOCABULARY_BILL_ACTION,
            PublisherId = 1,
            CreatedDateTime = DateTime.Now,
            ChangedDateTime = DateTime.Now,
            Title = Constants.VOCABULARY_BILL_ACTION,
            OwnerId = Constants.OWNER_PARTIES,
            TenantNodes = new List<TenantNode>
                {
                    new TenantNode
                    {
                        Id = null,
                        TenantId = 1,
                        PublicationStatusId = 1,
                        UrlPath = null,
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = Constants.BILL_ACTION
                    }
                },
            NodeTypeId = 36,
            Description = ""
        };
        yield return new Vocabulary {
            Id = null,
            Name = Constants.VOCABULARY_ORGANIZATION_ACT_RELATION_TYPE,
            PublisherId = 1,
            CreatedDateTime = DateTime.Now,
            ChangedDateTime = DateTime.Now,
            Title = Constants.VOCABULARY_ORGANIZATION_ACT_RELATION_TYPE,
            OwnerId = Constants.OWNER_PARTIES,
            TenantNodes = new List<TenantNode>
                {
                    new TenantNode
                    {
                        Id = null,
                        TenantId = 1,
                        PublicationStatusId = 1,
                        UrlPath = null,
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = Constants.ORGANIZATION_ACT_RELATION_TYPE
                    }
                },
            NodeTypeId = 36,
            Description = ""
        };
        yield return new Vocabulary {
            Id = null,
            Name = Constants.VOCABULARY_SUBDIVISION_TYPE,
            PublisherId = 1,
            CreatedDateTime = DateTime.Now,
            ChangedDateTime = DateTime.Now,
            Title = Constants.VOCABULARY_SUBDIVISION_TYPE,
            OwnerId = Constants.OWNER_GEOGRAPHY,
            TenantNodes = new List<TenantNode>
                {
                    new TenantNode
                    {
                        Id = null,
                        TenantId = 1,
                        PublicationStatusId = 1,
                        UrlPath = null,
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = Constants.SUBDIVISION_TYPE
                    }
                },
            NodeTypeId = 36,
            Description = ""
        };
        yield return new Vocabulary {
            Id = null,
            Name = Constants.VOCABULARY_UNITED_STATES_POLITICAL_PARTY_AFFILITION_TYPE,
            PublisherId = 1,
            CreatedDateTime = DateTime.Now,
            ChangedDateTime = DateTime.Now,
            Title = Constants.VOCABULARY_UNITED_STATES_POLITICAL_PARTY_AFFILITION_TYPE,
            OwnerId = Constants.OWNER_PARTIES,
            TenantNodes = new List<TenantNode>
                {
                    new TenantNode
                    {
                        Id = null,
                        TenantId = 1,
                        PublicationStatusId = 1,
                        UrlPath = null,
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = Constants.UNITED_STATES_POLITICAL_PARTY_AFFILITION_TYPE
                    }
                },
            NodeTypeId = 36,
            Description = ""
        };

        yield return new Vocabulary {
            Id = null,
            Name = Constants.VOCABULARY_CASE_PARTY_TYPE,
            PublisherId = 1,
            CreatedDateTime = DateTime.Now,
            ChangedDateTime = DateTime.Now,
            Title = Constants.VOCABULARY_CASE_PARTY_TYPE,
            OwnerId = Constants.OWNER_CASES,
            TenantNodes = new List<TenantNode>
                {
                    new TenantNode
                    {
                        Id = null,
                        TenantId = 1,
                        PublicationStatusId = 1,
                        UrlPath = null,
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = Constants.CASE_PARTY_TYPE
                    }
                },
            NodeTypeId = 36,
            Description = ""
        };
    }

    private static string GetVocabularyName(int id, string name)
    {
        return id switch {
            3797 => Constants.VOCABULARY_GEOGRAPHICAL_ENTITY,
            4126 => Constants.VOCABULARY_TOPICS,
            12622 => Constants.VOCABULARY_ORGANIZATION_TYPE,
            12637 => Constants.VOCABULARY_INTERORGANIZATIONAL_RELATION_TYPE,
            12652 => Constants.VOCABULARY_POLITICAL_ENTITY_RELATION_TYPE,
            12663 => Constants.VOCABULARY_PERSON_ORGANIZATION_RELATION_TYPE,
            16900 => Constants.VOCABULARY_INTERPERSONAL_RELATION_TYPE,
            27213 => Constants.VOCABULARY_PROFESSION,
            39428 => Constants.VOCABULARY_DENOMINATION,
            41212 => Constants.VOCABULARY_HAGUE_STATUS,
            42416 => Constants.VOCABULARY_DOCUMENT_TYPE,
            _ => name
        };
    }
    private static int GetOwner(int id)
    {
        return id switch {
            3797 => Constants.OWNER_GEOGRAPHY,
            12622 => Constants.OWNER_PARTIES,
            12637 => Constants.OWNER_PARTIES,
            12652 => Constants.OWNER_PARTIES,
            12663 => Constants.OWNER_PARTIES,
            16900 => Constants.OWNER_PARTIES,
            27213 => Constants.OWNER_PARTIES,
            39428 => Constants.OWNER_PARTIES,
            41212 => Constants.OWNER_PARTIES,
            42416 => Constants.OWNER_DOCUMENTATION,
            _ => Constants.PPL
        };
    }
    protected override async Task MigrateImpl()
    {
        await using var nodeIdReader = await _nodeIdReaderByUrlIdFactory.CreateAsync(_postgresConnection);
        await _vocabularyCreator.CreateAsync(GetVocabularies(), _postgresConnection);
        await _vocabularyCreator.CreateAsync(ReadVocabularies(), _postgresConnection);
        await using var tenantUpdater = await _tenantUpdaterSetTaggingVocabularyFactory.CreateAsync(_postgresConnection);
        await tenantUpdater.UpdateAsync(new TenantUpdaterSetTaggingVocabularyRequest {
            TenantId = Constants.PPL,
            VocabularyId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                TenantId = Constants.PPL,
                UrlId = 4126
            })
        });
    }
    private async IAsyncEnumerable<Vocabulary> ReadVocabularies()
    {

        var sql = $"""
            SELECT
                n.nid id,
                n.uid access_role_id,
                n.title,
                n.`status` node_status_id,
                FROM_UNIXTIME(n.created) created_date_time, 
                FROM_UNIXTIME(n.changed) changed_date_time,
                n.title `name`,
                nr.body description
            FROM node n
            JOIN node_revisions nr ON nr.nid = n.nid AND nr.vid = n.vid
            WHERE n.`type` = 'category_cont' AND n.nid not in (220, 12707, 42422)
            """;
        using var readCommand = _mySqlConnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;


        var reader = await readCommand.ExecuteReaderAsync();

        while (await reader.ReadAsync()) {
            var id = reader.GetInt32("id");
            var name = reader.GetString("name");
            yield return new Vocabulary {
                Id = null,
                PublisherId = reader.GetInt32("access_role_id"),
                CreatedDateTime = reader.GetDateTime("created_date_time"),
                ChangedDateTime = reader.GetDateTime("changed_date_time"),
                Title = GetVocabularyName(id, name),
                OwnerId = GetOwner(id),
                TenantNodes = new List<TenantNode>
                {
                    new TenantNode
                    {
                        Id = null,
                        TenantId = 1,
                        PublicationStatusId = reader.GetInt32("node_status_id"),
                        UrlPath = null,
                        NodeId = null,
                        SubgroupId = null,
                        UrlId = id
                    }
                },
                NodeTypeId = 38,
                Name = GetVocabularyName(id, name),
                Description = reader.GetString("description"),
            };

        }
        await reader.CloseAsync();
    }
}
