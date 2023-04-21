using PoundPupLegacy.Common;
using PoundPupLegacy.ViewModel.Models;

namespace PoundPupLegacy.Services;

public interface IFetchCasesService
{
    [RequireNamedArgs]
    Task<Cases> FetchCases(int pageSize, int pageNumber, int tenantId, int userId, CaseType caseType);
}
