namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class TypeOfAbuserInserterFactory : SingleIdInserterFactory<EventuallyIdentifiableTypeOfAbuser>
{
    protected override string TableName => "type_of_abuser";

    protected override bool AutoGenerateIdentity => false;

}
