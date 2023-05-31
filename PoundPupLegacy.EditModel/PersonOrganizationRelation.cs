namespace PoundPupLegacy.EditModel;


public interface ResolvedPersonOrganizationRelation : Relation
{
    PersonListItem Person { get; }
    OrganizationListItem Organization { get; }

    PersonOrganizationRelationTypeListItem PersonOrganizationRelationType { get; set; }
    GeographicalEntityListItem? GeographicalEntity { get; set; }

}

public interface ExistingPersonOrganizationRelation : ResolvedPersonOrganizationRelation, ExistingNode
{
}

public interface CompletedNewPersonOrganizationRelation : ResolvedPersonOrganizationRelation, NewNode
{
}

public static class PersonOrganizationRelationExtensions
{
    public static PersonOrganizationRelation.ForOrganization GetPersonOrganizationRelationForOrganization(this OrganizationListItem organizationListItem, PersonOrganizationRelationTypeListItem relationType, int ownerId, int publisherId)
    {
        return new PersonOrganizationRelation.ForOrganization.Incomplete.ToCreateForExistingOrganization {
            Person = null,
            Organization = organizationListItem,
            PersonOrganizationRelationType = relationType,
            GeographicalEntity = null,
            RelationDetails = RelationDetails.EmptyInstance,
            NodeDetailsForCreate = NodeDetails.EmptyInstance(48, "person organization relation", ownerId, publisherId)
        };
    }
    public static PersonOrganizationRelation.ForOrganization GetPersonOrganizationRelationForOrganization(this OrganizationName organizationName, PersonOrganizationRelationTypeListItem relationType, int ownerId, int publisherId)
    {
        return new PersonOrganizationRelation.ForOrganization.Incomplete.ToCreateForNewOrganization {
            Person = null,
            Organization = organizationName,
            PersonOrganizationRelationType = relationType,
            RelationDetails = RelationDetails.EmptyInstance,
            NodeDetailsForCreate = NodeDetails.EmptyInstance(48, "person organization relation", ownerId, publisherId)
        };
    }
    public static PersonOrganizationRelation.ForPerson GetPersonOrganizationRelationForPerson(this PersonListItem personListItem, PersonOrganizationRelationTypeListItem relationType, int ownerId, int publisherId)
    {
        return new PersonOrganizationRelation.ForPerson.Incomplete.ToCreateForExistingPerson {
            Person = personListItem,
            Organization = null,
            PersonOrganizationRelationType = relationType,
            RelationDetails = RelationDetails.EmptyInstance,
            NodeDetailsForCreate = NodeDetails.EmptyInstance(48,"person organization relation", ownerId, publisherId)
        };
    }
    public static PersonOrganizationRelation.ForPerson GetPersonOrganizationRelationForPerson(this PersonName personName, PersonOrganizationRelationTypeListItem relationType, int ownerId, int publisherId)
    {
        return new PersonOrganizationRelation.ForPerson.Incomplete.ToCreateForNewPerson {
            Person = personName,
            Organization = null,
            PersonOrganizationRelationType = relationType,
            RelationDetails = RelationDetails.EmptyInstance,
            NodeDetailsForCreate = NodeDetails.EmptyInstance(48, "person organization relation", ownerId, publisherId)
        };
    }
}

public abstract record PersonOrganizationRelation : Relation
{
    private PersonOrganizationRelation() { }
    public required RelationDetails RelationDetails { get; init; }
    public abstract NodeDetails NodeDetails { get; }
    public required PersonOrganizationRelationTypeListItem PersonOrganizationRelationType { get; set; }
    public GeographicalEntityListItem? GeographicalEntity { get; set; }

    public abstract record ForOrganization : PersonOrganizationRelation
    {
        private ForOrganization() { }
        public abstract PersonListItem? PersonItem { get; set; }
        public abstract OrganizationItem OrganizationItem { get; }

        [RequireNamedArgs]
        public abstract T Match<T>(
            Func<Complete, T> complete,
            Func<Incomplete, T> incomplete
        );

        public abstract record Complete : ForOrganization
        {

            private Complete() { }
            public abstract string PersonName { get; }
            public abstract string OrganizationName { get; }

            public override T Match<T>(
                Func<Complete, T> complete,
                Func<Incomplete, T> incomplete
            )
            {
                return complete(this);
            }

            public sealed record ToCreateForNewOrganization : Complete, NewNode
            {
                public override NodeDetails NodeDetails => NodeDetailsForCreate;
                public required NodeDetails.ForCreate NodeDetailsForCreate { get; init; }
                public required PersonListItem Person { get; set; }
                public required OrganizationName Organization { get; set; }
                private PersonListItem? personItem = null;
                public override PersonListItem? PersonItem {
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

                public override OrganizationItem OrganizationItem => Organization;
                public override string PersonName => Person.Name;
                public override string OrganizationName => Organization.Name;
            }

            public abstract record Resolved : Complete
            {
                private Resolved() { }
                public sealed record ToUpdate : Resolved, ExistingPersonOrganizationRelation
                {
                    public override NodeDetails NodeDetails => NodeDetailsForUpdate;
                    public required NodeDetails.ForUpdate NodeDetailsForUpdate { get; init; }
                    public required NodeIdentification NodeIdentification { get; init; }
                    public required PersonListItem Person { get; set; }
                    public required OrganizationListItem Organization { get; set; }

                    private PersonListItem? personItem = null;
                    public override PersonListItem? PersonItem {
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
                    public override OrganizationListItem OrganizationItem => Organization;
                    public override string PersonName => Person.Name;
                    public override string OrganizationName => Organization.Name;

                }

                public sealed record ToCreate : Resolved, CompletedNewPersonOrganizationRelation
                {
                    public override NodeDetails NodeDetails => NodeDetailsForCreate;
                    public required NodeDetails.ForCreate NodeDetailsForCreate { get; init; }
                    public required PersonListItem Person { get; set; }
                    public required OrganizationListItem Organization { get; set; }

                    private PersonListItem? personItem = null;
                    public override PersonListItem? PersonItem {
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

                    public override OrganizationItem OrganizationItem => Organization;
                    public override string PersonName => Person.Name;
                    public override string OrganizationName => Organization.Name;
                }
            }
        }
        public abstract record Incomplete : ForOrganization
        {
            public override NodeDetails NodeDetails => NodeDetailsForCreate;
            public required NodeDetails.ForCreate NodeDetailsForCreate { get; init; }
            public override T Match<T>(
                Func<Complete, T> completedPersonOrganizationRelationForOrganization,
                Func<Incomplete, T> incompletePersonOrganizationRelationForOrganization
            )
            {
                return incompletePersonOrganizationRelationForOrganization(this);
            }
            public abstract Complete GetCompletedRelation(PersonListItem personListItem);
            public sealed record ToCreateForExistingOrganization : Incomplete, NewNode
            {
                public required PersonListItem? Person { get; set; }
                public required OrganizationListItem Organization { get; set; }
                private PersonListItem? personItem = null;
                public override PersonListItem? PersonItem {
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
                public override OrganizationItem OrganizationItem => Organization;
                public override Complete GetCompletedRelation(PersonListItem personListItem)
                {
                    return new Complete.Resolved.ToCreate {
                        Person = personListItem,
                        Organization = Organization,
                        PersonOrganizationRelationType = PersonOrganizationRelationType,
                        GeographicalEntity = GeographicalEntity,
                        RelationDetails = RelationDetails,
                        NodeDetailsForCreate = NodeDetailsForCreate
                    };
                }
            }
            public sealed record ToCreateForNewOrganization : Incomplete, NewNode
            {
                public required PersonListItem? Person { get; set; }
                public required OrganizationName Organization { get; set; }
                private PersonListItem? personItem = null;
                public override PersonListItem? PersonItem {
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

                public override OrganizationItem OrganizationItem => Organization;
                public override Complete GetCompletedRelation(PersonListItem personListItem)
                {
                    return new Complete.ToCreateForNewOrganization {
                        Person = personListItem,
                        Organization = Organization,
                        PersonOrganizationRelationType = PersonOrganizationRelationType,
                        GeographicalEntity = GeographicalEntity,
                        RelationDetails = RelationDetails,
                        NodeDetailsForCreate = NodeDetailsForCreate
                    };
                }
            }
        }
    }

    public abstract record ForPerson : PersonOrganizationRelation
    {
        public abstract PersonItem PersonItem { get; }
        public abstract OrganizationListItem? OrganizationItem { get; set; }

        [RequireNamedArgs]
        public abstract T Match<T>(
            Func<Complete, T> complete,
            Func<Incomplete, T> incomplete
        );

        public abstract record Complete : ForPerson
        {
            public abstract string PersonName { get; }
            public abstract string OrganizationName { get; }
            public override T Match<T>(
                Func<Complete, T> complete,
                Func<Incomplete, T> incomplete
            )
            {
                return complete(this);
            }
            public sealed record ToCreateForNewPerson : Complete, NewNode
            {
                public override NodeDetails NodeDetails => NodeDetailsForCreate;
                public required NodeDetails.ForCreate NodeDetailsForCreate { get; init; }
                public required PersonName Person { get; set; }
                public required OrganizationListItem Organization { get; set; }
                public override PersonItem PersonItem => Person;

                private OrganizationListItem? organizationItem = null;
                public override OrganizationListItem? OrganizationItem {
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
                public override string PersonName => Person.Name;
                public override string OrganizationName => Organization.Name;
            }

            public abstract record Resolved : Complete
            {

                public sealed record ToUpdate : Resolved, ExistingPersonOrganizationRelation
                {
                    public override NodeDetails NodeDetails => NodeDetailsForUpdate;
                    public required NodeDetails.ForUpdate NodeDetailsForUpdate { get; init; }
                    public required NodeIdentification NodeIdentification { get; init; }
                    public int NodeId { get; init; }
                    public int UrlId { get; set; }
                    public required PersonListItem Person { get; set; }
                    public required OrganizationListItem Organization { get; set; }
                    public override PersonItem PersonItem => Person;

                    private OrganizationListItem? organizationItem = null;
                    public override OrganizationListItem? OrganizationItem {
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
                    public override string PersonName => Person.Name;
                    public override string OrganizationName => Organization.Name;

                }

                public sealed record ToCreate : Resolved, CompletedNewPersonOrganizationRelation
                {
                    public override NodeDetails NodeDetails => NodeDetailsForCreate;
                    public required NodeDetails.ForCreate NodeDetailsForCreate { get; init; }
                    public required PersonListItem Person { get; set; }
                    public required OrganizationListItem Organization { get; set; }
                    public override PersonItem PersonItem => Person;

                    private OrganizationListItem? organizationItem = null;
                    public override OrganizationListItem? OrganizationItem {
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
                    public override string PersonName => Person.Name;
                    public override string OrganizationName => Organization.Name;
                }

            }
        }
        public abstract record Incomplete : ForPerson, NewNode
        {
            public override NodeDetails NodeDetails => NodeDetailsForCreate;
            public required NodeDetails.ForCreate NodeDetailsForCreate { get; init; }
            public override T Match<T>(
                Func<Complete, T> complete,
                Func<Incomplete, T> incomplete
            )
            {
                return incomplete(this);
            }

            public abstract Complete GetCompletedRelation(OrganizationListItem organizationListItem);

            public sealed record ToCreateForExistingPerson : Incomplete
            {
                public required PersonListItem Person { get; set; }
                public required OrganizationListItem? Organization { get; set; }
                public override PersonItem PersonItem => Person;

                private OrganizationListItem? organizationItem = null;
                public override OrganizationListItem? OrganizationItem {
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
                public override Complete GetCompletedRelation(OrganizationListItem organizationListItem)
                {
                    return new Complete.Resolved.ToCreate {
                        Person = Person,
                        Organization = organizationListItem,
                        PersonOrganizationRelationType = PersonOrganizationRelationType,
                        GeographicalEntity = GeographicalEntity,
                        RelationDetails = RelationDetails,
                        NodeDetailsForCreate = NodeDetailsForCreate
                    };
                }
            }

            public sealed record ToCreateForNewPerson : Incomplete
            {
                public required PersonName Person { get; set; }
                public required OrganizationListItem? Organization { get; set; }
                public override PersonItem PersonItem => Person;
                private OrganizationListItem? organizationItem = null;
                public override OrganizationListItem? OrganizationItem {
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
                public override Complete GetCompletedRelation(OrganizationListItem organizationListItem)
                {
                    return new Complete.ToCreateForNewPerson {
                        Person = Person,
                        Organization = organizationListItem,
                        PersonOrganizationRelationType = PersonOrganizationRelationType,
                        GeographicalEntity = GeographicalEntity,
                        RelationDetails = RelationDetails,
                        NodeDetailsForCreate = NodeDetailsForCreate
                    };
                }
            }
        }
    }
}

