using Microsoft.AspNetCore.WebUtilities;
using PoundPupLegacy.ViewModel.Models;
using SearchOption = PoundPupLegacy.ViewModel.Models.SearchOption;

namespace PoundPupLegacy.Components;

public abstract partial class PagedSearchViewer<TListEntry> : PagedViewerBase<PagedSearchListSettings, TListEntry>
    where TListEntry : ListEntry
{
    protected sealed override PagedSearchListSettings PagedListSettings { get; } = new PagedSearchListSettings();

    protected sealed override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
        var parsedQuery = QueryHelpers.ParseQuery(uri.Query);
        if (parsedQuery.TryGetValue("search_term", out var search)) {
            PagedListSettings.SearchTerm = search.FirstOrDefault() ?? string.Empty;
        }
        if (parsedQuery.TryGetValue("search_option", out var searchOption)) {

            PagedListSettings.SearchOption = search.FirstOrDefault() switch {
                "contains" => SearchOption.Contains,
                "starts" => SearchOption.StartsWith,
                "ends" => SearchOption.EndsWith,
                "is_equal_to" => SearchOption.IsEqualTo,
                _ => SearchOption.Contains
            };
        }
    }
}
