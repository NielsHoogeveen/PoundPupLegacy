namespace PoundPupLegacy.ViewModel.UI.Services;

public interface IFetchWrongfulRemovalCasesService
{
    [RequireNamedArgs]
    Task<WrongfulRemovalCases> FetchCases(int pageSize, int pageNumber, int tenantId, int userId);
}
