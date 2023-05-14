namespace PoundPupLegacy.EditModel;


public interface InterOrganizationalRelation : Node
{
    string Description { get; set; }
    InterOrganizationalRelationTypeListItem InterOrganizationalRelationType { get; }
    DateTime? DateFrom { get; set; }
    DateTime? DateTo { get; set; }
    DateTimeRange DateRange { get; }
    DocumentListItem? ProofDocument { get; set; }
    decimal? MoneyInvolved { get; set; }
    int? NumberOfChildrenInvolved { get; set; }
    GeographicalEntityListItem? GeographicalEntity { get; set; }
    bool HasBeenDeleted { get; set; }
    string OrganizationFromName { get; }
    string OrganizationToName { get; }
    public OrganizationItem? OrganizationItemFrom { get; }
    public OrganizationItem? OrganizationItemTo { get; }
}

public interface InterOrganizationalRelation<TFrom, TTo> : InterOrganizationalRelation
    where TFrom : class?, OrganizationItem?
    where TTo : class?, OrganizationItem?
{

    TFrom OrganizationFrom { get; }
    TTo OrganizationTo { get; }

}

public interface CompletedInterOrganizationalRelation: InterOrganizationalRelation
{

}
public interface CompletedInterOrganizationalRelation<TFrom, TTo>: InterOrganizationalRelation<TFrom, TTo>, CompletedInterOrganizationalRelation
    where TFrom: class, OrganizationItem
    where TTo : class, OrganizationItem
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

public abstract record InterOrganizationalRelationBase : NodeBase, InterOrganizationalRelation
{
    public required string Description { get; set; }
    public required InterOrganizationalRelationTypeListItem InterOrganizationalRelationType { get; set; }
    public required DateTime? DateFrom { get; set; }
    public required DateTime? DateTo { get; set; }
    public DateTimeRange DateRange {
        get => new DateTimeRange(DateFrom, DateTo);
    }
    public DocumentListItem? ProofDocument { get; set; }
    public decimal? MoneyInvolved { get; set; }
    public int? NumberOfChildrenInvolved { get; set; }
    public required GeographicalEntityListItem? GeographicalEntity { get; set; }
    public bool HasBeenDeleted { get; set; }
    public abstract string OrganizationFromName { get; }
    public abstract string OrganizationToName { get; }
    public abstract OrganizationItem? OrganizationItemFrom { get; }
    public abstract OrganizationItem? OrganizationItemTo { get; }
}

[JsonSerializable(typeof(ExistingInterOrganizationalRelation))]
public partial class ExistingInterOrganizationalRelationJsonContext : JsonSerializerContext { }

public record ExistingInterOrganizationalRelation : InterOrganizationalRelationBase, CompletedInterOrganizationalRelation<OrganizationListItem, OrganizationListItem>, ExistingNode, ResolvedInterOrganizationalRelation
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
}

public record NewInterOrganizationalExistingRelation : InterOrganizationalRelationBase, CompletedInterOrganizationalRelation<OrganizationListItem, OrganizationListItem>, ResolvedInterOrganizationalRelation
{
    public required OrganizationListItem OrganizationFrom { get; set; }
    public required OrganizationListItem OrganizationTo { get; set; }
    public override string OrganizationFromName => OrganizationFrom.Name;
    public override string OrganizationToName => OrganizationTo.Name;
    public override OrganizationItem? OrganizationItemFrom => OrganizationFrom;
    public override OrganizationItem? OrganizationItemTo => OrganizationTo;
}

public record NewInterOrganizationalExistingFromRelation : InterOrganizationalRelationBase, InterOrganizationalRelation<OrganizationListItem, OrganizationListItem?>, NewInterOrganizationalRelation
{
    public required OrganizationListItem OrganizationFrom { get; set; }
    public required OrganizationListItem? OrganizationTo { get; set; }
    public override string OrganizationFromName => OrganizationFrom.Name;
    public override string OrganizationToName => OrganizationTo is null ? "" : OrganizationTo.Name;
    public override OrganizationItem? OrganizationItemFrom => OrganizationFrom;
    public override OrganizationItem? OrganizationItemTo => OrganizationTo;
}
public record NewInterOrganizationalExistingToRelation : InterOrganizationalRelationBase, InterOrganizationalRelation<OrganizationListItem?, OrganizationListItem>, NewInterOrganizationalRelation
{
    public required OrganizationListItem? OrganizationFrom { get; set; }
    public required OrganizationListItem OrganizationTo { get; set; }
    public override string OrganizationFromName => OrganizationFrom is null ? "" : OrganizationFrom.Name;
    public override string OrganizationToName => OrganizationTo.Name;
    public override OrganizationItem? OrganizationItemFrom => OrganizationFrom;
    public override OrganizationItem? OrganizationItemTo => OrganizationTo;
}

public record CompletedNewInterOrganizationalNewFromRelation : InterOrganizationalRelationBase, CompletedInterOrganizationalRelation<OrganizationName, OrganizationListItem>, CompletedNewInterOrganizationalRelation
{
    public required OrganizationName OrganizationFrom { get; set; }
    public required OrganizationListItem OrganizationTo { get; set; }
    public override string OrganizationFromName => OrganizationFrom.Name;
    public override string OrganizationToName => OrganizationTo.Name;
    public override OrganizationItem? OrganizationItemFrom => OrganizationFrom;
    public override OrganizationItem? OrganizationItemTo => OrganizationTo;
}
public record NewInterOrganizationalNewFromRelation : InterOrganizationalRelationBase, InterOrganizationalRelation<OrganizationName, OrganizationListItem?>, NewInterOrganizationalRelation
{
    public required OrganizationName OrganizationFrom { get; set; }
    public required OrganizationListItem? OrganizationTo { get; set; }
    public override string OrganizationFromName => OrganizationFrom.Name;
    public override string OrganizationToName => OrganizationTo is null? "": OrganizationTo.Name;
    public override OrganizationItem? OrganizationItemFrom => OrganizationFrom;
    public override OrganizationItem? OrganizationItemTo => OrganizationTo;
}
public record CompletedNewInterOrganizationalNewToRelation : InterOrganizationalRelationBase, CompletedInterOrganizationalRelation<OrganizationListItem, OrganizationName>, CompletedNewInterOrganizationalRelation
{
    public required OrganizationListItem OrganizationFrom { get; set; }
    public required OrganizationName OrganizationTo { get; set; }
    public override string OrganizationFromName => OrganizationFrom.Name;
    public override string OrganizationToName => OrganizationTo.Name;
    public override OrganizationItem? OrganizationItemFrom => OrganizationFrom;
    public override OrganizationItem? OrganizationItemTo => OrganizationTo;
}
public record NewInterOrganizationalNewToRelation : InterOrganizationalRelationBase, InterOrganizationalRelation<OrganizationListItem?, OrganizationName>, NewInterOrganizationalRelation
{
    public required OrganizationListItem? OrganizationFrom { get; set; }
    public required OrganizationName OrganizationTo { get; set; }
    public override string OrganizationFromName => OrganizationFrom is null ? "" : OrganizationFrom.Name;
    public override string OrganizationToName => OrganizationTo.Name;
    public override OrganizationItem? OrganizationItemFrom => OrganizationFrom;
    public override OrganizationItem? OrganizationItemTo => OrganizationTo;
}
