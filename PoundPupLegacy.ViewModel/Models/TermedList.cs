namespace PoundPupLegacy.ViewModel.Models;

public interface TermedList<TList, TEntry>
    where TList : IPagedList<TEntry>
    where TEntry : ListEntry
{
    SelectionItem[] TermNames { get; }

    TList Items { get; }
}
