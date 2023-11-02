namespace PoundPupLegacy.ViewModel.UI.Services;

public interface IFetchRecentUserPostsService
{
    [RequireNamedArgs]
    Task<RecentPosts> FetchRecentUserPosts(int pageSize, int pageNumber, int tenantId, int userId, int userIdPublisher);
}
