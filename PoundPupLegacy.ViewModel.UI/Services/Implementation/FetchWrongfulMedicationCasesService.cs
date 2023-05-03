using Npgsql;
using PoundPupLegacy.ViewModel.Readers;
using System.Data;

namespace PoundPupLegacy.ViewModel.UI.Services.Implementation;
internal sealed class FetchWrongfulMedicationCasesService : IFetchWrongfulMedicationCasesService
{
    private NpgsqlConnection _connection;
    private readonly ISingleItemDatabaseReaderFactory<WrongfulMedicationCasesDocumentReaderRequest, WrongfulMedicationCases> _abuseCasesDocumentReaderFactory;

    public FetchWrongfulMedicationCasesService(
        IDbConnection connection,
        ISingleItemDatabaseReaderFactory<WrongfulMedicationCasesDocumentReaderRequest, WrongfulMedicationCases> abuseCasesDocumentReaderFactory
        )
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        _connection = (NpgsqlConnection)connection;

        _abuseCasesDocumentReaderFactory = abuseCasesDocumentReaderFactory;
    }

    public async Task<WrongfulMedicationCases> FetchCases(int pageSize, int pageNumber, int tenantId, int userId, int[] selectedTerms)
    {
        var startIndex = (pageNumber - 1) * pageSize;

        try {
            await _connection.OpenAsync();
            await using var reader = await _abuseCasesDocumentReaderFactory.CreateAsync(_connection);
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
        }
        finally {
            if (_connection.State == ConnectionState.Open) {
                await _connection.CloseAsync();
            }
        }
    }
}
