﻿using MySqlConnector;
using Npgsql;
using PoundPupLegacy.Model;

namespace PoundPupLegacy.Convert;

internal partial class Program
{

    private static int NodeId = 100000;

    const string ConnectionStringMariaDb = "server=localhost;userid=root;Password=niels;database=ppldb";

    const string ConnectStringPostgresql = "Host=localhost;Username=postgres;Password=niels;Database=ppl;Include Error Detail=True";

    private record NodeType(int Id, string Name, string Description);

    private static async Task TruncateDatabase(NpgsqlConnection postgresqlConnection)
    {
        var sql = """
            TRUNCATE node 
            RESTART IDENTITY
            CASCADE;
            TRUNCATE principal 
            RESTART IDENTITY
            CASCADE;
            TRUNCATE user_group 
            RESTART IDENTITY
            CASCADE;
            TRUNCATE node_type 
            RESTART IDENTITY
            CASCADE;
            """;
        using var command = postgresqlConnection.CreateCommand();
        command.CommandType = System.Data.CommandType.Text;
        command.CommandText = sql;
        await command.ExecuteNonQueryAsync();
    }
    private static async Task AddNodeTypes(NpgsqlConnection postgresqlConnection)
    {
        var nodeTypes = new NodeType[]
        {
            new NodeType(1, "organization type", "Organizations are loosely defined as something a collection of people work together. Therefore a bill or a trip is also regarderd an organization, even though it does not have a formal position as such"),
            new NodeType(2, "inter-organizational relation type", "Defines the type of relation between two organizations"),
            new NodeType(3, "political entity relation type", "Defines the type of relation between a person or organization and a political entity"),
            new NodeType(4, "person organization relation type", "Defines the type of relation between a person and an organization"),
            new NodeType(5, "inter-personal relationship type", "Defines the type of relation between a person and another person"),
            new NodeType(6, "profession", "The type of professions a person can have"),
            new NodeType(7, "denomination", "The denomination of an organization"),
            new NodeType(8, "Hague status", "The hague status of an adoption agency"),
            new NodeType(9, "document type", "Defines the type of a document"),
            new NodeType(10, "document", "A text based document"),
            new NodeType(11, "first level global region", "First level subdivision of the world"),
            new NodeType(12, "secomnd level global region", "Second level subdivision of the world"),
            new NodeType(13, "basic country", "Countries that don't contain other countries and that are not part of another country"),
            new NodeType(14, "bound country", "Countries that are part of another country"),
            new NodeType(15, "country and first and bottom level subdivision", "Countries that are also first level subdivisions of another country and that allows no further subdivision"),
            new NodeType(16, "country and first and second level subdivision", "Countries that are also first and second level subdivisions of another country"),
            new NodeType(17, "first and bottom level subdivision", "Subdivision of a country that contains no further subdivisions"),
            new NodeType(18, "informal intermediate level subdivision", "Informal subdivision of a country that contains second level subdivisions"),
            new NodeType(19, "basic second level subdivision", "Second level subdivision of a country"),
            new NodeType(20, "binding country", "Country that contains other countries"),
            new NodeType(21, "country and intermediate level subdivision", "Countries that are also first level subdivisions of another country and that do allow further subdivision"),
            new NodeType(22, "formal intermediate level subdivision", "Formal subdivision of a country that contains second level subdivisions"),
            new NodeType(23, "organization", "A collection of people working together. Therefore a bill or a trip is also regarderd an organization, even though it does not have a formal position as such."),
            new NodeType(24, "person", "Person"),
            new NodeType(26, "abuse case", "Abuse case of a child that has been placed by court"),
            new NodeType(27, "child placement type", "Defined the type of a child placement"),
            new NodeType(28, "family size", "Defined the type family size"),
            new NodeType(29, "child trafficking case", "Trafficking case of children to be adopted"),
            new NodeType(30, "coerced adoption case", "Adoption that involved coercion"),
            new NodeType(31, "deportation case", "Adoptees deported to country of origin"),
            new NodeType(32, "father's rights violation case", "Adoptions where the rights of the biological father were violated"),
            new NodeType(33, "wrongful medication case", "Child placement situation where wrongful medication is present"),
            new NodeType(34, "wrongful removal case", "Children wrongfully removed from their family"),
            new NodeType(35, "blog post", "Blog post"),
            new NodeType(36, "article", "Article"),
            new NodeType(37, "discussion", "Discussion"),
            new NodeType(38, "vocabulary", "A set of terms"),
            new NodeType(39, "type of abuse", "Defines the types of abuse a child has endured"),
            new NodeType(40, "type of abuser", "Defines the relationship the abuser has with respect to the abused"),
            new NodeType(41, "basic nameable", "Can be used as a term without having additional data"),
            new NodeType(42, "page", "A simpe text node"),
            new NodeType(43, "review", "A book review"),
            new NodeType(44, "disrupted placement case", "A situation where the placement of a child was reverted"),
        };


        using var command = postgresqlConnection.CreateCommand();
        command.CommandType = System.Data.CommandType.Text;
        command.CommandText = "INSERT INTO node_type (id, name, description) values(@id, @name, @description)";
        command.Parameters.Add("id", NpgsqlTypes.NpgsqlDbType.Integer);
        command.Parameters.Add("name", NpgsqlTypes.NpgsqlDbType.Varchar);
        command.Parameters.Add("description", NpgsqlTypes.NpgsqlDbType.Varchar);
        command.Prepare();
        foreach (var nodeType in nodeTypes)
        {
            command.Parameters["id"].Value = nodeType.Id;
            command.Parameters["name"].Value = nodeType.Name;
            command.Parameters["description"].Value = nodeType.Description;
            await command.ExecuteNonQueryAsync();
        }
    }
    const int COLORADO_ADOPTION_CENTER = 105;
    const int ADOPTION = 4145;
    const int FOSTER_CARE = 4342;
    const int TO_BE_ADOPTED = 108;
    const int LEGAL_GUARDIANSHIP = 4346;
    const int INSTITUTION = 19384;
    const int ONE_TO_FOUR = 111;
    const int FOUR_TO_EIGHT = 112;
    const int EIGHT_TO_TWELVE = 113;
    const int MORE_THAN_TWELVE = 19392;
    const int CHILD_PLACEMENT_TYPE = 115;
    const int TYPE_OF_ABUSER = 116;
    const int FAMILY_SIZE = 117;

    const int NON_LETHAL_PHYSICAL_ABUSE = 118;
    const int LETHAL_PHYSICAL_ABUSE = 119;
    const int PHYSICAL_EXPLOITATION = 120;
    const int SEXUAL_ABUSE = 6979;
    const int SEXUAL_EXPLOITATION = 23662;
    const int NON_LETHAL_NEGLECT = 123;
    const int LETHAL_NEGLECT = 74428;
    const int NON_LETHAL_DEPRIVATION = 125;
    const int LETHAL_DEPRIVATION = 74533;
    const int ECONOMIC_EXPLOITATION = 44447;
    const int VERBAL_ABUSE = 74006;
    const int MEDICAL_ABUSE = 73882;
    const int DEATH_BY_UNKNOWN_CAUSE = 130;

    const int ADOPTIVE_FATHER = 131;
    const int FOSTER_FATHER = 132;
    const int ADOPTIVE_MOTHER = 7990;
    const int FOSTER_MOTHER = 134;
    const int LEGAL_GUARDIAN = 40242;
    const int ADOPTED_SIBLING = 136;
    const int FOSTER_SIBLING = 137;
    const int NON_ADOPTED_SIBLING = 138;
    const int NON_FOSTERED_SIBLING = 139;
    const int OTHER_FAMILY_MEMBER = 140;
    const int OTHER_NON_FAMILY_MEMBER = 141;
    const int UNDETERMINED = 142;
    const int TYPE_OF_ABUSE = 143;

    const int ANTIGUA_AND_BARBUDA = 4073;
    const int PALESTINE = 4082;
    const int SAINT_HELENA_ETC = 4087;
    const int SOUTH_SUDAN = 4093;
    const int SAINT_BARTH = 4097;
    const int SAINT_MARTIN = 4102;
    const int FRENCH_SOUTHERN_TERRITORIES = 4106;
    const int UNITED_STATES_MINOR_OUTLYING_ISLANDS = 4119;
    const int ALAND = 4128;
    const int CURACAO = 4129;
    const int SINT_MAARTEN = 4130;

    
    const int DOCUMENT_TYPES = 42416;
    const int ORGANIZATION_TYPE = 12622;

    private static async Task Migrate()
    {
        try
        {
            using var mysqlconnection = new MySqlConnection(ConnectionStringMariaDb);
            using var connection = new NpgsqlConnection(ConnectStringPostgresql);
            await mysqlconnection.OpenAsync();
            await connection.OpenAsync();
            //await MigratePublicationStatuses(connection);
            //await MigrateFiles(mysqlconnection, connection);

            //await TruncateDatabase(connection);
            //await AddNodeTypes(connection);
            //await MigrateUsers(mysqlconnection, connection);
            //await MigrateVocabularies(mysqlconnection, connection);
            //await MigrateBasicNameables(mysqlconnection, connection);
            //await MigrateChildPlacementTypes(mysqlconnection, connection);
            //await MigrateOrganizationTypes(mysqlconnection, connection);
            //await MigrateInterOrganizationalRelationTypes(mysqlconnection, connection);
            //await MigrateInterPersonalRelationTypes(mysqlconnection, connection);
            //await MigratePoliticalEntityRelationTypes(mysqlconnection, connection);
            //await MigratePersonOrganizationRelationTypes(mysqlconnection, connection);
            //await MigrateTypesOfAbuse(mysqlconnection, connection);
            //await MigrateTypesOfAbusers(mysqlconnection, connection);
            //await MigrateFamilySizes(connection);
            //await MigrateProfessions(mysqlconnection, connection);
            //await MigrateDenominations(mysqlconnection, connection);
            //await MigrateHagueStatuses(mysqlconnection, connection);
            //await MigrateDocumentTypes(mysqlconnection, connection);
            //await MigrateFirstLevelGlobalRegions(mysqlconnection, connection);
            //await MigrateSecondLevelGlobalRegions(mysqlconnection, connection);
            //await MigrateBasicCountries(mysqlconnection, connection);
            //await MigrateBindingCountries(mysqlconnection, connection);
            //await MigrateBoundCountries(mysqlconnection, connection);
            //await MigrateCountryAndFirstLevelSubdivisions(mysqlconnection, connection);
            //await MigrateCountryAndFirstAndSecondLevelSubdivisions(mysqlconnection, connection);
            //await MigrateFirstAndBottomLevelSubdivisions(mysqlconnection, connection);
            //await MigrateInformalIntermediateLevelSubdivisions(mysqlconnection, connection);
            //await MigrateFormalIntermediateLevelSubdivisions(mysqlconnection, connection);
            //await MigrateBasicSecondLevelSubdivisions(mysqlconnection, connection);
            //await MigrateBlogPosts(mysqlconnection, connection);
            //await MigrateArticles(mysqlconnection, connection);
            //await MigrateDiscussions(mysqlconnection, connection);
            //await MigrateAdoptionExports(mysqlconnection, connection);
            //await MigrateDocuments(mysqlconnection, connection);
            //await MigrateOrganizations(mysqlconnection, connection);
            //await MigratePersons(mysqlconnection, connection);
            //await MigrateAbuseCases(mysqlconnection, connection);
            //await MigrateChildTraffickingCases(mysqlconnection, connection);
            //await MigrateCoercedAdoptionCases(mysqlconnection, connection);
            //await MigrateDisruptedPlacementCases(mysqlconnection, connection);
            //await MigrateDeportationCases(mysqlconnection, connection);
            //await MigrateFathersRightsViolationCases(mysqlconnection, connection);
            //await MigrateWrongfulMedicationCases(mysqlconnection, connection);
            //await MigrateWrongfulRemovalCases(mysqlconnection, connection);
            //await MigrateLocations(mysqlconnection, connection);
            //await MigratePages(mysqlconnection, connection);
            //await MigrateReviews(mysqlconnection, connection);
            //await MigrateNodeTerms(mysqlconnection, connection);
            //await MigrateComments(mysqlconnection, connection);


            await mysqlconnection.CloseAsync();
            await connection.CloseAsync();
        }
        catch (MySqlException ex)
        {
            Console.WriteLine(ex.ToString());
            return;
        }
        catch (PostgresException ex)
        {
            Console.WriteLine(ex.ToString());
            return;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return;
        }
        finally
        {

        }
    }
    public static string CreateMD5(string input)
    {
        // Use input string to calculate MD5 hash
        using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
        {
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            return System.Convert.ToHexString(hashBytes); // .NET 5 +

            // Convert the byte array to hexadecimal string prior to .NET 5
            // StringBuilder sb = new System.Text.StringBuilder();
            // for (int i = 0; i < hashBytes.Length; i++)
            // {
            //     sb.Append(hashBytes[i].ToString("X2"));
            // }
            // return sb.ToString();
        }
    }
    static async Task Main(string[] args)
    {
        //Console.WriteLine(CreateMD5("wellhung"));
        await Migrate();
    }
}
