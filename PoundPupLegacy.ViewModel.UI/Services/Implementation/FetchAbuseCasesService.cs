using Npgsql;
using PoundPupLegacy.ViewModel.Readers;
using System.Data;

namespace PoundPupLegacy.ViewModel.UI.Services.Implementation;
internal sealed class FetchAbuseCasesService : IFetchAbuseCasesService
{
    private NpgsqlConnection _connection;
    private readonly ISingleItemDatabaseReaderFactory<AbuseCasesDocumentReaderRequest, AbuseCases> _abuseCasesDocumentReaderFactory;

    public FetchAbuseCasesService(
        IDbConnection connection,
        ISingleItemDatabaseReaderFactory<AbuseCasesDocumentReaderRequest, AbuseCases> abuseCasesDocumentReaderFactory
        )
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        _connection = (NpgsqlConnection)connection;

        _abuseCasesDocumentReaderFactory = abuseCasesDocumentReaderFactory;
    }

    public async Task<AbuseCases> FetchCases(int pageSize, int pageNumber, int tenantId, int userId, int[] selectedTerms)
    {
        var startIndex = (pageNumber - 1) * pageSize;

        try {
            await _connection.OpenAsync();
            await using var reader = await _abuseCasesDocumentReaderFactory.CreateAsync(_connection);
            var cases = await reader.ReadAsync(new AbuseCasesDocumentReaderRequest {
                StartIndex = startIndex,
                Length = pageSize,
                TenantId = tenantId,
                UserId = userId,
                SelectedTerms = selectedTerms
            });
            var result = cases is not null
                ? cases
                : new AbuseCases {
                    Items = new AbuseCaseList {
                        Entries = Array.Empty<CaseListEntry>(),
                        NumberOfEntries = 0,
                    },
                    TermNames = Array.Empty<SelectionItem>()
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
