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
    private UserLookupResponse() { }
    public sealed record ExistingUser(int Id) : UserLookupResponse
    {
    }
    public sealed record NewUser(string NameIdentifier) : UserLookupResponse
    {
    }
    public sealed record NoUser() : UserLookupResponse
    {
    }
}
public interface IUserService
{
    Task<UserRegistrationResponse> RegisterUser(CompletedUserRegistrationData registrationData);
    Task<UserLookupResponse> GetUserInfo(ClaimsPrincipal claimsPrincipal);
}
