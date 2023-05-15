namespace PoundPupLegacy.EditModel;

public interface InterPersonalRelation : Relation
{
    InterPersonalRelationTypeListItem InterPersonalRelationType { get; set; }
    string PersonFromName { get; }
    string PersonToName { get; }
    PartyItem.PersonItem? PersonItemFrom { get; }
    PartyItem.PersonItem? PersonItemTo { get; }
    PartyItem.PersonListItem? PersonListItemFrom { get; set; }
    PartyItem.PersonListItem? PersonListItemTo { get; set; }
    InterPersonalRelation SwapFromAndTo();
    RelationSide RelationSideThisPerson { get; }
}

public interface CompletedInterPersonalRelation : InterPersonalRelation
{
}

public interface NewInterPersonalRelation : InterPersonalRelation, NewNode
{
}
public interface IncompleteNewInterPersonalRelation : NewInterPersonalRelation
{
    public CompletedInterPersonalRelation GetCompletedRelation(PartyItem.PersonListItem personListItem);
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
    public abstract PartyItem.PersonItem? PersonItemFrom { get; }
    public abstract PartyItem.PersonItem? PersonItemTo { get; }

    public abstract InterPersonalRelation SwapFromAndTo();
    public abstract RelationSide RelationSideThisPerson { get; }
    public PartyItem.PersonListItem? PersonListItemFrom { get; set; }
    public PartyItem.PersonListItem? PersonListItemTo { get; set; }

}

[JsonSerializable(typeof(ExistingInterPersonalRelation))]
public partial class ExistingInterPersonalRelationJsonContext : JsonSerializerContext { }

public record ExistingInterPersonalRelation : InterPersonalRelationBase, ExistingNode, ResolvedInterPersonalRelation
{
    public required PartyItem.PersonListItem PersonFrom { get; set; }
    public required PartyItem.PersonListItem PersonTo { get; set; }
    public int NodeId { get; init; }
    public int UrlId { get; set; }
    [JsonIgnore]
    public override string PersonFromName => PersonFrom.Name;
    [JsonIgnore]
    public override string PersonToName => PersonTo.Name;
    [JsonIgnore]
    public override PartyItem.PersonItem? PersonItemFrom => PersonFrom;
    [JsonIgnore]
    public override PartyItem.PersonItem? PersonItemTo => PersonTo;
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

public record NewInterPersonalExistingRelation : InterPersonalRelationBase, ResolvedInterPersonalRelation
{
    public required PartyItem.PersonListItem PersonFrom { get; set; }
    public required PartyItem.PersonListItem PersonTo { get; set; }
    public override string PersonFromName => PersonFrom.Name;
    public override string PersonToName => PersonTo.Name;
    public override PartyItem.PersonItem? PersonItemFrom => PersonFrom;
    public override PartyItem.PersonItem? PersonItemTo => PersonTo;

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

public record NewInterPersonalExistingFromRelation : InterPersonalRelationBase, InterPersonalRelation, IncompleteNewInterPersonalRelation
{
    public required PartyItem.PersonListItem PersonFrom { get; set; }
    public required PartyItem.PersonListItem? PersonTo { get; set; }
    public override string PersonFromName => PersonFrom.Name;
    public override string PersonToName => PersonTo is null ? "" : PersonTo.Name;
    public override PartyItem.PersonItem? PersonItemFrom => PersonFrom;
    public override PartyItem.PersonItem? PersonItemTo => PersonTo;
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
    public CompletedInterPersonalRelation GetCompletedRelation(PartyItem.PersonListItem personTo)
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
public record NewInterPersonalExistingToRelation : InterPersonalRelationBase, InterPersonalRelation, IncompleteNewInterPersonalRelation
{
    public required PartyItem.PersonListItem? PersonFrom { get; set; }
    public required PartyItem.PersonListItem PersonTo { get; set; }
    public override string PersonFromName => PersonFrom is null ? "" : PersonFrom.Name;
    public override string PersonToName => PersonTo.Name;
    public override PartyItem.PersonItem? PersonItemFrom => PersonFrom;
    public override PartyItem.PersonItem? PersonItemTo => PersonTo;
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
    public CompletedInterPersonalRelation GetCompletedRelation(PartyItem.PersonListItem personFrom)
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

public record CompletedNewInterPersonalNewFromRelation : InterPersonalRelationBase, CompletedInterPersonalRelation, CompletedNewInterPersonalRelation
{
    public required PartyItem.PersonName PersonFrom { get; set; }
    public required PartyItem.PersonListItem PersonTo { get; set; }
    public override string PersonFromName => PersonFrom.Name;
    public override string PersonToName => PersonTo.Name;
    public override PartyItem.PersonItem? PersonItemFrom => PersonFrom;
    public override PartyItem.PersonItem? PersonItemTo => PersonTo;

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
public record NewInterPersonalNewFromRelation : InterPersonalRelationBase, InterPersonalRelation, IncompleteNewInterPersonalRelation
{
    public required PartyItem.PersonName PersonFrom { get; set; }
    public required PartyItem.PersonListItem? PersonTo { get; set; }
    public override string PersonFromName => PersonFrom.Name;
    public override string PersonToName => PersonTo is null ? "" : PersonTo.Name;
    public override PartyItem.PersonItem? PersonItemFrom => PersonFrom;
    public override PartyItem.PersonItem? PersonItemTo => PersonTo;
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
    public CompletedInterPersonalRelation GetCompletedRelation(PartyItem.PersonListItem personTo)
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
public record CompletedNewInterPersonalNewToRelation : InterPersonalRelationBase, CompletedInterPersonalRelation, CompletedNewInterPersonalRelation
{
    public required PartyItem.PersonListItem PersonFrom { get; set; }
    public required PartyItem.PersonName PersonTo { get; set; }
    public override string PersonFromName => PersonFrom.Name;
    public override string PersonToName => PersonTo.Name;
    public override PartyItem.PersonItem? PersonItemFrom => PersonFrom;
    public override PartyItem.PersonItem? PersonItemTo => PersonTo;
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
public record NewInterPersonalNewToRelation : InterPersonalRelationBase, InterPersonalRelation, IncompleteNewInterPersonalRelation
{
    public required PartyItem.PersonListItem? PersonFrom { get; set; }
    public required PartyItem.PersonName PersonTo { get; set; }
    public override string PersonFromName => PersonFrom is null ? "" : PersonFrom.Name;
    public override string PersonToName => PersonTo.Name;
    public override PartyItem.PersonItem? PersonItemFrom => PersonFrom;
    public override PartyItem.PersonItem? PersonItemTo => PersonTo;
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

    public CompletedInterPersonalRelation GetCompletedRelation(PartyItem.PersonListItem personFrom)
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
