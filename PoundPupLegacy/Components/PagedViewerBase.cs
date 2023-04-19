using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using PoundPupLegacy.ViewModel.Models;

namespace PoundPupLegacy.Components;

public abstract class PagedViewerBase<TPagedListSettings, TListEntry> : ViewerBase
    where TPagedListSettings : PagedListSettings
    where TListEntry : ListEntry
{

    /*
     * Inject the NavigationManager and suppress the null warning. 
     * Once the NavigationManager is injected, it will never be null.
     * In OnInializedAsync the NavigationManager has already been injected.
     */
    [Inject]
#pragma warning disable CS8618 
    protected NavigationManager NavigationManager { get; set; }
#pragma warning restore CS8618 
    protected TListEntry[]? ListEntries { get; set; }

    protected abstract Task<IPagedList<TListEntry>?> GetListEntriesAsync();

    protected abstract int PageSize { get; }

    protected abstract TPagedListSettings PagedListSettings { get; }

    protected sealed override void OnInitialized()
    {

    }
    protected sealed override void OnAfterRender(bool firstRender)
    {
        base.OnAfterRender(firstRender);
    }
    protected sealed override void OnParametersSet()
    {
        base.OnParametersSet();
    }
    protected sealed override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();
    }

    protected sealed override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
    }

    protected sealed override bool ShouldRender()
    {
        return base.ShouldRender();
    }

    protected override async Task OnInitializedAsync()
    {
        PagedListSettings.PageSize = PageSize;

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
