using PoundPupLegacy.ViewModel;

namespace PoundPupLegacy.Services;

public interface IFetchArticlesService
{
    public Task<Articles> GetArticles(List<int> selectedTerms, int startIndex, int length, int tenantId);
    public Task<Articles> GetArticles(int startIndex, int length, int tenantId);
}
