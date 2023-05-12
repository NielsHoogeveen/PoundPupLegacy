namespace PoundPupLegacy.Convert;

internal static class DependencyInjection
{

    public static async Task Migrate<T>(this IServiceProvider serviceProvider)
        where T : Migrator
    {
        await serviceProvider.GetService<T>()!.Migrate();
    }

    private static void AddMigrator<T>(this IServiceCollection services)
        where T : Migrator
    {
        services.AddTransient<T>();
    }
    public static void AddMigrators(this IServiceCollection services)
    {
        services.AddMigrator<AbuseCaseMigrator>();
        services.AddMigrator<AbuseCaseTypeOfAbuseMigrator>();
        services.AddMigrator<AbuseCaseTypeOfAbuserMigrator>();
        services.AddMigrator<AccessRolePrivilegeMigrator>();
        services.AddMigrator<ActionMigrator>();
        services.AddMigrator<ActMigrator>();
        services.AddMigrator<AdoptionImportMigrator>();
        services.AddMigrator<AdultAftermathMigrator>();
        services.AddMigrator<ArticleMigrator>();
        services.AddMigrator<AuthoringStatusMigrator>();
        services.AddMigrator<BasicCountryMigrator>();
        services.AddMigrator<BasicNameableMigrator>();
        services.AddMigrator<BasicSecondLevelSubdivisionMigrator>();
        services.AddMigrator<BillActionTypeMigrator>();
        services.AddMigrator<BillMigrator>();
        services.AddMigrator<BindingCountryMigrator>();
        services.AddMigrator<BlogPostMigrator>();
        services.AddMigrator<BoundCountryMigrator>();
        services.AddMigrator<CaseCaseRelationsMigrator>();
        services.AddMigrator<CasePartyTypeMigrator>();
        services.AddMigrator<CaseTypeMigrator>();
        services.AddMigrator<ChildPlacementTypeMigrator>();
        services.AddMigrator<ChildTraffickingCaseMigrator>();
        services.AddMigrator<CoercedAdoptionCaseMigrator>();
        services.AddMigrator<CommentMigrator>();
        services.AddMigrator<CountryAndFirstAndSecondLevelSubdivisionMigrator>();
        services.AddMigrator<CountryAndFirstLevelSubDivisionMigrator>();
        services.AddMigrator<CountrySubdivisionTypeMigratorPartOne>();
        services.AddMigrator<CountrySubdivisionTypeMigratorPartTwo>();
        services.AddMigrator<CountrySubdivisionTypeMigratorPartThree>();
        services.AddMigrator<DenominationMigrator>();
        services.AddMigrator<DeportationCaseMigrator>();
        services.AddMigrator<DiscussionMigrator>();
        services.AddMigrator<DisruptedPlacementCaseMigrator>();
        services.AddMigrator<DocumentableDocumentMigrator>();
        services.AddMigrator<DocumentMigratorCPCT>();
        services.AddMigrator<DocumentMigratorPPL>();
        services.AddMigrator<DocumentTypeMigrator>();
        services.AddMigrator<FamilySizeMigrator>();
        services.AddMigrator<FathersRightsViolationsCaseMigrator>();
        services.AddMigrator<FileMigratorCPCT>();
        services.AddMigrator<FileMigratorPPL>();
        services.AddMigrator<FirstAndBottomLevelSubdivisionMigrator>();
        services.AddMigrator<FirstLevelGlobalRegionMigrator>();
        services.AddMigrator<FormalIntermediateLevelSubdivisionMigrator>();
        services.AddMigrator<HagueStatusMigrator>();
        services.AddMigrator<InformalIntermediateLevelSubdivisionMigrator>();
        services.AddMigrator<InterCountryRelationTypeMigrator>();
        services.AddMigrator<InterOrganizationalRelationMigratorCPCT>();
        services.AddMigrator<InterOrganizationalRelationMigratorPPL>();
        services.AddMigrator<InterOrganizationalRelationTypeMigrator>();
        services.AddMigrator<InterPersonalRelationMigratorCPCT>();
        services.AddMigrator<InterPersonalRelationMigratorPPL>();
        services.AddMigrator<InterPersonalRelationTypeMigrator>();
        services.AddMigrator<LocationMigratorCPCT>();
        services.AddMigrator<LocationMigratorPPL>();
        services.AddMigrator<LocationMigratorCPCT>();
        services.AddMigrator<MemberOfCongressMigrator>();
        services.AddMigrator<MenuMigrator>();
        services.AddMigrator<NodeFileMigratorCPCT>();
        services.AddMigrator<NodeFileMigratorPPL>();
        services.AddMigrator<NodeTermMigrator>();
        services.AddMigrator<NodeTypeMigrator>();
        services.AddMigrator<OrganizationMigratorCPCT>();
        services.AddMigrator<OrganizationMigratorPPL>();
        services.AddMigrator<OrganizationTypeMigrator>();
        services.AddMigrator<PageMigrator>();
        services.AddMigrator<PartyPoliticalEntityRelationMigratorCPCT>();
        services.AddMigrator<PartyPoliticalEntityRelationMigratorPPL>();
        services.AddMigrator<PartyPoliticalEntityRelationTypeMigrator>();
        services.AddMigrator<PersonMigratorCPCT>();
        services.AddMigrator<PersonMigratorPPL>();
        services.AddMigrator<PersonOrganizationRelationTypeMigrator>();
        services.AddMigrator<PersonOrganizationRelationMigratorCPCT>();
        services.AddMigrator<PersonOrganizationRelationMigratorPPL>();
        services.AddMigrator<PollMigrator>();
        services.AddMigrator<PollStatusMigrator>();
        services.AddMigrator<ProfessionMigrator>();
        services.AddMigrator<PublicationStatusMigrator>();
        services.AddMigrator<RepresentativeHouseBillActionMigrator>();
        services.AddMigrator<ReviewMigrator>();
        services.AddMigrator<SearchableMigrator>();
        services.AddMigrator<SecondLevelGlobalRegionMigrator>();
        services.AddMigrator<SenatorSenateBillActionMigrator>();
        services.AddMigrator<SubdivisionTypeMigrator>();
        services.AddMigrator<TermHierarchyMigrator>();
        services.AddMigrator<TypeOfAbuseMigrator>();
        services.AddMigrator<TypeOfAbuserMigrator>();
        services.AddMigrator<UnitedStatesCongressionalMeetingMigrator>();
        services.AddMigrator<UnitedStatesPoliticalPartyAffliationMigrator>();
        services.AddMigrator<UserMigrator>();
        services.AddMigrator<VocabularyMigrator>();
        services.AddMigrator<WrongfulMedicationCaseMigrator>();
        services.AddMigrator<WrongfulRemovalCaseMigrator>();
    }
}
