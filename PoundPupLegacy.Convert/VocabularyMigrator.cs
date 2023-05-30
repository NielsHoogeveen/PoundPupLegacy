namespace PoundPupLegacy.Convert;

internal sealed class VocabularyMigrator(
    IDatabaseConnections databaseConnections,
    IMandatorySingleItemDatabaseReaderFactory<NodeIdReaderByUrlIdRequest, int> nodeIdReaderByUrlIdFactory,
    IEntityCreatorFactory<Vocabulary.ToCreate> vocabularyCreatorFactory
) : MigratorPPL(databaseConnections)
{
    protected override string Name => "vocabularies";
    private static async IAsyncEnumerable<Vocabulary.ToCreate> GetVocabularies()
    {
        await Task.CompletedTask;
        yield return new Vocabulary.ToCreate {
            Identification = new Identification.Possible {
                Id = null
            },
            NodeDetails = new NodeDetails.NodeDetailsForCreate {
                PublisherId = 1,
                CreatedDateTime = DateTime.Now,
                ChangedDateTime = DateTime.Now,
                Title = Constants.VOCABULARY_CHILD_PLACEMENT_TYPE,
                OwnerId = Constants.OWNER_CASES,
                AuthoringStatusId = 1,
                TenantNodes = new List<TenantNode.ToCreateForNewNode>
                {
                    new TenantNode.ToCreateForNewNode
                    {
                        Identification = new Identification.Possible {
                            Id = null
                        },
                        TenantId = 1,
                        PublicationStatusId = 1,
                        UrlPath = null,
                        SubgroupId = null,
                        UrlId = Constants.VOCABULARY_ID_CHILD_PLACEMENT_TYPE
                    }
                },
                NodeTypeId = 38,
                TermIds = new List<int>(),
            },
            VocabularyDetails = new VocabularyDetails {
                Description = "",
                Name = Constants.VOCABULARY_CHILD_PLACEMENT_TYPE,
                OwnerId = Constants.OWNER_CASES,
            },
        };
        yield return new Vocabulary.ToCreate {
            Identification = new Identification.Possible {
                Id = null
            },
            NodeDetails = new NodeDetails.NodeDetailsForCreate {
                PublisherId = 1,
                CreatedDateTime = DateTime.Now,
                ChangedDateTime = DateTime.Now,
                Title = Constants.VOCABULARY_TYPE_OF_ABUSE,
                OwnerId = Constants.OWNER_CASES,
                AuthoringStatusId = 1,
                TenantNodes = new List<TenantNode.ToCreateForNewNode>
                {
                    new TenantNode.ToCreateForNewNode
                    {
                        Identification = new Identification.Possible {
                            Id = null
                        },
                        TenantId = 1,
                        PublicationStatusId = 1,
                        UrlPath = null,
                        SubgroupId = null,
                        UrlId = Constants.VOCABULARY_ID_TYPE_OF_ABUSE
                    }
                },
                NodeTypeId = 38,
                TermIds = new List<int>(),
            },
            VocabularyDetails = new VocabularyDetails {
                Description = "",
                Name = Constants.VOCABULARY_TYPE_OF_ABUSE,
                OwnerId = Constants.OWNER_CASES,
            },
        };
        yield return new Vocabulary.ToCreate {
            Identification = new Identification.Possible {
                Id = null
            },
            NodeDetails = new NodeDetails.NodeDetailsForCreate {
                PublisherId = 1,
                CreatedDateTime = DateTime.Now,
                ChangedDateTime = DateTime.Now,
                Title = Constants.VOCABULARY_TYPE_OF_ABUSER,
                OwnerId = Constants.OWNER_CASES,
                AuthoringStatusId = 1,
                TenantNodes = new List<TenantNode.ToCreateForNewNode>
                {
                    new TenantNode.ToCreateForNewNode
                    {
                        Identification = new Identification.Possible {
                            Id = null
                        },
                        TenantId = 1,
                        PublicationStatusId = 1,
                        UrlPath = null,
                        SubgroupId = null,
                        UrlId = Constants.VOCABULARY_ID_TYPE_OF_ABUSER
                    }
                },
                NodeTypeId = 38,
                TermIds = new List<int>(),
            },
            VocabularyDetails = new VocabularyDetails {
                Description = "",
                Name = Constants.VOCABULARY_TYPE_OF_ABUSER,
                OwnerId = Constants.OWNER_CASES,
            },
        };
        yield return new Vocabulary.ToCreate {
            Identification = new Identification.Possible {
                Id = null
            },
            NodeDetails = new NodeDetails.NodeDetailsForCreate {
                PublisherId = 1,
                CreatedDateTime = DateTime.Now,
                ChangedDateTime = DateTime.Now,
                Title = Constants.VOCABULARY_FAMILY_SIZE,
                OwnerId = Constants.OWNER_CASES,
                AuthoringStatusId = 1,
                TenantNodes = new List<TenantNode.ToCreateForNewNode>
                {
                    new TenantNode.ToCreateForNewNode
                    {
                        Identification = new Identification.Possible {
                            Id = null
                        },
                        TenantId = 1,
                        PublicationStatusId = 1,
                        UrlPath = null,
                        SubgroupId = null,
                        UrlId = Constants.VOCABULARY_ID_FAMILY_SIZE
                    }
                },
                NodeTypeId = 38,
                TermIds = new List<int>(),
            },
            VocabularyDetails = new VocabularyDetails {
                Description = "",
                Name = Constants.VOCABULARY_FAMILY_SIZE,
                OwnerId = Constants.OWNER_CASES,
            },
        };
        yield return new Vocabulary.ToCreate {
            Identification = new Identification.Possible {
                Id = null
            },
            NodeDetails = new NodeDetails.NodeDetailsForCreate {
                PublisherId = 1,
                CreatedDateTime = DateTime.Now,
                ChangedDateTime = DateTime.Now,
                Title = Constants.VOCABULARY_BILL_ACTION,
                OwnerId = Constants.OWNER_PARTIES,
                AuthoringStatusId = 1,
                TenantNodes = new List<TenantNode.ToCreateForNewNode>
                {
                    new TenantNode.ToCreateForNewNode
                    {
                        Identification = new Identification.Possible {
                            Id = null
                        },
                        TenantId = 1,
                        PublicationStatusId = 1,
                        UrlPath = null,
                        SubgroupId = null,
                        UrlId = Constants.VOCABULARY_ID_BILL_ACTION
                    }
                },
                NodeTypeId = 38,
                TermIds = new List<int>(),
            },
            VocabularyDetails = new VocabularyDetails {
                Description = "",
                Name = Constants.VOCABULARY_BILL_ACTION,
                OwnerId = Constants.OWNER_PARTIES,
            },
        };
        yield return new Vocabulary.ToCreate {
            Identification = new Identification.Possible {
                Id = null
            },
            NodeDetails = new NodeDetails.NodeDetailsForCreate {
                PublisherId = 1,
                CreatedDateTime = DateTime.Now,
                ChangedDateTime = DateTime.Now,
                Title = Constants.VOCABULARY_ORGANIZATION_ACT_RELATION_TYPE,
                OwnerId = Constants.OWNER_PARTIES,
                AuthoringStatusId = 1,
                TenantNodes = new List<TenantNode.ToCreateForNewNode>
                {
                    new TenantNode.ToCreateForNewNode
                    {
                        Identification = new Identification.Possible {
                            Id = null
                        },
                        TenantId = 1,
                        PublicationStatusId = 1,
                        UrlPath = null,
                        SubgroupId = null,
                        UrlId = Constants.VOCABULARY_ID_ORGANIZATION_ACT_RELATION_TYPE
                    }
                },
                NodeTypeId = 38,
                TermIds = new List<int>(),
            },
            VocabularyDetails = new VocabularyDetails {
                Description = "",
                Name = Constants.VOCABULARY_ORGANIZATION_ACT_RELATION_TYPE,
                OwnerId = Constants.OWNER_PARTIES,
            },
        };
        yield return new Vocabulary.ToCreate {
            Identification = new Identification.Possible {
                Id = null
            },
            NodeDetails = new NodeDetails.NodeDetailsForCreate {
                PublisherId = 1,
                CreatedDateTime = DateTime.Now,
                ChangedDateTime = DateTime.Now,
                Title = Constants.VOCABULARY_SUBDIVISION_TYPE,
                OwnerId = Constants.OWNER_GEOGRAPHY,
                AuthoringStatusId = 1,
                TenantNodes = new List<TenantNode.ToCreateForNewNode>
                {
                    new TenantNode.ToCreateForNewNode
                    {
                        Identification = new Identification.Possible {
                            Id = null
                        },
                        TenantId = 1,
                        PublicationStatusId = 1,
                        UrlPath = null,
                        SubgroupId = null,
                        UrlId = Constants.VOCABULARY_ID_SUBDIVISION_TYPE
                    }
                },
                NodeTypeId = 38,
                TermIds = new List<int>(),
            },
            VocabularyDetails = new VocabularyDetails {
                Description = "",
                Name = Constants.VOCABULARY_SUBDIVISION_TYPE,
                OwnerId = Constants.OWNER_GEOGRAPHY,
            },
        };
        yield return new Vocabulary.ToCreate {
            Identification = new Identification.Possible {
                Id = null
            },
            NodeDetails = new NodeDetails.NodeDetailsForCreate {
                PublisherId = 1,
                CreatedDateTime = DateTime.Now,
                ChangedDateTime = DateTime.Now,
                Title = Constants.VOCABULARY_UNITED_STATES_POLITICAL_PARTY_AFFILITION_TYPE,
                OwnerId = Constants.OWNER_PARTIES,
                AuthoringStatusId = 1,
                TenantNodes = new List<TenantNode.ToCreateForNewNode>
                {
                    new TenantNode.ToCreateForNewNode
                    {
                        Identification = new Identification.Possible {
                            Id = null
                        },
                        TenantId = 1,
                        PublicationStatusId = 1,
                        UrlPath = null,
                        SubgroupId = null,
                        UrlId = Constants.VOCABULARY_ID_UNITED_STATES_POLITICAL_PARTY_AFFILITION_TYPE
                    }
                },
                NodeTypeId = 38,
                TermIds = new List<int>(),
            },
            VocabularyDetails = new VocabularyDetails {
                Description = "",
                Name = Constants.VOCABULARY_UNITED_STATES_POLITICAL_PARTY_AFFILITION_TYPE,
                OwnerId = Constants.OWNER_PARTIES,
            },
        };
        yield return new Vocabulary.ToCreate {
            Identification = new Identification.Possible {
                Id = null
            },
            NodeDetails = new NodeDetails.NodeDetailsForCreate {
                PublisherId = 1,
                CreatedDateTime = DateTime.Now,
                ChangedDateTime = DateTime.Now,
                Title = Constants.VOCABULARY_CASE_PARTY_TYPE,
                OwnerId = Constants.OWNER_CASES,
                AuthoringStatusId = 1,
                TenantNodes = new List<TenantNode.ToCreateForNewNode>
                {
                    new TenantNode.ToCreateForNewNode
                    {
                        Identification = new Identification.Possible {
                            Id = null
                        },
                        TenantId = 1,
                        PublicationStatusId = 1,
                        UrlPath = null,
                        SubgroupId = null,
                        UrlId = Constants.VOCABULARY_ID_CASE_PARTY_TYPE
                    }
                },
                NodeTypeId = 38,
                TermIds = new List<int>(),
            },
            VocabularyDetails = new VocabularyDetails {
                Description = "",
                Name = Constants.VOCABULARY_CASE_PARTY_TYPE,
                OwnerId = Constants.OWNER_CASES,
            },
        };
    }

    private static string GetVocabularyName(int id, string name)
    {
        return id switch {
            Constants.VOCABULARY_ID_TOPICS => Constants.VOCABULARY_TOPICS,
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
            Constants.VOCABULARY_ID_TOPICS => Constants.OWNER_SYSTEM,
            3797 => Constants.OWNER_GEOGRAPHY,
            12622 => Constants.OWNER_PARTIES,
            12637 => Constants.OWNER_PARTIES,
            12652 => Constants.OWNER_PARTIES,
            12663 => Constants.OWNER_PARTIES,
            16900 => Constants.OWNER_PARTIES,
            23399 => Constants.OWNER_GEOGRAPHY,
            27213 => Constants.OWNER_PARTIES,
            39428 => Constants.OWNER_PARTIES,
            41212 => Constants.OWNER_PARTIES,
            42416 => Constants.OWNER_DOCUMENTATION,
            _ => Constants.PPL
        };
    }
    protected override async Task MigrateImpl()
    {
        await using var nodeIdReader = await nodeIdReaderByUrlIdFactory.CreateAsync(_postgresConnection);
        await using var vocabularyCreator = await vocabularyCreatorFactory.CreateAsync(_postgresConnection);
        await vocabularyCreator.CreateAsync(GetVocabularies());
        await vocabularyCreator.CreateAsync(ReadVocabularies());
    }
    private async IAsyncEnumerable<Vocabulary.ToCreate> ReadVocabularies()
    {

        var sql = $"""
            SELECT
                n.nid id,
                n.uid access_role_id,
                case 
                    when n.nid = 3797 then 'Geography'
                    else n.title
                end title,
                n.`status` node_status_id,
                FROM_UNIXTIME(n.created) created_date_time, 
                FROM_UNIXTIME(n.changed) changed_date_time,
                n.title `name`,
                nr.body description
            FROM node n
            JOIN node_revisions nr ON nr.nid = n.nid AND nr.vid = n.vid
            WHERE n.`type` = 'category_cont' AND n.nid not in (220, 12707, 42422, 23399, 3797, 4126)
            """;
        using var readCommand = _mySqlConnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;


        var reader = await readCommand.ExecuteReaderAsync();

        while (await reader.ReadAsync()) {
            var id = reader.GetInt32("id");
            var name = reader.GetString("name");
            yield return new Vocabulary.ToCreate {
                Identification = new Identification.Possible {
                    Id = null
                },
                NodeDetails = new NodeDetails.NodeDetailsForCreate {
                    PublisherId = reader.GetInt32("access_role_id"),
                    CreatedDateTime = reader.GetDateTime("created_date_time"),
                    ChangedDateTime = reader.GetDateTime("changed_date_time"),
                    Title = GetVocabularyName(id, name),
                    OwnerId = GetOwner(id),
                    AuthoringStatusId = 1,
                    TenantNodes = new List<TenantNode.ToCreateForNewNode>
                    {
                        new TenantNode.ToCreateForNewNode
                        {
                            Identification = new Identification.Possible {
                                Id = null
                            },
                            TenantId = 1,
                            PublicationStatusId = reader.GetInt32("node_status_id"),
                            UrlPath = null,
                            SubgroupId = null,
                            UrlId = id
                        }
                    },
                    NodeTypeId = 38,
                    TermIds = new List<int>(),
                },
                VocabularyDetails = new VocabularyDetails {
                    Name = GetVocabularyName(id, name),
                    Description = reader.GetString("description"),
                    OwnerId = GetOwner(id),
                },
            };
        }
        await reader.CloseAsync();
    }
}
