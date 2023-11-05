namespace PoundPupLegacy.EditModel;

public static class PersonPoliticalEntityRelationExtensions
{
    public static PersonPoliticalEntityRelation GetPersonPoliticalEntityRelation(this PersonListItem personListItem, PersonPoliticalEntityRelationTypeListItem relationType, int ownerId, int publisherId, List<Tenant> tenants)
    {
        return new PersonPoliticalEntityRelation.Incomplete.ToCreateForExistingPerson {
            Person = personListItem,
            PartyPoliticalEntityRelationType = relationType,        
            RelationDetails = RelationDetails.EmptyInstance,
            NodeDetailsForCreate = NodeDetails.EmptyInstance(49, "person political entity relation", "person_political_entity_relation", ownerId, publisherId, tenants),
            PoliticalEntity = null
        };
    }
    public static PersonPoliticalEntityRelation GetPersonPoliticalEntityRelation(this PersonName personName, PersonPoliticalEntityRelationTypeListItem relationType, int ownerId, int publisherId, List<Tenant> tenants)
    {
        return new PersonPoliticalEntityRelation.Incomplete.ToCreateForNewPerson {
            Person = personName,
            PartyPoliticalEntityRelationType = relationType,        
            RelationDetails = RelationDetails.EmptyInstance,
            NodeDetailsForCreate = NodeDetails.EmptyInstance(49, "person political entity relation", "person_political_entity_relation", ownerId, publisherId, tenants),
            PoliticalEntity = null
        };
    }
}

public abstract record PersonPoliticalEntityRelation : Relation
{
    private PersonPoliticalEntityRelation() { }

    public required RelationDetails RelationDetails { get; init; }
    public abstract NodeDetails NodeDetails { get; }
    public required PersonPoliticalEntityRelationTypeListItem PartyPoliticalEntityRelationType { get; set; }

    [RequireNamedArgs]
    public abstract T Match<T>(
        Func<Incomplete, T> incompletePersonPoliticalEntityRelation,
        Func<Complete, T> completedPersonPoliticalEntityRelation
     );

    public abstract PersonItem? PersonItem { get; }
    public abstract PoliticalEntityListItem? PoliticalEntityItem { get; set; }
    public abstract record Incomplete : PersonPoliticalEntityRelation
    {
        private Incomplete() { }
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public required NodeDetails.ForCreate NodeDetailsForCreate { get; init; }
        public abstract Complete GetCompletedRelation(PoliticalEntityListItem politicalEntity);
        public override T Match<T>(
            Func<Incomplete, T> incomplete,
            Func<Complete, T> complete
         )
        {
            return incomplete(this); 
        }
        public sealed record ToCreateForNewPerson : Incomplete, NewNode
        {
            public required PersonName Person { get; set; }
            public required PoliticalEntityListItem? PoliticalEntity { get; set; }
            public override PersonItem? PersonItem => Person;
            public override PoliticalEntityListItem? PoliticalEntityItem {
                get => PoliticalEntity;
                set {
                    if (value != null) {
                        PoliticalEntity = value;
                    }
                }
            }
            public override Complete GetCompletedRelation(PoliticalEntityListItem politicalEntity)
            {
                return new Complete.ToCreateForNewPerson {
                    Person = Person,
                    PoliticalEntity = politicalEntity,
                    NodeDetailsForCreate = NodeDetailsForCreate,
                    PartyPoliticalEntityRelationType = PartyPoliticalEntityRelationType,
                    RelationDetails = RelationDetails
                };
            }
        }
        public sealed record ToCreateForExistingPerson : Incomplete, NewNode
        {
            public required PersonListItem Person { get; set; }
            public required PoliticalEntityListItem? PoliticalEntity { get; set; }
            public override PersonItem? PersonItem => Person;
            public override PoliticalEntityListItem? PoliticalEntityItem {
                get => PoliticalEntity;
                set {
                    if (value != null) {
                        PoliticalEntity = value;
                    }
                }
            }
            public override Complete GetCompletedRelation(PoliticalEntityListItem politicalEntity)
            {
                return new Complete.Resolved.ToCreateForExistingPerson {
                    Person = Person,
                    PoliticalEntity = politicalEntity,
                    NodeDetailsForCreate = NodeDetailsForCreate,
                    PartyPoliticalEntityRelationType = PartyPoliticalEntityRelationType,
                    RelationDetails = RelationDetails
                };
            }
        }
    }
    public abstract record Complete : PersonPoliticalEntityRelation
    {
        private Complete() { }
        public abstract string PersonName { get; }

        public abstract string PoliticalEntityName { get; }
        public override T Match<T>(
            Func<Incomplete, T> incompletePersonPoliticalEntityRelation,
            Func<Complete, T> completedPersonPoliticalEntityRelation
         )
        {
            return completedPersonPoliticalEntityRelation(this);
        }

        public abstract record Resolved : Complete
        {
            private Resolved() { }
            public sealed record ToUpdate : Resolved, ExistingNode
            {
                public override NodeDetails NodeDetails => NodeDetailsForUpdate;
                public required NodeDetails.ForUpdate NodeDetailsForUpdate { get; init; }
                public required NodeIdentification NodeIdentification { get; init; }
                public int NodeId { get; init; }
                public required PersonListItem Party { get; set; }
                public required PoliticalEntityListItem PoliticalEntity { get; set; }
                public override string PersonName => Party.Name;
                public override string PoliticalEntityName => PoliticalEntity.Name;
                public override PersonItem? PersonItem => Party;
                public override PoliticalEntityListItem? PoliticalEntityItem {
                    get => PoliticalEntity;
                    set {
                        if (value != null) {
                            PoliticalEntity = value;
                        }
                    }
                }
            }
            public sealed record ToCreateForExistingPerson : Resolved, NewNode
            {
                public override NodeDetails NodeDetails => NodeDetailsForCreate;
                public required NodeDetails.ForCreate NodeDetailsForCreate { get; init; }
                public required PersonListItem Person { get; set; }
                public required PoliticalEntityListItem PoliticalEntity { get; set; }
                public override string PersonName => Person.Name;
                public override string PoliticalEntityName => PoliticalEntity.Name;
                public override PersonItem? PersonItem => Person;
                public override PoliticalEntityListItem? PoliticalEntityItem {
                    get => PoliticalEntity;
                    set {
                        if (value != null) {
                            PoliticalEntity = value;
                        }
                    }
                }
            }
        }
        public sealed record ToCreateForNewPerson : Complete, NewNode
        {
            public override NodeDetails NodeDetails => NodeDetailsForCreate;
            public required NodeDetails.ForCreate NodeDetailsForCreate { get; init; }
            public required PersonName Person { get; set; }
            public required PoliticalEntityListItem PoliticalEntity { get; set; }
            public override string PersonName => Person.Name;
            public override string PoliticalEntityName => PoliticalEntity.Name;
            public override PersonItem? PersonItem => Person;
            public override PoliticalEntityListItem? PoliticalEntityItem {
                get => PoliticalEntity;
                set {
                    if (value != null) {
                        PoliticalEntity = value;
                    }
                }
            }
        }
    }
}

