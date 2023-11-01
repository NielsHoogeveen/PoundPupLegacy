using Microsoft.AspNetCore.Identity.UI.Services;
using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.Models;
using PoundPupLegacy.Readers;
using PoundPupLegacy.Updaters;
using PoundPupLegacy.ViewModel.Readers;
using System.Data;
using System.Security.Claims;

namespace PoundPupLegacy.Services.Implementation;

internal sealed class UserService(
    NpgsqlDataSource dataSource,
    ILogger<SiteDataService> logger,
    ISiteDataService siteDataService,
    IEmailSender emailSender,
    ISiteDataService siteDateService,
    ISingleItemDatabaseReaderFactory<UsersRolesToAsignReaderRequest, List<UserRolesToAssign>> userRolesToAssignFactory,
    ISingleItemDatabaseReaderFactory<UserByNameIdentifierReaderRequest, User> userByNameIdentifierReaderFactory,
    ISingleItemDatabaseReaderFactory<UserByEmailReaderRequest, User> userByEmailReaderFactory,
    IDatabaseUpdaterFactory<UserNameIdentifierUpdaterRequest> userNameIdentifierUpdaterFactory
) : DatabaseService(dataSource, logger), IUserService
{
    public async Task AssignUserRoles(UserRolesToAssign userRolesToAssign)
    {
        await WithConnection<Unit>(async (connection) => {
            var tx = await connection.BeginTransactionAsync();
            try {
                using var insertRoleStatement = connection.CreateCommand();
                insertRoleStatement.CommandType = CommandType.Text;
                insertRoleStatement.CommandTimeout = 60;
                insertRoleStatement.CommandText = """
                insert into user_role_user(user_id, user_role_id, expiry_date)
                values(@user_id, @user_role_id, @expiry_date);
                """;
                insertRoleStatement.Parameters.Add("user_id", NpgsqlTypes.NpgsqlDbType.Integer);
                insertRoleStatement.Parameters.Add("user_role_id", NpgsqlTypes.NpgsqlDbType.Integer);
                insertRoleStatement.Parameters.Add("expiry_date", NpgsqlTypes.NpgsqlDbType.Date);
                await insertRoleStatement.PrepareAsync();
                foreach(var tenant in userRolesToAssign.Tenants) {
                    foreach(var userRole in tenant.UserRoles.Where(x => x.HasBeenAssigned)) {
                        insertRoleStatement.Parameters["user_id"].Value = userRolesToAssign.UserId;
                        insertRoleStatement.Parameters["user_role_id"].Value = userRole.Id;
                        if (userRole.ExpiryDate is null) {
                            insertRoleStatement.Parameters["expiry_date"].Value  = DBNull.Value;
                        }
                        else {
                            insertRoleStatement.Parameters["expiry_date"].Value = userRole.ExpiryDate;
                        }
                        var x = await insertRoleStatement.ExecuteNonQueryAsync();
                    }
                }
                using var updateUserStatement = connection.CreateCommand();
                updateUserStatement.CommandType = CommandType.Text;
                updateUserStatement.CommandTimeout = 60;
                updateUserStatement.CommandText = """
                    update "user"
                    set user_status_id = 1
                    where id = @user_id;
                """;
                updateUserStatement.Parameters.Add("user_id", NpgsqlTypes.NpgsqlDbType.Integer);
                await updateUserStatement.PrepareAsync();
                updateUserStatement.Parameters["user_id"].Value = userRolesToAssign.UserId;
                await updateUserStatement.ExecuteNonQueryAsync();
                await tx.CommitAsync();
                siteDataService.RemoveUser(userRolesToAssign.UserId);
                return Unit.Instance;
            }
            catch (Exception e) {
                await tx.RollbackAsync();
                throw;
            }
        });
    }
    public async Task<List<UserRolesToAssign>> GetUserRolesToAssign(int userId)
    {
        return await WithConnection<List<UserRolesToAssign>>(async (connection) => {
            var reader = await userRolesToAssignFactory.CreateAsync(connection);
            var result = await reader.ReadAsync(new UsersRolesToAsignReaderRequest { UserId = userId });
            if(result is null) {
                return new();
            }
            return result;
        });
    }
    public async Task<UserRegistrationResponse> RegisterUser(CompletedUserRegistrationData registrationData) 
    {
        logger.LogInformation($"Start registration of user {registrationData.UserName}");
        var result = await WithConnection<UserRegistrationResponse>(async (connection) => {

            try {
                var statement = connection.CreateCommand();
                statement.CommandType = CommandType.Text;
                statement.CommandTimeout = 60;
                statement.CommandText = """
                insert into principal default values;
                insert into publisher(id, name) values(lastval(), @name);
                insert into "user"(id, created_date_time, user_status_id, name_identifier, registration_reason)
                values(lastval(), now(), 2, @name_identifier, @registration_reason);
                insert into user_role_user(user_id, user_role_id)
                select
                    lastval(),
                    pug.access_role_id_not_logged_in
                from tenant t
                join publishing_user_group pug on pug.id = t.id;
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
                logger.LogInformation($"User registration of user {registrationData.UserName} succeeded");
                return new UserRegistrationResponse.RegisteredUser(x);
            }
            catch (Exception e) {
                if(e.Message.StartsWith("23505")) {
                    return new UserRegistrationResponse.NameInUse();
                }
                logger.LogError($"Error processing registration of {registrationData.UserName} {0}",e);
                throw;
            }
        });
        if (result is UserRegistrationResponse.RegisteredUser) {
            foreach (var email in await GetAdminEmailAccounts()){
                await emailSender.SendEmailAsync(email, "New User Registration", $"A new user has registered: {registrationData.UserName}");
            }
        }
        return result;
    }

    private async Task<List<string>> GetAdminEmailAccounts()
    {
        var emails = new List<string>();
        await WithConnection<Unit>(async (connection) => {
            var statement = connection.CreateCommand();
            statement.CommandType = CommandType.Text;
            statement.CommandTimeout = 60;
            statement.CommandText = """
                select 
                    distinct
                    email 
                    from "user" u
                    join user_role_user uru on uru.user_id = u.id
                    join tenant t on t.administrator_role_id = uru.user_role_id
                """;
            await statement.PrepareAsync();
            var reader = await statement.ExecuteReaderAsync();
            while (reader.Read()) {
                emails.Add(reader.GetString(0));
            }
            return Unit.Instance;
        });
        return emails;

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
        var user = siteDataService.GetUserByNameIdentifier(key.Value);
        if(user is not null) {

            return new UserLookupResponse.ExistingUser(user);
        }
        try {
            return await WithConnection<UserLookupResponse>(async (connection) => {
                var userByNameIdentifierReader = await userByNameIdentifierReaderFactory.CreateAsync(connection);
                var user = await userByNameIdentifierReader.ReadAsync(new UserByNameIdentifierReaderRequest {
                    NameIdentifier = key.Value
                });
                if (user is not null) {
                    await siteDataService.GetMenuItemsForUser(user.Id);
                    return new UserLookupResponse.ExistingUser(user);
                }
                var email = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");
                if (email is null) {
                    return new UserLookupResponse.NewUser(key.Value);

                }
                var userByEmailReader = await userByEmailReaderFactory.CreateAsync(connection);
                var userFromEmail = await userByEmailReader.ReadAsync(new UserByEmailReaderRequest {
                    Email = email.Value
                });
                if (userFromEmail is not null) {
                    var userNameIdentifierUpdater = await userNameIdentifierUpdaterFactory.CreateAsync(connection);
                    await userNameIdentifierUpdater.UpdateAsync(new UserNameIdentifierUpdaterRequest {
                        NameIdentifier = key.Value,
                        UserId = userFromEmail.Id
                    });
                    return new UserLookupResponse.ExistingUser(userFromEmail);
                }
                return new UserLookupResponse.NewUser(key.Value);
            });
        }catch(Exception e) {
            Console.WriteLine(e.ToString());
            throw;
        }
    }
}
