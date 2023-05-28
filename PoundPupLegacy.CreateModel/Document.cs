﻿namespace PoundPupLegacy.CreateModel;

public abstract record Document : SimpleTextNode
{
    private Document() { }
    public required DocumentDetails DocumentDetails { get; init; }
    public required SimpleTextNodeDetails SimpleTextNodeDetails { get; init; }
    public abstract NodeIdentification NodeIdentification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public abstract T Match<T>(Func<DocumentToCreate, T> create, Func<DocumentToUpdate, T> update);
    public abstract void Match(Action<DocumentToCreate> create, Action<DocumentToUpdate> update);

    public sealed record DocumentToCreate : Document, NodeToCreate
    {
        public required NodeIdentification.NodeIdentificationForCreate NodeIdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }

        public override NodeIdentification NodeIdentification => NodeIdentificationForCreate;

        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public override T Match<T>(Func<DocumentToCreate, T> create, Func<DocumentToUpdate, T> update)
        {
            return create(this);
        }
        public override void Match(Action<DocumentToCreate> create, Action<DocumentToUpdate> update)
        {
            create(this);
        }
    }
    public sealed record DocumentToUpdate : Document, NodeToUpdate
    {
        public override NodeIdentification NodeIdentification => NodeIdentificationForUpdate;

        public override NodeDetails NodeDetails => NodeDetailsForUpdate;

        public required NodeIdentification.NodeIdentificationForUpdate NodeIdentificationForUpdate { get; init; }
        public required NodeDetails.NodeDetailsForUpdate NodeDetailsForUpdate { get; init; }
        public override T Match<T>(Func<DocumentToCreate, T> create, Func<DocumentToUpdate, T> update)
        {
            return update(this);
        }
        public override void Match(Action<DocumentToCreate> create, Action<DocumentToUpdate> update)
        {
            update(this);
        }
    }
}

public sealed record DocumentDetails
{
    public required FuzzyDate? Published { get; init; }
    public required string? SourceUrl { get; init; }
    public required int? DocumentTypeId { get; init; }
}
