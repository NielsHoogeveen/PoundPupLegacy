namespace PoundPupLegacy.EditModel.Mappers;

internal class NameableDetailsForCreateMapper : IMapper<NameableDetails, DomainModel.NameableDetails.ForCreate>
{
    public DomainModel.NameableDetails.ForCreate Map(NameableDetails source)
    {
        return new DomainModel.NameableDetails.ForCreate {
            Description = source.Description,
            FileIdTileImage = null,
            Terms = new List<DomainModel.Term.ToCreateForNewNameable>() { 
                new DomainModel.Term.ToCreateForNewNameable {
                    Identification = new Identification.Possible{ Id = null},
                    Name = source.Name,
                    ParentTermIds = new List<int>(),
                    VocabularyId = source.VocabularyId
                }
            },
        };
    }
}
