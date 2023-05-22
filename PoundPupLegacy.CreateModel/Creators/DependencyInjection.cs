using Microsoft.Extensions.DependencyInjection;

namespace PoundPupLegacy.CreateModel.Creators;

internal static class DependencyInjection
{
    internal static void AddCreateModelCreators(this IServiceCollection services)
    {
        services.AddCreateModelInserters();
        services.AddTransient<IEntityCreator<NewAbuseCase>, AbuseCaseCreator>();
        services.AddTransient<IEntityCreator<AbuseCaseTypeOfAbuse>, AbuseCaseTypeOfAbuseCreator>();
        services.AddTransient<IEntityCreator<AbuseCaseTypeOfAbuser>, AbuseCaseTypeOfAbuserCreator>();
        services.AddTransient<IEntityCreator<AccessRole>, AccessRoleCreator>();
        services.AddTransient<IEntityCreator<AccessRolePrivilege>, AccessRolePrivilegeCreator>();
        services.AddTransient<IEntityCreator<NewAct>, ActCreator>();
        services.AddTransient<IEntityCreator<ActionMenuItem>, ActionMenuItemCreator>();
        services.AddTransient<IEntityCreator<ActionMenuItem>, ActionMenuItemCreator>();
        services.AddTransient<IAnonimousUserCreator, AnonimousUserCreator>();
        services.AddTransient<IEntityCreator<AuthoringStatus>, AuthoringStatusCreator>();
        services.AddTransient<IEntityCreator<BasicAction>, BasicActionCreator>();
        services.AddTransient<IEntityCreator<NewBasicCountry>, BasicCountryCreator>();
        services.AddTransient<IEntityCreator<NewBasicNameable>, BasicNameableCreator>();
        services.AddTransient<IEntityCreator<BasicNameableType>, BasicNameableTypeCreator>();
        services.AddTransient<IEntityCreator<NewBasicSecondLevelSubdivision>, BasicSecondLevelSubdivisionCreator>();
        services.AddTransient<IEntityCreator<NewBillActionType>, BillActionTypeCreator>();
        services.AddTransient<IEntityCreator<NewBindingCountry>, BindingCountryCreator>();
        services.AddTransient<IEntityCreator<NewBlogPost>, BlogPostCreator>();
        services.AddTransient<IEntityCreator<NewBoundCountry>, BoundCountryCreator>();
        services.AddTransient<IEntityCreator<CaseCaseParties>, CaseCasePartiesCreator>();
        services.AddTransient<IEntityCreator<NewCasePartyType>, CasePartyTypeCreator>();
        services.AddTransient<IEntityCreator<CaseType>, CaseTypeCreator>();
        services.AddTransient<IEntityCreator<NewCasePartyType>, CasePartyTypeCreator>();
        services.AddTransient<IEntityCreator<NewChildPlacementType>, ChildPlacementTypeCreator>();
        services.AddTransient<IEntityCreator<NewChildTraffickingCase>, ChildTraffickingCaseCreator>();
        services.AddTransient<IEntityCreator<NewCoercedAdoptionCase>, CoercedAdoptionCaseCreator>();
        services.AddTransient<IEntityCreator<Collective>, CollectiveCreator>();
        services.AddTransient<IEntityCreator<CollectiveUser>, CollectiveUserCreator>();
        services.AddTransient<IEntityCreator<Comment>, CommentCreator>();
        services.AddTransient<IEntityCreator<NewCongressionalTermPoliticalPartyAffiliation>, CongressionalTermPoliticalPartyAffiliationCreator>();
        services.AddTransient<IEntityCreator<ContentSharingGroup>, ContentSharingGroupCreator>();
        services.AddTransient<IEntityCreator<NewCountryAndFirstAndBottomLevelSubdivision>, CountryAndFirstAndBottomLevelSubdivisionCreator>();
        services.AddTransient<IEntityCreator<NewCountryAndFirstAndSecondLevelSubdivision>, CountryAndFirstAndSecondLevelSubdivisionCreator>();
        services.AddTransient<IEntityCreator<NewCountryAndIntermediateLevelSubdivision>, CountryAndIntermediateLevelSubdivisionCreator>();
        services.AddTransient<IEntityCreator<CountrySubdivisionType>, CountrySubdivisionTypeCreator>();
        services.AddTransient<IEntityCreator<CreateNodeAction>, CreateNodeActionCreator>();
        services.AddTransient<IEntityCreator<DeleteNodeAction>, DeleteNodeActionCreator>();
        services.AddTransient<IEntityCreator<NewDenomination>, DenominationCreator>();
        services.AddTransient<IEntityCreator<NewDeportationCase>, DeportationCaseCreator>();
        services.AddTransient<IEntityCreator<NewDiscussion>, DiscussionCreator>();
        services.AddTransient<IEntityCreator<NewDisruptedPlacementCase>, DisruptedPlacementCaseCreator>();
        services.AddTransient<IEntityCreator<DocumentableDocument>, DocumentableDocumentCreator>();
        services.AddTransient<IEntityCreator<NewDocument>, DocumentCreator>();
        services.AddTransient<IEntityCreator<NewDocumentType>, DocumentTypeCreator>();
        services.AddTransient<IEntityCreator<EditNodeAction>, EditNodeActionCreator>();
        services.AddTransient<IEntityCreator<EditOwnNodeAction>, EditOwnNodeActionCreator>();
        services.AddTransient<IEntityCreator<NewFamilySize>, FamilySizeCreator>();
        services.AddTransient<IEntityCreator<NewFathersRightsViolationCase>, FathersRightsViolationCaseCreator>();
        services.AddTransient<IEntityCreator<File>, FileCreator>();
        services.AddTransient<IEntityCreator<NewFirstAndBottomLevelSubdivision>, FirstAndBottomLevelSubdivisionCreator>();
        services.AddTransient<IEntityCreator<NewFirstLevelGlobalRegion>, FirstLevelGlobalRegionCreator>();
        services.AddTransient<IEntityCreator<NewFormalIntermediateLevelSubdivision>, FormalIntermediateLevelSubdivisionCreator>();
        services.AddTransient<IEntityCreator<NewHagueStatus>, HagueStatusCreator>();
        services.AddTransient<IEntityCreator<NewHouseBill>, HouseBillCreator>();
        services.AddTransient<IEntityCreator<NewHouseTerm>, HouseTermCreator>();
        services.AddTransient<IEntityCreator<NewInformalIntermediateLevelSubdivision>, InformalIntermediateLevelSubdivisionCreator>();
        services.AddTransient<IEntityCreator<NewInterCountryRelation>, InterCountryRelationCreator>();
        services.AddTransient<IEntityCreator<NewInterCountryRelationType>, InterCountryRelationTypeCreator>();
        services.AddTransient<IEntityCreator<NewInterOrganizationalRelation>, InterOrganizationalRelationCreator>();
        services.AddTransient<IEntityCreator<NewInterOrganizationalRelationType>, InterOrganizationalRelationTypeCreator>();
        services.AddTransient<IEntityCreator<NewInterPersonalRelation>, InterPersonalRelationCreator>();
        services.AddTransient<IEntityCreator<NewInterPersonalRelationType>, InterPersonalRelationTypeCreator>();
        services.AddTransient<IEntityCreator<Location>, LocationCreator>();
        services.AddTransient<IEntityCreator<LocationLocatable>, LocationLocatableCreator>();
        services.AddTransient<IEntityCreator<NewMultiQuestionPoll>, MultiQuestionPollCreator>();
        services.AddTransient<IEntityCreator<NodeFile>, NodeFileCreator>();
        services.AddTransient<IEntityCreator<NodeTerm>, NodeTermCreator>();
        services.AddTransient<IEntityCreator<BasicNodeType>, NodeTypeCreator>();
        services.AddTransient<IEntityCreator<EventuallyIdentifiableOrganization>, OrganizationCreator>();
        services.AddTransient<IEntityCreator<NewOrganizationType>, OrganizationTypeCreator>();
        services.AddTransient<IEntityCreator<NewPage>, PageCreator>();
        services.AddTransient<IEntityCreator<NewPartyPoliticalEntityRelation>, PartyPoliticalEntityRelationCreator>();
        services.AddTransient<IEntityCreator<NewPartyPoliticalEntityRelationType>, PartyPoliticalEntityRelationTypeCreator>();
        services.AddTransient<IEntityCreator<NewPerson>, PersonCreator>();
        services.AddTransient<IEntityCreator<NewPersonOrganizationRelation>, PersonOrganizationRelationCreator>();
        services.AddTransient<IEntityCreator<NewPersonOrganizationRelationType>, PersonOrganizationRelationTypeCreator>();
        services.AddTransient<IEntityCreator<PollStatus>, PollStatusCreator>();
        services.AddTransient<IEntityCreator<ProfessionalRole>, ProfessionalRoleCreator>();
        services.AddTransient<IEntityCreator<NewProfession>, ProfessionCreator>();
        services.AddTransient<IEntityCreator<PublicationStatus>, PublicationStatusCreator>();
        services.AddTransient<IEntityCreator<NewRepresentativeHouseBillAction>, RepresentativeHouseBillActionCreator>();
        services.AddTransient<IEntityCreator<NewReview>, ReviewCreator>();
        services.AddTransient<IEntityCreator<NewSecondLevelGlobalRegion>, SecondLevelGlobalRegionCreator>();
        services.AddTransient<IEntityCreator<NewSenateBill>, SenateBillCreator>();
        services.AddTransient<IEntityCreator<NewSenateTerm>, SenateTermCreator>();
        services.AddTransient<IEntityCreator<NewSenatorSenateBillAction>, SenatorSenateBillActionCreator>();
        services.AddTransient<IEntityCreator<NewSingleQuestionPoll>, SingleQuestionPollCreator>();
        services.AddTransient<IEntityCreator<NewSubdivisionType>, SubdivisionTypeCreator>();
        services.AddTransient<IEntityCreator<Subgroup>, SubgroupCreator>();
        services.AddTransient<IEntityCreator<SystemGroup>, SystemGroupCreator>();
        services.AddTransient<IEntityCreator<Tenant>, TenantCreator>();
        services.AddTransient<IEntityCreator<TenantNodeMenuItem>, TenantNodeMenuItemCreator>();
        services.AddTransient<IEntityCreator<TermHierarchy>, TermHierarchyCreator>();
        services.AddTransient<IEntityCreator<NewTypeOfAbuse>, TypeOfAbuseCreator>();
        services.AddTransient<IEntityCreator<NewTypeOfAbuser>, TypeOfAbuserCreator>();
        services.AddTransient<IEntityCreator<NewUnitedStatesCongressionalMeeting>, UnitedStatesCongressionalMeetingCreator>();
        services.AddTransient<IEntityCreator<NewUnitedStatesPoliticalPartyAffliation>, UnitedStatesPoliticalPartyAffliationCreator>();
        services.AddTransient<IEntityCreator<User>, UserCreator>();
        services.AddTransient<IEntityCreator<UserGroupUserRoleUser>, UserGroupUserRoleUserCreator>();
        services.AddTransient<IEntityCreator<NewVocabulary>, VocabularyCreator>();
        services.AddTransient<IEntityCreator<ViewNodeTypeListAction>, ViewNodeTypeListActionCreator>();
        services.AddTransient<IEntityCreator<NewWrongfulMedicationCase>, WrongfulMedicationCaseCreator>();
        services.AddTransient<IEntityCreator<NewWrongfulRemovalCase>, WrongfulRemovalCaseCreator>();


















    }
}
