namespace PoundPupLegacy.EditModel;

//[JsonSerializable(typeof(OrganizationPoliticalEntityRelation.Complete.Resolved.ToUpdate), TypeInfoPropertyName = "OrganizationPoliticalEntityRelationCompleteResolvedToUpdate")]
//[JsonSerializable(typeof(TenantNodeDetails.ForCreate), TypeInfoPropertyName = "TenantNodeDetailsForCreate")]
//[JsonSerializable(typeof(TenantNodeDetails.ForUpdate), TypeInfoPropertyName = "TenantNodeDetailsForUpdate")]
//[JsonSerializable(typeof(NodeDetails.ForUpdate), TypeInfoPropertyName = "NodeDetailsForUpdate")]
//[JsonSerializable(typeof(NodeDetails.ForCreate), TypeInfoPropertyName = "NodeDetailsForCreate")]
//public partial class ExistingOrganizationPoliticalEntityRelationJsonContext : JsonSerializerContext { }

public static class OrganizationPoliticalEntityRelationExtension
{
    public static OrganizationPoliticalEntityRelation.Incomplete.ToCreateForExistingOrganization GetNewPoliticalEntityRelationOrganization(this OrganizationListItem organizationListItem, OrganizationPoliticalEntityRelationTypeListItem relationType, int ownerId, int publishedId, List<TenantDetails> tenants)
    {
        return new OrganizationPoliticalEntityRelation.Incomplete.ToCreateForExistingOrganization {
            Organization = organizationListItem,
            PartyPoliticalEntityRelationType = relationType,
            PoliticalEntity = null,
            NodeDetailsForCreate = NodeDetails.EmptyInstance(49, "organization political entity relation", ownerId, publishedId, tenants),
            RelationDetails = RelationDetails.EmptyInstance,
        };
    }
    public static OrganizationPoliticalEntityRelation.Incomplete.ToCreateForNewOrganization GetNewPoliticalEntityRelationOrganization(this OrganizationName organizationName, OrganizationPoliticalEntityRelationTypeListItem relationType, int ownerId, int publishedId, List<TenantDetails> tenants)
    {
        return new OrganizationPoliticalEntityRelation.Incomplete.ToCreateForNewOrganization {
            Organization = organizationName,
            PartyPoliticalEntityRelationType = relationType,
            PoliticalEntity = null,
            NodeDetailsForCreate = NodeDetails.EmptyInstance(49, "organization political entity relation", ownerId, publishedId, tenants),
            RelationDetails = RelationDetails.EmptyInstance,
        };
    }
}
public abstract record OrganizationPoliticalEntityRelation : Relation
{
    private OrganizationPoliticalEntityRelation() { }

    [RequireNamedArgs]
    public abstract T Match<T>(
        Func<Incomplete.ToCreateForNewOrganization, T> newOrganizationPoliticalEntityRelationNewOrganization,
        Func<Incomplete.ToCreateForExistingOrganization, T> newOrganizationPoliticalEntityRelationExistingOrganization,
        Func<Complete.Resolved.ToUpdate, T> existingOrganizationPoliticalEntityRelation,
        Func<Complete.ToCreateForNewOrganization, T> completedNewOrganizationPoliticalEntityRelationNewOrganization,
        Func<Complete.Resolved.ToCreateForExistingOrganization, T> completedNewOrganizationPoliticalEntityRelationExistingOrganization
     );

    [RequireNamedArgs]
    public abstract T Match<T>(
        Func<Incomplete, T> incompleteOrganizationPoliticalEntityRelation,
        Func<Complete, T> completedOrganizationPoliticalEntityRelation
     );
    public required RelationDetails RelationDetails { get; init; }
    public abstract NodeDetails NodeDetails { get; }
    public required OrganizationPoliticalEntityRelationTypeListItem PartyPoliticalEntityRelationType { get; set; }

    public abstract OrganizationItem? OrganizationItem { get; }
    public abstract PoliticalEntityListItem? PoliticalEntityItem { get; }

    public abstract record Incomplete : OrganizationPoliticalEntityRelation
    {
        private Incomplete() { }
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public required NodeDetails.ForCreate NodeDetailsForCreate { get; init; }
        public override T Match<T>(
            Func<Incomplete, T> incompleteOrganizationPoliticalEntityRelation,
            Func<Complete, T> completedOrganizationPoliticalEntityRelation
         )
        {
            return incompleteOrganizationPoliticalEntityRelation(this);
        }

        public abstract Complete GetCompletedRelation(PoliticalEntityListItem politicalEntity);
        public sealed record ToCreateForNewOrganization : Incomplete, NewNode
        {
            public override T Match<T>(
                Func<ToCreateForNewOrganization, T> newOrganizationPoliticalEntityRelationNewOrganization,
                Func<ToCreateForExistingOrganization, T> newOrganizationPoliticalEntityRelationExistingOrganization,
                Func<Complete.Resolved.ToUpdate, T> existingOrganizationPoliticalEntityRelation,
                Func<Complete.ToCreateForNewOrganization, T> completedNewOrganizationPoliticalEntityRelationNewOrganization,
                Func<Complete.Resolved.ToCreateForExistingOrganization, T> completedNewOrganizationPoliticalEntityRelationExistingOrganization
             )
            {
                return newOrganizationPoliticalEntityRelationNewOrganization(this);
            }

            public required OrganizationName Organization { get; set; }
            public required PoliticalEntityListItem? PoliticalEntity { get; set; }
            public override OrganizationItem? OrganizationItem => Organization;
            public override PoliticalEntityListItem? PoliticalEntityItem => PoliticalEntity;
            public override Complete GetCompletedRelation(PoliticalEntityListItem politicalEntity)
            {
                return new Complete.ToCreateForNewOrganization {
                    Organization = Organization,
                    PoliticalEntity = politicalEntity,
                    NodeDetailsForCreate = NodeDetailsForCreate,
                    PartyPoliticalEntityRelationType = PartyPoliticalEntityRelationType,
                    RelationDetails = RelationDetails
                };
            }
        }
        public sealed record ToCreateForExistingOrganization : Incomplete, NewNode
        {
            public override T Match<T>(
                Func<ToCreateForNewOrganization, T> newOrganizationPoliticalEntityRelationNewOrganization,
                Func<ToCreateForExistingOrganization, T> newOrganizationPoliticalEntityRelationExistingOrganization,
                Func<Complete.Resolved.ToUpdate, T> existingOrganizationPoliticalEntityRelation,
                Func<Complete.ToCreateForNewOrganization, T> completedNewOrganizationPoliticalEntityRelationNewOrganization,
                Func<Complete.Resolved.ToCreateForExistingOrganization, T> completedNewOrganizationPoliticalEntityRelationExistingOrganization
             )
            {
                return newOrganizationPoliticalEntityRelationExistingOrganization(this);
            }

            public required OrganizationListItem Organization { get; set; }
            public required PoliticalEntityListItem? PoliticalEntity { get; set; }
            public override OrganizationItem? OrganizationItem => Organization;
            public override PoliticalEntityListItem? PoliticalEntityItem => PoliticalEntity;
            public override Complete GetCompletedRelation(PoliticalEntityListItem politicalEntity)
            {
                return new Complete.Resolved.ToCreateForExistingOrganization {
                    Organization = Organization,
                    PoliticalEntity = politicalEntity,
                    NodeDetailsForCreate = NodeDetailsForCreate,
                    PartyPoliticalEntityRelationType = PartyPoliticalEntityRelationType,
                    RelationDetails = RelationDetails
                };
            }
        }
    }
    public abstract record Complete : OrganizationPoliticalEntityRelation
    {
        private Complete() { }
        public override T Match<T>(
            Func<Incomplete, T> incompleteOrganizationPoliticalEntityRelation,
            Func<Complete, T> completedOrganizationPoliticalEntityRelation
         )
        {
            return completedOrganizationPoliticalEntityRelation(this);
        }

        public abstract string OrganizationName { get; }
         
        public abstract string PoliticalEntityName { get; }
        public abstract record Resolved : Complete
        {
            private Resolved() { }
            public sealed record ToUpdate : Resolved, ExistingNode
            {
                public override NodeDetails NodeDetails => NodeDetailsForUpdate;
                public required NodeDetails.ForUpdate NodeDetailsForUpdate { get; init; }
                public required NodeIdentification NodeIdentification { get; init; }

                public override T Match<T>(
                    Func<Incomplete.ToCreateForNewOrganization, T> newOrganizationPoliticalEntityRelationNewOrganization,
                    Func<Incomplete.ToCreateForExistingOrganization, T> newOrganizationPoliticalEntityRelationExistingOrganization,
                    Func<ToUpdate, T> existingOrganizationPoliticalEntityRelation,
                    Func<ToCreateForNewOrganization, T> completedNewOrganizationPoliticalEntityRelationNewOrganization,
                    Func<ToCreateForExistingOrganization, T> completedNewOrganizationPoliticalEntityRelationExistingOrganization
                 )
                {
                    return existingOrganizationPoliticalEntityRelation(this);
                }
                public required OrganizationListItem Party { get; set; }
                public required PoliticalEntityListItem PoliticalEntity { get; set; }
                public override string OrganizationName => Party.Name; 
                public override string PoliticalEntityName => PoliticalEntity.Name;
                public override OrganizationItem? OrganizationItem => Party;
                public override PoliticalEntityListItem? PoliticalEntityItem => PoliticalEntity;
            }
            public sealed record ToCreateForExistingOrganization : Resolved, NewNode
            {
                public override NodeDetails NodeDetails => NodeDetailsForCreate;
                public required NodeDetails.ForCreate NodeDetailsForCreate { get; init; }

                public override T Match<T>(
                    Func<Incomplete.ToCreateForNewOrganization, T> newOrganizationPoliticalEntityRelationNewOrganization,
                    Func<Incomplete.ToCreateForExistingOrganization, T> newOrganizationPoliticalEntityRelationExistingOrganization,
                    Func<ToUpdate, T> existingOrganizationPoliticalEntityRelation,
                    Func<ToCreateForNewOrganization, T> completedNewOrganizationPoliticalEntityRelationNewOrganization,
                    Func<ToCreateForExistingOrganization, T> completedNewOrganizationPoliticalEntityRelationExistingOrganization
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
        public sealed record ToCreateForNewOrganization : Complete, NewNode
        {
            public override NodeDetails NodeDetails => NodeDetailsForCreate;
            public required NodeDetails.ForCreate NodeDetailsForCreate { get; init; }
            public override T Match<T>(
                Func<Incomplete.ToCreateForNewOrganization, T> newOrganizationPoliticalEntityRelationNewOrganization,
                Func<Incomplete.ToCreateForExistingOrganization, T> newOrganizationPoliticalEntityRelationExistingOrganization,
                Func<Resolved.ToUpdate, T> existingOrganizationPoliticalEntityRelation,
                Func<ToCreateForNewOrganization, T> completedNewOrganizationPoliticalEntityRelationNewOrganization,
                Func<Resolved.ToCreateForExistingOrganization, T> completedNewOrganizationPoliticalEntityRelationExistingOrganization
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

