using PoundPupLegacy.Common.UI.Components;

namespace PoundPupLegacy.ViewModel.UI.Components;

public abstract class PagedViewerBase<TPagedListSettings, TListEntry> : ViewerBase
    where TPagedListSettings : PagedListSettings
    where TListEntry : ListEntry
{

    [Inject]
    public required NavigationManager NavigationManager { get; init; }
    protected TListEntry[] ListEntries { get; set; } = Array.Empty<TListEntry>();

    protected abstract Task<IPagedList<TListEntry>> GetListEntriesAsync();

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
        await InitializeAsync(false);
    }

    protected virtual async Task InitializeAsync(bool isReloading)
    {
        if (!isReloading) {
            PagedListSettings.PageSize = PageSize;
            var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
            PagedListSettings.Path = uri.AbsolutePath;
            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("page", out var pageValue)) {
                if (int.TryParse(pageValue, out int providedPageNumber)) {
                    PagedListSettings.PageNumber = providedPageNumber;
                }
            }
        }
        var list = await GetListEntriesAsync();
        if (list is not null) {
            ListEntries = list.Entries;
            PagedListSettings.NumberOfEntries = list.NumberOfEntries;
        }

    }

    protected async Task Reload()
    {
        await InitializeAsync(true);
        var url = PagedListSettings.Path + "?" + PagedListSettings.GetQueryString(PagedListSettings.PageNumber);
        NavigationManager.NavigateTo(url);
        StateHasChanged();
    }

    protected async Task SelectPage(int pageNumber)
    {
        PagedListSettings.PageNumber = pageNumber;
        await Reload();
    }
}
