namespace PoundPupLegacy.ViewModel.UI.Services;

public interface IFetchAbuseCasesService
{
    [RequireNamedArgs]
    Task<AbuseCases> FetchCases(int pageSize, int pageNumber, int tenantId, int userId, int[] selectedTerms);
}
