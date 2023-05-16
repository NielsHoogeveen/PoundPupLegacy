namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(FathersRightsViolationCases))]
public partial class FathersRightsViolationCasesJsonContext : JsonSerializerContext { }

public sealed record FathersRightsViolationCases: TermedList<FathersRightsViolationCaseList, CaseListEntry> 
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
    public required FathersRightsViolationCaseList Items { get; init; }

}
