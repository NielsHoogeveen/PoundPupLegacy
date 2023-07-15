using PoundPupLegacy.DomainModel;
using PoundPupLegacy.DomainModel.Creators;
using PoundPupLegacy.DomainModel.Readers;

namespace PoundPupLegacy.Convert;

internal sealed class PersonMigratorCPCT(
    IDatabaseConnections databaseConnections,
    IMandatorySingleItemDatabaseReaderFactory<NodeIdReaderByUrlIdRequest, int> nodeIdReaderFactory,
    ISingleItemDatabaseReaderFactory<TenantNodeReaderByUrlIdRequest, TenantNode.ToCreate.ForExistingNode> tenantNodeReaderByUrlIdFactory,
    IEntityCreatorFactory<Person.ToCreate> personCreatorFactory
) : MigratorCPCT(
    databaseConnections, 
    nodeIdReaderFactory, 
    tenantNodeReaderByUrlIdFactory
)
{
    protected override string Name => "persons (cpct)";

    protected override async Task MigrateImpl()
    {
        await using var personCreator = await personCreatorFactory.CreateAsync(_postgresConnection);
        await using var nodeIdReader = await nodeIdReaderFactory.CreateAsync(_postgresConnection);
        await personCreator.CreateAsync(ReadPersons(nodeIdReader));
    }
    private async IAsyncEnumerable<Person.ToCreate> ReadPersons(
        IMandatorySingleItemDatabaseReader<NodeIdReaderByUrlIdRequest, int> nodeIdReader
    )
    {

        var sql = $"""
                SELECT
                DISTINCT
                n.nid id,
                n.uid access_role_id,
                TRIM(n.title) title,
                n.`status` node_status_id,
                FROM_UNIXTIME(n.created) created_date_time, 
                FROM_UNIXTIME(n.changed) changed_date_time
                FROM node n
                WHERE n.`type` = 'adopt_person'
                AND n.nid > 33162 and n.nid not in (
                35063,
                35343,
                38251, 
                38252, 
                38253, 
                38254, 
                38255, 
                38257, 
                38258, 
                37168, 
                33320, 
                36272, 
                33648, 
                33627, 
                33222, 
                36067, 
                35811,
                35142,
                41441,
                34879,
                34623,
                34572,
                36603,
                34344,
                34274,
                34214,
                36362,
                33636,
                41917,
                49167,
                49894
                )
                
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
            var id = reader.GetInt32("id");
            var title = id switch {
                37074 => "Paul Cook (A Child's Hope Foundation)",
                _ => reader.GetString("title")
            };


            var vocabularyNames = new List<Term.ToCreateForNewNameable> {
                new Term.ToCreateForNewNameable {
                    Identification = new Identification.Possible {
                        Id = null,
                    },
                    VocabularyId = vocabularyId,
                    Name = title,
                    ParentTermIds = new List<int>(),
                }
            };

            yield return new Person.ToCreate {
                Identification = new Identification.Possible {
                    Id = null
                },
                NodeDetails = new NodeDetails.ForCreate {
                    PublisherId = reader.GetInt32("access_role_id"),
                    CreatedDateTime = reader.GetDateTime("created_date_time"),
                    ChangedDateTime = reader.GetDateTime("changed_date_time"),
                    Title = title,
                    OwnerId = Constants.OWNER_PARTIES,
                    AuthoringStatusId = 1,
                    TenantNodes = new List<TenantNode.ToCreate.ForNewNode>
                    {
                        new TenantNode.ToCreate.ForNewNode
                        {
                            Identification = new Identification.Possible {
                                Id = null
                            },
                            TenantId = Constants.PPL,
                            PublicationStatusId = 1,
                            UrlPath = null,
                            SubgroupId = null,
                            UrlId = null
                        },
                        new TenantNode.ToCreate.ForNewNode
                        {
                            Identification = new Identification.Possible {
                                Id = null
                            },
                            TenantId = Constants.CPCT,
                            PublicationStatusId = 2,
                            UrlPath = null,
                            SubgroupId = null,
                            UrlId = id
                        }
                    },
                    NodeTypeId = 24,
                    TermIds = new List<int>(),
                },
                NameableDetails = new NameableDetails.ForCreate {
                    Description = "",
                    FileIdTileImage = null,
                    Terms = vocabularyNames,
                },
                LocatableDetails = new LocatableDetails.ForCreate {
                    Locations = new List<Location.ToCreate>(),
                },
                PersonDetails = new PersonDetails.ForCreate {
                    DateOfBirth = null,
                    DateOfDeath = null,
                    FileIdPortrait = null,
                    FirstName = null,
                    LastName = null,
                    MiddleName = null,
                    FullName = null,
                    GovtrackId = null,
                    Bioguide = null,
                    Suffix = null,
                    InterPersonalRelationsToCreateFrom = new List<InterPersonalRelation.ToCreate.ForNewPersonFrom>(),
                    InterPersonalRelationsToCreateTo = new List<InterPersonalRelation.ToCreate.ForNewPersonTo>(),
                    PartyPoliticalEntityRelationsToCreate = new List<PartyPoliticalEntityRelation.ToCreate.ForNewParty>(),
                    PersonOrganizationRelationsToCreate = new List<PersonOrganizationRelation.ToCreate.ForNewPerson>(),
                    ProfessionalRolesToCreate = new List<ProfessionalRoleToCreateForNewPerson>(),
                },
            };

        }
        await reader.CloseAsync();
    }
}
