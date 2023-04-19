using Microsoft.AspNetCore.WebUtilities;
using PoundPupLegacy.ViewModel.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.Razor.Internal;

namespace PoundPupLegacy.Components;

public abstract class PagedViewerBase<TPagedListSettings, TListEntry>: ViewerBase
    where TPagedListSettings : PagedListSettings
    where TListEntry: ListEntry
{
    [RazorInject]
    protected NavigationManager? NavigationManager { get; set; }
    protected TListEntry[]? ListEntries { get; set; }

    protected abstract Task<IPagedList<TListEntry>?> GetListEntriesAsync();

    protected abstract int PageSize { get; }

    protected abstract TPagedListSettings PagedListSettings { get; }

    protected sealed override void OnInitialized()
    {

    }

    protected override async Task OnInitializedAsync()
    {
        PagedListSettings.PageSize = PageSize;

        if (NavigationManager is not null) {
            var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
            PagedListSettings.Path = uri.AbsolutePath;
            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("page", out var pageValue)) {
                if (int.TryParse(pageValue, out int providedPageNumber)) {
                    PagedListSettings.PageNumber = providedPageNumber;
                }
            }
            var list = await GetListEntriesAsync();
            if (list is not null) {
                ListEntries = list.Entries;
                PagedListSettings.NumberOfEntries = list.NumberOfEntries;
            }
        }
    }
}
