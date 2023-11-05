namespace PoundPupLegacy.Services;

public interface INodeService
{
    Task<string?> GetRedirectPath(int nodeId, int tenantId);
    Task<string?> GetRedirectPath(string urlPath, int tenantId);
}
