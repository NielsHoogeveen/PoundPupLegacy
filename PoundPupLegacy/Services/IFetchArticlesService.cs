using PoundPupLegacy.Common;
using PoundPupLegacy.ViewModel.Models;

namespace PoundPupLegacy.Services;

public interface IFetchArticlesService
{
    [RequireNamedArgs]
    public Task<Articles> GetArticles(int tenantId, List<int> selectedTerms, int pageNumber, int pageSize, string termNamePrefix);
}
