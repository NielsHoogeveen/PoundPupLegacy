namespace PoundPupLegacy.Convert;

internal sealed class PersonMigratorCPCT(
    IDatabaseConnections databaseConnections,
    IMandatorySingleItemDatabaseReaderFactory<NodeIdReaderByUrlIdRequest, int> nodeIdReaderFactory,
    ISingleItemDatabaseReaderFactory<TenantNodeReaderByUrlIdRequest, NewTenantNodeForExistingNode> tenantNodeReaderByUrlIdFactory,
    IEntityCreatorFactory<EventuallyIdentifiablePerson> personCreatorFactory
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
    private async IAsyncEnumerable<NewPerson> ReadPersons(
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


            var vocabularyNames = new List<NewTermForNewNameble> {
                new NewTermForNewNameble {
                    VocabularyId = vocabularyId,
                    Name = title,
                    ParentTermIds = new List<int>(),
                }
            };

            yield return new NewPerson {
                Id = null,
                PublisherId = reader.GetInt32("access_role_id"),
                CreatedDateTime = reader.GetDateTime("created_date_time"),
                ChangedDateTime = reader.GetDateTime("changed_date_time"),
                Title = title,
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
                        UrlId = null
                    },
                    new NewTenantNodeForNewNode
                    {
                        Id = null,
                        TenantId = Constants.CPCT,
                        PublicationStatusId = 2,
                        UrlPath = null,
                        SubgroupId = null,
                        UrlId = id
                    }
                },
                NodeTypeId = 24,
                Description = "",
                FileIdTileImage = null,
                Terms = vocabularyNames,
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
                ProfessionalRoles = new List<EventuallyIdentifiableProfessionalRoleForNewPerson>(),
                PersonOrganizationRelations = new List<EventuallyIdentifiablePersonOrganizationRelationForNewPerson>(),
                NodeTermIds = new List<int>(),
                NewLocations = new List<EventuallyIdentifiableLocation>(),
                PartyPoliticalEntityRelations = new List<EventuallyIdentifiablePartyPoliticalEntityRelationForNewParty>(),
                InterPersonalRelationsToAddFrom = new List<EventuallyIdentifiableInterPersonalRelationForNewPersonFrom>(),
                InterPersonalRelationsToAddTo = new List<EventuallyIdentifiableInterPersonalRelationForNewPersonTo>(),
            };

        }
        await reader.CloseAsync();
    }

}
