namespace PoundPupLegacy.ViewModel.UI.Services;

public interface IFetchRecentPostsService
{
    [RequireNamedArgs]
    Task<RecentPosts> FetchRecentPosts(int pageSize, int pageNumber, int tenantId, int userId);
}
