using PoundPupLegacy.ViewModel.Models;

namespace PoundPupLegacy.Services;

public interface IFetchArticlesService
{
    public Task<Articles> GetArticles(int tenantId, List<int> selectedTerms, int startIndex, int length);
}
