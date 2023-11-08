using PoundPupLegacy.Models;

namespace PoundPupLegacy.Services;

public interface ISessionChatService
{
    Guid SessionToken { get; }
    Task RegisterForActivation(Func<Task> func);
    Task RegisterForAfterActivation(Func<Task> func);
    Task RegisterForNewMessage(Func<Task> func);
    Task SendMessage(Chat chat, string message);
    Task Activate(Chat chat);
    Task Deactivate(Chat chat);
    IEnumerable<int> ActiveChatIds { get; }
    Task<int?> GetChat(int userId);
    Task AddChat(int userId);
    Task<List<Common.User>> GetUsers(string searchString);
    Task RefreshActiveChats();


}
