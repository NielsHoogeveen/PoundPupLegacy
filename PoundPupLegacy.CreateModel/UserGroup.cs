﻿namespace PoundPupLegacy.CreateModel;

public interface UserGroup : Identifiable
{
    public string Name { get; }
    public string Description { get; }

    public AdministratorRole AdministratorRole { get; }

}