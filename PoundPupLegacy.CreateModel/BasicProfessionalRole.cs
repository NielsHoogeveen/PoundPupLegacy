namespace PoundPupLegacy.CreateModel;

public sealed record BasicProfessionalRole : ProfessionalRoleBase, EventuallyIdentifiableProfessionalRole
{
    public required int? Id { get; set; }
}
