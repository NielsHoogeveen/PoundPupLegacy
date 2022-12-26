using MySqlConnector;
using Npgsql;

namespace PoundPupLegacy.Convert
{
    internal partial class Program
    {

        private static int NodeId = 100000;

        const string ConnectionStringMariaDb = "server=localhost;userid=root;Password=niels;database=ppldb";

        const string ConnectStringPostgresql = "Host=localhost;Username=postgres;Password=niels;Database=ppl;Include Error Detail=True";

        private record NodeType(int Id, string Name, string Description);

        private static void AddNodeTypes(NpgsqlConnection postgresqlConnection)
        {
            var nodeTypes = new NodeType[]
            {
                new NodeType(1, "organization type", "Organizations are loosely defined as something a collection of people work together. Therefore a bill or a trip is also regarderd an organization, even though it does not have a formal position as such"),
                new NodeType(2, "affiliation type", "Defines the type of relation between two organizations"),
                new NodeType(3, "political entity relation type", "Defines the type of relation between a person or organization and a political entity"),
                new NodeType(4, "position type", "Defines the type of relation between a person and an organization"),
                new NodeType(5, "personal relationship type", "Defines the type of relation between a person and another person"),
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
                new NodeType(25, "attachment therapist", "Person who practices attachment therapy"),
                new NodeType(26, "abuse case", "Abuse case of a child that has been placed by court"),
                new NodeType(27, "child placement type", "Defined the type of a child placement"),
                new NodeType(28, "family size", "Defined the type family size"),
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
                command.ExecuteNonQuery();
            }

        }

        private static void Migrate()
        {
            try
            {
                using var mysqlconnection = new MySqlConnection(ConnectionStringMariaDb);
                using var postgresqlconnection = new NpgsqlConnection(ConnectStringPostgresql);
                mysqlconnection.Open();
                postgresqlconnection.Open();
                //MigrateUsers(mysqlconnection, postgresqlconnection);
                MigrateFiles(mysqlconnection, postgresqlconnection);
                AddNodeTypes(postgresqlconnection);
                MigrateSelectionOptions(mysqlconnection, postgresqlconnection, 12622, 1, "organization_type");
                MigrateSelectionOptions(mysqlconnection, postgresqlconnection, 12637, 2, "affiliation_type");
                MigrateSelectionOptions(mysqlconnection, postgresqlconnection, 12652, 3, "political_entity_relation_type");
                MigrateSelectionOptions(mysqlconnection, postgresqlconnection, 12663, 4, "position_type");
                MigrateSelectionOptions(mysqlconnection, postgresqlconnection, 16900, 5, "personal_relationship_type");
                MigrateSelectionOptions(mysqlconnection, postgresqlconnection, 27213, 6, "profession");
                MigrateSelectionOptions(mysqlconnection, postgresqlconnection, 39428, 7, "denomination");
                MigrateSelectionOptions(mysqlconnection, postgresqlconnection, 41212, 8, "hague_status");
                MigrateSelectionOptions(mysqlconnection, postgresqlconnection, 42416, 9, "document_type");
                MigrateFirstLevelGlobalRegions(mysqlconnection, postgresqlconnection);
                MigrateSecondLevelGlobalRegions(mysqlconnection, postgresqlconnection);
                MigrateBasicCountries(mysqlconnection, postgresqlconnection);
                MigrateBindingCountries(mysqlconnection, postgresqlconnection);
                MigrateBoundCountries(mysqlconnection, postgresqlconnection);
                MigrateBasicCountryAndFirstLevelSubdivisions(mysqlconnection, postgresqlconnection);
                MigrateCountryAndFirstAndSecondLevelSubdivisions(mysqlconnection, postgresqlconnection);
                MigrateFirstAndBottomLevelSubdivisions(mysqlconnection, postgresqlconnection);
                MigrateInformalIntermediateLevelSubdivisions(mysqlconnection, postgresqlconnection);
                MigrateFormalIntermediateLevelSubdivisions(mysqlconnection, postgresqlconnection);
                MigrateBasicSecondLevelSubdivisions(mysqlconnection, postgresqlconnection);
                MigrateOrganizations(mysqlconnection, postgresqlconnection);
                MigratePersons(mysqlconnection, postgresqlconnection);
                MigrateAttachmentTherapists(mysqlconnection, postgresqlconnection);
                MigrateLocations(mysqlconnection, postgresqlconnection);
                MigrateDocuments(mysqlconnection, postgresqlconnection);
                MigrateChildPlacementTypes(postgresqlconnection);
                MigrateFamilySizes(postgresqlconnection);
                MigrateAbuseCases(mysqlconnection, postgresqlconnection);
                mysqlconnection.Close();
                postgresqlconnection.Close();
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

        static void Main(string[] args)
        {

            //foreach (var s in ReadDirectSubDivisionCsv())
            //{
            //    System.Console.WriteLine($"{s.Id},{s.CountryId},{s.ISO3166_2Code}");
            //}
            Migrate();
        }
    }
}
