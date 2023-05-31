namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(TenantNodeDetails.ForUpdate))]
public partial class ExistingTenantNodeDetailsJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(TenantNodeDetails.ForUpdate))]
public partial class NewTenantNodeDetailsJsonContext : JsonSerializerContext { }

public abstract record TenantNodeDetails
{
    public abstract IEnumerable<TenantNode> TenantNodes { get; }

    public sealed record ForUpdate: TenantNodeDetails
    {
        private List<TenantNode.ToCreateForExistingNode> tenantNodesToAdd = new();

        public List<TenantNode.ToCreateForExistingNode> TenantNodesToAdd {
            get => tenantNodesToAdd;
            init {
                if (value is not null) {
                    tenantNodesToAdd = value;
                }
            }
        }
        private List<TenantNode.ToUpdate> tenantNodesToUpdate = new();

        public List<TenantNode.ToUpdate> TenantNodesToUpdate {
            get => tenantNodesToUpdate;
            init {
                if (value is not null) {
                    tenantNodesToUpdate = value;
                }
            }
        }

        public override IEnumerable<TenantNode> TenantNodes => GetTenantNodes();

        private IEnumerable<TenantNode> GetTenantNodes()
        {
            foreach (var elem in tenantNodesToUpdate) {
                yield return elem;
            }
            foreach (var elem in tenantNodesToAdd) {
                yield return elem;
            }
        }
    }
    public sealed record ForCreate: TenantNodeDetails
    {
        public static ForCreate EmptyInstance => new ForCreate {
            TenantNodesToAdd = new List<TenantNode.ToCreateForNewNode>(),
        };

        private List<TenantNode.ToCreateForNewNode> tenantNodesToAdd = new();

        public List<TenantNode.ToCreateForNewNode> TenantNodesToAdd {
            get => tenantNodesToAdd;
            init {
                if (value is not null) {
                    tenantNodesToAdd = value;
                }
            }
        }

        public override IEnumerable<TenantNode> TenantNodes => TenantNodesToAdd;

    }
}
