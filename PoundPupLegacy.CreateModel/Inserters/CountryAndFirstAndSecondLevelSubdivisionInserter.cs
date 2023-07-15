﻿namespace PoundPupLegacy.DomainModel.Inserters;

internal sealed class CountryAndFirstAndSecondLevelSubdivisionInserterFactory : SingleIdInserterFactory<CountryAndFirstAndSecondLevelSubdivision.ToCreate>
{
    protected override string TableName => "country_and_first_and_second_level_subdivision";

    protected override bool AutoGenerateIdentity => false;

}
