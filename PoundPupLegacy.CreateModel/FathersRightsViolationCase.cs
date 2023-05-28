﻿namespace PoundPupLegacy.CreateModel;

public abstract record FathersRightsViolationCase : Case
{
    private FathersRightsViolationCase() { }
    public abstract CaseDetails CaseDetails { get; }
    public abstract LocatableDetails LocatableDetails { get; }
    public abstract NodeIdentification NodeIdentification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public abstract NameableDetails NameableDetails { get; }
    public abstract T Match<T>(Func<FathersRightsViolationCaseToCreate, T> create, Func<FathersRightsViolationCaseToUpdate, T> update);
    public abstract void Match(Action<FathersRightsViolationCaseToCreate> create, Action<FathersRightsViolationCaseToUpdate> update);

    public sealed record FathersRightsViolationCaseToCreate : FathersRightsViolationCase, CaseToCreate
    {
        public override NodeIdentification NodeIdentification => NodeIdentificationForCreate;
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public override NameableDetails NameableDetails => NameableDetailsForCreate;
        public override CaseDetails CaseDetails => CaseDetailsForCreate;
        public override LocatableDetails LocatableDetails => LocatableDetailsForCreate;
        public required NodeIdentification.NodeIdentificationForCreate NodeIdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public required NameableDetails.NameableDetailsForCreate NameableDetailsForCreate { get; init; }
        public required CaseDetails.CaseDetailsForCreate CaseDetailsForCreate { get; init; }
        public required LocatableDetails.LocatableDetailsForCreate LocatableDetailsForCreate { get; init; }
        public override T Match<T>(Func<FathersRightsViolationCaseToCreate, T> create, Func<FathersRightsViolationCaseToUpdate, T> update)
        {
            return create(this);
        }
        public override void Match(Action<FathersRightsViolationCaseToCreate> create, Action<FathersRightsViolationCaseToUpdate> update)
        {
            create(this);
        }
    }
    public sealed record FathersRightsViolationCaseToUpdate : FathersRightsViolationCase, CaseToUpdate
    {
        public required NodeIdentification.NodeIdentificationForUpdate NodeIdentificationForUpdate { get; init; }
        public required NodeDetails.NodeDetailsForUpdate NodeDetailsForUpdate { get; init; }
        public override NameableDetails NameableDetails => NameableDetailsForUpdate;
        public override NodeIdentification NodeIdentification => NodeIdentificationForUpdate;
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public override CaseDetails CaseDetails => CaseDetailsForUpdate;
        public override LocatableDetails LocatableDetails => LocatableDetailsForUpdate;
        public required NameableDetails.NameableDetailsForUpdate NameableDetailsForUpdate { get; init; }
        public required CaseDetails.CaseDetailsForUpdate CaseDetailsForUpdate { get; init; }
        public required LocatableDetails.LocatableDetailsForUpdate LocatableDetailsForUpdate { get; init; }
        public override T Match<T>(Func<FathersRightsViolationCaseToCreate, T> create, Func<FathersRightsViolationCaseToUpdate, T> update)
        {
            return update(this);
        }
        public override void Match(Action<FathersRightsViolationCaseToCreate> create, Action<FathersRightsViolationCaseToUpdate> update)
        {
            update(this);
        }
    }
}
