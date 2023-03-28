using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.ViewModel;
using PoundPupLegacy.ViewModel.Readers;
using System.Data;

namespace PoundPupLegacy.Services.Implementation;

public record SubgroupService : ISubgroupService
{
    private readonly NpgsqlConnection _connection;
    private readonly IDatabaseReaderFactory<SubgroupsDocumentReader> _subgroupsDocumentReaderFactory;

    public SubgroupService(
        NpgsqlConnection connection,
        IDatabaseReaderFactory<SubgroupsDocumentReader> subgroupsDocumentReaderFactory)
    {
        _connection = connection;
        _subgroupsDocumentReaderFactory = subgroupsDocumentReaderFactory;
    }

    public async Task<SubgroupPagedList?> GetSubGroupPagedList(int userId, int subgroupId, int limit, int offset)
    {
        
        try {
            await _connection.OpenAsync();
            await using var reader = await _subgroupsDocumentReaderFactory.CreateAsync(_connection);
            return await reader.ReadAsync(new SubgroupsDocumentReader.SubgroupDocumentRequest {
                UserId = userId, 
                SubgroupId = subgroupId, 
                Limit = limit, 
                Offset = offset

            });
        }
        finally {
            if(_connection.State == ConnectionState.Open) {
                await _connection.CloseAsync();
            }
            
        }
    }
}
