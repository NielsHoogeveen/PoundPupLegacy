﻿namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(DeportationCaseList))]
public partial class DeportationCaseListJsonContext : JsonSerializerContext { }

public record DeportationCaseList : IPagedList<CaseListEntry>
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
