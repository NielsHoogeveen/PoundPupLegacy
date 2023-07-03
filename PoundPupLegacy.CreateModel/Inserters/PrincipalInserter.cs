﻿namespace PoundPupLegacy.CreateModel.Inserters;

using Request = PrincipalToCreate;

public class PrincipalInserterFactory : ConditionalAutoGenerateIdDatabaseInserterFactory<Request>
{
    public override string TableName => "principal";
    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
        };
    }

}
