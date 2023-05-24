using Microsoft.Extensions.Logging;
using PoundPupLegacy.CreateModel;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal sealed class PersonEditService(
    IDbConnection connection,
    ILogger<PersonEditService> logger,
    ITenantRefreshService tenantRefreshService,
    ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, NewPerson> personCreateDocumentReaderFactory,
    ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, ExistingPerson> personUpdateDocumentReaderFactory,
    IDatabaseUpdaterFactory<PersonUpdaterRequest> personUpdateFactory,
    ISaveService<IEnumerable<Tag>> tagSaveService,
    ISaveService<IEnumerable<TenantNode>> tenantNodesSaveService,
    ISaveService<IEnumerable<File>> filesSaveService,
    ISaveService<IEnumerable<Location>> locationsSaveService,
    INameableCreatorFactory<EventuallyIdentifiablePerson> personEntityCreatorFactory,
    ITextService textService

) : PartyEditServiceBase<Person, ExistingPerson, NewPerson, CreateModel.NewPerson>(
    connection,
    logger,
    tagSaveService,
    tenantNodesSaveService,
    filesSaveService,
    textService,
    tenantRefreshService
), IEditService<Person, Person>
{

    public async Task<Person?> GetViewModelAsync(int urlId, int userId, int tenantId)
    {
        return await WithConnection(async (connection) => {
            await using var reader = await personUpdateDocumentReaderFactory.CreateAsync(connection);
            return await reader.ReadAsync(new NodeUpdateDocumentRequest {
                UrlId = urlId,
                UserId = userId,
                TenantId = tenantId
            });
        });
    }

    protected override async Task StoreAdditional(Person person, int nodeId, NpgsqlConnection connection)
    {
        await base.StoreAdditional(person, nodeId, connection);
        await locationsSaveService.SaveAsync(person.Locations, connection);
    }
    public async Task<Person?> GetViewModelAsync(int userId, int tenantId)
    {
        return await WithConnection(async (connection) => {
            await using var reader = await personCreateDocumentReaderFactory.CreateAsync(connection);
            return await reader.ReadAsync(new NodeCreateDocumentRequest {
                NodeTypeId = Constants.PERSON,
                UserId = userId,
                TenantId = tenantId
            });
        });
    }
    protected sealed override async Task<int> StoreNew(NewPerson person, NpgsqlConnection connection)
    {
        var now = DateTime.Now;
        var creationPerson = new CreateModel.NewPerson {
            Id = null,
            Title = person.Title,
            Description = person.Description,
            PublisherId = person.PublisherId,
            CreatedDateTime = now,
            ChangedDateTime = now,
            FileIdTileImage = null,
            NodeTypeId = Constants.ORGANIZATION,
            OwnerId = person.OwnerId,
            AuthoringStatusId = 1,
            TenantNodes = person.TenantNodes.Select(x => new CreateModel.NewTenantNodeForNewNode {
                NodeId = null,
                TenantId = x.TenantId,
                UrlPath = x.UrlPath,
                PublicationStatusId = x.PublicationStatusId,
                SubgroupId = x.SubgroupId,
                Id = null,
                UrlId = null
            }).ToList(),
            VocabularyNames = new List<CreateModel.VocabularyName>(),
            Bioguide = null,
            DateOfBirth = null,
            DateOfDeath = null,
            FileIdPortrait = null,
            FirstName = null,
            FullName = null,
            GovtrackId = null,
            LastName = null,
            MiddleName = null,
            Suffix = null,
            ProfessionalRoles = new List<EventuallyIdentifiableProfessionalRole>(),
            PersonOrganizationRelations = new List<CreateModel.NewPersonOrganizationRelation>(),
            NodeTermIds = new List<int>(),
        };
        await using var personEntityCreator = await personEntityCreatorFactory.CreateAsync(connection);
        await personEntityCreator.CreateAsync(creationPerson);
        return creationPerson.Id!.Value;
    }

    protected sealed override async Task StoreExisting(ExistingPerson person, NpgsqlConnection connection)
    {
        var updater = await personUpdateFactory.CreateAsync(connection);

        await updater.UpdateAsync(new PersonUpdaterRequest {
            Title = person.Title,
            Description = person.Description,
            NodeId = person.NodeId,
            Bioguide = null,
            DateOfBirth = null,
            DateOfDeath = null,
            FileIdPortrait = null,
            FileIdTileImage = null,
            FirstName = null,
            FullName = null,
            GovtrackId = null,
            LastName = null,
            MiddleName = null,
            Suffix = null,
        });
    }

}
