using PoundPupLegacy.Db;
using PoundPupLegacy.Model;

namespace PoundPupLegacy.Convert;

internal sealed class NodeTypeMigrator : PPLMigrator
{
    protected override string Name => "node types";

    public NodeTypeMigrator(MySqlToPostgresConverter converter) : base(converter)
    {

    }
    protected override async Task MigrateImpl()
    {
        await NodeTypeCreator.CreateAsync(GetNodeTypes(), _postgresConnection);
        await CreateNodeActionCreator.CreateAsync(GetNodeTypes().Select(x => new CreateNodeAction { Id = null, NodeTypeId = x.Id!.Value }), _postgresConnection);
        await DeleteNodeActionCreator.CreateAsync(GetNodeTypes().Select(x => new DeleteNodeAction { Id = null, NodeTypeId = x.Id!.Value }), _postgresConnection);
        await EditNodeActionCreator.CreateAsync(GetNodeTypes().Select(x => new EditNodeAction { Id = null, NodeTypeId = x.Id!.Value }), _postgresConnection);
        await CaseTypeCreator.CreateAsync(GetCaseTypes(), _postgresConnection);
        await CreateNodeActionCreator.CreateAsync(GetCaseTypes().Select(x => new CreateNodeAction { Id = null, NodeTypeId = x.Id!.Value }), _postgresConnection);
        await DeleteNodeActionCreator.CreateAsync(GetCaseTypes().Select(x => new DeleteNodeAction { Id = null, NodeTypeId = x.Id!.Value }), _postgresConnection);
        await EditNodeActionCreator.CreateAsync(GetCaseTypes().Select(x => new EditNodeAction { Id = null, NodeTypeId = x.Id!.Value }), _postgresConnection);

    }
    internal static async IAsyncEnumerable<CaseType> GetCaseTypes()
    {
        await Task.CompletedTask;
        yield return new CaseType(26, "abuse case", "Abuse case of a child that has been placed by court", new List<int> { 1, 2, 3, 4, 5, 6});
        yield return new CaseType(29, "child trafficking case", "Trafficking case of children to be adopted", new List<int> { 2, 4, 5 });
        yield return new CaseType(30, "coerced adoption case", "Adoption that involved coercion", new List<int> { 2 });
        yield return new CaseType(31, "deportation case", "Adoptees deported to country of origin", new List<int>());
        yield return new CaseType(32, "father's rights violation case", "Adoptions where the rights of the biological father were violated", new List<int> { 2 });
        yield return new CaseType(33, "wrongful medication case", "Child placement situation where wrongful medication is present", new List<int> { 7 });
        yield return new CaseType(34, "wrongful removal case", "Children wrongfully removed from their family", new List<int> { 7 });
        yield return new CaseType(44, "disrupted placement case", "A situation where the placement of a child was reverted", new List<int> { 2, 4 });

    }
    internal static async IAsyncEnumerable<BasicNodeType> GetNodeTypes()
    {
        await Task.CompletedTask;
        yield return new BasicNodeType(1, "organization type", "Organizations are loosely defined as something a collection of people work together. Therefore a bill or a trip is also regarderd an organization, even though it does not have a formal position as such");
        yield return new BasicNodeType(2, "inter-organizational relation type", "Defines the type of relation between two organizations");
        yield return new BasicNodeType(3, "party political entity relation type", "Defines the type of relation between a person or organization and a political entity");
        yield return new BasicNodeType(4, "person organization relation type", "Defines the type of relation between a person and an organization");
        yield return new BasicNodeType(5, "inter-personal relationship type", "Defines the type of relation between a person and another person");
        yield return new BasicNodeType(6, "profession", "The type of professions a person can have");
        yield return new BasicNodeType(7, "denomination", "The denomination of an organization");
        yield return new BasicNodeType(8, "Hague status", "The hague status of an adoption agency");
        yield return new BasicNodeType(9, "document type", "Defines the type of a document");
        yield return new BasicNodeType(10, "document", "A text based document");
        yield return new BasicNodeType(11, "first level global region", "First level subdivision of the world");
        yield return new BasicNodeType(12, "secomnd level global region", "Second level subdivision of the world");
        yield return new BasicNodeType(13, "basic country", "Countries that don't contain other countries and that are not part of another country");
        yield return new BasicNodeType(14, "bound country", "Countries that are part of another country");
        yield return new BasicNodeType(15, "country and first and bottom level subdivision", "Countries that are also first level subdivisions of another country and that allows no further subdivision");
        yield return new BasicNodeType(16, "country and first and second level subdivision", "Countries that are also first and second level subdivisions of another country");
        yield return new BasicNodeType(17, "first and bottom level subdivision", "Subdivision of a country that contains no further subdivisions");
        yield return new BasicNodeType(18, "informal intermediate level subdivision", "Informal subdivision of a country that contains second level subdivisions");
        yield return new BasicNodeType(19, "basic second level subdivision", "Second level subdivision of a country");
        yield return new BasicNodeType(20, "binding country", "Country that contains other countries");
        yield return new BasicNodeType(21, "country and intermediate level subdivision", "Countries that are also first level subdivisions of another country and that do allow further subdivision");
        yield return new BasicNodeType(22, "formal intermediate level subdivision", "Formal subdivision of a country that contains second level subdivisions");
        yield return new BasicNodeType(23, "organization", "A collection of people working together. Therefore a bill or a trip is also regarderd an organization, even though it does not have a formal position as such.");
        yield return new BasicNodeType(24, "person", "Person");
        yield return new BasicNodeType(27, "child placement type", "Defined the type of a child placement");
        yield return new BasicNodeType(28, "family size", "Defined the type family size");
        yield return new BasicNodeType(35, "blog post", "Blog post");
        yield return new BasicNodeType(36, "article", "Article");
        yield return new BasicNodeType(37, "discussion", "Discussion");
        yield return new BasicNodeType(38, "vocabulary", "A set of terms");
        yield return new BasicNodeType(39, "type of abuse", "Defines the types of abuse a child has endured");
        yield return new BasicNodeType(40, "type of abuser", "Defines the relationship the abuser has with respect to the abused");
        yield return new BasicNodeType(41, "basic nameable", "Can be used as a term without having additional data");
        yield return new BasicNodeType(42, "page", "A simpe text node");
        yield return new BasicNodeType(43, "review", "A book review");
        
        yield return new BasicNodeType(45, "inter country relation", "A relation between two countries");
        yield return new BasicNodeType(46, "inter personal relation", "A relation between two persons");
        yield return new BasicNodeType(47, "inter organizational relation", "A relation between two organizations");
        yield return new BasicNodeType(48, "person organization relation", "A relation between a person and an organization");
        yield return new BasicNodeType(49, "party political entity relation", "A relation between a person and an organization");
        yield return new BasicNodeType(50, "inter country relation type", "The type of relation two countries can have");
        yield return new BasicNodeType(51, "subdivision type", "The type of a subdivision of a country can have");
        yield return new BasicNodeType(52, "united states congressional meeting", "The two year period the United States congress comes together");
        yield return new BasicNodeType(53, "single question poll", "Poll that asks a single question");
        yield return new BasicNodeType(54, "multi question poll", "Poll that asks a multiple questions");
        yield return new BasicNodeType(55, "poll question", "Poll question");
    }

}
