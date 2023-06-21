using Quartz;

namespace PoundPupLegacy.Services;

public interface INodeAccessService: IJob
{
    public Task Log(int userId, int tenantId, int nodeId);
}
