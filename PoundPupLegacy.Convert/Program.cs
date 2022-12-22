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
                new NodeType(10, "document inclusion type", "Rules with respect to attachments"),
                new NodeType(11, "first level global region", "First level subdivision of the world"),
                new NodeType(12, "region", "regions of the continents"),
                new NodeType(13, "top level country", "countries"),
                new NodeType(14, "bound country", "countries that are part of another country"),
                new NodeType(15, "subdivision country", "countries that are both top level countries and sub divisions of another country"),
                new NodeType(16, "subdivision region country", "countries that are both top level countries and sub divisions of another country and regions within that country"),
                new NodeType(17, "direct subdivision", "Subdivision of a country"),
                new NodeType(18, "country region", "Region of a country"),
                new NodeType(19, "regional subdivision", "Subdivision of a region of a country"),
                new NodeType(20, "binding country", "Country that contains other countries"),

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
                AddNodeTypes(postgresqlconnection);
                //MigrateUsers(mysqlconnection, postgresqlconnection);
                MigrateSelectionOptions(mysqlconnection, postgresqlconnection, 12622, 1, "organization_type");
                MigrateSelectionOptions(mysqlconnection, postgresqlconnection, 12637, 2, "affiliation_type");
                MigrateSelectionOptions(mysqlconnection, postgresqlconnection, 12652, 3, "political_entity_relation_type");
                MigrateSelectionOptions(mysqlconnection, postgresqlconnection, 12663, 4, "position_type");
                MigrateSelectionOptions(mysqlconnection, postgresqlconnection, 16900, 5, "personal_relationship_type");
                MigrateSelectionOptions(mysqlconnection, postgresqlconnection, 27213, 6, "profession");
                MigrateSelectionOptions(mysqlconnection, postgresqlconnection, 39428, 7, "denomination");
                MigrateSelectionOptions(mysqlconnection, postgresqlconnection, 41212, 8, "hague_status");
                MigrateSelectionOptions(mysqlconnection, postgresqlconnection, 42416, 9, "document_type");
                MigrateSelectionOptions(mysqlconnection, postgresqlconnection, 42422, 10, "document_inclusion_type");
                MigrateFirstLevelGlobalRegions(mysqlconnection, postgresqlconnection);
                MigrateSecondLevelGlobalRegions(mysqlconnection, postgresqlconnection);
                MigrateBasicCountries(mysqlconnection, postgresqlconnection);
                MigrateBindingCountries(mysqlconnection, postgresqlconnection);
                MigrateBoundCountries(mysqlconnection, postgresqlconnection);
                MigrateBasicCountryAndFirstLevelSubdivisions(mysqlconnection, postgresqlconnection);
                //MigrateRegionSubdivisionCountries(mysqlconnection, postgresqlconnection);
                //MigrateSubdivisions(mysqlconnection, postgresqlconnection);
                //MigrateCountryRegions(mysqlconnection, postgresqlconnection);
                //MigrateRegionSubdivisions(mysqlconnection, postgresqlconnection);
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
