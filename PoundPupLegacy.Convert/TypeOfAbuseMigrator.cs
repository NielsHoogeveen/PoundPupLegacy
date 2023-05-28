namespace PoundPupLegacy.Convert;

internal sealed class TypeOfAbuseMigrator(
        IDatabaseConnections databaseConnections,
        IMandatorySingleItemDatabaseReaderFactory<NodeIdReaderByUrlIdRequest, int> nodeIdReaderFactory,
        IMandatorySingleItemDatabaseReaderFactory<TermIdReaderByNameRequest, int> termIdReaderFactory,
        IMandatorySingleItemDatabaseReaderFactory<FileIdReaderByTenantFileIdRequest, int> fileIdReaderByTenantFileIdFactory,
        IEntityCreatorFactory<EventuallyIdentifiableTypeOfAbuse> typeOfAbuseCreatorFactory
    ) : MigratorPPL(databaseConnections)
{

    protected override string Name => "types of abuse";

    protected override async Task MigrateImpl()
    {
        await using var fileIdReaderByTenantFileId = await fileIdReaderByTenantFileIdFactory.CreateAsync(_postgresConnection);
        await using var typeOfAbuseCreator = await typeOfAbuseCreatorFactory.CreateAsync(_postgresConnection);
        await using var nodeIdReader = await nodeIdReaderFactory.CreateAsync(_postgresConnection);
        await using var termIdReader = await termIdReaderFactory.CreateAsync(_postgresConnection);
        await typeOfAbuseCreator.CreateAsync(ReadTypesOfAbuse(nodeIdReader,termIdReader,fileIdReaderByTenantFileId));
    }
    private async IAsyncEnumerable<TypeOfAbuseToCreate> ReadTypesOfAbuse(
        IMandatorySingleItemDatabaseReader<NodeIdReaderByUrlIdRequest, int> nodeIdReader,
        IMandatorySingleItemDatabaseReader<TermIdReaderByNameRequest, int> termIdReader,
        IMandatorySingleItemDatabaseReader<FileIdReaderByTenantFileIdRequest, int> fileIdReaderByTenantFileId
    )
    {

        var sql = $"""
                SELECT
                t.id,
                1 access_role_id,
                NOW() created_date_time,
                NOW() changed_date_time,
                t.title,
                1 node_status_id,
                39 node_type_id,
                case 
                  when nr.body IS NULL then ''
                  ELSE nr.body
                	end description,
                t.topic_name,
                t.first_parent_topic_name,
                t.second_parent_topic_name,
                case 
                when cc.field_tile_image_fid = 0 then null
                ELSE cc.field_tile_image_fid 
                end file_id_tile_image,
                ua.dst url_path
                FROM(
                SELECT {Constants.NON_LETHAL_PHYSICAL_ABUSE} AS id, 'Non-lethal physical abuse' AS title, 'non-lethal physical abuse' AS topic_name, 'physical abuse' AS first_parent_topic_name, NULL AS second_parent_topic_name
                UNION
                SELECT {Constants.LETHAL_PHYSICAL_ABUSE}, 'Lethal physical abuse', 'lethal physical abuse', 'physical abuse', 'lethal abuse'
                UNION
                SELECT {Constants.PHYSICAL_EXPLOITATION}, 'Physical exploitation', 'physical exploitation', 'exploitation', null
                UNION
                SELECT {Constants.SEXUAL_ABUSE}, 'Sexual abuse', 'sexual abuse', 'Child abuse forms', null
                UNION
                SELECT {Constants.SEXUAL_EXPLOITATION}, 'Sexual exploitation', 'sexual exploitation', 'exploitation', 'sexual abuse'
                UNION
                SELECT {Constants.NON_LETHAL_NEGLECT}, 'Non-lethal neglect', 'non-lethal neglect' , 'neglect', null
                UNION
                SELECT {Constants.LETHAL_NEGLECT}, 'Lethal neglect', 'lethal neglect' , 'lethal abuse', 'neglect'
                UNION
                SELECT {Constants.NON_LETHAL_DEPRIVATION}, 'Non-lethal deprivation', 'non-lethal deprivation' , 'deprivation', null
                UNION
                SELECT {Constants.LETHAL_DEPRIVATION}, 'Lethal deprivation', 'lethal deprivation' , 'lethal abuse', 'deprivation'
                UNION
                SELECT {Constants.ECONOMIC_EXPLOITATION}, 'Economic exploitation', 'economic exploitation' , 'exploitation', null
                UNION
                SELECT {Constants.VERBAL_ABUSE}, 'Verbal abuse', 'verbal abuse' , 'emotional abuse', NULL
                UNION
                SELECT {Constants.MEDICAL_ABUSE}, 'Medical abuse', 'medical abuse' , 'Child abuse forms', NULL
                UNION
                SELECT {Constants.DEATH_BY_UNKNOWN_CAUSE}, 'Death by unknown cause', NULL, NULL , null
                ) AS t
                LEFT JOIN node n ON n.title = t.topic_name AND n.`type` = 'category_cat'
                LEFT JOIN url_alias ua ON cast(SUBSTRING(ua.src, 6) AS INT) = n.nid
                LEFT JOIN category c ON c.cid = n.nid AND c.cnid = 4126
                LEFT JOIN content_type_category_cat cc on cc.nid = n.nid AND cc.vid = n.vid
                LEFT JOIN node_revisions nr ON nr.nid = n.nid AND nr.vid = n.vid
                """;
        using var readCommand = _mySqlConnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;


        var reader = await readCommand.ExecuteReaderAsync();
        var vocabularyIdTypeOfAbuse = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
            TenantId = Constants.PPL,
            UrlId = Constants.VOCABULARY_ID_TYPE_OF_ABUSE
        });

        var vocabularyIdTopics = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
            TenantId = Constants.PPL,
            UrlId = Constants.VOCABULARY_ID_TOPICS
        });

        while (await reader.ReadAsync()) {
            var id = reader.GetInt32("id");
            var name = reader.GetString("title");
            var topicName = reader.IsDBNull("topic_name") ? null : reader.GetString("topic_name");
            var firstParentTopicName = reader.IsDBNull("first_parent_topic_name") ? null : reader.GetString("first_parent_topic_name");
            var secondParentTopicName = reader.IsDBNull("second_parent_topic_name") ? null : reader.GetString("second_parent_topic_name");

            var vocabularyNames = new List<NewTermForNewNameable>
            {
                new NewTermForNewNameable
                { 
                    VocabularyId = vocabularyIdTypeOfAbuse,
                    Name = name,
                    ParentTermIds = new List<int>(),
                }
            };
            if (topicName != null) {
                var lst = new List<int>();
                if (firstParentTopicName != null) {
                    lst.Add(await termIdReader.ReadAsync(new TermIdReaderByNameRequest {
                        Name = firstParentTopicName,
                        VocabularyId = vocabularyIdTopics
                    }));
                }
                if (secondParentTopicName != null) {
                    lst.Add(await termIdReader.ReadAsync(new TermIdReaderByNameRequest {
                        Name = secondParentTopicName,
                        VocabularyId = vocabularyIdTopics
                    }));
                }
                vocabularyNames.Add(new NewTermForNewNameable {
                    VocabularyId = vocabularyIdTopics,
                    Name = topicName,
                    ParentTermIds = lst
                });
            }

            yield return new TypeOfAbuseToCreate {
                Id = null,
                PublisherId = reader.GetInt32("access_role_id"),
                CreatedDateTime = reader.GetDateTime("created_date_time"),
                ChangedDateTime = reader.GetDateTime("changed_date_time"),
                Title = name,
                OwnerId = Constants.OWNER_CASES,
                AuthoringStatusId = 1,
                TenantNodes = new List<NewTenantNodeForNewNode>
                {
                    new NewTenantNodeForNewNode
                    {
                        Id = null,
                        TenantId = Constants.PPL,
                        PublicationStatusId = reader.GetInt32("node_status_id"),
                        UrlPath = reader.IsDBNull("url_path") ? null : reader.GetString("url_path"),
                        SubgroupId = null,
                        UrlId = id
                    },
                    new NewTenantNodeForNewNode
                    {
                        Id = null,
                        TenantId = Constants.CPCT,
                        PublicationStatusId = 2,
                        UrlPath = null,
                        SubgroupId = null,
                        UrlId = id < 33163 ? id : null
                    }
                },
                NodeTypeId = 39,
                Description = reader.GetString("description"),
                FileIdTileImage = reader.IsDBNull("file_id_tile_image")
                ? null
                : await fileIdReaderByTenantFileId.ReadAsync(new FileIdReaderByTenantFileIdRequest {
                    TenantId = Constants.PPL,
                    TenantFileId = reader.GetInt32("file_id_tile_image"),
                }),
                Terms = vocabularyNames,
                TermIds = new List<int>(),
            };

        }
        await reader.CloseAsync();
    }

}
