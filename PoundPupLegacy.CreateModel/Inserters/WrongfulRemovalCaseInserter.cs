using PoundPupLegacy.DomainModel;

namespace PoundPupLegacy.DomainModel.Inserters;

internal sealed class WrongfulRemovalCaseInserterFactory : SingleIdInserterFactory<WrongfulRemovalCase.ToCreate>
{
    protected override string TableName => "wrongful_removal_case";

    protected override bool AutoGenerateIdentity => false;
}
