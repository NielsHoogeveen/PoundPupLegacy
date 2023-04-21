namespace PoundPupLegacy.ViewModel.Models;

public record MemberOfCongress
{
    public required string Name { get; init; }

    public required string Path { get; init; }

    public required string FilePathPortrait { get; init; }


    private BasicLink[] letters = Array.Empty<BasicLink>();
    public required BasicLink[] Letters {
        get => letters;
        init {
            if (value is not null) {
                letters = value;
            }
        }
    }
    private BasicLink[] bills = Array.Empty<BasicLink>();
    public required BasicLink[] Bills {
        get => bills;
        init {
            if (value is not null) {
                bills = value;
            }
        }
    }
    private PartyMembership[] parties = Array.Empty<PartyMembership>();
    public required PartyMembership[] Parties {
        get => parties;
        init {
            if (value is not null) {
                parties = value;
            }
        }
    }
    private CongressionalTerm[] terms = Array.Empty<CongressionalTerm>();
    public required CongressionalTerm[] Terms {
        get => terms;
        init {
            if (value is not null) {
                terms = value;
            }
        }
    }
}
