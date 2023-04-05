namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class SystemGroupInserterFactory : SingleIdInserterFactory<SystemGroup>
{
    protected override string TableName => "system_group";

    protected override bool AutoGenerateIdentity => false;

}
