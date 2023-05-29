namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class WrongfulRemovalCaseInserterFactory : SingleIdInserterFactory<WrongfulRemovalCase.WrongfulRemovalCaseToCreate>
{
    protected override string TableName => "wrongful_removal_case";

    protected override bool AutoGenerateIdentity => false;
}
