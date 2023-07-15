using PoundPupLegacy.Common;
using PoundPupLegacy.DomainModel;
using Quartz;
using System.Data;
using System.Data.Common;

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
    private IDbConnection _dbConnection;
    private ILogger<NodeAccessService> _logger;

    private SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
    public NodeAccessService(
        IDatabaseInserterFactory<NodeAccess> nodeAccessInserterFactory,
        ILogger<NodeAccessService> logger,
        IDbConnection dbConnection)
    {
        _nodeAccessInserterFactory = nodeAccessInserterFactory;
        _dbConnection = dbConnection;
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
            _dbConnection.Open();
            try {
                _logger.LogInformation($"Flushing {lst.Count} entries to node access table");
                await using var inserter = await _nodeAccessInserterFactory.CreateAsync(_dbConnection);
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
                _dbConnection.Close();
            }
        }
    }

    public async Task Execute(IJobExecutionContext context)
    {
        await Flush();
    }
}
