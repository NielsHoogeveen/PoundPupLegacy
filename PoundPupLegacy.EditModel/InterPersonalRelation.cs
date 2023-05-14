namespace PoundPupLegacy.EditModel;

public interface InterPersonalRelation : Relation
{
    InterPersonalRelationTypeListItem InterPersonalRelationType { get; }
    string PersonFromName { get; }
    string PersonToName { get; }
    PersonItem? PersonItemFrom { get; }
    PersonItem? PersonItemTo { get; }
    InterPersonalRelation SwapFromAndTo();
    RelationSide RelationSideThisPerson { get; }
}

public interface CompletedInterPersonalRelation : InterPersonalRelation
{

}

public interface NewInterPersonalRelation : InterPersonalRelation, NewNode
{

}
public interface CompletedNewInterPersonalRelation : NewInterPersonalRelation, CompletedInterPersonalRelation
{
}
public interface ResolvedInterPersonalRelation : CompletedInterPersonalRelation
{
}

public abstract record InterPersonalRelationBase : RelationBase, InterPersonalRelation
{
    public required InterPersonalRelationTypeListItem InterPersonalRelationType { get; set; }
    public abstract string PersonFromName { get; }
    public abstract string PersonToName { get; }
    public abstract PersonItem? PersonItemFrom { get; }
    public abstract PersonItem? PersonItemTo { get; }

    public abstract InterPersonalRelation SwapFromAndTo();
    public abstract RelationSide RelationSideThisPerson { get; }
}

[JsonSerializable(typeof(ExistingInterPersonalRelation))]
public partial class ExistingInterPersonalRelationJsonContext : JsonSerializerContext { }

public record ExistingInterPersonalRelation : InterPersonalRelationBase, ExistingNode, ResolvedInterPersonalRelation
{
    public required PersonListItem PersonFrom { get; set; }
    public required PersonListItem PersonTo { get; set; }
    public int NodeId { get; init; }
    public int UrlId { get; set; }
    [JsonIgnore]
    public override string PersonFromName => PersonFrom.Name;
    [JsonIgnore]
    public override string PersonToName => PersonTo.Name;
    [JsonIgnore]
    public override PersonItem? PersonItemFrom => PersonFrom;
    [JsonIgnore]
    public override PersonItem? PersonItemTo => PersonTo;
    public override ExistingInterPersonalRelation SwapFromAndTo()
    {
        var tmp = PersonFrom;
        PersonFrom = PersonTo;
        PersonTo = tmp;
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

public record NewInterPersonalExistingRelation : InterPersonalRelationBase, ResolvedInterPersonalRelation
{
    public required PersonListItem PersonFrom { get; set; }
    public required PersonListItem PersonTo { get; set; }
    public override string PersonFromName => PersonFrom.Name;
    public override string PersonToName => PersonTo.Name;
    public override PersonItem? PersonItemFrom => PersonFrom;
    public override PersonItem? PersonItemTo => PersonTo;

    public override NewInterPersonalExistingRelation SwapFromAndTo()
    {
        var tmp = PersonFrom;
        PersonFrom = PersonTo;
        PersonTo = tmp;
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

public record NewInterPersonalExistingFromRelation : InterPersonalRelationBase, InterPersonalRelation, NewInterPersonalRelation
{
    public required PersonListItem PersonFrom { get; set; }
    public required PersonListItem? PersonTo { get; set; }
    public override string PersonFromName => PersonFrom.Name;
    public override string PersonToName => PersonTo is null ? "" : PersonTo.Name;
    public override PersonItem? PersonItemFrom => PersonFrom;
    public override PersonItem? PersonItemTo => PersonTo;
    public override NewInterPersonalExistingToRelation SwapFromAndTo()
    {
        return new NewInterPersonalExistingToRelation {
            PersonFrom = this.PersonTo,
            PersonTo = this.PersonFrom,
            DateFrom = this.DateFrom,
            DateTo = this.DateTo,
            Description = this.Description,
            InterPersonalRelationType = this.InterPersonalRelationType,
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
    public override RelationSide RelationSideThisPerson => RelationSide.From;

}
public record NewInterPersonalExistingToRelation : InterPersonalRelationBase, InterPersonalRelation, NewInterPersonalRelation
{
    public required PersonListItem? PersonFrom { get; set; }
    public required PersonListItem PersonTo { get; set; }
    public override string PersonFromName => PersonFrom is null ? "" : PersonFrom.Name;
    public override string PersonToName => PersonTo.Name;
    public override PersonItem? PersonItemFrom => PersonFrom;
    public override PersonItem? PersonItemTo => PersonTo;
    public override NewInterPersonalExistingFromRelation SwapFromAndTo()
    {
        return new NewInterPersonalExistingFromRelation {
            PersonFrom = this.PersonTo,
            PersonTo = this.PersonFrom,
            DateFrom = this.DateFrom,
            DateTo = this.DateTo,
            Description = this.Description,
            InterPersonalRelationType = this.InterPersonalRelationType,
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
    public override RelationSide RelationSideThisPerson => RelationSide.To;
}

public record CompletedNewInterPersonalNewFromRelation : InterPersonalRelationBase, CompletedInterPersonalRelation, CompletedNewInterPersonalRelation
{
    public required PersonName PersonFrom { get; set; }
    public required PersonListItem PersonTo { get; set; }
    public override string PersonFromName => PersonFrom.Name;
    public override string PersonToName => PersonTo.Name;
    public override PersonItem? PersonItemFrom => PersonFrom;
    public override PersonItem? PersonItemTo => PersonTo;

    public override CompletedNewInterPersonalNewToRelation SwapFromAndTo()
    {
        return new CompletedNewInterPersonalNewToRelation {
            PersonFrom = this.PersonTo,
            PersonTo = this.PersonFrom,
            DateFrom = this.DateFrom,
            DateTo = this.DateTo,
            Description = this.Description,
            InterPersonalRelationType = this.InterPersonalRelationType,
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
    public override RelationSide RelationSideThisPerson => RelationSide.From;
}
public record NewInterPersonalNewFromRelation : InterPersonalRelationBase, InterPersonalRelation, NewInterPersonalRelation
{
    public required PersonName PersonFrom { get; set; }
    public required PersonListItem? PersonTo { get; set; }
    public override string PersonFromName => PersonFrom.Name;
    public override string PersonToName => PersonTo is null ? "" : PersonTo.Name;
    public override PersonItem? PersonItemFrom => PersonFrom;
    public override PersonItem? PersonItemTo => PersonTo;
    public override NewInterPersonalNewToRelation SwapFromAndTo()
    {
        return new NewInterPersonalNewToRelation {
            PersonFrom = this.PersonTo,
            PersonTo = this.PersonFrom,
            DateFrom = this.DateFrom,
            DateTo = this.DateTo,
            Description = this.Description,
            InterPersonalRelationType = this.InterPersonalRelationType,
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
    public override RelationSide RelationSideThisPerson => RelationSide.From;
}
public record CompletedNewInterPersonalNewToRelation : InterPersonalRelationBase, CompletedInterPersonalRelation, CompletedNewInterPersonalRelation
{
    public required PersonListItem PersonFrom { get; set; }
    public required PersonName PersonTo { get; set; }
    public override string PersonFromName => PersonFrom.Name;
    public override string PersonToName => PersonTo.Name;
    public override PersonItem? PersonItemFrom => PersonFrom;
    public override PersonItem? PersonItemTo => PersonTo;
    public override CompletedNewInterPersonalNewFromRelation SwapFromAndTo()
    {
        return new CompletedNewInterPersonalNewFromRelation {
            PersonFrom = this.PersonTo,
            PersonTo = this.PersonFrom,
            DateFrom = this.DateFrom,
            DateTo = this.DateTo,
            Description = this.Description,
            InterPersonalRelationType = this.InterPersonalRelationType,
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
    public override RelationSide RelationSideThisPerson => RelationSide.To;
}
public record NewInterPersonalNewToRelation : InterPersonalRelationBase, InterPersonalRelation, NewInterPersonalRelation
{
    public required PersonListItem? PersonFrom { get; set; }
    public required PersonName PersonTo { get; set; }
    public override string PersonFromName => PersonFrom is null ? "" : PersonFrom.Name;
    public override string PersonToName => PersonTo.Name;
    public override PersonItem? PersonItemFrom => PersonFrom;
    public override PersonItem? PersonItemTo => PersonTo;
    public override NewInterPersonalNewFromRelation SwapFromAndTo()
    {
        return new NewInterPersonalNewFromRelation {
            PersonFrom = this.PersonTo,
            PersonTo = this.PersonFrom,
            DateFrom = this.DateFrom,
            DateTo = this.DateTo,
            Description = this.Description,
            InterPersonalRelationType = this.InterPersonalRelationType,
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
    public override RelationSide RelationSideThisPerson => RelationSide.To;
}
