﻿namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class CoercedAdoptionCaseInserterFactory : SingleIdInserterFactory<CoercedAdoptionCase>
{
    protected override string TableName => "coerced_adoption_case";

    protected override bool AutoGenerateIdentity => false;

}
