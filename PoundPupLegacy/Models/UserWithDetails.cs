using PoundPupLegacy.Common;
using System.Text.Json.Serialization;

namespace PoundPupLegacy.Models;

[JsonSerializable(typeof(UserWithDetails))]
internal partial class UserJsonContext : JsonSerializerContext { }


public record UserWithDetails: User
{
    public required string NameIdentifier { get; init; }

    private HashSet<UserAction> _actions = new();
    public required HashSet<UserAction> Actions {
        get => _actions;

        init {
            if(value is not null) {
                _actions = value;
            }
        }
    }
    private HashSet<UserCreateAction> _createActions = new();
    public required HashSet<UserCreateAction> CreateActions {
        get => _createActions;

        init {
            if (value is not null) {
                _createActions = value;
            }
        }
    }

    private HashSet<UserEditAction> _editActions = new();
    public required HashSet<UserEditAction> EditActions {
        get => _editActions;

        init {
            if (value is not null) {
                _editActions = value;
            }
        }
    }

    private HashSet<UserEditOwnAction> _editOwnActions = new();
    public required HashSet<UserEditOwnAction> EditOwnActions {
        get => _editOwnActions;

        init {
            if (value is not null) {
                _editOwnActions = value;
            }
        }
    }

    private HashSet<UserNamedAction> _userNamedActions = new();
    public required HashSet<UserNamedAction> NamedActions {
        get => _userNamedActions;

        init {
            if (value is not null) {
                _userNamedActions = value;
            }
        }
    }

    private List<MenuItem> _menuItems = new();
    public required List<MenuItem> MenuItems {
        get => _menuItems;

        init {
            if (value is not null) {
                _menuItems = value;
            }
        }
    }
}
