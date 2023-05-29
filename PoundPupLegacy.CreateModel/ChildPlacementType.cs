﻿namespace PoundPupLegacy.CreateModel;

public abstract record ChildPlacementType : Nameable
{
    private ChildPlacementType() { }
    public abstract Identification Identification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public abstract NameableDetails NameableDetails { get; }
    public abstract T Match<T>(Func<ChildPlacementTypeToCreate, T> create, Func<ChildPlacementTypeToUpdate, T> update);
    public abstract void Match(Action<ChildPlacementTypeToCreate> create, Action<ChildPlacementTypeToUpdate> update);

    public sealed record ChildPlacementTypeToCreate : ChildPlacementType, NameableToCreate
    {
        public override Identification Identification => IdentificationForCreate;
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public override NameableDetails NameableDetails => NameableDetailsForCreate;
        public required Identification.IdentificationForCreate IdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public required NameableDetails.NameableDetailsForCreate NameableDetailsForCreate { get; init; }
        public override T Match<T>(Func<ChildPlacementTypeToCreate, T> create, Func<ChildPlacementTypeToUpdate, T> update)
        {
            return create(this);
        }
        public override void Match(Action<ChildPlacementTypeToCreate> create, Action<ChildPlacementTypeToUpdate> update)
        {
            create(this);
        }
    }
    public sealed record ChildPlacementTypeToUpdate : ChildPlacementType, NameableToUpdate
    {
        public required Identification.IdentificationForUpdate IdentificationForUpdate { get; init; }
        public required NodeDetails.NodeDetailsForUpdate NodeDetailsForUpdate { get; init; }
        public override NameableDetails NameableDetails => NameableDetailsForUpdate;
        public override Identification Identification => IdentificationForUpdate;
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public required NameableDetails.NameableDetailsForUpdate NameableDetailsForUpdate { get; init; }
        public override T Match<T>(Func<ChildPlacementTypeToCreate, T> create, Func<ChildPlacementTypeToUpdate, T> update)
        {
            return update(this);
        }
        public override void Match(Action<ChildPlacementTypeToCreate> create, Action<ChildPlacementTypeToUpdate> update)
        {
            update(this);
        }
    }
}
