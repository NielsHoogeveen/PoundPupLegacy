using PoundPupLegacy.ViewModel;

namespace PoundPupLegacy.Services;

public interface IFetchNodeService
{
    Task<Node?> FetchNode(int id, HttpContext context);
}
