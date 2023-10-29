﻿using Microsoft.Extensions.Logging;
using Npgsql;
using PoundPupLegacy.ViewModel.Readers;
using System.Data;

namespace PoundPupLegacy.ViewModel.UI.Services.Implementation;
internal sealed class FetchChildTraffickingCasesService(
    NpgsqlDataSource dataSoruce,
    ILogger<FetchChildTraffickingCasesService> logger,
    ISingleItemDatabaseReaderFactory<ChildTraffickingCasesDocumentReaderRequest, ChildTraffickingCases> abuseCasesDocumentReaderFactory
) : DatabaseService(dataSoruce, logger), IFetchChildTraffickingCasesService
{

    public async Task<ChildTraffickingCases> FetchCases(int pageSize, int pageNumber, int tenantId, int userId, int[] selectedTerms)
    {
        var startIndex = (pageNumber - 1) * pageSize;

        return await WithConnection(async (connection) => {
            await using var reader = await abuseCasesDocumentReaderFactory.CreateAsync(connection);
            var cases = await reader.ReadAsync(new ChildTraffickingCasesDocumentReaderRequest {
                Length = pageSize,
                StartIndex = startIndex,
                TenantId = tenantId,
                UserId = userId,
                SelectedTerms = selectedTerms
            });
            var result = cases is not null
                ? cases
                : new ChildTraffickingCases {
                    TermNames = Array.Empty<SelectionItem>(),
                    Items = new ChildTraffickingCaseList {
                        Entries = Array.Empty<CaseTeaserListEntry>(),
                        NumberOfEntries = 0,
                    }
                };
            return result;
        });
    }
}
