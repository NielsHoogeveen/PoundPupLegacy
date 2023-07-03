using PoundPupLegacy.Common;
using System.Text.Json.Serialization;

namespace PoundPupLegacy.Models;

[JsonSerializable(typeof(MenuItem))]
internal partial class MenuItemJsonContext : JsonSerializerContext
{

}

public sealed record MenuItem : Link
{
    public required int MenuItemId { get; init; }
    public required int? ActionId { get; init; }
    public required string Path { get; init; }
    public required string Title { get; init; }
}


