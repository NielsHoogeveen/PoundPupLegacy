using PoundPupLegacy.Common;
using PoundPupLegacy.Models;

namespace PoundPupLegacy.Services;

public interface ISiteDataService
{
    (int, string) GetDefaultCountry(int tenantId);

    Task<bool> InitializeAsync();

    User? GetUserByNameIdentifier(string id);

    Task<bool> HasAccess(int userId, string path);

    Task<bool> CanEdit(ViewModel.Models.Node node, int userId);

    Task<bool> CanViewNodeAccess(int userId);

    Task<bool> CanCreate(int nodeTypeId, int userGroupId, int userId);

    Tenant GetTenant();

    Task<List<MenuItem>> GetMenuItemsForUser(int userId);
    
    Task<List<Chat>> GetChats(int userId);

    Task RefreshTenants();
    Task RemoveUser(int userId);

    Task<UserWithDetails?> GetUser(int userId);

    bool DoesSubgroupExist(int subgroupId);

}
