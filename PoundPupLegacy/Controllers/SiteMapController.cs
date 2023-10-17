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
            var uri = new UriBuilder(request.Scheme, request.Host.Host, request.Host.Port ?? -1, request.PathBase).Uri;
            var tenantId = siteDataService.GetTenantId(uri);
            return new ContentResult {
                Content = await siteMapService.GetSiteMapIndex(tenantId),
                ContentType = "application/xml"
            };
        }
        [Route("sitemap{index}.xml")]
        public async Task<IActionResult> Index(int index)
        {
            var request = HttpContext.Request;
            var uri = new UriBuilder(request.Scheme, request.Host.Host, request.Host.Port ?? -1, request.PathBase).Uri;
            var tenantId = siteDataService.GetTenantId(uri);
            return new ContentResult {
                Content = await siteMapService.GetSiteMap(tenantId, index),
                ContentType = "application/xml"
            };
        }

    }
}
