using PoundPupLegacy.Models;

namespace PoundPupLegacy.Services;

public interface IUserProfileService
{
    Task<UserProfile?> GetUserProfile(int userId);
    Task Store(UserProfile userProfile);
}
