namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(ExistingOrganizationPoliticalEntityRelation))]
public partial class ExistingOrganizationPoliticalEntityRelationJsonContext : JsonSerializerContext { }

public static class OrganizationPoliticalEntityRelationExtension
{
    public static NewOrganizationPoliticalEntityRelationExistingOrganization GetNewPoliticalEntityRelationOrganization(this OrganizationListItem organizationListItem, OrganizationPoliticalEntityRelationTypeListItem relationType, int ownerId, int publishedId)
    {
        return new NewOrganizationPoliticalEntityRelationExistingOrganization {
            Organization = organizationListItem,
            OrganizationPoliticalEntityRelationType = relationType,
            Title = "",
            DateFrom = null,
            DateTo = null,
            Description = "",
            Files = new List<File>(),
            HasBeenDeleted = false,
            NodeTypeName = "party political entity relation",
            OwnerId = ownerId,
            PublisherId = publishedId,
            PoliticalEntity = null,
            ProofDocument = null,
            Tags = new List<Tags>(),
            TenantNodesToAdd = new List<TenantNode.NewTenantNodeForNewNode>(),
            Tenants = new List<Tenant>(),
        };
    }
    public static NewOrganizationPoliticalEntityRelationNewOrganization GetNewPoliticalEntityRelationOrganization(this OrganizationName organizationName, OrganizationPoliticalEntityRelationTypeListItem relationType, int ownerId, int publishedId)
    {
        return new NewOrganizationPoliticalEntityRelationNewOrganization {
            Organization = organizationName,
            OrganizationPoliticalEntityRelationType = relationType,
            Title = "",
            DateFrom = null,
            DateTo = null,
            Description = "",
            Files = new List<File>(),
            HasBeenDeleted = false,
            NodeTypeName = "party political entity relation",
            OwnerId = ownerId,
            PublisherId = publishedId,
            PoliticalEntity = null,
            ProofDocument = null,
            Tags = new List<Tags>(),
            TenantNodesToAdd = new List<TenantNode.NewTenantNodeForNewNode>(),
            Tenants = new List<Tenant>(),
        };
    }
}
    public abstract record OrganizationPoliticalEntityRelation : RelationBase
{
    private OrganizationPoliticalEntityRelation() { }

    [RequireNamedArgs]
    public abstract T Match<T>(
        Func<NewOrganizationPoliticalEntityRelationNewOrganization, T> newOrganizationPoliticalEntityRelationNewOrganization,
        Func<NewOrganizationPoliticalEntityRelationExistingOrganization, T> newOrganizationPoliticalEntityRelationExistingOrganization,
        Func<ExistingOrganizationPoliticalEntityRelation, T> existingOrganizationPoliticalEntityRelation,
        Func<CompletedNewOrganizationPoliticalEntityRelationNewOrganization, T> completedNewOrganizationPoliticalEntityRelationNewOrganization,
        Func<CompletedNewOrganizationPoliticalEntityRelationExistingOrganization, T> completedNewOrganizationPoliticalEntityRelationExistingOrganization
     );

    [RequireNamedArgs]
    public abstract T Match<T>(
        Func<IncompleteOrganizationPoliticalEntityRelation, T> incompleteOrganizationPoliticalEntityRelation,
        Func<CompletedOrganizationPoliticalEntityRelation, T> completedOrganizationPoliticalEntityRelation
     );
    public required OrganizationPoliticalEntityRelationTypeListItem OrganizationPoliticalEntityRelationType { get; set; }

    public abstract OrganizationItem? OrganizationItem { get; }
    public abstract PoliticalEntityListItem? PoliticalEntityItem { get; }

    public abstract record IncompleteOrganizationPoliticalEntityRelation : OrganizationPoliticalEntityRelation
    {
        private IncompleteOrganizationPoliticalEntityRelation() { }
        public override T Match<T>(
            Func<IncompleteOrganizationPoliticalEntityRelation, T> incompleteOrganizationPoliticalEntityRelation,
            Func<CompletedOrganizationPoliticalEntityRelation, T> completedOrganizationPoliticalEntityRelation
         )
        {
            return incompleteOrganizationPoliticalEntityRelation(this);
        }

        public abstract CompletedOrganizationPoliticalEntityRelation GetCompletedRelation(PoliticalEntityListItem politicalEntity);
        public sealed record NewOrganizationPoliticalEntityRelationNewOrganization : IncompleteOrganizationPoliticalEntityRelation, NewNode
        {
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


            public override T Match<T>(
                Func<NewOrganizationPoliticalEntityRelationNewOrganization, T> newOrganizationPoliticalEntityRelationNewOrganization,
                Func<NewOrganizationPoliticalEntityRelationExistingOrganization, T> newOrganizationPoliticalEntityRelationExistingOrganization,
                Func<ExistingOrganizationPoliticalEntityRelation, T> existingOrganizationPoliticalEntityRelation,
                Func<CompletedNewOrganizationPoliticalEntityRelationNewOrganization, T> completedNewOrganizationPoliticalEntityRelationNewOrganization,
                Func<CompletedNewOrganizationPoliticalEntityRelationExistingOrganization, T> completedNewOrganizationPoliticalEntityRelationExistingOrganization
             )
            {
                return newOrganizationPoliticalEntityRelationNewOrganization(this);
            }

            public required OrganizationName Organization { get; set; }
            public required PoliticalEntityListItem? PoliticalEntity { get; set; }
            public override OrganizationItem? OrganizationItem => Organization;
            public override PoliticalEntityListItem? PoliticalEntityItem => PoliticalEntity;
            public override CompletedOrganizationPoliticalEntityRelation GetCompletedRelation(PoliticalEntityListItem politicalEntity)
            {
                return new CompletedNewOrganizationPoliticalEntityRelationNewOrganization {
                    Organization = Organization,
                    PoliticalEntity = politicalEntity,
                    DateFrom = DateFrom,
                    DateTo = DateTo,
                    OrganizationPoliticalEntityRelationType = OrganizationPoliticalEntityRelationType,
                    Description = Description,
                    Files = Files,
                    HasBeenDeleted = HasBeenDeleted,
                    NodeTypeName = NodeTypeName,
                    OwnerId = OwnerId,
                    PublisherId = PublisherId,
                    ProofDocument = ProofDocument,
                    Tags = Tags,
                    TenantNodesToAdd = TenantNodesToAdd,
                    Tenants = Tenants,
                    Title = Title
                };
            }
        }
        public sealed record NewOrganizationPoliticalEntityRelationExistingOrganization : IncompleteOrganizationPoliticalEntityRelation, NewNode
        {
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


            public override T Match<T>(
                Func<NewOrganizationPoliticalEntityRelationNewOrganization, T> newOrganizationPoliticalEntityRelationNewOrganization,
                Func<NewOrganizationPoliticalEntityRelationExistingOrganization, T> newOrganizationPoliticalEntityRelationExistingOrganization,
                Func<ExistingOrganizationPoliticalEntityRelation, T> existingOrganizationPoliticalEntityRelation,
                Func<CompletedNewOrganizationPoliticalEntityRelationNewOrganization, T> completedNewOrganizationPoliticalEntityRelationNewOrganization,
                Func<CompletedNewOrganizationPoliticalEntityRelationExistingOrganization, T> completedNewOrganizationPoliticalEntityRelationExistingOrganization
             )
            {
                return newOrganizationPoliticalEntityRelationExistingOrganization(this);
            }

            public required OrganizationListItem Organization { get; set; }
            public required PoliticalEntityListItem? PoliticalEntity { get; set; }
            public override OrganizationItem? OrganizationItem => Organization;
            public override PoliticalEntityListItem? PoliticalEntityItem => PoliticalEntity;
            public override CompletedOrganizationPoliticalEntityRelation GetCompletedRelation(PoliticalEntityListItem politicalEntity)
            {
                return new CompletedNewOrganizationPoliticalEntityRelationExistingOrganization {
                    Organization = Organization,
                    PoliticalEntity = politicalEntity,
                    DateFrom = DateFrom,
                    DateTo = DateTo,
                    OrganizationPoliticalEntityRelationType = OrganizationPoliticalEntityRelationType,
                    Description = Description,
                    Files = Files,
                    HasBeenDeleted = HasBeenDeleted,
                    NodeTypeName = NodeTypeName,
                    OwnerId = OwnerId,
                    PublisherId = PublisherId,
                    ProofDocument = ProofDocument,
                    Tags = Tags,
                    TenantNodesToAdd = TenantNodesToAdd,
                    Tenants = Tenants,
                    Title = Title
                };
            }
        }
    }
    public abstract record CompletedOrganizationPoliticalEntityRelation : OrganizationPoliticalEntityRelation
    {
        private CompletedOrganizationPoliticalEntityRelation() { }
        public override T Match<T>(
            Func<IncompleteOrganizationPoliticalEntityRelation, T> incompleteOrganizationPoliticalEntityRelation,
            Func<CompletedOrganizationPoliticalEntityRelation, T> completedOrganizationPoliticalEntityRelation
         )
        {
            return completedOrganizationPoliticalEntityRelation(this);
        }

        public abstract string OrganizationName { get; }

        public abstract string PoliticalEntityName { get; }
        public abstract record ResolvedOrganizationPoliticalEntityRelation : CompletedOrganizationPoliticalEntityRelation
        {
            private ResolvedOrganizationPoliticalEntityRelation() { }
            public sealed record ExistingOrganizationPoliticalEntityRelation : ResolvedOrganizationPoliticalEntityRelation, ExistingNode
            {
                public override T Match<T>(
                    Func<NewOrganizationPoliticalEntityRelationNewOrganization, T> newOrganizationPoliticalEntityRelationNewOrganization,
                    Func<NewOrganizationPoliticalEntityRelationExistingOrganization, T> newOrganizationPoliticalEntityRelationExistingOrganization,
                    Func<ExistingOrganizationPoliticalEntityRelation, T> existingOrganizationPoliticalEntityRelation,
                    Func<CompletedNewOrganizationPoliticalEntityRelationNewOrganization, T> completedNewOrganizationPoliticalEntityRelationNewOrganization,
                    Func<CompletedNewOrganizationPoliticalEntityRelationExistingOrganization, T> completedNewOrganizationPoliticalEntityRelationExistingOrganization
                 )
                {
                    return existingOrganizationPoliticalEntityRelation(this);
                }
                public int NodeId { get; init; }

                public int UrlId { get; set; }
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
                public required OrganizationListItem Organization { get; set; }
                public required PoliticalEntityListItem PoliticalEntity { get; set; }
                public override string OrganizationName => Organization.Name;
                public override string PoliticalEntityName => PoliticalEntity.Name;
                public override OrganizationItem? OrganizationItem => Organization;
                public override PoliticalEntityListItem? PoliticalEntityItem => PoliticalEntity;
            }
            public sealed record CompletedNewOrganizationPoliticalEntityRelationExistingOrganization : ResolvedOrganizationPoliticalEntityRelation, NewNode
            {
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


                public override T Match<T>(
                    Func<NewOrganizationPoliticalEntityRelationNewOrganization, T> newOrganizationPoliticalEntityRelationNewOrganization,
                    Func<NewOrganizationPoliticalEntityRelationExistingOrganization, T> newOrganizationPoliticalEntityRelationExistingOrganization,
                    Func<ExistingOrganizationPoliticalEntityRelation, T> existingOrganizationPoliticalEntityRelation,
                    Func<CompletedNewOrganizationPoliticalEntityRelationNewOrganization, T> completedNewOrganizationPoliticalEntityRelationNewOrganization,
                    Func<CompletedNewOrganizationPoliticalEntityRelationExistingOrganization, T> completedNewOrganizationPoliticalEntityRelationExistingOrganization
                 )
                {
                    return completedNewOrganizationPoliticalEntityRelationExistingOrganization(this);
                }

                public required OrganizationListItem Organization { get; set; }
                public required PoliticalEntityListItem PoliticalEntity { get; set; }

                public override string OrganizationName => Organization.Name;
                public override string PoliticalEntityName => PoliticalEntity.Name;
                public override OrganizationItem? OrganizationItem => Organization;
                public override PoliticalEntityListItem? PoliticalEntityItem => PoliticalEntity;
            }
        }
        public sealed record CompletedNewOrganizationPoliticalEntityRelationNewOrganization : CompletedOrganizationPoliticalEntityRelation, NewNode
        {
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


            public override T Match<T>(
                Func<NewOrganizationPoliticalEntityRelationNewOrganization, T> newOrganizationPoliticalEntityRelationNewOrganization,
                Func<NewOrganizationPoliticalEntityRelationExistingOrganization, T> newOrganizationPoliticalEntityRelationExistingOrganization,
                Func<ExistingOrganizationPoliticalEntityRelation, T> existingOrganizationPoliticalEntityRelation,
                Func<CompletedNewOrganizationPoliticalEntityRelationNewOrganization, T> completedNewOrganizationPoliticalEntityRelationNewOrganization,
                Func<CompletedNewOrganizationPoliticalEntityRelationExistingOrganization, T> completedNewOrganizationPoliticalEntityRelationExistingOrganization
             )
            {
                return completedNewOrganizationPoliticalEntityRelationNewOrganization(this);
            }

            public required OrganizationName Organization { get; set; }
            public required PoliticalEntityListItem PoliticalEntity { get; set; }
            public override string OrganizationName => Organization.Name;
            public override string PoliticalEntityName => PoliticalEntity.Name;
            public override OrganizationItem? OrganizationItem => Organization;
            public override PoliticalEntityListItem? PoliticalEntityItem => PoliticalEntity;

        }
    }


}

