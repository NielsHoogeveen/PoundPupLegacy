namespace PoundPupLegacy.EditModel.Mappers;

internal class NameableDetailsForCreateMapper : IMapper<NameableDetails, CreateModel.NameableDetails.ForCreate>
{
    public CreateModel.NameableDetails.ForCreate Map(NameableDetails source)
    {
        return new CreateModel.NameableDetails.ForCreate {
            Description = source.Description,
            FileIdTileImage = null,
            Terms = new List<CreateModel.Term.ToCreateForNewNameable>(),
        };
    }
}
