namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(GeographicalEntityListItem))]
public partial class GeographicalEntityListItemJsonContext : JsonSerializerContext { }

public sealed record GeographicalEntityListItem : EditListItemBase<GeographicalEntityListItem>
{
}
