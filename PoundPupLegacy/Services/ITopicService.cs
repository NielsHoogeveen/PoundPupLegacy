using PoundPupLegacy.ViewModel;
using SearchOption = PoundPupLegacy.ViewModel.SearchOption;

namespace PoundPupLegacy.Services;

public interface ITopicService
{
    Task<Topics> FetchTopics(int limit, int offset, string searchTerm, SearchOption searchOption);
}
