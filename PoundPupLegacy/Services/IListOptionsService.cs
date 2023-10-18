using PoundPupLegacy.Models;

namespace PoundPupLegacy.Services;

public interface IListOptionsService
{
    public Task<List<ListOption>> GetListOptions(int tenantId, int userId);
}
