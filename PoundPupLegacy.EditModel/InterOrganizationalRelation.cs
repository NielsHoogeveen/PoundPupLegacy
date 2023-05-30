using static PoundPupLegacy.EditModel.NodeDetails;

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
            InterOrganizationalRelationDetails = InterOrganizationalRelationDetails.EmptyInstance(interOrganizationalRelationType),
            NodeDetailsForCreate = NodeDetails.EmptyInstance(47, "inter organizational relation", ownerId, publisherId),
            RelationDetails = RelationDetails.EmptyInstance,
        };
    }
    public static NewInterOrganizationalNewFromRelation GetNewInterOrganizationalRelationFrom(this OrganizationName organizationName, InterOrganizationalRelationTypeListItem interOrganizationalRelationType, int ownerId, int publisherId)
    {
        return new NewInterOrganizationalNewFromRelation {
            OrganizationFrom = organizationName,
            OrganizationTo = null,
            InterOrganizationalRelationDetails = InterOrganizationalRelationDetails.EmptyInstance(interOrganizationalRelationType),
            NodeDetailsForCreate = NodeDetails.EmptyInstance(47, "inter organizational relation", ownerId, publisherId),
            RelationDetails = RelationDetails.EmptyInstance,
        };
    }
    public static NewInterOrganizationalExistingToRelation GetNewInterOrganizationalRelationTo(this OrganizationListItem organizationListItem, InterOrganizationalRelationTypeListItem interOrganizationalRelationType, int ownerId, int publisherId)
    {
        return new NewInterOrganizationalExistingToRelation {
            OrganizationFrom = null,
            OrganizationTo = organizationListItem,
            InterOrganizationalRelationDetails = InterOrganizationalRelationDetails.EmptyInstance(interOrganizationalRelationType),
            NodeDetailsForCreate = NodeDetails.EmptyInstance(47, "inter organizational relation", ownerId, publisherId),
            RelationDetails = RelationDetails.EmptyInstance,
        };

    }
    public static NewInterOrganizationalNewToRelation GetNewInterOrganizationalRelationTo(this OrganizationName organizationName, InterOrganizationalRelationTypeListItem interOrganizationalRelationType, int ownerId, int publisherId)
    {
        return new NewInterOrganizationalNewToRelation {
            OrganizationFrom = null,
            OrganizationTo = organizationName,
            InterOrganizationalRelationDetails = InterOrganizationalRelationDetails.EmptyInstance(interOrganizationalRelationType),
            NodeDetailsForCreate = NodeDetails.EmptyInstance(47, "inter organizational relation", ownerId, publisherId),
            RelationDetails = RelationDetails.EmptyInstance,
        };
    }
}
public abstract record InterOrganizationalRelation : Relation
{
    private InterOrganizationalRelation()
    {
    }
    public abstract void SetName(string name);
    public required RelationDetails RelationDetails { get; init; }
    public abstract NodeDetails NodeDetails { get; }
    public required InterOrganizationalRelationDetails InterOrganizationalRelationDetails { get; init; }
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

                public sealed record NewInterOrganizationalNewFromRelation : NewIncompleteInterOrganizationalRelationFrom, NewNode
                {
                    public override NodeDetails NodeDetails => NodeDetailsForCreate;
                    public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
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
                            InterOrganizationalRelationDetails = InterOrganizationalRelationDetails,
                            NodeDetailsForCreate = NodeDetailsForCreate,
                            RelationDetails = RelationDetails,
                        };
                    }
                    public sealed override CompletedInterOrganizationalRelationFrom GetCompletedRelation(OrganizationListItem organizationListItemTo)
                    {
                        return new CompletedNewInterOrganizationalNewFromRelation {
                            OrganizationFrom = OrganizationFrom,
                            OrganizationTo = organizationListItemTo,
                            InterOrganizationalRelationDetails = InterOrganizationalRelationDetails,
                            NodeDetailsForCreate = NodeDetailsForCreate,
                            RelationDetails = RelationDetails,
                        };
                    }
                }

                public sealed record NewInterOrganizationalExistingFromRelation : NewIncompleteInterOrganizationalRelationFrom, NewNode
                {
                    public override NodeDetails NodeDetails => NodeDetailsForCreate;
                    public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
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
                            InterOrganizationalRelationDetails = InterOrganizationalRelationDetails,
                            NodeDetailsForCreate = NodeDetailsForCreate,
                            RelationDetails = RelationDetails,
                        };
                    }

                    public sealed override CompletedInterOrganizationalRelationFrom GetCompletedRelation(OrganizationListItem organizationListItemTo)
                    {
                        return new NewInterOrganizationalExistingRelationFrom {
                            OrganizationFrom = OrganizationFrom,
                            OrganizationTo = organizationListItemTo,
                            InterOrganizationalRelationDetails = InterOrganizationalRelationDetails,
                            NodeDetailsForCreate = NodeDetailsForCreate,
                            RelationDetails = RelationDetails,
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

            public sealed record CompletedNewInterOrganizationalNewFromRelation : CompletedInterOrganizationalRelationFrom, NewNode
            {
                public override NodeDetails NodeDetails => NodeDetailsForCreate;
                public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
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
                        InterOrganizationalRelationDetails = InterOrganizationalRelationDetails,
                        NodeDetailsForCreate = NodeDetailsForCreate,
                        RelationDetails = RelationDetails,
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
                    public override NodeDetails NodeDetails => NodeDetailsForUpdate;
                    public required NodeDetails.NodeDetailsForUpdate NodeDetailsForUpdate { get; init; }
                    public required NodeIdentification NodeIdentification { get; init; }

                    public sealed override ExistingInterOrganizationalRelationTo SwapFromAndTo()
                    {
                        return new ExistingInterOrganizationalRelationTo {
                            OrganizationFrom = OrganizationTo,
                            OrganizationTo = OrganizationFrom,
                            InterOrganizationalRelationDetails = InterOrganizationalRelationDetails,
                            NodeDetailsForUpdate = NodeDetailsForUpdate,
                            RelationDetails = RelationDetails,
                            NodeIdentification = NodeIdentification,
                        };
                    }
                }

                public sealed record NewInterOrganizationalExistingRelationFrom : ResolvedInterOrganizationalRelationFrom, NewNode
                {
                    public override NodeDetails NodeDetails => NodeDetailsForCreate;
                    public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
                    public sealed override NewInterOrganizationalExistingRelationTo SwapFromAndTo()
                    {
                        return new NewInterOrganizationalExistingRelationTo {
                            OrganizationFrom = OrganizationTo,
                            OrganizationTo = OrganizationFrom,
                            InterOrganizationalRelationDetails = InterOrganizationalRelationDetails,
                            NodeDetailsForCreate = NodeDetailsForCreate,
                            RelationDetails = RelationDetails,
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
                public sealed record NewInterOrganizationalExistingToRelation : NewIncompleteInterOrganizationalRelationTo, NewNode
                {
                    public override NodeDetails NodeDetails => NodeDetailsForCreate;
                    public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
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
                            InterOrganizationalRelationDetails = InterOrganizationalRelationDetails,
                            NodeDetailsForCreate = NodeDetailsForCreate,
                            RelationDetails = RelationDetails,
                        };
                    }

                    public sealed override CompletedInterOrganizationalRelationTo GetCompletedRelation(OrganizationListItem organizationListItemFrom)
                    {
                        return new NewInterOrganizationalExistingRelationTo {
                            OrganizationFrom = organizationListItemFrom,
                            OrganizationTo = OrganizationTo,
                            InterOrganizationalRelationDetails = InterOrganizationalRelationDetails,
                            NodeDetailsForCreate = NodeDetailsForCreate,
                            RelationDetails = RelationDetails,
                        };
                    }
                }

                public sealed record NewInterOrganizationalNewToRelation : NewIncompleteInterOrganizationalRelationTo, NewNode
                {
                    public override NodeDetails NodeDetails => NodeDetailsForCreate;
                    public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
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
                            InterOrganizationalRelationDetails = InterOrganizationalRelationDetails,
                            NodeDetailsForCreate = NodeDetailsForCreate,
                            RelationDetails = RelationDetails,
                        };
                    }
                    public sealed override CompletedInterOrganizationalRelationTo GetCompletedRelation(OrganizationListItem organizationListItemFrom)
                    {
                        return new CompletedNewInterOrganizationalNewToRelation {
                            OrganizationFrom = organizationListItemFrom,
                            OrganizationTo = OrganizationTo,
                            InterOrganizationalRelationDetails = InterOrganizationalRelationDetails,
                            NodeDetailsForCreate = NodeDetailsForCreate,
                            RelationDetails = RelationDetails,
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
            public sealed record CompletedNewInterOrganizationalNewToRelation : CompletedInterOrganizationalRelationTo, NewNode
            {
                public override NodeDetails NodeDetails => NodeDetailsForCreate;
                public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
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
                        InterOrganizationalRelationDetails = InterOrganizationalRelationDetails,
                        RelationDetails = RelationDetails,
                        NodeDetailsForCreate = NodeDetailsForCreate
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
                    public override NodeDetails NodeDetails => NodeDetailsForUpdate;
                    public required NodeDetails.NodeDetailsForUpdate NodeDetailsForUpdate { get; init; }
                    public required NodeIdentification NodeIdentification { get; init; }
                    public sealed override ExistingInterOrganizationalRelationFrom SwapFromAndTo()
                    {
                        return new ExistingInterOrganizationalRelationFrom {
                            OrganizationFrom = OrganizationTo,
                            OrganizationTo = OrganizationFrom,
                            InterOrganizationalRelationDetails = InterOrganizationalRelationDetails,
                            NodeIdentification = NodeIdentification,
                            RelationDetails = RelationDetails,
                            NodeDetailsForUpdate = NodeDetailsForUpdate
                        };
                    }
                }

                public sealed record NewInterOrganizationalExistingRelationTo : ResolvedInterOrganizationalRelationTo, NewNode
                {
                    public override NodeDetails NodeDetails => NodeDetailsForCreate;
                    public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }

                    public sealed override NewInterOrganizationalExistingRelationFrom SwapFromAndTo()
                    {
                        return new NewInterOrganizationalExistingRelationFrom {
                            OrganizationFrom = OrganizationTo,
                            OrganizationTo = OrganizationFrom,
                            InterOrganizationalRelationDetails = InterOrganizationalRelationDetails,
                            RelationDetails = RelationDetails,
                            NodeDetailsForCreate = NodeDetailsForCreate
                        };
                    }
                }
            }
        }
    }
}

public sealed record InterOrganizationalRelationDetails
{
    public static InterOrganizationalRelationDetails EmptyInstance(InterOrganizationalRelationTypeListItem interOrganizationalRelationType) => new InterOrganizationalRelationDetails
    {
        GeographicalEntity = null,
        MoneyInvolved = null,
        InterOrganizationalRelationType = interOrganizationalRelationType,
        NumberOfChildrenInvolved = null
    };
    public required InterOrganizationalRelationTypeListItem InterOrganizationalRelationType { get; set; }
    public decimal? MoneyInvolved { get; set; }
    public int? NumberOfChildrenInvolved { get; set; }
    public required GeographicalEntityListItem? GeographicalEntity { get; set; }

}