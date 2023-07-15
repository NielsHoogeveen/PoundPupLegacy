using PoundPupLegacy.DomainModel;

namespace PoundPupLegacy.DomainModel.Inserters;

internal sealed class BasicNameableInserterFactory : SingleIdInserterFactory<BasicNameable.ToCreate>
{
    protected override string TableName => "basic_nameable";

    protected override bool AutoGenerateIdentity => false;

}
