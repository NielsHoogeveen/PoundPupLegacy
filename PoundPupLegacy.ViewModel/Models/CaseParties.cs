using System.Text.Json.Serialization;

namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(CaseParties))]
public partial class CasePartiesJsonContext : JsonSerializerContext { }

public record CaseParties
{
    public required string PartyTypeName { get; init; }
    public string? OrganizationsText { get; init; }
    public string? PersonsText { get; init; }

    private BasicLink[] _persons = Array.Empty<BasicLink>();
    public required BasicLink[] Persons {
        get => _persons;
        init {
            if (value is not null) {
                _persons = value;
            }
        }
    }
    private BasicLink[] _organizations = Array.Empty<BasicLink>();
    public required BasicLink[] Organizations {
        get => _organizations;
        init {
            if (value is not null) {
                _organizations = value;
            }
        }
    }
}
