using System.Text.Json.Serialization;

namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(AdoptionImportValue))]
public partial class AdoptionImportValueJsonContext : JsonSerializerContext { }

public sealed record AdoptionImportValue
{
    public required int Year { get; init; }
    public required int NumberOfChildren { get; init; }
}
