﻿namespace PoundPupLegacy.ViewModel;

public record SelectionItem
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool Selected { get; set; }
}
