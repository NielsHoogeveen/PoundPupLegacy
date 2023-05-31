namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class NameableDetailsToUpdateMapper : IMapper<EditModel.NameableDetails, CreateModel.NameableDetails.ForUpdate>
{
    public CreateModel.NameableDetails.ForUpdate Map(EditModel.NameableDetails source)
    {
        return new CreateModel.NameableDetails.ForUpdate {
            TermsToAdd = new List<CreateModel.Term.ToCreateForExistingNameable>(),
            Description = source.Description,
            FileIdTileImage = null
        };
    }
}
