using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.Models;
using PoundPupLegacy.Readers;
using PoundPupLegacy.Updaters;
using System.Data;
using System.Security.Claims;

namespace PoundPupLegacy.Services.Implementation;

internal sealed class UserService(
    IDbConnection connection,
    ILogger<SiteDataService> logger,
    ISiteDataService siteDataService,
    ISingleItemDatabaseReaderFactory<UserByNameIdentifierReaderRequest, UserIdByNameIdentifier> userByNameIdentifierReaderFactory,
    ISingleItemDatabaseReaderFactory<UserByEmailReaderRequest, UserIdByEmail> userByEmailReaderFactory,
    IDatabaseUpdaterFactory<UserNameIdentifierUpdaterRequest> userNameIdentifierUpdaterFactory
) : DatabaseService(connection, logger), IUserService
{

    public async Task<UserRegistrationResponse> RegisterUser(CompletedUserRegistrationData registrationData) 
    {
        return await WithConnection<UserRegistrationResponse>(async (connection) => {
            try {
                var statement = connection.CreateCommand();
                statement.CommandType = CommandType.Text;
                statement.CommandTimeout = 60;
                statement.CommandText = """
                insert into principal default values;
                insert into publisher(id, name) values(lastval(), @name);
                insert into "user"(id, created_date_time, user_status_id, name_identifier, registration_reason)
                values(lastval(), now(), 1, @name_identifier, @registration_reason);
                insert into user_role_user(user_id, user_role_id)
                select
                    lastval(),
                    t.access_role_id_not_logged_in
                from tenant t
                where t.id = @tenant_id;
                SELECT lastval();
                """;
                statement.Parameters.Add("name", NpgsqlTypes.NpgsqlDbType.Varchar);
                statement.Parameters.Add("name_identifier", NpgsqlTypes.NpgsqlDbType.Varchar);
                statement.Parameters.Add("registration_reason", NpgsqlTypes.NpgsqlDbType.Text);
                statement.Parameters.Add("tenant_id", NpgsqlTypes.NpgsqlDbType.Integer);
                await statement.PrepareAsync();
                statement.Parameters["name"].Value = registrationData.UserName;
                statement.Parameters["name_identifier"].Value = registrationData.NameIdentifier;
                statement.Parameters["registration_reason"].Value = registrationData.RegistrationReason;
                statement.Parameters["tenant_id"].Value = siteDataService.GetTenant().Id;
                var x = await statement.ExecuteNonQueryAsync();
                return new UserRegistrationResponse.RegisteredUser(x);
            }
            catch (Exception e) {
                if(e.Message.StartsWith("23505")) {
                    return new UserRegistrationResponse.NameInUse();
                }
                throw;
            }
        });
    }
    public async Task<UserLookupResponse> GetUserInfo(ClaimsPrincipal claimsPrincipal)
    {

        if (claimsPrincipal == null) {
            return new UserLookupResponse.NoUser();
        }
        var key = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
        if(key is null) {
            return new UserLookupResponse.NoUser();
        }
        var userIdCached = siteDataService.GetUserByNameIdentifier(key.Value);
        if(userIdCached is not null) {

            return new UserLookupResponse.ExistingUser(userIdCached.Value);
        }
        return await WithConnection<UserLookupResponse>(async (connection) => {
            var userByNameIdentifierReader = await userByNameIdentifierReaderFactory.CreateAsync(connection);
            var userId = await userByNameIdentifierReader.ReadAsync(new UserByNameIdentifierReaderRequest {
                NameIdentifier = key.Value
            });
            if(userId is not null) {
                await siteDataService.GetMenuItemsForUser(userId.UserId);
                return new UserLookupResponse.ExistingUser(userId.UserId);
            }
            var email = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");
            if (email is null) {
                return new UserLookupResponse.NewUser(key.Value);

            }
            var userByEmailReader = await userByEmailReaderFactory.CreateAsync(connection);
            var userIdFromEmail = await userByEmailReader.ReadAsync(new UserByEmailReaderRequest { 
                Email = email.Value 
            });
            if(userIdFromEmail is not null) {
                var userNameIdentifierUpdater = await userNameIdentifierUpdaterFactory.CreateAsync(connection);
                await userNameIdentifierUpdater.UpdateAsync(new UserNameIdentifierUpdaterRequest {
                    NameIdentifier = key.Value,
                    UserId = userIdFromEmail.UserId
                });
                return new UserLookupResponse.ExistingUser(userIdFromEmail.UserId);
            }
            return new UserLookupResponse.NewUser(key.Value);
        });
    }
}
