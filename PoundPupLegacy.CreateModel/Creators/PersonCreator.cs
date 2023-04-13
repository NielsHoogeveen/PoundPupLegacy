namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class PersonCreator : EntityCreator<Person>
{
    private readonly IDatabaseInserterFactory<Node> _nodeInserterFactory;
    private readonly IDatabaseInserterFactory<Searchable> _searchableInserterFactory;
    private readonly IDatabaseInserterFactory<Documentable> _documentableInserterFactory;
    private readonly IDatabaseInserterFactory<Locatable> _locatableInserterFactory;
    private readonly IDatabaseInserterFactory<Nameable> _nameableInserterFactory;
    private readonly IDatabaseInserterFactory<Party> _partyInserterFactory;
    private readonly IDatabaseInserterFactory<Person> _personInserterFactory;
    private readonly IDatabaseInserterFactory<Term> _termInserterFactory;
    private readonly IMandatorySingleItemDatabaseReaderFactory<TermReaderByNameRequest, Term> _termReaderFactory;
    private readonly IDatabaseInserterFactory<TermHierarchy> _termHierarchyInserterFactory;
    private readonly IMandatorySingleItemDatabaseReaderFactory<VocabularyIdReaderByOwnerAndNameRequest, int> _vocabularyIdReaderFactory;
    private readonly IDatabaseInserterFactory<TenantNode> _tenantNodeInserterFactory;
    private readonly IEntityCreator<ProfessionalRole> _professionalRoleCreator;
    private readonly IEntityCreator<PersonOrganizationRelation> _personOrganizationRelationCreator;


    public PersonCreator(
        IDatabaseInserterFactory<Node> nodeInserterFactory,
        IDatabaseInserterFactory<Searchable> searchableInserterFactory,
        IDatabaseInserterFactory<Documentable> documentableInserterFactory,
        IDatabaseInserterFactory<Locatable> locatableInserterFactory,
        IDatabaseInserterFactory<Nameable> nameableInserterFactory,
        IDatabaseInserterFactory<Party> partyInserterFactory,
        IDatabaseInserterFactory<Person> personInserterFactory,
        IDatabaseInserterFactory<Term> termInserterFactory,
        IMandatorySingleItemDatabaseReaderFactory<TermReaderByNameRequest, Term> termReaderFactory,
        IDatabaseInserterFactory<TermHierarchy> termHierarchyInserterFactory,
        IMandatorySingleItemDatabaseReaderFactory<VocabularyIdReaderByOwnerAndNameRequest, int> vocabularyIdReaderFactory,
        IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory,
        IEntityCreator<ProfessionalRole> professionalRoleCreator,
        IEntityCreator<PersonOrganizationRelation> personOrganizationRelationCreator
    )
    {
        _nodeInserterFactory = nodeInserterFactory;
        _searchableInserterFactory = searchableInserterFactory;
        _documentableInserterFactory = documentableInserterFactory;
        _locatableInserterFactory = locatableInserterFactory;
        _nameableInserterFactory = nameableInserterFactory;
        _partyInserterFactory = partyInserterFactory;
        _personInserterFactory = personInserterFactory;
        _termInserterFactory = termInserterFactory;
        _termReaderFactory = termReaderFactory;
        _termHierarchyInserterFactory = termHierarchyInserterFactory;
        _vocabularyIdReaderFactory = vocabularyIdReaderFactory;
        _tenantNodeInserterFactory = tenantNodeInserterFactory;
        _professionalRoleCreator = professionalRoleCreator;
        _personOrganizationRelationCreator = personOrganizationRelationCreator;
    }

    public override async Task CreateAsync(IAsyncEnumerable<Person> persons, IDbConnection connection)
    {

        await using var nodeWriter = await _nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await _searchableInserterFactory.CreateAsync(connection);
        await using var documentableWriter = await _documentableInserterFactory.CreateAsync(connection);
        await using var locatableWriter = await _locatableInserterFactory.CreateAsync(connection);
        await using var nameableWriter = await _nameableInserterFactory.CreateAsync(connection);
        await using var partyWriter = await _partyInserterFactory.CreateAsync(connection);
        await using var personWriter = await _personInserterFactory.CreateAsync(connection);
        await using var termWriter = await _termInserterFactory.CreateAsync(connection);
        await using var termReader = await _termReaderFactory.CreateAsync(connection);
        await using var termHierarchyWriter = await _termHierarchyInserterFactory.CreateAsync(connection);
        await using var vocabularyIdReader = await _vocabularyIdReaderFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await _tenantNodeInserterFactory.CreateAsync(connection);

        await foreach (var person in persons) {
            await nodeWriter.InsertAsync(person);
            await searchableWriter.InsertAsync(person);
            await documentableWriter.InsertAsync(person);
            await locatableWriter.InsertAsync(person);
            await nameableWriter.InsertAsync(person);
            await partyWriter.InsertAsync(person);
            await personWriter.InsertAsync(person);
            await WriteTerms(person, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in person.TenantNodes) {
                tenantNode.NodeId = person.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

            foreach (var role in person.ProfessionalRoles) {
                role.PersonId = person.Id;
            }
            await _professionalRoleCreator.CreateAsync(person.ProfessionalRoles.ToAsyncEnumerable(), connection);

            foreach (var relation in person.PersonOrganizationRelations) {
                relation.PersonId = person.Id;
            }
            await _personOrganizationRelationCreator.CreateAsync(person.PersonOrganizationRelations.ToAsyncEnumerable(), connection);

        }
    }
}
