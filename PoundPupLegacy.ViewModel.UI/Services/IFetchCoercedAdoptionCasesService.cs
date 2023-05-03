namespace PoundPupLegacy.ViewModel.UI.Services;

public interface IFetchCoercedAdoptionCasesService
{
    [RequireNamedArgs]
    Task<CoercedAdoptionCases> FetchCases(int pageSize, int pageNumber, int tenantId, int userId, int[] selectedTerms);
}
