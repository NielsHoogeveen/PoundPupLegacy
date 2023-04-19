using PoundPupLegacy.ViewModel.Models;
namespace PoundPupLegacy.Components;

public abstract class PagedViewer<TListEntry>: PagedViewerBase<PagedListSettings, TListEntry>
    where TListEntry : ListEntry
{
    protected sealed override PagedListSettings PagedListSettings { get; } = new PagedListSettings();

    protected sealed override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
    }
}
