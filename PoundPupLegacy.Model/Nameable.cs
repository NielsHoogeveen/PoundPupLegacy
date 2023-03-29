﻿namespace PoundPupLegacy.CreateModel;

public interface Nameable : Searchable
{
    string Description { get; }

    int? FileIdTileImage { get; }

    public List<VocabularyName> VocabularyNames { get; }
}
