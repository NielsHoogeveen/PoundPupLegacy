namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class SubdivisionTypeInserterFactory : SingleIdInserterFactory<NewSubdivisionType>
{
    protected override string TableName => "subdivision_type";

    protected override bool AutoGenerateIdentity => false;

}
