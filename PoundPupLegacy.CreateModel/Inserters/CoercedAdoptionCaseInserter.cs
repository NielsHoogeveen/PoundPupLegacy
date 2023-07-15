using PoundPupLegacy.DomainModel;

namespace PoundPupLegacy.DomainModel.Inserters;

internal sealed class CoercedAdoptionCaseInserterFactory : SingleIdInserterFactory<CoercedAdoptionCase.ToCreate>
{
    protected override string TableName => "coerced_adoption_case";

    protected override bool AutoGenerateIdentity => false;

}
