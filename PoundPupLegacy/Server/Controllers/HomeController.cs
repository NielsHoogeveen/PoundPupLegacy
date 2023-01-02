using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Data;

namespace PoundPupLegacy.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly NpgsqlConnection _connection;

        public HomeController(ILogger<UserController> logger, NpgsqlConnection connection)
        {
            _logger = logger;
            _connection = connection;
        }

        [HttpGet]
        public string Get()
        {
            _connection.Open();
            var sql = $"""
                SELECT json_build_object(
                    'Id', u.id, 
                    'Name', a.name, 
                    'Created', u.created_date_time, 
                    'AboutMe', u.about_me, 
                    'AnimalWithin', u.animal_within, 
                    'RelationToChildPlacement', u.relation_to_child_placement, 
                    'Avatar', u.avatar, 
                    'Email', u.email, 
                    'Password', u.password
                )
                FROM public."user" u
                JOIN access_role a on a.id = u.id
                WHERE u.id = 2
                """;

            using var readCommand = _connection.CreateCommand();
            readCommand.CommandType = CommandType.Text;
            readCommand.CommandTimeout = 300;
            readCommand.CommandText = sql;

            var reader = readCommand.ExecuteReader();
            reader.Read();
            var res = reader.GetString(0);
            reader.Close();
            _connection.Close();
            return res;
        }

    }
}