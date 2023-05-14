namespace PoundPupLegacy.EditModel;

public enum RelationSide
{
    From,
    To
}

public interface InterOrganizationalRelation : Relation
{
    InterOrganizationalRelationTypeListItem InterOrganizationalRelationType { get; }
    decimal? MoneyInvolved { get; set; }
    int? NumberOfChildrenInvolved { get; set; }
    GeographicalEntityListItem? GeographicalEntity { get; set; }
    string OrganizationFromName { get; }
    string OrganizationToName { get; }
    OrganizationItem? OrganizationItemFrom { get; }
    OrganizationItem? OrganizationItemTo { get; }
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
public interface ResolvedInterOrganizationalRelation : CompletedInterOrganizationalRelation
{
}

public abstract record InterOrganizationalRelationBase : RelationBase, InterOrganizationalRelation
{
    public required InterOrganizationalRelationTypeListItem InterOrganizationalRelationType { get; set; }
    public decimal? MoneyInvolved { get; set; }
    public int? NumberOfChildrenInvolved { get; set; }
    public required GeographicalEntityListItem? GeographicalEntity { get; set; }
    public abstract string OrganizationFromName { get; }
    public abstract string OrganizationToName { get; }
    public abstract OrganizationItem? OrganizationItemFrom { get; }
    public abstract OrganizationItem? OrganizationItemTo { get; }

    public abstract InterOrganizationalRelation SwapFromAndTo();
    public abstract RelationSide RelationSideThisOrganization { get; }
}

[JsonSerializable(typeof(ExistingInterOrganizationalRelation))]
public partial class ExistingInterOrganizationalRelationJsonContext : JsonSerializerContext { }

public record ExistingInterOrganizationalRelation : InterOrganizationalRelationBase, ExistingNode, ResolvedInterOrganizationalRelation
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
        var tmp = OrganizationFrom;
        OrganizationFrom = OrganizationTo;
        OrganizationTo = tmp;
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
    public required OrganizationListItem OrganizationFrom { get; set; }
    public required OrganizationListItem OrganizationTo { get; set; }
    public override string OrganizationFromName => OrganizationFrom.Name;
    public override string OrganizationToName => OrganizationTo.Name;
    public override OrganizationItem? OrganizationItemFrom => OrganizationFrom;
    public override OrganizationItem? OrganizationItemTo => OrganizationTo;

    public override NewInterOrganizationalExistingRelation SwapFromAndTo()
    {
        var tmp = OrganizationFrom;
        OrganizationFrom = OrganizationTo;
        OrganizationTo = tmp;
        if(SettableRelationSideThisOrganization == RelationSide.To) {
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

public record NewInterOrganizationalExistingFromRelation : InterOrganizationalRelationBase, InterOrganizationalRelation, NewInterOrganizationalRelation
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
            OrganizationFrom = this.OrganizationTo,
            OrganizationTo = this.OrganizationFrom,
            DateFrom = this.DateFrom,
            DateTo = this.DateTo,
            Description = this.Description,
            GeographicalEntity = this.GeographicalEntity,
            InterOrganizationalRelationType = this.InterOrganizationalRelationType,
            MoneyInvolved = this.MoneyInvolved,
            NumberOfChildrenInvolved = this.NumberOfChildrenInvolved,
            ProofDocument = this.ProofDocument,
            NodeTypeName = this.NodeTypeName,
            Files   = this.Files,
            HasBeenDeleted  = this.HasBeenDeleted,
            OwnerId = this.OwnerId,
            PublisherId = this.PublisherId,
            Tags = this.Tags,
            TenantNodes = this.TenantNodes,
            Tenants = this.Tenants,
            Title  = this.Title,
        };
    }
    public override RelationSide RelationSideThisOrganization => RelationSide.From;

}
public record NewInterOrganizationalExistingToRelation : InterOrganizationalRelationBase, InterOrganizationalRelation, NewInterOrganizationalRelation
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
            OrganizationFrom = this.OrganizationTo,
            OrganizationTo = this.OrganizationFrom,
            DateFrom = this.DateFrom,
            DateTo = this.DateTo,
            Description = this.Description,
            GeographicalEntity = this.GeographicalEntity,
            InterOrganizationalRelationType = this.InterOrganizationalRelationType,
            MoneyInvolved = this.MoneyInvolved,
            NumberOfChildrenInvolved = this.NumberOfChildrenInvolved,
            ProofDocument = this.ProofDocument,
            NodeTypeName = this.NodeTypeName,
            Files = this.Files,
            HasBeenDeleted = this.HasBeenDeleted,
            OwnerId = this.OwnerId,
            PublisherId = this.PublisherId,
            Tags = this.Tags,
            TenantNodes = this.TenantNodes,
            Tenants = this.Tenants,
            Title = this.Title,
        };
    }
    public override RelationSide RelationSideThisOrganization => RelationSide.To;
}

public record CompletedNewInterOrganizationalNewFromRelation : InterOrganizationalRelationBase, CompletedInterOrganizationalRelation, CompletedNewInterOrganizationalRelation
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
            OrganizationFrom = this.OrganizationTo,
            OrganizationTo = this.OrganizationFrom,
            DateFrom = this.DateFrom,
            DateTo = this.DateTo,
            Description = this.Description,
            GeographicalEntity = this.GeographicalEntity,
            InterOrganizationalRelationType = this.InterOrganizationalRelationType,
            MoneyInvolved = this.MoneyInvolved,
            NumberOfChildrenInvolved = this.NumberOfChildrenInvolved,
            ProofDocument = this.ProofDocument,
            NodeTypeName = this.NodeTypeName,
            Files = this.Files,
            HasBeenDeleted = this.HasBeenDeleted,
            OwnerId = this.OwnerId,
            PublisherId = this.PublisherId,
            Tags = this.Tags,
            TenantNodes = this.TenantNodes,
            Tenants = this.Tenants,
            Title = this.Title,
        };
    }
    public override RelationSide RelationSideThisOrganization => RelationSide.From;
}
public record NewInterOrganizationalNewFromRelation : InterOrganizationalRelationBase, InterOrganizationalRelation, NewInterOrganizationalRelation
{
    public required OrganizationName OrganizationFrom { get; set; }
    public required OrganizationListItem? OrganizationTo { get; set; }
    public override string OrganizationFromName => OrganizationFrom.Name;
    public override string OrganizationToName => OrganizationTo is null? "": OrganizationTo.Name;
    public override OrganizationItem? OrganizationItemFrom => OrganizationFrom;
    public override OrganizationItem? OrganizationItemTo => OrganizationTo;
    public override NewInterOrganizationalNewToRelation SwapFromAndTo()
    {
        return new NewInterOrganizationalNewToRelation {
            OrganizationFrom = this.OrganizationTo,
            OrganizationTo = this.OrganizationFrom,
            DateFrom = this.DateFrom,
            DateTo = this.DateTo,
            Description = this.Description,
            GeographicalEntity = this.GeographicalEntity,
            InterOrganizationalRelationType = this.InterOrganizationalRelationType,
            MoneyInvolved = this.MoneyInvolved,
            NumberOfChildrenInvolved = this.NumberOfChildrenInvolved,
            ProofDocument = this.ProofDocument,
            NodeTypeName = this.NodeTypeName,
            Files = this.Files,
            HasBeenDeleted = this.HasBeenDeleted,
            OwnerId = this.OwnerId,
            PublisherId = this.PublisherId,
            Tags = this.Tags,
            TenantNodes = this.TenantNodes,
            Tenants = this.Tenants,
            Title = this.Title,
        };
    }
    public override RelationSide RelationSideThisOrganization => RelationSide.From;
}
public record CompletedNewInterOrganizationalNewToRelation : InterOrganizationalRelationBase, CompletedInterOrganizationalRelation, CompletedNewInterOrganizationalRelation
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
            OrganizationFrom = this.OrganizationTo,
            OrganizationTo = this.OrganizationFrom,
            DateFrom = this.DateFrom,
            DateTo = this.DateTo,
            Description = this.Description,
            GeographicalEntity = this.GeographicalEntity,
            InterOrganizationalRelationType = this.InterOrganizationalRelationType,
            MoneyInvolved = this.MoneyInvolved,
            NumberOfChildrenInvolved = this.NumberOfChildrenInvolved,
            ProofDocument = this.ProofDocument,
            NodeTypeName = this.NodeTypeName,
            Files = this.Files,
            HasBeenDeleted = this.HasBeenDeleted,
            OwnerId = this.OwnerId,
            PublisherId = this.PublisherId,
            Tags = this.Tags,
            TenantNodes = this.TenantNodes,
            Tenants = this.Tenants,
            Title = this.Title,
        };
    }
    public override RelationSide RelationSideThisOrganization => RelationSide.To;
}
public record NewInterOrganizationalNewToRelation : InterOrganizationalRelationBase, InterOrganizationalRelation, NewInterOrganizationalRelation
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
            OrganizationFrom = this.OrganizationTo,
            OrganizationTo = this.OrganizationFrom,
            DateFrom = this.DateFrom,
            DateTo = this.DateTo,
            Description = this.Description,
            GeographicalEntity = this.GeographicalEntity,
            InterOrganizationalRelationType = this.InterOrganizationalRelationType,
            MoneyInvolved = this.MoneyInvolved,
            NumberOfChildrenInvolved = this.NumberOfChildrenInvolved,
            ProofDocument = this.ProofDocument,
            NodeTypeName = this.NodeTypeName,
            Files = this.Files,
            HasBeenDeleted = this.HasBeenDeleted,
            OwnerId = this.OwnerId,
            PublisherId = this.PublisherId,
            Tags = this.Tags,
            TenantNodes = this.TenantNodes,
            Tenants = this.Tenants,
            Title = this.Title,
        };
    }
    public override RelationSide RelationSideThisOrganization => RelationSide.To;
}
