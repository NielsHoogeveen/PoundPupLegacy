﻿
namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(ExistingInterPersonalRelation))]
public partial class ExistingInterPersonalRelationJsonContext : JsonSerializerContext { }


public abstract record InterPersonalRelation : RelationBase
{
    private InterPersonalRelation()
    {
    }

    [RequireNamedArgs]
    public abstract T Match<T>(
            Func<ExistingInterPersonalRelation, T> existingInterPersonalRelation,
            Func<NewInterPersonalExistingRelation, T> newInterPersonalExistingRelation,
            Func<CompletedNewInterPersonalNewFromRelation, T> completedNewInterPersonalNewFromRelation,
            Func<CompletedNewInterPersonalNewToRelation, T> completedNewInterPersonalNewToRelation,
            Func<NewInterPersonalExistingFromRelation, T> newInterPersonalExistingFromRelation,
            Func<NewInterPersonalExistingToRelation, T> newInterPersonalExistingToRelation,
            Func<NewInterPersonalNewFromRelation, T> newInterPersonalNewFromRelation,
            Func<NewInterPersonalNewToRelation, T> newInterPersonalNewToRelation
        );

    [RequireNamedArgs]
    public abstract T Match<T>(
        Func<IncompleteNewInterPersonalFromRelation, T> incompleteNewInterPersonalFromRelation,
        Func<IncompleteNewInterPersonalToRelation, T> incompleteNewInterPersonalToRelation,
        Func<CompletedNewInterPersonalNewFromRelation, T> completedNewInterPersonalNewFromRelation,
        Func<CompletedNewInterPersonalNewToRelation, T> completedNewInterPersonalNewToRelation,
        Func<ResolvedInterPersonalRelation, T> resolvedInterPersonalRelation
    );

    public required InterPersonalRelationTypeListItem InterPersonalRelationType { get; set; }
    public abstract string PersonFromName { get; }
    public abstract string PersonToName { get; }
    public abstract PersonItem? PersonItemFrom { get; }
    public abstract PersonItem? PersonItemTo { get; }

    public abstract InterPersonalRelation SwapFromAndTo();
    public abstract RelationSide RelationSideThisPerson { get; }
    public PersonListItem? PersonListItemFrom { get; set; }
    public PersonListItem? PersonListItemTo { get; set; }
    public abstract record IncompleteNewInterPersonalRelation : InterPersonalRelation, NewNode
    {

        private IncompleteNewInterPersonalRelation() { }

        public abstract CompletedInterPersonalRelation GetCompletedRelation(PersonListItem personListItem);
        public abstract record IncompleteNewInterPersonalFromRelation : IncompleteNewInterPersonalRelation
        {

            private IncompleteNewInterPersonalFromRelation() { }
            public required PersonListItem? PersonTo { get; set; }
            public override string PersonToName => PersonTo is null ? "" : PersonTo.Name;
            public override PersonItem? PersonItemTo => PersonTo;
            public override T Match<T>(
                Func<IncompleteNewInterPersonalFromRelation, T> incompleteNewInterPersonalFromRelation,
                Func<IncompleteNewInterPersonalToRelation, T> incompleteNewInterPersonalToRelation,
                Func<CompletedNewInterPersonalNewFromRelation, T> completedNewInterPersonalNewFromRelation,
                Func<CompletedNewInterPersonalNewToRelation, T> completedNewInterPersonalNewToRelation,
                Func<ResolvedInterPersonalRelation, T> resolvedInterPersonalRelation
            )
            {
                return incompleteNewInterPersonalFromRelation(this);
            }

            public sealed record NewInterPersonalExistingFromRelation : IncompleteNewInterPersonalFromRelation
            {
                public override T Match<T>(
                    Func<ExistingInterPersonalRelation, T> existingInterPersonalRelation,
                    Func<NewInterPersonalExistingRelation, T> newInterPersonalExistingRelation,
                    Func<CompletedNewInterPersonalNewFromRelation, T> completedNewInterPersonalNewFromRelation,
                    Func<CompletedNewInterPersonalNewToRelation, T> completedNewInterPersonalNewToRelation,
                    Func<NewInterPersonalExistingFromRelation, T> newInterPersonalExistingFromRelation,
                    Func<NewInterPersonalExistingToRelation, T> newInterPersonalExistingToRelation,
                    Func<NewInterPersonalNewFromRelation, T> newInterPersonalNewFromRelation,
                    Func<NewInterPersonalNewToRelation, T> newInterPersonalNewToRelation
                )
                {
                    return newInterPersonalExistingFromRelation(this);
                }

                public required PersonListItem PersonFrom { get; set; }
                public override string PersonFromName => PersonFrom.Name;
                public override PersonItem? PersonItemFrom => PersonFrom;
                public override NewInterPersonalExistingToRelation SwapFromAndTo()
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
                        TenantNodes = TenantNodes,
                        Tenants = Tenants,
                        Title = Title,
                        PersonListItemFrom = PersonListItemTo,
                        PersonListItemTo = PersonListItemFrom,
                    };
                }
                public override CompletedInterPersonalRelation GetCompletedRelation(PersonListItem personTo)
                {
                    return new NewInterPersonalExistingRelation {
                        PersonFrom = PersonFrom,
                        PersonTo = personTo,
                        InterPersonalRelationType = InterPersonalRelationType,
                        DateFrom = DateFrom,
                        DateTo = DateTo,
                        Description = Description,
                        ProofDocument = ProofDocument,
                        PersonListItemFrom = PersonListItemFrom,
                        PersonListItemTo = PersonListItemTo,
                        HasBeenDeleted = HasBeenDeleted,
                        Files = Files,
                        Tags = Tags,
                        TenantNodes = TenantNodes,
                        Tenants = Tenants,
                        NodeTypeName = NodeTypeName,
                        OwnerId = OwnerId,
                        PublisherId = PublisherId,
                        Title = Title,
                        SettableRelationSideThisPerson = RelationSide.From
                    };
                }

                public override RelationSide RelationSideThisPerson => RelationSide.From;

            }
            public sealed record NewInterPersonalNewFromRelation : IncompleteNewInterPersonalFromRelation
            {
                public override T Match<T>(
                    Func<ExistingInterPersonalRelation, T> existingInterPersonalRelation,
                    Func<NewInterPersonalExistingRelation, T> newInterPersonalExistingRelation,
                    Func<CompletedNewInterPersonalNewFromRelation, T> completedNewInterPersonalNewFromRelation,
                    Func<CompletedNewInterPersonalNewToRelation, T> completedNewInterPersonalNewToRelation,
                    Func<NewInterPersonalExistingFromRelation, T> newInterPersonalExistingFromRelation,
                    Func<NewInterPersonalExistingToRelation, T> newInterPersonalExistingToRelation,
                    Func<NewInterPersonalNewFromRelation, T> newInterPersonalNewFromRelation,
                    Func<NewInterPersonalNewToRelation, T> newInterPersonalNewToRelation
                )
                {
                    return newInterPersonalNewFromRelation(this);
                }
                public required PersonName PersonFrom { get; set; }
                public override string PersonFromName => PersonFrom.Name;
                public override PersonItem? PersonItemFrom => PersonFrom;
                public override NewInterPersonalNewToRelation SwapFromAndTo()
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
                        TenantNodes = TenantNodes,
                        Tenants = Tenants,
                        Title = Title,
                        PersonListItemFrom = PersonListItemTo,
                        PersonListItemTo = PersonListItemFrom,
                    };
                }
                public override CompletedInterPersonalRelation GetCompletedRelation(PersonListItem personTo)
                {
                    return new CompletedNewInterPersonalNewFromRelation {
                        PersonFrom = PersonFrom,
                        PersonTo = personTo,
                        InterPersonalRelationType = InterPersonalRelationType,
                        DateFrom = DateFrom,
                        DateTo = DateTo,
                        Description = Description,
                        ProofDocument = ProofDocument,
                        PersonListItemFrom = PersonListItemFrom,
                        PersonListItemTo = PersonListItemTo,
                        HasBeenDeleted = HasBeenDeleted,
                        Files = Files,
                        Tags = Tags,
                        TenantNodes = TenantNodes,
                        Tenants = Tenants,
                        NodeTypeName = NodeTypeName,
                        OwnerId = OwnerId,
                        PublisherId = PublisherId,
                        Title = Title,
                    };
                }

                public override RelationSide RelationSideThisPerson => RelationSide.From;
            }

        }
        public abstract record IncompleteNewInterPersonalToRelation : IncompleteNewInterPersonalRelation
        {

            private IncompleteNewInterPersonalToRelation() { }

            public PersonListItem? PersonFrom { get; set; }
            public override string PersonFromName => PersonFrom is null ? "" : PersonFrom.Name;
            public override PersonItem? PersonItemFrom => PersonFrom;
            public override T Match<T>(
                Func<IncompleteNewInterPersonalFromRelation, T> incompleteNewInterPersonalFromRelation,
                Func<IncompleteNewInterPersonalToRelation, T> incompleteNewInterPersonalToRelation,
                Func<CompletedNewInterPersonalNewFromRelation, T> completedNewInterPersonalNewFromRelation,
                Func<CompletedNewInterPersonalNewToRelation, T> completedNewInterPersonalNewToRelation,
                Func<ResolvedInterPersonalRelation, T> resolvedInterPersonalRelation
            )
            {
                return incompleteNewInterPersonalToRelation(this);
            }

            public sealed record NewInterPersonalExistingToRelation : IncompleteNewInterPersonalToRelation
            {
                public override T Match<T>(
                    Func<ExistingInterPersonalRelation, T> existingInterPersonalRelation,
                    Func<NewInterPersonalExistingRelation, T> newInterPersonalExistingRelation,
                    Func<CompletedNewInterPersonalNewFromRelation, T> completedNewInterPersonalNewFromRelation,
                    Func<CompletedNewInterPersonalNewToRelation, T> completedNewInterPersonalNewToRelation,
                    Func<NewInterPersonalExistingFromRelation, T> newInterPersonalExistingFromRelation,
                    Func<NewInterPersonalExistingToRelation, T> newInterPersonalExistingToRelation,
                    Func<NewInterPersonalNewFromRelation, T> newInterPersonalNewFromRelation,
                    Func<NewInterPersonalNewToRelation, T> newInterPersonalNewToRelation
                )
                {
                    return newInterPersonalExistingToRelation(this);
                }

                public required PersonListItem PersonTo { get; set; }
                public override string PersonToName => PersonTo.Name;
                public override PersonItem? PersonItemTo => PersonTo;
                public override NewInterPersonalExistingFromRelation SwapFromAndTo()
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
                        TenantNodes = TenantNodes,
                        Tenants = Tenants,
                        Title = Title,
                        PersonListItemFrom = PersonListItemTo,
                        PersonListItemTo = PersonListItemFrom,
                    };
                }
                public override CompletedInterPersonalRelation GetCompletedRelation(PersonListItem personFrom)
                {
                    return new NewInterPersonalExistingRelation {
                        PersonFrom = personFrom,
                        PersonTo = PersonTo,
                        InterPersonalRelationType = InterPersonalRelationType,
                        DateFrom = DateFrom,
                        DateTo = DateTo,
                        Description = Description,
                        ProofDocument = ProofDocument,
                        PersonListItemFrom = PersonListItemFrom,
                        PersonListItemTo = PersonListItemTo,
                        HasBeenDeleted = HasBeenDeleted,
                        Files = Files,
                        Tags = Tags,
                        TenantNodes = TenantNodes,
                        Tenants = Tenants,
                        NodeTypeName = NodeTypeName,
                        OwnerId = OwnerId,
                        PublisherId = PublisherId,
                        Title = Title,
                        SettableRelationSideThisPerson = RelationSide.To
                    };
                }
                public override RelationSide RelationSideThisPerson => RelationSide.To;
            }
            public sealed record NewInterPersonalNewToRelation : IncompleteNewInterPersonalToRelation
            {
                public override T Match<T>(
                    Func<ExistingInterPersonalRelation, T> existingInterPersonalRelation,
                    Func<NewInterPersonalExistingRelation, T> newInterPersonalExistingRelation,
                    Func<CompletedNewInterPersonalNewFromRelation, T> completedNewInterPersonalNewFromRelation,
                    Func<CompletedNewInterPersonalNewToRelation, T> completedNewInterPersonalNewToRelation,
                    Func<NewInterPersonalExistingFromRelation, T> newInterPersonalExistingFromRelation,
                    Func<NewInterPersonalExistingToRelation, T> newInterPersonalExistingToRelation,
                    Func<NewInterPersonalNewFromRelation, T> newInterPersonalNewFromRelation,
                    Func<NewInterPersonalNewToRelation, T> newInterPersonalNewToRelation
                )
                {
                    return newInterPersonalNewToRelation(this);
                }

                public required PersonName PersonTo { get; set; }
                public override string PersonToName => PersonTo.Name;
                public override PersonItem? PersonItemTo => PersonTo;
                public override NewInterPersonalNewFromRelation SwapFromAndTo()
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
                        TenantNodes = TenantNodes,
                        Tenants = Tenants,
                        Title = Title,
                        PersonListItemFrom = PersonListItemTo,
                        PersonListItemTo = PersonListItemFrom,
                    };
                }

                public override CompletedInterPersonalRelation GetCompletedRelation(PersonListItem personFrom)
                {
                    return new CompletedNewInterPersonalNewToRelation {
                        PersonFrom = personFrom,
                        PersonTo = PersonTo,
                        InterPersonalRelationType = InterPersonalRelationType,
                        DateFrom = DateFrom,
                        DateTo = DateTo,
                        Description = Description,
                        ProofDocument = ProofDocument,
                        PersonListItemFrom = PersonListItemFrom,
                        PersonListItemTo = PersonListItemTo,
                        HasBeenDeleted = HasBeenDeleted,
                        Files = Files,
                        Tags = Tags,
                        TenantNodes = TenantNodes,
                        Tenants = Tenants,
                        NodeTypeName = NodeTypeName,
                        OwnerId = OwnerId,
                        PublisherId = PublisherId,
                        Title = Title,
                    };
                }

                public override RelationSide RelationSideThisPerson => RelationSide.To;
            }
        }
    }
    public abstract record CompletedInterPersonalRelation : InterPersonalRelation
    {
        private CompletedInterPersonalRelation() { }
        public abstract record CompletedNewInterPersonalRelation : CompletedInterPersonalRelation, NewNode
        {
            private CompletedNewInterPersonalRelation() { }
            public sealed record CompletedNewInterPersonalNewFromRelation : CompletedNewInterPersonalRelation
            {
                public override T Match<T>(
                    Func<ExistingInterPersonalRelation, T> existingInterPersonalRelation,
                    Func<NewInterPersonalExistingRelation, T> newInterPersonalExistingRelation,
                    Func<CompletedNewInterPersonalNewFromRelation, T> completedNewInterPersonalNewFromRelation,
                    Func<CompletedNewInterPersonalNewToRelation, T> completedNewInterPersonalNewToRelation,
                    Func<NewInterPersonalExistingFromRelation, T> newInterPersonalExistingFromRelation,
                    Func<NewInterPersonalExistingToRelation, T> newInterPersonalExistingToRelation,
                    Func<NewInterPersonalNewFromRelation, T> newInterPersonalNewFromRelation,
                    Func<NewInterPersonalNewToRelation, T> newInterPersonalNewToRelation
                )
                {
                    return completedNewInterPersonalNewFromRelation(this);
                }
                public override T Match<T>(
                    Func<IncompleteNewInterPersonalFromRelation, T> incompleteNewInterPersonalFromRelation,
                    Func<IncompleteNewInterPersonalToRelation, T> incompleteNewInterPersonalToRelation,
                    Func<CompletedNewInterPersonalNewFromRelation, T> completedNewInterPersonalNewFromRelation,
                    Func<CompletedNewInterPersonalNewToRelation, T> completedNewInterPersonalNewToRelation,
                    Func<ResolvedInterPersonalRelation, T> resolvedInterPersonalRelation
                )
                {
                    return completedNewInterPersonalNewFromRelation(this);
                }

                public required PersonName PersonFrom { get; set; }
                public required PersonListItem PersonTo { get; set; }
                public override string PersonFromName => PersonFrom.Name;
                public override string PersonToName => PersonTo.Name;
                public override PersonItem? PersonItemFrom => PersonFrom;
                public override PersonItem? PersonItemTo => PersonTo;

                public override CompletedNewInterPersonalNewToRelation SwapFromAndTo()
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
                        TenantNodes = TenantNodes,
                        Tenants = Tenants,
                        Title = Title,
                        PersonListItemFrom = PersonListItemTo,
                        PersonListItemTo = PersonListItemFrom,
                    };
                }
                public override RelationSide RelationSideThisPerson => RelationSide.From;
            }
            public sealed record CompletedNewInterPersonalNewToRelation : CompletedNewInterPersonalRelation
            {
                public override T Match<T>(
                    Func<ExistingInterPersonalRelation, T> existingInterPersonalRelation,
                    Func<NewInterPersonalExistingRelation, T> newInterPersonalExistingRelation,
                    Func<CompletedNewInterPersonalNewFromRelation, T> completedNewInterPersonalNewFromRelation,
                    Func<CompletedNewInterPersonalNewToRelation, T> completedNewInterPersonalNewToRelation,
                    Func<NewInterPersonalExistingFromRelation, T> newInterPersonalExistingFromRelation,
                    Func<NewInterPersonalExistingToRelation, T> newInterPersonalExistingToRelation,
                    Func<NewInterPersonalNewFromRelation, T> newInterPersonalNewFromRelation,
                    Func<NewInterPersonalNewToRelation, T> newInterPersonalNewToRelation
                )
                {
                    return completedNewInterPersonalNewToRelation(this);
                }
                public override T Match<T>(
                    Func<IncompleteNewInterPersonalFromRelation, T> incompleteNewInterPersonalFromRelation,
                    Func<IncompleteNewInterPersonalToRelation, T> incompleteNewInterPersonalToRelation,
                    Func<CompletedNewInterPersonalNewFromRelation, T> completedNewInterPersonalNewFromRelation,
                    Func<CompletedNewInterPersonalNewToRelation, T> completedNewInterPersonalNewToRelation,
                    Func<ResolvedInterPersonalRelation, T> resolvedInterPersonalRelation
                )
                {
                    return completedNewInterPersonalNewToRelation(this);
                }

                public required PersonListItem PersonFrom { get; set; }
                public required PersonName PersonTo { get; set; }
                public override string PersonFromName => PersonFrom.Name;
                public override string PersonToName => PersonTo.Name;
                public override PersonItem? PersonItemFrom => PersonFrom;
                public override PersonItem? PersonItemTo => PersonTo;
                public override CompletedNewInterPersonalNewFromRelation SwapFromAndTo()
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
                        TenantNodes = TenantNodes,
                        Tenants = Tenants,
                        Title = Title,
                        PersonListItemFrom = PersonListItemTo,
                        PersonListItemTo = PersonListItemFrom,
                    };
                }

                public override RelationSide RelationSideThisPerson => RelationSide.To;
            }
        }
        public abstract record ResolvedInterPersonalRelation : CompletedInterPersonalRelation
        {
            private ResolvedInterPersonalRelation() { }
            public required PersonListItem PersonFrom { get; set; }
            public required PersonListItem PersonTo { get; set; }
            public override string PersonFromName => PersonFrom.Name;
            public override string PersonToName => PersonTo.Name;
            public override PersonItem? PersonItemFrom => PersonFrom;
            public override PersonItem? PersonItemTo => PersonTo;
            public override T Match<T>(
                Func<IncompleteNewInterPersonalFromRelation, T> incompleteNewInterPersonalFromRelation,
                Func<IncompleteNewInterPersonalToRelation, T> incompleteNewInterPersonalToRelation,
                Func<CompletedNewInterPersonalNewFromRelation, T> completedNewInterPersonalNewFromRelation,
                Func<CompletedNewInterPersonalNewToRelation, T> completedNewInterPersonalNewToRelation,
                Func<ResolvedInterPersonalRelation, T> resolvedInterPersonalRelation
            )
            {
                return resolvedInterPersonalRelation(this);
            }

            public sealed record ExistingInterPersonalRelation : ResolvedInterPersonalRelation, ExistingNode
            {
                public override T Match<T>(
                    Func<ExistingInterPersonalRelation, T> existingInterPersonalRelation,
                    Func<NewInterPersonalExistingRelation, T> newInterPersonalExistingRelation,
                    Func<CompletedNewInterPersonalNewFromRelation, T> completedNewInterPersonalNewFromRelation,
                    Func<CompletedNewInterPersonalNewToRelation, T> completedNewInterPersonalNewToRelation,
                    Func<NewInterPersonalExistingFromRelation, T> newInterPersonalExistingFromRelation,
                    Func<NewInterPersonalExistingToRelation, T> newInterPersonalExistingToRelation,
                    Func<NewInterPersonalNewFromRelation, T> newInterPersonalNewFromRelation,
                    Func<NewInterPersonalNewToRelation, T> newInterPersonalNewToRelation
                )
                {
                    return existingInterPersonalRelation(this);
                }

                public int NodeId { get; init; }
                public int UrlId { get; set; }
                public override ExistingInterPersonalRelation SwapFromAndTo()
                {
                    (PersonTo, PersonFrom) = (PersonFrom, PersonTo);
                    (PersonListItemTo, PersonListItemFrom) = (PersonListItemFrom, PersonListItemTo);

                    if (SettableRelationSideThisPerson == RelationSide.To) {
                        SettableRelationSideThisPerson = RelationSide.From;
                    }
                    else {
                        SettableRelationSideThisPerson = RelationSide.To;
                    }
                    return this;
                }

                public override RelationSide RelationSideThisPerson => SettableRelationSideThisPerson;

                public required RelationSide SettableRelationSideThisPerson { get; set; }
            }

            public sealed record NewInterPersonalExistingRelation : ResolvedInterPersonalRelation
            {
                public override T Match<T>(
                    Func<ExistingInterPersonalRelation, T> existingInterPersonalRelation,
                    Func<NewInterPersonalExistingRelation, T> newInterPersonalExistingRelation,
                    Func<CompletedNewInterPersonalNewFromRelation, T> completedNewInterPersonalNewFromRelation,
                    Func<CompletedNewInterPersonalNewToRelation, T> completedNewInterPersonalNewToRelation,
                    Func<NewInterPersonalExistingFromRelation, T> newInterPersonalExistingFromRelation,
                    Func<NewInterPersonalExistingToRelation, T> newInterPersonalExistingToRelation,
                    Func<NewInterPersonalNewFromRelation, T> newInterPersonalNewFromRelation,
                    Func<NewInterPersonalNewToRelation, T> newInterPersonalNewToRelation
                )
                {
                    return newInterPersonalExistingRelation(this);
                }

                public override NewInterPersonalExistingRelation SwapFromAndTo()
                {
                    (PersonTo, PersonFrom) = (PersonFrom, PersonTo);
                    (PersonListItemTo, PersonListItemFrom) = (PersonListItemFrom, PersonListItemTo);
                    if (SettableRelationSideThisPerson == RelationSide.To) {
                        SettableRelationSideThisPerson = RelationSide.From;
                    }
                    else {
                        SettableRelationSideThisPerson = RelationSide.To;
                    }
                    return this;
                }
                public override RelationSide RelationSideThisPerson => SettableRelationSideThisPerson;

                public required RelationSide SettableRelationSideThisPerson { get; set; }
            }

        }
    }
}

