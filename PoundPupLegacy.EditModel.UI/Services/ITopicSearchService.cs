namespace PoundPupLegacy.EditModel.UI.Services;

public interface ITopicSearchService
{
    [RequireNamedArgs]
    Task<List<NodeTerm.ForCreate>> GetTerms(int tenantId, string searchString, int[] nodeTypeIds);

    Task<bool> DoesTopicExist(string name);
}
