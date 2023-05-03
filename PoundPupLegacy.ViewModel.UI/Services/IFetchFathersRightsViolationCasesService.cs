namespace PoundPupLegacy.ViewModel.UI.Services;

public interface IFetchFathersRightsViolationCasesService
{
    [RequireNamedArgs]
    Task<FathersRightsViolationCases> FetchCases(int pageSize, int pageNumber, int tenantId, int userId, int[] selectedTerms);
}
