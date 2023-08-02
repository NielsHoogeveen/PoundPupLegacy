﻿namespace PoundPupLegacy.DomainModel.Inserters;

internal sealed class MemberOfCongressInserterFactory : SingleIdInserterFactory<MemberOfCongressToCreateForExistingPerson>
{
    protected override string TableName => "member_of_congress";

    protected override bool AutoGenerateIdentity => false;

}