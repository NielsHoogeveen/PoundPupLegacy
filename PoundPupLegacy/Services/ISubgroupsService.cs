using PoundPupLegacy.Models;

namespace PoundPupLegacy.Services;

public interface ISubgroupsService
{
    public Task<List<Subgroup>> GetSubgroups(int tenantId);
}
