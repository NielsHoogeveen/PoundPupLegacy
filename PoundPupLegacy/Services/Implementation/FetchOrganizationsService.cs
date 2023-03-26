using Npgsql;
using PoundPupLegacy.ViewModel;
using PoundPupLegacy.ViewModel.Readers;
using System.Data;
using SearchOption = PoundPupLegacy.ViewModel.SearchOption;

namespace PoundPupLegacy.Services.Implementation;

internal class FetchOrganizationsService : IFetchOrganizationsService
{
    private readonly NpgsqlConnection _connection;

    public FetchOrganizationsService(
        NpgsqlConnection connection)
    {
        _connection = connection;
    }

    public async Task<OrganizationSearch> FetchOrganizations(int userId, int tenantId, int limit, int offset, string searchTerm, SearchOption searchOption, int? organizationTypeId, int? countryId)
    {
        
        try {
            await _connection.OpenAsync();
            await using OrganizationsDocumentReader reader = await OrganizationsDocumentReader.CreateAsync(_connection); 
            return await reader.ReadAsync(new OrganizationsDocumentReader.OrganizationsDocumentRequest { 
                UserId = userId,
                TenantId = tenantId,
                Limit = limit,
                Offset = offset,
                SearchTerm = searchTerm,
                SearchOption = searchOption,
                OrganizationTypeId = organizationTypeId,
                CountryId = countryId
            });
        }
        finally {
            if (_connection.State == ConnectionState.Open) {
                await _connection.CloseAsync();
            }
        }
    }
}
