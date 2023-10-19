using Microsoft.AspNetCore.Mvc;
using PoundPupLegacy.Services;

namespace PoundPupLegacy.Controllers
{
    public class SiteMapController(ISiteDataService siteDataService, ISiteMapService siteMapService) : Controller
    {

        [Route("sitemap.xml")]
        public async Task<IActionResult> Index()
        {
            var request = HttpContext.Request;
            return new ContentResult {
                Content = await siteMapService.GetSiteMapIndex(),
                ContentType = "application/xml"
            };
        }
        [Route("sitemap{index}.xml")]
        public async Task<IActionResult> Index(int index)
        {
            var request = HttpContext.Request;
            return new ContentResult {
                Content = await siteMapService.GetSiteMap(index),
                ContentType = "application/xml"
            };
        }
        [Route("robots.txt")]
        public IActionResult Robots()
        {
            var request = HttpContext.Request;
            var domainName = siteDataService.GetDomainName();
            var text = $"""
                User-agent: Googlebot

                User-agent: *
                Allow: /

                Sitemap: http://{domainName}/sitemap.xml
                """;

            return new ContentResult {
                Content = text,
                ContentType = "text"
            };
        }


    }
}
