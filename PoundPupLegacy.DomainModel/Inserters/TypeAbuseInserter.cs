namespace PoundPupLegacy.DomainModel.Inserters;

internal sealed class TypeOfAbuseInserterFactory : SingleIdInserterFactory<TypeOfAbuse.ToCreate>
{
    protected override string TableName => "type_of_abuse";

    protected override bool AutoGenerateIdentity => false;

}
