using PoundPupLegacy.Admin.View;

namespace PoundPupLegacy.Admin.UI.Services
{
    internal interface ITenantRetrieveService
    {
        Task<Tenant?> GetTenant(int tenantId);
    }
}