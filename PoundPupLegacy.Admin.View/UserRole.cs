namespace PoundPupLegacy.Admin.View;

public record UserRole
{
    public int UserRoleId { get; init; }

    public required string UserRoleName { get; init; }

    public required bool IsAdministrator { get; init; }

    private List<User> _users = new List<User>();
    public required List<User> Users {
        get => _users;
        init {
            if (value is not null) {
                _users = value;
            }
        }
    }
    private List<BasicActionPrivilege> _basicActionPrivileges = new List<BasicActionPrivilege>();
    public required List<BasicActionPrivilege> BasicActionPrivileges {
        get => _basicActionPrivileges;
        init {
            if (value is not null) {
                _basicActionPrivileges = value;
            }
        }
    }
    private List<CreateNodeActionPrivilege> _createNodeActionPrivileges = new List<CreateNodeActionPrivilege>();
    public required List<CreateNodeActionPrivilege> CreateNodeActionPrivileges {
        get => _createNodeActionPrivileges;
        init {
            if (value is not null) {
                _createNodeActionPrivileges = value;
            }
        }
    }
    private List<EditNodeActionPrivilege> _editNodeActionPrivileges = new List<EditNodeActionPrivilege>();
    public required List<EditNodeActionPrivilege> EditNodeActionPrivileges {
        get => _editNodeActionPrivileges;
        init {
            if (value is not null) {
                _editNodeActionPrivileges = value;
            }
        }
    }
    private List<EditOwnNodeActionPrivilege> _editOwnNodeActionPrivileges = new List<EditOwnNodeActionPrivilege>();
    public required List<EditOwnNodeActionPrivilege> EditOwnNodeActionPrivileges {
        get => _editOwnNodeActionPrivileges;
        init {
            if (value is not null) {
                _editOwnNodeActionPrivileges = value;
            }
        }
    }
    private List<NamedActionPrivilege> _namedActionPrivileges = new List<NamedActionPrivilege>();
    public required List<NamedActionPrivilege> NamedActionPrivileges {
        get => _namedActionPrivileges;
        init {
            if (value is not null) {
                _namedActionPrivileges = value;
            }
        }
    }
    private List<MenuItem> _tenantNodeMenuItem = new List<MenuItem>();
    public required List<MenuItem> TenantNodeMenuItem {
        get => _tenantNodeMenuItem;
        init {
            if (value is not null) {
                _tenantNodeMenuItem = value;
            }
        }
    }


}
