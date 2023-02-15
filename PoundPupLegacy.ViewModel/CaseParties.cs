namespace PoundPupLegacy.ViewModel;

public record CaseParties
{
    public required string PartyTypeName { get; init; }
    public required string? OrganizationsText { get; init; }
    public required string? PersonsText { get; init; }

    private Link[] _persons = Array.Empty<Link>();
    public required Link[] Persons { 
        get => _persons;
        init
        {
            if (value is not null)
            {
                _persons = value;
            }
        } 
    }
    private Link[] _organizations = Array.Empty<Link>();
    public required Link[] Organizations { 
        get => _organizations;
        init
        {
            if(value is not null)
            {
                _organizations = value;
            }
        }
    }
}
