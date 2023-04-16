﻿namespace PoundPupLegacy.ViewModel.Models;

public record SubgroupPagedList : PagedList<SubgroupListEntry>
{
    public required SubgroupListEntry[] Entries { get; init; }

    public int NumberOfEntries { get; set; }
    public int PageNumber { get; set; }
    public int NumberOfPages { get; set; }
    public string QueryString { get; set; } = "";

    public string Path => "subgroups";
}