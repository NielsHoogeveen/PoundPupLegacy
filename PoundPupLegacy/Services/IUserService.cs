using PoundPupLegacy.Common;
using PoundPupLegacy.Models;
using System.Security.Claims;

namespace PoundPupLegacy.Services;

public abstract record UserRegistrationResponse
{
    private UserRegistrationResponse() { }
    public sealed record RegisteredUser(int Id) : UserRegistrationResponse
    {
    }
    public sealed record NameInUse() : UserRegistrationResponse
    {
    }
}
public abstract record UserLookupResponse
{
    public abstract User User { get; }
    private UserLookupResponse() { }
    public sealed record ExistingUser(User user) : UserLookupResponse
    {
        public int Id => user.Id;
        public string? Name => user.Name;
        public override User User => user;
    }
    public sealed record NewUser(string NameIdentifier) : UserLookupResponse
    {
        public override User User => new User{
            Id = 0, 
            Name = null 
        };
    }
    public sealed record NoUser() : UserLookupResponse
    {
        public override User User => new User {
            Id = 0,
            Name = null
        };
    }
}
public interface IUserService
{
    Task<List<UserRolesToAssign>> GetUserRolesToAssign(int userId);
    Task AssignUserRoles(UserRolesToAssign userRolesToAssign);
    Task<UserRegistrationResponse> RegisterUser(CompletedUserRegistrationData registrationData);
    Task<UserLookupResponse> GetUserInfo(ClaimsPrincipal claimsPrincipal);
}
