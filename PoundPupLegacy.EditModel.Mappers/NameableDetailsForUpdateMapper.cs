namespace PoundPupLegacy.EditModel.Mappers;

internal class NameableDetailsForUpdateMapper : IMapper<NameableDetails, DomainModel.NameableDetails.ForUpdate>
{
    public DomainModel.NameableDetails.ForUpdate Map(NameableDetails source)
    {
        return new DomainModel.NameableDetails.ForUpdate {
            TermsToAdd = new List<DomainModel.Term.ToCreateForExistingNameable>(),
            TermsToUpdate = new List<DomainModel.Term.ToUpdate>(),
            TermsToRemove = new List<int>(),
            Description = source.Description,
            FileIdTileImage = null
        };
    }
}
