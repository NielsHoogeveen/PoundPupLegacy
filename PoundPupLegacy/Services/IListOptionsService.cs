using PoundPupLegacy.Models;

namespace PoundPupLegacy.Services;

public interface IListOptionsService
{
    public Task<List<ListOptions>> GetListOptions(int tenantId, int userId);
}
