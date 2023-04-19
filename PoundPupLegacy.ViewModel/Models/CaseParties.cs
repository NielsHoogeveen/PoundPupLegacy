namespace PoundPupLegacy.ViewModel.Models;

public record CaseParties
{
    public required string PartyTypeName { get; init; }
    public string? OrganizationsText { get; init; }
    public string? PersonsText { get; init; }

    private Link[] _persons = Array.Empty<Link>();
    public required Link[] Persons {
        get => _persons;
        init {
            if (value is not null) {
                _persons = value;
            }
        }
    }
    private Link[] _organizations = Array.Empty<Link>();
    public required Link[] Organizations {
        get => _organizations;
        init {
            if (value is not null) {
                _organizations = value;
            }
        }
    }
}
