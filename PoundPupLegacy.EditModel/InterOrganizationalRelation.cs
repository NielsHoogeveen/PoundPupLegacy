using static PoundPupLegacy.EditModel.OrganizationItem;
using static PoundPupLegacy.EditModel.InterOrganizationalRelation;

namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(ExistingInterOrganizationalRelation))]
public partial class ExistingInterOrganizationalRelationJsonContext : JsonSerializerContext { }

public abstract record InterOrganizationalRelation : RelationBase
{
    private InterOrganizationalRelation()
    {
    }

    [RequireNamedArgs]
    public abstract T Match<T>(
        Func<ExistingInterOrganizationalRelation, T> existingInterOrganizationalRelation,
        Func<NewInterOrganizationalExistingRelation, T> newInterOrganizationalExistingRelation,
        Func<NewInterOrganizationalExistingFromRelation, T> newInterOrganizationalExistingFromRelation,
        Func<NewInterOrganizationalExistingToRelation, T> newInterOrganizationalExistingToRelation,
        Func<CompletedNewInterOrganizationalNewFromRelation, T> completedNewInterOrganizationalNewFromRelation,
        Func<NewInterOrganizationalNewFromRelation, T> newInterOrganizationalNewFromRelation,
        Func<CompletedNewInterOrganizationalNewToRelation, T> completedNewInterOrganizationalNewToRelation,
        Func<NewInterOrganizationalNewToRelation, T> newInterOrganizationalNewToRelation
    );

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
    public abstract InterOrganizationalRelation SwapFromAndTo();
    public abstract RelationSide RelationSideThisOrganization { get; }

    public abstract record CompletedInterOrganizationalRelation : InterOrganizationalRelation
    {
    }

    public abstract record CompletedNewInterOrganizationalRelation : CompletedInterOrganizationalRelation, NewNode
    {
    }
    public abstract record ResolvedInterOrganizationalRelation : CompletedInterOrganizationalRelation
    {
    }
    public sealed record ExistingInterOrganizationalRelation : ResolvedInterOrganizationalRelation, ExistingNode
    {
        public required OrganizationListItem OrganizationFrom { get; set; }
        public required OrganizationListItem OrganizationTo { get; set; }
        public int NodeId { get; init; }
        public int UrlId { get; set; }
        [JsonIgnore]
        public override string OrganizationFromName => OrganizationFrom.Name;
        [JsonIgnore]
        public override string OrganizationToName => OrganizationTo.Name;
        [JsonIgnore]
        public override OrganizationItem? OrganizationItemFrom => OrganizationFrom;
        [JsonIgnore]
        public override OrganizationItem? OrganizationItemTo => OrganizationTo;
        public override ExistingInterOrganizationalRelation SwapFromAndTo()
        {
            (OrganizationTo, OrganizationFrom) = (OrganizationFrom, OrganizationTo);
            (OrganizationListItemTo, OrganizationListItemFrom) = (OrganizationListItemFrom, OrganizationListItemTo);
            if (SettableRelationSideThisOrganization == RelationSide.To) {
                SettableRelationSideThisOrganization = RelationSide.From;
            }
            else {
                SettableRelationSideThisOrganization = RelationSide.To;
            }
            return this;
        }

        public override RelationSide RelationSideThisOrganization => SettableRelationSideThisOrganization;
        public required RelationSide SettableRelationSideThisOrganization { get; set; }

        public override T Match<T>(
            Func<ExistingInterOrganizationalRelation, T> existingInterOrganizationalRelation,
            Func<NewInterOrganizationalExistingRelation, T> newInterOrganizationalExistingRelation,
            Func<NewInterOrganizationalExistingFromRelation, T> newInterOrganizationalExistingFromRelation,
            Func<NewInterOrganizationalExistingToRelation, T> newInterOrganizationalExistingToRelation,
            Func<CompletedNewInterOrganizationalNewFromRelation, T> completedNewInterOrganizationalNewFromRelation,
            Func<NewInterOrganizationalNewFromRelation, T> newInterOrganizationalNewFromRelation,
            Func<CompletedNewInterOrganizationalNewToRelation, T> completedNewInterOrganizationalNewToRelation,
            Func<NewInterOrganizationalNewToRelation, T> newInterOrganizationalNewToRelation
        )
        {
            return existingInterOrganizationalRelation(this);
        }

    }

    public sealed record NewInterOrganizationalExistingRelation : ResolvedInterOrganizationalRelation
    {
        public required OrganizationListItem OrganizationFrom { get; set; }
        public required OrganizationListItem OrganizationTo { get; set; }
        public override string OrganizationFromName => OrganizationFrom.Name;
        public override string OrganizationToName => OrganizationTo.Name;
        public override OrganizationItem? OrganizationItemFrom => OrganizationFrom;
        public override OrganizationItem? OrganizationItemTo => OrganizationTo;

        public override NewInterOrganizationalExistingRelation SwapFromAndTo()
        {
            (OrganizationTo, OrganizationFrom) = (OrganizationFrom, OrganizationTo);
            (OrganizationListItemTo, OrganizationListItemFrom) = (OrganizationListItemFrom, OrganizationListItemTo);
            if (SettableRelationSideThisOrganization == RelationSide.To) {
                SettableRelationSideThisOrganization = RelationSide.From;
            }
            else {
                SettableRelationSideThisOrganization = RelationSide.To;
            }
            return this;
        }
        public override RelationSide RelationSideThisOrganization => SettableRelationSideThisOrganization;

        public required RelationSide SettableRelationSideThisOrganization { get; set; }
        public override T Match<T>(
            Func<ExistingInterOrganizationalRelation, T> existingInterOrganizationalRelation,
            Func<NewInterOrganizationalExistingRelation, T> newInterOrganizationalExistingRelation,
            Func<NewInterOrganizationalExistingFromRelation, T> newInterOrganizationalExistingFromRelation,
            Func<NewInterOrganizationalExistingToRelation, T> newInterOrganizationalExistingToRelation,
            Func<CompletedNewInterOrganizationalNewFromRelation, T> completedNewInterOrganizationalNewFromRelation,
            Func<NewInterOrganizationalNewFromRelation, T> newInterOrganizationalNewFromRelation,
            Func<CompletedNewInterOrganizationalNewToRelation, T> completedNewInterOrganizationalNewToRelation,
            Func<NewInterOrganizationalNewToRelation, T> newInterOrganizationalNewToRelation
        )
        {
            return newInterOrganizationalExistingRelation(this);
        }

    }

    public sealed record NewInterOrganizationalExistingFromRelation : InterOrganizationalRelation
    {
        public required OrganizationListItem OrganizationFrom { get; set; }
        public required OrganizationListItem? OrganizationTo { get; set; }
        public override string OrganizationFromName => OrganizationFrom.Name;
        public override string OrganizationToName => OrganizationTo is null ? "" : OrganizationTo.Name;
        public override OrganizationItem? OrganizationItemFrom => OrganizationFrom;
        public override OrganizationItem? OrganizationItemTo => OrganizationTo;
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

        public CompletedInterOrganizationalRelation GetCompletedRelation(OrganizationListItem organizationListItemTo)
        {
            return new NewInterOrganizationalExistingRelation {
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
                SettableRelationSideThisOrganization = RelationSide.To
            };
        }

        public override RelationSide RelationSideThisOrganization => RelationSide.From;

        public override T Match<T>(
            Func<ExistingInterOrganizationalRelation, T> existingInterOrganizationalRelation,
            Func<NewInterOrganizationalExistingRelation, T> newInterOrganizationalExistingRelation,
            Func<NewInterOrganizationalExistingFromRelation, T> newInterOrganizationalExistingFromRelation,
            Func<NewInterOrganizationalExistingToRelation, T> newInterOrganizationalExistingToRelation,
            Func<CompletedNewInterOrganizationalNewFromRelation, T> completedNewInterOrganizationalNewFromRelation,
            Func<NewInterOrganizationalNewFromRelation, T> newInterOrganizationalNewFromRelation,
            Func<CompletedNewInterOrganizationalNewToRelation, T> completedNewInterOrganizationalNewToRelation,
            Func<NewInterOrganizationalNewToRelation, T> newInterOrganizationalNewToRelation
        )
        {
            return newInterOrganizationalExistingFromRelation(this);
        }


    }
    public sealed record NewInterOrganizationalExistingToRelation : InterOrganizationalRelation
    {
        public required OrganizationListItem? OrganizationFrom { get; set; }
        public required OrganizationListItem OrganizationTo { get; set; }
        public override string OrganizationFromName => OrganizationFrom is null ? "" : OrganizationFrom.Name;
        public override string OrganizationToName => OrganizationTo.Name;
        public override OrganizationItem? OrganizationItemFrom => OrganizationFrom;
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

        public CompletedInterOrganizationalRelation GetCompletedRelation(OrganizationListItem organizationListItemFrom)
        {
            return new NewInterOrganizationalExistingRelation {
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
                SettableRelationSideThisOrganization = RelationSide.To
            };
        }
        public override T Match<T>(
            Func<ExistingInterOrganizationalRelation, T> existingInterOrganizationalRelation,
            Func<NewInterOrganizationalExistingRelation, T> newInterOrganizationalExistingRelation,
            Func<NewInterOrganizationalExistingFromRelation, T> newInterOrganizationalExistingFromRelation,
            Func<NewInterOrganizationalExistingToRelation, T> newInterOrganizationalExistingToRelation,
            Func<CompletedNewInterOrganizationalNewFromRelation, T> completedNewInterOrganizationalNewFromRelation,
            Func<NewInterOrganizationalNewFromRelation, T> newInterOrganizationalNewFromRelation,
            Func<CompletedNewInterOrganizationalNewToRelation, T> completedNewInterOrganizationalNewToRelation,
            Func<NewInterOrganizationalNewToRelation, T> newInterOrganizationalNewToRelation
        )
        {
            return newInterOrganizationalExistingToRelation(this);
        }

        public override RelationSide RelationSideThisOrganization => RelationSide.To;
    }

    public sealed record CompletedNewInterOrganizationalNewFromRelation : CompletedNewInterOrganizationalRelation
    {
        public required OrganizationName OrganizationFrom { get; set; }
        public required OrganizationListItem OrganizationTo { get; set; }
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
        public override T Match<T>(
            Func<ExistingInterOrganizationalRelation, T> existingInterOrganizationalRelation,
            Func<NewInterOrganizationalExistingRelation, T> newInterOrganizationalExistingRelation,
            Func<NewInterOrganizationalExistingFromRelation, T> newInterOrganizationalExistingFromRelation,
            Func<NewInterOrganizationalExistingToRelation, T> newInterOrganizationalExistingToRelation,
            Func<CompletedNewInterOrganizationalNewFromRelation, T> completedNewInterOrganizationalNewFromRelation,
            Func<NewInterOrganizationalNewFromRelation, T> newInterOrganizationalNewFromRelation,
            Func<CompletedNewInterOrganizationalNewToRelation, T> completedNewInterOrganizationalNewToRelation,
            Func<NewInterOrganizationalNewToRelation, T> newInterOrganizationalNewToRelation
        )
        {
            return completedNewInterOrganizationalNewFromRelation(this);
        }

        public override RelationSide RelationSideThisOrganization => RelationSide.From;
    }
    public sealed record NewInterOrganizationalNewFromRelation : InterOrganizationalRelation
    {
        public required OrganizationName OrganizationFrom { get; set; }
        public required OrganizationListItem? OrganizationTo { get; set; }
        public override string OrganizationFromName => OrganizationFrom.Name;
        public override string OrganizationToName => OrganizationTo is null ? "" : OrganizationTo.Name;
        public override OrganizationItem? OrganizationItemFrom => OrganizationFrom;
        public override OrganizationItem? OrganizationItemTo => OrganizationTo;
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
        public CompletedInterOrganizationalRelation GetCompletedRelation(OrganizationListItem organizationListItemTo)
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

        public override RelationSide RelationSideThisOrganization => RelationSide.From;
        public override T Match<T>(
            Func<ExistingInterOrganizationalRelation, T> existingInterOrganizationalRelation,
            Func<NewInterOrganizationalExistingRelation, T> newInterOrganizationalExistingRelation,
            Func<NewInterOrganizationalExistingFromRelation, T> newInterOrganizationalExistingFromRelation,
            Func<NewInterOrganizationalExistingToRelation, T> newInterOrganizationalExistingToRelation,
            Func<CompletedNewInterOrganizationalNewFromRelation, T> completedNewInterOrganizationalNewFromRelation,
            Func<NewInterOrganizationalNewFromRelation, T> newInterOrganizationalNewFromRelation,
            Func<CompletedNewInterOrganizationalNewToRelation, T> completedNewInterOrganizationalNewToRelation,
            Func<NewInterOrganizationalNewToRelation, T> newInterOrganizationalNewToRelation
        )
        {
            return newInterOrganizationalNewFromRelation(this);
        }

    }
    public sealed record CompletedNewInterOrganizationalNewToRelation : CompletedNewInterOrganizationalRelation
    {
        public required OrganizationListItem OrganizationFrom { get; set; }
        public required OrganizationName OrganizationTo { get; set; }
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
        public override RelationSide RelationSideThisOrganization => RelationSide.To;
        public override T Match<T>(
            Func<ExistingInterOrganizationalRelation, T> existingInterOrganizationalRelation,
            Func<NewInterOrganizationalExistingRelation, T> newInterOrganizationalExistingRelation,
            Func<NewInterOrganizationalExistingFromRelation, T> newInterOrganizationalExistingFromRelation,
            Func<NewInterOrganizationalExistingToRelation, T> newInterOrganizationalExistingToRelation,
            Func<CompletedNewInterOrganizationalNewFromRelation, T> completedNewInterOrganizationalNewFromRelation,
            Func<NewInterOrganizationalNewFromRelation, T> newInterOrganizationalNewFromRelation,
            Func<CompletedNewInterOrganizationalNewToRelation, T> completedNewInterOrganizationalNewToRelation,
            Func<NewInterOrganizationalNewToRelation, T> newInterOrganizationalNewToRelation
        )
        {
            return completedNewInterOrganizationalNewToRelation(this);
        }

    }
    public sealed record NewInterOrganizationalNewToRelation : InterOrganizationalRelation
    {
        public required OrganizationListItem? OrganizationFrom { get; set; }
        public required OrganizationName OrganizationTo { get; set; }
        public override string OrganizationFromName => OrganizationFrom is null ? "" : OrganizationFrom.Name;
        public override string OrganizationToName => OrganizationTo.Name;
        public override OrganizationItem? OrganizationItemFrom => OrganizationFrom;
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
        public CompletedInterOrganizationalRelation GetCompletedRelation(OrganizationListItem organizationListItemFrom)
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
        public override RelationSide RelationSideThisOrganization => RelationSide.To;
        public override T Match<T>(
            Func<ExistingInterOrganizationalRelation, T> existingInterOrganizationalRelation,
            Func<NewInterOrganizationalExistingRelation, T> newInterOrganizationalExistingRelation,
            Func<NewInterOrganizationalExistingFromRelation, T> newInterOrganizationalExistingFromRelation,
            Func<NewInterOrganizationalExistingToRelation, T> newInterOrganizationalExistingToRelation,
            Func<CompletedNewInterOrganizationalNewFromRelation, T> completedNewInterOrganizationalNewFromRelation,
            Func<NewInterOrganizationalNewFromRelation, T> newInterOrganizationalNewFromRelation,
            Func<CompletedNewInterOrganizationalNewToRelation, T> completedNewInterOrganizationalNewToRelation,
            Func<NewInterOrganizationalNewToRelation, T> newInterOrganizationalNewToRelation
        )
        {
            return newInterOrganizationalNewToRelation(this);
        }

    }
}

