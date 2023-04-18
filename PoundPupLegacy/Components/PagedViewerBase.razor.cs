using PoundPupLegacy.ViewModel.Models;

namespace PoundPupLegacy.Components;

public abstract partial class PagedViewerBase<TPagedListSettings, TListEntry>
    where TPagedListSettings : PagedListSettings
    where TListEntry: ListEntry
{
}
