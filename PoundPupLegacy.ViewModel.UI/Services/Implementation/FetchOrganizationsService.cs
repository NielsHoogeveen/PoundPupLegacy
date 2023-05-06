﻿using Npgsql;
using PoundPupLegacy.ViewModel.Readers;
using System.Data;
using SearchOption = PoundPupLegacy.Common.SearchOption;

namespace PoundPupLegacy.ViewModel.UI.Services.Implementation;

internal sealed class FetchOrganizationsService : IFetchOrganizationsService
{
    private readonly NpgsqlConnection _connection;
    private readonly ISingleItemDatabaseReaderFactory<OrganizationsDocumentReaderRequest, OrganizationSearch> _organizationsDocumentReaderFactory;
    public FetchOrganizationsService(
        IDbConnection connection,
        ISingleItemDatabaseReaderFactory<OrganizationsDocumentReaderRequest, OrganizationSearch> organizationsDocumentReaderFactory)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        _connection = (NpgsqlConnection)connection;

        _organizationsDocumentReaderFactory = organizationsDocumentReaderFactory;
    }

    public async Task<OrganizationSearch> FetchOrganizations(int userId, int tenantId, int pageSize, int pageNumber, string searchTerm, SearchOption searchOption, int? organizationTypeId, int? countryId)
    {

        var offset = (pageNumber - 1) * pageSize;
        try {
            await _connection.OpenAsync();
            await using var reader = await _organizationsDocumentReaderFactory.CreateAsync(_connection);
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
        }
        finally {
            if (_connection.State == ConnectionState.Open) {
                await _connection.CloseAsync();
            }
        }
    }
}