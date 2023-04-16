using PoundPupLegacy.ViewModel.Models;

namespace PoundPupLegacy.Services;

public interface IFetchCasesService
{
    Task<Cases> FetchCases(int limit, int offset, int tenantId, int userId, CaseType caseType);
}
