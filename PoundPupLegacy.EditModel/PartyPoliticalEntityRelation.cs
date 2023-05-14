namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(ExistingPartyPoliticalEntityRelation))]
public partial class ExistingPartyPoliticalEntityRelationJsonContext : JsonSerializerContext { }

public interface PartyPoliticalEntityRelation : Relation
{
    PartyPoliticalEntityRelationTypeListItem PartyPoliticalEntityRelationType { get; set; }

    PartyItem? PartyItem { get; }
    PoliticalEntityListItem? PoliticalEntityItem { get; }
}

public interface CompletedPartyPoliticalEntityRelation: PartyPoliticalEntityRelation
{
    string PartyName { get; }

    string PoliticalEntityName { get; }
}
public interface ResolvedPartyPoliticalEntityRelation: CompletedPartyPoliticalEntityRelation
{

}
public record CompletedNewPartyPoliticalEntityRelationNewParty : PartyPoliticalEntityRelationBase, NewNode, CompletedPartyPoliticalEntityRelation
{
    public required PartyName Party { get; set; }
    public required PoliticalEntityListItem PoliticalEntity { get; set; }
    public string PartyName => Party.Name;
    public string PoliticalEntityName => PoliticalEntity.Name;
    public override PartyItem? PartyItem => Party;
    public override PoliticalEntityListItem? PoliticalEntityItem => PoliticalEntity;

}
public record CompletedNewPartyPoliticalEntityRelationExistingParty : PartyPoliticalEntityRelationBase, NewNode, CompletedPartyPoliticalEntityRelation
{
    public required PartyListItem Party { get; set; }
    public required PoliticalEntityListItem PoliticalEntity { get; set; }

    public string PartyName => Party.Name;
    public string PoliticalEntityName => PoliticalEntity.Name;
    public override PartyItem? PartyItem => Party;
    public override PoliticalEntityListItem? PoliticalEntityItem => PoliticalEntity;
}

public record NewPartyPoliticalEntityRelationNewParty : PartyPoliticalEntityRelationBase, NewNode
{
    public required PartyName Party { get; set; }
    public required PoliticalEntityListItem? PoliticalEntity { get; set; }
    public override PartyItem? PartyItem => Party;
    public override PoliticalEntityListItem? PoliticalEntityItem => PoliticalEntity;
}
public record NewPartyPoliticalEntityRelationExistingParty : PartyPoliticalEntityRelationBase, NewNode
{
    public required PartyListItem Party { get; set; }
    public required PoliticalEntityListItem? PoliticalEntity { get; set; }
    public override PartyItem? PartyItem => Party;
    public override PoliticalEntityListItem? PoliticalEntityItem => PoliticalEntity;
}

public record ExistingPartyPoliticalEntityRelation : PartyPoliticalEntityRelationBase, ExistingNode, ResolvedPartyPoliticalEntityRelation
{
    public int NodeId { get; init; }

    public int UrlId { get; set; }
    public required PartyListItem Party { get; set; }
    public required PoliticalEntityListItem PoliticalEntity { get; set; }
    public string PartyName => Party.Name;
    public string PoliticalEntityName => PoliticalEntity.Name;
    public override PartyItem? PartyItem => Party;
    public override PoliticalEntityListItem? PoliticalEntityItem => PoliticalEntity;


}
public abstract record PartyPoliticalEntityRelationBase : RelationBase, PartyPoliticalEntityRelation
{
    public required PartyPoliticalEntityRelationTypeListItem PartyPoliticalEntityRelationType { get; set; }

    public abstract PartyItem? PartyItem { get; }
    public abstract PoliticalEntityListItem? PoliticalEntityItem { get; }

}
