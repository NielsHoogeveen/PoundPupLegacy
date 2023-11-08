using PoundPupLegacy.Common;
using PoundPupLegacy.Models;
using System;

namespace PoundPupLegacy.Services.Implementation;

public class SessionChatService : ISessionChatService
{
    private readonly IChatService _chatService;
    private readonly ISiteDataService siteDataService;
    private readonly IUserService _userService;
    private readonly ILogger _logger;
    private readonly IHttpContextAccessor _accessor;

    private HashSet<int> activeChats = new HashSet<int>();

    public Guid SessionToken { get; }

    private UserLookupResponse? _userLookupResponse;
    public SessionChatService(
        IChatService chatService,
        ISiteDataService siteDataService,
        IUserService userService,
        ILogger<SessionChatService> logger,
        IHttpContextAccessor accessor)
    {
        _chatService = chatService;
        this.siteDataService = siteDataService;
        _userService = userService;
        _accessor = accessor;
        _logger = logger;
        SessionToken = Guid.NewGuid();
    }

    bool hasBeenInitialized = false;

    private SemaphoreSlim initializationSemaphore = new SemaphoreSlim(1, 1);
    private async Task InitializeAsync()
    {
        await initializationSemaphore.WaitAsync();
        try {
            if (!hasBeenInitialized) {
                _logger.LogInformation($"{nameof(SessionChatService)} initialized with Id {SessionToken}");
                var user = _accessor.HttpContext?.User;
                if (user is null) {
                    throw new Exception("User is null");
                }
                _userLookupResponse = await _userService.GetUserInfo(user);
                if (_userLookupResponse is null) {
                    throw new Exception("UserLookupResponse is null");
                }
                await _chatService.RegisterForNewMessage(_userLookupResponse.User.Id, OnNewMessage);
                await _chatService.RegisterForActivation(_userLookupResponse.User.Id, OnActivate);
                hasBeenInitialized = true;
            }
        }
        finally {
            initializationSemaphore.Release();
        }
    }
    private async Task OnNewMessage()
    {
        if (NewMessageFunc is not null) {
            await NewMessageFunc();
        }
    }
    private async Task OnActivate()
    {
        if (ActivateFunc is not null) {
            await ActivateFunc();
        }
        if (AfterActivationFunc is not null) {
            await AfterActivationFunc();
        }
    }


    private Func<Task>? AfterActivationFunc = null;

    private Func<Task>? NewMessageFunc = null;

    private Func<Task>? ActivateFunc = null;

    public async Task RegisterForAfterActivation(Func<Task> func)
    {
        await InitializeAsync();
        AfterActivationFunc = func;
    }

    public async Task RegisterForActivation(Func<Task> func)
    {
        await InitializeAsync();
        ActivateFunc = func;
    }

    public async Task RegisterForNewMessage(Func<Task> func)
    {
        await InitializeAsync();
        NewMessageFunc = func;
    }

    public async Task SendMessage(Chat chat, string message)
    {
        await _chatService.SendMessage(_userLookupResponse!.User.Id, chat, message, async () => {
            foreach (var participant in chat.Participants) {
                await siteDataService.RemoveUser(participant.Id);
            }
        });
    }

    public IEnumerable<int> ActiveChatIds => activeChats;


    public async Task Activate(Chat chat)
    {

        activeChats.Add(chat.Id);
        if (chat.HasUnreadMessages && _userLookupResponse is not null) {
            await _chatService.Activate(chat, async () => {
                await siteDataService.RemoveUser(chat.Self.Id);
            });
            
        }
        else {
            await OnActivate();
        }
    }
    public async Task Deactivate(Chat chat)
    {
        activeChats.Remove(chat.Id);
        if (AfterActivationFunc is not null) {
            await AfterActivationFunc();
        }
    }
    public async Task<int?> GetChat(int userId)
    {
        return await _chatService.GetChat(_userLookupResponse!.User.Id, userId);
    }

    public async Task AddChat(int userId)
    {
        await _chatService.AddChat(_userLookupResponse!.User.Id, userId, async (id) => {
            activeChats.Add(id);
            await siteDataService.RemoveUser(_userLookupResponse!.User.Id);
        });
    }
    public async Task<List<User>> GetUsers(string searchString)
    {
        return await _chatService.GetUsers(searchString);
    }
    public async Task RefreshActiveChats()
    {
        if (AfterActivationFunc is not null) {
            await AfterActivationFunc();
        }
    }
}
