namespace PoundPupLegacy.ViewModel.UI.Services;

public interface IFetchArticlesService
{
    [RequireNamedArgs]
    public Task<Articles> GetArticles(int tenantId, List<int> selectedTerms, int pageNumber, int pageSize, string termNamePrefix);
}
