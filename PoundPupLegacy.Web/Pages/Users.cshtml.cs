using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Npgsql;
using System.Data;
using PoundPupLegacy.ViewModel;

namespace PoundPupLegacy.Web.Pages
{
    public class UsersModel : PageModel
    {
        public List<User>? Users { get; set; }

        private NpgsqlConnection _connection;
        private readonly ILogger<UsersModel> _logger;
        public UsersModel(ILogger<UsersModel> logger, NpgsqlConnection connection)
        {
            _logger = logger;
            _connection = connection;
        }

        public async Task OnGet()
        {
            _connection.Open();
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
            Users = reader.GetFieldValue<List<User>>(0);
            _connection.Close();

        }
    }
}
