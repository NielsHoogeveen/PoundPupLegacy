using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.DomainModel;
using Quartz;

namespace PoundPupLegacy.Services.Implementation;

public class NodeAccessService : INodeAccessService
{
    public record NodeAccessToWrite
    {
        public required DateTime DateTime { get; init; }
        public required int UserId { get; init; }
        public required int TenantId { get; init; }
        public required int NodeId { get; init; }
    }

    private List<NodeAccessToWrite> nodeAccessesToWrites = new List<NodeAccessToWrite>();

    private IDatabaseInserterFactory<NodeAccess> _nodeAccessInserterFactory;
    private NpgsqlDataSource _dataSource;
    private ILogger<NodeAccessService> _logger;

    private SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
    public NodeAccessService(
        IDatabaseInserterFactory<NodeAccess> nodeAccessInserterFactory,
        ILogger<NodeAccessService> logger,
        NpgsqlDataSource dataSource)
    {
        _nodeAccessInserterFactory = nodeAccessInserterFactory;
        _dataSource = dataSource;
        _logger = logger;
    }

    

    public async Task Log(int userId, int tenantId, int nodeId)
    {
        await semaphore.WaitAsync();
        try {
            nodeAccessesToWrites.Add(new NodeAccessToWrite { 
                DateTime = DateTime.UtcNow, 
                UserId = userId, 
                TenantId = tenantId, 
                NodeId = nodeId 
            });
        }
        finally {
            semaphore.Release();
        }
    }

    private async Task Flush()
    {
        List<NodeAccessToWrite>? lst = null;
        await semaphore.WaitAsync();
        try {
            lst = nodeAccessesToWrites;
            nodeAccessesToWrites = new List<NodeAccessToWrite>();
        }
        finally {
            semaphore.Release();
        }
        if(lst is not null) {
            var connection = _dataSource.CreateConnection();
            connection.Open();
            try {
                _logger.LogInformation($"Flushing {lst.Count} entries to node access table");
                await using var inserter = await _nodeAccessInserterFactory.CreateAsync(connection);
                foreach (var elem in lst) {
                    await inserter.InsertAsync(new NodeAccess {
                        DateTime = elem.DateTime,
                        UserId = elem.UserId,
                        TenantId = elem.TenantId,
                        NodeId = elem.NodeId,
                        Identification = new Identification.Possible { Id = null }
                    });
                }
            }
            finally {
                connection.Close();
            }
        }
    }

    public async Task Execute(IJobExecutionContext context)
    {
        await Flush();
    }
}
