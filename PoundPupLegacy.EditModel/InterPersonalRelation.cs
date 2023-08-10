namespace PoundPupLegacy.EditModel;

public static class InterPersonalRelationExtentions{
    public static InterPersonalRelation.From.Incomplete.ToCreateForExistingPerson GetNewInterPersonalRelationFrom(this PersonListItem personListItem, InterPersonalRelationTypeListItem interPersonalRelationType, int ownerId, int publisherId, List<TenantDetails> tenants)
    {
        return new InterPersonalRelation.From.Incomplete.ToCreateForExistingPerson {
            PersonFrom = personListItem,
            PersonTo = null,
            InterPersonalRelationType = interPersonalRelationType,
            NodeDetailsForCreate = NodeDetails.EmptyInstance(46, "inter personal relation", ownerId, publisherId, tenants),
            RelationDetails = RelationDetails.EmptyInstance,
        };
    }
    public static InterPersonalRelation.To.Incomplete.ToCreateForExistingPerson GetNewInterPersonalRelationTo(this PersonListItem personListItem, InterPersonalRelationTypeListItem interPersonalRelationType, int ownerId, int publisherId, List<TenantDetails> tenants)
    {
        return new InterPersonalRelation.To.Incomplete.ToCreateForExistingPerson {
            PersonFrom = null,
            PersonTo = personListItem,
            InterPersonalRelationType = interPersonalRelationType,
            NodeDetailsForCreate = NodeDetails.EmptyInstance(46, "inter personal relation", ownerId, publisherId, tenants),
            RelationDetails = RelationDetails.EmptyInstance,
        };

    }
    public static InterPersonalRelation.From.Incomplete.ToCreateForNewPerson GetNewInterPersonalRelationFrom(this PersonName personName, InterPersonalRelationTypeListItem interPersonalRelationType, int ownerId, int publisherId, List<TenantDetails> tenants)
    {
        return new InterPersonalRelation.From.Incomplete.ToCreateForNewPerson {
            PersonFrom = personName,
            PersonTo = null,
            InterPersonalRelationType = interPersonalRelationType,
            NodeDetailsForCreate = NodeDetails.EmptyInstance(46, "inter personal relation", ownerId, publisherId, tenants),
            RelationDetails = RelationDetails.EmptyInstance,
        };
    }
    public static InterPersonalRelation.To.Incomplete.ToCreateForNewPerson GetNewInterPersonalRelationTo(this PersonName personName, InterPersonalRelationTypeListItem interPersonalRelationType, int ownerId, int publisherId, List<TenantDetails> tenants)
    {
        return new InterPersonalRelation.To.Incomplete.ToCreateForNewPerson {
            PersonFrom = null,
            PersonTo = personName,
            InterPersonalRelationType = interPersonalRelationType,
            NodeDetailsForCreate = NodeDetails.EmptyInstance(46, "inter personal relation", ownerId, publisherId, tenants),
            RelationDetails = RelationDetails.EmptyInstance,
        };
    }
}


public abstract record InterPersonalRelation : Relation
{
    private InterPersonalRelation()
    {
    }
    public abstract void SetName(string name);
    public required RelationDetails RelationDetails { get; init; }
    public abstract NodeDetails NodeDetails { get; }
    public required InterPersonalRelationTypeListItem InterPersonalRelationType { get; set; }
    public abstract string PersonFromName { get; }
    public abstract string PersonToName { get; }
    public abstract RelationSide RelationSideThisPerson { get; }
    public abstract record From : InterPersonalRelation
    {
        private From() { }
        public sealed override RelationSide RelationSideThisPerson => RelationSide.From;
        public abstract To SwapFromAndTo();
        public abstract PersonItem? PersonItemFrom { get; }
        public abstract PersonListItem? PersonItemTo { get; set; }

        public abstract T Match<T>(
            Func<Incomplete, T> incompleteInterPersonalRelationFrom,
            Func<Complete, T> completedInterPersonalRelationFrom
            );
        public abstract record Incomplete : From
        {
            private Incomplete() { }
            public override NodeDetails NodeDetails => NodeDetailsForCreate;
            public required NodeDetails.ForCreate NodeDetailsForCreate { get; init; }
            public sealed override T Match<T>(
                Func<Incomplete, T> incompleteInterPersonalRelationFrom,
                Func<Complete, T> completedInterPersonalRelationFrom
            )
            {
                return incompleteInterPersonalRelationFrom(this);
            }
            public abstract Complete GetCompletedRelation(PersonListItem organizationListItemFrom);
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

            public sealed record ToCreateForNewPerson : Incomplete, NewNode
            {
                public required PersonName PersonFrom { get; set; }
                public sealed override void SetName(string name)
                {
                    PersonFrom.Name = name;
                }
                public sealed override string PersonFromName => PersonFrom.Name;
                public sealed override PersonItem? PersonItemFrom => PersonFrom;
                public sealed override To.Incomplete.ToCreateForNewPerson SwapFromAndTo()
                {
                    return new To.Incomplete.ToCreateForNewPerson {
                        PersonFrom = PersonTo,
                        PersonTo = PersonFrom,
                        InterPersonalRelationType = InterPersonalRelationType,
                        NodeDetailsForCreate = NodeDetailsForCreate,
                        RelationDetails = RelationDetails,
                    };
                }
                public sealed override Complete GetCompletedRelation(PersonListItem organizationListItemTo)
                {
                    return new Complete.ToCreateForNewPerson {
                        PersonFrom = PersonFrom,
                        PersonTo = organizationListItemTo,
                        InterPersonalRelationType = InterPersonalRelationType,
                        NodeDetailsForCreate = NodeDetailsForCreate,
                        RelationDetails = RelationDetails,
                    };
                }
            }

            public sealed record ToCreateForExistingPerson : Incomplete, NewNode
            {
                public required PersonListItem PersonFrom { get; set; }
                public sealed override void SetName(string name)
                {
                    PersonFrom.Name = name;
                }
                public sealed override string PersonFromName => PersonFrom.Name;
                public sealed override PersonItem? PersonItemFrom => PersonFrom;
                public sealed override To.Incomplete.ToCreateForExistingPerson SwapFromAndTo()
                {
                    return new To.Incomplete.ToCreateForExistingPerson {
                        PersonFrom = PersonTo,
                        PersonTo = PersonFrom,
                        InterPersonalRelationType = InterPersonalRelationType,
                        NodeDetailsForCreate = NodeDetailsForCreate,
                        RelationDetails = RelationDetails,
                    };
                }

                public sealed override Complete GetCompletedRelation(PersonListItem organizationListItemTo)
                {
                    return new Complete.Resolved.ToCreate {
                        PersonFrom = PersonFrom,
                        PersonTo = organizationListItemTo,
                        InterPersonalRelationType = InterPersonalRelationType,
                        NodeDetailsForCreate = NodeDetailsForCreate,
                        RelationDetails = RelationDetails,
                    };
                }
            }
        }

        public abstract record Complete : From
        {
            public required PersonListItem PersonTo { get; set; }

            private Complete() { }
            public sealed override T Match<T>(
                Func<Incomplete, T> incompleteInterPersonalRelationFrom,
                Func<Complete, T> completedInterPersonalRelationFrom
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

            public sealed record ToCreateForNewPerson : Complete, NewNode
            {
                public override NodeDetails NodeDetails => NodeDetailsForCreate;
                public required NodeDetails.ForCreate NodeDetailsForCreate { get; init; }
                public required PersonName PersonFrom { get; set; }

                public sealed override void SetName(string name)
                {
                    PersonFrom.Name = name;
                }
                public sealed override string PersonFromName => PersonFrom.Name;
                public sealed override string PersonToName => PersonTo.Name;
                public sealed override PersonItem? PersonItemFrom => PersonFrom;
                public sealed override To.Complete.ToCreateForNewPerson SwapFromAndTo()
                {
                    return new To.Complete.ToCreateForNewPerson {
                        PersonFrom = PersonTo,
                        PersonTo = PersonFrom,
                        InterPersonalRelationType = InterPersonalRelationType,
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
                    PersonFrom.Name = name;
                }

                public required PersonListItem PersonFrom { get; set; }
                public sealed override string PersonFromName => PersonFrom.Name;
                public sealed override string PersonToName => PersonTo.Name;
                public sealed override PersonItem? PersonItemFrom => PersonFrom;

                public sealed record ToUpdate : Resolved, ExistingNode
                {
                    public override NodeDetails NodeDetails => NodeDetailsForUpdate;
                    public required NodeDetails.ForUpdate NodeDetailsForUpdate { get; init; }
                    public required NodeIdentification NodeIdentification { get; init; }

                    public sealed override To.Complete.Resolved.ToUpdate SwapFromAndTo()
                    {
                        return new To.Complete.Resolved.ToUpdate {
                            PersonFrom = PersonTo,
                            PersonTo = PersonFrom,
                            InterPersonalRelationType = InterPersonalRelationType,
                            NodeDetailsForUpdate = NodeDetailsForUpdate,
                            RelationDetails = RelationDetails,
                            NodeIdentification = NodeIdentification,
                        };
                    }
                }

                public sealed record ToCreate : Resolved, NewNode
                {
                    public override NodeDetails NodeDetails => NodeDetailsForCreate;
                    public required NodeDetails.ForCreate NodeDetailsForCreate { get; init; }
                    public sealed override To.Complete.Resolved.ToCreate SwapFromAndTo()
                    {
                        return new To.Complete.Resolved.ToCreate {
                            PersonFrom = PersonTo,
                            PersonTo = PersonFrom,
                            InterPersonalRelationType = InterPersonalRelationType,
                            NodeDetailsForCreate = NodeDetailsForCreate,
                            RelationDetails = RelationDetails,
                        };
                    }
                }
            }
        }
    }
    public abstract record To : InterPersonalRelation
    {
        public abstract PersonListItem? PersonItemFrom { get; set; }
        public abstract PersonItem? PersonItemTo { get; }

        private To() { }
        public abstract T Match<T>(
            Func<Incomplete, T> incompleteInterPersonalRelationTo,
            Func<Complete, T> completedInterPersonalRelationTo
        );

        public abstract From SwapFromAndTo();
        public sealed override RelationSide RelationSideThisPerson => RelationSide.To;
        public abstract record Incomplete : To
        {
            public override NodeDetails NodeDetails => NodeDetailsForCreate;
            public required NodeDetails.ForCreate NodeDetailsForCreate { get; init; }
            private Incomplete() { }

            public sealed override T Match<T>(
                Func<Incomplete, T> incompleteInterPersonalRelationTo,
                Func<Complete, T> completedInterPersonalRelationTo
            )
            {
                return incompleteInterPersonalRelationTo(this);
            }
            public abstract Complete GetCompletedRelation(PersonListItem organizationListItemFrom);

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
            public sealed record ToCreateForExistingPerson : Incomplete, NewNode
            {
                public required PersonListItem PersonTo { get; set; }
                public sealed override void SetName(string name)
                {
                    PersonTo.Name = name;
                }
                public sealed override string PersonToName => PersonTo.Name;
                public sealed override PersonItem? PersonItemTo => PersonTo;
                public sealed override From.Incomplete.ToCreateForExistingPerson SwapFromAndTo()
                {
                    return new From.Incomplete.ToCreateForExistingPerson {
                        PersonFrom = PersonTo,
                        PersonTo = PersonFrom,
                        InterPersonalRelationType = InterPersonalRelationType,
                        NodeDetailsForCreate = NodeDetailsForCreate,
                        RelationDetails = RelationDetails,
                    };
                }

                public sealed override Complete GetCompletedRelation(PersonListItem organizationListItemFrom)
                {
                    return new Complete.Resolved.ToCreate {
                        PersonFrom = organizationListItemFrom,
                        PersonTo = PersonTo,
                        InterPersonalRelationType = InterPersonalRelationType,
                        NodeDetailsForCreate = NodeDetailsForCreate,
                        RelationDetails = RelationDetails,
                    };
                }
            }

            public sealed record ToCreateForNewPerson : Incomplete, NewNode
            {
                public required PersonName PersonTo { get; set; }
                public sealed override void SetName(string name)
                {
                    PersonTo.Name = name;
                }
                public sealed override string PersonToName => PersonTo.Name;
                public sealed override PersonItem? PersonItemTo => PersonTo;

                public sealed override From.Incomplete.ToCreateForNewPerson SwapFromAndTo()
                {
                    return new From.Incomplete.ToCreateForNewPerson {
                        PersonFrom = PersonTo,
                        PersonTo = PersonFrom,
                        InterPersonalRelationType = InterPersonalRelationType,
                        NodeDetailsForCreate = NodeDetailsForCreate,
                        RelationDetails = RelationDetails,
                    };
                }
                public sealed override Complete GetCompletedRelation(PersonListItem organizationListItemFrom)
                {
                    return new Complete.ToCreateForNewPerson {
                        PersonFrom = organizationListItemFrom,
                        PersonTo = PersonTo,
                        InterPersonalRelationType = InterPersonalRelationType,
                        NodeDetailsForCreate = NodeDetailsForCreate,
                        RelationDetails = RelationDetails,
                    };
                }
            }
        }

        public abstract record Complete : To
        {
            private Complete() { }
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
                Func<Incomplete, T> incompleteInterPersonalRelationTo,
                Func<Complete, T> completedInterPersonalRelationTo
            )
            {
                return completedInterPersonalRelationTo(this);
            }

            public sealed record ToCreateForNewPerson : Complete, NewNode
            {
                public override NodeDetails NodeDetails => NodeDetailsForCreate;
                public required NodeDetails.ForCreate NodeDetailsForCreate { get; init; }
                public required PersonName PersonTo { get; set; }

                public sealed override void SetName(string name)
                {
                    PersonTo.Name = name;
                }
                public sealed override string PersonToName => PersonTo.Name;

                public sealed override PersonItem? PersonItemTo => PersonTo;
                public sealed override From.Complete.ToCreateForNewPerson SwapFromAndTo()
                {
                    return new From.Complete.ToCreateForNewPerson {
                        PersonFrom = PersonTo,
                        PersonTo = PersonFrom,
                        InterPersonalRelationType = InterPersonalRelationType,
                        RelationDetails = RelationDetails,
                        NodeDetailsForCreate = NodeDetailsForCreate,
                    };
                }
            }

            public abstract record Resolved : Complete
            {
                private Resolved() { }
                public required PersonListItem PersonTo { get; set; }

                public sealed override void SetName(string name)
                {
                    PersonTo.Name = name;
                }

                public sealed override string PersonToName => PersonTo.Name;

                public sealed override PersonItem? PersonItemTo => PersonTo;

                public sealed record ToUpdate : Resolved, ExistingNode
                {
                    public override NodeDetails NodeDetails => NodeDetailsForUpdate;
                    public required NodeDetails.ForUpdate NodeDetailsForUpdate { get; init; }
                    public required NodeIdentification NodeIdentification { get; init; }

                    public sealed override From.Complete.Resolved.ToUpdate SwapFromAndTo()
                    {
                        return new From.Complete.Resolved.ToUpdate {
                            PersonFrom = PersonTo,
                            PersonTo = PersonFrom,
                            InterPersonalRelationType = InterPersonalRelationType,
                            RelationDetails = RelationDetails,
                            NodeIdentification = NodeIdentification,
                            NodeDetailsForUpdate = NodeDetailsForUpdate,
                        };
                    }
                }

                public sealed record ToCreate : Resolved, NewNode
                {
                    public override NodeDetails NodeDetails => NodeDetailsForCreate;
                    public required NodeDetails.ForCreate NodeDetailsForCreate { get; init; }
                    public sealed override From.Complete.Resolved.ToCreate SwapFromAndTo()
                    {
                        return new From.Complete.Resolved.ToCreate {
                            PersonFrom = PersonTo,
                            PersonTo = PersonFrom,
                            InterPersonalRelationType = InterPersonalRelationType,
                            RelationDetails = RelationDetails,
                            NodeDetailsForCreate = NodeDetailsForCreate,
                        };
                    }
                }
            }
        }
    }
}