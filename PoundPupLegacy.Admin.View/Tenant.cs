namespace PoundPupLegacy.Admin.View;

[JsonSerializable(typeof(Tenant))]
public partial class TenantJsonContext : JsonSerializerContext { }

public class Tenant
{
    public int Id { get; init; }

    public required string DomainName { get; set; }

    public required int CountryIdDefault { get; init; }

    public required string CountryNameDefault { get; init; }

    public required string? FrontPageText { get; set; }

    public required string? Logo { get; set; }

    public required string? Subtitle { get; set; }

    public required string? FooterText { get; set; }

    public required string? CssFile { get; set; }

    private List<UserRole> _userRoles = new List<UserRole>();
    public required List<UserRole> UserRoles {
        get => _userRoles;
        init {
            if (value is not null) {
                _userRoles = value;
            }
        }
    }

}
