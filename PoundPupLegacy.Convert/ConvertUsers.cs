using MySqlConnector;
using Npgsql;
using NpgsqlTypes;
using System.Data;

namespace PoundPupLegacy.Convert
{
    internal partial class Program
    {


        private class User
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public DateTime Created { get; set; }
            public string? AboutMe { get; set; }
            public string? AnimalWithin { get; set; }
            public string RelationToChildPlacement { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string? Avatar { get; set; }
        }

        private static void MigrateUsers(MySqlConnection mysqlconnection, NpgsqlConnection postgresqlconnection)
        {

            using var readCommand = mysqlconnection.CreateCommand();
            readCommand.CommandType = CommandType.Text;
            readCommand.CommandTimeout = 300;
            readCommand.CommandText = "SELECT DISTINCT \r\nu.uid id, \r\nu.name ,  \r\nFROM_UNIXTIME(u.created) created, \r\nctb.field_about_me_value about_me, \r\nctb.field_animal_within_value animal_within,\r\nctb.field_relation_to_child_placeme_value relation_to_child_placement,\r\nu.mail email,\r\nu.pass password,\r\nu.picture avatar\r\nfrom users u\r\nINNER JOIN bio b ON 1=1\r\nINNER JOIN node n2 ON n2.nid = b.nid\r\nINNER JOIN node_revisions nr ON nr.nid = n2.nid AND nr.vid = n2.vid\r\ninner JOIN content_type_uprofile ctb ON ctb.nid = n2.nid AND ctb.vid = n2.vid\r\nWHERE u.uid IN (\r\n\tSELECT distinct\r\n\t\tu.uid \r\n\tFROM users u\r\n\tJOIN node n ON n.uid = u.uid\r\n\tWHERE n.`type` NOT IN ('uprofile', 'usernode') AND u.uid <> 0\t\r\n\r\n)\r\nAND b.nid IN (\r\n\tSELECT MAX(b.nid)\r\n\tFROM bio b\r\n\twhere b.uid = u.uid\r\n) \r\nORDER BY u.uid";

            using var writeCommand = postgresqlconnection.CreateCommand();
            writeCommand.CommandType = CommandType.Text;
            writeCommand.CommandTimeout = 300;
            writeCommand.CommandText = """INSERT INTO public."user" (id, name, created, about_me, animal_within, relation_to_child_placement, email, password, avatar) VALUES(@id, @name, @created, @about_me, @animal_within, @relation_to_child_placement, @email, @password, @avatar)""";
            writeCommand.Parameters.Add("id", NpgsqlDbType.Integer);
            writeCommand.Parameters.Add("name", NpgsqlDbType.Varchar);
            writeCommand.Parameters.Add("created", NpgsqlDbType.Timestamp);
            writeCommand.Parameters.Add("about_me", NpgsqlDbType.Varchar);
            writeCommand.Parameters.Add("animal_within", NpgsqlDbType.Varchar);
            writeCommand.Parameters.Add("relation_to_child_placement", NpgsqlDbType.Varchar);
            writeCommand.Parameters.Add("email", NpgsqlDbType.Varchar);
            writeCommand.Parameters.Add("password", NpgsqlDbType.Varchar);
            writeCommand.Parameters.Add("avatar", NpgsqlDbType.Varchar);
            writeCommand.Prepare();

            var reader = readCommand.ExecuteReader();

            var adminUser = new User
            {
                Id = 1,
                Name = "admin",
                Created = DateTime.Parse("2006-10-31 22:12:58"),
                AboutMe = null,
                AnimalWithin = null,
                Email = "nielshoogev1@gmail.com",
                RelationToChildPlacement = "Other",
                Password = "02f8ae09323dc8efff5062539d09a32a",
                Avatar = null,

            };
            WriteUser(writeCommand, adminUser);
            while (reader.Read())
            {
                var aboutMe = reader.IsDBNull("about_me") ? null : reader.GetString("about_me") == "" ? null : reader.GetString("about_me");
                var animalWithing = reader.IsDBNull("animal_within") ? null : reader.GetString("animal_within") == "" ? null : reader.GetString("animal_within");
                var relationToChildPlacement = reader.IsDBNull("relation_to_child_placement") ? "Other" : reader.GetString("relation_to_child_placement");
                var avatar = reader.IsDBNull("avatar") ? null : reader.GetString("avatar") == "" ? null : reader.GetString("avatar");
                var user = new User
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
                WriteUser(writeCommand, user);
            }
        }

        private static void WriteUser(NpgsqlCommand command, User user)
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
