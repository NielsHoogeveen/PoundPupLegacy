namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(ExistingPersonOrganizationRelationForPerson))]
public partial class ExistingPersonOrganizationRelationForPersonJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(ExistingPersonOrganizationRelationForOrganization))]
public partial class ExistingPersonOrganizationRelationForOrganizationJsonContext : JsonSerializerContext { }

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

public abstract record PersonOrganizationRelation : RelationBase
{
    private PersonOrganizationRelation() { }
    public required PersonOrganizationRelationTypeListItem PersonOrganizationRelationType { get; set; }
    public GeographicalEntityListItem? GeographicalEntity { get; set; }

    public abstract record PersonOrganizationRelationForOrganization : PersonOrganizationRelation
    {
        private PersonOrganizationRelationForOrganization() { }
        public abstract PersonListItem? PersonItem { get; set; }
        public abstract OrganizationItem OrganizationItem { get; }

        [RequireNamedArgs]
        public abstract T Match<T>(
            Func<CompletedNewPersonOrganizationRelationNewOrganization, T> completedNewPersonOrganizationRelationNewOrganization,
            Func<ExistingPersonOrganizationRelationForOrganization, T> existingPersonOrganizationRelationForOrganization,
            Func<CompletedNewPersonOrganizationRelationForOrganization, T> completedNewPersonOrganizationRelationForOrganization,
            Func<NewPersonOrganizationRelationExistingOrganization, T> newPersonOrganizationRelationExistingOrganization,
            Func<NewPersonOrganizationRelationNewOrganization, T> newPersonOrganizationRelationNewOrganization
        );
        [RequireNamedArgs]
        public abstract T Match<T>(
            Func<CompletedPersonOrganizationRelationForOrganization, T> completedPersonOrganizationRelationForOrganization,
            Func<IncompletePersonOrganizationRelationForOrganization, T> incompletePersonOrganizationRelationForOrganization
        );

        public abstract record CompletedPersonOrganizationRelationForOrganization : PersonOrganizationRelationForOrganization
        {

            private CompletedPersonOrganizationRelationForOrganization() { }
            public abstract string PersonName { get; }
            public abstract string OrganizationName { get; }

            public override T Match<T>(
                Func<CompletedPersonOrganizationRelationForOrganization, T> completedPersonOrganizationRelationForOrganization,
                Func<IncompletePersonOrganizationRelationForOrganization, T> incompletePersonOrganizationRelationForOrganization
            )
            {
                return completedPersonOrganizationRelationForOrganization(this);
            }

            public sealed record CompletedNewPersonOrganizationRelationNewOrganization : CompletedPersonOrganizationRelationForOrganization, NewNode
            {
                public override T Match<T>(
                    Func<CompletedNewPersonOrganizationRelationNewOrganization, T> completedNewPersonOrganizationRelationNewOrganization,
                    Func<ExistingPersonOrganizationRelationForOrganization, T> existingPersonOrganizationRelationForOrganization,
                    Func<CompletedNewPersonOrganizationRelationForOrganization, T> completedNewPersonOrganizationRelationForOrganization,
                    Func<NewPersonOrganizationRelationExistingOrganization, T> newPersonOrganizationRelationExistingOrganization,
                    Func<NewPersonOrganizationRelationNewOrganization, T> newPersonOrganizationRelationNewOrganization
                )
                {
                    return completedNewPersonOrganizationRelationNewOrganization(this);
                }
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

            public abstract record ResolvedPersonOrganizationRelationForOrganization : CompletedPersonOrganizationRelationForOrganization
            {
                private ResolvedPersonOrganizationRelationForOrganization() { }
                public sealed record ExistingPersonOrganizationRelationForOrganization : ResolvedPersonOrganizationRelationForOrganization, ExistingPersonOrganizationRelation
                {
                    public override T Match<T>(
                        Func<CompletedNewPersonOrganizationRelationNewOrganization, T> completedNewPersonOrganizationRelationNewOrganization,
                        Func<ExistingPersonOrganizationRelationForOrganization, T> existingPersonOrganizationRelationForOrganization,
                        Func<CompletedNewPersonOrganizationRelationForOrganization, T> completedNewPersonOrganizationRelationForOrganization,
                        Func<NewPersonOrganizationRelationExistingOrganization, T> newPersonOrganizationRelationExistingOrganization,
                        Func<NewPersonOrganizationRelationNewOrganization, T> newPersonOrganizationRelationNewOrganization
                    )
                    {
                        return existingPersonOrganizationRelationForOrganization(this);
                    }

                    public int NodeId { get; init; }

                    public int UrlId { get; set; }
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

                public sealed record CompletedNewPersonOrganizationRelationForOrganization : ResolvedPersonOrganizationRelationForOrganization, CompletedNewPersonOrganizationRelation
                {
                    public override T Match<T>(
                        Func<CompletedNewPersonOrganizationRelationNewOrganization, T> completedNewPersonOrganizationRelationNewOrganization,
                        Func<ExistingPersonOrganizationRelationForOrganization, T> existingPersonOrganizationRelationForOrganization,
                        Func<CompletedNewPersonOrganizationRelationForOrganization, T> completedNewPersonOrganizationRelationForOrganization,
                        Func<NewPersonOrganizationRelationExistingOrganization, T> newPersonOrganizationRelationExistingOrganization,
                        Func<NewPersonOrganizationRelationNewOrganization, T> newPersonOrganizationRelationNewOrganization
                    )
                    {
                        return completedNewPersonOrganizationRelationForOrganization(this);
                    }
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
        public abstract record IncompletePersonOrganizationRelationForOrganization : PersonOrganizationRelationForOrganization
        {
            public override T Match<T>(
                Func<CompletedPersonOrganizationRelationForOrganization, T> completedPersonOrganizationRelationForOrganization,
                Func<IncompletePersonOrganizationRelationForOrganization, T> incompletePersonOrganizationRelationForOrganization
            )
            {
                return incompletePersonOrganizationRelationForOrganization(this);
            }
            public abstract CompletedPersonOrganizationRelationForOrganization GetCompletedRelation(PersonListItem personListItem);
            public sealed record NewPersonOrganizationRelationExistingOrganization : IncompletePersonOrganizationRelationForOrganization, NewNode
            {
                public override T Match<T>(
                    Func<CompletedNewPersonOrganizationRelationNewOrganization, T> completedNewPersonOrganizationRelationNewOrganization,
                    Func<ExistingPersonOrganizationRelationForOrganization, T> existingPersonOrganizationRelationForOrganization,
                    Func<CompletedNewPersonOrganizationRelationForOrganization, T> completedNewPersonOrganizationRelationForOrganization,
                    Func<NewPersonOrganizationRelationExistingOrganization, T> newPersonOrganizationRelationExistingOrganization,
                    Func<NewPersonOrganizationRelationNewOrganization, T> newPersonOrganizationRelationNewOrganization
                )
                {
                    return newPersonOrganizationRelationExistingOrganization(this);
                }

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
                public override CompletedPersonOrganizationRelationForOrganization GetCompletedRelation(PersonListItem personListItem)
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
            public sealed record NewPersonOrganizationRelationNewOrganization : IncompletePersonOrganizationRelationForOrganization, NewNode
            {
                public override T Match<T>(
                    Func<CompletedNewPersonOrganizationRelationNewOrganization, T> completedNewPersonOrganizationRelationNewOrganization,
                    Func<ExistingPersonOrganizationRelationForOrganization, T> existingPersonOrganizationRelationForOrganization,
                    Func<CompletedNewPersonOrganizationRelationForOrganization, T> completedNewPersonOrganizationRelationForOrganization,
                    Func<NewPersonOrganizationRelationExistingOrganization, T> newPersonOrganizationRelationExistingOrganization,
                    Func<NewPersonOrganizationRelationNewOrganization, T> newPersonOrganizationRelationNewOrganization
                )
                {
                    return newPersonOrganizationRelationNewOrganization(this);
                }

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
                public override CompletedPersonOrganizationRelationForOrganization GetCompletedRelation(PersonListItem personListItem)
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
        }
    }

    public abstract record PersonOrganizationRelationForPerson : PersonOrganizationRelation
    {
        public abstract PersonItem PersonItem { get; }
        public abstract OrganizationListItem? OrganizationItem { get; set; }

        [RequireNamedArgs]
        public abstract T Match<T>(
            Func<CompletedNewPersonOrganizationRelationNewPerson, T> completedNewPersonOrganizationRelationNewPerson,
            Func<ExistingPersonOrganizationRelationForPerson, T> existingPersonOrganizationRelationForPerson,
            Func<CompletedNewPersonOrganizationRelationForPerson, T> completedNewPersonOrganizationRelationForPerson,
            Func<NewPersonOrganizationRelationExistingPerson, T> newPersonOrganizationRelationExistingPerson,
            Func<NewPersonOrganizationRelationNewPerson, T> newPersonOrganizationRelationNewPerson
        );

        [RequireNamedArgs]
        public abstract T Match<T>(
            Func<CompletedPersonOrganizationRelationForPerson, T> completedPersonOrganizationRelationForPerson,
            Func<IncompletePersonOrganizationRelationForPerson, T> incompletePersonOrganizationRelationForPerson
        );

        public abstract record CompletedPersonOrganizationRelationForPerson : PersonOrganizationRelationForPerson
        {
            public abstract string PersonName { get; }
            public abstract string OrganizationName { get; }
            public override T Match<T>(
                Func<CompletedPersonOrganizationRelationForPerson, T> completedPersonOrganizationRelationForPerson,
                Func<IncompletePersonOrganizationRelationForPerson, T> incompletePersonOrganizationRelationForPerson
            )
            {
                return completedPersonOrganizationRelationForPerson(this);
            }

            public sealed record CompletedNewPersonOrganizationRelationNewPerson : CompletedPersonOrganizationRelationForPerson, NewNode
            {
                public override T Match<T>(
                    Func<CompletedNewPersonOrganizationRelationNewPerson, T> completedNewPersonOrganizationRelationNewPerson,
                    Func<ExistingPersonOrganizationRelationForPerson, T> existingPersonOrganizationRelationForPerson,
                    Func<CompletedNewPersonOrganizationRelationForPerson, T> completedNewPersonOrganizationRelationForPerson,
                    Func<NewPersonOrganizationRelationExistingPerson, T> newPersonOrganizationRelationExistingPerson,
                    Func<NewPersonOrganizationRelationNewPerson, T> newPersonOrganizationRelationNewPerson
                )
                {
                    return completedNewPersonOrganizationRelationNewPerson(this);
                }
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

            public abstract record ResolvedPersonOrganizationRelationForPerson : CompletedPersonOrganizationRelationForPerson
            {

                public sealed record ExistingPersonOrganizationRelationForPerson : ResolvedPersonOrganizationRelationForPerson, ExistingPersonOrganizationRelation
                {
                    public override T Match<T>(
                        Func<CompletedNewPersonOrganizationRelationNewPerson, T> completedNewPersonOrganizationRelationNewPerson,
                        Func<ExistingPersonOrganizationRelationForPerson, T> existingPersonOrganizationRelationForPerson,
                        Func<CompletedNewPersonOrganizationRelationForPerson, T> completedNewPersonOrganizationRelationForPerson,
                        Func<NewPersonOrganizationRelationExistingPerson, T> newPersonOrganizationRelationExistingPerson,
                        Func<NewPersonOrganizationRelationNewPerson, T> newPersonOrganizationRelationNewPerson
                    )
                    {
                        return existingPersonOrganizationRelationForPerson(this);
                    }
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

                public sealed record CompletedNewPersonOrganizationRelationForPerson : ResolvedPersonOrganizationRelationForPerson, CompletedNewPersonOrganizationRelation
                {
                    public override T Match<T>(
                        Func<CompletedNewPersonOrganizationRelationNewPerson, T> completedNewPersonOrganizationRelationNewPerson,
                        Func<ExistingPersonOrganizationRelationForPerson, T> existingPersonOrganizationRelationForPerson,
                        Func<CompletedNewPersonOrganizationRelationForPerson, T> completedNewPersonOrganizationRelationForPerson,
                        Func<NewPersonOrganizationRelationExistingPerson, T> newPersonOrganizationRelationExistingPerson,
                        Func<NewPersonOrganizationRelationNewPerson, T> newPersonOrganizationRelationNewPerson
                    )
                    {
                        return completedNewPersonOrganizationRelationForPerson(this);
                    }
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
        public abstract record IncompletePersonOrganizationRelationForPerson : PersonOrganizationRelationForPerson, NewNode
        {
            public override T Match<T>(
                Func<CompletedPersonOrganizationRelationForPerson, T> completedPersonOrganizationRelationForPerson,
                Func<IncompletePersonOrganizationRelationForPerson, T> incompletePersonOrganizationRelationForPerson
            )
            {
                return incompletePersonOrganizationRelationForPerson(this);
            }

            public abstract CompletedPersonOrganizationRelationForPerson GetCompletedRelation(OrganizationListItem organizationListItem);

            public sealed record NewPersonOrganizationRelationExistingPerson : IncompletePersonOrganizationRelationForPerson
            {
                public override T Match<T>(
                    Func<CompletedNewPersonOrganizationRelationNewPerson, T> completedNewPersonOrganizationRelationNewPerson,
                    Func<ExistingPersonOrganizationRelationForPerson, T> existingPersonOrganizationRelationForPerson,
                    Func<CompletedNewPersonOrganizationRelationForPerson, T> completedNewPersonOrganizationRelationForPerson,
                    Func<NewPersonOrganizationRelationExistingPerson, T> newPersonOrganizationRelationExistingPerson,
                    Func<NewPersonOrganizationRelationNewPerson, T> newPersonOrganizationRelationNewPerson
                )
                {
                    return newPersonOrganizationRelationExistingPerson(this);
                }
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

                public override CompletedPersonOrganizationRelationForPerson GetCompletedRelation(OrganizationListItem organizationListItem)
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

            public sealed record NewPersonOrganizationRelationNewPerson : IncompletePersonOrganizationRelationForPerson
            {
                public override T Match<T>(
                    Func<CompletedNewPersonOrganizationRelationNewPerson, T> completedNewPersonOrganizationRelationNewPerson,
                    Func<ExistingPersonOrganizationRelationForPerson, T> existingPersonOrganizationRelationForPerson,
                    Func<CompletedNewPersonOrganizationRelationForPerson, T> completedNewPersonOrganizationRelationForPerson,
                    Func<NewPersonOrganizationRelationExistingPerson, T> newPersonOrganizationRelationExistingPerson,
                    Func<NewPersonOrganizationRelationNewPerson, T> newPersonOrganizationRelationNewPerson
                )
                {
                    return newPersonOrganizationRelationNewPerson(this);
                }
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
                public override CompletedPersonOrganizationRelationForPerson GetCompletedRelation(OrganizationListItem organizationListItem)
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
        }
    }
}

