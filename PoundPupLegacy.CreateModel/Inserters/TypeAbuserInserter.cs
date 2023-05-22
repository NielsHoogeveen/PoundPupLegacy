namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class TypeOfAbuserInserterFactory : SingleIdInserterFactory<NewTypeOfAbuser>
{
    protected override string TableName => "type_of_abuser";

    protected override bool AutoGenerateIdentity => false;

}
