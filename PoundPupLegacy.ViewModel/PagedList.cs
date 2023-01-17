namespace PoundPupLegacy.ViewModel;

public static class PagedListExtensions
{

    public static int StartPage(this PagedList pl)
    {
        var res = pl.GetStartPage();
        return res;
    }
    public static int EndPage(this PagedList pl)
    {
        var res = pl.GetEndPage();
        return res;
    }

    private static int GetStartPage(this PagedList pl)
    {
        if (pl.PageNumber <= 4)
        {
            return 1;
        }
        if (pl.PageNumber > pl.NumberOfPages - 4 && pl.NumberOfPages < 9)
        {
            return pl.NumberOfPages;

        }
        if (pl.PageNumber > pl.NumberOfPages - 4)
        {
            return pl.NumberOfPages - 9;
        }
        return pl.PageNumber - 4;
    }
    private static int GetEndPage(this PagedList pl)
    {
        if(pl.NumberOfPages - pl.PageNumber < 5)
        {
            return pl.NumberOfPages + 1;
        }
        if(pl.PageNumber < 6)
        {
            return Math.Min(10, pl.NumberOfPages);
        }
        return pl.PageNumber + 5;
        
    }
    public static string GetQueryString(this PagedList pl, int pageNumbeer)
    {
        if (string.IsNullOrEmpty(pl.QueryString)) 
        {
            return $"page={pageNumbeer}";
        }
        return $"{pl.QueryString}&page={pageNumbeer}";
    }
}

public interface PagedList
{
    public int NumberOfEntries { get; set; }
    public int PageNumber { get; set; }
    public int NumberOfPages { get; set; }
    public string QueryString { get; set; }

    public string Path { get; }
}
