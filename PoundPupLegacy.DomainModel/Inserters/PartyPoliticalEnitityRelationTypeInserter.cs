﻿using PoundPupLegacy.DomainModel;

namespace PoundPupLegacy.DomainModel.Inserters;

using Request = PartyPoliticalEntityRelationType.ToCreate;

internal sealed class PartyPoliticalEntityRelationTypeInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    private static readonly NonNullableBooleanDatabaseParameter HasConcreteSubtype = new() { Name = "has_concrete_subtype" };

    public override string TableName => "party_political_entity_relation_type";
    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(HasConcreteSubtype, request.PartyPoliticalEntityRelationTypeDetails.HasConcreteSubtype),
        };
    }
}