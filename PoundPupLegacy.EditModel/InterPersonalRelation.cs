namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(ExistingInterPersonalRelationFrom))]
public partial class ExistingInterPersonalRelationFromJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(ExistingInterPersonalRelationTo))]
public partial class ExistingInterPersonalRelationToJsonContext : JsonSerializerContext { }

public static class InterPersonalRelationExtentions{
    public static NewInterPersonalExistingFromRelation GetNewInterPersonalRelationFrom(this PersonListItem personListItem, InterPersonalRelationTypeListItem interPersonalRelationType, int ownerId, int publisherId)
    {
        return new NewInterPersonalExistingFromRelation {
            PersonFrom = personListItem,
            PersonTo = null,
            InterPersonalRelationType = interPersonalRelationType,
            Title = "",
            DateFrom = null,
            DateTo = null,
            Description = "",
            Files = new List<File>(),
            NodeTypeName = "inter personal relation",
            OwnerId = ownerId,
            PublisherId = publisherId,
            ProofDocument = null,
            Tags = new List<Tags>(),
            TenantNodesToAdd = new List<TenantNode.NewTenantNodeForNewNode>(),
            Tenants = new List<Tenant>(),
        };
    }
    public static NewInterPersonalExistingToRelation GetNewInterPersonalRelationTo(this PersonListItem personListItem, InterPersonalRelationTypeListItem interPersonalRelationType, int ownerId, int publisherId)
    {
        return new NewInterPersonalExistingToRelation {
            PersonFrom = null,
            PersonTo = personListItem,
            InterPersonalRelationType = interPersonalRelationType,
            Title = "",
            DateFrom = null,
            DateTo = null,
            Description = "",
            Files = new List<File>(),
            NodeTypeName = "inter personal relation",
            OwnerId = ownerId,
            PublisherId = publisherId,
            ProofDocument = null,
            Tags = new List<Tags>(),
            TenantNodesToAdd = new List<TenantNode.NewTenantNodeForNewNode>(),
            Tenants = new List<Tenant>(),
        };

    }
    public static NewInterPersonalNewFromRelation GetNewInterPersonalRelationFrom(this PersonName personName, InterPersonalRelationTypeListItem interPersonalRelationType, int ownerId, int publisherId)
    {
        return new NewInterPersonalNewFromRelation {
            PersonFrom = personName,
            PersonTo = null,
            InterPersonalRelationType = interPersonalRelationType,
            Title = "",
            DateFrom = null,
            DateTo = null,
            Description = "",
            Files = new List<File>(),
            NodeTypeName = "inter personal relation",
            OwnerId = ownerId,
            PublisherId = publisherId,
            ProofDocument = null,
            Tags = new List<Tags>(),
            TenantNodesToAdd = new List<TenantNode.NewTenantNodeForNewNode>(),
            Tenants = new List<Tenant>(),
        };
    }
    public static NewInterPersonalNewToRelation GetNewInterPersonalRelationTo(this PersonName personName, InterPersonalRelationTypeListItem interPersonalRelationType, int ownerId, int publisherId)
    {
        return new NewInterPersonalNewToRelation {
            PersonFrom = null,
            PersonTo = personName,
            InterPersonalRelationType = interPersonalRelationType,
            Title = "",
            DateFrom = null,
            DateTo = null,
            Description = "",
            Files = new List<File>(),
            NodeTypeName = "inter personal relation",
            OwnerId = ownerId,
            PublisherId = publisherId,
            ProofDocument = null,
            Tags = new List<Tags>(),
            TenantNodesToAdd = new List<TenantNode.NewTenantNodeForNewNode>(),
            Tenants = new List<Tenant>(),
        };
    }
}


public abstract record InterPersonalRelation : RelationBase
{
    private InterPersonalRelation()
    {
    }
    public abstract void SetName(string name);

    public required InterPersonalRelationTypeListItem InterPersonalRelationType { get; set; }
    public abstract string PersonFromName { get; }
    public abstract string PersonToName { get; }
    public abstract RelationSide RelationSideThisPerson { get; }
    public abstract record InterPersonalRelationFrom : InterPersonalRelation
    {
        private InterPersonalRelationFrom() { }
        public sealed override RelationSide RelationSideThisPerson => RelationSide.From;
        public abstract InterPersonalRelationTo SwapFromAndTo();
        public abstract PersonItem? PersonItemFrom { get; }
        public abstract PersonListItem? PersonItemTo { get; set; }

        public abstract T Match<T>(
            Func<IncompleteInterPersonalRelationFrom, T> incompleteInterPersonalRelationFrom,
            Func<CompletedInterPersonalRelationFrom, T> completedInterPersonalRelationFrom
            );
        public abstract record IncompleteInterPersonalRelationFrom : InterPersonalRelationFrom
        {
            private IncompleteInterPersonalRelationFrom() { }
            public sealed override T Match<T>(
                Func<IncompleteInterPersonalRelationFrom, T> incompleteInterPersonalRelationFrom,
                Func<CompletedInterPersonalRelationFrom, T> completedInterPersonalRelationFrom
            )
            {
                return incompleteInterPersonalRelationFrom(this);
            }
            public abstract CompletedInterPersonalRelationFrom GetCompletedRelation(PersonListItem organizationListItemFrom);
            public abstract record NewIncompleteInterPersonalRelationFrom : IncompleteInterPersonalRelationFrom
            {
                public required PersonListItem? PersonTo { get; set; }

                private PersonListItem? organizationItemTo = null;

                public sealed override PersonListItem? PersonItemTo {
                    get {
                        if (organizationItemTo is null) {
                            organizationItemTo = PersonTo;
                        }
                        return organizationItemTo;
                    }
                    set {
                        organizationItemTo = value;
                    }
                }
                public sealed override string PersonToName => PersonTo is null ? "" : PersonTo.Name;

                public sealed record NewInterPersonalNewFromRelation : NewIncompleteInterPersonalRelationFrom, NewNode
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

                    public required PersonName PersonFrom { get; set; }
                    public sealed override void SetName(string name)
                    {
                        PersonFrom.Name = name;
                    }
                    public sealed override string PersonFromName => PersonFrom.Name;
                    public sealed override PersonItem? PersonItemFrom => PersonFrom;
                    public sealed override NewInterPersonalNewToRelation SwapFromAndTo()
                    {
                        return new NewInterPersonalNewToRelation {
                            PersonFrom = PersonTo,
                            PersonTo = PersonFrom,
                            DateFrom = DateFrom,
                            DateTo = DateTo,
                            Description = Description,
                            InterPersonalRelationType = InterPersonalRelationType,
                            ProofDocument = ProofDocument,
                            NodeTypeName = NodeTypeName,
                            Files = Files,
                            HasBeenDeleted = HasBeenDeleted,
                            OwnerId = OwnerId,
                            PublisherId = PublisherId,
                            Tags = Tags,
                            TenantNodesToAdd = TenantNodesToAdd,
                            Tenants = Tenants,
                            Title = Title
                        };
                    }
                    public sealed override CompletedInterPersonalRelationFrom GetCompletedRelation(PersonListItem organizationListItemTo)
                    {
                        return new CompletedNewInterPersonalNewFromRelation {
                            PersonFrom = PersonFrom,
                            PersonTo = organizationListItemTo,
                            DateFrom = DateFrom,
                            DateTo = DateTo,
                            Description = Description,
                            InterPersonalRelationType = InterPersonalRelationType,
                            ProofDocument = ProofDocument,
                            NodeTypeName = NodeTypeName,
                            Files = Files,
                            HasBeenDeleted = HasBeenDeleted,
                            OwnerId = OwnerId,
                            PublisherId = PublisherId,
                            Tags = Tags,
                            TenantNodesToAdd = TenantNodesToAdd,
                            Tenants = Tenants,
                            Title = Title,
                        };
                    }
                }

                public sealed record NewInterPersonalExistingFromRelation : NewIncompleteInterPersonalRelationFrom, NewNode
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

                    public required PersonListItem PersonFrom { get; set; }
                    public sealed override void SetName(string name)
                    {
                        PersonFrom.Name = name;
                    }
                    public sealed override string PersonFromName => PersonFrom.Name;
                    public sealed override PersonItem? PersonItemFrom => PersonFrom;
                    public sealed override NewInterPersonalExistingToRelation SwapFromAndTo()
                    {
                        return new NewInterPersonalExistingToRelation {
                            PersonFrom = PersonTo,
                            PersonTo = PersonFrom,
                            DateFrom = DateFrom,
                            DateTo = DateTo,
                            Description = Description,
                            InterPersonalRelationType = InterPersonalRelationType,
                            ProofDocument = ProofDocument,
                            NodeTypeName = NodeTypeName,
                            Files = Files,
                            HasBeenDeleted = HasBeenDeleted,
                            OwnerId = OwnerId,
                            PublisherId = PublisherId,
                            Tags = Tags,
                            TenantNodesToAdd = TenantNodesToAdd,
                            Tenants = Tenants,
                            Title = Title
                        };
                    }

                    public sealed override CompletedInterPersonalRelationFrom GetCompletedRelation(PersonListItem organizationListItemTo)
                    {
                        return new NewInterPersonalExistingRelationFrom {
                            PersonFrom = PersonFrom,
                            PersonTo = organizationListItemTo,
                            DateFrom = DateFrom,
                            DateTo = DateTo,
                            Description = Description,
                            InterPersonalRelationType = InterPersonalRelationType,
                            ProofDocument = ProofDocument,
                            NodeTypeName = NodeTypeName,
                            Files = Files,
                            HasBeenDeleted = HasBeenDeleted,
                            OwnerId = OwnerId,
                            PublisherId = PublisherId,
                            Tags = Tags,
                            TenantNodesToAdd = TenantNodesToAdd,
                            Tenants = Tenants,
                            Title = Title,
                        };
                    }
                }
            }
        }

        public abstract record CompletedInterPersonalRelationFrom : InterPersonalRelationFrom
        {
            public required PersonListItem PersonTo { get; set; }

            private CompletedInterPersonalRelationFrom() { }
            public sealed override T Match<T>(
                Func<IncompleteInterPersonalRelationFrom, T> incompleteInterPersonalRelationFrom,
                Func<CompletedInterPersonalRelationFrom, T> completedInterPersonalRelationFrom
            )
            {
                return completedInterPersonalRelationFrom(this);
            }

            private PersonListItem? organizationItemTo = null;

            public sealed override PersonListItem? PersonItemTo {
                get {
                    if (organizationItemTo is null) {
                        organizationItemTo = PersonTo;
                    }
                    return organizationItemTo;
                }
                set {
                    organizationItemTo = value;
                }
            }

            public sealed record CompletedNewInterPersonalNewFromRelation : CompletedInterPersonalRelationFrom, NewNode
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

                public required PersonName PersonFrom { get; set; }

                public sealed override void SetName(string name)
                {
                    PersonFrom.Name = name;
                }
                public sealed override string PersonFromName => PersonFrom.Name;
                public sealed override string PersonToName => PersonTo.Name;
                public sealed override PersonItem? PersonItemFrom => PersonFrom;
                public sealed override CompletedNewInterPersonalNewToRelation SwapFromAndTo()
                {
                    return new CompletedNewInterPersonalNewToRelation {
                        PersonFrom = PersonTo,
                        PersonTo = PersonFrom,
                        DateFrom = DateFrom,
                        DateTo = DateTo,
                        Description = Description,
                        InterPersonalRelationType = InterPersonalRelationType,
                        ProofDocument = ProofDocument,
                        NodeTypeName = NodeTypeName,
                        Files = Files,
                        HasBeenDeleted = HasBeenDeleted,
                        OwnerId = OwnerId,
                        PublisherId = PublisherId,
                        Tags = Tags,
                        TenantNodesToAdd = TenantNodesToAdd,
                        Tenants = Tenants,
                        Title = Title
                    };
                }
            }

            public abstract record ResolvedInterPersonalRelationFrom : CompletedInterPersonalRelationFrom
            {
                private ResolvedInterPersonalRelationFrom() { }
                public sealed override void SetName(string name)
                {
                    PersonFrom.Name = name;
                }

                public required PersonListItem PersonFrom { get; set; }
                public sealed override string PersonFromName => PersonFrom.Name;
                public sealed override string PersonToName => PersonTo.Name;
                public sealed override PersonItem? PersonItemFrom => PersonFrom;

                public sealed record ExistingInterPersonalRelationFrom : ResolvedInterPersonalRelationFrom, ExistingNode
                {
                    public int NodeId { get; init; }
                    public int UrlId { get; set; }

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
                    public sealed override ExistingInterPersonalRelationTo SwapFromAndTo()
                    {
                        return new ExistingInterPersonalRelationTo {
                            PersonFrom = PersonTo,
                            PersonTo = PersonFrom,
                            DateFrom = DateFrom,
                            DateTo = DateTo,
                            Description = Description,
                            InterPersonalRelationType = InterPersonalRelationType,
                            ProofDocument = ProofDocument,
                            NodeTypeName = NodeTypeName,
                            Files = Files,
                            HasBeenDeleted = HasBeenDeleted,
                            OwnerId = OwnerId,
                            PublisherId = PublisherId,
                            Tags = Tags,
                            TenantNodesToAdd = TenantNodesToAdd,
                            Tenants = Tenants,
                            Title = Title
                        };

                    }

                }

                public sealed record NewInterPersonalExistingRelationFrom : ResolvedInterPersonalRelationFrom, NewNode
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

                    public sealed override NewInterPersonalExistingRelationTo SwapFromAndTo()
                    {
                        return new NewInterPersonalExistingRelationTo {
                            PersonFrom = PersonTo,
                            PersonTo = PersonFrom,
                            DateFrom = DateFrom,
                            DateTo = DateTo,
                            Description = Description,
                            InterPersonalRelationType = InterPersonalRelationType,
                            ProofDocument = ProofDocument,
                            NodeTypeName = NodeTypeName,
                            Files = Files,
                            HasBeenDeleted = HasBeenDeleted,
                            OwnerId = OwnerId,
                            PublisherId = PublisherId,
                            Tags = Tags,
                            TenantNodesToAdd = TenantNodesToAdd,
                            Tenants = Tenants,
                            Title = Title
                        };
                    }
                }
            }
        }
    }
    public abstract record InterPersonalRelationTo : InterPersonalRelation
    {
        public abstract PersonListItem? PersonItemFrom { get; set; }
        public abstract PersonItem? PersonItemTo { get; }

        private InterPersonalRelationTo() { }
        public abstract T Match<T>(
            Func<IncompleteInterPersonalRelationTo, T> incompleteInterPersonalRelationTo,
            Func<CompletedInterPersonalRelationTo, T> completedInterPersonalRelationTo
        );

        public abstract InterPersonalRelationFrom SwapFromAndTo();
        public sealed override RelationSide RelationSideThisPerson => RelationSide.To;
        public abstract record IncompleteInterPersonalRelationTo : InterPersonalRelationTo
        {
            private IncompleteInterPersonalRelationTo() { }

            public sealed override T Match<T>(
                Func<IncompleteInterPersonalRelationTo, T> incompleteInterPersonalRelationTo,
                Func<CompletedInterPersonalRelationTo, T> completedInterPersonalRelationTo
            )
            {
                return incompleteInterPersonalRelationTo(this);
            }
            public abstract CompletedInterPersonalRelationTo GetCompletedRelation(PersonListItem organizationListItemFrom);

            public abstract record NewIncompleteInterPersonalRelationTo : IncompleteInterPersonalRelationTo
            {
                public required PersonListItem? PersonFrom { get; set; }
                public sealed override string PersonFromName => PersonFrom is null ? "" : PersonFrom.Name;
                private PersonListItem? organizationItemFrom = null;
                public sealed override PersonListItem? PersonItemFrom {
                    get {
                        if (organizationItemFrom is null) {
                            organizationItemFrom = PersonFrom;
                        }
                        return organizationItemFrom;
                    }
                    set {
                        organizationItemFrom = value;
                    }
                }
                public sealed record NewInterPersonalExistingToRelation : NewIncompleteInterPersonalRelationTo, NewNode
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

                    public required PersonListItem PersonTo { get; set; }
                    public sealed override void SetName(string name)
                    {
                        PersonTo.Name = name;
                    }
                    public sealed override string PersonToName => PersonTo.Name;
                    public sealed override PersonItem? PersonItemTo => PersonTo;
                    public sealed override NewInterPersonalExistingFromRelation SwapFromAndTo()
                    {
                        return new NewInterPersonalExistingFromRelation {
                            PersonFrom = PersonTo,
                            PersonTo = PersonFrom,
                            DateFrom = DateFrom,
                            DateTo = DateTo,
                            Description = Description,
                            InterPersonalRelationType = InterPersonalRelationType,
                            ProofDocument = ProofDocument,
                            NodeTypeName = NodeTypeName,
                            Files = Files,
                            HasBeenDeleted = HasBeenDeleted,
                            OwnerId = OwnerId,
                            PublisherId = PublisherId,
                            Tags = Tags,
                            TenantNodesToAdd = TenantNodesToAdd,
                            Tenants = Tenants,
                            Title = Title
                        };
                    }

                    public sealed override CompletedInterPersonalRelationTo GetCompletedRelation(PersonListItem organizationListItemFrom)
                    {
                        return new NewInterPersonalExistingRelationTo {
                            PersonFrom = organizationListItemFrom,
                            PersonTo = PersonTo,
                            DateFrom = DateFrom,
                            DateTo = DateTo,
                            Description = Description,
                            InterPersonalRelationType = InterPersonalRelationType,
                            ProofDocument = ProofDocument,
                            NodeTypeName = NodeTypeName,
                            Files = Files,
                            HasBeenDeleted = HasBeenDeleted,
                            OwnerId = OwnerId,
                            PublisherId = PublisherId,
                            Tags = Tags,
                            TenantNodesToAdd = TenantNodesToAdd,
                            Tenants = Tenants,
                            Title = Title,
                        };
                    }
                }

                public sealed record NewInterPersonalNewToRelation : NewIncompleteInterPersonalRelationTo, NewNode
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

                    public required PersonName PersonTo { get; set; }
                    public sealed override void SetName(string name)
                    {
                        PersonTo.Name = name;
                    }
                    public sealed override string PersonToName => PersonTo.Name;
                    public sealed override PersonItem? PersonItemTo => PersonTo;

                    public sealed override NewInterPersonalNewFromRelation SwapFromAndTo()
                    {
                        return new NewInterPersonalNewFromRelation {
                            PersonFrom = PersonTo,
                            PersonTo = PersonFrom,
                            DateFrom = DateFrom,
                            DateTo = DateTo,
                            Description = Description,
                            InterPersonalRelationType = InterPersonalRelationType,
                            ProofDocument = ProofDocument,
                            NodeTypeName = NodeTypeName,
                            Files = Files,
                            HasBeenDeleted = HasBeenDeleted,
                            OwnerId = OwnerId,
                            PublisherId = PublisherId,
                            Tags = Tags,
                            TenantNodesToAdd = TenantNodesToAdd,
                            Tenants = Tenants,
                            Title = Title
                        };
                    }
                    public sealed override CompletedInterPersonalRelationTo GetCompletedRelation(PersonListItem organizationListItemFrom)
                    {
                        return new CompletedNewInterPersonalNewToRelation {
                            PersonFrom = organizationListItemFrom,
                            PersonTo = PersonTo,
                            DateFrom = DateFrom,
                            DateTo = DateTo,
                            Description = Description,
                            InterPersonalRelationType = InterPersonalRelationType,
                            ProofDocument = ProofDocument,
                            NodeTypeName = NodeTypeName,
                            Files = Files,
                            HasBeenDeleted = HasBeenDeleted,
                            OwnerId = OwnerId,
                            PublisherId = PublisherId,
                            Tags = Tags,
                            TenantNodesToAdd = TenantNodesToAdd,
                            Tenants = Tenants,
                            Title = Title,
                        };
                    }
                }
            }
        }

        public abstract record CompletedInterPersonalRelationTo : InterPersonalRelationTo
        {
            private CompletedInterPersonalRelationTo() { }
            public required PersonListItem PersonFrom { get; set; }

            private PersonListItem? organizationItemFrom = null;
            public sealed override PersonListItem? PersonItemFrom {
                get {
                    if (organizationItemFrom is null) {
                        organizationItemFrom = PersonFrom;
                    }
                    return organizationItemFrom;
                }
                set {
                    organizationItemFrom = value;
                }
            }
            public sealed override string PersonFromName => PersonFrom.Name;

            public sealed override T Match<T>(
                Func<IncompleteInterPersonalRelationTo, T> incompleteInterPersonalRelationTo,
                Func<CompletedInterPersonalRelationTo, T> completedInterPersonalRelationTo
            )
            {
                return completedInterPersonalRelationTo(this);
            }

            public sealed record CompletedNewInterPersonalNewToRelation : CompletedInterPersonalRelationTo, NewNode
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

                public required PersonName PersonTo { get; set; }

                public sealed override void SetName(string name)
                {
                    PersonTo.Name = name;
                }
                public sealed override string PersonToName => PersonTo.Name;

                public sealed override PersonItem? PersonItemTo => PersonTo;
                public sealed override CompletedNewInterPersonalNewFromRelation SwapFromAndTo()
                {
                    return new CompletedNewInterPersonalNewFromRelation {
                        PersonFrom = PersonTo,
                        PersonTo = PersonFrom,
                        DateFrom = DateFrom,
                        DateTo = DateTo,
                        Description = Description,
                        InterPersonalRelationType = InterPersonalRelationType,
                        ProofDocument = ProofDocument,
                        NodeTypeName = NodeTypeName,
                        Files = Files,
                        HasBeenDeleted = HasBeenDeleted,
                        OwnerId = OwnerId,
                        PublisherId = PublisherId,
                        Tags = Tags,
                        TenantNodesToAdd = TenantNodesToAdd,
                        Tenants = Tenants,
                        Title = Title
                    };
                }
            }

            public abstract record ResolvedInterPersonalRelationTo : CompletedInterPersonalRelationTo
            {
                private ResolvedInterPersonalRelationTo() { }
                public required PersonListItem PersonTo { get; set; }

                public sealed override void SetName(string name)
                {
                    PersonTo.Name = name;
                }

                public sealed override string PersonToName => PersonTo.Name;

                public sealed override PersonItem? PersonItemTo => PersonTo;

                public sealed record ExistingInterPersonalRelationTo : ResolvedInterPersonalRelationTo, ExistingNode
                {
                    public int NodeId { get; init; }
                    public int UrlId { get; set; }
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
                    public sealed override ExistingInterPersonalRelationFrom SwapFromAndTo()
                    {
                        return new ExistingInterPersonalRelationFrom {
                            PersonFrom = PersonTo,
                            PersonTo = PersonFrom,
                            DateFrom = DateFrom,
                            DateTo = DateTo,
                            Description = Description,
                            InterPersonalRelationType = InterPersonalRelationType,
                            ProofDocument = ProofDocument,
                            NodeTypeName = NodeTypeName,
                            Files = Files,
                            HasBeenDeleted = HasBeenDeleted,
                            OwnerId = OwnerId,
                            PublisherId = PublisherId,
                            Tags = Tags,
                            TenantNodesToAdd = TenantNodesToAdd,
                            Tenants = Tenants,
                            Title = Title
                        };
                    }
                }

                public sealed record NewInterPersonalExistingRelationTo : ResolvedInterPersonalRelationTo, NewNode
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


                    public sealed override NewInterPersonalExistingRelationFrom SwapFromAndTo()
                    {
                        return new NewInterPersonalExistingRelationFrom {
                            PersonFrom = PersonTo,
                            PersonTo = PersonFrom,
                            DateFrom = DateFrom,
                            DateTo = DateTo,
                            Description = Description,
                            InterPersonalRelationType = InterPersonalRelationType,
                            ProofDocument = ProofDocument,
                            NodeTypeName = NodeTypeName,
                            Files = Files,
                            HasBeenDeleted = HasBeenDeleted,
                            OwnerId = OwnerId,
                            PublisherId = PublisherId,
                            Tags = Tags,
                            TenantNodesToAdd = TenantNodesToAdd,
                            Tenants = Tenants,
                            Title = Title
                        };
                    }
                }
            }
        }
    }
}