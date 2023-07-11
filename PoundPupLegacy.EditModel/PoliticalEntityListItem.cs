namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(PoliticalEntityListItem))]
public partial class PoliticalEntityListItemJsonContext : JsonSerializerContext { }

public sealed record PoliticalEntityListItem : EditListItemBase<PoliticalEntityListItem>
{
}
