namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(GlobalRegion))]
public partial class GlobalRegionJsonContext : JsonSerializerContext { }

public sealed record GlobalRegion : NameableBase, GeographicalEntity
{
}
