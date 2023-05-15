namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(ExistingPersonPoliticalEntityRelation))]
public partial class ExistingPersonPoliticalEntityRelationJsonContext : JsonSerializerContext { }

public interface PersonPoliticalEntityRelation : Relation
{
    PersonPoliticalEntityRelationTypeListItem PersonPoliticalEntityRelationType { get; set; }

    PersonItem? PersonItem { get; }
    PoliticalEntityListItem? PoliticalEntityItem { get; }
}
public interface IncompletePersonPoliticalEntityRelation : PersonPoliticalEntityRelation
{
    CompletedPersonPoliticalEntityRelation GetCompletedRelation(PoliticalEntityListItem politicalEntity);
}
public interface CompletedPersonPoliticalEntityRelation: PersonPoliticalEntityRelation
{
    string PersonName { get; }

    string PoliticalEntityName { get; }
}
public interface ResolvedPersonPoliticalEntityRelation: CompletedPersonPoliticalEntityRelation
{

}
public record CompletedNewPersonPoliticalEntityRelationNewPerson : PersonPoliticalEntityRelationBase, NewNode, CompletedPersonPoliticalEntityRelation
{
    public required PersonItem.PersonName Person { get; set; }
    public required PoliticalEntityListItem PoliticalEntity { get; set; }
    public string PersonName => Person.Name;
    public string PoliticalEntityName => PoliticalEntity.Name;
    public override PersonItem? PersonItem => Person;
    public override PoliticalEntityListItem? PoliticalEntityItem => PoliticalEntity;

}
public record CompletedNewPersonPoliticalEntityRelationExistingPerson : PersonPoliticalEntityRelationBase, NewNode, CompletedPersonPoliticalEntityRelation
{
    public required PersonItem.PersonListItem Person { get; set; }
    public required PoliticalEntityListItem PoliticalEntity { get; set; }

    public string PersonName => Person.Name;
    public string PoliticalEntityName => PoliticalEntity.Name;
    public override PersonItem? PersonItem => Person;
    public override PoliticalEntityListItem? PoliticalEntityItem => PoliticalEntity;
}

public record NewPersonPoliticalEntityRelationNewPerson : PersonPoliticalEntityRelationBase, NewNode, IncompletePersonPoliticalEntityRelation
{
    public required PersonItem.PersonName Person { get; set; }
    public required PoliticalEntityListItem? PoliticalEntity { get; set; }
    public override PersonItem? PersonItem => Person;
    public override PoliticalEntityListItem? PoliticalEntityItem => PoliticalEntity;
    public CompletedPersonPoliticalEntityRelation GetCompletedRelation(PoliticalEntityListItem politicalEntity)
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
public record NewPersonPoliticalEntityRelationExistingPerson : PersonPoliticalEntityRelationBase, NewNode, IncompletePersonPoliticalEntityRelation
{
    public required PersonItem.PersonListItem Person { get; set; }
    public required PoliticalEntityListItem? PoliticalEntity { get; set; }
    public override PersonItem? PersonItem => Person;
    public override PoliticalEntityListItem? PoliticalEntityItem => PoliticalEntity;
    public CompletedPersonPoliticalEntityRelation GetCompletedRelation(PoliticalEntityListItem politicalEntity)
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

public record ExistingPersonPoliticalEntityRelation : PersonPoliticalEntityRelationBase, ExistingNode, ResolvedPersonPoliticalEntityRelation
{
    public int NodeId { get; init; }

    public int UrlId { get; set; }
    public required PersonItem.PersonListItem Person { get; set; }
    public required PoliticalEntityListItem PoliticalEntity { get; set; }
    public string PersonName => Person.Name;
    public string PoliticalEntityName => PoliticalEntity.Name;
    public override PersonItem? PersonItem => Person;
    public override PoliticalEntityListItem? PoliticalEntityItem => PoliticalEntity;


}
public abstract record PersonPoliticalEntityRelationBase : RelationBase, PersonPoliticalEntityRelation
{
    public required PersonPoliticalEntityRelationTypeListItem PersonPoliticalEntityRelationType { get; set; }

    public abstract PersonItem? PersonItem { get; }
    public abstract PoliticalEntityListItem? PoliticalEntityItem { get; }

}
