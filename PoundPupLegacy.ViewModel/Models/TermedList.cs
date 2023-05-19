namespace PoundPupLegacy.ViewModel.Models;

public abstract record TermedListBase<TList, TEntry>: TermedList<TList, TEntry>
    where TList : IPagedList<TEntry>
    where TEntry : ListEntry
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
    public required TList Items { get; init; }

}

public interface TermedList<TList, TEntry>
    where TList : IPagedList<TEntry>
    where TEntry : ListEntry
{
    SelectionItem[] TermNames { get; }

    TList Items { get; }
}
