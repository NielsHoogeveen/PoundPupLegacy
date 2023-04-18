﻿namespace PoundPupLegacy.ViewModel.Models;

public record Persons : IPagedList<PersonListEntry>
{
    private PersonListEntry[] _entries = Array.Empty<PersonListEntry>();
    public required PersonListEntry[] Entries
    {
        get => _entries;
        init
        {
            if (value is not null)
            {
                _entries = value;
            }
        }
    }

    public required int NumberOfEntries { get; init; }

}
