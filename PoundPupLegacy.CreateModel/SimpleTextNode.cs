﻿namespace PoundPupLegacy.CreateModel;

public interface SimpleTextNode : Searchable
{
    string Text { get; }

    string Teaser { get; }
}