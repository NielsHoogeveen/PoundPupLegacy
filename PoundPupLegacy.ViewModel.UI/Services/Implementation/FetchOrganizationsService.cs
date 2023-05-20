using Microsoft.Extensions.Logging;
using PoundPupLegacy.ViewModel.Readers;
using System.Data;
using SearchOption = PoundPupLegacy.Common.SearchOption;

namespace PoundPupLegacy.ViewModel.UI.Services.Implementation;

internal sealed class FetchOrganizationsService(
    IDbConnection connection,
    ILogger<FetchOrganizationsService> logger,
    ISingleItemDatabaseReaderFactory<OrganizationsDocumentReaderRequest, OrganizationSearch> organizationsDocumentReaderFactory
) : DatabaseService(connection, logger), IFetchOrganizationsService
{
    public async Task<OrganizationSearch> FetchOrganizations(int userId, int tenantId, int pageSize, int pageNumber, string searchTerm, SearchOption searchOption, int? organizationTypeId, int? countryId)
    {

        var offset = (pageNumber - 1) * pageSize;
        return await WithConnection(async (connection) => {
            await using var reader = await organizationsDocumentReaderFactory.CreateAsync(connection);
            var organizations = await reader.ReadAsync(new OrganizationsDocumentReaderRequest {
                UserId = userId,
                TenantId = tenantId,
                Limit = pageSize,
                Offset = offset,
                SearchTerm = searchTerm,
                SearchOption = searchOption,
                OrganizationTypeId = organizationTypeId,
                CountryId = countryId
            });
            if (organizations is null) {
                return new OrganizationSearch {
                    Organizations = new Organizations {
                        NumberOfEntries = 0,
                        Entries = Array.Empty<OrganizationListEntry>()
                    },
                    OrganizationTypes = Array.Empty<SelectionItem>(),
                    Countries = Array.Empty<SelectionItem>()
                };
            }
            else {
                return organizations;
            }
        });
    }
}
