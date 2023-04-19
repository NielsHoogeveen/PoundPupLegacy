namespace PoundPupLegacy.ViewModel.Models;

public interface IPagedList
{
    public int NumberOfEntries { get; }
}

public interface IPagedList<T> : IPagedList
    where T : ListEntry
{
    public T[] Entries { get; }
}

public record PagedListSettings
{
    public int NumberOfEntries { get; set; } = 0;
    public int PageNumber { get; set; } = 1;
    public int NumberOfPages => NumberOfEntries / PageSize + (NumberOfEntries % PageSize == 0 ? 0 : 1);
    public virtual string QueryString { get; set; } = "";
    public string Path { get; set; } = "";
    public int PageSize { get; set; } = 25;
    public int StartPage => GetStartPage();

    public int EndPage => GetEndPage();

    private int GetStartPage()
    {
        if (PageNumber <= 4) {
            return 1;
        }
        if (PageNumber > NumberOfPages - 4 && NumberOfPages < 9) {
            return NumberOfPages;

        }
        if (PageNumber > NumberOfPages - 4) {
            return NumberOfPages - 9;
        }
        return PageNumber - 4;
    }
    private int GetEndPage()
    {
        if (NumberOfPages - PageNumber < 5) {
            return NumberOfPages + 1;
        }
        if (PageNumber < 6) {
            return Math.Min(10, NumberOfPages);
        }
        return PageNumber + 5;
    }
    public virtual string GetQueryString(int pageNumber)
    {
        if (string.IsNullOrEmpty(QueryString)) {
            return $"page={pageNumber}";
        }
        return $"{QueryString}&page={pageNumber}";
    }
}

public record PagedSearchListSettings : PagedListSettings
{
    public string SearchTerm { get; set; } = string.Empty;
    public SearchOption SearchOption { get; set; }

    private string SearchOptionAsText(SearchOption searchOption)
    {
        return searchOption switch {
            SearchOption.Contains => "contains",
            SearchOption.StartsWith => "starts",
            SearchOption.EndsWith => "ends",
            SearchOption.IsEqualTo => "is_equal_to",
            _ => "contains",
        };
    }
    public override string GetQueryString(int pageNumber)
    {
        return base.GetQueryString(pageNumber) + $"&search_term={SearchTerm}&search_option={SearchOptionAsText(SearchOption)}";
    }
}
