namespace PoundPupLegacy.DomainModel;

public interface MemberOfCongressToCreateForNewPerson : MemberOfCongress, ProfessionalRoleToCreateForNewPerson
{
}
public interface MemberOfCongressToCreateForExistingPerson : MemberOfCongress, ProfessionalRoleToCreateForExistingPerson
{
}
public interface MemberOfCongressToUpdate : MemberOfCongress, ProfessionalRoleToUpdate
{
}
public interface MemberOfCongress : ProfessionalRole
{
}

