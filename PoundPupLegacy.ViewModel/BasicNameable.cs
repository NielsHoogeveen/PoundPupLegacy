﻿namespace PoundPupLegacy.ViewModel;

public record BasicNameable : Nameable
{
    public int Id { get; set; }
    public int NodeTypeId { get; set; }
    public string Title { get; set; }
    public string Text { get; set; }
    public Authoring Authoring { get; set; }

    public bool HasBeenPublished { get; set; }

    public Link[] BreadCrumElements { get; set; }

    public string Description { get; set; }
    public Link[] Tags { get; set; }
    public Comment[] Comments { get; set; }
    public Link[] SubTopics { get; set; }
    public Link[] SuperTopics { get; set; }
    private File[] _files = Array.Empty<File>();
    public required File[] Files
    {
        get => _files;
        init
        {
            if (value is not null)
            {
                _files = value;
            }
        }
    }

}
