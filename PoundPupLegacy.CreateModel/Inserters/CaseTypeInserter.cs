namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class CaseTypeInserterFactory : SingleIdInserterFactory<CaseType>
{
    protected override string TableName => "case_type";

    protected override bool AutoGenerateIdentity => false;

}
