namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(ExistingPersonPoliticalEntityRelation))]
public partial class ExistingPersonPoliticalEntityRelationJsonContext : JsonSerializerContext { }

public static class PersonPoliticalEntityRelationExtensions
{
    public static PersonPoliticalEntityRelation GetPersonPoliticalEntityRelation(this PersonListItem personListItem, PersonPoliticalEntityRelationTypeListItem relationType, int ownerId, int publishedId)
    {
        return new NewPersonPoliticalEntityRelationExistingPerson {
            Person = personListItem,
            PersonPoliticalEntityRelationType = relationType,
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
    public static PersonPoliticalEntityRelation GetPersonPoliticalEntityRelation(this PersonName personName, PersonPoliticalEntityRelationTypeListItem relationType, int ownerId, int publishedId)
    {
        return new NewPersonPoliticalEntityRelationNewPerson {
            Person = personName,
            PersonPoliticalEntityRelationType = relationType,
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


public abstract record PersonPoliticalEntityRelation : RelationBase
{
    private PersonPoliticalEntityRelation() { }

    [RequireNamedArgs]
    public abstract T Match<T>(
        Func<NewPersonPoliticalEntityRelationNewPerson, T> newPersonPoliticalEntityRelationNewPerson,
        Func<NewPersonPoliticalEntityRelationExistingPerson, T> newPersonPoliticalEntityRelationExistingPerson,
        Func<ExistingPersonPoliticalEntityRelation, T> existingPersonPoliticalEntityRelation,
        Func<CompletedNewPersonPoliticalEntityRelationNewPerson, T> completedNewPersonPoliticalEntityRelationNewPerson,
        Func<CompletedNewPersonPoliticalEntityRelationExistingPerson, T> completedNewPersonPoliticalEntityRelationExistingPerson
     );
    [RequireNamedArgs]
    public abstract T Match<T>(
        Func<IncompletePersonPoliticalEntityRelation, T> incompletePersonPoliticalEntityRelation,
        Func<CompletedPersonPoliticalEntityRelation, T> completedPersonPoliticalEntityRelation
     );

    public required PersonPoliticalEntityRelationTypeListItem PersonPoliticalEntityRelationType { get; set; }

    public abstract PersonItem? PersonItem { get; }
    public abstract PoliticalEntityListItem? PoliticalEntityItem { get; }

    public abstract record IncompletePersonPoliticalEntityRelation : PersonPoliticalEntityRelation
    {
        private IncompletePersonPoliticalEntityRelation() { }
        public abstract CompletedPersonPoliticalEntityRelation GetCompletedRelation(PoliticalEntityListItem politicalEntity);
        public override T Match<T>(
            Func<IncompletePersonPoliticalEntityRelation, T> incompletePersonPoliticalEntityRelation,
            Func<CompletedPersonPoliticalEntityRelation, T> completedPersonPoliticalEntityRelation
         )
        {
            return incompletePersonPoliticalEntityRelation(this); 
        }
        public sealed record NewPersonPoliticalEntityRelationNewPerson : IncompletePersonPoliticalEntityRelation, NewNode
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
                Func<NewPersonPoliticalEntityRelationNewPerson, T> newPersonPoliticalEntityRelationNewPerson,
                Func<NewPersonPoliticalEntityRelationExistingPerson, T> newPersonPoliticalEntityRelationExistingPerson,
                Func<ExistingPersonPoliticalEntityRelation, T> existingPersonPoliticalEntityRelation,
                Func<CompletedNewPersonPoliticalEntityRelationNewPerson, T> completedNewPersonPoliticalEntityRelationNewPerson,
                Func<CompletedNewPersonPoliticalEntityRelationExistingPerson, T> completedNewPersonPoliticalEntityRelationExistingPerson
             )
            {
                return newPersonPoliticalEntityRelationNewPerson(this);
            }

            public required PersonName Person { get; set; }
            public required PoliticalEntityListItem? PoliticalEntity { get; set; }
            public override PersonItem? PersonItem => Person;
            public override PoliticalEntityListItem? PoliticalEntityItem => PoliticalEntity;
            public override CompletedPersonPoliticalEntityRelation GetCompletedRelation(PoliticalEntityListItem politicalEntity)
            {
                return new CompletedNewPersonPoliticalEntityRelationNewPerson {
                    Person = Person,
                    PoliticalEntity = politicalEntity,
                    DateFrom = DateFrom,
                    DateTo = DateTo,
                    PersonPoliticalEntityRelationType = PersonPoliticalEntityRelationType,
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
        public sealed record NewPersonPoliticalEntityRelationExistingPerson : IncompletePersonPoliticalEntityRelation, NewNode
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
                Func<NewPersonPoliticalEntityRelationNewPerson, T> newPersonPoliticalEntityRelationNewPerson,
                Func<NewPersonPoliticalEntityRelationExistingPerson, T> newPersonPoliticalEntityRelationExistingPerson,
                Func<ExistingPersonPoliticalEntityRelation, T> existingPersonPoliticalEntityRelation,
                Func<CompletedNewPersonPoliticalEntityRelationNewPerson, T> completedNewPersonPoliticalEntityRelationNewPerson,
                Func<CompletedNewPersonPoliticalEntityRelationExistingPerson, T> completedNewPersonPoliticalEntityRelationExistingPerson
             )
            {
                return newPersonPoliticalEntityRelationExistingPerson(this);
            }

            public required PersonListItem Person { get; set; }
            public required PoliticalEntityListItem? PoliticalEntity { get; set; }
            public override PersonItem? PersonItem => Person;
            public override PoliticalEntityListItem? PoliticalEntityItem => PoliticalEntity;
            public override CompletedPersonPoliticalEntityRelation GetCompletedRelation(PoliticalEntityListItem politicalEntity)
            {
                return new CompletedNewPersonPoliticalEntityRelationExistingPerson {
                    Person = Person,
                    PoliticalEntity = politicalEntity,
                    DateFrom = DateFrom,
                    DateTo = DateTo,
                    PersonPoliticalEntityRelationType = PersonPoliticalEntityRelationType,
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
    public abstract record CompletedPersonPoliticalEntityRelation : PersonPoliticalEntityRelation
    {
        private CompletedPersonPoliticalEntityRelation() { }
        public abstract string PersonName { get; }

        public abstract string PoliticalEntityName { get; }
        public override T Match<T>(
            Func<IncompletePersonPoliticalEntityRelation, T> incompletePersonPoliticalEntityRelation,
            Func<CompletedPersonPoliticalEntityRelation, T> completedPersonPoliticalEntityRelation
         )
        {
            return completedPersonPoliticalEntityRelation(this);
        }

        public abstract record ResolvedPersonPoliticalEntityRelation : CompletedPersonPoliticalEntityRelation
        {
            private ResolvedPersonPoliticalEntityRelation() { }
            public sealed record ExistingPersonPoliticalEntityRelation : ResolvedPersonPoliticalEntityRelation, ExistingNode
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

                public override T Match<T>(
                    Func<NewPersonPoliticalEntityRelationNewPerson, T> newPersonPoliticalEntityRelationNewPerson,
                    Func<NewPersonPoliticalEntityRelationExistingPerson, T> newPersonPoliticalEntityRelationExistingPerson,
                    Func<ExistingPersonPoliticalEntityRelation, T> existingPersonPoliticalEntityRelation,
                    Func<CompletedNewPersonPoliticalEntityRelationNewPerson, T> completedNewPersonPoliticalEntityRelationNewPerson,
                    Func<CompletedNewPersonPoliticalEntityRelationExistingPerson, T> completedNewPersonPoliticalEntityRelationExistingPerson
                 )
                {
                    return existingPersonPoliticalEntityRelation(this);
                }
                public int NodeId { get; init; }

                public int UrlId { get; set; }
                public required PersonListItem Person { get; set; }
                public required PoliticalEntityListItem PoliticalEntity { get; set; }
                public override string PersonName => Person.Name;
                public override string PoliticalEntityName => PoliticalEntity.Name;
                public override PersonItem? PersonItem => Person;
                public override PoliticalEntityListItem? PoliticalEntityItem => PoliticalEntity;
            }
            public sealed record CompletedNewPersonPoliticalEntityRelationExistingPerson : ResolvedPersonPoliticalEntityRelation, NewNode
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
                    Func<NewPersonPoliticalEntityRelationNewPerson, T> newPersonPoliticalEntityRelationNewPerson,
                    Func<NewPersonPoliticalEntityRelationExistingPerson, T> newPersonPoliticalEntityRelationExistingPerson,
                    Func<ExistingPersonPoliticalEntityRelation, T> existingPersonPoliticalEntityRelation,
                    Func<CompletedNewPersonPoliticalEntityRelationNewPerson, T> completedNewPersonPoliticalEntityRelationNewPerson,
                    Func<CompletedNewPersonPoliticalEntityRelationExistingPerson, T> completedNewPersonPoliticalEntityRelationExistingPerson
                 )
                {
                    return completedNewPersonPoliticalEntityRelationExistingPerson(this);
                }

                public required PersonListItem Person { get; set; }
                public required PoliticalEntityListItem PoliticalEntity { get; set; }

                public override string PersonName => Person.Name;
                public override string PoliticalEntityName => PoliticalEntity.Name;
                public override PersonItem? PersonItem => Person;
                public override PoliticalEntityListItem? PoliticalEntityItem => PoliticalEntity;
            }
        }
        public sealed record CompletedNewPersonPoliticalEntityRelationNewPerson : CompletedPersonPoliticalEntityRelation, NewNode
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
                Func<NewPersonPoliticalEntityRelationNewPerson, T> newPersonPoliticalEntityRelationNewPerson,
                Func<NewPersonPoliticalEntityRelationExistingPerson, T> newPersonPoliticalEntityRelationExistingPerson,
                Func<ExistingPersonPoliticalEntityRelation, T> existingPersonPoliticalEntityRelation,
                Func<CompletedNewPersonPoliticalEntityRelationNewPerson, T> completedNewPersonPoliticalEntityRelationNewPerson,
                Func<CompletedNewPersonPoliticalEntityRelationExistingPerson, T> completedNewPersonPoliticalEntityRelationExistingPerson
             )
            {
                return completedNewPersonPoliticalEntityRelationNewPerson(this);
            }

            public required PersonName Person { get; set; }
            public required PoliticalEntityListItem PoliticalEntity { get; set; }
            public override string PersonName => Person.Name;
            public override string PoliticalEntityName => PoliticalEntity.Name;
            public override PersonItem? PersonItem => Person;
            public override PoliticalEntityListItem? PoliticalEntityItem => PoliticalEntity;

        }
    }


}

