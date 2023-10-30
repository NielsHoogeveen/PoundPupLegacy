namespace PoundPupLegacy.ViewModel.UI.Services;

public interface IFetchNameablesService
{
    Task<Nameables?> FetchNameables(int tenantId, int userId, int nodeTypeId);
}
