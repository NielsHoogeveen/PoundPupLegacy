﻿namespace PoundPupLegacy.Convert;

internal sealed class NodeTypeMigrator : PPLMigrator
{
    protected override string Name => "node types";

    public NodeTypeMigrator(MySqlToPostgresConverter converter) : base(converter)
    {

    }
    protected override async Task MigrateImpl()
    {
        await new NodeTypeCreator().CreateAsync(GetNodeTypes(), _postgresConnection);
        await new CreateNodeActionCreator().CreateAsync(GetNodeTypes().Select(x => new CreateNodeAction { Id = null, NodeTypeId = x.Id!.Value }), _postgresConnection);
        await new DeleteNodeActionCreator().CreateAsync(GetNodeTypes().Select(x => new DeleteNodeAction { Id = null, NodeTypeId = x.Id!.Value }), _postgresConnection);
        await new EditNodeActionCreator().CreateAsync(GetNodeTypes().Select(x => new EditNodeAction { Id = null, NodeTypeId = x.Id!.Value }), _postgresConnection);

    }
    internal static async IAsyncEnumerable<BasicNodeType> GetNodeTypes()
    {
        await Task.CompletedTask;
        yield return new BasicNodeType(1, "organization type", "Organizations are loosely defined as something a collection of people work together. Therefore a bill or a trip is also regarderd an organization, even though it does not have a formal position as such", false);
        yield return new BasicNodeType(2, "inter-organizational relation type", "Defines the type of relation between two organizations", false);
        yield return new BasicNodeType(3, "party political entity relation type", "Defines the type of relation between a person or organization and a political entity", false);
        yield return new BasicNodeType(4, "person organization relation type", "Defines the type of relation between a person and an organization", false);
        yield return new BasicNodeType(5, "inter-personal relationship type", "Defines the type of relation between a person and another person", false);
        yield return new BasicNodeType(6, "profession", "The type of professions a person can have", false);
        yield return new BasicNodeType(7, "denomination", "The denomination of an organization", false);
        yield return new BasicNodeType(8, "Hague status", "The hague status of an adoption agency", false);
        yield return new BasicNodeType(9, "document type", "Defines the type of a document", false);
        yield return new BasicNodeType(10, "document", "A text based document", false);
        yield return new BasicNodeType(11, "first level global region", "First level subdivision of the world", false);
        yield return new BasicNodeType(12, "second level global region", "Second level subdivision of the world", false);
        yield return new BasicNodeType(13, "basic country", "Countries that don't contain other countries and that are not part of another country", false);
        yield return new BasicNodeType(14, "bound country", "Countries that are part of another country", false);
        yield return new BasicNodeType(15, "country and first and bottom level subdivision", "Countries that are also first level subdivisions of another country and that allows no further subdivision", false);
        yield return new BasicNodeType(16, "country and first and second level subdivision", "Countries that are also first and second level subdivisions of another country", false);
        yield return new BasicNodeType(17, "first and bottom level subdivision", "Subdivision of a country that contains no further subdivisions", false);
        yield return new BasicNodeType(18, "informal intermediate level subdivision", "Informal subdivision of a country that contains second level subdivisions", false);
        yield return new BasicNodeType(19, "basic second level subdivision", "Second level subdivision of a country", false);
        yield return new BasicNodeType(20, "binding country", "Country that contains other countries", false);
        yield return new BasicNodeType(21, "country and intermediate level subdivision", "Countries that are also first level subdivisions of another country and that do allow further subdivision", false);
        yield return new BasicNodeType(22, "formal intermediate level subdivision", "Formal subdivision of a country that contains second level subdivisions", false);
        yield return new BasicNodeType(23, "organization", "A collection of people working together. Therefore a bill or a trip is also regarderd an organization, even though it does not have a formal position as such.", false);
        yield return new BasicNodeType(24, "person", "Person", false);
        yield return new BasicNodeType(27, "child placement type", "Defined the type of a child placement", false);
        yield return new BasicNodeType(28, "family size", "Defined the type family size", false);
        yield return new BasicNodeType(35, "blog post", "Blog post", true);
        yield return new BasicNodeType(36, "article", "Article", true);
        yield return new BasicNodeType(37, "discussion", "Discussion", true);
        yield return new BasicNodeType(38, "vocabulary", "A set of terms", false);
        yield return new BasicNodeType(39, "type of abuse", "Defines the types of abuse a child has endured", false);
        yield return new BasicNodeType(40, "type of abuser", "Defines the relationship the abuser has with respect to the abused", false);
        yield return new BasicNodeType(41, "basic nameable", "Can be used as a term without having additional data", false);
        yield return new BasicNodeType(42, "page", "A simpe text node", false);
        yield return new BasicNodeType(43, "review", "A book review", false);

        yield return new BasicNodeType(45, "inter country relation", "A relation between two countries", false);
        yield return new BasicNodeType(46, "inter personal relation", "A relation between two persons", false);
        yield return new BasicNodeType(47, "inter organizational relation", "A relation between two organizations", false);
        yield return new BasicNodeType(48, "person organization relation", "A relation between a person and an organization", false);
        yield return new BasicNodeType(49, "party political entity relation", "A relation between a person and an organization", false);
        yield return new BasicNodeType(50, "inter country relation type", "The type of relation two countries can have", false);
        yield return new BasicNodeType(51, "subdivision type", "The type of a subdivision of a country can have", false);
        yield return new BasicNodeType(52, "united states congressional meeting", "The two year period the United States congress comes together", false);
        yield return new BasicNodeType(53, "single question poll", "Poll that asks a single question", false);
        yield return new BasicNodeType(54, "multi question poll", "Poll that asks a multiple questions", false);
        yield return new BasicNodeType(55, "poll question", "Poll question", false);
        yield return new BasicNodeType(56, "house bill", "A bill that is introduced in the US house of representatives", false);
        yield return new BasicNodeType(57, "senate bill", "A bill that is introduced in the US senate", false);
        yield return new BasicNodeType(58, "bill action type", "The type of actions a person can take with respect to a bill", false);
        yield return new BasicNodeType(59, "senator", "Represents a United States senator", false);
        yield return new BasicNodeType(60, "representative", "Represents a member of the United States House of Representatives", false);
        yield return new BasicNodeType(61, "basic profession", "A profession", false);
        yield return new BasicNodeType(62, "united states political party affilition", "Political party affiliations as used in the United States of America", false);
        yield return new BasicNodeType(63, "united states political party", "Political party of the United States of America", false);
        yield return new BasicNodeType(64, "congressional term political party affiliation", "The political party affiliation of a member of congresss during a term", false);
        yield return new BasicNodeType(65, "senate term", "A term of a United States Senator", false);
        yield return new BasicNodeType(66, "house term", "A term of a United States Representative", false);
    }

}
