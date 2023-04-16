﻿namespace PoundPupLegacy.ViewModel.Models;

public record PersonListEntry : ListEntry
{
    public required string Path { get; init; }
    public required string Title { get; init; }
    public required bool HasBeenPublished { get; init; }

}