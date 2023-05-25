﻿namespace PoundPupLegacy.CreateModel;

public interface EventuallyIdentifiableMemberOfCongressForExistingPerson : MemberOfCongress, EventuallyIdentifiableProfessionalRoleForExistingPerson
{
}
public interface EventuallyIdentifiableMemberOfCongressForNewPerson : MemberOfCongress, EventuallyIdentifiableProfessionalRoleForNewPerson
{
}
public interface ImmediatelyIdentifiableMemberOfCongress : MemberOfCongress, ImmediatelyIdentifiableProfessionalRole
{
}
public interface MemberOfCongress : ProfessionalRole
{
}
