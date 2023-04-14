namespace PoundPupLegacy.Convert;

internal sealed class NodeTypeMigrator : MigratorPPL
{
    protected override string Name => "node types";

    private readonly IEntityCreator<BasicNodeType> _nodeTypeCreator;
    private readonly IEntityCreator<CreateNodeAction> _createNodeActionCreator;
    private readonly IEntityCreator<DeleteNodeAction> _deleteNodeActionCreator;
    private readonly IEntityCreator<EditNodeAction> _editNodeActionCreator;


    public NodeTypeMigrator(
        IDatabaseConnections databaseConnections,
        IEntityCreator<BasicNodeType> nodeTypeCreator,
        IEntityCreator<CreateNodeAction> createNodeActionCreator,
        IEntityCreator<DeleteNodeAction> deleteNodeActionCreator,
        IEntityCreator<EditNodeAction> editNodeActionCreator
    ) : base(databaseConnections)
    {
        _nodeTypeCreator = nodeTypeCreator;
        _createNodeActionCreator = createNodeActionCreator;
        _deleteNodeActionCreator = deleteNodeActionCreator;
        _editNodeActionCreator = editNodeActionCreator;

    }
    protected override async Task MigrateImpl()
    {
        await _nodeTypeCreator.CreateAsync(GetNodeTypes(), _postgresConnection);
        await _createNodeActionCreator.CreateAsync(GetNodeTypes().Select(x => new CreateNodeAction { Id = null, NodeTypeId = x.Id!.Value }), _postgresConnection);
        await _deleteNodeActionCreator.CreateAsync(GetNodeTypes().Select(x => new DeleteNodeAction { Id = null, NodeTypeId = x.Id!.Value }), _postgresConnection);
        await _editNodeActionCreator.CreateAsync(GetNodeTypes().Select(x => new EditNodeAction { Id = null, NodeTypeId = x.Id!.Value }), _postgresConnection);

    }
    internal static async IAsyncEnumerable<BasicNodeType> GetNodeTypes()
    {
        await Task.CompletedTask;
        yield return BasicNodeType.Create(1, "organization type", "Organizations are loosely defined as something a collection of people work together. Therefore a bill or a trip is also regarderd an organization, even though it does not have a formal position as such", false);
        yield return BasicNodeType.Create(2, "inter-organizational relation type", "Defines the type of relation between two organizations", false);
        yield return BasicNodeType.Create(3, "party political entity relation type", "Defines the type of relation between a person or organization and a political entity", false);
        yield return BasicNodeType.Create(4, "person organization relation type", "Defines the type of relation between a person and an organization", false);
        yield return BasicNodeType.Create(5, "inter-personal relationship type", "Defines the type of relation between a person and another person", false);
        yield return BasicNodeType.Create(6, "profession", "The type of professions a person can have", false);
        yield return BasicNodeType.Create(7, "denomination", "The denomination of an organization", false);
        yield return BasicNodeType.Create(8, "Hague status", "The hague status of an adoption agency", false);
        yield return BasicNodeType.Create(9, "document type", "Defines the type of a document", false);
        yield return BasicNodeType.Create(10, "document", "A text based document", false);
        yield return BasicNodeType.Create(11, "first level global region", "First level subdivision of the world", false);
        yield return BasicNodeType.Create(12, "second level global region", "Second level subdivision of the world", false);
        yield return BasicNodeType.Create(13, "basic country", "Countries that don't contain other countries and that are not part of another country", false);
        yield return BasicNodeType.Create(14, "bound country", "Countries that are part of another country", false);
        yield return BasicNodeType.Create(15, "country and first and bottom level subdivision", "Countries that are also first level subdivisions of another country and that allows no further subdivision", false);
        yield return BasicNodeType.Create(16, "country and first and second level subdivision", "Countries that are also first and second level subdivisions of another country", false);
        yield return BasicNodeType.Create(17, "first and bottom level subdivision", "Subdivision of a country that contains no further subdivisions", false);
        yield return BasicNodeType.Create(18, "informal intermediate level subdivision", "Informal subdivision of a country that contains second level subdivisions", false);
        yield return BasicNodeType.Create(19, "basic second level subdivision", "Second level subdivision of a country", false);
        yield return BasicNodeType.Create(20, "binding country", "Country that contains other countries", false);
        yield return BasicNodeType.Create(21, "country and intermediate level subdivision", "Countries that are also first level subdivisions of another country and that do allow further subdivision", false);
        yield return BasicNodeType.Create(22, "formal intermediate level subdivision", "Formal subdivision of a country that contains second level subdivisions", false);
        yield return BasicNodeType.Create(23, "organization", "A collection of people working together. Therefore a bill or a trip is also regarderd an organization, even though it does not have a formal position as such.", false);
        yield return BasicNodeType.Create(24, "person", "Person", false);
        yield return BasicNodeType.Create(27, "child placement type", "Defined the type of a child placement", false);
        yield return BasicNodeType.Create(28, "family size", "Defined the type family size", false);
        yield return BasicNodeType.Create(35, "blog post", "Blog post", true);
        yield return BasicNodeType.Create(36, "article", "Article", true);
        yield return BasicNodeType.Create(37, "discussion", "Discussion", true);
        yield return BasicNodeType.Create(38, "vocabulary", "A set of terms", false);
        yield return BasicNodeType.Create(39, "type of abuse", "Defines the types of abuse a child has endured", false);
        yield return BasicNodeType.Create(40, "type of abuser", "Defines the relationship the abuser has with respect to the abused", false);
        yield return BasicNodeType.Create(41, "basic nameable", "Can be used as a term without having additional data", false);
        yield return BasicNodeType.Create(42, "page", "A simpe text node", false);
        yield return BasicNodeType.Create(43, "review", "A book review", false);

        yield return BasicNodeType.Create(45, "inter country relation", "A relation between two countries", false);
        yield return BasicNodeType.Create(46, "inter personal relation", "A relation between two persons", false);
        yield return BasicNodeType.Create(47, "inter organizational relation", "A relation between two organizations", false);
        yield return BasicNodeType.Create(48, "person organization relation", "A relation between a person and an organization", false);
        yield return BasicNodeType.Create(49, "party political entity relation", "A relation between a person and an organization", false);
        yield return BasicNodeType.Create(50, "inter country relation type", "The type of relation two countries can have", false);
        yield return BasicNodeType.Create(51, "subdivision type", "The type of a subdivision of a country can have", false);
        yield return BasicNodeType.Create(52, "united states congressional meeting", "The two year period the United States congress comes together", false);
        yield return BasicNodeType.Create(53, "single question poll", "Poll that asks a single question", false);
        yield return BasicNodeType.Create(54, "multi question poll", "Poll that asks a multiple questions", false);
        yield return BasicNodeType.Create(55, "poll question", "Poll question", false);
        yield return BasicNodeType.Create(56, "house bill", "A bill that is introduced in the US house of representatives", false);
        yield return BasicNodeType.Create(57, "senate bill", "A bill that is introduced in the US senate", false);
        yield return BasicNodeType.Create(58, "bill action type", "The type of actions a person can take with respect to a bill", false);
        yield return BasicNodeType.Create(59, "senator", "Represents a United States senator", false);
        yield return BasicNodeType.Create(60, "representative", "Represents a member of the United States House of Representatives", false);
        yield return BasicNodeType.Create(61, "basic profession", "A profession", false);
        yield return BasicNodeType.Create(62, "united states political party affilition", "Political party affiliations as used in the United States of America", false);
        yield return BasicNodeType.Create(63, "united states political party", "Political party of the United States of America", false);
        yield return BasicNodeType.Create(64, "congressional term political party affiliation", "The political party affiliation of a member of congresss during a term", false);
        yield return BasicNodeType.Create(65, "senate term", "A term of a United States Senator", false);
        yield return BasicNodeType.Create(66, "house term", "A term of a United States Representative", false);
    }

}
