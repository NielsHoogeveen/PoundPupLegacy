﻿namespace PoundPupLegacy.ViewModel.Models;

public record OrganizationListEntry : ListEntry
{
    public required string Path { get; init; }
    public required string Title { get; init; }
}
