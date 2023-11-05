namespace PoundPupLegacy.DomainModel.Updaters;

using PoundPupLegacy.DomainModel;
using PoundPupLegacy.DomainModel.Creators;
using Request = Person.ToUpdate;

internal sealed class PersonChangerFactory(
    IDatabaseUpdaterFactory<Request> databaseUpdaterFactory,
    IDatabaseUpdaterFactory<PersonOrganizationRelation.ToUpdate> personOrganizationRelationUpdaterFactory,
    IEntityCreatorFactory<PersonOrganizationRelation.ToCreate.ForExistingParticipants> personOrganizationRelationCreatorFactory,
    IDatabaseUpdaterFactory<InterPersonalRelation.ToUpdate> interPersonalRelationsUpdaterFactory,
    IEntityCreatorFactory<InterPersonalRelation.ToCreate.ForExistingParticipants> interPersonalRelationsCreatorFactory,
    IDatabaseUpdaterFactory<PartyPoliticalEntityRelation.ToUpdate> partyPoliticalEntityUpdaterFactory,
    IEntityCreatorFactory<PartyPoliticalEntityRelation.ToCreate.ForExistingParty> partyPoliticalEntityCreatorFactory,
    NodeDetailsChangerFactory nodeDetailsChangerFactory,
    IDatabaseUpdaterFactory<Term.ToUpdate> termUpdaterFactory,
    DatabaseMaterializedViewRefresherFactory termViewRefresherFactory
) : IEntityChangerFactory<Request>
{
    public async Task<IEntityChanger<Request>> CreateAsync(IDbConnection connection)
    {
        return new PersonChanger(
            await databaseUpdaterFactory.CreateAsync(connection),
            await personOrganizationRelationUpdaterFactory.CreateAsync(connection),
            await personOrganizationRelationCreatorFactory.CreateAsync(connection),
            await interPersonalRelationsUpdaterFactory.CreateAsync(connection),
            await interPersonalRelationsCreatorFactory.CreateAsync(connection),
            await partyPoliticalEntityUpdaterFactory.CreateAsync(connection),
            await partyPoliticalEntityCreatorFactory.CreateAsync(connection),
            await nodeDetailsChangerFactory.CreateAsync(connection),
            await termUpdaterFactory.CreateAsync(connection),
            await termViewRefresherFactory.CreateAsync(connection, "nameable_descendency")
        );
    }
}
public sealed class PersonChanger(
    IDatabaseUpdater<Request> databaseUpdater,
    IDatabaseUpdater<PersonOrganizationRelation.ToUpdate> personOrganizationRelationUpdater,
    IEntityCreator<PersonOrganizationRelation.ToCreate.ForExistingParticipants> personOrganizationRelationCreator,
    IDatabaseUpdater<InterPersonalRelation.ToUpdate> interPersonalRelationsUpdater,
    IEntityCreator<InterPersonalRelation.ToCreate.ForExistingParticipants> interPersonalRelationsCreator,
    IDatabaseUpdater<PartyPoliticalEntityRelation.ToUpdate> partyPoliticalEntityUpdater,
    IEntityCreator<PartyPoliticalEntityRelation.ToCreate.ForExistingParty> partyPoliticalEntityCreator,
    NodeDetailsChanger nodeDetailsChanger,
    IDatabaseUpdater<Term.ToUpdate> termUpdater,
    DatabaseMaterializedViewRefresher termViewRefresher
) : NameableChanger<Request>(databaseUpdater, nodeDetailsChanger, termUpdater, termViewRefresher)
{
    protected override async Task Process(Request request)
    {
        await base.Process(request);
        foreach (var personOrganizationRelations in request.PersonDetails.PersonOrganizationRelationsToCreate) {
            await personOrganizationRelationCreator.CreateAsync(personOrganizationRelations);
        }
        foreach (var personOrganizationRelations in request.PersonDetails.PersonOrganizationRelationsToUpdates) {
            await personOrganizationRelationUpdater.UpdateAsync(personOrganizationRelations);
        }
        foreach (var interPersonalRelations in request.PersonDetails.InterPersonalRelationFromToUpdate) {
            await interPersonalRelationsUpdater.UpdateAsync(interPersonalRelations);
        }
        foreach (var interPersonalRelations in request.PersonDetails.InterPersonalRelationToToUpdate) {
            await interPersonalRelationsUpdater.UpdateAsync(interPersonalRelations);
        }
        foreach (var interPersonalRelations in request.PersonDetails.InterPersonalRelationsFromToCreate) {
            await interPersonalRelationsCreator.CreateAsync(interPersonalRelations);
        }
        foreach (var interPersonalRelations in request.PersonDetails.InterPersonalRelationsToToCreate) {
            await interPersonalRelationsCreator.CreateAsync(interPersonalRelations);
        }
        foreach (var partyPoliticalEntityRelation in request.PersonDetails.PartyPoliticalEntityRelationToUpdate) {
            await partyPoliticalEntityUpdater.UpdateAsync(partyPoliticalEntityRelation);
        }
        foreach (var partyPoliticalEntityRelation in request.PersonDetails.PartyPoliticalEntityRelationsToCreate) {
            await partyPoliticalEntityCreator.CreateAsync(partyPoliticalEntityRelation);
        }
    }

    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await personOrganizationRelationUpdater.DisposeAsync();
        await personOrganizationRelationCreator.DisposeAsync();
        await interPersonalRelationsUpdater.DisposeAsync();
        await interPersonalRelationsCreator.DisposeAsync();
        await partyPoliticalEntityUpdater.DisposeAsync();
        await partyPoliticalEntityCreator.DisposeAsync();
    }
}

internal sealed class PersonUpdaterFactory : DatabaseUpdaterFactory<Request>
{
    private static readonly NonNullableIntegerDatabaseParameter NodeId = new() { Name = "node_id" };
    private static readonly NonNullableStringDatabaseParameter Title = new() { Name = "title" };
    private static readonly NonNullableStringDatabaseParameter Description = new() { Name = "description" };
    private static readonly NullableDateTimeDatabaseParameter DateOfBirth = new() { Name = "date_of_birth" };
    private static readonly NullableDateTimeDatabaseParameter DateOfDeath = new() { Name = "date_of_death" };
    private static readonly NullableIntegerDatabaseParameter FileIdPortrait = new() { Name = "file_id_portrait" };
    private static readonly NullableStringDatabaseParameter FirstName = new() { Name = "first_name" };
    private static readonly NullableStringDatabaseParameter MiddleName = new() { Name = "middle_name" };
    private static readonly NullableStringDatabaseParameter LastName = new() { Name = "last_name" };
    private static readonly NullableStringDatabaseParameter FullName = new() { Name = "full_name" };
    private static readonly NullableStringDatabaseParameter Suffix = new() { Name = "suffix" };
    private static readonly NullableIntegerDatabaseParameter GovtrackId = new() { Name = "govtrack_id" };
    private static readonly NullableStringDatabaseParameter Bioguide = new() { Name = "bioguide" };

    public override string Sql => $"""
        update node 
        set 
            title=@title
        where id = @node_id;
        update nameable 
        set 
            description=@description
        where id = @node_id;
        update person 
        set 
            date_of_birth=@date_of_birth,
            date_of_death= @date_of_death,
            file_id_portrait=@file_id_portrait,
            first_name= @first_name,
            middle_name=@middle_name,
            last_name= @last_name,
            full_name=@full_name,
            suffix= @suffix,
            govtrack_id=@govtrack_id,
            bioguide= @bioguide
        where id = @node_id;
        """;
    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new List<ParameterValue> {
            ParameterValue.Create(NodeId, request.Identification.Id),
            ParameterValue.Create(Title, request.NodeDetails.Title),
            ParameterValue.Create(Description, request.NameableDetails.Description),
            ParameterValue.Create(DateOfBirth, request.PersonDetails.DateOfBirth),
            ParameterValue.Create(DateOfDeath, request.PersonDetails.DateOfDeath),
            ParameterValue.Create(FileIdPortrait, request.PersonDetails.FileIdPortrait),
            ParameterValue.Create(GovtrackId, request.PersonDetails.GovtrackId),
            ParameterValue.Create(FirstName, request.PersonDetails.FirstName),
            ParameterValue.Create(MiddleName, request.PersonDetails.MiddleName),
            ParameterValue.Create(LastName, request.PersonDetails.LastName),
            ParameterValue.Create(Suffix, request.PersonDetails.Suffix),
            ParameterValue.Create(FullName, request.PersonDetails.FullName),
            ParameterValue.Create(Bioguide, request.PersonDetails.Bioguide),
        };
    }
}

