using System.Text.Json.Serialization;

namespace PoundPupLegacy.Models;

public sealed record UserEditAction
{
    public required int NodeTypeId { get; init; }
    public required string NodeTypeName { get; init; }
}
