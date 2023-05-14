namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(ExistingPersonOrganizationRelation))]
public partial class ExistingPersonOrganizationRelationJsonContext : JsonSerializerContext { }

public enum PartyType
{
    Person,
    Organization
}

public interface PersonOrganizationRelation : Relation
{
    PersonOrganizationRelationTypeListItem PersonOrganizationRelationType { get; set; }
    GeographicalEntityListItem? GeographicalEntity { get; set; }

    PersonItem? PersonItem { get; }
    OrganizationItem? OrganizationItem { get; }
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
    public required PersonListItem Person { get; set; }
    public required OrganizationListItem Organization { get; set; }
    public override PersonItem? PersonItem => Person;
    public override OrganizationItem? OrganizationItem => Organization;
    public override PartyType LeadingPartyType => SettableLeadingPartyType;
    public required PartyType SettableLeadingPartyType { get; init; }
    public string PersonName => Person.Name;
    public string OrganizationName => Organization.Name;
}

public record NewPersonOrganizationRelationExistingPerson : PersonOrganizationRelationBase, NewNode
{
    public required PersonListItem Person { get; set; }
    public required OrganizationListItem? Organization { get; set; }
    public override PersonItem? PersonItem => Person;
    public override OrganizationItem? OrganizationItem => Organization;
    public override PartyType LeadingPartyType => PartyType.Person;
}

public record NewPersonOrganizationRelationExistingOrganization : PersonOrganizationRelationBase, NewNode
{
    public required PersonListItem? Person { get; set; }
    public required OrganizationListItem Organization { get; set; }
    public override PersonItem? PersonItem => Person;
    public override OrganizationItem? OrganizationItem => Organization;
    public override PartyType LeadingPartyType => PartyType.Organization;
}

public record NewPersonOrganizationRelationNewOrganization : PersonOrganizationRelationBase, NewNode
{
    public required PersonListItem? Person { get; set; }
    public required OrganizationName Organization { get; set; }
    public override PersonItem? PersonItem => Person;
    public override OrganizationItem? OrganizationItem => Organization;
    public override PartyType LeadingPartyType => PartyType.Organization;
}
public record NewPersonOrganizationRelationNewPerson : PersonOrganizationRelationBase, NewNode
{
    public required PersonName Person { get; set; }
    public required OrganizationListItem? Organization { get; set; }
    public override PersonItem? PersonItem => Person;
    public override OrganizationItem? OrganizationItem => Organization;
    public override PartyType LeadingPartyType => PartyType.Person;
}
public record ExistingPersonOrganizationRelation: PersonOrganizationRelationBase, ExistingNode, ResolvedPersonOrganizationRelation
{
    public int NodeId { get; init; }

    public int UrlId { get; set; }
    public required PersonListItem Person { get; set; }
    public required OrganizationListItem Organization { get; set; }
    public override PersonItem? PersonItem => Person;
    public override OrganizationItem? OrganizationItem => Organization;
    public override PartyType LeadingPartyType => SettableLeadingPartyType;
    public required PartyType SettableLeadingPartyType { get; init; }
    public string PersonName => Person.Name;
    public string OrganizationName => Organization.Name;

}
public abstract record PersonOrganizationRelationBase : RelationBase, PersonOrganizationRelation
{
    public required PersonOrganizationRelationTypeListItem PersonOrganizationRelationType { get; set; }
    public GeographicalEntityListItem? GeographicalEntity { get; set; }
    public abstract PersonItem? PersonItem { get; }
    public abstract OrganizationItem? OrganizationItem {get;}
    public abstract PartyType LeadingPartyType { get; }
}
