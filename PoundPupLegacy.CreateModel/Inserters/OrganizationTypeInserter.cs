﻿namespace PoundPupLegacy.CreateModel.Inserters;

using Request = NewOrganizationType;

internal sealed class OrganizationTypeInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    private static readonly NonNullableBooleanDatabaseParameter HasConcreteSubtype = new() { Name = "has_concrete_subtype" };

    public override string TableName => "organization_type";
    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(HasConcreteSubtype, request.HasConcreteSubtype),
        };
    }
}
