using PoundPupLegacy.DomainModel;

namespace PoundPupLegacy.DomainModel.Inserters;

internal sealed class RepresentativeInserterFactory : SingleIdInserterFactory<Representative.ToCreateForExistingPerson>
{
    protected override string TableName => "representative";

    protected override bool AutoGenerateIdentity => false;

}
