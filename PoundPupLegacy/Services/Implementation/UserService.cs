using PoundPupLegacy.Common;
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
) : DatabaseService(connection, logger) , IUserService
{
    public async Task<int> GetUserId(ClaimsPrincipal claimsPrincipal)
    {

        if (claimsPrincipal == null) {
            return 0;
        }
        var key = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
        if(key is null) {
            return 0;
        }
        var userIdCached = siteDataService.GetUserByNameIdentifier(key.Value);
        if(userIdCached is not null) {
            return userIdCached.Value;
        }
        return await WithConnection(async (connection) => {
            var userByNameIdentifierReader = await userByNameIdentifierReaderFactory.CreateAsync(connection);
            var userId = await userByNameIdentifierReader.ReadAsync(new UserByNameIdentifierReaderRequest {
                NameIdentifier = key.Value
            });
            if(userId is not null) {
                await siteDataService.GetMenuItemsForUser(userId.UserId);
                return userId.UserId;
            }
            var email = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");
            if (email is null) {
                return 0;

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
                return userIdFromEmail.UserId;
            }
            return 0;
        });
    }
}
