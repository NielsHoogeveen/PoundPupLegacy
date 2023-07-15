using PoundPupLegacy.DomainModel;

namespace PoundPupLegacy.DomainModel.Inserters;

internal sealed class TypeOfAbuserInserterFactory : SingleIdInserterFactory<TypeOfAbuser.ToCreate>
{
    protected override string TableName => "type_of_abuser";

    protected override bool AutoGenerateIdentity => false;

}
