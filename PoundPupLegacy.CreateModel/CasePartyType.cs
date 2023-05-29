﻿namespace PoundPupLegacy.CreateModel;

public abstract record CasePartyType : Nameable
{
    private CasePartyType() { }
    public abstract Identification Identification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public abstract NameableDetails NameableDetails { get; }
    public abstract T Match<T>(Func<CasePartyTypeToCreate, T> create, Func<CasePartyTypeToUpdate, T> update);
    public abstract void Match(Action<CasePartyTypeToCreate> create, Action<CasePartyTypeToUpdate> update);

    public sealed record CasePartyTypeToCreate : CasePartyType, NameableToCreate
    {
        public override Identification Identification => IdentificationForCreate;
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public override NameableDetails NameableDetails => NameableDetailsForCreate;
        public required Identification.IdentificationForCreate IdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public required NameableDetails.NameableDetailsForCreate NameableDetailsForCreate { get; init; }
        public override T Match<T>(Func<CasePartyTypeToCreate, T> create, Func<CasePartyTypeToUpdate, T> update)
        {
            return create(this);
        }
        public override void Match(Action<CasePartyTypeToCreate> create, Action<CasePartyTypeToUpdate> update)
        {
            create(this);
        }
    }
    public sealed record CasePartyTypeToUpdate : CasePartyType, NameableToUpdate
    {
        public required Identification.IdentificationForUpdate IdentificationForUpdate { get; init; }
        public required NodeDetails.NodeDetailsForUpdate NodeDetailsForUpdate { get; init; }
        public override NameableDetails NameableDetails => NameableDetailsForUpdate;
        public override Identification Identification => IdentificationForUpdate;
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public required NameableDetails.NameableDetailsForUpdate NameableDetailsForUpdate { get; init; }
        public override T Match<T>(Func<CasePartyTypeToCreate, T> create, Func<CasePartyTypeToUpdate, T> update)
        {
            return update(this);
        }
        public override void Match(Action<CasePartyTypeToCreate> create, Action<CasePartyTypeToUpdate> update)
        {
            update(this);
        }
    }
}
