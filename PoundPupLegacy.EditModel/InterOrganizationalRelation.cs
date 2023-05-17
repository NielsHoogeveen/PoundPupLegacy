namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(ExistingInterOrganizationalRelationFrom))]
public partial class ExistingInterOrganizationalRelationJsonContext : JsonSerializerContext { }

public abstract record InterOrganizationalRelation : RelationBase
{
    private InterOrganizationalRelation()
    {
    }

    public required InterOrganizationalRelationTypeListItem InterOrganizationalRelationType { get; set; }
    public decimal? MoneyInvolved { get; set; }
    public int? NumberOfChildrenInvolved { get; set; }
    public required GeographicalEntityListItem? GeographicalEntity { get; set; }
    public OrganizationListItem? OrganizationListItemFrom { get; set; }
    public OrganizationListItem? OrganizationListItemTo { get; set; }
    public abstract string OrganizationFromName { get; }
    public abstract string OrganizationToName { get; }
    public abstract OrganizationItem? OrganizationItemFrom { get; }
    public abstract OrganizationItem? OrganizationItemTo { get; }
    public abstract RelationSide RelationSideThisOrganization { get; }
    public abstract record InterOrganizationalRelationFrom : InterOrganizationalRelation
    {
        private InterOrganizationalRelationFrom() { }
        public override RelationSide RelationSideThisOrganization => RelationSide.From;
        public abstract InterOrganizationalRelationTo SwapFromAndTo();

        public abstract T Match<T>(
            Func<IncompleteInterOrganizationalRelationFrom, T> incompleteInterOrganizationalRelationFrom,
            Func<CompletedInterOrganizationalRelationFrom, T> completedInterOrganizationalRelationFrom
            );
        public abstract record IncompleteInterOrganizationalRelationFrom : InterOrganizationalRelationFrom
        {
            private IncompleteInterOrganizationalRelationFrom() { }
            public override T Match<T>(
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
                public override OrganizationItem? OrganizationItemTo => OrganizationTo;
                public override string OrganizationToName => OrganizationTo is null ? "" : OrganizationTo.Name;

                public sealed record NewInterOrganizationalNewFromRelation : NewIncompleteInterOrganizationalRelationFrom
                {
                    public required OrganizationName OrganizationFrom { get; set; }
                    public override string OrganizationFromName => OrganizationFrom.Name;
                    public override OrganizationItem? OrganizationItemFrom => OrganizationFrom;
                    public override NewInterOrganizationalNewToRelation SwapFromAndTo()
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
                            Title = Title,
                            OrganizationListItemFrom = OrganizationListItemTo,
                            OrganizationListItemTo = OrganizationListItemFrom,
                        };
                    }
                    public override CompletedInterOrganizationalRelationFrom GetCompletedRelation(OrganizationListItem organizationListItemTo)
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
                    public override string OrganizationFromName => OrganizationFrom.Name;
                    public override OrganizationItem? OrganizationItemFrom => OrganizationFrom;
                    public override NewInterOrganizationalExistingToRelation SwapFromAndTo()
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
                            Title = Title,
                            OrganizationListItemFrom = OrganizationListItemTo,
                            OrganizationListItemTo = OrganizationListItemFrom,
                        };
                    }

                    public override CompletedInterOrganizationalRelationFrom GetCompletedRelation(OrganizationListItem organizationListItemTo)
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

            private CompletedInterOrganizationalRelationFrom() { }
            public override T Match<T>(
                Func<IncompleteInterOrganizationalRelationFrom, T> incompleteInterOrganizationalRelationFrom,
                Func<CompletedInterOrganizationalRelationFrom, T> completedInterOrganizationalRelationFrom
            )
            {
                return completedInterOrganizationalRelationFrom(this);
            }

            public abstract void SetName(string name);
            public sealed record CompletedNewInterOrganizationalNewFromRelation : CompletedInterOrganizationalRelationFrom
            {
                public required OrganizationName OrganizationFrom { get; set; }
                public required OrganizationListItem OrganizationTo { get; set; }

                public override void SetName(string name)
                {
                    OrganizationFrom.Name = name;
                }
                public override string OrganizationFromName => OrganizationFrom.Name;
                public override string OrganizationToName => OrganizationTo.Name;
                public override OrganizationItem? OrganizationItemFrom => OrganizationFrom;
                public override OrganizationItem? OrganizationItemTo => OrganizationTo;

                public override CompletedNewInterOrganizationalNewToRelation SwapFromAndTo()
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
                        Title = Title,
                        OrganizationListItemFrom = OrganizationListItemTo,
                        OrganizationListItemTo = OrganizationListItemFrom,
                    };
                }
            }

            public abstract record ResolvedInterOrganizationalRelationFrom : CompletedInterOrganizationalRelationFrom
            {
                private ResolvedInterOrganizationalRelationFrom() { }
                public override void SetName(string name)
                {
                    OrganizationFrom.Name = name;
                }

                public required OrganizationListItem OrganizationFrom { get; set; }
                public required OrganizationListItem OrganizationTo { get; set; }
                public override string OrganizationFromName => OrganizationFrom.Name;
                public override string OrganizationToName => OrganizationTo.Name;
                public override OrganizationItem? OrganizationItemFrom => OrganizationFrom;
                public override OrganizationItem? OrganizationItemTo => OrganizationTo;

                public sealed record ExistingInterOrganizationalRelationFrom : ResolvedInterOrganizationalRelationFrom, ExistingNode
                {
                    public int NodeId { get; init; }
                    public int UrlId { get; set; }
                    public override ExistingInterOrganizationalRelationTo SwapFromAndTo()
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
                            Title = Title,
                            OrganizationListItemFrom = OrganizationListItemTo,
                            OrganizationListItemTo = OrganizationListItemFrom,
                        };

                    }

                }

                public sealed record NewInterOrganizationalExistingRelationFrom : ResolvedInterOrganizationalRelationFrom
                {

                    public override NewInterOrganizationalExistingRelationTo SwapFromAndTo()
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
                            Title = Title,
                            OrganizationListItemFrom = OrganizationListItemTo,
                            OrganizationListItemTo = OrganizationListItemFrom,
                        };
                    }
                }
            }
        }
    }
    public abstract record InterOrganizationalRelationTo : InterOrganizationalRelation
    {

        private InterOrganizationalRelationTo() { }
        public abstract T Match<T>(
            Func<IncompleteInterOrganizationalRelationTo, T> incompleteInterOrganizationalRelationTo,
            Func<CompletedInterOrganizationalRelationTo, T> completedInterOrganizationalRelationTo
        );

        public abstract InterOrganizationalRelationFrom SwapFromAndTo();
        public override RelationSide RelationSideThisOrganization => RelationSide.To;
        public abstract record IncompleteInterOrganizationalRelationTo : InterOrganizationalRelationTo
        {
            private IncompleteInterOrganizationalRelationTo() { }

            public override T Match<T>(
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
                public override string OrganizationFromName => OrganizationFrom is null ? "" : OrganizationFrom.Name;
                public override OrganizationItem? OrganizationItemFrom => OrganizationFrom;
                public sealed record NewInterOrganizationalExistingToRelation : NewIncompleteInterOrganizationalRelationTo
                {
                    public required OrganizationListItem OrganizationTo { get; set; }
                    public override string OrganizationToName => OrganizationTo.Name;
                    public override OrganizationItem? OrganizationItemTo => OrganizationTo;
                    public override NewInterOrganizationalExistingFromRelation SwapFromAndTo()
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
                            Title = Title,
                            OrganizationListItemFrom = OrganizationListItemTo,
                            OrganizationListItemTo = OrganizationListItemFrom,
                        };
                    }

                    public override CompletedInterOrganizationalRelationTo GetCompletedRelation(OrganizationListItem organizationListItemFrom)
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
                    public override string OrganizationToName => OrganizationTo.Name;
                    public override OrganizationItem? OrganizationItemTo => OrganizationTo;

                    public override NewInterOrganizationalNewFromRelation SwapFromAndTo()
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
                            Title = Title,
                            OrganizationListItemFrom = OrganizationListItemTo,
                            OrganizationListItemTo = OrganizationListItemFrom,
                        };
                    }
                    public override CompletedInterOrganizationalRelationTo GetCompletedRelation(OrganizationListItem organizationListItemFrom)
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
            public override T Match<T>(
                Func<IncompleteInterOrganizationalRelationTo, T> incompleteInterOrganizationalRelationTo,
                Func<CompletedInterOrganizationalRelationTo, T> completedInterOrganizationalRelationTo
            )
            {
                return completedInterOrganizationalRelationTo(this);
            }

            public abstract void SetName(string name);
            public sealed record CompletedNewInterOrganizationalNewToRelation : CompletedInterOrganizationalRelationTo
            {
                public required OrganizationListItem OrganizationFrom { get; set; }
                public required OrganizationName OrganizationTo { get; set; }

                public override void SetName(string name)
                {
                    OrganizationTo.Name = name;
                }
                public override string OrganizationFromName => OrganizationFrom.Name;
                public override string OrganizationToName => OrganizationTo.Name;
                public override OrganizationItem? OrganizationItemFrom => OrganizationFrom;
                public override OrganizationItem? OrganizationItemTo => OrganizationTo;
                public override CompletedNewInterOrganizationalNewFromRelation SwapFromAndTo()
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
                        Title = Title,
                        OrganizationListItemFrom = OrganizationListItemTo,
                        OrganizationListItemTo = OrganizationListItemFrom,
                    };
                }
            }

            public abstract record ResolvedInterOrganizationalRelationTo : CompletedInterOrganizationalRelationTo
            {
                private ResolvedInterOrganizationalRelationTo() { }
                public required OrganizationListItem OrganizationFrom { get; set; }
                public required OrganizationListItem OrganizationTo { get; set; }

                public override void SetName(string name)
                {
                    OrganizationTo.Name = name;
                }

                public override string OrganizationFromName => OrganizationFrom.Name;
                public override string OrganizationToName => OrganizationTo.Name;
                public override OrganizationItem? OrganizationItemFrom => OrganizationFrom;
                public override OrganizationItem? OrganizationItemTo => OrganizationTo;

                public sealed record ExistingInterOrganizationalRelationTo : ResolvedInterOrganizationalRelationTo, ExistingNode
                {
                    public int NodeId { get; init; }
                    public int UrlId { get; set; }
                    public override ExistingInterOrganizationalRelationFrom SwapFromAndTo()
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
                            Title = Title,
                            OrganizationListItemFrom = OrganizationListItemTo,
                            OrganizationListItemTo = OrganizationListItemFrom,
                        };
                    }
                }

                public sealed record NewInterOrganizationalExistingRelationTo : ResolvedInterOrganizationalRelationTo
                {

                    public override NewInterOrganizationalExistingRelationFrom SwapFromAndTo()
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
                            Title = Title,
                            OrganizationListItemFrom = OrganizationListItemTo,
                            OrganizationListItemTo = OrganizationListItemFrom,
                        };
                    }
                }
            }

        }
    }
}