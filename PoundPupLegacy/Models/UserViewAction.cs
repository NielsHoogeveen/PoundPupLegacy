using System.Text.Json.Serialization;

namespace PoundPupLegacy.Models;

public sealed record UserViewAction: IUserAction
{
    public required int NodeTypeId { get; init; }
    public required string NodeTypeName { get; init; }
    public required string ViewerPath { get; init; }
    public string Path => $"/{ViewerPath}/{{Id:int}}";
}
