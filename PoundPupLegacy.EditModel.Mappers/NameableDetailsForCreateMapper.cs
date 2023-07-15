namespace PoundPupLegacy.EditModel.Mappers;

internal class NameableDetailsForCreateMapper : IMapper<NameableDetails, DomainModel.NameableDetails.ForCreate>
{
    public DomainModel.NameableDetails.ForCreate Map(NameableDetails source)
    {
        return new DomainModel.NameableDetails.ForCreate {
            Description = source.Description,
            FileIdTileImage = null,
            Terms = new List<DomainModel.Term.ToCreateForNewNameable>(),
        };
    }
}
