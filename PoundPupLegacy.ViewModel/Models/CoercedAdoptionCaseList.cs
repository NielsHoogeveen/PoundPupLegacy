﻿namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(CoercedAdoptionCaseList))]
public partial class CoercedAdoptionCaseListJsonContext : JsonSerializerContext { }

public sealed record CoercedAdoptionCaseList : IPagedList<CaseListEntry>
{
    private CaseListEntry[] _entries = Array.Empty<CaseListEntry>();
    public CaseListEntry[] Entries {
        get => _entries;
        set {
            if (value != null) {
                _entries = value;
            }
        }
    }

    public required int NumberOfEntries { get; init; }

}
