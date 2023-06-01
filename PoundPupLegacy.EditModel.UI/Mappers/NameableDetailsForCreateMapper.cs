namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class NameableDetailsForCreateMapper : IMapper<EditModel.NameableDetails, CreateModel.NameableDetails.ForCreate>
{
    public CreateModel.NameableDetails.ForCreate Map(EditModel.NameableDetails source)
    {
        return new CreateModel.NameableDetails.ForCreate {
            Description = source.Description,
            FileIdTileImage = null,
            Terms = new List<CreateModel.Term.ToCreateForNewNameable>(),
        };
    }
}
