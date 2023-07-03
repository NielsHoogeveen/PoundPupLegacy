namespace PoundPupLegacy.CreateModel;

public interface PrincipalToCreate: Principal, PossiblyIdentifiable
{
}

public interface PrincipalToUpdate: Principal, CertainlyIdentifiable
{
}

public interface Principal
{
}

