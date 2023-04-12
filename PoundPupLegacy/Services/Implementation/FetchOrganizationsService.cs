using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.ViewModel;
using PoundPupLegacy.ViewModel.Readers;
using System.Data;
using SearchOption = PoundPupLegacy.ViewModel.SearchOption;

namespace PoundPupLegacy.Services.Implementation;

internal sealed class FetchOrganizationsService : IFetchOrganizationsService
{
    private readonly NpgsqlConnection _connection;
    private readonly IDatabaseReaderFactory<OrganizationsDocumentReader> _organizationsDocumentReaderFactory;
    public FetchOrganizationsService(
        IDbConnection connection,
        IDatabaseReaderFactory<OrganizationsDocumentReader> organizationsDocumentReaderFactory)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        _connection = (NpgsqlConnection)connection;

        _organizationsDocumentReaderFactory = organizationsDocumentReaderFactory;
    }

    public async Task<OrganizationSearch> FetchOrganizations(int userId, int tenantId, int limit, int offset, string searchTerm, SearchOption searchOption, int? organizationTypeId, int? countryId)
    {

        try {
            await _connection.OpenAsync();
            await using var reader = await _organizationsDocumentReaderFactory.CreateAsync(_connection);
            var organizations = await reader.ReadAsync(new OrganizationsDocumentReader.Request {
                UserId = userId,
                TenantId = tenantId,
                Limit = limit,
                Offset = offset,
                SearchTerm = searchTerm,
                SearchOption = searchOption,
                OrganizationTypeId = organizationTypeId,
                CountryId = countryId
            });
            if (organizations is not null)
                return organizations;
            return new OrganizationSearch {
                Organizations = new Organizations {
                    Entries = Array.Empty<BasicListEntry>(),
                    NumberOfEntries = 0
                },
                Countries = Array.Empty<SelectionItem>(),
                OrganizationTypes = Array.Empty<SelectionItem>(),
            };
        }
        finally {
            if (_connection.State == ConnectionState.Open) {
                await _connection.CloseAsync();
            }
        }
    }
}
