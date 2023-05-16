namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(ChildTraffickingCases))]
public partial class ChildTraffickingCasesJsonContext : JsonSerializerContext { }

public sealed record ChildTraffickingCases : TermedList<ChildTraffickingCaseList, CaseListEntry>
{
    private SelectionItem[] termNames = Array.Empty<SelectionItem>();
    public SelectionItem[] TermNames {
        get => termNames;
        set {
            if (value != null) {
                termNames = value;
            }
        }
    }
    public required ChildTraffickingCaseList Items { get; init; }

}
