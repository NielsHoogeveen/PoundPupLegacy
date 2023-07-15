using PoundPupLegacy.DomainModel;

namespace PoundPupLegacy.DomainModel.Inserters;

internal sealed class SubdivisionTypeInserterFactory : SingleIdInserterFactory<SubdivisionType.ToCreate>
{
    protected override string TableName => "subdivision_type";

    protected override bool AutoGenerateIdentity => false;

}
