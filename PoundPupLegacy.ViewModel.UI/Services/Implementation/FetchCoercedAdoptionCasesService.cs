using Npgsql;
using PoundPupLegacy.ViewModel.Readers;
using System.Data;

namespace PoundPupLegacy.ViewModel.UI.Services.Implementation;
internal sealed class FetchCoercedAdoptionCasesService : IFetchCoercedAdoptionCasesService
{
    private NpgsqlConnection _connection;
    private readonly ISingleItemDatabaseReaderFactory<CoercedAdoptionCasesDocumentReaderRequest, CoercedAdoptionCases> _abuseCasesDocumentReaderFactory;

    public FetchCoercedAdoptionCasesService(
        IDbConnection connection,
        ISingleItemDatabaseReaderFactory<CoercedAdoptionCasesDocumentReaderRequest, CoercedAdoptionCases> abuseCasesDocumentReaderFactory
        )
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        _connection = (NpgsqlConnection)connection;

        _abuseCasesDocumentReaderFactory = abuseCasesDocumentReaderFactory;
    }

    public async Task<CoercedAdoptionCases> FetchCases(int pageSize, int pageNumber, int tenantId, int userId, int[] selectedTerms)
    {
        var startIndex = (pageNumber - 1) * pageSize;

        try {
            await _connection.OpenAsync();
            await using var reader = await _abuseCasesDocumentReaderFactory.CreateAsync(_connection);
            var cases = await reader.ReadAsync(new CoercedAdoptionCasesDocumentReaderRequest {
                Length = pageSize,
                StartIndex = startIndex,
                TenantId = tenantId,
                UserId = userId,
                SelectedTerms = selectedTerms
            });
            var result = cases is not null
                ? cases
                : new CoercedAdoptionCases {
                    TermNames = Array.Empty<SelectionItem>(),
                    Items = new CoercedAdoptionCaseList {
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
