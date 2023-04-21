﻿namespace PoundPupLegacy.ViewModel.Models;

public record CongressionalTerm
{
    public required string MemberType { get; init; }

    public required BasicLink State { get; init; }
    public DateTime? DateFrom { get; init; }
    public DateTime? DateTo { get; init; }
}
