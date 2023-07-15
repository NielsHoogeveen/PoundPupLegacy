namespace PoundPupLegacy.DomainModel.Inserters;

internal sealed class AccessRoleInserterFactory : SingleIdInserterFactory<AccessRole>
{
    protected override string TableName => "access_role";

    protected override bool AutoGenerateIdentity => false;

}

