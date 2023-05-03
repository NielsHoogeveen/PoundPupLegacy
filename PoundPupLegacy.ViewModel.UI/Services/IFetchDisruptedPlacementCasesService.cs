namespace PoundPupLegacy.ViewModel.UI.Services;

public interface IFetchDisruptedPlacementCasesService
{
    [RequireNamedArgs]
    Task<DisruptedPlacementCases> FetchCases(int pageSize, int pageNumber, int tenantId, int userId, int[] selectedTerms);
}
