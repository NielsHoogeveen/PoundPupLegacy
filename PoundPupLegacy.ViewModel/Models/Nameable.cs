namespace PoundPupLegacy.ViewModel.Models;

public abstract record NameableBase: DocumentableBase, Nameable
{
    public required string Description { get; init; }

    private BasicLink[] subTopics = Array.Empty<BasicLink>();
    public required BasicLink[] SubTopics {
        get => subTopics;
        init {
            if (value is not null) {
                subTopics = value;
            }
        }
    }

    private BasicLink[] superTopics = Array.Empty<BasicLink>();
    public required BasicLink[] SuperTopics {
        get => superTopics;
        init {
            if (value is not null) {
                superTopics = value;
            }
        }
    }
    private RelatedCaseType[] cases = Array.Empty<RelatedCaseType>();
    public RelatedCaseType[] Cases {
        get => cases;
        init {
            if (value is not null) {
                cases = value;
            }
        }
    }
}
public interface Nameable : Documentable
{
    string Description { get; }

    BasicLink[] SubTopics { get; }
    BasicLink[] SuperTopics { get; }
    RelatedCaseType[] Cases { get; }

}
