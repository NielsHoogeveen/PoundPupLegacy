namespace PoundPupLegacy.Convert;

internal sealed class NodeTypeMigrator(
    IDatabaseConnections databaseConnections,
    IEntityCreator<BasicNodeType> nodeTypeCreator,
    IEntityCreator<BasicNameableType> nameableTypeCreator,
    IEntityCreator<CreateNodeAction> createNodeActionCreator,
    IEntityCreator<DeleteNodeAction> deleteNodeActionCreator,
    IEntityCreator<EditNodeAction> editNodeActionCreator
) : MigratorPPL(databaseConnections)
{
    protected override string Name => "node types";

    protected override async Task MigrateImpl()
    {
        await nodeTypeCreator.CreateAsync(GetBasicNodeTypes().ToAsyncEnumerable(), _postgresConnection);
        await nameableTypeCreator.CreateAsync(GetNameableTypes().ToAsyncEnumerable(), _postgresConnection);

        List<NodeType> nodeTypes = GetBasicNodeTypes().OfType<NodeType>().ToList();
        nodeTypes.AddRange(GetNameableTypes().OfType<NodeType>());

        await createNodeActionCreator.CreateAsync(GetNodeTypes().Select(x => new CreateNodeAction { Id = null, NodeTypeId = x.Id!.Value }), _postgresConnection);
        await deleteNodeActionCreator.CreateAsync(GetNodeTypes().Select(x => new DeleteNodeAction { Id = null, NodeTypeId = x.Id!.Value }), _postgresConnection);
        await editNodeActionCreator.CreateAsync(GetNodeTypes().Select(x => new EditNodeAction { Id = null, NodeTypeId = x.Id!.Value }), _postgresConnection);
    }

    internal async static IAsyncEnumerable<NodeType> GetNodeTypes()
    {
        await Task.CompletedTask;
        foreach (var nodeType in GetBasicNodeTypes()) {
            yield return nodeType;
        }
        foreach (var nodeType in GetNameableTypes()) {
            yield return nodeType;
        }
    }

    private static IEnumerable<BasicNameableType> GetNameableTypes()
    {
        yield return BasicNameableType.Create(11, "first level global region", "First level subdivision of the world", false, "Regions");
        yield return BasicNameableType.Create(12, "second level global region", "Second level subdivision of the world", false, "Regions");

        yield return BasicNameableType.Create(13, "basic country", "Countries that don't contain other countries and that are not part of another country", false, "Countries");
        yield return BasicNameableType.Create(14, "bound country", "Countries that are part of another country", false, "Countries");
        yield return BasicNameableType.Create(15, "country and first and bottom level subdivision", "Countries that are also first level subdivisions of another country and that allows no further subdivision", false, "Countries");
        yield return BasicNameableType.Create(16, "country and first and second level subdivision", "Countries that are also first and second level subdivisions of another country", false, "Countries");

        yield return BasicNameableType.Create(17, "first and bottom level subdivision", "Subdivision of a country that contains no further subdivisions", false, "Subdivisions");
        yield return BasicNameableType.Create(18, "informal intermediate level subdivision", "Informal subdivision of a country that contains second level subdivisions", false, "Subdivisions");
        yield return BasicNameableType.Create(19, "basic second level subdivision", "Second level subdivision of a country", false, "Subdivisions");

        yield return BasicNameableType.Create(20, "binding country", "Country that contains other countries", false, "Countries");
        yield return BasicNameableType.Create(21, "country and intermediate level subdivision", "Countries that are also first level subdivisions of another country and that do allow further subdivision", false, "Countries");

        yield return BasicNameableType.Create(22, "formal intermediate level subdivision", "Formal subdivision of a country that contains second level subdivisions", false, "Subdivisions");

        yield return BasicNameableType.Create(23, "organization", "A collection of people working together. Therefore a bill or a trip is also regarderd an organization, even though it does not have a formal position as such.", false, "Organizations");
        yield return BasicNameableType.Create(24, "person", "Person", false, "Persons");

        yield return BasicNameableType.Create(36, "act", "Act", false, "Acts");

        yield return BasicNameableType.Create(56, "house bill", "A bill that is introduced in the US house of representatives", false, "Bills");
        yield return BasicNameableType.Create(57, "senate bill", "A bill that is introduced in the US senate", false, "Bills");

        yield return BasicNameableType.Create(41, "basic nameable", "Can be used as a term without having additional data", false, "Topics");

        yield return BasicNameableType.Create(59, "senator", "Represents a United States senator", false, "Persons");
        yield return BasicNameableType.Create(60, "representative", "Represents a member of the United States House of Representatives", false, "Persons");

        yield return BasicNameableType.Create(63, "united states political party", "Political party of the United States of America", false, "Organizations");

        yield return BasicNameableType.Create(1, "organization type", "Organizations are loosely defined as something a collection of people work together. Therefore a bill or a trip is also regarderd an organization, even though it does not have a formal position as such", false, "Topics");
        yield return BasicNameableType.Create(2, "inter-organizational relation type", "Defines the type of relation between two organizations", false, "Topics");
        yield return BasicNameableType.Create(3, "party political entity relation type", "Defines the type of relation between a person or organization and a political entity", false, "Topics");
        yield return BasicNameableType.Create(4, "person organization relation type", "Defines the type of relation between a person and an organization", false, "Topics");
        yield return BasicNameableType.Create(5, "inter-personal relationship type", "Defines the type of relation between a person and another person", false, "Topics");
        yield return BasicNameableType.Create(6, "profession", "The type of professions a person can have", false, "Topics");
        yield return BasicNameableType.Create(7, "denomination", "The denomination of an organization", false, "Topics");
        yield return BasicNameableType.Create(8, "Hague status", "The hague status of an adoption agency", false, "Topics");
        yield return BasicNameableType.Create(9, "document type", "Defines the type of a document", false, "Topics");
        yield return BasicNameableType.Create(27, "child placement type", "Defined the type of a child placement", false, "Topics");
        yield return BasicNameableType.Create(28, "family size", "Defined the type family size", false, "Topics");
        yield return BasicNameableType.Create(39, "type of abuse", "Defines the types of abuse a child has endured", false, "Topics");
        yield return BasicNameableType.Create(40, "type of abuser", "Defines the relationship the abuser has with respect to the abused", false, "Topics");
        yield return BasicNameableType.Create(50, "inter country relation type", "The type of relation two countries can have", false, "Topics");
        yield return BasicNameableType.Create(51, "subdivision type", "The type of a subdivision of a country can have", false, "Topics");
        yield return BasicNameableType.Create(58, "bill action type", "The type of actions a person can take with respect to a bill", false, "Topics");
        yield return BasicNameableType.Create(61, "basic profession", "A profession", false, "Topics");

    }
    private static IEnumerable<BasicNodeType> GetBasicNodeTypes()
    {
        yield return BasicNodeType.Create(10, "document", "A text based document", false);

        yield return BasicNodeType.Create(35, "blog post", "Blog post", true);
        yield return BasicNodeType.Create(37, "discussion", "Discussion", true);
        yield return BasicNodeType.Create(38, "vocabulary", "A set of terms", false);
        yield return BasicNodeType.Create(42, "page", "A simpe text node", false);
        yield return BasicNodeType.Create(43, "review", "A book review", false);

        yield return BasicNodeType.Create(45, "inter country relation", "A relation between two countries", false);
        yield return BasicNodeType.Create(46, "inter personal relation", "A relation between two persons", false);
        yield return BasicNodeType.Create(47, "inter organizational relation", "A relation between two organizations", false);
        yield return BasicNodeType.Create(48, "person organization relation", "A relation between a person and an organization", false);
        yield return BasicNodeType.Create(49, "party political entity relation", "A relation between a person and an organization", false);
        yield return BasicNodeType.Create(52, "united states congressional meeting", "The two year period the United States congress comes together", false);
        yield return BasicNodeType.Create(53, "single question poll", "Poll that asks a single question", false);
        yield return BasicNodeType.Create(54, "multi question poll", "Poll that asks a multiple questions", false);
        yield return BasicNodeType.Create(55, "poll question", "Poll question", false);
        yield return BasicNodeType.Create(62, "united states political party affilition", "Political party affiliations as used in the United States of America", false);

        yield return BasicNodeType.Create(64, "congressional term political party affiliation", "The political party affiliation of a member of congresss during a term", false);
        yield return BasicNodeType.Create(65, "senate term", "A term of a United States Senator", false);
        yield return BasicNodeType.Create(66, "house term", "A term of a United States Representative", false);
    }
}
