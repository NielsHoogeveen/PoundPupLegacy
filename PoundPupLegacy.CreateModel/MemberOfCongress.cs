﻿namespace PoundPupLegacy.CreateModel;

public interface EventuallyIdentifiableMemberOfCongressForExistingPerson : EventuallyIdentifiableMemberOfCongress, EventuallyIdentifiableProfessionalRoleForExistingPerson
{
}
public interface EventuallyIdentifiableMemberOfCongressForNewPerson : EventuallyIdentifiableMemberOfCongress, EventuallyIdentifiableProfessionalRoleForNewPerson
{
}
public interface ImmediatelyIdentifiableMemberOfCongress : MemberOfCongress, ImmediatelyIdentifiableProfessionalRole
{
}

public interface EventuallyIdentifiableMemberOfCongress : MemberOfCongress, EventuallyIdentifiableProfessionalRole
{
}

public interface MemberOfCongress : ProfessionalRole
{
}
