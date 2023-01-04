using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Data;
using System.Text.Json;
using PoundPupLegacy.ViewModel;

namespace PoundPupLegacy.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly NpgsqlConnection _connection;

        public UserController(ILogger<UserController> logger, NpgsqlConnection connection)
        {
            _logger = logger;
            _connection = connection;
        }

        [HttpGet]
        public async Task<List<User>> Get()
        {
            await _connection.OpenAsync();
            var sql = $"""
                WITH 
                users AS(
                	SELECT
                		u.id, 
                		ar.name, 
                		u.created_date_time, 
                		u.about_me, 
                		u.animal_within, 
                		u.relation_to_child_placement, 
                		u.avatar, 
                		u.email, 
                		u.password
                	FROM public."user" u
                	JOIN public."access_role" ar on ar.id = u.id
                	ORDER BY ar.name
                ),
                doc as (
                	SELECT json_agg(
                		json_build_object(
                			'Id', u.id, 
                			'Name', u.name, 
                			'Created', u.created_date_time, 
                			'AboutMe', u.about_me, 
                			'AnimalWithin', u.animal_within, 
                			'RelationToChildPlacement', u.relation_to_child_placement, 
                			'Avatar', u.avatar, 
                			'Email', u.email, 
                			'Password', u.password
                		) :: jsonb
                	) ::jsonb as agg
                	FROM users u
                ) 
                SELECT agg from doc
                """;

            using var readCommand = _connection.CreateCommand();
            readCommand.CommandType = CommandType.Text;
            readCommand.CommandTimeout = 300;
            readCommand.CommandText = sql;

            await using var reader = await readCommand.ExecuteReaderAsync();
            await reader.ReadAsync();
            var someValue = reader.GetFieldValue<List<User>>(0);
            _connection.Close();
            return someValue;

        }

    }
}