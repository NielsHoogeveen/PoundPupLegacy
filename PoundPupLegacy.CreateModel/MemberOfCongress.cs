namespace PoundPupLegacy.CreateModel;

public interface MemberOfCongressToCreate : MemberOfCongress, ProfessionalRoleToCreate
{
}
public interface MemberOfCongressToUpdate : MemberOfCongress, ProfessionalRoleToUpdate
{
}
public interface MemberOfCongress : ProfessionalRole
{
}
