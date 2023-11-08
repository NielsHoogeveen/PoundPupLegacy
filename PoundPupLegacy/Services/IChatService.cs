using PoundPupLegacy.Models;

namespace PoundPupLegacy.Services;

public interface IChatService
{
    Task RegisterForActivation(int userId, Func<Task> notification);
    Task RegisterForNewMessage(int userId, Func<Task> notification);
    Task Activate(Chat chat, Func<Task> callback);
    Task SendMessage(int userId, Chat chat, string message, Func<Task> func);
    Task<int?> GetChat(int userId1, int userId2);
    Task AddChat(int userId1, int userId2, Func<int, Task> func);
    Task<List<Common.User>> GetUsers(string searchString);

}
