﻿using Microsoft.Extensions.Logging;
using PoundPupLegacy.ViewModel.Readers;
using System.Data;

namespace PoundPupLegacy.ViewModel.UI.Services.Implementation;
internal sealed class FetchWrongfulMedicationCasesService(
    IDbConnection connection,
    ILogger<FetchWrongfulMedicationCasesService> logger,
    ISingleItemDatabaseReaderFactory<WrongfulMedicationCasesDocumentReaderRequest, WrongfulMedicationCases> abuseCasesDocumentReaderFactory
) : DatabaseService(connection, logger), IFetchWrongfulMedicationCasesService
{
    public async Task<WrongfulMedicationCases> FetchCases(int pageSize, int pageNumber, int tenantId, int userId, int[] selectedTerms)
    {
        var startIndex = (pageNumber - 1) * pageSize;

        return await WithConnection(async (connection) => {
            await using var reader = await abuseCasesDocumentReaderFactory.CreateAsync(connection);
            var cases = await reader.ReadAsync(new WrongfulMedicationCasesDocumentReaderRequest {
                Length = pageSize,
                StartIndex = startIndex,
                TenantId = tenantId,
                UserId = userId,
                SelectedTerms = selectedTerms
            });
            var result = cases is not null
                ? cases
                : new WrongfulMedicationCases {
                    TermNames = Array.Empty<SelectionItem>(),
                    Items = new WrongfulMedicationCaseList {
                        Entries = Array.Empty<CaseListEntry>(),
                        NumberOfEntries = 0,
                    }
                };
            return result;
        });
    }
}
