using PoundPupLegacy.Common;

namespace PoundPupLegacy.Services.Implementation;

public class ActiveUserService: IActiveUserService
{
    private SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);

    private Dictionary<int, ActiveUser> _activeUsers = new Dictionary<int, ActiveUser>();

    public List<ActiveUser> ActiveUsers => _activeUsers.Values.ToList();

    public event Func<Task>? Notify;

    public async Task RegisterActiveUser(User user)
    {
        await semaphore.WaitAsync();
        try {
            if (!_activeUsers.TryAdd(user.Id, new ActiveUser {
                User = user,
                DateTime = DateTime.Now,
                Count = 1
            })) {
                _activeUsers[user.Id].Count++;
            }
        }
        finally {
            if (Notify is not null) {
                await Notify.Invoke();
            }
            semaphore.Release();
        }
    }

    public async Task UnRegisterActiveUser(User user, bool unregisterAll)
    {
        await semaphore.WaitAsync();
        try {
            if (_activeUsers.TryGetValue(user.Id, out var activeUser)) {
                if (activeUser is not null) {
                    if (activeUser.Count > 1 && !unregisterAll) {
                        activeUser.Count--;
                    }
                    else {
                        _activeUsers.Remove(user.Id);
                    }
                }
            }
        }
        finally {
            if (Notify is not null) {
                await Notify.Invoke();
            }
            semaphore.Release();
        }
    }
}
