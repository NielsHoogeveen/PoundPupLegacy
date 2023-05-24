﻿namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class MemberOfCongressInserterFactory : SingleIdInserterFactory<EventuallyIdentifiableMemberOfCongress>
{
    protected override string TableName => "member_of_congress";

    protected override bool AutoGenerateIdentity => false;

}
