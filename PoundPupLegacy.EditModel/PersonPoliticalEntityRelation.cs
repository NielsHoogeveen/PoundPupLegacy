namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(ExistingPersonPoliticalEntityRelation))]
public partial class ExistingPersonPoliticalEntityRelationJsonContext : JsonSerializerContext { }

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
                    TenantNodes = TenantNodes,
                    Tenants = Tenants,
                    Title = Title
                };
            }
        }
        public sealed record NewPersonPoliticalEntityRelationExistingPerson : IncompletePersonPoliticalEntityRelation, NewNode
        {
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
                    TenantNodes = TenantNodes,
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

