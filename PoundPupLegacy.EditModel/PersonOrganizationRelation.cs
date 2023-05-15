namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(ExistingPersonOrganizationRelationForPerson))]
public partial class ExistingPersonOrganizationRelationForPersonJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(ExistingPersonOrganizationRelationForOrganization))]
public partial class ExistingPersonOrganizationRelationForOrganizationJsonContext : JsonSerializerContext { }


public interface PersonOrganizationRelation : Relation
{
    PersonOrganizationRelationTypeListItem PersonOrganizationRelationType { get; set; }
    GeographicalEntityListItem? GeographicalEntity { get; set; }

}

public interface PersonOrganizationRelationForPerson : PersonOrganizationRelation
{
    PersonItem PersonItem { get; }
    OrganizationItem.OrganizationListItem? OrganizationItem { get; set; }
}
public interface PersonOrganizationRelationForOrganization : PersonOrganizationRelation
{
    PersonItem.PersonListItem? PersonItem { get; set; }
    OrganizationItem OrganizationItem { get; }

}
public interface CompletedPersonOrganizationRelation: PersonOrganizationRelation
{

}
public interface CompletedPersonOrganizationRelationForPerson : CompletedPersonOrganizationRelation, PersonOrganizationRelationForPerson
{
    string PersonName { get; }
    string OrganizationName { get; }

}
public interface CompletedPersonOrganizationRelationForOrganization : CompletedPersonOrganizationRelation, PersonOrganizationRelationForOrganization
{
    string PersonName { get; }
    string OrganizationName { get; }

}
public interface ResolvedPersonOrganizationRelation: CompletedPersonOrganizationRelation
{
}
public interface ResolvedPersonOrganizationRelationForPerson : ResolvedPersonOrganizationRelation, CompletedPersonOrganizationRelationForPerson
{
}
public interface ResolvedPersonOrganizationRelationForOrganization : ResolvedPersonOrganizationRelation, CompletedPersonOrganizationRelationForOrganization
{
}
public interface ExistingPersonOrganizationRelation : ResolvedPersonOrganizationRelation, ExistingNode
{
    PersonItem.PersonListItem Person { get;  }
    OrganizationItem.OrganizationListItem Organization { get;  }

}

public interface CompletedNewPersonOrganizationRelation: NewNode, CompletedPersonOrganizationRelation
{
    PersonItem.PersonListItem Person { get; }
    OrganizationItem.OrganizationListItem Organization { get; }


}

public record CompletedNewPersonOrganizationRelationForOrganization: PersonOrganizationRelationBase, CompletedNewPersonOrganizationRelation, CompletedPersonOrganizationRelationForOrganization
{
    public required PersonItem.PersonListItem Person { get; set; }
    public required OrganizationItem.OrganizationListItem Organization { get; set; }

    private PersonItem.PersonListItem? personItem = null;
    public PersonItem.PersonListItem? PersonItem {
        get {
            if (personItem == null) {
                personItem = Person;
            }
            return personItem;
        }
        set {
            personItem = value;
        }
    }

    public OrganizationItem OrganizationItem => Organization;
    public string PersonName => Person.Name;
    public string OrganizationName => Organization.Name;
}
public record CompletedNewPersonOrganizationRelationForPerson : PersonOrganizationRelationBase, CompletedNewPersonOrganizationRelation, CompletedPersonOrganizationRelationForPerson
{
    public required PersonItem.PersonListItem Person { get; set; }
    public required OrganizationItem.OrganizationListItem Organization { get; set; }
    public PersonItem PersonItem => Person;

    private OrganizationItem.OrganizationListItem? organizationItem = null;
    public OrganizationItem.OrganizationListItem? OrganizationItem {
        get {
            if (organizationItem == null) {
                organizationItem = Organization;
            }
            return organizationItem;
        }
        set {
            organizationItem = value;
        }
    }
    public string PersonName => Person.Name;
    public string OrganizationName => Organization.Name;
}

public record NewPersonOrganizationRelationExistingPerson : PersonOrganizationRelationBase, NewNode, PersonOrganizationRelationForPerson
{
    public required PersonItem.PersonListItem Person { get; set; }
    public required OrganizationItem.OrganizationListItem? Organization { get; set; }
    public PersonItem PersonItem => Person;

    private OrganizationItem.OrganizationListItem? organizationItem = null;
    public OrganizationItem.OrganizationListItem? OrganizationItem {
        get {
            if (organizationItem == null) {
                organizationItem = Organization;
            }
            return organizationItem;
        }
        set {
            organizationItem = value;
        }
    }

    public CompletedPersonOrganizationRelationForPerson GetCompletedRelation(OrganizationItem.OrganizationListItem organizationListItem)
    {
        return new CompletedNewPersonOrganizationRelationForPerson {
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

public record NewPersonOrganizationRelationExistingOrganization : PersonOrganizationRelationBase, NewNode, PersonOrganizationRelationForOrganization
{
    public required PersonItem.PersonListItem? Person { get; set; }
    public required OrganizationItem.OrganizationListItem Organization { get; set; }
    private PersonItem.PersonListItem? personItem = null;
    public PersonItem.PersonListItem? PersonItem {
        get {
            if (personItem == null) {
                personItem = Person;
            }
            return personItem;
        }
        set {
            personItem = value;
        }
    }
    public OrganizationItem OrganizationItem => Organization;
    public CompletedPersonOrganizationRelationForOrganization GetCompletedRelation(PersonItem.PersonListItem personListItem)
    {
        return new CompletedNewPersonOrganizationRelationForOrganization {
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
public record CompletedNewPersonOrganizationRelationNewOrganization : PersonOrganizationRelationBase, NewNode, CompletedPersonOrganizationRelationForOrganization
{
    public required PersonItem.PersonListItem Person { get; set; }
    public required OrganizationItem.OrganizationName Organization { get; set; }
    private PersonItem.PersonListItem? personItem = null;
    public PersonItem.PersonListItem? PersonItem {
        get {
            if (personItem == null) {
                personItem = Person;
            }
            return personItem;
        }
        set {
            personItem = value;
        }
    }

    public OrganizationItem OrganizationItem => Organization;
    public string PersonName => Person.Name;
    public string OrganizationName => Organization.Name;
}

public record NewPersonOrganizationRelationNewOrganization : PersonOrganizationRelationBase, NewNode, PersonOrganizationRelationForOrganization
{
    public required PersonItem.PersonListItem? Person { get; set; }
    public required OrganizationItem.OrganizationName Organization { get; set; }
    private PersonItem.PersonListItem? personItem = null;
    public PersonItem.PersonListItem? PersonItem {
        get {
            if (personItem == null) {
                personItem = Person;
            }
            return personItem;
        }
        set {
            personItem = value;
        }
    }

    public OrganizationItem OrganizationItem => Organization;
    public CompletedPersonOrganizationRelationForOrganization GetCompletedRelation(PersonItem.PersonListItem personListItem)
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
public record CompletedNewPersonOrganizationRelationNewPerson : PersonOrganizationRelationBase, NewNode, CompletedPersonOrganizationRelationForPerson
{
    public required PersonItem.PersonName Person { get; set; }
    public required OrganizationItem.OrganizationListItem Organization { get; set; }
    public PersonItem PersonItem => Person;

    private OrganizationItem.OrganizationListItem? organizationItem = null;
    public OrganizationItem.OrganizationListItem? OrganizationItem {
        get {
            if (organizationItem == null) {
                organizationItem = Organization;
            }
            return organizationItem;
        }
        set {
            organizationItem = value;
        }
    }
    public string PersonName => Person.Name;
    public string OrganizationName => Organization.Name;
}

public record NewPersonOrganizationRelationNewPerson : PersonOrganizationRelationBase, NewNode, PersonOrganizationRelationForPerson
{
    public required PersonItem.PersonName Person { get; set; }
    public required OrganizationItem.OrganizationListItem? Organization { get; set; }
    public PersonItem PersonItem => Person;

    private OrganizationItem.OrganizationListItem? organizationItem = null;
    public OrganizationItem.OrganizationListItem? OrganizationItem {
        get {
            if (organizationItem == null) {
                organizationItem = Organization;
            }
            return organizationItem;
        }
        set {
            organizationItem = value;
        }
    }
    public CompletedPersonOrganizationRelationForPerson GetCompletedRelation(OrganizationItem.OrganizationListItem organizationListItem)
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
public record ExistingPersonOrganizationRelationForOrganization: PersonOrganizationRelationBase, ExistingPersonOrganizationRelation, ResolvedPersonOrganizationRelationForOrganization
{
    public int NodeId { get; init; }

    public int UrlId { get; set; }
    public required PersonItem.PersonListItem Person { get; set; }
    public required OrganizationItem.OrganizationListItem Organization { get; set; }

    private PersonItem.PersonListItem? personItem = null;
    public PersonItem.PersonListItem? PersonItem {
        get {
            if (personItem == null) {
                personItem = Person;
            }
            return personItem;
        }
        set {
            personItem = value;
        }
    }

    public OrganizationItem OrganizationItem => Organization;
    public string PersonName => Person.Name;
    public string OrganizationName => Organization.Name;

}

public record ExistingPersonOrganizationRelationForPerson : PersonOrganizationRelationBase, ExistingPersonOrganizationRelation, ResolvedPersonOrganizationRelationForPerson
{
    public int NodeId { get; init; }

    public int UrlId { get; set; }
    public required PersonItem.PersonListItem Person { get; set; }
    public required OrganizationItem.OrganizationListItem Organization { get; set; }
    public PersonItem PersonItem => Person;

    private OrganizationItem.OrganizationListItem? organizationItem = null;
    public OrganizationItem.OrganizationListItem? OrganizationItem {
        get {
            if (organizationItem == null) {
                organizationItem = Organization;
            }
            return organizationItem;
        }
        set {
            organizationItem = value;
        }
    }
    public string PersonName => Person.Name;
    public string OrganizationName => Organization.Name;

}
public abstract record PersonOrganizationRelationBaseForOrganization : PersonOrganizationRelationBase, PersonOrganizationRelationForOrganization
{
    public abstract PersonItem.PersonListItem? PersonItem { get; set; }
    public abstract OrganizationItem OrganizationItem { get; }

}
public abstract record PersonOrganizationRelationBaseForPerson : PersonOrganizationRelationBase, PersonOrganizationRelationForPerson
{
    public abstract PersonItem PersonItem { get; }
    public abstract OrganizationItem.OrganizationListItem? OrganizationItem { get; set; }
}
public abstract record PersonOrganizationRelationBase : RelationBase, PersonOrganizationRelation
{
    public required PersonOrganizationRelationTypeListItem PersonOrganizationRelationType { get; set; }
    public GeographicalEntityListItem? GeographicalEntity { get; set; }
}
