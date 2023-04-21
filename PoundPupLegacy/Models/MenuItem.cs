using PoundPupLegacy.Common;

namespace PoundPupLegacy.Models;

public record MenuItem: Link
{
    public required string Path { get; init; }
    public required string Title { get; init; }
}


