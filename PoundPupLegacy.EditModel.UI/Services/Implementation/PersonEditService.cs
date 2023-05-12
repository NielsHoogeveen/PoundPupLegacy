using Npgsql;
using PoundPupLegacy.CreateModel.Creators;
using PoundPupLegacy.EditModel.Readers;
using PoundPupLegacy.EditModel.Updaters;
using System.Data;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal sealed class PersonEditService : PartyEditServiceBase<Person, CreateModel.Person>, IEditService<Person>
{

    private readonly ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, Person> _personUpdateDocumentReaderFactory;
    private readonly ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, Person> _personCreateDocumentReaderFactory;
    private readonly ISaveService<IEnumerable<Location>> _locationsSaveService;
    private readonly IDatabaseUpdaterFactory<PersonUpdaterRequest> _personUpdateFactory;
    private readonly IEntityCreator<CreateModel.Person> _personEntityCreator;
    public PersonEditService(
        IDbConnection connection,
        ITenantRefreshService tenantRefreshService,
        ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, Person> personUpdateDocumentReaderFactory,
        ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, Person> personCreateDocumentReaderFactory,
        IDatabaseUpdaterFactory<PersonUpdaterRequest> personUpdateFactory,
        ISaveService<IEnumerable<Tag>> tagSaveService,
        ISaveService<IEnumerable<TenantNode>> tenantNodesSaveService,
        ISaveService<IEnumerable<File>> filesSaveService,
        ISaveService<IEnumerable<Location>> locationsSaveService,
        IEntityCreator<CreateModel.Person> personEntityCreator,
        ITextService textService

    ) : base(
        connection,
        tagSaveService,
        tenantNodesSaveService,
        filesSaveService,
        textService,
        tenantRefreshService)
    {
        _personUpdateDocumentReaderFactory = personUpdateDocumentReaderFactory;
        _personCreateDocumentReaderFactory = personCreateDocumentReaderFactory;
        _locationsSaveService = locationsSaveService;
        _personUpdateFactory = personUpdateFactory;
        _personEntityCreator = personEntityCreator;
    }
    public async Task<Person?> GetViewModelAsync(int urlId, int userId, int tenantId)
    {
        try {
            await _connection.OpenAsync();
            await using var reader = await _personUpdateDocumentReaderFactory.CreateAsync(_connection);
            return await reader.ReadAsync(new NodeUpdateDocumentRequest {
                UrlId = urlId,
                UserId = userId,
                TenantId = tenantId
            });
        }
        finally {
            if (_connection.State == ConnectionState.Open) {
                await _connection.CloseAsync();
            }
        }
    }

    protected override async Task StoreAdditional(Person person)
    {
        await base.StoreAdditional(person);
        await _locationsSaveService.SaveAsync(person.Locations, _connection);
    }
    public async Task<Person?> GetViewModelAsync(int userId, int tenantId)
    {
        try {
            await _connection.OpenAsync();
            await using var reader = await _personCreateDocumentReaderFactory.CreateAsync(_connection);
            return await reader.ReadAsync(new NodeCreateDocumentRequest {
                NodeTypeId = Constants.PERSON,
                UserId = userId,
                TenantId = tenantId
            });
        }
        finally {
            if (_connection.State == ConnectionState.Open) {
                await _connection.CloseAsync();
            }
        }
    }
    protected sealed override async Task StoreNew(Person person, NpgsqlConnection connection)
    {
        var now = DateTime.Now;
        await _personEntityCreator.CreateAsync(new CreateModel.Person {
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
            TenantNodes = person.TenantNodes.Select(x => new CreateModel.TenantNode {
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
            ProfessionalRoles = new List<CreateModel.ProfessionalRole>(),
            PersonOrganizationRelations  = new List<CreateModel.PersonOrganizationRelation>()
        }
        , connection
        );
    }

    protected sealed override async Task StoreExisting(Person person, NpgsqlConnection connection)
    {
        if (!person.NodeId.HasValue) {
            throw new Exception("NodeId of person should have a value");
        }
        var updater = await _personUpdateFactory.CreateAsync(connection);

        await updater.UpdateAsync(new PersonUpdaterRequest {
            Title = person.Title,
            Description = person.Description,
            NodeId = person.NodeId.Value,
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
