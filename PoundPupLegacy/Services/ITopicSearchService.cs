using PoundPupLegacy.Common;
using PoundPupLegacy.EditModel;

namespace PoundPupLegacy.Services;

public interface ITopicSearchService
{
    [RequireNamedArgs]
    Task<List<Tag>> GetTerms(int nodeId, int tenantId, string searchString);
}
