using System.Text.Json.Serialization;

namespace PoundPupLegacy.Models;
[JsonSerializable(typeof(List<UserRolesToAssign>))]
internal partial class UserRolesToAssignJsonContext : JsonSerializerContext { }
public record UserRolesToAssign
{
    public required int UserId { get; init; }
    public required string UserName { get; init; }
    public required string RegistrationReason { get; init; }

    private List<TenantUserRoles> tenants = new();
    public required List<TenantUserRoles> Tenants {
        get => tenants;
        init {
            if (value is not null) {
                tenants = value;
            }
        }
    }
}
public record TenantUserRoles
{
    public required int Id { get; init; }
    public required string DomainName { get; init; }

    private List<UserRole> userRoles = new();
    public required List<UserRole> UserRoles {
        get => userRoles;
        init {
            if (value is not null) {
                userRoles = value;
            }
        }
    }

}

public record UserRole
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public bool HasBeenAssigned { get; set; } = false;
    public DateTime? ExpiryDate { get; set; }
}
