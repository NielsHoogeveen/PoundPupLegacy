using System.Text.Json.Serialization;

namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(Authoring))]
public partial class AuthoringJsonContext : JsonSerializerContext { }

public sealed record Authoring
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required DateTime CreatedDateTime { get; init; }
    public required DateTime ChangedDateTime { get; init; }
}
