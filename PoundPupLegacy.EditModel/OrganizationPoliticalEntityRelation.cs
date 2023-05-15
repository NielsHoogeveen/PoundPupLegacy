namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(ExistingOrganizationPoliticalEntityRelation))]
public partial class ExistingOrganizationPoliticalEntityRelationJsonContext : JsonSerializerContext { }

public interface OrganizationPoliticalEntityRelation : Relation
{
    OrganizationPoliticalEntityRelationTypeListItem OrganizationPoliticalEntityRelationType { get; set; }

    PartyItem.OrganizationItem? OrganizationItem { get; }
    PoliticalEntityListItem? PoliticalEntityItem { get; }
}
public interface IncompleteOrganizationPoliticalEntityRelation : OrganizationPoliticalEntityRelation
{
    CompletedOrganizationPoliticalEntityRelation GetCompletedRelation(PoliticalEntityListItem politicalEntity);
}
public interface CompletedOrganizationPoliticalEntityRelation: OrganizationPoliticalEntityRelation
{
    string OrganizationName { get; }

    string PoliticalEntityName { get; }
}
public interface ResolvedOrganizationPoliticalEntityRelation: CompletedOrganizationPoliticalEntityRelation
{

}
public record CompletedNewOrganizationPoliticalEntityRelationNewOrganization : OrganizationPoliticalEntityRelationBase, NewNode, CompletedOrganizationPoliticalEntityRelation
{
    public required PartyItem.OrganizationName Organization { get; set; }
    public required PoliticalEntityListItem PoliticalEntity { get; set; }
    public string OrganizationName => Organization.Name;
    public string PoliticalEntityName => PoliticalEntity.Name;
    public override PartyItem.OrganizationItem? OrganizationItem => Organization;
    public override PoliticalEntityListItem? PoliticalEntityItem => PoliticalEntity;

}
public record CompletedNewOrganizationPoliticalEntityRelationExistingOrganization : OrganizationPoliticalEntityRelationBase, NewNode, CompletedOrganizationPoliticalEntityRelation
{
    public required PartyItem.OrganizationListItem Organization { get; set; }
    public required PoliticalEntityListItem PoliticalEntity { get; set; }

    public string OrganizationName => Organization.Name;
    public string PoliticalEntityName => PoliticalEntity.Name;
    public override PartyItem.OrganizationItem? OrganizationItem => Organization;
    public override PoliticalEntityListItem? PoliticalEntityItem => PoliticalEntity;
}

public record NewOrganizationPoliticalEntityRelationNewOrganization : OrganizationPoliticalEntityRelationBase, NewNode, IncompleteOrganizationPoliticalEntityRelation
{
    public required PartyItem.OrganizationName Organization { get; set; }
    public required PoliticalEntityListItem? PoliticalEntity { get; set; }
    public override PartyItem.OrganizationItem? OrganizationItem => Organization;
    public override PoliticalEntityListItem? PoliticalEntityItem => PoliticalEntity;
    public CompletedOrganizationPoliticalEntityRelation GetCompletedRelation(PoliticalEntityListItem politicalEntity)
    {
        return new CompletedNewOrganizationPoliticalEntityRelationNewOrganization {
            Organization = Organization,
            PoliticalEntity = politicalEntity,
            DateFrom = DateFrom,
            DateTo = DateTo,
            OrganizationPoliticalEntityRelationType = OrganizationPoliticalEntityRelationType,
            Description = Description,
            Files = Files,
            HasBeenDeleted = HasBeenDeleted,
            NodeTypeName = NodeTypeName,
            OwnerId = OwnerId,
            PublisherId = PublisherId,
            ProofDocument = ProofDocument,
            Tags = Tags,
            TenantNodes = TenantNodes,
            Tenants = Tenants,
            Title = Title
        };
    }
}
public record NewOrganizationPoliticalEntityRelationExistingOrganization : OrganizationPoliticalEntityRelationBase, NewNode, IncompleteOrganizationPoliticalEntityRelation
{
    public required PartyItem.OrganizationListItem Organization { get; set; }
    public required PoliticalEntityListItem? PoliticalEntity { get; set; }
    public override PartyItem.OrganizationItem? OrganizationItem => Organization;
    public override PoliticalEntityListItem? PoliticalEntityItem => PoliticalEntity;
    public CompletedOrganizationPoliticalEntityRelation GetCompletedRelation(PoliticalEntityListItem politicalEntity)
    {
        return new CompletedNewOrganizationPoliticalEntityRelationExistingOrganization {
            Organization = Organization,
            PoliticalEntity = politicalEntity,
            DateFrom = DateFrom,
            DateTo = DateTo,
            OrganizationPoliticalEntityRelationType = OrganizationPoliticalEntityRelationType,
            Description = Description,
            Files = Files,
            HasBeenDeleted = HasBeenDeleted,
            NodeTypeName = NodeTypeName,
            OwnerId = OwnerId,
            PublisherId = PublisherId,
            ProofDocument = ProofDocument,
            Tags = Tags,
            TenantNodes = TenantNodes,
            Tenants = Tenants,
            Title = Title
        };
    }

}

public record ExistingOrganizationPoliticalEntityRelation : OrganizationPoliticalEntityRelationBase, ExistingNode, ResolvedOrganizationPoliticalEntityRelation
{
    public int NodeId { get; init; }

    public int UrlId { get; set; }
    public required PartyItem.OrganizationListItem Organization { get; set; }
    public required PoliticalEntityListItem PoliticalEntity { get; set; }
    public string OrganizationName => Organization.Name;
    public string PoliticalEntityName => PoliticalEntity.Name;
    public override PartyItem.OrganizationItem? OrganizationItem => Organization;
    public override PoliticalEntityListItem? PoliticalEntityItem => PoliticalEntity;


}
public abstract record OrganizationPoliticalEntityRelationBase : RelationBase, OrganizationPoliticalEntityRelation
{
    public required OrganizationPoliticalEntityRelationTypeListItem OrganizationPoliticalEntityRelationType { get; set; }

    public abstract PartyItem.OrganizationItem? OrganizationItem { get; }
    public abstract PoliticalEntityListItem? PoliticalEntityItem { get; }

}
