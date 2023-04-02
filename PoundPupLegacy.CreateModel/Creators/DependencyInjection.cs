using Microsoft.Extensions.DependencyInjection;

namespace PoundPupLegacy.CreateModel.Creators;

public static class DependencyInjection
{
    public static void AddEntityCreators(this IServiceCollection services)
    {
        services.AddTransient<IEntityCreator<AbuseCase>, AbuseCaseCreator>();
        services.AddTransient<IEntityCreator<AccessRole>, AccessRoleCreator>();
        services.AddTransient<IEntityCreator<AccessRolePrivilege>, AccessRolePrivilegeCreator>();
        services.AddTransient<IEntityCreator<Act>, ActCreator>();
        services.AddTransient<IEntityCreator<ActionMenuItem>, ActionMenuItemCreator>();
        services.AddTransient<IEntityCreator<ActionMenuItem>, ActionMenuItemCreator>();
        services.AddTransient<IAnonimousUserCreator, AnonimousUserCreator>();
        services.AddTransient<IEntityCreator<Article>, ArticleCreator>();
        services.AddTransient<IEntityCreator<BasicAction>, BasicActionCreator>();
        services.AddTransient<IEntityCreator<BasicCountry>, BasicCountryCreator>();
        services.AddTransient<IEntityCreator<BasicNameable>, BasicNameableCreator>();
        services.AddTransient<IEntityCreator<BasicSecondLevelSubdivision>, BasicSecondLevelSubdivisionCreator>();
        services.AddTransient<IEntityCreator<BillActionType>, BillActionTypeCreator>();
        services.AddTransient<IEntityCreator<BindingCountry>, BindingCountryCreator>();
        services.AddTransient<IEntityCreator<BlogPost>, BlogPostCreator>();
        services.AddTransient<IEntityCreator<BoundCountry>, BoundCountryCreator>();
        services.AddTransient<IEntityCreator<CaseCaseParties>, CaseCasePartiesCreator>();
        services.AddTransient<IEntityCreator<CasePartyType>, CasePartyTypeCreator>();
        services.AddTransient<IEntityCreator<CaseType>, CaseTypeCreator>();
        services.AddTransient<IEntityCreator<CasePartyType>, CasePartyTypeCreator>();
        services.AddTransient<IEntityCreator<ChildTraffickingCase>, ChildTraffickingCaseCreator>();
        services.AddTransient<IEntityCreator<CoercedAdoptionCase>, CoercedAdoptionCaseCreator>();
        services.AddTransient<IEntityCreator<Collective>, CollectiveCreator>();
        services.AddTransient<IEntityCreator<CollectiveUser>, CollectiveUserCreator>();
        services.AddTransient<IEntityCreator<Comment>, CommentCreator>();
        services.AddTransient<IEntityCreator<CongressionalTermPoliticalPartyAffiliation>, CongressionalTermPoliticalPartyAffiliationCreator>();
        services.AddTransient<IEntityCreator<ContentSharingGroup>, ContentSharingGroupCreator>();
        services.AddTransient<IEntityCreator<CountryAndFirstAndBottomLevelSubdivision>, CountryAndFirstAndBottomLevelSubdivisionCreator>();
        services.AddTransient<IEntityCreator<CountryAndFirstAndSecondLevelSubdivision>, CountryAndFirstAndSecondLevelSubdivisionCreator>();
        services.AddTransient<IEntityCreator<CountryAndIntermediateLevelSubdivision>, CountryAndIntermediateLevelSubdivisionCreator>();
        services.AddTransient<IEntityCreator<CountrySubdivisionType>, CountrySubdivisionTypeCreator>();
        services.AddTransient<IEntityCreator<CreateNodeAction>, CreateNodeActionCreator>();
        services.AddTransient<IEntityCreator<DeleteNodeAction>, DeleteNodeActionCreator>();
        services.AddTransient<IEntityCreator<Denomination>, DenominationCreator>();
        services.AddTransient<IEntityCreator<DeportationCase>, DeportationCaseCreator>();
        services.AddTransient<IEntityCreator<Discussion>, DiscussionCreator>();
        services.AddTransient<IEntityCreator<DisruptedPlacementCase>, DisruptedPlacementCaseCreator>();
        services.AddTransient<IEntityCreator<DocumentableDocument>, DocumentableDocumentCreator>();
        services.AddTransient<IEntityCreator<Document>, DocumentCreator>();
        services.AddTransient<IEntityCreator<DocumentType>, DocumentTypeCreator>();
        services.AddTransient<IEntityCreator<EditNodeAction>, EditNodeActionCreator>();
        services.AddTransient<IEntityCreator<EditOwnNodeAction>, EditOwnNodeActionCreator>();
        services.AddTransient<IEntityCreator<FamilySize>, FamilySizeCreator>();
        services.AddTransient<IEntityCreator<FathersRightsViolationCase>, FathersRightsViolationCaseCreator>();
        services.AddTransient<IEntityCreator<File>, FileCreator>();
        services.AddTransient<IEntityCreator<FirstAndBottomLevelSubdivision>, FirstAndBottomLevelSubdivisionCreator>();
        services.AddTransient<IEntityCreator<FirstLevelGlobalRegion>, FirstLevelGlobalRegionCreator>();
        services.AddTransient<IEntityCreator<FormalIntermediateLevelSubdivision>, FormalIntermediateLevelSubdivisionCreator>();
        services.AddTransient<IEntityCreator<HagueStatus>, HagueStatusCreator>();
        services.AddTransient<IEntityCreator<HouseBill>, HouseBillCreator>();
        services.AddTransient<IEntityCreator<HouseTerm>, HouseTermCreator>();
        services.AddTransient<IEntityCreator<InformalIntermediateLevelSubdivision>, InformalIntermediateLevelSubdivisionCreator>();
        services.AddTransient<IEntityCreator<InterCountryRelation>, InterCountryRelationCreator>();
        services.AddTransient<IEntityCreator<InterCountryRelationType>, InterCountryRelationTypeCreator>();
        services.AddTransient<IEntityCreator<InterOrganizationalRelation>, InterOrganizationalRelationCreator>();
        services.AddTransient<IEntityCreator<InterOrganizationalRelationType>, InterOrganizationalRelationTypeCreator>();
        services.AddTransient<IEntityCreator<InterPersonalRelation>, InterPersonalRelationCreator>();
        services.AddTransient<IEntityCreator<InterPersonalRelationType>, InterPersonalRelationTypeCreator>();
        services.AddTransient<IEntityCreator<Location>, LocationCreator>();
        services.AddTransient<IEntityCreator<LocationLocatable>, LocationLocatableCreator>();
        services.AddTransient<IEntityCreator<MultiQuestionPoll>, MultiQuestionPollCreator>();
        services.AddTransient<IEntityCreator<NodeFile>, NodeFileCreator>();
        services.AddTransient<IEntityCreator<NodeTerm>, NodeTermCreator>();
        services.AddTransient<IEntityCreator<BasicNodeType>, NodeTypeCreator>();
        services.AddTransient<IEntityCreator<Organization>, OrganizationCreator>();
        services.AddTransient<IEntityCreator<OrganizationType>, OrganizationTypeCreator>();
        services.AddTransient<IEntityCreator<Page>, PageCreator>();
        services.AddTransient<IEntityCreator<PartyPoliticalEntityRelation>, PartyPoliticalEntityRelationCreator>();
        services.AddTransient<IEntityCreator<PartyPoliticalEntityRelationType>, PartyPoliticalEntityRelationTypeCreator>();
        services.AddTransient<IEntityCreator<Person>, PersonCreator>();
        services.AddTransient<IEntityCreator<PersonOrganizationRelation>, PersonOrganizationRelationCreator>();
        services.AddTransient<IEntityCreator<PersonOrganizationRelationType>, PersonOrganizationRelationTypeCreator>();
        services.AddTransient<IEntityCreator<PollStatus>, PollStatusCreator>();
        services.AddTransient<IEntityCreator<ProfessionalRole>, ProfessionalRoleCreator>();
        services.AddTransient<IEntityCreator<Profession>, ProfessionCreator>();
        services.AddTransient<IEntityCreator<PublicationStatus>, PublicationStatusCreator>();
        services.AddTransient<IEntityCreator<RepresentativeHouseBillAction>, RepresentativeHouseBillActionCreator>();
        services.AddTransient<IEntityCreator<Review>, ReviewCreator>();
        services.AddTransient<IEntityCreator<SecondLevelGlobalRegion>, SecondLevelGlobalRegionCreator>();
        services.AddTransient<IEntityCreator<SenateBill>, SenateBillCreator>();
        services.AddTransient<IEntityCreator<SenateTerm>, SenateTermCreator>();
        services.AddTransient<IEntityCreator<SenatorSenateBillAction>, SenatorSenateBillActionCreator>();
        services.AddTransient<IEntityCreator<SingleQuestionPoll>, SingleQuestionPollCreator>();
        services.AddTransient<IEntityCreator<SubdivisionType>, SubdivisionTypeCreator>();
        services.AddTransient<IEntityCreator<Subgroup>, SubgroupCreator>();
        services.AddTransient<ISystemGroupCreator, SystemGroupCreator>();
        services.AddTransient<IEntityCreator<Tenant>, TenantCreator>();
        services.AddTransient<IEntityCreator<TenantNodeMenuItem>, TenantNodeMenuItemCreator>();
        services.AddTransient<IEntityCreator<TermHierarchy>, TermHierarchyCreator>();
        services.AddTransient<IEntityCreator<TypeOfAbuse>, TypeOfAbuseCreator>();
        services.AddTransient<IEntityCreator<TypeOfAbuser>, TypeOfAbuserCreator>();
        services.AddTransient<IEntityCreator<UnitedStatesCongressionalMeeting>, UnitedStatesCongressionalMeetingCreator>();
        services.AddTransient<IEntityCreator<UnitedStatesPoliticalPartyAffliation>, UnitedStatesPoliticalPartyAffliationCreator>();
        services.AddTransient<IEntityCreator<User>, UserCreator>();
        services.AddTransient<IEntityCreator<UserGroupUserRoleUser>, UserGroupUserRoleUserCreator>();
        services.AddTransient<IEntityCreator<Vocabulary>, VocabularyCreator>();
        services.AddTransient<IEntityCreator<WrongfulMedicationCase>, WrongfulMedicationCaseCreator>();
        services.AddTransient<IEntityCreator<WrongfulRemovalCase>, WrongfulRemovalCaseCreator>();


















    }
}
