namespace PoundPupLegacy.ViewModel.UI.Services;

public interface IFetchDeportationCasesService
{
    [RequireNamedArgs]
    Task<DeportationCases> FetchCases(int pageSize, int pageNumber, int tenantId, int userId, int[] selectedTerms);
}
