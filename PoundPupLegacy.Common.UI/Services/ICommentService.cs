using PoundPupLegacy.Common;

namespace PoundPupLegacy.Services;

public interface ICommentService
{
    Task<bool> CanCreateComment(int userId);
    Task<bool> CanEditComment(int userId);
    Task<bool> CanEditOwnComment(int userId);
    Task<int> Save(Comment.ToCreate comment);
    Task Save(Comment.ToUpdate comment);
}

