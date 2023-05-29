﻿namespace PoundPupLegacy.CreateModel;

public abstract record BasicCountry : TopLevelCountry
{
    private BasicCountry() { }
    public required TopLevelCountryDetails TopLevelCountryDetails { get; init; }
    public required CountryDetails CountryDetails { get; init; }
    public required PoliticalEntityDetails PoliticalEntityDetails { get; init; }
    public abstract Identification Identification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public abstract NameableDetails NameableDetails { get; }
    public abstract T Match<T>(Func<BasicCountryToCreate, T> create, Func<BasicCountryToUpdate, T> update);
    public abstract void Match(Action<BasicCountryToCreate> create, Action<BasicCountryToUpdate> update);

    public sealed record BasicCountryToCreate : BasicCountry, TopLevelCountryToCreate
    {
        public override Identification Identification => IdentificationForCreate;
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public override NameableDetails NameableDetails => NameableDetailsForCreate;
        public required Identification.IdentificationForCreate IdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public required NameableDetails.NameableDetailsForCreate NameableDetailsForCreate { get; init; }
        public override T Match<T>(Func<BasicCountryToCreate, T> create, Func<BasicCountryToUpdate, T> update)
        {
            return create(this);
        }
        public override void Match(Action<BasicCountryToCreate> create, Action<BasicCountryToUpdate> update)
        {
            create(this);
        }
    }
    public sealed record BasicCountryToUpdate : BasicCountry, TopLevelCountryToUpdate
    {
        public required Identification.IdentificationForUpdate IdentificationForUpdate { get; init; }
        public required NodeDetails.NodeDetailsForUpdate NodeDetailsForUpdate { get; init; }
        public override NameableDetails NameableDetails => NameableDetailsForUpdate;
        public override Identification Identification => IdentificationForUpdate;
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public required NameableDetails.NameableDetailsForUpdate NameableDetailsForUpdate { get; init; }
        public override T Match<T>(Func<BasicCountryToCreate, T> create, Func<BasicCountryToUpdate, T> update)
        {
            return update(this);
        }
        public override void Match(Action<BasicCountryToCreate> create, Action<BasicCountryToUpdate> update)
        {
            update(this);
        }
    }
}
