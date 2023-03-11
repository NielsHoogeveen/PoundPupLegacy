using PoundPupLegacy.EditModel;

namespace PoundPupLegacy.Services;

public interface ITopicSearchService
{
    Task<List<Tag>> GetTerms(int? nodeId, int tenantId, string str);
}
