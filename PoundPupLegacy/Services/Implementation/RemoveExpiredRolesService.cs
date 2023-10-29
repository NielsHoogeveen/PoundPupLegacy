using Npgsql;
using PoundPupLegacy.Common;
using Quartz;
using System.Data;

namespace PoundPupLegacy.Services.Implementation;

public class RemoveExpiredRolesService(
    NpgsqlDataSource dataSource,
    ILogger<RemoveExpiredRolesService> logger
) : DatabaseService(dataSource, logger), IRemoveExpiredRolesService
{

    public async Task Execute(IJobExecutionContext context)
    {
        await WithConnection(async (connection) => {
            var statement = connection.CreateCommand();
            statement.CommandType = CommandType.Text;
            statement.CommandTimeout = 60;
            statement.CommandText = """
                delete from user_role_user where expiry_date < now();
                """;
            var i = await statement.ExecuteNonQueryAsync();
            logger.LogInformation($"Removed {i} expired roles");
            return Common.Unit.Instance;
        });
    }
}
