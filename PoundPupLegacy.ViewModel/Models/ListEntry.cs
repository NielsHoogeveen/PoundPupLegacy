namespace PoundPupLegacy.ViewModel.Models;

public abstract record ListEntryBase: LinkBase, ListEntry
{

}
public interface ListEntry : Link
{
}
