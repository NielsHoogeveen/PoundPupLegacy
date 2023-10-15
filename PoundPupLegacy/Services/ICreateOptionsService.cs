using PoundPupLegacy.Models;

namespace PoundPupLegacy.Services;

public interface ICreateOptionsService
{
    public Task<List<CreateOptions>> GetCreateOptions(int tenantId, int userId);
}
