﻿using Microsoft.Extensions.DependencyInjection;

namespace PoundPupLegacy.CreateModel.Creators;

internal static class DependencyInjection
{
    internal static void AddCreateModelCreators(this IServiceCollection services)
    {
        services.AddCreateModelInserters();
        services.AddTransient<NodeDetailsCreatorFactory>();
        services.AddTransient<TermCreatorFactory>();
        services.AddTransient<LocatableDetailsCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<EventuallyIdentifiableAbuseCase>, AbuseCaseCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<AbuseCaseTypeOfAbuse>, AbuseCaseTypeOfAbuseCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<AbuseCaseTypeOfAbuser>, AbuseCaseTypeOfAbuserCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<AccessRole>, AccessRoleCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<AccessRolePrivilege>, AccessRolePrivilegeCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<EventuallyIdentifiableAct>, ActCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<ActionMenuItem>, ActionMenuItemCreatorFactory>();
        services.AddTransient<IAnonimousUserCreator, AnonimousUserCreator>();
        services.AddTransient<IEntityCreatorFactory<AuthoringStatus>, AuthoringStatusCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<BasicAction>, BasicActionCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<EventuallyIdentifiableBasicCountry>, BasicCountryCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<EventuallyIdentifiableBasicNameable>, BasicNameableCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<BasicNameableType>, BasicNameableTypeCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<EventuallyIdentifiableBasicSecondLevelSubdivision>, BasicSecondLevelSubdivisionCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<EventuallyIdentifiableBillActionType>, BillActionTypeCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<EventuallyIdentifiableBindingCountry>, BindingCountryCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<EventuallyIdentifiableBlogPost>, BlogPostCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<EventuallyIdentifiableBoundCountry>, BoundCountryCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<ExistingCaseNewCaseParties>, CaseCasePartiesCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<EventuallyIdentifiableCasePartyType>, CasePartyTypeCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<CaseType>, CaseTypeCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<EventuallyIdentifiableCasePartyType>, CasePartyTypeCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<EventuallyIdentifiableChildPlacementType>, ChildPlacementTypeCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<EventuallyIdentifiableChildTraffickingCase>, ChildTraffickingCaseCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<EventuallyIdentifiableCoercedAdoptionCase>, CoercedAdoptionCaseCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<Collective>, CollectiveCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<CollectiveUser>, CollectiveUserCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<Comment>, CommentCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<EventuallyIdentifiableCongressionalTermPoliticalPartyAffiliation>, CongressionalTermPoliticalPartyAffiliationCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<ContentSharingGroup>, ContentSharingGroupCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<EventuallyIdentifiableCountryAndFirstAndBottomLevelSubdivision>, CountryAndFirstAndBottomLevelSubdivisionCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<EventuallyIdentifiableCountryAndFirstAndSecondLevelSubdivision>, CountryAndFirstAndSecondLevelSubdivisionCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<EventuallyIdentifiableCountryAndIntermediateLevelSubdivision>, CountryAndIntermediateLevelSubdivisionCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<CountrySubdivisionType>, CountrySubdivisionTypeCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<CreateNodeAction>, CreateNodeActionCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<DeleteNodeAction>, DeleteNodeActionCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<EventuallyIdentifiableDenomination>, DenominationCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<EventuallyIdentifiableDeportationCase>, DeportationCaseCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<EventuallyIdentifiableDiscussion>, DiscussionCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<EventuallyIdentifiableDisruptedPlacementCase>, DisruptedPlacementCaseCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<DocumentableDocument>, DocumentableDocumentCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<EventuallyIdentifiableDocument>, DocumentCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<EventuallyIdentifiableDocumentType>, DocumentTypeCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<EditNodeAction>, EditNodeActionCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<EditOwnNodeAction>, EditOwnNodeActionCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<EventuallyIdentifiableFamilySize>, FamilySizeCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<EventuallyIdentifiableFathersRightsViolationCase>, FathersRightsViolationCaseCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<File>, FileCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<EventuallyIdentifiableFirstAndBottomLevelSubdivision>, FirstAndBottomLevelSubdivisionCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<EventuallyIdentifiableFirstLevelGlobalRegion>, FirstLevelGlobalRegionCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<EventuallyIdentifiableFormalIntermediateLevelSubdivision>, FormalIntermediateLevelSubdivisionCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<EventuallyIdentifiableHagueStatus>, HagueStatusCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<EventuallyIdentifiableHouseBill>, HouseBillCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<EventuallyIdentifiableHouseTerm>, HouseTermCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<EventuallyIdentifiableInformalIntermediateLevelSubdivision>, InformalIntermediateLevelSubdivisionCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<EventuallyIdentifiableInterCountryRelation>, InterCountryRelationCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<EventuallyIdentifiableInterCountryRelationType>, InterCountryRelationTypeCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<EventuallyIdentifiableInterOrganizationalRelationForExistingParticipants>, InterOrganizationalRelationCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<EventuallyIdentifiableInterOrganizationalRelationType>, InterOrganizationalRelationTypeCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<EventuallyIdentifiableInterPersonalRelationForExistingParticipants>, InterPersonalRelationCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<EventuallyIdentifiableInterPersonalRelationType>, InterPersonalRelationTypeCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<EventuallyIdentifiableLocation>, LocationCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<LocationLocatable>, LocationLocatableCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<EventuallyIdentifiableMultiQuestionPoll>, MultiQuestionPollCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<NodeFile>, NodeFileCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<NodeTerm>, NodeTermCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<BasicNodeType>, NodeTypeCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<EventuallyIdentifiableOrganization>, OrganizationCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<EventuallyIdentifiableOrganizationType>, OrganizationTypeCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<EventuallyIdentifiablePage>, PageCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<EventuallyIdentifiablePartyPoliticalEntityRelationForExistingParty>, PartyPoliticalEntityRelationCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<EventuallyIdentifiablePartyPoliticalEntityRelationType>, PartyPoliticalEntityRelationTypeCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<EventuallyIdentifiablePerson>, PersonCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<EventuallyIdentifiablePersonOrganizationRelationForExistingParticipants>, PersonOrganizationRelationCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<EventuallyIdentifiablePersonOrganizationRelationType>, PersonOrganizationRelationTypeCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<PollStatus>, PollStatusCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<EventuallyIdentifiableProfessionalRoleForExistingPerson>, ProfessionalRoleCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<EventuallyIdentifiableProfession>, ProfessionCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<PublicationStatus>, PublicationStatusCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<EventuallyIdentifiablePollQuestion>, PollQuestionCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<EventuallyIdentifiableRepresentativeHouseBillAction>, RepresentativeHouseBillActionCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<EventuallyIdentifiableSecondLevelGlobalRegion>, SecondLevelGlobalRegionCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<EventuallyIdentifiableSenateBill>, SenateBillCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<EventuallyIdentifiableSenateTerm>, SenateTermCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<EventuallyIdentifiableSenatorSenateBillAction>, SenatorSenateBillActionCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<EventuallyIdentifiableSingleQuestionPoll>, SingleQuestionPollCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<EventuallyIdentifiableSubdivisionType>, SubdivisionTypeCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<Subgroup>, SubgroupCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<SystemGroup>, SystemGroupCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<Tenant>, TenantCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<TenantNodeMenuItem>, TenantNodeMenuItemCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<TermHierarchy>, TermHierarchyCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<EventuallyIdentifiableTypeOfAbuse>, TypeOfAbuseCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<EventuallyIdentifiableTypeOfAbuser>, TypeOfAbuserCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<EventuallyIdentifiableUnitedStatesCongressionalMeeting>, UnitedStatesCongressionalMeetingCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<EventuallyIdentifiableUnitedStatesPoliticalPartyAffliation>, UnitedStatesPoliticalPartyAffliationCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<User>, UserCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<UserGroupUserRoleUser>, UserGroupUserRoleUserCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<EventuallyIdentifiableVocabulary>, VocabularyCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<ViewNodeTypeListAction>, ViewNodeTypeListActionCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<EventuallyIdentifiableWrongfulMedicationCase>, WrongfulMedicationCaseCreatorFactory>();
        services.AddTransient<IEntityCreatorFactory<EventuallyIdentifiableWrongfulRemovalCase>, WrongfulRemovalCaseCreatorFactory>();
    }
}
