namespace PoundPupLegacy.DomainModel.Updaters;

using PoundPupLegacy.DomainModel;
using PoundPupLegacy.DomainModel.Creators;
using Request = OrganizationToUpdate;
internal sealed class OrganizationChangerFactory(
    IDatabaseUpdaterFactory<Request> databaseUpdaterFactory,
    IDatabaseUpdaterFactory<PersonOrganizationRelation.ToUpdate> personOrganizationRelationUpdaterFactory,
    IEntityCreatorFactory<PersonOrganizationRelation.ToCreate.ForExistingParticipants> personOrganizationRelationCreatorFactory,
    IDatabaseUpdaterFactory<InterOrganizationalRelation.ToUpdate> interOrganizationalRelationsUpdaterFactory,
    IEntityCreatorFactory<InterOrganizationalRelation.ToCreate.ForExistingParticipants> interOrganizationalRelationsCreatorFactory,
    IDatabaseUpdaterFactory<PartyPoliticalEntityRelation.ToUpdate> partyPoliticalEntityUpdaterFactory,
    IEntityCreatorFactory<PartyPoliticalEntityRelation.ToCreate.ForExistingParty> partyPoliticalEntityCreatorFactory,
    NodeDetailsChangerFactory nodeDetailsChangerFactory,
    IDatabaseUpdaterFactory<LocationUpdaterRequest> locationUpdaterFactory,
    LocatableDetailsCreatorFactory locatableDetailsCreatorFactory) : IEntityChangerFactory<Request>
{
    public async Task<IEntityChanger<Request>> CreateAsync(IDbConnection connection)
    {
        return new OrganizationChanger(
            await databaseUpdaterFactory.CreateAsync(connection),
            await personOrganizationRelationUpdaterFactory.CreateAsync(connection),
            await personOrganizationRelationCreatorFactory.CreateAsync(connection),
            await interOrganizationalRelationsUpdaterFactory.CreateAsync(connection),
            await interOrganizationalRelationsCreatorFactory.CreateAsync(connection),
            await partyPoliticalEntityUpdaterFactory.CreateAsync(connection),
            await partyPoliticalEntityCreatorFactory.CreateAsync(connection),
            await nodeDetailsChangerFactory.CreateAsync(connection),
            await locationUpdaterFactory.CreateAsync(connection),
            await locatableDetailsCreatorFactory.CreateAsync(connection)
        );
    }
}
public sealed class OrganizationChanger(
    IDatabaseUpdater<Request> databaseUpdater,
    IDatabaseUpdater<PersonOrganizationRelation.ToUpdate> personOrganizationRelationUpdater,
    IEntityCreator<PersonOrganizationRelation.ToCreate.ForExistingParticipants> personOrganizationRelationCreator,
    IDatabaseUpdater<InterOrganizationalRelation.ToUpdate> interOrganizationalRelationsUpdater,
    IEntityCreator<InterOrganizationalRelation.ToCreate.ForExistingParticipants> interOrganizationalRelationsCreator,
    IDatabaseUpdater<PartyPoliticalEntityRelation.ToUpdate> partyPoliticalEntityUpdater,
    IEntityCreator<PartyPoliticalEntityRelation.ToCreate.ForExistingParty> partyPoliticalEntityCreator,
    NodeDetailsChanger nodeDetailsChanger,
    IDatabaseUpdater<LocationUpdaterRequest> locationUpdater,
    LocatableDetailsCreator locatableDetailsCreator
) : LocatableChanger<Request>(databaseUpdater, nodeDetailsChanger, locationUpdater, locatableDetailsCreator)
{
    protected override async Task Process(Request request)
    {
        await base.Process(request);
        foreach (var personOrganizationRelations in request.OrganizationDetails.PersonOrganizationRelationsToCreate) {
            await personOrganizationRelationCreator.CreateAsync(personOrganizationRelations);
        }
        foreach (var personOrganizationRelations in request.OrganizationDetails.PersonOrganizationRelationsToUpdate) {
            await personOrganizationRelationUpdater.UpdateAsync(personOrganizationRelations);
        }
        foreach (var interOrganizationalRelations in request.OrganizationDetails.InterOrganizationalRelationsFromToUpdate) {
            await interOrganizationalRelationsUpdater.UpdateAsync(interOrganizationalRelations);
        }
        foreach (var interOrganizationalRelations in request.OrganizationDetails.InterOrganizationalRelationsToToUpdate) {
            await interOrganizationalRelationsUpdater.UpdateAsync(interOrganizationalRelations);
        }
        foreach (var interOrganizationalRelations in request.OrganizationDetails.InterOrganizationalRelationsFromToCreate) {
            await interOrganizationalRelationsCreator.CreateAsync(interOrganizationalRelations);
        }
        foreach (var interOrganizationalRelations in request.OrganizationDetails.InterOrganizationalRelationsToToCreate) {
            await interOrganizationalRelationsCreator.CreateAsync(interOrganizationalRelations);
        }
        foreach (var partyPoliticalEntityRelation in request.OrganizationDetails.PartyPoliticalEntityRelationsToUpdates) {
            await partyPoliticalEntityUpdater.UpdateAsync(partyPoliticalEntityRelation);
        }
        foreach (var partyPoliticalEntityRelation in request.OrganizationDetails.PartyPoliticalEntityRelationsToCreate) {
            await partyPoliticalEntityCreator.CreateAsync(partyPoliticalEntityRelation);
        }
    }

    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await personOrganizationRelationUpdater.DisposeAsync();
        await personOrganizationRelationCreator.DisposeAsync();
        await interOrganizationalRelationsUpdater.DisposeAsync();
        await interOrganizationalRelationsCreator.DisposeAsync();
        await partyPoliticalEntityUpdater.DisposeAsync();
        await partyPoliticalEntityCreator.DisposeAsync();
    }
}

internal sealed class OrganizationUpdaterFactory : DatabaseUpdaterFactory<Request>
{
    private static readonly NonNullableIntegerDatabaseParameter NodeId = new() { Name = "node_id" };
    private static readonly NonNullableStringDatabaseParameter Title = new() { Name = "title" };
    private static readonly NonNullableStringDatabaseParameter Description = new() { Name = "description" };
    private static readonly NullableStringDatabaseParameter WebsiteUrl = new() { Name = "website_url" };
    private static readonly NullableStringDatabaseParameter EmailAddress = new() { Name = "email_address" };
    private static readonly NullableFuzzyDateDatabaseParameter Established = new() { Name = "established" };
    private static readonly NullableFuzzyDateDatabaseParameter Terminated = new() { Name = "terminated" };

    public override string Sql => $"""
        update node 
        set 
            title=@title
        where id = @node_id;
        update nameable 
        set 
            description=@description
        where id = @node_id;
        update organization 
        set 
            website_url=@website_url,
            email_address=@email_address,
            established=@established,
            terminated=@terminated
        where id = @node_id;
        """;
    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new List<ParameterValue> {
            ParameterValue.Create(NodeId, request.Identification.Id),
            ParameterValue.Create(Title, request.NodeDetails.Title),
            ParameterValue.Create(Description, request.NameableDetails.Description),
            ParameterValue.Create(WebsiteUrl, request.OrganizationDetails.WebsiteUrl),
            ParameterValue.Create(EmailAddress, request.OrganizationDetails.EmailAddress),
            ParameterValue.Create(Established, request.OrganizationDetails.Established),
            ParameterValue.Create(Terminated, request.OrganizationDetails.Terminated)
        };
    }
}

