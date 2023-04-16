﻿namespace PoundPupLegacy.ViewModel.Models;

public record Comment
{
    public required int Id { get; init; }
    public required string Title { get; init; }
    public required int NodeStatusId { get; init; }
    public required string Text { get; init; }
    public required Authoring Authoring { get; init; }
    public required List<Comment> Comments { get; init; }

}