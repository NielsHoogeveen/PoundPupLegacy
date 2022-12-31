﻿namespace PoundPupLegacy.Model;

public record File : Identifiable
{
    public required int Id { get; init; }

    public required String Path { get; init; }

    public required String Name { get; init; }

    public required String MimeType { get; init; }

    public required int Size { get; init; }
}