using System.Data;

namespace PoundPupLegacy.Common.Test;

public class EmptySelectTests
{
    [Fact]
    public async Task EmptySelectDoesNotCrash()
    {
        var connection = DatabaseValidatorBase.GetConnection();
        try {
            var t = connection.GetType();
            await connection.OpenAsync();
            var command1 = connection.CreateCommand();
            command1.CommandText = """
            select
            t.id tenant_id
            from tenant t
            where t.id = @tenant_id
            """;
            command1.Parameters.Add("tenant_id", NpgsqlTypes.NpgsqlDbType.Integer);
            await command1.PrepareAsync();
            command1.Parameters["tenant_id"].Value = 6;
            var reader = await command1.ExecuteReaderAsync();
            var tenantId = 1;
            if(await reader.ReadAsync()) {
                tenantId = reader.GetInt32(0);
            }
            await reader.CloseAsync();

            var command2 = connection.CreateCommand();
            command2.CommandText = """
            select
            tn.id tenant_id,
            tn.url_id,
            tn.url_path
            from tenant_node tn 
            where tn.url_path is not null
            and tn.id = @tenant_id
            """;

            command2.Parameters.Add("tenant_id", NpgsqlTypes.NpgsqlDbType.Integer);
            await command2.PrepareAsync();
            command2.Parameters["tenant_id"].Value = 6;
            var reader2 = await command2.ExecuteReaderAsync();
            while(await reader.ReadAsync()) {

            }
            await reader2.CloseAsync();
        }catch(Exception e) {
            Console.WriteLine(e.ToString());
            throw;
        }
        finally {
            await connection.CloseAsync();
        }

    }
}
