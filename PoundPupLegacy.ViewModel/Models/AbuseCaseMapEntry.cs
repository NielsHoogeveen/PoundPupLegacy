namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(AbuseCaseMapEntry[]))]
public partial class AbuseCasesMapEntriesJsonContext : JsonSerializerContext { }



public record AbuseCaseMapEntry: LinkBase
{
    public required decimal Latitude { get; init; }
    public required decimal Longitude { get; init; }
}
