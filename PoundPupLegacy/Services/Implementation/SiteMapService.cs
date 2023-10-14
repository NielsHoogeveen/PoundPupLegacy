using PoundPupLegacy.Common;
using PoundPupLegacy.Models;
using PoundPupLegacy.Readers;
using System.Data;

namespace PoundPupLegacy.Services.Implementation;

internal class SiteMapService(
    IDbConnection connection,
    ILogger<SiteDataService> _logger,
    IEnumerableDatabaseReaderFactory<SiteMapReaderRequest, SiteMapElement> siteMapReaderFactory
) : DatabaseService(connection, _logger), ISiteMapService
{
    public async Task WriteSiteMap(int tenantId, StreamWriter writer)
    {

        await writer.WriteAsync("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        await writer.WriteAsync("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">");
        int i = 0;
        await WithConnection(async (connection) => {
            await using var siteMapReader = await siteMapReaderFactory.CreateAsync(connection);
            await foreach (var siteMapElement in siteMapReader.ReadAsync(new SiteMapReaderRequest { TenantId = tenantId})) {
                await writer.WriteAsync($"<url>");
                await writer.WriteAsync($"<loc>{siteMapElement.Path}</loc>");
                if(siteMapElement.ChangeFrequency is not null) {
                    await writer.WriteAsync($"<changefreq>{siteMapElement.ChangeFrequency}</changefreq>");
                }
                if (siteMapElement.LastChanged is not null) {
                    await writer.WriteAsync($"<lastmod>{siteMapElement.LastChanged.Value.ToString("yyyy-MM-dd")}</lastmod>");
                }
                await writer.WriteAsync($"</url>");
                if (i % 100 == 0) {
                    await writer.FlushAsync();
                }
                i++;
            }
            return Unit.Instance;
        });
        await writer.WriteAsync($"</urlset>");
        await writer.FlushAsync();
    }
}
