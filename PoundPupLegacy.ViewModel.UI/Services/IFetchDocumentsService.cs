namespace PoundPupLegacy.ViewModel.UI.Services;

public interface IFetchDocumentsService
{
    [RequireNamedArgs]
    public Task<Documents> GetArticles(int tenantId, int userId, int[] selectedTerms, int pageNumber, int pageSize);
}
