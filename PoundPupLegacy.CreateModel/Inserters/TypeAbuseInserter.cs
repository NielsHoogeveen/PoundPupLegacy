namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class TypeOfAbuseInserterFactory : SingleIdInserterFactory<TypeOfAbuse>
{
    protected override string TableName => "type_of_abuse";

    protected override bool AutoGenerateIdentity => false;

}
