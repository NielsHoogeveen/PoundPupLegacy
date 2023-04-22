namespace PoundPupLegacy.ViewModel.UI.Services;

public interface IFetchCasesService
{
    [RequireNamedArgs]
    Task<Cases> FetchCases(int pageSize, int pageNumber, int tenantId, int userId, CaseType caseType);
}
