using MySqlConnector;
using Npgsql;
using NpgsqlTypes;
using PoundPupLegacy.Db;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert;

internal partial class Program
{


    private static int GetAdoptionExportRelationId(int countryIdTo, int? countryIdFrom, string? countryNameFrom, NpgsqlConnection connection)
    {
        const string sql1 = """
        SELECT id FROM adoption_export_relation WHERE country_id_to = @country_id_to AND country_id_from = @country_id_from AND country_name_from IS NULL
        """;
        const string sql2 = """
        SELECT id FROM adoption_export_relation WHERE country_id_to = @country_id_to AND country_id_from IS NULL AND country_name_from = @country_name_from
        """;
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = countryIdFrom is null ? sql2 : sql1;

        command.Parameters.Add("country_id_to", NpgsqlDbType.Integer);
        if (countryIdFrom is null)
        {
            command.Parameters.Add("country_name_from", NpgsqlDbType.Varchar);
        }
        else
        {
            command.Parameters.Add("country_id_from", NpgsqlDbType.Integer);
        }

        command.Prepare();
        command.Parameters["country_id_to"].Value = countryIdTo;
        if (countryIdFrom is not null)
        {
            command.Parameters["country_id_from"].Value = countryIdFrom;
        }
        else
        {
            command.Parameters["country_name_from"].Value = countryNameFrom;
        }
        var reader = command.ExecuteReader();

        reader.Read();
        var id = reader.GetInt32(0);
        reader.Close();
        return id;
    }

    private static async Task MigrateAdoptionExports(MySqlConnection mysqlconnection, NpgsqlConnection connection)
    {
        await AdoptionExportRelationCreator.CreateAsync(ReadAdoptionExportRelations(mysqlconnection), connection);
        await AdoptionExportYearCreator.CreateAsync(ReadAdoptionExportYears(mysqlconnection, connection), connection);
    }
    private static IEnumerable<AdoptionExportRelation> ReadAdoptionExportRelations(MySqlConnection mysqlconnection)
    {

        var sql = $"""
                SELECT
                DISTINCT
                n.nid country_id_to,
                n.country_id country_id_from,
                case when country_id IS NULL then country_name END country_name_from
                FROM
                (
                SELECT DISTINCT
                n.nid,
                n.vid,
                m.`row`,
                case 
                    when m.value = '<a href="/laos">Laos</a>' then 3967
                    when m.value = '<a href="/guyana">Guyana</a>' then 3927
                    when m.value = '<a href="/burma">Burma</a>' then 3969
                    when m.value = '<a href="/myamar">Myamar</a>' then 3969
                    when m.value = '<a href="/domican_repubic">Domican Repubic</a>' then 3900
                    when m.value = 'Palestine Authority' then 4082
                    when m.value = '<a href="/palestine_authorities">Palestine Authorities</a>' then 4082
                    when m.value = 'Gaza Strip' then 4082
                    when m.value = '<a href="/tunesia">Tunesia</a>' then 3834
                    when m.value = '<a href="/bosnia_herzigovina">Bosnia and Herzegovina Federation</a>' then 3999
                    when m.value = '<a href="/united kingdom">United Kingdom</a>' then 6185
                    when m.value = '<a href="/cote_ivoir">Côte d’Ivoire</a>' then 3839
                    when m.value = '<a href="/cote_ivoir">Cote d\'Ivoire</a>' then 3839
                    when m.value = '<a href="/laos">Laos</a>' then 3967
                    when m.value = '<a href="/morrocco">Morrocco</a>' then 3832
                    when m.value = '<a href="/guyana">Guyana</a>' then 3927
                    when m.value = '<a href="/antigua_and_barbuda ">Antigua and Barbuda</a>' then 4073
                    when m.value = '<a href="/antigua_and_barbuda">Antigua and Barbuda</a>' then 4073
                    when m.value = '<a href="czeck_republic">Czeck Replublic</a>' then 4029
                    when m.value = '<a href="/surinam">Surinam</a>' then 3930
                    when m.value = '<a href="/node/3857">Democratic Republic of The Congo</a>' then 3857
                    when m.value = '<a href="/mauretania">Mauretania</a>' then 3846
                    when m.value = '<a href="/marhsall_islands">Marshall Islands</a>' then 4050
                    when m.value = '<a href="/kyrgystan">Kyrgystan</a>' then 3952
                    when m.value = '<a href="/sao_tome_et_principe">Sao Tome et Principe</a>' then 3860
                    when m.value = '<a href="/sao_tome_e_pricipe">Sao Tome e Pricipe</a>' then 3860
                    when m.value = '<a href="/comores">Comores</a>' then 3863
                    when m.value = '<a href="/timor-leste">Timor-Leste</a>' then 3973
                    when m.value = '<a href="/new_zealan">New Zealand</a>' then 4039
                    when m.value = '<a href="/russia">Russia</a>' then 4034
                    when m.value = '<a href="/tadjikistan">Tadjikistan</a>' then 3954
                    when m.value = '<a href="/saint_barthelemy">St. Barthelemy</a>' then 4097
                    when m.value = '<a href="/south_sudan">South Sudan</a>' then 4093
                    when m.value = '<a href="/guinea_bissau">Guinea-Bissau</a>' then 3843
                    when m.value = '<a href="/guinea_bissau">Guinea Bissau</a>' then 3843
                    when m.value = '<a href="/guinea">Guinea</a>' then 3842
                    when m.value = '<a href="/equitorial_guinea">Equitorial Guinea</a>' then 3858
                    when m.value = '<a href="/equatorial_guinea">Equatorial Guinea</a>' then 3858
                    when m.value = '<a href="/ecuatorial_guinea">Ecuatorial Guinea</a>' then 3858
                    when m.value = '<a href="/papua_new_guinea">Papua New Guinea</a>' then 4045
                    when m.value = '<a href="/niger">Niger</a>' then 3847
                    when m.value = '<a href="/nigeria">Nigeria</a>' then 3848
                    when m.value = '<a href="/solvakia">Slovakia</a>' then 4035
                    when m.value = '<a href="/slovakia">Slovakia</a>' then 4035
                    when m.value = '<a href="/slovakia">Slovakia</a>' then 4035
                    when m.value = '<a href="/democratic_republic_of_the_congo">Democratic Republic of the Congo</a>' then 3857
                    when m.value = '<a href="/democratic_republic_of_the_congo">Democratic Republic of the Congo</a>' then 3857
                    when m.value = '<a href="/republic_of_the_congo">Republic of The Congo (Brazzaville)</a>' then 3856
                    when m.value = '<a href="/republic_of_the_congo">Republic of The Congo (Brazzaville)</a>' then 3856
                    when m.value = '<a href="/dominican_repubic">Dominican Repubic</a>' then 3899
                    when m.value = '<a href="/dominican_republic">Dominican Republic</a>' then 3899
                    when m.value = '<a href="/dominica">Dominica</a>' then 3900
                    when m.value = '<a href="/dominca">Dominica</a>' then 3900
                    when m.value = '<a href="/somalia">Somalia</a>' then 3867
                    when m.value = '<a href="/slovakia">Algeria</a>' then 3829
                    when a.src IS NOT NULL then CAST(SUBSTR(a.src, 6, 6) AS INT)
                	ELSE NULL 
                END country_id,
                case when a.src IS NULL then m.value ELSE NULL END country_name
                FROM node n
                JOIN node_field_matrix_data m ON m.nid = n.nid AND m.vid = n.vid AND m.field_name = 'field_adoption_imports'
                LEFT JOIN url_alias a ON a.dst = SUBSTR(m.value, LOCATE(a.dst,m.value),LENGTH(a.dst))
                WHERE m.col = 1
                ) AS n
                LEFT JOIN node n2 ON n2.nid = country_id AND n2.`type` = 'country_type'
                WHERE (n.country_id IS NOT NULL AND n2.nid IS NOT NULL) OR (country_id IS NULL AND n2.nid IS NULL) 
                """;
        using var readCommand = mysqlconnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;


        var reader = readCommand.ExecuteReader();

        while (reader.Read())
        {
            var country = new AdoptionExportRelation
            {
                CountryIdTo = reader.GetInt32("country_id_to"),
                CountryIdFrom = reader.IsDBNull("country_id_from") ? null : reader.GetInt32("country_id_from"),
                CountryNameFrom = reader.IsDBNull("country_name_from") ? null : reader.GetString("country_name_from"),
            };
            yield return country;

        }
        reader.Close();
    }

    private static IEnumerable<AdoptionExportYear> ReadAdoptionExportYears(MySqlConnection mysqlconnection, NpgsqlConnection connection)
    {

        var sql = $"""
                SELECT
                DISTINCT
                n.nid country_id_to,
                country_id country_id_from,
                country_name country_name_from,
                m.col + 1998 `year`,
                cast(m.`value` AS INT) number_of_children
                FROM(
                SELECT 
                n.nid,
                n.vid,
                n.`row`,
                n.country_id,
                case when country_id IS NULL then country_name END country_name
                FROM
                (
                SELECT DISTINCT
                n.nid,
                n.vid,
                m.`row`,
                case 
                    when m.value = '<a href="/laos">Laos</a>' then 3967
                    when m.value = '<a href="/guyana">Guyana</a>' then 3927
                    when m.value = '<a href="/burma">Burma</a>' then 3969
                    when m.value = '<a href="/myamar">Myamar</a>' then 3969
                    when m.value = '<a href="/domican_repubic">Domican Repubic</a>' then 3900
                    when m.value = 'Palestine Authority' then 4082
                    when m.value = '<a href="/palestine_authorities">Palestine Authorities</a>' then 4082
                    when m.value = 'Gaza Strip' then 4082
                    when m.value = '<a href="/tunesia">Tunesia</a>' then 3834
                    when m.value = '<a href="/bosnia_herzigovina">Bosnia and Herzegovina Federation</a>' then 3999
                    when m.value = '<a href="/united kingdom">United Kingdom</a>' then 6185
                    when m.value = '<a href="/cote_ivoir">Côte d’Ivoire</a>' then 3839
                    when m.value = '<a href="/cote_ivoir">Cote d\'Ivoire</a>' then 3839
                    when m.value = '<a href="/laos">Laos</a>' then 3967
                    when m.value = '<a href="/morrocco">Morrocco</a>' then 3832
                    when m.value = '<a href="/guyana">Guyana</a>' then 3927
                    when m.value = '<a href="/antigua_and_barbuda ">Antigua and Barbuda</a>' then 4073
                    when m.value = '<a href="/antigua_and_barbuda">Antigua and Barbuda</a>' then 4073
                    when m.value = '<a href="czeck_republic">Czeck Replublic</a>' then 4029
                    when m.value = '<a href="/surinam">Surinam</a>' then 3930
                    when m.value = '<a href="/node/3857">Democratic Republic of The Congo</a>' then 3857
                    when m.value = '<a href="/mauretania">Mauretania</a>' then 3846
                    when m.value = '<a href="/marhsall_islands">Marshall Islands</a>' then 4050
                    when m.value = '<a href="/kyrgystan">Kyrgystan</a>' then 3952
                    when m.value = '<a href="/sao_tome_et_principe">Sao Tome et Principe</a>' then 3860
                    when m.value = '<a href="/sao_tome_e_pricipe">Sao Tome e Pricipe</a>' then 3860
                    when m.value = '<a href="/comores">Comores</a>' then 3863
                    when m.value = '<a href="/timor-leste">Timor-Leste</a>' then 3973
                    when m.value = '<a href="/new_zealan">New Zealand</a>' then 4039
                    when m.value = '<a href="/russia">Russia</a>' then 4034
                    when m.value = '<a href="/tadjikistan">Tadjikistan</a>' then 3954
                    when m.value = '<a href="/saint_barthelemy">St. Barthelemy</a>' then 4097
                    when m.value = '<a href="/south_sudan">South Sudan</a>' then 4093
                    when m.value = '<a href="/guinea_bissau">Guinea-Bissau</a>' then 3843
                    when m.value = '<a href="/guinea_bissau">Guinea Bissau</a>' then 3843
                    when m.value = '<a href="/guinea">Guinea</a>' then 3842
                    when m.value = '<a href="/equitorial_guinea">Equitorial Guinea</a>' then 3858
                    when m.value = '<a href="/equatorial_guinea">Equatorial Guinea</a>' then 3858
                    when m.value = '<a href="/ecuatorial_guinea">Ecuatorial Guinea</a>' then 3858
                    when m.value = '<a href="/papua_new_guinea">Papua New Guinea</a>' then 4045
                    when m.value = '<a href="/niger">Niger</a>' then 3847
                    when m.value = '<a href="/nigeria">Nigeria</a>' then 3848
                    when m.value = '<a href="/solvakia">Slovakia</a>' then 4035
                    when m.value = '<a href="/slovakia">Slovakia</a>' then 4035
                    when m.value = '<a href="/slovakia">Slovakia</a>' then 4035
                    when m.value = '<a href="/democratic_republic_of_the_congo">Democratic Republic of the Congo</a>' then 3857
                    when m.value = '<a href="/democratic_republic_of_the_congo">Democratic Republic of the Congo</a>' then 3857
                    when m.value = '<a href="/republic_of_the_congo">Republic of The Congo (Brazzaville)</a>' then 3856
                    when m.value = '<a href="/republic_of_the_congo">Republic of The Congo (Brazzaville)</a>' then 3856
                    when m.value = '<a href="/dominican_repubic">Dominican Repubic</a>' then 3899
                    when m.value = '<a href="/dominican_republic">Dominican Republic</a>' then 3899
                    when m.value = '<a href="/dominica">Dominica</a>' then 3900
                    when m.value = '<a href="/dominca">Dominica</a>' then 3900
                    when m.value = '<a href="/somalia">Somalia</a>' then 3867
                    when m.value = '<a href="/slovakia">Algeria</a>' then 3829
                    when a.src IS NOT NULL then CAST(SUBSTR(a.src, 6, 6) AS INT)
                    ELSE NULL 
                END country_id,
                case when a.src IS NULL then m.value ELSE NULL END country_name
                FROM node n
                JOIN node_field_matrix_data m ON m.nid = n.nid AND m.vid = n.vid AND m.field_name = 'field_adoption_imports'
                LEFT JOIN url_alias a ON a.dst = SUBSTR(m.value, LOCATE(a.dst,m.value),LENGTH(a.dst))
                WHERE m.col = 1
                ) AS n
                LEFT JOIN node n2 ON n2.nid = country_id AND n2.`type` = 'country_type'
                WHERE (n.country_id IS NOT NULL AND n2.nid IS NOT NULL) OR (country_id IS NULL AND n2.nid IS NULL) 
                ) AS n
                JOIN node_field_matrix_data m ON m.nid = n.nid AND m.vid = n.vid AND m.`row` = n.`row` AND m.field_name = 'field_adoption_imports'
                WHERE m.col <> 1
                ORDER BY n.nid, country_id, m.col
                """;
        using var readCommand = mysqlconnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;


        var reader = readCommand.ExecuteReader();

        while (reader.Read())
        {
            var country = new AdoptionExportYear
            {
                AdoptionExportRelationId = GetAdoptionExportRelationId(
                    reader.GetInt32("country_id_to"),
                    reader.IsDBNull("country_id_from") ? null : reader.GetInt32("country_id_from"),
                    reader.IsDBNull("country_name_from") ? null : reader.GetString("country_name_from"),
                    connection
                    ),
                Year = reader.GetInt32("year"),
                NumberOfChildren = reader.GetInt32("number_of_children"),
            };
            yield return country;

        }
        reader.Close();
    }
}
