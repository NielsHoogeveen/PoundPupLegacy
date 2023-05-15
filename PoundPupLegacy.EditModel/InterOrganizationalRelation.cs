namespace PoundPupLegacy.EditModel;

public interface InterOrganizationalRelation : Relation
{
    InterOrganizationalRelationTypeListItem InterOrganizationalRelationType { get; }
    decimal? MoneyInvolved { get; set; }
    int? NumberOfChildrenInvolved { get; set; }
    GeographicalEntityListItem? GeographicalEntity { get; set; }
    string OrganizationFromName { get; }
    string OrganizationToName { get; }
    PartyItem.OrganizationItem? OrganizationItemFrom { get; }
    PartyItem.OrganizationItem? OrganizationItemTo { get; }
    PartyItem.OrganizationListItem? OrganizationListItemFrom { get; set; }
    PartyItem.OrganizationListItem? OrganizationListItemTo { get; set; }

    InterOrganizationalRelation SwapFromAndTo();
    RelationSide RelationSideThisOrganization { get; }
}

public interface CompletedInterOrganizationalRelation: InterOrganizationalRelation
{
}

public interface NewInterOrganizationalRelation: InterOrganizationalRelation, NewNode
{

}
public interface CompletedNewInterOrganizationalRelation : NewInterOrganizationalRelation, CompletedInterOrganizationalRelation
{
}
public interface IncompleteNewInterOrganizationalRelation : NewInterOrganizationalRelation
{
    public CompletedInterOrganizationalRelation GetCompletedRelation(PartyItem.OrganizationListItem organizationListItem);
}
public interface ResolvedInterOrganizationalRelation : CompletedInterOrganizationalRelation
{
}

public abstract record InterOrganizationalRelationBase : RelationBase, InterOrganizationalRelation
{
    public required InterOrganizationalRelationTypeListItem InterOrganizationalRelationType { get; set; }
    public decimal? MoneyInvolved { get; set; }
    public int? NumberOfChildrenInvolved { get; set; }
    public required GeographicalEntityListItem? GeographicalEntity { get; set; }
    public PartyItem.OrganizationListItem? OrganizationListItemFrom { get; set; }
    public PartyItem.OrganizationListItem? OrganizationListItemTo { get; set; }
    public abstract string OrganizationFromName { get; }
    public abstract string OrganizationToName { get; }
    public abstract PartyItem.OrganizationItem? OrganizationItemFrom { get; }
    public abstract PartyItem.OrganizationItem? OrganizationItemTo { get; }
    public abstract InterOrganizationalRelation SwapFromAndTo();
    public abstract RelationSide RelationSideThisOrganization { get; }
}

[JsonSerializable(typeof(ExistingInterOrganizationalRelation))]
public partial class ExistingInterOrganizationalRelationJsonContext : JsonSerializerContext { }

public record ExistingInterOrganizationalRelation : InterOrganizationalRelationBase, ExistingNode, ResolvedInterOrganizationalRelation
{
    public required PartyItem.OrganizationListItem OrganizationFrom { get; set; }
    public required PartyItem.OrganizationListItem OrganizationTo { get; set; }
    public int NodeId { get; init; } 
    public int UrlId { get; set; }
    [JsonIgnore]
    public override string OrganizationFromName => OrganizationFrom.Name;
    [JsonIgnore]
    public override string OrganizationToName => OrganizationTo.Name;
    [JsonIgnore]
    public override PartyItem.OrganizationItem? OrganizationItemFrom => OrganizationFrom;
    [JsonIgnore]
    public override PartyItem.OrganizationItem? OrganizationItemTo => OrganizationTo;
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
}

public record NewInterOrganizationalExistingRelation : InterOrganizationalRelationBase, ResolvedInterOrganizationalRelation
{
    public required PartyItem.OrganizationListItem OrganizationFrom { get; set; }
    public required PartyItem.OrganizationListItem OrganizationTo { get; set; }
    public override string OrganizationFromName => OrganizationFrom.Name;
    public override string OrganizationToName => OrganizationTo.Name;
    public override PartyItem.OrganizationItem? OrganizationItemFrom => OrganizationFrom;
    public override PartyItem.OrganizationItem? OrganizationItemTo => OrganizationTo;

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
}

public record NewInterOrganizationalExistingFromRelation : InterOrganizationalRelationBase, InterOrganizationalRelation, IncompleteNewInterOrganizationalRelation
{
    public required PartyItem.OrganizationListItem OrganizationFrom { get; set; }
    public required PartyItem.OrganizationListItem? OrganizationTo { get; set; }
    public override string OrganizationFromName => OrganizationFrom.Name;
    public override string OrganizationToName => OrganizationTo is null ? "" : OrganizationTo.Name;
    public override PartyItem.OrganizationItem? OrganizationItemFrom => OrganizationFrom;
    public override PartyItem.OrganizationItem? OrganizationItemTo => OrganizationTo;
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
            Files   = Files,
            HasBeenDeleted  = HasBeenDeleted,
            OwnerId = OwnerId,
            PublisherId = PublisherId,
            Tags = Tags,
            TenantNodes = TenantNodes,
            Tenants = Tenants,
            Title  = Title,
            OrganizationListItemFrom = OrganizationListItemTo,
            OrganizationListItemTo = OrganizationListItemFrom,
        };
    }

    public CompletedInterOrganizationalRelation GetCompletedRelation(PartyItem.OrganizationListItem organizationListItemTo)
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

}
public record NewInterOrganizationalExistingToRelation : InterOrganizationalRelationBase, InterOrganizationalRelation, IncompleteNewInterOrganizationalRelation
{
    public required PartyItem.OrganizationListItem? OrganizationFrom { get; set; }
    public required PartyItem.OrganizationListItem OrganizationTo { get; set; }
    public override string OrganizationFromName => OrganizationFrom is null ? "" : OrganizationFrom.Name;
    public override string OrganizationToName => OrganizationTo.Name;
    public override PartyItem.OrganizationItem? OrganizationItemFrom => OrganizationFrom;
    public override PartyItem.OrganizationItem? OrganizationItemTo => OrganizationTo;
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

    public CompletedInterOrganizationalRelation GetCompletedRelation(PartyItem.OrganizationListItem organizationListItemFrom)
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

    public override RelationSide RelationSideThisOrganization => RelationSide.To;
}

public record CompletedNewInterOrganizationalNewFromRelation : InterOrganizationalRelationBase, CompletedInterOrganizationalRelation, CompletedNewInterOrganizationalRelation
{
    public required PartyItem.OrganizationName OrganizationFrom { get; set; }
    public required PartyItem.OrganizationListItem OrganizationTo { get; set; }
    public override string OrganizationFromName => OrganizationFrom.Name;
    public override string OrganizationToName => OrganizationTo.Name;
    public override PartyItem.OrganizationItem? OrganizationItemFrom => OrganizationFrom;
    public override PartyItem.OrganizationItem? OrganizationItemTo => OrganizationTo;

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
    public override RelationSide RelationSideThisOrganization => RelationSide.From;
}
public record NewInterOrganizationalNewFromRelation : InterOrganizationalRelationBase, InterOrganizationalRelation, IncompleteNewInterOrganizationalRelation
{
    public required PartyItem.OrganizationName OrganizationFrom { get; set; }
    public required PartyItem.OrganizationListItem? OrganizationTo { get; set; }
    public override string OrganizationFromName => OrganizationFrom.Name;
    public override string OrganizationToName => OrganizationTo is null? "": OrganizationTo.Name;
    public override PartyItem.OrganizationItem? OrganizationItemFrom => OrganizationFrom;
    public override PartyItem.OrganizationItem? OrganizationItemTo => OrganizationTo;
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
    public CompletedInterOrganizationalRelation GetCompletedRelation(PartyItem.OrganizationListItem organizationListItemTo)
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
}
public record CompletedNewInterOrganizationalNewToRelation : InterOrganizationalRelationBase, CompletedInterOrganizationalRelation, CompletedNewInterOrganizationalRelation
{
    public required PartyItem.OrganizationListItem OrganizationFrom { get; set; }
    public required PartyItem.OrganizationName OrganizationTo { get; set; }
    public override string OrganizationFromName => OrganizationFrom.Name;
    public override string OrganizationToName => OrganizationTo.Name;
    public override PartyItem.OrganizationItem? OrganizationItemFrom => OrganizationFrom;
    public override PartyItem.OrganizationItem? OrganizationItemTo => OrganizationTo;
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
}
public record NewInterOrganizationalNewToRelation : InterOrganizationalRelationBase, InterOrganizationalRelation, IncompleteNewInterOrganizationalRelation
{
    public required PartyItem.OrganizationListItem? OrganizationFrom { get; set; }
    public required PartyItem.OrganizationName OrganizationTo { get; set; }
    public override string OrganizationFromName => OrganizationFrom is null ? "" : OrganizationFrom.Name;
    public override string OrganizationToName => OrganizationTo.Name;
    public override PartyItem.OrganizationItem? OrganizationItemFrom => OrganizationFrom;
    public override PartyItem.OrganizationItem? OrganizationItemTo => OrganizationTo;

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
    public CompletedInterOrganizationalRelation GetCompletedRelation(PartyItem.OrganizationListItem organizationListItemFrom)
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
}
