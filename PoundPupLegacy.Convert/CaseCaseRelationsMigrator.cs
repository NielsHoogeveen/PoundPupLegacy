﻿namespace PoundPupLegacy.Convert;

internal class CaseCaseRelationsMigrator(
        IDatabaseConnections databaseConnections,
        IMandatorySingleItemDatabaseReaderFactory<NodeIdReaderByUrlIdRequest, int> nodeIdReaderFactory,
        IEntityCreatorFactory<ExistingCaseNewCaseParties> caseCasePartiesCreatorFactory
    ) : MigratorPPL(databaseConnections)
{
    protected override string Name => "case case relation";

    protected override async Task MigrateImpl()
    {
        await using var nodeIdReader = await nodeIdReaderFactory.CreateAsync(_postgresConnection);
        await using var caseCasePartiesCreator = await caseCasePartiesCreatorFactory.CreateAsync(_postgresConnection);
        await caseCasePartiesCreator.CreateAsync(ReadAbuseCaseHomestudyParties(nodeIdReader));
        await caseCasePartiesCreator.CreateAsync(ReadAbuseCasePlacementParties(nodeIdReader));
        await caseCasePartiesCreator.CreateAsync(ReadAbuseCasePostPlacementParties(nodeIdReader));
        await caseCasePartiesCreator.CreateAsync(ReadAbuseCaseFacilitatorParties(nodeIdReader));
        await caseCasePartiesCreator.CreateAsync(ReadAbuseCaseInstitutionParties(nodeIdReader));
        await caseCasePartiesCreator.CreateAsync(ReadAbuseCaseTherapyParties(nodeIdReader));
        await caseCasePartiesCreator.CreateAsync(ReadChildTraffickingCasePlacementParties(nodeIdReader));
        await caseCasePartiesCreator.CreateAsync(ReadChildTraffickingCaseFacilitatorParties(nodeIdReader));
        await caseCasePartiesCreator.CreateAsync(ReadChildTraffickingCaseOrphanageParties(nodeIdReader));
        await caseCasePartiesCreator.CreateAsync(ReadDisruptedPlacementCasePlacementParties(nodeIdReader));
        await caseCasePartiesCreator.CreateAsync(ReadDisruptedPlacementCaseFacilitatorParties(nodeIdReader));
        await caseCasePartiesCreator.CreateAsync(ReadCoercedAdoptionCasePlacementParties(nodeIdReader));
        await caseCasePartiesCreator.CreateAsync(ReadFathersRightsViolationCasePlacementParties(nodeIdReader));
        await caseCasePartiesCreator.CreateAsync(ReadWrongfulRemovalCaseAuthorityParties(nodeIdReader));
        await caseCasePartiesCreator.CreateAsync(ReadWrongfulMedicationCaseAuthorityParties(nodeIdReader));
    }
    private async IAsyncEnumerable<ExistingCaseNewCaseParties> ReadCaseCaseRelations(
        string sql,
        int casePartyTypeId,
        IMandatorySingleItemDatabaseReader<NodeIdReaderByUrlIdRequest, int> nodeIdReader)
    {

        using var readCommand = _mySqlConnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;


        var reader = await readCommand.ExecuteReaderAsync();


        while (await reader.ReadAsync()) {
            var id = reader.GetInt32("id");
            var organizationIds = new List<int>();
            foreach (var entry in reader.IsDBNull("organization_ids") ? new List<int>() : reader.GetString("organization_ids").Split(",").Where(x => !string.IsNullOrEmpty(x)).Select(x => int.Parse(x))) {
                organizationIds.Add(await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                    UrlId = entry,
                    TenantId = Constants.PPL
                }));
            }
            var personIds = new List<int>();
            foreach (var entry in reader.IsDBNull("person_ids") ? new List<int>() : reader.GetString("person_ids").Split(",").Where(x => !string.IsNullOrEmpty(x)).Select(x => int.Parse(x))) {
                personIds.Add(await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                    UrlId = entry,
                    TenantId = Constants.PPL
                }));
            }

            yield return new ExistingCaseNewCaseParties {
                CaseId = await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                    UrlId = id,
                    TenantId = Constants.PPL
                }),
                CaseParties = new NewCaseParties {
                    Id = null,
                    Organizations = reader.IsDBNull("organizations_text") ? null : reader.GetString("organizations_text"),
                    Persons = reader.IsDBNull("persons_text") ? null : reader.GetString("persons_text"),
                    OrganizationIds = organizationIds,
                    PersonIds = personIds
                },
                CasePartyTypeId = casePartyTypeId,
            };

        }
        await reader.CloseAsync();

    }
    private async IAsyncEnumerable<ExistingCaseNewCaseParties> ReadAbuseCaseHomestudyParties(
        IMandatorySingleItemDatabaseReader<NodeIdReaderByUrlIdRequest, int> nodeIdReader)
    {
        var sql = $"""
                SELECT
                *
                FROM(
                SELECT
                nid id,
                field_homestudy_agency_value organizations_text,
                GROUP_CONCAT(distinct field_homestudy_agency_0_nid SEPARATOR ',') organization_ids,
                GROUP_CONCAT(distinct field_homestudy_person_value SEPARATOR ' ') persons_text,
                GROUP_CONCAT(distinct field_homestudy_person_0_nid SEPARATOR ',') person_ids
                FROM(
                SELECT
                n.nid, 
                case 
                	when c.field_homestudy_agency_value = '' then null
                	ELSE c.field_homestudy_agency_value
                END field_homestudy_agency_value,
                case 
                	when o.field_homestudy_agency_0_nid = 0 then null
                	ELSE o.field_homestudy_agency_0_nid
                END field_homestudy_agency_0_nid,	
                case
                	when p.field_homestudy_person_value = '' then null
                	ELSE p.field_homestudy_person_value
                END field_homestudy_person_value,
                case
                	when pp.field_homestudy_person_0_nid = 0 then null
                	ELSE pp.field_homestudy_person_0_nid
                END field_homestudy_person_0_nid	
                FROM content_type_case c
                JOIN node n ON n.nid = c.nid AND n.vid = c.vid
                LEFT JOIN content_field_homestudy_agency_0 o ON o.nid = n.nid AND o.vid = n.vid
                LEFT JOIN content_field_homestudy_person p ON p.nid = n.nid AND p.vid = n.vid
                LEFT JOIN content_field_homestudy_person_0 pp ON pp.nid = n.nid AND pp.vid = n.vid
                ) x
                group BY id, field_homestudy_agency_value
                ) x
                WHERE NOT (organizations_text IS NULL AND organization_ids IS NULL AND persons_text IS NULL AND person_ids IS NULL)
                """;
        await foreach (var elem in ReadCaseCaseRelations(
            sql,
            await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                UrlId = Constants.HOMESTUDY_CASE_TYPE,
                TenantId = Constants.PPL
            }),
            nodeIdReader
        )) {
            yield return elem;
        }
    }
    private async IAsyncEnumerable<ExistingCaseNewCaseParties> ReadAbuseCasePlacementParties(
        IMandatorySingleItemDatabaseReader<NodeIdReaderByUrlIdRequest, int> nodeIdReader)
    {
        var sql = $"""
                SELECT
                *
                FROM(
                SELECT
                id,
                field_placement_agency_value organizations_text,
                GROUP_CONCAT(distinct field_placement_agency_0_nid SEPARATOR ',') organization_ids,
                GROUP_CONCAT(distinct field_placement_person_value SEPARATOR ' ') persons_text,
                GROUP_CONCAT(distinct field_placement_person_0_nid SEPARATOR ',') person_ids
                FROM(
                SELECT
                n.nid id, 
                case 
                	when c.field_placement_agency_value = '' then null
                	ELSE c.field_placement_agency_value
                END field_placement_agency_value,
                case 
                	when o.field_placement_agency_0_nid = 0 then null
                	ELSE o.field_placement_agency_0_nid
                END field_placement_agency_0_nid,	
                case
                	when p.field_placement_person_value = '' then null
                	ELSE p.field_placement_person_value
                END field_placement_person_value,
                case
                	when pp.field_placement_person_0_nid = 0 then null
                	ELSE pp.field_placement_person_0_nid
                END field_placement_person_0_nid	
                FROM content_type_case c
                JOIN node n ON n.nid = c.nid AND n.vid = c.vid
                LEFT JOIN content_field_placement_agency_0 o ON o.nid = n.nid AND o.vid = n.vid
                LEFT JOIN content_field_placement_person p ON p.nid = n.nid AND p.vid = n.vid
                LEFT JOIN content_field_placement_person_0 pp ON pp.nid = n.nid AND pp.vid = n.vid
                ) x
                group BY id, field_placement_agency_value
                ) x
                WHERE NOT (organizations_text IS NULL AND organization_ids IS NULL AND persons_text IS NULL AND person_ids IS NULL)
                """;
        await foreach (var elem in ReadCaseCaseRelations(
            sql,
            await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                UrlId = Constants.PLACEMENT_CASE_TYPE,
                TenantId = Constants.PPL
            }),
            nodeIdReader
        )) {
            yield return elem;
        }
    }

    private async IAsyncEnumerable<ExistingCaseNewCaseParties> ReadAbuseCasePostPlacementParties(
        IMandatorySingleItemDatabaseReader<NodeIdReaderByUrlIdRequest, int> nodeIdReader)
    {
        var sql = $"""
                SELECT
                *
                FROM(
                SELECT
                nid id,
                field_post_placement_agency_value organizations_text,
                GROUP_CONCAT(distinct field_post_placement_agency_0_nid SEPARATOR ',') organization_ids,
                GROUP_CONCAT(distinct field_post_placement_person_value SEPARATOR ' ') persons_text,
                GROUP_CONCAT(distinct field_post_placement_person_0_nid SEPARATOR ',') person_ids
                FROM(
                SELECT
                n.nid, 
                case 
                	when c.field_post_placement_agency_value = '' then null
                	ELSE c.field_post_placement_agency_value
                END field_post_placement_agency_value,
                case 
                	when o.field_post_placement_agency_0_nid = 0 then null
                	ELSE o.field_post_placement_agency_0_nid
                END field_post_placement_agency_0_nid,	
                case
                	when p.field_post_placement_person_value = '' then null
                	ELSE p.field_post_placement_person_value
                END field_post_placement_person_value,
                case
                	when pp.field_post_placement_person_0_nid = 0 then null
                	ELSE pp.field_post_placement_person_0_nid
                END field_post_placement_person_0_nid	
                FROM content_type_case c
                JOIN node n ON n.nid = c.nid AND n.vid = c.vid
                LEFT JOIN content_field_post_placement_agency_0 o ON o.nid = n.nid AND o.vid = n.vid
                LEFT JOIN content_field_post_placement_person p ON p.nid = n.nid AND p.vid = n.vid
                LEFT JOIN content_field_post_placement_person_0 pp ON pp.nid = n.nid AND pp.vid = n.vid
                ) x
                group BY id, field_post_placement_agency_value
                ) x
                WHERE NOT (organizations_text IS NULL AND organization_ids IS NULL AND persons_text IS NULL AND person_ids IS NULL)
                """;
        await foreach (var elem in ReadCaseCaseRelations(
            sql,
            await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                UrlId = Constants.POSTPLACEMENT_CASE_TYPE,
                TenantId = Constants.PPL
            }),
            nodeIdReader
        )) {
            yield return elem;
        }
    }
    private async IAsyncEnumerable<ExistingCaseNewCaseParties> ReadAbuseCaseFacilitatorParties(
        IMandatorySingleItemDatabaseReader<NodeIdReaderByUrlIdRequest, int> nodeIdReader)
    {
        var sql = $"""
                SELECT
                *
                FROM(
                SELECT
                nid id,
                GROUP_CONCAT(distinct field_facilitator_organization_value SEPARATOR ' ') organizations_text,
                GROUP_CONCAT(distinct field_facilitator_organizatio_0_nid SEPARATOR ',') organization_ids,
                GROUP_CONCAT(distinct field_facilitator_person_value SEPARATOR ' ') persons_text,
                GROUP_CONCAT(distinct field_facilitator_person_0_nid SEPARATOR ',') person_ids
                FROM(
                SELECT
                n.nid, 
                case 
                	when ot.field_facilitator_organization_value = '' then null
                	ELSE ot.field_facilitator_organization_value
                END field_facilitator_organization_value,
                case 
                	when o.field_facilitator_organizatio_0_nid = 0 then null
                	ELSE o.field_facilitator_organizatio_0_nid
                END field_facilitator_organizatio_0_nid,	
                case
                	when p.field_facilitator_person_value = '' then null
                	ELSE p.field_facilitator_person_value
                END field_facilitator_person_value,
                case
                	when pp.field_facilitator_person_0_nid = 0 then null
                	ELSE pp.field_facilitator_person_0_nid
                END field_facilitator_person_0_nid	
                FROM content_type_case c
                JOIN node n ON n.nid = c.nid AND n.vid = c.vid
                LEFT JOIN content_field_facilitator_organization ot ON ot.nid = n.nid AND ot.vid = n.vid
                LEFT JOIN content_field_facilitator_organizatio_0 o ON o.nid = n.nid AND o.vid = n.vid
                LEFT JOIN content_field_facilitator_person p ON p.nid = n.nid AND p.vid = n.vid
                LEFT JOIN content_field_facilitator_person_0 pp ON pp.nid = n.nid AND pp.vid = n.vid
                ) x
                group BY id
                ) x
                WHERE NOT (organizations_text IS NULL AND organization_ids IS NULL AND persons_text IS NULL AND person_ids IS NULL)
                """;
        await foreach (var elem in ReadCaseCaseRelations(
            sql,
            await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                UrlId = Constants.FACILITATION_CASE_TYPE,
                TenantId = Constants.PPL
            }),
            nodeIdReader
        )) {
            yield return elem;
        }
    }

    private async IAsyncEnumerable<ExistingCaseNewCaseParties> ReadAbuseCaseInstitutionParties(
        IMandatorySingleItemDatabaseReader<NodeIdReaderByUrlIdRequest, int> nodeIdReader)
    {
        var sql = $"""
                SELECT
                *
                FROM(
                SELECT
                nid id,
                field_institution_value organizations_text,
                GROUP_CONCAT(distinct field_institution_0_nid SEPARATOR ',') organization_ids,
                GROUP_CONCAT(distinct field_institution_person_value SEPARATOR ' ') persons_text,
                GROUP_CONCAT(distinct field_institution_person_0_nid SEPARATOR ',') person_ids
                FROM(
                SELECT
                n.nid, 
                case 
                	when c.field_institution_value = '' then null
                	ELSE c.field_institution_value
                END field_institution_value,
                case 
                	when o.field_institution_0_nid = 0 then null
                	ELSE o.field_institution_0_nid
                END field_institution_0_nid,	
                case
                	when p.field_institution_person_value = '' then null
                	ELSE p.field_institution_person_value
                END field_institution_person_value,
                case
                	when pp.field_institution_person_0_nid = 0 then null
                	ELSE pp.field_institution_person_0_nid
                END field_institution_person_0_nid	
                FROM content_type_case c
                JOIN node n ON n.nid = c.nid AND n.vid = c.vid
                LEFT JOIN content_field_institution_0 o ON o.nid = n.nid AND o.vid = n.vid
                LEFT JOIN content_field_institution_person p ON p.nid = n.nid AND p.vid = n.vid
                LEFT JOIN content_field_institution_person_0 pp ON pp.nid = n.nid AND pp.vid = n.vid
                ) x
                group BY id, field_institution_value
                ) x
                WHERE NOT (organizations_text IS NULL AND organization_ids IS NULL AND persons_text IS NULL AND person_ids IS NULL)
                """;
        await foreach (var elem in ReadCaseCaseRelations(
            sql,
            await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                UrlId = Constants.INSTITUTION_CASE_TYPE,
                TenantId = Constants.PPL
            }),
            nodeIdReader
        )) {
            yield return elem;
        }
    }
    private async IAsyncEnumerable<ExistingCaseNewCaseParties> ReadAbuseCaseTherapyParties(
        IMandatorySingleItemDatabaseReader<NodeIdReaderByUrlIdRequest, int> nodeIdReader)
    {
        var sql = $"""
                SELECT
                *
                FROM(
                SELECT
                nid id,
                null organizations_text,
                null organization_ids,
                NULL persons_text,
                GROUP_CONCAT(distinct field_field_attachment_therapist_nid SEPARATOR ',') person_ids
                FROM(
                SELECT
                n.nid, 
                case 
                	when o.field_attachment_therapist_nid = 0 then null
                	ELSE o.field_attachment_therapist_nid
                END field_field_attachment_therapist_nid
                FROM content_type_case c
                JOIN node n ON n.nid = c.nid AND n.vid = c.vid
                LEFT JOIN content_field_attachment_therapist o ON o.nid = n.nid AND o.vid = n.vid
                ) x
                group BY id
                ) x
                WHERE NOT (organizations_text IS NULL AND organization_ids IS NULL AND persons_text IS NULL AND person_ids IS NULL)
                """;
        await foreach (var elem in ReadCaseCaseRelations(
            sql,
            await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                UrlId = Constants.THERAPY_CASE_TYPE,
                TenantId = Constants.PPL
            }),
            nodeIdReader
        )) {
            yield return elem;
        }
    }
    private async IAsyncEnumerable<ExistingCaseNewCaseParties> ReadChildTraffickingCasePlacementParties(
        IMandatorySingleItemDatabaseReader<NodeIdReaderByUrlIdRequest, int> nodeIdReader)
    {
        var sql = $"""
                SELECT
                *
                FROM(
                SELECT
                nid id,
                GROUP_CONCAT(distinct field_agencies_value SEPARATOR ' ') organizations_text,
                GROUP_CONCAT(distinct field_agencies_0_nid SEPARATOR ',') organization_ids,
                GROUP_CONCAT(distinct field_agency_person_value SEPARATOR ' ') persons_text,
                GROUP_CONCAT(distinct field_agency_person_0_nid SEPARATOR ',') person_ids
                FROM(
                SELECT
                n.nid, 
                case 
                	when ot.field_agencies_value = '' then null
                	ELSE ot.field_agencies_value
                END field_agencies_value,
                case 
                	when o.field_agencies_0_nid = 0 then null
                	ELSE o.field_agencies_0_nid
                END field_agencies_0_nid,	
                case
                	when p.field_agency_person_value = '' then null
                	ELSE p.field_agency_person_value
                END field_agency_person_value,
                case
                	when pp.field_agency_person_0_nid = 0 then null
                	ELSE pp.field_agency_person_0_nid
                END field_agency_person_0_nid	
                FROM content_type_child_trafficking_case c
                JOIN node n ON n.nid = c.nid AND n.vid = c.vid
                LEFT JOIN content_field_agencies ot ON ot.nid = n.nid AND ot.vid = n.vid
                LEFT JOIN content_field_agencies_0 o ON o.nid = n.nid AND o.vid = n.vid
                LEFT JOIN content_field_agency_person p ON p.nid = n.nid AND p.vid = n.vid
                LEFT JOIN content_field_agency_person_0 pp ON pp.nid = n.nid AND pp.vid = n.vid
                ) x
                group BY id
                ) x
                WHERE NOT (organizations_text IS NULL AND organization_ids IS NULL AND persons_text IS NULL AND person_ids IS NULL)
                """;
        await foreach (var elem in ReadCaseCaseRelations(
            sql,
            await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                UrlId = Constants.PLACEMENT_CASE_TYPE,
                TenantId = Constants.PPL
            }),
            nodeIdReader
        )) {
            yield return elem;
        }
    }

    private async IAsyncEnumerable<ExistingCaseNewCaseParties> ReadChildTraffickingCaseFacilitatorParties(
        IMandatorySingleItemDatabaseReader<NodeIdReaderByUrlIdRequest, int> nodeIdReader)
    {

        var sql = $"""
                SELECT
                *
                FROM(
                SELECT
                nid id,
                GROUP_CONCAT(distinct field_facilitator_organization_value SEPARATOR ' ') organizations_text,
                GROUP_CONCAT(distinct field_facilitator_organizatio_0_nid SEPARATOR ',') organization_ids,
                GROUP_CONCAT(distinct field_facilitator_person_value SEPARATOR ' ') persons_text,
                GROUP_CONCAT(distinct field_facilitator_person_0_nid SEPARATOR ',') person_ids
                FROM(
                SELECT
                n.nid, 
                case 
                	when ot.field_facilitator_organization_value = '' then null
                	ELSE ot.field_facilitator_organization_value
                END field_facilitator_organization_value,
                case 
                	when o.field_facilitator_organizatio_0_nid = 0 then null
                	ELSE o.field_facilitator_organizatio_0_nid
                END field_facilitator_organizatio_0_nid,	
                case
                	when p.field_facilitator_person_value = '' then null
                	ELSE p.field_facilitator_person_value
                END field_facilitator_person_value,
                case
                	when pp.field_facilitator_person_0_nid = 0 then null
                	ELSE pp.field_facilitator_person_0_nid
                END field_facilitator_person_0_nid	
                FROM content_type_child_trafficking_case c
                JOIN node n ON n.nid = c.nid AND n.vid = c.vid
                LEFT JOIN content_field_facilitator_organization ot ON ot.nid = n.nid AND ot.vid = n.vid
                LEFT JOIN content_field_facilitator_organizatio_0 o ON o.nid = n.nid AND o.vid = n.vid
                LEFT JOIN content_field_facilitator_person p ON p.nid = n.nid AND p.vid = n.vid
                LEFT JOIN content_field_facilitator_person_0 pp ON pp.nid = n.nid AND pp.vid = n.vid
                ) x
                group BY id
                ) x
                WHERE NOT (organizations_text IS NULL AND organization_ids IS NULL AND persons_text IS NULL AND person_ids IS NULL)
                """;
        await foreach (var elem in ReadCaseCaseRelations(
            sql,
            await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                UrlId = Constants.FACILITATION_CASE_TYPE,
                TenantId = Constants.PPL
            }),
            nodeIdReader
        )) {
            yield return elem;
        }
    }

    private async IAsyncEnumerable<ExistingCaseNewCaseParties> ReadChildTraffickingCaseOrphanageParties(
        IMandatorySingleItemDatabaseReader<NodeIdReaderByUrlIdRequest, int> nodeIdReader)
    {
        var sql = $"""
                SELECT
                *
                FROM(
                SELECT
                nid id,
                GROUP_CONCAT(distinct field_orphanages_value SEPARATOR ' ') organizations_text,
                GROUP_CONCAT(distinct field_orphanages_0_nid SEPARATOR ',') organization_ids,
                GROUP_CONCAT(distinct field_orphanage_persons_value SEPARATOR ' ') persons_text,
                GROUP_CONCAT(distinct field_orphanage_persons_0_nid SEPARATOR ',') person_ids
                FROM(
                SELECT
                n.nid, 
                case 
                	when ot.field_orphanages_value = '' then null
                	ELSE ot.field_orphanages_value
                END field_orphanages_value,
                case 
                	when o.field_orphanages_0_nid = 0 then null
                	ELSE o.field_orphanages_0_nid
                END field_orphanages_0_nid,	
                case
                	when p.field_orphanage_persons_value = '' then null
                	ELSE p.field_orphanage_persons_value
                END field_orphanage_persons_value,
                case
                	when pp.field_orphanage_persons_0_nid = 0 then null
                	ELSE pp.field_orphanage_persons_0_nid
                END field_orphanage_persons_0_nid	
                FROM content_type_child_trafficking_case c
                JOIN node n ON n.nid = c.nid AND n.vid = c.vid
                LEFT JOIN content_field_orphanages ot ON ot.nid = n.nid AND ot.vid = n.vid
                LEFT JOIN content_field_orphanages_0 o ON o.nid = n.nid AND o.vid = n.vid
                LEFT JOIN content_field_orphanage_persons p ON p.nid = n.nid AND p.vid = n.vid
                LEFT JOIN content_field_orphanage_persons_0 pp ON pp.nid = n.nid AND pp.vid = n.vid
                ) x
                group BY id
                ) x
                WHERE NOT (organizations_text IS NULL AND organization_ids IS NULL AND persons_text IS NULL AND person_ids IS NULL)
                """;
        await foreach (var elem in ReadCaseCaseRelations(
            sql,
            await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                UrlId = Constants.INSTITUTION_CASE_TYPE,
                TenantId = Constants.PPL
            }),
            nodeIdReader
        )) {
            yield return elem;
        }
    }

    private async IAsyncEnumerable<ExistingCaseNewCaseParties> ReadDisruptedPlacementCasePlacementParties(
        IMandatorySingleItemDatabaseReader<NodeIdReaderByUrlIdRequest, int> nodeIdReader)
    {
        var sql = $"""
                SELECT
                *
                FROM(
                SELECT
                nid id,
                GROUP_CONCAT(distinct field_agency_involved_value SEPARATOR ' ') organizations_text,
                GROUP_CONCAT(distinct field_agency_involved_0_nid SEPARATOR ',') organization_ids,
                GROUP_CONCAT(distinct field_agency_person_1_value SEPARATOR ' ') persons_text,
                GROUP_CONCAT(distinct field_agency_person_2_nid SEPARATOR ',') person_ids
                FROM(
                SELECT
                n.nid, 
                case 
                	when c.field_agency_involved_value = '' then null
                	ELSE c.field_agency_involved_value
                END field_agency_involved_value,
                case 
                	when o.field_agency_involved_0_nid = 0 then null
                	ELSE o.field_agency_involved_0_nid
                END field_agency_involved_0_nid,	
                case
                	when c.field_agency_person_1_value = '' then null
                	ELSE c.field_agency_person_1_value
                END field_agency_person_1_value,
                case
                	when pp.field_agency_person_2_nid = 0 then null
                	ELSE pp.field_agency_person_2_nid
                END field_agency_person_2_nid	
                FROM content_type_disrupted_placement_case c
                JOIN node n ON n.nid = c.nid AND n.vid = c.vid
                LEFT JOIN content_field_agency_involved_0 o ON o.nid = n.nid AND o.vid = n.vid
                LEFT JOIN content_field_agency_person_2 pp ON pp.nid = n.nid AND pp.vid = n.vid
                ) x
                group BY id
                ) x
                WHERE NOT (organizations_text IS NULL AND organization_ids IS NULL AND persons_text IS NULL AND person_ids IS NULL)
                """;
        await foreach (var elem in ReadCaseCaseRelations(
            sql,
            await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                UrlId = Constants.PLACEMENT_CASE_TYPE,
                TenantId = Constants.PPL
            }),
            nodeIdReader
        )) {
            yield return elem;
        }
    }
    private async IAsyncEnumerable<ExistingCaseNewCaseParties> ReadDisruptedPlacementCaseFacilitatorParties(
        IMandatorySingleItemDatabaseReader<NodeIdReaderByUrlIdRequest, int> nodeIdReader)
    {
        var sql = $"""
                SELECT
                *
                FROM(
                SELECT
                nid id,
                GROUP_CONCAT(distinct field_facilitator_organization_value SEPARATOR ' ') organizations_text,
                GROUP_CONCAT(distinct field_facilitator_organizatio_0_nid SEPARATOR ',') organization_ids,
                GROUP_CONCAT(distinct field_facilitator_person_value SEPARATOR ' ') persons_text,
                GROUP_CONCAT(distinct field_facilitator_person_0_nid SEPARATOR ',') person_ids
                FROM(
                SELECT
                n.nid, 
                case 
                	when ot.field_facilitator_organization_value = '' then null
                	ELSE ot.field_facilitator_organization_value
                END field_facilitator_organization_value,
                case 
                	when o.field_facilitator_organizatio_0_nid = 0 then null
                	ELSE o.field_facilitator_organizatio_0_nid
                END field_facilitator_organizatio_0_nid,	
                case
                	when p.field_facilitator_person_value = '' then null
                	ELSE p.field_facilitator_person_value
                END field_facilitator_person_value,
                case
                	when pp.field_facilitator_person_0_nid = 0 then null
                	ELSE pp.field_facilitator_person_0_nid
                END field_facilitator_person_0_nid
                FROM content_type_disrupted_placement_case c
                JOIN node n ON n.nid = c.nid AND n.vid = c.vid
                LEFT JOIN content_field_facilitator_organization ot ON ot.nid = n.nid AND ot.vid = n.vid
                LEFT JOIN content_field_facilitator_organizatio_0 o ON o.nid = n.nid AND o.vid = n.vid
                LEFT JOIN content_field_facilitator_person p ON p.nid = n.nid AND p.vid = n.vid
                LEFT JOIN content_field_facilitator_person_0 pp ON pp.nid = n.nid AND pp.vid = n.vid
                ) x
                group BY id
                ) x
                WHERE NOT (organizations_text IS NULL AND organization_ids IS NULL AND persons_text IS NULL AND person_ids IS NULL)
                """;
        await foreach (var elem in ReadCaseCaseRelations(
            sql,
            await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                UrlId = Constants.FACILITATION_CASE_TYPE,
                TenantId = Constants.PPL
            }),
            nodeIdReader
        )) {
            yield return elem;
        }
    }
    private async IAsyncEnumerable<ExistingCaseNewCaseParties> ReadCoercedAdoptionCasePlacementParties(
        IMandatorySingleItemDatabaseReader<NodeIdReaderByUrlIdRequest, int> nodeIdReader)
    {
        var sql = $"""
                SELECT
                *
                FROM(
                SELECT
                nid id,
                GROUP_CONCAT(distinct field_agencies_involved_value SEPARATOR ' ') organizations_text,
                GROUP_CONCAT(distinct field_agencies_involved_0_nid SEPARATOR ',') organization_ids,
                GROUP_CONCAT(distinct field_persons_involved_value SEPARATOR ' ') persons_text,
                GROUP_CONCAT(distinct field_persons_involved_0_nid SEPARATOR ',') person_ids
                FROM(
                SELECT
                n.nid, 
                case 
                	when ot.field_agencies_involved_value = '' then null
                	ELSE ot.field_agencies_involved_value
                END field_agencies_involved_value,
                case 
                	when o.field_agencies_involved_0_nid = 0 then null
                	ELSE o.field_agencies_involved_0_nid
                END field_agencies_involved_0_nid,	
                case
                	when p.field_persons_involved_value = '' then null
                	ELSE p.field_persons_involved_value
                END field_persons_involved_value,
                case
                	when pp.field_persons_involved_0_nid = 0 then null
                	ELSE pp.field_persons_involved_0_nid
                END field_persons_involved_0_nid	
                FROM content_type_coerced_adoption_cases c
                JOIN node n ON n.nid = c.nid AND n.vid = c.vid
                LEFT JOIN content_field_agencies_involved ot ON ot.nid = n.nid AND ot.vid = n.vid
                LEFT JOIN content_field_agencies_involved_0 o ON o.nid = n.nid AND o.vid = n.vid
                LEFT JOIN content_field_persons_involved p ON p.nid = n.nid AND p.vid = n.vid
                LEFT JOIN content_field_persons_involved_0 pp ON pp.nid = n.nid AND pp.vid = n.vid
                ) x
                group BY id
                ) x
                WHERE NOT (organizations_text IS NULL AND organization_ids IS NULL AND persons_text IS NULL AND person_ids IS NULL)
                """;
        await foreach (var elem in ReadCaseCaseRelations(
            sql,
            await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                UrlId = Constants.PLACEMENT_CASE_TYPE,
                TenantId = Constants.PPL
            }),
            nodeIdReader
        )) {
            yield return elem;
        }
    }
    private async IAsyncEnumerable<ExistingCaseNewCaseParties> ReadFathersRightsViolationCasePlacementParties(
        IMandatorySingleItemDatabaseReader<NodeIdReaderByUrlIdRequest, int> nodeIdReader)
    {
        var sql = $"""
                SELECT
                *
                FROM(
                SELECT
                nid id,
                GROUP_CONCAT(distinct field_organizations_involved_0_value SEPARATOR ' ') organizations_text,
                GROUP_CONCAT(distinct field_organizations_involved_1_nid SEPARATOR ',') organization_ids,
                GROUP_CONCAT(distinct field_persons_involved_1_value SEPARATOR ' ') persons_text,
                GROUP_CONCAT(distinct field_persons_involved_2_nid SEPARATOR ',') person_ids
                FROM(
                SELECT
                n.nid, 
                case 
                	when c.field_organizations_involved_0_value = '' then null
                	ELSE c.field_organizations_involved_0_value
                END field_organizations_involved_0_value,
                case 
                	when o.field_organizations_involved_1_nid = 0 then null
                	ELSE o.field_organizations_involved_1_nid
                END field_organizations_involved_1_nid,	
                case
                	when c.field_persons_involved_1_value = '' then null
                	ELSE c.field_persons_involved_1_value
                END field_persons_involved_1_value,
                case
                	when pp.field_persons_involved_2_nid = 0 then null
                	ELSE pp.field_persons_involved_2_nid
                END field_persons_involved_2_nid	
                FROM content_type_fathers_rights_violations c
                JOIN node n ON n.nid = c.nid AND n.vid = c.vid
                LEFT JOIN content_field_organizations_involved_1 o ON o.nid = n.nid AND o.vid = n.vid
                LEFT JOIN content_field_persons_involved_2 pp ON pp.nid = n.nid AND pp.vid = n.vid
                ) x
                group BY id
                ) x
                WHERE NOT (organizations_text IS NULL AND organization_ids IS NULL AND persons_text IS NULL AND person_ids IS NULL)
                """;
        await foreach (var elem in ReadCaseCaseRelations(
            sql,
            await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                UrlId = Constants.PLACEMENT_CASE_TYPE,
                TenantId = Constants.PPL
            }),
            nodeIdReader
        )) {
            yield return elem;
        }
    }
    private async IAsyncEnumerable<ExistingCaseNewCaseParties> ReadWrongfulRemovalCaseAuthorityParties(
        IMandatorySingleItemDatabaseReader<NodeIdReaderByUrlIdRequest, int> nodeIdReader)
    {
        var sql = $"""
                SELECT
                *
                FROM(
                SELECT
                nid id,
                GROUP_CONCAT(distinct field_authorities_involved_value SEPARATOR ',') organizations_text,
                null organization_ids,
                NULL persons_text,
                NULL person_ids
                FROM(
                SELECT
                n.nid, 
                case
                	when o.field_authorities_involved_value = '' then null
                	ELSE o.field_authorities_involved_value
                END field_authorities_involved_value
                FROM content_type_wrongful_removal_case c
                JOIN node n ON n.nid = c.nid AND n.vid = c.vid
                LEFT JOIN content_field_authorities_involved o ON o.nid = n.nid AND o.vid = n.vid
                ) x
                group BY id
                ) x
                WHERE NOT (organizations_text IS NULL AND organization_ids IS NULL AND persons_text IS NULL AND person_ids IS NULL)
                """;
        await foreach (var elem in ReadCaseCaseRelations(
            sql,
            await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                UrlId = Constants.AUTHORITIES_CASE_TYPE,
                TenantId = Constants.PPL
            }),
            nodeIdReader
        )) {
            yield return elem;
        }
    }
    private async IAsyncEnumerable<ExistingCaseNewCaseParties> ReadWrongfulMedicationCaseAuthorityParties(
        IMandatorySingleItemDatabaseReader<NodeIdReaderByUrlIdRequest, int> nodeIdReader)
    {
        var sql = $"""
                SELECT
                *
                FROM(
                SELECT
                nid id,
                GROUP_CONCAT(distinct field_authorities_involved_0_value SEPARATOR ',') organizations_text,
                null organization_ids,
                NULL persons_text,
                NULL person_ids
                FROM(
                SELECT
                n.nid, 
                case
                	when o.field_authorities_involved_0_value = '' then null
                	ELSE o.field_authorities_involved_0_value
                END field_authorities_involved_0_value
                FROM content_type_wrongful_medication_case c
                JOIN node n ON n.nid = c.nid AND n.vid = c.vid
                LEFT JOIN content_field_authorities_involved_0 o ON o.nid = n.nid AND o.vid = n.vid
                ) x
                group BY id
                ) x
                WHERE NOT (organizations_text IS NULL AND organization_ids IS NULL AND persons_text IS NULL AND person_ids IS NULL)
                """;
        await foreach (var elem in ReadCaseCaseRelations(
            sql,
            await nodeIdReader.ReadAsync(new NodeIdReaderByUrlIdRequest {
                UrlId = Constants.AUTHORITIES_CASE_TYPE,
                TenantId = Constants.PPL
            }),
            nodeIdReader
        )) {
            yield return elem;
        }
    }

}
