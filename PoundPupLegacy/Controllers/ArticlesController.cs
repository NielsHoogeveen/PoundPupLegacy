using Microsoft.AspNetCore.Mvc;
using PoundPupLegacy.Services;

namespace PoundPupLegacy.Controllers;

[Route("articles")]
public class ArticlesController : Controller
{

    const string TERM_NAME_PREFIX = "term-name-";
    private FetchArticlesService _fetchArticlesService;
    public ArticlesController(FetchArticlesService fetchArticlesService)
    {
        _fetchArticlesService = fetchArticlesService;
    }

    private IEnumerable<int> GetTermIds(IEnumerable<string> values)
    {
        foreach (var term in values) 
        {
            if (term.StartsWith(TERM_NAME_PREFIX))
            {
                var remainder = term.Substring(TERM_NAME_PREFIX.Length);
                if(int.TryParse(remainder, out int termId))
                {
                    yield return termId;
                }
            }
        }
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {

        var query = this.HttpContext.Request.Query;
        var termIds = query == null? new List<int>(): GetTermIds(query.Keys).ToList();
        var articles = termIds.Any() ? await _fetchArticlesService.GetArticles(termIds) : await _fetchArticlesService.GetArticles();
        return View("Articles", articles);
    }

}
