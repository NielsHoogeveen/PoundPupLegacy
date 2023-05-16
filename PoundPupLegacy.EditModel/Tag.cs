﻿namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(TagNodeType))]
public partial class TagNodeTypeJsonContext : JsonSerializerContext { }

public sealed record TagNodeType
{
    public required int[] NodeTypeIds { get; init; }

    public required string TagLabelName { get; init; }

}

public sealed record Tags
{
    public required TagNodeType TagNodeType { get; init; }


    private List<Tag> entries = new();
    public List<Tag> Entries { get => entries; init { if (value is not null) entries = value; } } 
}

public sealed record Tag
{
    public required int? NodeId { get; set; }

    public required int TermId { get; init; }

    public required string Name { get; init; }

    public required int NodeTypeId { get; init; }

    public bool IsStored { get; set; } = true;

    public bool HasBeenDeleted { get; set; } = false;

    public void SetToDeleted()
    {
        HasBeenDeleted = true;
    }
}
