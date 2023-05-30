namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(ExistingPersonPoliticalEntityRelation))]
public partial class ExistingPersonPoliticalEntityRelationJsonContext : JsonSerializerContext { }

public static class PersonPoliticalEntityRelationExtensions
{
    public static PersonPoliticalEntityRelation GetPersonPoliticalEntityRelation(this PersonListItem personListItem, PersonPoliticalEntityRelationTypeListItem relationType, int ownerId, int publisherId)
    {
        return new NewPersonPoliticalEntityRelationExistingPerson {
            Person = personListItem,
            PersonPoliticalEntityRelationType = relationType,        
            RelationDetails = RelationDetails.EmptyInstance,
            NodeDetailsForCreate = NodeDetails.EmptyInstance(49, "person political entity relation", ownerId, publisherId),
            PoliticalEntity = null
        };
    }
    public static PersonPoliticalEntityRelation GetPersonPoliticalEntityRelation(this PersonName personName, PersonPoliticalEntityRelationTypeListItem relationType, int ownerId, int publisherId)
    {
        return new NewPersonPoliticalEntityRelationNewPerson {
            Person = personName,
            PersonPoliticalEntityRelationType = relationType,        
            RelationDetails = RelationDetails.EmptyInstance,
            NodeDetailsForCreate = NodeDetails.EmptyInstance(49, "person political entity relation", ownerId, publisherId),
            PoliticalEntity = null
        };
    }
}

public abstract record PersonPoliticalEntityRelation : Relation
{
    private PersonPoliticalEntityRelation() { }

    public required RelationDetails RelationDetails { get; init; }
    public abstract NodeDetails NodeDetails { get; }
    public required PersonPoliticalEntityRelationTypeListItem PersonPoliticalEntityRelationType { get; set; }

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
            public override NodeDetails NodeDetails => NodeDetailsForCreate;
            public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }

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
                    NodeDetailsForCreate = NodeDetailsForCreate,
                    PersonPoliticalEntityRelationType = PersonPoliticalEntityRelationType,
                    RelationDetails = RelationDetails
                };
            }
        }
        public sealed record NewPersonPoliticalEntityRelationExistingPerson : IncompletePersonPoliticalEntityRelation, NewNode
        {
            public override NodeDetails NodeDetails => NodeDetailsForCreate;
            public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
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
                    NodeDetailsForCreate = NodeDetailsForCreate,
                    PersonPoliticalEntityRelationType = PersonPoliticalEntityRelationType,
                    RelationDetails = RelationDetails
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
                public override NodeDetails NodeDetails => NodeDetailsForUpdate;
                public required NodeDetails.NodeDetailsForUpdate NodeDetailsForUpdate { get; init; }
                public required NodeIdentification NodeIdentification { get; init; }
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
                public override NodeDetails NodeDetails => NodeDetailsForCreate;
                public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }

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
            public override NodeDetails NodeDetails => NodeDetailsForCreate;
            public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
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

