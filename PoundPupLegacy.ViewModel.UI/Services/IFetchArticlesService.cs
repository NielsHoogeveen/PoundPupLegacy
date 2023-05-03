namespace PoundPupLegacy.ViewModel.UI.Services;

public interface IFetchArticlesService
{
    [RequireNamedArgs]
    public Task<Articles> GetArticles(int tenantId, int userId, int[] selectedTerms, int pageNumber, int pageSize);
}
