namespace PoundPupLegacy.EditModel.UI.Services;

public interface ITopicSearchService
{
    [RequireNamedArgs]
    Task<List<Tag>> GetTerms(int? nodeId, int tenantId, string searchString, int[] nodeTypeIds);
}
