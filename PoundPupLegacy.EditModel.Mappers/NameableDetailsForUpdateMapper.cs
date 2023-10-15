namespace PoundPupLegacy.EditModel.Mappers;

internal class NameableDetailsForUpdateMapper : IMapper<NameableDetails, DomainModel.NameableDetails.ForUpdate>
{
    public DomainModel.NameableDetails.ForUpdate Map(NameableDetails source)
    {
        return new DomainModel.NameableDetails.ForUpdate {
            TermsToAdd = new List<DomainModel.Term.ToCreateForExistingNameable>(),
            TermToUpdate = new DomainModel.Term.ToUpdate { 
                Identification = new Identification.Certain {
                    Id = source.TopicId!.Value,
                },
                Name = source.Name,
                NameableId = source.TopicId!.Value,
                ParentTermIds = new List<int>(),
                VocabularyId = source.VocabularyId,
            },
            Description = source.Description,
            FileIdTileImage = null
        };
    }
}
