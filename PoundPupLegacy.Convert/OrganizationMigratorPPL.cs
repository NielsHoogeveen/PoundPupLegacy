using System.Collections.Immutable;

namespace PoundPupLegacy.Convert;

internal sealed class OrganizationMigratorPPL(
    IDatabaseConnections databaseConnections,
    IMandatorySingleItemDatabaseReaderFactory<NodeIdReaderByUrlIdRequest, int> nodeIdReaderByUrlIdFactory,
    IMandatorySingleItemDatabaseReaderFactory<TermIdReaderByNameRequest, int> termIdReaderFactory,
    IMandatorySingleItemDatabaseReaderFactory<TermNameReaderByNameableIdRequest, string> termReaderByNameableIdFactory,
    IEntityCreatorFactory<EventuallyIdentifiableOrganization> organizationCreatorFactory
) : MigratorPPL(databaseConnections)
{
    protected override string Name => "organizations (ppl)";

    private async IAsyncEnumerable<EventuallyIdentifiableOrganization> GetOrganizations(
        IMandatorySingleItemDatabaseReader<NodeIdReaderByUrlIdRequest, int> nodeIdReader,
        IMandatorySingleItemDatabaseReader<TermIdReaderByNameRequest, int> termIdReader
    )
    {
        var vocabularyId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
            TenantId = Constants.PPL,
            UrlId = Constants.VOCABULARY_ID_TOPICS
        });

        yield return new NewBasicOrganization {
            Id = null,
            PublisherId = 1,
            CreatedDateTime = DateTime.Now,
            ChangedDateTime = DateTime.Now,
            Title = "Colorado Adoption Center",
            OwnerId = Constants.OWNER_PARTIES,
            AuthoringStatusId = 1,
            TenantNodes = new List<NewTenantNodeForNewNode>
            {
                new NewTenantNodeForNewNode
                {
                    Id = null,
                    TenantId = Constants.PPL,
                    PublicationStatusId = 1,
                    UrlPath = null,
                    SubgroupId = null,
                    UrlId = Constants.COLORADO_ADOPTION_CENTER
                },
                new NewTenantNodeForNewNode
                {
                    Id = null,
                    TenantId = Constants.CPCT,
                    PublicationStatusId = 2,
                    UrlPath = null,
                    SubgroupId = null,
                    UrlId = Constants.COLORADO_ADOPTION_CENTER
                }
            },
            NodeTypeId = 23,
            WebsiteUrl = null,
            EmailAddress = null,
            Established = null,
            Terminated = null,
            Description = "",
            FileIdTileImage = null,
            Terms = new List<NewTermForNewNameble> {
                new NewTermForNewNameble {
                    VocabularyId = vocabularyId,
                    Name = "Colorado Adoption Center",
                    ParentTermIds = new List<int>{
                        await termIdReader.ReadAsync(new TermIdReaderByNameRequest {
                            Name = "adoption agencies",
                            VocabularyId = vocabularyId
                        })
                    },
                }
            },
            OrganizationTypeIds = new List<int>
            {
                await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest
                {
                    TenantId = Constants.PPL,
                    UrlId = 12625
                })
            },
            NodeTermIds = new List<int>(),
            NewLocations = new List<EventuallyIdentifiableLocation>(),
            PartyPoliticalEntityRelations = new List<EventuallyIdentifiablePartyPoliticalEntityRelationForNewParty>(),
            PersonOrganizationRelations = new List<EventuallyIdentifiablePersonOrganizationRelationForNewOrganization>(),
            InterOrganizationalRelationsToAddFrom = new List<EventuallyIdentifiableInterOrganizationalRelationForNewOrganizationFrom>(),
            InterOrganizationalRelationsToAddTo = new List<EventuallyIdentifiableInterOrganizationalRelationForNewOrganizationTo>(),
        };
        yield return new NewUnitedStatesPoliticalParty {
            Id = null,
            PublisherId = 1,
            CreatedDateTime = DateTime.Now,
            ChangedDateTime = DateTime.Now,
            Title = "Popular Democratic Party",
            OwnerId = Constants.OWNER_PARTIES,
            AuthoringStatusId = 1,
            TenantNodes = new List<NewTenantNodeForNewNode>
            {
                new NewTenantNodeForNewNode
                {
                    Id = null,
                    TenantId = Constants.PPL,
                    PublicationStatusId = 1,
                    UrlPath = null,
                    SubgroupId = null,
                    UrlId = Constants.POPULAR_DEMOCRAT_PARTY
                },
                new NewTenantNodeForNewNode
                {
                    Id = null,
                    TenantId = Constants.CPCT,
                    PublicationStatusId = 2,
                    UrlPath = null,
                    SubgroupId = null,
                    UrlId = Constants.POPULAR_DEMOCRAT_PARTY
                }
            },
            NodeTypeId = 63,
            WebsiteUrl = null,
            EmailAddress = null,
            Established = null,
            Terminated = null,
            Description = "",
            FileIdTileImage = null,
            Terms = new List<NewTermForNewNameble> {
                new NewTermForNewNameble {
                    VocabularyId = vocabularyId,
                    Name = "Popular Democratic Party",
                    ParentTermIds = new List<int>{
                        await termIdReader.ReadAsync(new TermIdReaderByNameRequest {
                            Name = "political party" ,
                            VocabularyId = vocabularyId
                        })
                    },
                }
            },
            OrganizationTypeIds = new List<int>
            {
                await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest
                {
                    TenantId = Constants.PPL,
                    UrlId = Constants.POLITICAL_PARTY
                })
            },
            NodeTermIds = new List<int>(),
            NewLocations = new List<EventuallyIdentifiableLocation>(),
            PartyPoliticalEntityRelations = new List<EventuallyIdentifiablePartyPoliticalEntityRelationForNewParty>(),
            PersonOrganizationRelations = new List<EventuallyIdentifiablePersonOrganizationRelationForNewOrganization>(),
            InterOrganizationalRelationsToAddFrom = new List<EventuallyIdentifiableInterOrganizationalRelationForNewOrganizationFrom>(),
            InterOrganizationalRelationsToAddTo = new List<EventuallyIdentifiableInterOrganizationalRelationForNewOrganizationTo>(),

        };
        yield return new NewUnitedStatesPoliticalParty {
            Id = null,
            PublisherId = 1,
            CreatedDateTime = DateTime.Now,
            ChangedDateTime = DateTime.Now,
            Title = "Libertarian Party",
            OwnerId = Constants.OWNER_PARTIES,
            AuthoringStatusId = 1,
            TenantNodes = new List<NewTenantNodeForNewNode>
            {
                new NewTenantNodeForNewNode
                {
                    Id = null,
                    TenantId = Constants.PPL,
                    PublicationStatusId = 1,
                    UrlPath = null,
                    SubgroupId = null,
                    UrlId = Constants.LIBERTARIAN_PARTY
                },
                new NewTenantNodeForNewNode
                {
                    Id = null,
                    TenantId = Constants.CPCT,
                    PublicationStatusId = 2,
                    UrlPath = null,
                    SubgroupId = null,
                    UrlId = Constants.LIBERTARIAN_PARTY
                }
            },
            NodeTypeId = 63,
            WebsiteUrl = null,
            EmailAddress = null,
            Established = null,
            Terminated = null,
            Description = "",
            FileIdTileImage = null,
            Terms = new List<NewTermForNewNameble> {
                new NewTermForNewNameble {
                    VocabularyId = vocabularyId,
                    Name = "Libertarian Party",
                    ParentTermIds = new List<int>{
                        await termIdReader.ReadAsync(new TermIdReaderByNameRequest {
                            Name = "political party" ,
                            VocabularyId = vocabularyId
                        }) 
                    },
                }
            },
            OrganizationTypeIds = new List<int>
            {
                await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest
                {
                    TenantId = Constants.PPL,
                    UrlId = Constants.POLITICAL_PARTY
                })
            },
            NodeTermIds = new List<int>(),
            NewLocations = new List<EventuallyIdentifiableLocation>(),
            PartyPoliticalEntityRelations = new List<EventuallyIdentifiablePartyPoliticalEntityRelationForNewParty>(),
            PersonOrganizationRelations = new List<EventuallyIdentifiablePersonOrganizationRelationForNewOrganization>(),
            InterOrganizationalRelationsToAddFrom = new List<EventuallyIdentifiableInterOrganizationalRelationForNewOrganizationFrom>(),
            InterOrganizationalRelationsToAddTo = new List<EventuallyIdentifiableInterOrganizationalRelationForNewOrganizationTo>(),
        };
        yield return new NewBasicOrganization {
            Id = null,
            PublisherId = 1,
            CreatedDateTime = DateTime.Now,
            ChangedDateTime = DateTime.Now,
            Title = "Government of Italy",
            OwnerId = Constants.OWNER_PARTIES,
            AuthoringStatusId = 1,
            TenantNodes = new List<NewTenantNodeForNewNode>
            {
                new NewTenantNodeForNewNode
                {
                    Id = null,
                    TenantId = Constants.PPL,
                    PublicationStatusId = 1,
                    UrlPath = null,
                    SubgroupId = null,
                    UrlId = 17036
                },
                new NewTenantNodeForNewNode
                {
                    Id = null,
                    TenantId = Constants.CPCT,
                    PublicationStatusId = 2,
                    UrlPath = null,
                    SubgroupId = null,
                    UrlId = 17036
                }
            },
            NodeTypeId = 23,
            WebsiteUrl = null,
            EmailAddress = null,
            Established = null,
            Terminated = null,
            Description = "",
            FileIdTileImage = null,
            Terms = new List<NewTermForNewNameble> {
                new NewTermForNewNameble {
                    VocabularyId = vocabularyId,
                    Name = "Government of Italy",
                    ParentTermIds = new List<int>{
                        await termIdReader.ReadAsync(new TermIdReaderByNameRequest {
                            Name = "governmental organization" ,
                            VocabularyId = vocabularyId
                        })
                    },
                }
            },
            OrganizationTypeIds = new List<int>
            {
                await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest
                {
                    TenantId = Constants.PPL,
                    UrlId = 12630
                })
            },
            NodeTermIds = new List<int>(),
            NewLocations = new List<EventuallyIdentifiableLocation>(),
            PartyPoliticalEntityRelations = new List<EventuallyIdentifiablePartyPoliticalEntityRelationForNewParty>(),
            PersonOrganizationRelations = new List<EventuallyIdentifiablePersonOrganizationRelationForNewOrganization>(),
            InterOrganizationalRelationsToAddFrom = new List<EventuallyIdentifiableInterOrganizationalRelationForNewOrganizationFrom>(),
            InterOrganizationalRelationsToAddTo = new List<EventuallyIdentifiableInterOrganizationalRelationForNewOrganizationTo>(),
        };
    }

    protected override async Task MigrateImpl()
    {
        await using var nodeIdReader = await nodeIdReaderByUrlIdFactory.CreateAsync(_postgresConnection);
        await using var termIdReader = await termIdReaderFactory.CreateAsync(_postgresConnection);
        await using var termReaderByNameableId = await termReaderByNameableIdFactory.CreateAsync(_postgresConnection);
        await using var organizationCreator = await organizationCreatorFactory.CreateAsync(_postgresConnection);
        await organizationCreator.CreateAsync(GetOrganizations(nodeIdReader,termIdReader));
        await organizationCreator.CreateAsync(ReadOrganizations(nodeIdReader, termIdReader, termReaderByNameableId));
    }
    private async IAsyncEnumerable<EventuallyIdentifiableOrganization> ReadOrganizations(
        IMandatorySingleItemDatabaseReader<NodeIdReaderByUrlIdRequest, int> nodeIdReader,
        IMandatorySingleItemDatabaseReader<TermIdReaderByNameRequest, int> termIdReader,
        IMandatorySingleItemDatabaseReader<TermNameReaderByNameableIdRequest, string> termReaderByNameableId
    )
    {

        var sql = $"""
            SELECT
                n.nid id,
                n.uid access_role_id,
                case 
                    when n.nid = 8315 then 'Compassionate Hearts (PA)'
                    when n.nid = 46082 then 'Compassionate Hearts (MT)'
                    else n.title
                end title,
                n.`status` node_status_id,
                FROM_UNIXTIME(n.created) created_date_time, 
                FROM_UNIXTIME(n.changed) changed_date_time,
                23 node_type_id,
                o.field_website_2_url website_url,
                FROM_UNIXTIME(o.field_start_date_0_value) established, 
                FROM_UNIXTIME(UNIX_TIMESTAMP(o.field_end_date_value)) `terminated`,
                o.field_email_address_email email_address,
                o.field_description_3_value description,
                case 
                    when c.title IS NOT NULL then c.title
            		when c2.title IS NOT NULL then c2.title
                    when n.nid = 8315 then 'Compassionate Hearts (PA)'
                    when n.nid = 46082 then 'Compassionate Hearts (MT)'
                    ELSE n.title
                END topic_name,
                case 
                    when c.topic_parent_names IS NOT NULL then c.topic_parent_names
                    ELSE c2.topic_parent_names
                END topic_parent_names,
                ua.dst url_path,
                	group_concat(nots.nid SEPARATOR ',') organization_types
            FROM node n 
            LEFT JOIN url_alias ua ON cast(SUBSTRING(ua.src, 6) AS INT) = n.nid
            LEFT JOIN category_node cna ON cna.nid = n.nid AND cna.cid = 38518
            LEFT JOIN category_node cnb ON cnb.nid = n.nid AND cnb.cid = 38308
            JOIN content_type_adopt_orgs o ON o.nid = n.nid AND o.vid = n.vid
            JOIN category_node cnot ON cnot.nid = n.nid
            JOIN category `cot` ON `cot`.cid = cnot.cid AND `cot`.cnid = 12622
            JOIN node nots ON nots.nid = `cot`.cid
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
                GROUP BY 
                    n.nid,
                    n.title,
                    cc.field_tile_image_title,
                    cc.field_related_page_nid
            ) c ON c.field_related_page_nid = n.nid
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
            ) c2 ON c2.title = n.title
                	WHERE n.`type` = 'adopt_orgs'
            AND n.nid NOT IN (11108, 7760, 12700, 30638)
            AND cna.nid IS NULL
            AND cnb.nid IS NULL
            GROUP BY
                n.nid,
                n.uid,
                n.title,
                n.`status`,
                n.created, 
                n.`changed`,
                o.field_website_2_url,
                o.field_start_date_0_value, 
                o.field_end_date_value,
                o.field_email_address_email,
                o.field_description_3_value,
                c.title,
                c2.title,
                	c.topic_parent_names ,
                	c2.topic_parent_names,
                ua.dst
            """;
        using var readCommand = _mySqlConnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;


        var reader = await readCommand.ExecuteReaderAsync();
        var vocabularyId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
            TenantId = Constants.PPL,
            UrlId = Constants.VOCABULARY_ID_TOPICS
        });

        while (await reader.ReadAsync()) {
            var vocabularyNames = new List<NewTermForNewNameble>();


            var typeIds = reader
                .GetString("organization_types")
                .Split(',')
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(x => int.Parse(x));
            var organizationOrganizationTypeIds = new List<int>();
            foreach (var typeId in typeIds) {
                var organizationTypeId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                    UrlId = typeId,
                    TenantId = Constants.PPL
                });
                organizationOrganizationTypeIds.Add(organizationTypeId);
            }

            async IAsyncEnumerable<string> GetTermNamesForOrganizationsTypes(IEnumerable<int> organizationTypeIds)
            {
                foreach (var organizationTypeId in organizationTypeIds) {
                    var res = await termReaderByNameableId.ReadAsync(new TermNameReaderByNameableIdRequest {
                        NameableId = organizationTypeId,
                        VocabularyId = vocabularyId
                    });
                    yield return res;
                }
            }

            var id = reader.GetInt32("id");

            var name = reader.IsDBNull("topic_name")
                ? reader.GetString("title")
                : reader.GetString("topic_name");

            var topicName = reader.GetString("topic_name");
            var organizationTypeTermNames = await GetTermNamesForOrganizationsTypes(organizationOrganizationTypeIds).ToListAsync();
            var topicParentNames = reader.IsDBNull("topic_parent_names")
                ? organizationTypeTermNames
                : reader.GetString("topic_parent_names")
                    .Split(',')
                    .Where(x => !string.IsNullOrEmpty(x))
                    .Where(x => x != "European Union")
                    .Select(x => x.Replace("Colorado", "Colorado (state of the USA)"))
                    .Select(x => x.Replace("New York", "New York (state of the USA)"))
                    .Select(x => x.Replace("Illinois", "Illinois (state of the USA)"))
                    .Select(x => x.Replace("Texas", "Texas (state of the USA)"))
                    .Select(x => x.Replace("Utah", "Texas (state of the USA)"))
                    .Select(x => x.Replace("Arizona", "Arizona (state of the USA)"))
                    .Select(x => x.Replace("Connecticut", "Connecticut (state of the USA)"))
                    .Select(x => x.Replace("District of Columbia", "District of Columbia (state of the USA)"))
                    .Select(x => x.Replace("Florida", "Florida (state of the USA)"))
                    .Select(x => x.Replace("Georgia (state)", "Georgia (state of the USA)"))
                    .Select(x => x.Replace("Kansas", "Kansas (state of the USA)"))
                    .Select(x => x.Replace("Kentucky", "Kentucky (state of the USA)"))
                    .Select(x => x.Replace("Maine", "Maine (state of the USA)"))
                    .Select(x => x.Replace("Michigan", "Michigan (state of the USA)"))
                    .Select(x => x.Replace("Mississippi", "Mississippi (state of the USA)"))
                    .Select(x => x.Replace("Nebraska", "Nebraska (state of the USA)"))
                    .Select(x => x.Replace("New Jersey", "New Jersey (state of the USA)"))
                    .Select(x => x.Replace("Oklahoma", "Oklahoma (state of the USA)"))
                    .Select(x => x.Replace("Oregon", "Oregon (state of the USA)"))
                    .Select(x => x.Replace("South Carolina", "South Carolina (state of the USA)"))
                    .Select(x => x.Replace("Tennessee", "Tennessee (state of the USA)"))
                    .Select(x => x.Replace("Washington", "Washington (state of the USA)"))
                    .Select(x => x.Replace("Wisconsin", "Wisconsin (state of the USA)"))
                    .Select(x => x.Replace("Missouri", "Missouri (state of the USA)"))
                    .ToImmutableList()
                    .AddRange(organizationTypeTermNames)
                    .Distinct()
                    .ToList();
            List<int> topicParentIds = new List<int>();
            foreach (var topicParentName in topicParentNames) {
                topicParentIds.Add(await termIdReader.ReadAsync(new TermIdReaderByNameRequest {
                    Name = topicParentName,
                    VocabularyId = vocabularyId
                }));
            }
            vocabularyNames.Add(new NewTermForNewNameble {
                VocabularyId = vocabularyId,
                Name = topicName,
                ParentTermIds = topicParentIds,
            });

            if (id == Constants.DEMOCRATIC_PARTY || id == Constants.REPUBLICAN_PARTY) {
                yield return new NewUnitedStatesPoliticalParty {
                    Id = null,
                    PublisherId = reader.GetInt32("access_role_id"),
                    CreatedDateTime = reader.GetDateTime("created_date_time"),
                    ChangedDateTime = reader.GetDateTime("changed_date_time"),
                    Title = reader.GetString("title"),
                    OwnerId = Constants.OWNER_PARTIES,
                    AuthoringStatusId = 1,
                    TenantNodes = new List<NewTenantNodeForNewNode> {
                        new NewTenantNodeForNewNode {
                            Id = null,
                            TenantId = Constants.PPL,
                            PublicationStatusId = reader.GetInt32("node_status_id"),
                            UrlPath = reader.IsDBNull("url_path") ? null : reader.GetString("url_path"),
                            SubgroupId = null,
                            UrlId = id
                        },
                        new NewTenantNodeForNewNode {
                            Id = null,
                            TenantId = Constants.CPCT,
                            PublicationStatusId = 2,
                            UrlPath = null,
                            SubgroupId = null,
                            UrlId = id < 33163 ? id : null
                        }
                    },
                    NodeTypeId = 63,
                    WebsiteUrl = reader.IsDBNull("website_url") ? null : reader.GetString("website_url"),
                    EmailAddress = reader.IsDBNull("email_address") ? null : reader.GetString("email_address"),
                    Description = reader.IsDBNull("description") ? "" : reader.GetString("description"),
                    Established = reader.IsDBNull("established") ? null : (new DateTimeRange(reader.GetDateTime("established"), reader.GetDateTime("established"))).ToFuzzyDate(),
                    Terminated = reader.IsDBNull("terminated") ? null : (new DateTimeRange(reader.GetDateTime("terminated"), reader.GetDateTime("terminated"))).ToFuzzyDate(),
                    FileIdTileImage = null,
                    Terms = vocabularyNames,
                    OrganizationTypeIds = new List<int>
                    {
                        await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest
                        {
                            TenantId = Constants.PPL,
                            UrlId = Constants.POLITICAL_PARTY
                        })
                    },
                    NodeTermIds = new List<int>(),
                    NewLocations = new List<EventuallyIdentifiableLocation>(),
                    PartyPoliticalEntityRelations = new List<EventuallyIdentifiablePartyPoliticalEntityRelationForNewParty>(),
                    PersonOrganizationRelations = new List<EventuallyIdentifiablePersonOrganizationRelationForNewOrganization>(),
                    InterOrganizationalRelationsToAddFrom = new List<EventuallyIdentifiableInterOrganizationalRelationForNewOrganizationFrom>(),
                    InterOrganizationalRelationsToAddTo = new List<EventuallyIdentifiableInterOrganizationalRelationForNewOrganizationTo>(),
                };
            }
            else {
                yield return new NewBasicOrganization {
                    Id = null,
                    PublisherId = reader.GetInt32("access_role_id"),
                    CreatedDateTime = reader.GetDateTime("created_date_time"),
                    ChangedDateTime = reader.GetDateTime("changed_date_time"),
                    Title = name,
                    OwnerId = Constants.OWNER_PARTIES,
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
                    NodeTypeId = reader.GetInt16("node_type_id"),
                    WebsiteUrl = reader.IsDBNull("website_url") ? null : reader.GetString("website_url"),
                    EmailAddress = reader.IsDBNull("email_address") ? null : reader.GetString("email_address"),
                    Description = reader.IsDBNull("description") ? "" : reader.GetString("description"),
                    Established = reader.IsDBNull("established") ? null : (new DateTimeRange(reader.GetDateTime("established").Date, reader.GetDateTime("established").Date.AddDays(1).AddMilliseconds(-1))).ToFuzzyDate(),
                    Terminated = reader.IsDBNull("terminated") ? null : (new DateTimeRange(reader.GetDateTime("terminated").Date, reader.GetDateTime("terminated").Date.AddDays(1).AddMilliseconds(-1))).ToFuzzyDate(),
                    FileIdTileImage = null,
                    Terms = vocabularyNames,
                    OrganizationTypeIds = organizationOrganizationTypeIds,
                    NodeTermIds = new List<int>(),
                    NewLocations = new List<EventuallyIdentifiableLocation>(),
                    PartyPoliticalEntityRelations = new List<EventuallyIdentifiablePartyPoliticalEntityRelationForNewParty>(),
                    PersonOrganizationRelations = new List<EventuallyIdentifiablePersonOrganizationRelationForNewOrganization>(),
                    InterOrganizationalRelationsToAddFrom = new List<EventuallyIdentifiableInterOrganizationalRelationForNewOrganizationFrom>(),
                    InterOrganizationalRelationsToAddTo = new List<EventuallyIdentifiableInterOrganizationalRelationForNewOrganizationTo>(),
                };
            }

        }
        await reader.CloseAsync();
    }


}
