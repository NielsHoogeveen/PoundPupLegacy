﻿namespace PoundPupLegacy.ViewModel.Models;

public record SubdivisionListItem
{
    public required string Name { get; init; }

    public required string Path { get; init; }
}