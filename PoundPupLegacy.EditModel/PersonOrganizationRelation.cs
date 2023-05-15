namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(ExistingPersonOrganizationRelation))]
public partial class ExistingPersonOrganizationRelationJsonContext : JsonSerializerContext { }


public interface PersonOrganizationRelation : Relation
{
    PersonOrganizationRelationTypeListItem PersonOrganizationRelationType { get; set; }
    GeographicalEntityListItem? GeographicalEntity { get; set; }

    PartyItem.PersonItem? PersonItem { get; }
    PartyItem.OrganizationItem? OrganizationItem { get; }
    PartyType LeadingPartyType { get; }
}

public interface CompletedPersonOrganizationRelation : PersonOrganizationRelation
{
    string PersonName { get; }
    string OrganizationName { get; }

}
public interface ResolvedPersonOrganizationRelation : CompletedPersonOrganizationRelation
{

}
public record CompletedNewPersonOrganizationRelation: PersonOrganizationRelationBase, NewNode, CompletedPersonOrganizationRelation
{
    public required PartyItem.PersonListItem Person { get; set; }
    public required PartyItem.OrganizationListItem Organization { get; set; }
    public override PartyItem.PersonItem? PersonItem => Person;
    public override PartyItem.OrganizationItem? OrganizationItem => Organization;
    public override PartyType LeadingPartyType => SettableLeadingPartyType;
    public required PartyType SettableLeadingPartyType { get; init; }
    public string PersonName => Person.Name;
    public string OrganizationName => Organization.Name;
}

public record NewPersonOrganizationRelationExistingPerson : PersonOrganizationRelationBase, NewNode
{
    public required PartyItem.PersonListItem Person { get; set; }
    public required PartyItem.OrganizationListItem? Organization { get; set; }
    public override PartyItem.PersonItem? PersonItem => Person;
    public override PartyItem.OrganizationItem? OrganizationItem => Organization;
    public override PartyType LeadingPartyType => PartyType.Person;

    public CompletedPersonOrganizationRelation GetCompletedRelation(PartyItem.OrganizationListItem organizationListItem)
    {
        return new CompletedNewPersonOrganizationRelation {
            Person = Person,
            Organization = organizationListItem,
            DateFrom = DateFrom,
            DateTo = DateTo,
            PersonOrganizationRelationType = PersonOrganizationRelationType,
            GeographicalEntity = GeographicalEntity,
            SettableLeadingPartyType = LeadingPartyType,
            Description = Description,
            Files = Files,
            HasBeenDeleted = HasBeenDeleted,
            NodeTypeName = NodeTypeName,
            OwnerId = OwnerId,
            PublisherId = PublisherId,
            ProofDocument = ProofDocument,
            Tags = Tags,
            TenantNodes = TenantNodes,
            Title = Title,
            Tenants = Tenants,
        };
    }
}

public record NewPersonOrganizationRelationExistingOrganization : PersonOrganizationRelationBase, NewNode
{
    public required PartyItem.PersonListItem? Person { get; set; }
    public required PartyItem.OrganizationListItem Organization { get; set; }
    public override PartyItem.PersonItem? PersonItem => Person;
    public override PartyItem.OrganizationItem? OrganizationItem => Organization;
    public override PartyType LeadingPartyType => PartyType.Organization;
    public CompletedPersonOrganizationRelation GetCompletedRelation(PartyItem.PersonListItem personListItem)
    {
        return new CompletedNewPersonOrganizationRelation {
            Person = personListItem,
            Organization = Organization,
            DateFrom = DateFrom,
            DateTo = DateTo,
            PersonOrganizationRelationType = PersonOrganizationRelationType,
            GeographicalEntity = GeographicalEntity,
            SettableLeadingPartyType = LeadingPartyType,
            Description = Description,
            Files = Files,
            HasBeenDeleted = HasBeenDeleted,
            NodeTypeName = NodeTypeName,
            OwnerId = OwnerId,
            PublisherId = PublisherId,
            ProofDocument = ProofDocument,
            Tags = Tags,
            TenantNodes = TenantNodes,
            Title = Title,
            Tenants = Tenants,
        };
    }
}
public record CompletedNewPersonOrganizationRelationNewOrganization : PersonOrganizationRelationBase, NewNode, CompletedPersonOrganizationRelation
{
    public required PartyItem.PersonListItem Person { get; set; }
    public required PartyItem.OrganizationName Organization { get; set; }
    public override PartyItem.PersonItem? PersonItem => Person;
    public override PartyItem.OrganizationItem? OrganizationItem => Organization;
    public override PartyType LeadingPartyType => PartyType.Organization;
    public string PersonName => Person.Name;
    public string OrganizationName => Organization.Name;
}

public record NewPersonOrganizationRelationNewOrganization : PersonOrganizationRelationBase, NewNode
{
    public required PartyItem.PersonListItem? Person { get; set; }
    public required PartyItem.OrganizationName Organization { get; set; }
    public override PartyItem.PersonItem? PersonItem => Person;
    public override PartyItem.OrganizationItem? OrganizationItem => Organization;
    public override PartyType LeadingPartyType => PartyType.Organization;
    public CompletedPersonOrganizationRelation GetCompletedRelation(PartyItem.PersonListItem personListItem)
    {
        return new CompletedNewPersonOrganizationRelationNewOrganization {
            Person = personListItem,
            Organization = Organization,
            DateFrom = DateFrom,
            DateTo = DateTo,
            PersonOrganizationRelationType = PersonOrganizationRelationType,
            GeographicalEntity = GeographicalEntity,
            Description = Description,
            Files = Files,
            HasBeenDeleted = HasBeenDeleted,
            NodeTypeName = NodeTypeName,
            OwnerId = OwnerId,
            PublisherId = PublisherId,
            ProofDocument = ProofDocument,
            Tags = Tags,
            TenantNodes = TenantNodes,
            Title = Title,
            Tenants = Tenants,
        };
    }

}
public record CompletedNewPersonOrganizationRelationNewPerson : PersonOrganizationRelationBase, NewNode, CompletedPersonOrganizationRelation
{
    public required PartyItem.PersonName Person { get; set; }
    public required PartyItem.OrganizationListItem Organization { get; set; }
    public override PartyItem.PersonItem? PersonItem => Person;
    public override PartyItem.OrganizationItem? OrganizationItem => Organization;
    public override PartyType LeadingPartyType => PartyType.Person;
    public string PersonName => Person.Name;
    public string OrganizationName => Organization.Name;
}

public record NewPersonOrganizationRelationNewPerson : PersonOrganizationRelationBase, NewNode
{
    public required PartyItem.PersonName Person { get; set; }
    public required PartyItem.OrganizationListItem? Organization { get; set; }
    public override PartyItem.PersonItem? PersonItem => Person;
    public override PartyItem.OrganizationItem? OrganizationItem => Organization;
    public override PartyType LeadingPartyType => PartyType.Person;
    public CompletedPersonOrganizationRelation GetCompletedRelation(PartyItem.OrganizationListItem organizationListItem)
    {
        return new CompletedNewPersonOrganizationRelationNewPerson {
            Person = Person,
            Organization = organizationListItem,
            DateFrom = DateFrom,
            DateTo = DateTo,
            PersonOrganizationRelationType = PersonOrganizationRelationType,
            GeographicalEntity = GeographicalEntity,
            Description = Description,
            Files = Files,
            HasBeenDeleted = HasBeenDeleted,
            NodeTypeName = NodeTypeName,
            OwnerId = OwnerId,
            PublisherId = PublisherId,
            ProofDocument = ProofDocument,
            Tags = Tags,
            TenantNodes = TenantNodes,
            Title = Title,
            Tenants = Tenants,
        };
    }

}
public record ExistingPersonOrganizationRelation: PersonOrganizationRelationBase, ExistingNode, ResolvedPersonOrganizationRelation
{
    public int NodeId { get; init; }

    public int UrlId { get; set; }
    public required PartyItem.PersonListItem Person { get; set; }
    public required PartyItem.OrganizationListItem Organization { get; set; }
    public override PartyItem.PersonItem? PersonItem => Person;
    public override PartyItem.OrganizationItem? OrganizationItem => Organization;
    public override PartyType LeadingPartyType => SettableLeadingPartyType;
    public required PartyType SettableLeadingPartyType { get; init; }
    public string PersonName => Person.Name;
    public string OrganizationName => Organization.Name;

}
public abstract record PersonOrganizationRelationBase : RelationBase, PersonOrganizationRelation
{
    public required PersonOrganizationRelationTypeListItem PersonOrganizationRelationType { get; set; }
    public GeographicalEntityListItem? GeographicalEntity { get; set; }
    public abstract PartyItem.PersonItem? PersonItem { get; }
    public abstract PartyItem.OrganizationItem? OrganizationItem {get;}
    public abstract PartyType LeadingPartyType { get; }
}
