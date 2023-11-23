namespace PoundPupLegacy.ViewModel.UI.Services;

public interface IFetchAbuseCasesMapService
{
     Task<AbuseCaseMapEntry[]> FetchCasesMap(int tenantId, int userId);
}
