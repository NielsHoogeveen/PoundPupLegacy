﻿namespace PoundPupLegacy.CreateModel;

public sealed record TermHierarchy: IRequest
{
    public required int TermIdPartent { get; init; }

    public required int TermIdChild { get; init; }
}
