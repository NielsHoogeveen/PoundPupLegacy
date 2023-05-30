﻿using static PoundPupLegacy.EditModel.NodeDetails;

namespace PoundPupLegacy.EditModel;
[JsonSerializable(typeof(InterOrganizationalRelation.From.Complete), TypeInfoPropertyName = "InterOrganizationalRelationFromComplete")]
public partial class InterOrganizationalRelationFromJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(InterOrganizationalRelation.To.Complete), TypeInfoPropertyName = "InterOrganizationalRelationToComplete")]
public partial class InterOrganizationalRelationToJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(InterOrganizationalRelation.From.Complete.Resolved.ToUpdate), TypeInfoPropertyName = "InterOrganizationalRelationFromCompleteResolvedToUpdate")]
public partial class ExistingInterOrganizationalRelationFromJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(InterOrganizationalRelation.To.Complete.Resolved.ToUpdate), TypeInfoPropertyName = "InterOrganizationalRelationToCompleteResolvedToUpdate")]
public partial class ExistingInterOrganizationalRelationToJsonContext : JsonSerializerContext { }
public static class InterOrganizationalRelationExtentions
{
    public static InterOrganizationalRelation.From.Incomplete.New.ForExistingOrganization GetNewInterOrganizationalRelationFrom(this OrganizationListItem organizationListItem, InterOrganizationalRelationTypeListItem interOrganizationalRelationType, int ownerId, int publisherId)
    {
        return new InterOrganizationalRelation.From.Incomplete.New.ForExistingOrganization {
            OrganizationFrom = organizationListItem,
            OrganizationTo = null,
            InterOrganizationalRelationDetails = InterOrganizationalRelationDetails.EmptyInstance(interOrganizationalRelationType),
            NodeDetailsForCreate = NodeDetails.EmptyInstance(47, "inter organizational relation", ownerId, publisherId),
            RelationDetails = RelationDetails.EmptyInstance,
        };
    }
    public static InterOrganizationalRelation.From.Incomplete.New.ForNewOrganization GetNewInterOrganizationalRelationFrom(this OrganizationName organizationName, InterOrganizationalRelationTypeListItem interOrganizationalRelationType, int ownerId, int publisherId)
    {
        return new InterOrganizationalRelation.From.Incomplete.New.ForNewOrganization {
            OrganizationFrom = organizationName,
            OrganizationTo = null,
            InterOrganizationalRelationDetails = InterOrganizationalRelationDetails.EmptyInstance(interOrganizationalRelationType),
            NodeDetailsForCreate = NodeDetails.EmptyInstance(47, "inter organizational relation", ownerId, publisherId),
            RelationDetails = RelationDetails.EmptyInstance,
        };
    }
    public static InterOrganizationalRelation.To.Incomplete.New.ForExistingOrganization GetNewInterOrganizationalRelationTo(this OrganizationListItem organizationListItem, InterOrganizationalRelationTypeListItem interOrganizationalRelationType, int ownerId, int publisherId)
    {
        return new InterOrganizationalRelation.To.Incomplete.New.ForExistingOrganization {
            OrganizationFrom = null,
            OrganizationTo = organizationListItem,
            InterOrganizationalRelationDetails = InterOrganizationalRelationDetails.EmptyInstance(interOrganizationalRelationType),
            NodeDetailsForCreate = NodeDetails.EmptyInstance(47, "inter organizational relation", ownerId, publisherId),
            RelationDetails = RelationDetails.EmptyInstance,
        };

    }
    public static InterOrganizationalRelation.To.Incomplete.New.ForNewOrganization GetNewInterOrganizationalRelationTo(this OrganizationName organizationName, InterOrganizationalRelationTypeListItem interOrganizationalRelationType, int ownerId, int publisherId)
    {
        return new InterOrganizationalRelation.To.Incomplete.New.ForNewOrganization {
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
    public abstract record From : InterOrganizationalRelation
    {
        private From() { }
        public sealed override RelationSide RelationSideThisOrganization => RelationSide.From;
        public abstract To SwapFromAndTo();
        public abstract OrganizationItem? OrganizationItemFrom { get; }
        public abstract OrganizationListItem? OrganizationItemTo { get; set; }

        public abstract T Match<T>(
            Func<Incomplete, T> incompleteInterOrganizationalRelationFrom,
            Func<Complete, T> completedInterOrganizationalRelationFrom
            );
        public abstract record Incomplete : From
        {
            private Incomplete() { }
            public sealed override T Match<T>(
                Func<Incomplete, T> incompleteInterOrganizationalRelationFrom,
                Func<Complete, T> completedInterOrganizationalRelationFrom
            )
            {
                return incompleteInterOrganizationalRelationFrom(this);
            }
            public abstract Complete GetCompletedRelation(OrganizationListItem organizationListItemFrom);
            public abstract record New : Incomplete
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

                public sealed record ForNewOrganization : New, NewNode
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
                    public sealed override To.Incomplete.New.ForNewOrganization SwapFromAndTo()
                    {
                        return new To.Incomplete.New.ForNewOrganization {
                            OrganizationFrom = OrganizationTo,
                            OrganizationTo = OrganizationFrom,
                            InterOrganizationalRelationDetails = InterOrganizationalRelationDetails,
                            NodeDetailsForCreate = NodeDetailsForCreate,
                            RelationDetails = RelationDetails,
                        };
                    }
                    public sealed override Complete GetCompletedRelation(OrganizationListItem organizationListItemTo)
                    {
                        return new Complete.ToCreateForNewOrganization {
                            OrganizationFrom = OrganizationFrom,
                            OrganizationTo = organizationListItemTo,
                            InterOrganizationalRelationDetails = InterOrganizationalRelationDetails,
                            NodeDetailsForCreate = NodeDetailsForCreate,
                            RelationDetails = RelationDetails,
                        };
                    }
                }

                public sealed record ForExistingOrganization : New, NewNode
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
                    public sealed override To.Incomplete.New.ForExistingOrganization SwapFromAndTo()
                    {
                        return new To.Incomplete.New.ForExistingOrganization {
                            OrganizationFrom = OrganizationTo,
                            OrganizationTo = OrganizationFrom,
                            InterOrganizationalRelationDetails = InterOrganizationalRelationDetails,
                            NodeDetailsForCreate = NodeDetailsForCreate,
                            RelationDetails = RelationDetails,
                        };
                    }

                    public sealed override Complete GetCompletedRelation(OrganizationListItem organizationListItemTo)
                    {
                        return new Complete.Resolved.ToCreate {
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

        public abstract record Complete : From
        {
            public required OrganizationListItem OrganizationTo { get; set; }

            private Complete() { }
            public sealed override T Match<T>(
                Func<Incomplete, T> incompleteInterOrganizationalRelationFrom,
                Func<Complete, T> completedInterOrganizationalRelationFrom
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

            public sealed record ToCreateForNewOrganization : Complete, NewNode
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
                public sealed override To.Complete.ToCreateForNewOrganization SwapFromAndTo()
                {
                    return new To.Complete.ToCreateForNewOrganization {
                        OrganizationFrom = OrganizationTo,
                        OrganizationTo = OrganizationFrom,
                        InterOrganizationalRelationDetails = InterOrganizationalRelationDetails,
                        NodeDetailsForCreate = NodeDetailsForCreate,
                        RelationDetails = RelationDetails,
                    };
                }
            }

            public abstract record Resolved : Complete
            {
                private Resolved() { }
                public sealed override void SetName(string name)
                {
                    OrganizationFrom.Name = name;
                }

                public required OrganizationListItem OrganizationFrom { get; set; }
                public sealed override string OrganizationFromName => OrganizationFrom.Name;
                public sealed override string OrganizationToName => OrganizationTo.Name;
                public sealed override OrganizationItem? OrganizationItemFrom => OrganizationFrom;

                public sealed record ToUpdate : Resolved, ExistingNode
                {
                    public override NodeDetails NodeDetails => NodeDetailsForUpdate;
                    public required NodeDetails.NodeDetailsForUpdate NodeDetailsForUpdate { get; init; }
                    public required NodeIdentification NodeIdentification { get; init; }

                    public sealed override To.Complete.Resolved.ToUpdate SwapFromAndTo()
                    {
                        return new To.Complete.Resolved.ToUpdate {
                            OrganizationFrom = OrganizationTo,
                            OrganizationTo = OrganizationFrom,
                            InterOrganizationalRelationDetails = InterOrganizationalRelationDetails,
                            NodeDetailsForUpdate = NodeDetailsForUpdate,
                            RelationDetails = RelationDetails,
                            NodeIdentification = NodeIdentification,
                        };
                    }
                }

                public sealed record ToCreate : Resolved, NewNode
                {
                    public override NodeDetails NodeDetails => NodeDetailsForCreate;
                    public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
                    public sealed override To.Complete.Resolved.ToCreate SwapFromAndTo()
                    {
                        return new To.Complete.Resolved.ToCreate {
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
    public abstract record To : InterOrganizationalRelation
    {
        public abstract OrganizationListItem? OrganizationItemFrom { get; set; }
        public abstract OrganizationItem? OrganizationItemTo { get; }

        private To() { }
        public abstract T Match<T>(
            Func<Incomplete, T> incompleteInterOrganizationalRelationTo,
            Func<Complete, T> completedInterOrganizationalRelationTo
        );

        public abstract From SwapFromAndTo();
        public sealed override RelationSide RelationSideThisOrganization => RelationSide.To;
        public abstract record Incomplete : To
        {
            private Incomplete() { }

            public sealed override T Match<T>(
                Func<Incomplete, T> incompleteInterOrganizationalRelationTo,
                Func<Complete, T> completedInterOrganizationalRelationTo
            )
            {
                return incompleteInterOrganizationalRelationTo(this);
            }
            public abstract Complete GetCompletedRelation(OrganizationListItem organizationListItemFrom);

            public abstract record New : Incomplete
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
                public sealed record ForExistingOrganization : New, NewNode
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
                    public sealed override From.Incomplete.New.ForExistingOrganization SwapFromAndTo()
                    {
                        return new From.Incomplete.New.ForExistingOrganization {
                            OrganizationFrom = OrganizationTo,
                            OrganizationTo = OrganizationFrom,
                            InterOrganizationalRelationDetails = InterOrganizationalRelationDetails,
                            NodeDetailsForCreate = NodeDetailsForCreate,
                            RelationDetails = RelationDetails,
                        };
                    }

                    public sealed override Complete GetCompletedRelation(OrganizationListItem organizationListItemFrom)
                    {
                        return new Complete.Resolved.ToCreate {
                            OrganizationFrom = organizationListItemFrom,
                            OrganizationTo = OrganizationTo,
                            InterOrganizationalRelationDetails = InterOrganizationalRelationDetails,
                            NodeDetailsForCreate = NodeDetailsForCreate,
                            RelationDetails = RelationDetails,
                        };
                    }
                }

                public sealed record ForNewOrganization : New, NewNode
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

                    public sealed override From.Incomplete.New.ForNewOrganization SwapFromAndTo()
                    {
                        return new From.Incomplete.New.ForNewOrganization {
                            OrganizationFrom = OrganizationTo,
                            OrganizationTo = OrganizationFrom,
                            InterOrganizationalRelationDetails = InterOrganizationalRelationDetails,
                            NodeDetailsForCreate = NodeDetailsForCreate,
                            RelationDetails = RelationDetails,
                        };
                    }
                    public sealed override Complete GetCompletedRelation(OrganizationListItem organizationListItemFrom)
                    {
                        return new Complete.ToCreateForNewOrganization {
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

        public abstract record Complete : To
        {
            private Complete() { }
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
                Func<Incomplete, T> incompleteInterOrganizationalRelationTo,
                Func<Complete, T> completedInterOrganizationalRelationTo
            )
            {
                return completedInterOrganizationalRelationTo(this);
            }
            public sealed record ToCreateForNewOrganization : Complete, NewNode
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
                public sealed override From.Complete.ToCreateForNewOrganization SwapFromAndTo()
                {
                    return new From.Complete.ToCreateForNewOrganization {
                        OrganizationFrom = OrganizationTo,
                        OrganizationTo = OrganizationFrom,
                        InterOrganizationalRelationDetails = InterOrganizationalRelationDetails,
                        RelationDetails = RelationDetails,
                        NodeDetailsForCreate = NodeDetailsForCreate
                    };
                }
            }

            public abstract record Resolved : Complete
            {
                private Resolved() { }
                public required OrganizationListItem OrganizationTo { get; set; }

                public sealed override void SetName(string name)
                {
                    OrganizationTo.Name = name;
                }

                public sealed override string OrganizationToName => OrganizationTo.Name;

                public sealed override OrganizationItem? OrganizationItemTo => OrganizationTo;

                public sealed record ToUpdate : Resolved, ExistingNode
                {
                    public override NodeDetails NodeDetails => NodeDetailsForUpdate;
                    public required NodeDetails.NodeDetailsForUpdate NodeDetailsForUpdate { get; init; }
                    public required NodeIdentification NodeIdentification { get; init; }
                    public sealed override From.Complete.Resolved.ToUpdate SwapFromAndTo()
                    {
                        return new From.Complete.Resolved.ToUpdate {
                            OrganizationFrom = OrganizationTo,
                            OrganizationTo = OrganizationFrom,
                            InterOrganizationalRelationDetails = InterOrganizationalRelationDetails,
                            NodeIdentification = NodeIdentification,
                            RelationDetails = RelationDetails,
                            NodeDetailsForUpdate = NodeDetailsForUpdate
                        };
                    }
                }

                public sealed record ToCreate : Resolved, NewNode
                {
                    public override NodeDetails NodeDetails => NodeDetailsForCreate;
                    public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }

                    public sealed override From.Complete.Resolved.ToCreate SwapFromAndTo()
                    {
                        return new From.Complete.Resolved.ToCreate {
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