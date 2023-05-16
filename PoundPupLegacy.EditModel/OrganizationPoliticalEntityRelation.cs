namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(ExistingOrganizationPoliticalEntityRelation))]
public partial class ExistingOrganizationPoliticalEntityRelationJsonContext : JsonSerializerContext { }

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
                    TenantNodes = TenantNodes,
                    Tenants = Tenants,
                    Title = Title
                };
            }
        }
        public sealed record NewOrganizationPoliticalEntityRelationExistingOrganization : IncompleteOrganizationPoliticalEntityRelation, NewNode
        {
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
                    TenantNodes = TenantNodes,
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
                public required OrganizationListItem Organization { get; set; }
                public required PoliticalEntityListItem PoliticalEntity { get; set; }
                public override string OrganizationName => Organization.Name;
                public override string PoliticalEntityName => PoliticalEntity.Name;
                public override OrganizationItem? OrganizationItem => Organization;
                public override PoliticalEntityListItem? PoliticalEntityItem => PoliticalEntity;
            }
            public sealed record CompletedNewOrganizationPoliticalEntityRelationExistingOrganization : ResolvedOrganizationPoliticalEntityRelation, NewNode
            {
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

