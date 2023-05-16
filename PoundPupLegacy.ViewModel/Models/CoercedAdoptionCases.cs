namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(CoercedAdoptionCases))]
public partial class CoercedAdoptionCasesJsonContext : JsonSerializerContext { }

public sealed record CoercedAdoptionCases: TermedList<CoercedAdoptionCaseList, CaseListEntry> 
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
    public required CoercedAdoptionCaseList Items { get; init; }

}
