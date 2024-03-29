﻿namespace PoundPupLegacy.Common;

public sealed record BasicLink : Link
{
    public required string Path { get; init; }
    public required string Title { get; init; }
}
