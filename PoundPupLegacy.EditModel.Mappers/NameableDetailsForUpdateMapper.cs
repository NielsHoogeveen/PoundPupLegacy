namespace PoundPupLegacy.EditModel.Mappers;

internal class NameableDetailsForUpdateMapper : IMapper<NameableDetails, CreateModel.NameableDetails.ForUpdate>
{
    public CreateModel.NameableDetails.ForUpdate Map(NameableDetails source)
    {
        return new CreateModel.NameableDetails.ForUpdate {
            TermsToAdd = new List<CreateModel.Term.ToCreateForExistingNameable>(),
            Description = source.Description,
            FileIdTileImage = null
        };
    }
}
