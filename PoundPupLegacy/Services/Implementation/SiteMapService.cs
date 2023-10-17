using PoundPupLegacy.Common;
using PoundPupLegacy.Models;
using PoundPupLegacy.Readers;
using System.Data;
using System.Text;

namespace PoundPupLegacy.Services.Implementation;

internal class SiteMapService(
    IDbConnection connection,
    ILogger<SiteDataService> _logger,
    IEnumerableDatabaseReaderFactory<SiteMapReaderRequest, SiteMapElement> siteMapReaderFactory,
    IMandatorySingleItemDatabaseReaderFactory<SiteMapCountReaderRequest, SiteMapCount> siteMapCountReaderFactory
) : DatabaseService(connection, _logger), ISiteMapService
{
    public async Task<string> GetSiteMapIndex(int tenantId)
    {
        var count = await WithConnection(async (connection) => {
            await using var siteMapCountReader = await siteMapCountReaderFactory.CreateAsync(connection);
            return await siteMapCountReader.ReadAsync(new SiteMapCountReaderRequest { TenantId = tenantId });
        });
        var strBuilder = new StringBuilder();
        foreach (var index in Enumerable.Range(0, count.Count / 5000 + 1)) {
            strBuilder.Append($"""
            <sitemap>
                <loc>http://{count.DomainName}/sitemap{index}.xml</loc>
                </sitemap>
            """);
        }
        var doc = $"""
            <?xml version="1.0" encoding="UTF-8"?>
            <sitemapindex xmlns="http://www.sitemaps.org/schemas/sitemap/0.9">
                {strBuilder}                
            </sitemapindex>
            """;
        var utf8 = Encoding.UTF8;
        byte[] utfBytes = utf8.GetBytes(doc);
        return utf8.GetString(utfBytes, 0, utfBytes.Length);
    }

    public async Task<string> GetSiteMap(int tenantId, int index)
    {
        var strBuilder = new StringBuilder();
        var elements = await WithConnection(async (connection) => {
            await using var siteMapReader = await siteMapReaderFactory.CreateAsync(connection);
            await foreach (var siteMapElement in siteMapReader.ReadAsync(new SiteMapReaderRequest { TenantId = tenantId, Index = index})) {

                if (siteMapElement.ChangeFrequency is not null) {

                    strBuilder.Append($"""
                    <url>
                        <loc>{siteMapElement.Path}</loc>
                        <changefreq>{siteMapElement.ChangeFrequency}</changefreq>
                    </url>
                    """
                    );
                }
                if (siteMapElement.LastChanged is not null) {
                    strBuilder.Append($"""
                    <url>
                        <loc>{siteMapElement.Path}</loc>
                        <lastmod>{siteMapElement.LastChanged.Value.ToString("yyyy-MM-dd")}</lastmod>
                    </url>
                    """
                    );
                }
            }
            return strBuilder.ToString();
        });

        var doc = $"""
            <?xml version="1.0" encoding="UTF-8"?>
            <urlset xmlns="http://www.sitemaps.org/schemas/sitemap/0.9">
                {elements}
            </urlset>
            """;
        var utf8 = Encoding.UTF8;
        byte[] utfBytes = utf8.GetBytes(doc);
        return utf8.GetString(utfBytes, 0, utfBytes.Length);
    }
}
