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
            PoliticalEntity = null,
            NodeDetailsForCreate = NodeDetails.EmptyInstance(49, "organization political entity relation", ownerId, publishedId),
            RelationDetails = RelationDetails.EmptyInstance,
        };
    }
    public static NewOrganizationPoliticalEntityRelationNewOrganization GetNewPoliticalEntityRelationOrganization(this OrganizationName organizationName, OrganizationPoliticalEntityRelationTypeListItem relationType, int ownerId, int publishedId)
    {
        return new NewOrganizationPoliticalEntityRelationNewOrganization {
            Organization = organizationName,
            OrganizationPoliticalEntityRelationType = relationType,
            PoliticalEntity = null,
            NodeDetailsForCreate = NodeDetails.EmptyInstance(49, "organization political entity relation", ownerId, publishedId),
            RelationDetails = RelationDetails.EmptyInstance,
        };
    }
}
public abstract record OrganizationPoliticalEntityRelation : Relation
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
    public required RelationDetails RelationDetails { get; init; }
    public abstract NodeDetails NodeDetails { get; }
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
            public override NodeDetails NodeDetails => NodeDetailsForCreate;
            public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
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
                    NodeDetailsForCreate = NodeDetailsForCreate,
                    OrganizationPoliticalEntityRelationType = OrganizationPoliticalEntityRelationType,
                    RelationDetails = RelationDetails
                };
            }
        }
        public sealed record NewOrganizationPoliticalEntityRelationExistingOrganization : IncompleteOrganizationPoliticalEntityRelation, NewNode
        {
            public override NodeDetails NodeDetails => NodeDetailsForCreate;
            public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
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
                    NodeDetailsForCreate = NodeDetailsForCreate,
                    OrganizationPoliticalEntityRelationType = OrganizationPoliticalEntityRelationType,
                    RelationDetails = RelationDetails
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
                public override NodeDetails NodeDetails => NodeDetailsForUpdate;
                public required NodeDetails.NodeDetailsForUpdate NodeDetailsForUpdate { get; init; }
                public required NodeIdentification NodeIdentification { get; init; }

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
                public required OrganizationListItem Organization { get; set; }
                public required PoliticalEntityListItem PoliticalEntity { get; set; }
                public override string OrganizationName => Organization.Name;
                public override string PoliticalEntityName => PoliticalEntity.Name;
                public override OrganizationItem? OrganizationItem => Organization;
                public override PoliticalEntityListItem? PoliticalEntityItem => PoliticalEntity;
            }
            public sealed record CompletedNewOrganizationPoliticalEntityRelationExistingOrganization : ResolvedOrganizationPoliticalEntityRelation, NewNode
            {
                public override NodeDetails NodeDetails => NodeDetailsForCreate;
                public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }

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
            public override NodeDetails NodeDetails => NodeDetailsForCreate;
            public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
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

