namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(TenantNodeDetails.ExistingTenantNodeDetails))]
public partial class ExistingTenantNodeDetailsJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(TenantNodeDetails.ExistingTenantNodeDetails))]
public partial class NewTenantNodeDetailsJsonContext : JsonSerializerContext { }

public abstract record TenantNodeDetails
{
    public abstract IEnumerable<TenantNode> TenantNodes { get; }

    public sealed record ExistingTenantNodeDetails: TenantNodeDetails
    {
        private List<TenantNode.NewTenantNodeForExistingNode> tenantNodesToAdd = new();

        public List<TenantNode.NewTenantNodeForExistingNode> TenantNodesToAdd {
            get => tenantNodesToAdd;
            init {
                if (value is not null) {
                    tenantNodesToAdd = value;
                }
            }
        }
        private List<TenantNode.ExistingTenantNode> tenantNodesToUpdate = new();

        public List<TenantNode.ExistingTenantNode> TenantNodesToUpdate {
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
    public sealed record NewTenantNodeDetails: TenantNodeDetails
    {
        public static NewTenantNodeDetails EmptyInstance => new NewTenantNodeDetails {
            TenantNodesToAdd = new List<TenantNode.NewTenantNodeForNewNode>(),
        };

        private List<TenantNode.NewTenantNodeForNewNode> tenantNodesToAdd = new();

        public List<TenantNode.NewTenantNodeForNewNode> TenantNodesToAdd {
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
