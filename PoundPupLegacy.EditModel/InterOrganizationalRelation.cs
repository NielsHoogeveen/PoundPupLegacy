namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(ExistingInterOrganizationalRelationFrom))]
public partial class ExistingInterOrganizationalRelationFromJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(ExistingInterOrganizationalRelationTo))]
public partial class ExistingInterOrganizationalRelationToJsonContext : JsonSerializerContext { }
public static class InterOrganizationalRelationExtentions
{
    public static NewInterOrganizationalExistingFromRelation GetNewInterOrganizationalRelationFrom(this OrganizationListItem organizationListItem, InterOrganizationalRelationTypeListItem interOrganizationalRelationType, int ownerId, int publisherId)
    {
        return new NewInterOrganizationalExistingFromRelation {
            OrganizationFrom = organizationListItem,
            OrganizationTo = null,
            InterOrganizationalRelationType = interOrganizationalRelationType,
            Title = "",
            DateFrom = null,
            DateTo = null,
            Description = "",
            Files = new List<File>(),
            NodeTypeName = "inter organizational relation",
            OwnerId = ownerId,
            PublisherId = publisherId,
            GeographicalEntity = null,
            ProofDocument = null,
            Tags = new List<Tags>(),
            TenantNodes = new List<TenantNode>(),
            Tenants = new List<Tenant>(),
        };
    }
    public static NewInterOrganizationalNewFromRelation GetNewInterOrganizationalRelationFrom(this OrganizationName organizationName, InterOrganizationalRelationTypeListItem interOrganizationalRelationType, int ownerId, int publisherId)
    {
        return new NewInterOrganizationalNewFromRelation {
            OrganizationFrom = organizationName,
            OrganizationTo = null,
            InterOrganizationalRelationType = interOrganizationalRelationType,
            Title = "",
            DateFrom = null,
            DateTo = null,
            Description = "",
            Files = new List<File>(),
            NodeTypeName = "inter organizational relation",
            OwnerId = ownerId,
            PublisherId = publisherId,
            GeographicalEntity = null,
            ProofDocument = null,
            Tags = new List<Tags>(),
            TenantNodes = new List<TenantNode>(),
            Tenants = new List<Tenant>(),
        };
    }
    public static NewInterOrganizationalExistingToRelation GetNewInterOrganizationalRelationTo(this OrganizationListItem organizationListItem, InterOrganizationalRelationTypeListItem interOrganizationalRelationType, int ownerId, int publisherId)
    {
        return new NewInterOrganizationalExistingToRelation {
            OrganizationFrom = null,
            OrganizationTo = organizationListItem,
            InterOrganizationalRelationType = interOrganizationalRelationType,
            Title = "",
            DateFrom = null,
            DateTo = null,
            Description = "",
            Files = new List<File>(),
            NodeTypeName = "inter organizational relation",
            OwnerId = ownerId,
            PublisherId = publisherId,
            GeographicalEntity = null,
            ProofDocument = null,
            Tags = new List<Tags>(),
            TenantNodes = new List<TenantNode>(),
            Tenants = new List<Tenant>(),
        };

    }
    public static NewInterOrganizationalNewToRelation GetNewInterOrganizationalRelationTo(this OrganizationName organizationName, InterOrganizationalRelationTypeListItem interOrganizationalRelationType, int ownerId, int publisherId)
    {
        return new NewInterOrganizationalNewToRelation {
            OrganizationFrom = null,
            OrganizationTo = organizationName,
            InterOrganizationalRelationType = interOrganizationalRelationType,
            Title = "",
            DateFrom = null,
            DateTo = null,
            Description = "",
            Files = new List<File>(),
            NodeTypeName = "inter organizational relation",
            OwnerId = ownerId,
            PublisherId = publisherId,
            GeographicalEntity = null,
            ProofDocument = null,
            Tags = new List<Tags>(),
            TenantNodes = new List<TenantNode>(),
            Tenants = new List<Tenant>(),
        };
    }
}
public abstract record InterOrganizationalRelation : RelationBase
{
    private InterOrganizationalRelation()
    {
    }
    public abstract void SetName(string name);

    public required InterOrganizationalRelationTypeListItem InterOrganizationalRelationType { get; set; }
    public decimal? MoneyInvolved { get; set; }
    public int? NumberOfChildrenInvolved { get; set; }
    public required GeographicalEntityListItem? GeographicalEntity { get; set; }
    public abstract string OrganizationFromName { get; }
    public abstract string OrganizationToName { get; }
    public abstract RelationSide RelationSideThisOrganization { get; }
    public abstract record InterOrganizationalRelationFrom : InterOrganizationalRelation
    {
        private InterOrganizationalRelationFrom() { }
        public sealed override RelationSide RelationSideThisOrganization => RelationSide.From;
        public abstract InterOrganizationalRelationTo SwapFromAndTo();
        public abstract OrganizationItem? OrganizationItemFrom { get; }
        public abstract OrganizationListItem? OrganizationItemTo { get; set; }

        public abstract T Match<T>(
            Func<IncompleteInterOrganizationalRelationFrom, T> incompleteInterOrganizationalRelationFrom,
            Func<CompletedInterOrganizationalRelationFrom, T> completedInterOrganizationalRelationFrom
            );
        public abstract record IncompleteInterOrganizationalRelationFrom : InterOrganizationalRelationFrom
        {
            private IncompleteInterOrganizationalRelationFrom() { }
            public sealed override T Match<T>(
                Func<IncompleteInterOrganizationalRelationFrom, T> incompleteInterOrganizationalRelationFrom,
                Func<CompletedInterOrganizationalRelationFrom, T> completedInterOrganizationalRelationFrom
            )
            {
                return incompleteInterOrganizationalRelationFrom(this);
            }
            public abstract CompletedInterOrganizationalRelationFrom GetCompletedRelation(OrganizationListItem organizationListItemFrom);
            public abstract record NewIncompleteInterOrganizationalRelationFrom : IncompleteInterOrganizationalRelationFrom
            {
                public required OrganizationListItem? OrganizationTo { get; set; }

                private OrganizationListItem? organizationItemTo = null;

                public sealed override OrganizationListItem? OrganizationItemTo {
                    get {
                        if (organizationItemTo is null) {
                            organizationItemTo = OrganizationTo;
                        }
                        return organizationItemTo;
                    }
                    set {
                        organizationItemTo = value;
                    }
                }
                public sealed override string OrganizationToName => OrganizationTo is null ? "" : OrganizationTo.Name;

                public sealed record NewInterOrganizationalNewFromRelation : NewIncompleteInterOrganizationalRelationFrom
                {
                    public required OrganizationName OrganizationFrom { get; set; }

                    public sealed override void SetName(string name)
                    {
                        OrganizationFrom.Name = name;
                    }
                    public sealed override string OrganizationFromName => OrganizationFrom.Name;
                    public sealed override OrganizationItem? OrganizationItemFrom => OrganizationFrom;
                    public sealed override NewInterOrganizationalNewToRelation SwapFromAndTo()
                    {
                        return new NewInterOrganizationalNewToRelation {
                            OrganizationFrom = OrganizationTo,
                            OrganizationTo = OrganizationFrom,
                            DateFrom = DateFrom,
                            DateTo = DateTo,
                            Description = Description,
                            GeographicalEntity = GeographicalEntity,
                            InterOrganizationalRelationType = InterOrganizationalRelationType,
                            MoneyInvolved = MoneyInvolved,
                            NumberOfChildrenInvolved = NumberOfChildrenInvolved,
                            ProofDocument = ProofDocument,
                            NodeTypeName = NodeTypeName,
                            Files = Files,
                            HasBeenDeleted = HasBeenDeleted,
                            OwnerId = OwnerId,
                            PublisherId = PublisherId,
                            Tags = Tags,
                            TenantNodes = TenantNodes,
                            Tenants = Tenants,
                            Title = Title
                        };
                    }
                    public sealed override CompletedInterOrganizationalRelationFrom GetCompletedRelation(OrganizationListItem organizationListItemTo)
                    {
                        return new CompletedNewInterOrganizationalNewFromRelation {
                            OrganizationFrom = OrganizationFrom,
                            OrganizationTo = organizationListItemTo,
                            DateFrom = DateFrom,
                            DateTo = DateTo,
                            Description = Description,
                            GeographicalEntity = GeographicalEntity,
                            InterOrganizationalRelationType = InterOrganizationalRelationType,
                            MoneyInvolved = MoneyInvolved,
                            NumberOfChildrenInvolved = NumberOfChildrenInvolved,
                            ProofDocument = ProofDocument,
                            NodeTypeName = NodeTypeName,
                            Files = Files,
                            HasBeenDeleted = HasBeenDeleted,
                            OwnerId = OwnerId,
                            PublisherId = PublisherId,
                            Tags = Tags,
                            TenantNodes = TenantNodes,
                            Tenants = Tenants,
                            Title = Title,
                        };
                    }
                }

                public sealed record NewInterOrganizationalExistingFromRelation : NewIncompleteInterOrganizationalRelationFrom
                {
                    public required OrganizationListItem OrganizationFrom { get; set; }
                    public sealed override void SetName(string name)
                    {
                        OrganizationFrom.Name = name;
                    }
                    public sealed override string OrganizationFromName => OrganizationFrom.Name;
                    public sealed override OrganizationItem? OrganizationItemFrom => OrganizationFrom;
                    public sealed override NewInterOrganizationalExistingToRelation SwapFromAndTo()
                    {
                        return new NewInterOrganizationalExistingToRelation {
                            OrganizationFrom = OrganizationTo,
                            OrganizationTo = OrganizationFrom,
                            DateFrom = DateFrom,
                            DateTo = DateTo,
                            Description = Description,
                            GeographicalEntity = GeographicalEntity,
                            InterOrganizationalRelationType = InterOrganizationalRelationType,
                            MoneyInvolved = MoneyInvolved,
                            NumberOfChildrenInvolved = NumberOfChildrenInvolved,
                            ProofDocument = ProofDocument,
                            NodeTypeName = NodeTypeName,
                            Files = Files,
                            HasBeenDeleted = HasBeenDeleted,
                            OwnerId = OwnerId,
                            PublisherId = PublisherId,
                            Tags = Tags,
                            TenantNodes = TenantNodes,
                            Tenants = Tenants,
                            Title = Title
                        };
                    }

                    public sealed override CompletedInterOrganizationalRelationFrom GetCompletedRelation(OrganizationListItem organizationListItemTo)
                    {
                        return new NewInterOrganizationalExistingRelationFrom {
                            OrganizationFrom = OrganizationFrom,
                            OrganizationTo = organizationListItemTo,
                            DateFrom = DateFrom,
                            DateTo = DateTo,
                            Description = Description,
                            GeographicalEntity = GeographicalEntity,
                            InterOrganizationalRelationType = InterOrganizationalRelationType,
                            MoneyInvolved = MoneyInvolved,
                            NumberOfChildrenInvolved = NumberOfChildrenInvolved,
                            ProofDocument = ProofDocument,
                            NodeTypeName = NodeTypeName,
                            Files = Files,
                            HasBeenDeleted = HasBeenDeleted,
                            OwnerId = OwnerId,
                            PublisherId = PublisherId,
                            Tags = Tags,
                            TenantNodes = TenantNodes,
                            Tenants = Tenants,
                            Title = Title,
                        };
                    }
                }
            }
        }

        public abstract record CompletedInterOrganizationalRelationFrom : InterOrganizationalRelationFrom
        {
            public required OrganizationListItem OrganizationTo { get; set; }

            private CompletedInterOrganizationalRelationFrom() { }
            public sealed override T Match<T>(
                Func<IncompleteInterOrganizationalRelationFrom, T> incompleteInterOrganizationalRelationFrom,
                Func<CompletedInterOrganizationalRelationFrom, T> completedInterOrganizationalRelationFrom
            )
            {
                return completedInterOrganizationalRelationFrom(this);
            }

            private OrganizationListItem? organizationItemTo = null;

            public sealed override OrganizationListItem? OrganizationItemTo {
                get {
                    organizationItemTo ??= OrganizationTo;
                    return organizationItemTo;
                }
                set {
                    organizationItemTo = value;
                }
            }

            public sealed record CompletedNewInterOrganizationalNewFromRelation : CompletedInterOrganizationalRelationFrom
            {
                public required OrganizationName OrganizationFrom { get; set; }
                
                public sealed override void SetName(string name)
                {
                    OrganizationFrom.Name = name;
                }
                public sealed override string OrganizationFromName => OrganizationFrom.Name;
                public sealed override string OrganizationToName => OrganizationTo.Name;
                public sealed override OrganizationItem? OrganizationItemFrom => OrganizationFrom;
                public sealed override CompletedNewInterOrganizationalNewToRelation SwapFromAndTo()
                {
                    return new CompletedNewInterOrganizationalNewToRelation {
                        OrganizationFrom = OrganizationTo,
                        OrganizationTo = OrganizationFrom,
                        DateFrom = DateFrom,
                        DateTo = DateTo,
                        Description = Description,
                        GeographicalEntity = GeographicalEntity,
                        InterOrganizationalRelationType = InterOrganizationalRelationType,
                        MoneyInvolved = MoneyInvolved,
                        NumberOfChildrenInvolved = NumberOfChildrenInvolved,
                        ProofDocument = ProofDocument,
                        NodeTypeName = NodeTypeName,
                        Files = Files,
                        HasBeenDeleted = HasBeenDeleted,
                        OwnerId = OwnerId,
                        PublisherId = PublisherId,
                        Tags = Tags,
                        TenantNodes = TenantNodes,
                        Tenants = Tenants,
                        Title = Title
                    };
                }
            }

            public abstract record ResolvedInterOrganizationalRelationFrom : CompletedInterOrganizationalRelationFrom
            {
                private ResolvedInterOrganizationalRelationFrom() { }
                public sealed override void SetName(string name)
                {
                    OrganizationFrom.Name = name;
                }

                public required OrganizationListItem OrganizationFrom { get; set; }
                public sealed override string OrganizationFromName => OrganizationFrom.Name;
                public sealed override string OrganizationToName => OrganizationTo.Name;
                public sealed override OrganizationItem? OrganizationItemFrom => OrganizationFrom;

                public sealed record ExistingInterOrganizationalRelationFrom : ResolvedInterOrganizationalRelationFrom, ExistingNode
                {
                    public int NodeId { get; init; }
                    public int UrlId { get; set; }
                    public sealed override ExistingInterOrganizationalRelationTo SwapFromAndTo()
                    {
                        return new ExistingInterOrganizationalRelationTo {
                            OrganizationFrom = OrganizationTo,
                            OrganizationTo = OrganizationFrom,
                            DateFrom = DateFrom,
                            DateTo = DateTo,
                            Description = Description,
                            GeographicalEntity = GeographicalEntity,
                            InterOrganizationalRelationType = InterOrganizationalRelationType,
                            MoneyInvolved = MoneyInvolved,
                            NumberOfChildrenInvolved = NumberOfChildrenInvolved,
                            ProofDocument = ProofDocument,
                            NodeTypeName = NodeTypeName,
                            Files = Files,
                            HasBeenDeleted = HasBeenDeleted,
                            OwnerId = OwnerId,
                            PublisherId = PublisherId,
                            Tags = Tags,
                            TenantNodes = TenantNodes,
                            Tenants = Tenants,
                            Title = Title
                        };
                    }
                }

                public sealed record NewInterOrganizationalExistingRelationFrom : ResolvedInterOrganizationalRelationFrom
                {

                    public sealed override NewInterOrganizationalExistingRelationTo SwapFromAndTo()
                    {
                        return new NewInterOrganizationalExistingRelationTo {
                            OrganizationFrom = OrganizationTo,
                            OrganizationTo = OrganizationFrom,
                            DateFrom = DateFrom,
                            DateTo = DateTo,
                            Description = Description,
                            GeographicalEntity = GeographicalEntity,
                            InterOrganizationalRelationType = InterOrganizationalRelationType,
                            MoneyInvolved = MoneyInvolved,
                            NumberOfChildrenInvolved = NumberOfChildrenInvolved,
                            ProofDocument = ProofDocument,
                            NodeTypeName = NodeTypeName,
                            Files = Files,
                            HasBeenDeleted = HasBeenDeleted,
                            OwnerId = OwnerId,
                            PublisherId = PublisherId,
                            Tags = Tags,
                            TenantNodes = TenantNodes,
                            Tenants = Tenants,
                            Title = Title
                        };
                    }
                }
            }
        }
    }
    public abstract record InterOrganizationalRelationTo : InterOrganizationalRelation
    {
        public abstract OrganizationListItem? OrganizationItemFrom { get; set; }
        public abstract OrganizationItem? OrganizationItemTo { get; }

        private InterOrganizationalRelationTo() { }
        public abstract T Match<T>(
            Func<IncompleteInterOrganizationalRelationTo, T> incompleteInterOrganizationalRelationTo,
            Func<CompletedInterOrganizationalRelationTo, T> completedInterOrganizationalRelationTo
        );

        public abstract InterOrganizationalRelationFrom SwapFromAndTo();
        public sealed override RelationSide RelationSideThisOrganization => RelationSide.To;
        public abstract record IncompleteInterOrganizationalRelationTo : InterOrganizationalRelationTo
        {
            private IncompleteInterOrganizationalRelationTo() { }

            public sealed override T Match<T>(
                Func<IncompleteInterOrganizationalRelationTo, T> incompleteInterOrganizationalRelationTo,
                Func<CompletedInterOrganizationalRelationTo, T> completedInterOrganizationalRelationTo
            )
            {
                return incompleteInterOrganizationalRelationTo(this);
            }
            public abstract CompletedInterOrganizationalRelationTo GetCompletedRelation(OrganizationListItem organizationListItemFrom);

            public abstract record NewIncompleteInterOrganizationalRelationTo : IncompleteInterOrganizationalRelationTo
            {
                public required OrganizationListItem? OrganizationFrom { get; set; }
                public sealed override string OrganizationFromName => OrganizationFrom is null ? "" : OrganizationFrom.Name;
                private OrganizationListItem? organizationItemFrom = null;
                public sealed override OrganizationListItem? OrganizationItemFrom {
                    get {
                        if (organizationItemFrom is null) {
                            organizationItemFrom = OrganizationFrom;
                        }
                        return organizationItemFrom;
                    }
                    set {
                        organizationItemFrom = value;
                    }
                }
                public sealed record NewInterOrganizationalExistingToRelation : NewIncompleteInterOrganizationalRelationTo
                {
                    public required OrganizationListItem OrganizationTo { get; set; }
                    public sealed override void SetName(string name)
                    {
                        OrganizationTo.Name = name;
                    }

                    public sealed override string OrganizationToName => OrganizationTo.Name;
                    public sealed override OrganizationItem? OrganizationItemTo => OrganizationTo;
                    public sealed override NewInterOrganizationalExistingFromRelation SwapFromAndTo()
                    {
                        return new NewInterOrganizationalExistingFromRelation {
                            OrganizationFrom = OrganizationTo,
                            OrganizationTo = OrganizationFrom,
                            DateFrom = DateFrom,
                            DateTo = DateTo,
                            Description = Description,
                            GeographicalEntity = GeographicalEntity,
                            InterOrganizationalRelationType = InterOrganizationalRelationType,
                            MoneyInvolved = MoneyInvolved,
                            NumberOfChildrenInvolved = NumberOfChildrenInvolved,
                            ProofDocument = ProofDocument,
                            NodeTypeName = NodeTypeName,
                            Files = Files,
                            HasBeenDeleted = HasBeenDeleted,
                            OwnerId = OwnerId,
                            PublisherId = PublisherId,
                            Tags = Tags,
                            TenantNodes = TenantNodes,
                            Tenants = Tenants,
                            Title = Title
                        };
                    }

                    public sealed override CompletedInterOrganizationalRelationTo GetCompletedRelation(OrganizationListItem organizationListItemFrom)
                    {
                        return new NewInterOrganizationalExistingRelationTo {
                            OrganizationFrom = organizationListItemFrom,
                            OrganizationTo = OrganizationTo,
                            DateFrom = DateFrom,
                            DateTo = DateTo,
                            Description = Description,
                            GeographicalEntity = GeographicalEntity,
                            InterOrganizationalRelationType = InterOrganizationalRelationType,
                            MoneyInvolved = MoneyInvolved,
                            NumberOfChildrenInvolved = NumberOfChildrenInvolved,
                            ProofDocument = ProofDocument,
                            NodeTypeName = NodeTypeName,
                            Files = Files,
                            HasBeenDeleted = HasBeenDeleted,
                            OwnerId = OwnerId,
                            PublisherId = PublisherId,
                            Tags = Tags,
                            TenantNodes = TenantNodes,
                            Tenants = Tenants,
                            Title = Title,
                        };
                    }
                }

                public sealed record NewInterOrganizationalNewToRelation : NewIncompleteInterOrganizationalRelationTo
                {
                    public required OrganizationName OrganizationTo { get; set; }
                    public sealed override void SetName(string name)
                    {
                        OrganizationTo.Name = name;
                    }
                    public sealed override string OrganizationToName => OrganizationTo.Name;
                    public sealed override OrganizationItem? OrganizationItemTo => OrganizationTo;

                    public sealed override NewInterOrganizationalNewFromRelation SwapFromAndTo()
                    {
                        return new NewInterOrganizationalNewFromRelation {
                            OrganizationFrom = OrganizationTo,
                            OrganizationTo = OrganizationFrom,
                            DateFrom = DateFrom,
                            DateTo = DateTo,
                            Description = Description,
                            GeographicalEntity = GeographicalEntity,
                            InterOrganizationalRelationType = InterOrganizationalRelationType,
                            MoneyInvolved = MoneyInvolved,
                            NumberOfChildrenInvolved = NumberOfChildrenInvolved,
                            ProofDocument = ProofDocument,
                            NodeTypeName = NodeTypeName,
                            Files = Files,
                            HasBeenDeleted = HasBeenDeleted,
                            OwnerId = OwnerId,
                            PublisherId = PublisherId,
                            Tags = Tags,
                            TenantNodes = TenantNodes,
                            Tenants = Tenants,
                            Title = Title
                        };
                    }
                    public sealed override CompletedInterOrganizationalRelationTo GetCompletedRelation(OrganizationListItem organizationListItemFrom)
                    {
                        return new CompletedNewInterOrganizationalNewToRelation {
                            OrganizationFrom = organizationListItemFrom,
                            OrganizationTo = OrganizationTo,
                            DateFrom = DateFrom,
                            DateTo = DateTo,
                            Description = Description,
                            GeographicalEntity = GeographicalEntity,
                            InterOrganizationalRelationType = InterOrganizationalRelationType,
                            MoneyInvolved = MoneyInvolved,
                            NumberOfChildrenInvolved = NumberOfChildrenInvolved,
                            ProofDocument = ProofDocument,
                            NodeTypeName = NodeTypeName,
                            Files = Files,
                            HasBeenDeleted = HasBeenDeleted,
                            OwnerId = OwnerId,
                            PublisherId = PublisherId,
                            Tags = Tags,
                            TenantNodes = TenantNodes,
                            Tenants = Tenants,
                            Title = Title,
                        };
                    }
                }
            }
        }

        public abstract record CompletedInterOrganizationalRelationTo : InterOrganizationalRelationTo
        {
            private CompletedInterOrganizationalRelationTo() { }
            public required OrganizationListItem OrganizationFrom { get; set; }

            private OrganizationListItem? organizationItemFrom = null;
            public sealed override OrganizationListItem? OrganizationItemFrom {
                get {
                    organizationItemFrom ??= OrganizationFrom;
                    return organizationItemFrom;
                }
                set {
                    organizationItemFrom = value;
                }
            }
            public sealed override string OrganizationFromName => OrganizationFrom.Name;

            public sealed override T Match<T>(
                Func<IncompleteInterOrganizationalRelationTo, T> incompleteInterOrganizationalRelationTo,
                Func<CompletedInterOrganizationalRelationTo, T> completedInterOrganizationalRelationTo
            )
            {
                return completedInterOrganizationalRelationTo(this);
            }
            public sealed record CompletedNewInterOrganizationalNewToRelation : CompletedInterOrganizationalRelationTo
            {
                public required OrganizationName OrganizationTo { get; set; }

                public sealed override void SetName(string name)
                {
                    OrganizationTo.Name = name;
                }
                public sealed override string OrganizationToName => OrganizationTo.Name;

                public sealed override OrganizationItem? OrganizationItemTo => OrganizationTo;
                public sealed override CompletedNewInterOrganizationalNewFromRelation SwapFromAndTo()
                {
                    return new CompletedNewInterOrganizationalNewFromRelation {
                        OrganizationFrom = OrganizationTo,
                        OrganizationTo = OrganizationFrom,
                        DateFrom = DateFrom,
                        DateTo = DateTo,
                        Description = Description,
                        GeographicalEntity = GeographicalEntity,
                        InterOrganizationalRelationType = InterOrganizationalRelationType,
                        MoneyInvolved = MoneyInvolved,
                        NumberOfChildrenInvolved = NumberOfChildrenInvolved,
                        ProofDocument = ProofDocument,
                        NodeTypeName = NodeTypeName,
                        Files = Files,
                        HasBeenDeleted = HasBeenDeleted,
                        OwnerId = OwnerId,
                        PublisherId = PublisherId,
                        Tags = Tags,
                        TenantNodes = TenantNodes,
                        Tenants = Tenants,
                        Title = Title
                    };
                }
            }

            public abstract record ResolvedInterOrganizationalRelationTo : CompletedInterOrganizationalRelationTo
            {
                private ResolvedInterOrganizationalRelationTo() { }
                public required OrganizationListItem OrganizationTo { get; set; }

                public sealed override void SetName(string name)
                {
                    OrganizationTo.Name = name;
                }

                public sealed override string OrganizationToName => OrganizationTo.Name;

                public sealed override OrganizationItem? OrganizationItemTo => OrganizationTo;

                public sealed record ExistingInterOrganizationalRelationTo : ResolvedInterOrganizationalRelationTo, ExistingNode
                {
                    public int NodeId { get; init; }
                    public int UrlId { get; set; }
                    public sealed override ExistingInterOrganizationalRelationFrom SwapFromAndTo()
                    {
                        return new ExistingInterOrganizationalRelationFrom {
                            OrganizationFrom = OrganizationTo,
                            OrganizationTo = OrganizationFrom,
                            DateFrom = DateFrom,
                            DateTo = DateTo,
                            Description = Description,
                            GeographicalEntity = GeographicalEntity,
                            InterOrganizationalRelationType = InterOrganizationalRelationType,
                            MoneyInvolved = MoneyInvolved,
                            NumberOfChildrenInvolved = NumberOfChildrenInvolved,
                            ProofDocument = ProofDocument,
                            NodeTypeName = NodeTypeName,
                            Files = Files,
                            HasBeenDeleted = HasBeenDeleted,
                            OwnerId = OwnerId,
                            PublisherId = PublisherId,
                            Tags = Tags,
                            TenantNodes = TenantNodes,
                            Tenants = Tenants,
                            Title = Title
                        };
                    }
                }

                public sealed record NewInterOrganizationalExistingRelationTo : ResolvedInterOrganizationalRelationTo
                {

                    public sealed override NewInterOrganizationalExistingRelationFrom SwapFromAndTo()
                    {
                        return new NewInterOrganizationalExistingRelationFrom {
                            OrganizationFrom = OrganizationTo,
                            OrganizationTo = OrganizationFrom,
                            DateFrom = DateFrom,
                            DateTo = DateTo,
                            Description = Description,
                            GeographicalEntity = GeographicalEntity,
                            InterOrganizationalRelationType = InterOrganizationalRelationType,
                            MoneyInvolved = MoneyInvolved,
                            NumberOfChildrenInvolved = NumberOfChildrenInvolved,
                            ProofDocument = ProofDocument,
                            NodeTypeName = NodeTypeName,
                            Files = Files,
                            HasBeenDeleted = HasBeenDeleted,
                            OwnerId = OwnerId,
                            PublisherId = PublisherId,
                            Tags = Tags,
                            TenantNodes = TenantNodes,
                            Tenants = Tenants,
                            Title = Title
                        };
                    }
                }
            }

        }
    }
}