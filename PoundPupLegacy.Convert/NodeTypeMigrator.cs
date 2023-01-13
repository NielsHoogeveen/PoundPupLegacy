using PoundPupLegacy.Model;
using PoundPupLegacy.Db;

namespace PoundPupLegacy.Convert;

internal sealed class NodeTypeMigrator : Migrator
{
    protected override string Name => "node types";

    public NodeTypeMigrator(MySqlToPostgresConverter converter): base(converter)
    {

    }
    protected override async Task MigrateImpl()
    {
        await NodeTypeCreator.CreateAsync(GetNodeTypes(), _postgresConnection);
    }

    private static async IAsyncEnumerable<NodeType> GetNodeTypes()
    {
        await Task.CompletedTask;
        yield return new NodeType(1, "organization type", "Organizations are loosely defined as something a collection of people work together. Therefore a bill or a trip is also regarderd an organization, even though it does not have a formal position as such");
        yield return new NodeType(2, "inter-organizational relation type", "Defines the type of relation between two organizations");
        yield return new NodeType(3, "political entity relation type", "Defines the type of relation between a person or organization and a political entity");
        yield return new NodeType(4, "person organization relation type", "Defines the type of relation between a person and an organization");
        yield return new NodeType(5, "inter-personal relationship type", "Defines the type of relation between a person and another person");
        yield return new NodeType(6, "profession", "The type of professions a person can have");
        yield return new NodeType(7, "denomination", "The denomination of an organization");
        yield return new NodeType(8, "Hague status", "The hague status of an adoption agency");
        yield return new NodeType(9, "document type", "Defines the type of a document");
        yield return new NodeType(10, "document", "A text based document");
        yield return new NodeType(11, "first level global region", "First level subdivision of the world");
        yield return new NodeType(12, "secomnd level global region", "Second level subdivision of the world");
        yield return new NodeType(13, "basic country", "Countries that don't contain other countries and that are not part of another country");
        yield return new NodeType(14, "bound country", "Countries that are part of another country");
        yield return new NodeType(15, "country and first and bottom level subdivision", "Countries that are also first level subdivisions of another country and that allows no further subdivision");
        yield return new NodeType(16, "country and first and second level subdivision", "Countries that are also first and second level subdivisions of another country");
        yield return new NodeType(17, "first and bottom level subdivision", "Subdivision of a country that contains no further subdivisions");
        yield return new NodeType(18, "informal intermediate level subdivision", "Informal subdivision of a country that contains second level subdivisions");
        yield return new NodeType(19, "basic second level subdivision", "Second level subdivision of a country");
        yield return new NodeType(20, "binding country", "Country that contains other countries");
        yield return new NodeType(21, "country and intermediate level subdivision", "Countries that are also first level subdivisions of another country and that do allow further subdivision");
        yield return new NodeType(22, "formal intermediate level subdivision", "Formal subdivision of a country that contains second level subdivisions");
        yield return new NodeType(23, "organization", "A collection of people working together. Therefore a bill or a trip is also regarderd an organization, even though it does not have a formal position as such.");
        yield return new NodeType(24, "person", "Person");
        yield return new NodeType(26, "abuse case", "Abuse case of a child that has been placed by court");
        yield return new NodeType(27, "child placement type", "Defined the type of a child placement");
        yield return new NodeType(28, "family size", "Defined the type family size");
        yield return new NodeType(29, "child trafficking case", "Trafficking case of children to be adopted");
        yield return new NodeType(30, "coerced adoption case", "Adoption that involved coercion");
        yield return new NodeType(31, "deportation case", "Adoptees deported to country of origin");
        yield return new NodeType(32, "father's rights violation case", "Adoptions where the rights of the biological father were violated");
        yield return new NodeType(33, "wrongful medication case", "Child placement situation where wrongful medication is present");
        yield return new NodeType(34, "wrongful removal case", "Children wrongfully removed from their family");
        yield return new NodeType(35, "blog post", "Blog post");
        yield return new NodeType(36, "article", "Article");
        yield return new NodeType(37, "discussion", "Discussion");
        yield return new NodeType(38, "vocabulary", "A set of terms");
        yield return new NodeType(39, "type of abuse", "Defines the types of abuse a child has endured");
        yield return new NodeType(40, "type of abuser", "Defines the relationship the abuser has with respect to the abused");
        yield return new NodeType(41, "basic nameable", "Can be used as a term without having additional data");
        yield return new NodeType(42, "page", "A simpe text node");
        yield return new NodeType(43, "review", "A book review");
        yield return new NodeType(44, "disrupted placement case", "A situation where the placement of a child was reverted");
    }

}
