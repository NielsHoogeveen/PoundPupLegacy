﻿using PoundPupLegacy.DomainModel;

namespace PoundPupLegacy.DomainModel.Inserters;

using Request = PersonOrganizationRelationType.ToCreate;

internal sealed class PersonOrganizationRelationTypeInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    private static readonly NonNullableBooleanDatabaseParameter HasConcreteSubtype = new() { Name = "has_concrete_subtype" };

    public override string TableName => "person_organization_relation_type";
    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(HasConcreteSubtype, request.PersonOrganizationRelationTypeDetails.HasConcreteSubtype),
        };
    }
}