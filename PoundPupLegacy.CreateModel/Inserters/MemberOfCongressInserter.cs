namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class MemberOfCongressInserterFactory : SingleIdInserterFactory<IdentifiableMemberOfCongress>
{
    protected override string TableName => "member_of_congress";

    protected override bool AutoGenerateIdentity => false;

}
