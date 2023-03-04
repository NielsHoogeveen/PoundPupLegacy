namespace PoundPupLegacy.ViewModel;

public record MemberOfCongress
{
    public required string Name { get; init; }

    public required string Path { get; init; }

    public required string FilePathPortrait { get; init; }


    private Link[] letters = Array.Empty<Link>();
    public required Link[] Letters {
        get => letters;
        init {
            if (value is not null) {
                letters = value;
            }
        }
    }
    private Link[] bills = Array.Empty<Link>();
    public required Link[] Bills {
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
