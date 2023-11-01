using PoundPupLegacy.Common;

namespace PoundPupLegacy.Services;

public interface IActiveUserService
{
    Task RegisterActiveUser(User user);
    Task UnRegisterActiveUser(User user, bool unregisterAll);
    List<ActiveUser> ActiveUsers { get; }

    event Func<Task> Notify;

}
