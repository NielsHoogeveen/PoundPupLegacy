namespace PoundPupLegacy.ViewModel.Models;

public record PartyCase
{
    public required string CasePartyTypeName { get; init; }

    private Link[] _cases = Array.Empty<Link>();
    public required Link[] Cases
    {
        get => _cases;
        init
        {
            if (value is not null)
            {
                _cases = value;
            }
        }
    }
}
