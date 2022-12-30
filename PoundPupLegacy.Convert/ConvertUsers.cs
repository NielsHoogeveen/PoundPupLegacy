using MySqlConnector;
using Npgsql;
using NpgsqlTypes;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert
{
    internal partial class Program
    {

        private static void MigrateUsers(MySqlConnection mysqlconnection, NpgsqlConnection postgresqlconnection)
        {
            WriteUser(ReadUsers(mysqlconnection), postgresqlconnection);
        }
        private static IEnumerable<User> ReadUsers(MySqlConnection mysqlconnection)
        {

            using var readCommand = mysqlconnection.CreateCommand();
            readCommand.CommandType = CommandType.Text;
            readCommand.CommandTimeout = 300;
            readCommand.CommandText = """
                SELECT 
                    DISTINCT u.uid id, 
                    u.name ,  
                    FROM_UNIXTIME(u.created) created, 
                    CASE 
                		WHEN ctb.field_about_me_value = '' then null
                	    ELSE ctb.field_about_me_value 
                    END about_me, 
                	CASE 
                	    WHEN ctb.field_animal_within_value = '' then null
                		ELSE ctb.field_animal_within_value
                	END animal_within,
                	CASE
                        WHEN ctb.field_relation_to_child_placeme_value  = '' then null
                        ELSE ctb.field_relation_to_child_placeme_value
                	END relation_to_child_placement,
                    u.mail email,
                    u.pass password,
                    CASE 
                	    WHEN u.picture = '' then null
                		ELSE u.picture
                    END avatar
                FROM users u
                LEFT JOIN bio b ON b.uid = u.uid
                LEFT JOIN node n2 ON n2.nid = b.nid
                LEFT JOIN node_revisions nr ON nr.nid = n2.nid AND nr.vid = n2.vid
                LEFT JOIN content_type_uprofile ctb ON ctb.nid = n2.nid AND ctb.vid = n2.vid
                WHERE u.uid IN (
                    SELECT distinct
                    u.uid 
                    FROM users u
                    JOIN node n ON n.uid = u.uid
                    WHERE n.`type` NOT IN ('uprofile', 'usernode') AND u.uid <> 0
                )
                AND (b.nid is NULL OR b.nid IN (
                    SELECT 
                        MAX(b.nid)
                    FROM bio b
                    WHERE b.uid = u.uid
                ) )
                """;

            var reader = readCommand.ExecuteReader();

            while (reader.Read())
            {
                var aboutMe = reader.IsDBNull("about_me") ? null : reader.GetString("about_me") == "" ? null : reader.GetString("about_me");
                var animalWithing = reader.IsDBNull("animal_within") ? null : reader.GetString("animal_within") == "" ? null : reader.GetString("animal_within");
                var relationToChildPlacement = reader.IsDBNull("relation_to_child_placement") ? "Other" : reader.GetString("relation_to_child_placement");
                var avatar = reader.IsDBNull("avatar") ? null : reader.GetString("avatar") == "" ? null : reader.GetString("avatar");
                yield return new User
                {
                    Id = reader.GetInt32("id"),
                    Name = reader.GetString("name"),
                    Created = reader.GetDateTime("created"),
                    AboutMe = aboutMe == "(NULL)" ? null : aboutMe,
                    AnimalWithin = animalWithing == "(NULL)" ? "" : animalWithing,
                    RelationToChildPlacement = relationToChildPlacement == "(NULL)" ? "Other" : relationToChildPlacement,
                    Email = reader.GetString("email"),
                    Password = reader.GetString("password"),
                    Avatar = avatar
                };
            }
            reader.Close();
        }

        private static void WriteUser(IEnumerable<User> users, NpgsqlConnection postgresqlconnection)
        {
            using var command = postgresqlconnection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandTimeout = 300;
            command.CommandText = """INSERT INTO public."user" (id, name, created, about_me, animal_within, relation_to_child_placement, email, password, avatar) VALUES(@id, @name, @created, @about_me, @animal_within, @relation_to_child_placement, @email, @password, @avatar)""";
            command.Parameters.Add("id", NpgsqlDbType.Integer);
            command.Parameters.Add("name", NpgsqlDbType.Varchar);
            command.Parameters.Add("created", NpgsqlDbType.Timestamp);
            command.Parameters.Add("about_me", NpgsqlDbType.Varchar);
            command.Parameters.Add("animal_within", NpgsqlDbType.Varchar);
            command.Parameters.Add("relation_to_child_placement", NpgsqlDbType.Varchar);
            command.Parameters.Add("email", NpgsqlDbType.Varchar);
            command.Parameters.Add("password", NpgsqlDbType.Varchar);
            command.Parameters.Add("avatar", NpgsqlDbType.Varchar);
            command.Prepare();

            foreach (var user in users)
            {
                command.Parameters["id"].Value = user.Id;
                command.Parameters["name"].Value = user.Name;
                command.Parameters["created"].Value = user.Created;
                if (user.AboutMe != null)
                {
                    command.Parameters["about_me"].Value = user.AboutMe;
                }
                else
                {
                    command.Parameters["about_me"].Value = DBNull.Value;
                }
                if (user.AnimalWithin != null)
                {
                    command.Parameters["animal_within"].Value = user.AnimalWithin;
                }
                else
                {
                    command.Parameters["animal_within"].Value = DBNull.Value;
                }
                command.Parameters["relation_to_child_placement"].Value = user.RelationToChildPlacement;
                command.Parameters["email"].Value = user.Email;
                command.Parameters["password"].Value = user.Password;
                if (user.Avatar != null)
                {
                    command.Parameters["avatar"].Value = user.Avatar;
                }
                else
                {
                    command.Parameters["avatar"].Value = DBNull.Value;
                }
                command.ExecuteNonQuery();
            }
        }
    }
}
