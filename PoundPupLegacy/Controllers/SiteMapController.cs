using Microsoft.AspNetCore.Mvc;
using PoundPupLegacy.Services;

namespace PoundPupLegacy.Controllers
{
    [Route("sitemap.xml")]
    public class SiteMapController(ISiteDataService siteDataService, ISiteMapService siteMapService) : Controller
    {
        
        public async Task<IActionResult> Index()
        {
            var request = HttpContext.Request;
            Response.ContentType = "application/xml";
            var uri = new UriBuilder(request.Scheme, request.Host.Host, request.Host.Port ?? -1, request.PathBase).Uri;
            var tenantId = siteDataService.GetTenantId(uri);
            using (StreamWriter writer = new StreamWriter(Response.Body, System.Text.Encoding.UTF8)) {
                await siteMapService.WriteSiteMap(tenantId, writer);
                return new EmptyResult();
            }
        }
    }
}
