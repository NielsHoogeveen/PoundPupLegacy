using PoundPupLegacy.Models;

namespace PoundPupLegacy.Services;

public interface ICreateOptionsService
{
    public Task<List<CreateOption>> GetCreateOptions(int tenantId, int userId);
}
