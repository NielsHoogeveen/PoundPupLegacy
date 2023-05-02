namespace PoundPupLegacy.ViewModel.UI.Services;

public interface IFetchChildTraffickingCasesService
{
    [RequireNamedArgs]
    Task<ChildTraffickingCases> FetchCases(int pageSize, int pageNumber, int tenantId, int userId);
}
