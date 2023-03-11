using PoundPupLegacy.ViewModel;

namespace PoundPupLegacy.Services;

public interface IFetchArticlesService
{
    public Task<Articles> GetArticles(int tenantId, List<int> selectedTerms, int startIndex, int length);
    public Task<Articles> GetArticles(int tenantId, int startIndex, int length);
}
