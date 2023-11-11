namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(MemberOfCongress))]
public partial class MemberOfCongressJsonContext : JsonSerializerContext { }

public sealed record MemberOfCongress : LinkBase 
{
    public required string FilePathPortrait { get; init; }
    public required DateTime From { get; init; }
    public required DateTime To { get; init; }

    private BasicLink[] letters = Array.Empty<BasicLink>();
    public required BasicLink[] Letters {
        get => letters;
        init {
            if (value is not null) {
                letters = value;
            }
        }
    }
    private BillAndAction[] bills = Array.Empty<BillAndAction>();
    public required BillAndAction[] Bills {
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
}

public record BillAndAction
{
    public required BasicLink Bill { get; init; }

    public required BasicLink BillAction { get; init; }

}
