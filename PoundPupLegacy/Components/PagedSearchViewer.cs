using Microsoft.AspNetCore.WebUtilities;
using PoundPupLegacy.ViewModel.Models;
using SearchOption = PoundPupLegacy.ViewModel.Models.SearchOption;

namespace PoundPupLegacy.Components;

public abstract partial class PagedSearchViewer<TListEntry> : PagedViewerBase<PagedSearchListSettings, TListEntry>
    where TListEntry : ListEntry
{
    protected sealed override PagedSearchListSettings PagedListSettings { get; } = new PagedSearchListSettings();


    
    protected sealed override async Task InitializeAsync(bool isReloading)
    {
        if (!isReloading) {
            var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
            var parsedQuery = QueryHelpers.ParseQuery(uri.Query);
            if (parsedQuery.TryGetValue("search_term", out var search)) {
                PagedListSettings.SearchTerm = search.FirstOrDefault() ?? string.Empty;
            }
            else {
                PagedListSettings.SearchTerm = string.Empty;
            }
            if (parsedQuery.TryGetValue("search_option", out var searchOption)) {

                PagedListSettings.SearchOption = searchOption.FirstOrDefault() switch {
                    "contains" => SearchOption.Contains,
                    "starts_with" => SearchOption.StartsWith,
                    "ends_with" => SearchOption.EndsWith,
                    "is_equal_to" => SearchOption.IsEqualTo,
                    _ => SearchOption.Contains
                };
            }
            else {
                PagedListSettings.SearchOption = SearchOption.Contains;
            }
        }
        await base.InitializeAsync(isReloading);
    }
    protected async Task OnSearch()
    {
        await Reload();
    }

}
