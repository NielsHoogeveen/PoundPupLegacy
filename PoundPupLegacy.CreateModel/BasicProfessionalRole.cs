namespace PoundPupLegacy.CreateModel;

public sealed record BasicProfessionalRole : ProfessionalRoleBase, IdentifiableProfessionalRole
{
    public required int? Id { get; set; }
}
