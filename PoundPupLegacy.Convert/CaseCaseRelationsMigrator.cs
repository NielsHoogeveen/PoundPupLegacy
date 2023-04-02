namespace PoundPupLegacy.Convert;

internal class CaseCaseRelationsMigrator : PPLMigrator
{
    public CaseCaseRelationsMigrator(MySqlToPostgresConverter mySqlToPostgresConverter) : base(mySqlToPostgresConverter)
    {
    }

    protected override string Name => "case case relation";

    protected override async Task MigrateImpl()
    {
        await new CaseCaseRelationsCreator().CreateAsync(ReadAbuseCaseHomestudyParties(), _postgresConnection);
        await new CaseCaseRelationsCreator().CreateAsync(ReadAbuseCasePlacementParties(), _postgresConnection);
        await new CaseCaseRelationsCreator().CreateAsync(ReadAbuseCasePostPlacementParties(), _postgresConnection);
        await new CaseCaseRelationsCreator().CreateAsync(ReadAbuseCaseFacilitatorParties(), _postgresConnection);
        await new CaseCaseRelationsCreator().CreateAsync(ReadAbuseCaseInstitutionParties(), _postgresConnection);
        await new CaseCaseRelationsCreator().CreateAsync(ReadAbuseCaseTherapyParties(), _postgresConnection);
        await new CaseCaseRelationsCreator().CreateAsync(ReadChildTraffickingCasePlacementParties(), _postgresConnection);
        await new CaseCaseRelationsCreator().CreateAsync(ReadChildTraffickingCaseFacilitatorParties(), _postgresConnection);
        await new CaseCaseRelationsCreator().CreateAsync(ReadChildTraffickingCaseOrphanageParties(), _postgresConnection);
        await new CaseCaseRelationsCreator().CreateAsync(ReadDisruptedPlacementCasePlacementParties(), _postgresConnection);
        await new CaseCaseRelationsCreator().CreateAsync(ReadDisruptedPlacementCaseFacilitatorParties(), _postgresConnection);
        await new CaseCaseRelationsCreator().CreateAsync(ReadCoercedAdoptionCasePlacementParties(), _postgresConnection);
        await new CaseCaseRelationsCreator().CreateAsync(ReadFathersRightsViolationCasePlacementParties(), _postgresConnection);
        await new CaseCaseRelationsCreator().CreateAsync(ReadWrongfulRemovalCaseAuthorityParties(), _postgresConnection);
        await new CaseCaseRelationsCreator().CreateAsync(ReadWrongfulMedicationCaseAuthorityParties(), _postgresConnection);
    }
    private async IAsyncEnumerable<CaseCaseParties> ReadCaseCaseRelations(string sql, int casePartyTypeId)
    {
        using var readCommand = MysqlConnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;


        var reader = await readCommand.ExecuteReaderAsync();


        while (await reader.ReadAsync()) {
            var id = reader.GetInt32("id");
            var organizationIds = new List<int>();
            foreach (var entry in reader.IsDBNull("organization_ids") ? new List<int>() : reader.GetString("organization_ids").Split(",").Where(x => !string.IsNullOrEmpty(x)).Select(x => int.Parse(x))) {
                organizationIds.Add(await _nodeIdReader.ReadAsync(new NodeIdReaderByUrlId.Request {
                    UrlId = entry,
                    TenantId = Constants.PPL
                }));
            }
            var personIds = new List<int>();
            foreach (var entry in reader.IsDBNull("person_ids") ? new List<int>() : reader.GetString("person_ids").Split(",").Where(x => !string.IsNullOrEmpty(x)).Select(x => int.Parse(x))) {
                personIds.Add(await _nodeIdReader.ReadAsync(new NodeIdReaderByUrlId.Request {
                    UrlId = entry,
                    TenantId = Constants.PPL
                }));
            }

            yield return new CaseCaseParties {
                CaseId = await _nodeIdReader.ReadAsync(new NodeIdReaderByUrlId.Request {
                    UrlId = id,
                    TenantId = Constants.PPL
                }),
                CaseParties = new CaseParties {
                    Id = null,
                    Organizations = reader.IsDBNull("organizations_text") ? null : reader.GetString("organizations_text"),
                    Persons = reader.IsDBNull("persons_text") ? null : reader.GetString("persons_text"),
                    OrganizationIds = organizationIds,
                    PersonsIds = personIds
                },
                CasePartyTypeId = casePartyTypeId,
            };

        }
        await reader.CloseAsync();

    }
    private async IAsyncEnumerable<CaseCaseParties> ReadAbuseCaseHomestudyParties()
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
        await foreach (var elem in ReadCaseCaseRelations(sql, await _nodeIdReader.ReadAsync(new NodeIdReaderByUrlId.Request {
            UrlId = Constants.HOMESTUDY_CASE_TYPE,
            TenantId = Constants.PPL
        }))) {
            yield return elem;
        }
    }
    private async IAsyncEnumerable<CaseCaseParties> ReadAbuseCasePlacementParties()
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
        await foreach (var elem in ReadCaseCaseRelations(sql, await _nodeIdReader.ReadAsync(new NodeIdReaderByUrlId.Request {
            UrlId = Constants.PLACEMENT_CASE_TYPE,
            TenantId = Constants.PPL
        }))) {
            yield return elem;
        }
    }

    private async IAsyncEnumerable<CaseCaseParties> ReadAbuseCasePostPlacementParties()
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
        await foreach (var elem in ReadCaseCaseRelations(sql, await _nodeIdReader.ReadAsync(new NodeIdReaderByUrlId.Request {
            UrlId = Constants.POSTPLACEMENT_CASE_TYPE,
            TenantId = Constants.PPL
        }))) {
            yield return elem;
        }
    }
    private async IAsyncEnumerable<CaseCaseParties> ReadAbuseCaseFacilitatorParties()
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
        await foreach (var elem in ReadCaseCaseRelations(sql, await _nodeIdReader.ReadAsync(new NodeIdReaderByUrlId.Request {
            UrlId = Constants.FACILITATION_CASE_TYPE,
            TenantId = Constants.PPL
        }))) {
            yield return elem;
        }
    }

    private async IAsyncEnumerable<CaseCaseParties> ReadAbuseCaseInstitutionParties()
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
        await foreach (var elem in ReadCaseCaseRelations(sql, await _nodeIdReader.ReadAsync(new NodeIdReaderByUrlId.Request {
            UrlId = Constants.INSTITUTION_CASE_TYPE,
            TenantId = Constants.PPL
        }))) {
            yield return elem;
        }
    }
    private async IAsyncEnumerable<CaseCaseParties> ReadAbuseCaseTherapyParties()
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
        await foreach (var elem in ReadCaseCaseRelations(sql, await _nodeIdReader.ReadAsync(new NodeIdReaderByUrlId.Request {
            UrlId = Constants.THERAPY_CASE_TYPE,
            TenantId = Constants.PPL
        }))) {
            yield return elem;
        }
    }
    private async IAsyncEnumerable<CaseCaseParties> ReadChildTraffickingCasePlacementParties()
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
        await foreach (var elem in ReadCaseCaseRelations(sql, await _nodeIdReader.ReadAsync(new NodeIdReaderByUrlId.Request {
            UrlId = Constants.PLACEMENT_CASE_TYPE,
            TenantId = Constants.PPL
        }))) {
            yield return elem;
        }
    }

    private async IAsyncEnumerable<CaseCaseParties> ReadChildTraffickingCaseFacilitatorParties()
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
        await foreach (var elem in ReadCaseCaseRelations(sql, await _nodeIdReader.ReadAsync(new NodeIdReaderByUrlId.Request {
            UrlId = Constants.FACILITATION_CASE_TYPE,
            TenantId = Constants.PPL
        }))) {
            yield return elem;
        }
    }

    private async IAsyncEnumerable<CaseCaseParties> ReadChildTraffickingCaseOrphanageParties()
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
        await foreach (var elem in ReadCaseCaseRelations(sql, await _nodeIdReader.ReadAsync(new NodeIdReaderByUrlId.Request {
            UrlId = Constants.INSTITUTION_CASE_TYPE,
            TenantId = Constants.PPL
        }))) {
            yield return elem;
        }
    }

    private async IAsyncEnumerable<CaseCaseParties> ReadDisruptedPlacementCasePlacementParties()
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
        await foreach (var elem in ReadCaseCaseRelations(sql, await _nodeIdReader.ReadAsync(new NodeIdReaderByUrlId.Request {
            UrlId = Constants.PLACEMENT_CASE_TYPE,
            TenantId = Constants.PPL
        }))) {
            yield return elem;
        }
    }
    private async IAsyncEnumerable<CaseCaseParties> ReadDisruptedPlacementCaseFacilitatorParties()
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
        await foreach (var elem in ReadCaseCaseRelations(sql, await _nodeIdReader.ReadAsync(new NodeIdReaderByUrlId.Request {
            UrlId = Constants.FACILITATION_CASE_TYPE,
            TenantId = Constants.PPL
        }))) {
            yield return elem;
        }
    }
    private async IAsyncEnumerable<CaseCaseParties> ReadCoercedAdoptionCasePlacementParties()
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
        await foreach (var elem in ReadCaseCaseRelations(sql, await _nodeIdReader.ReadAsync(new NodeIdReaderByUrlId.Request {
            UrlId = Constants.PLACEMENT_CASE_TYPE,
            TenantId = Constants.PPL
        }))) {
            yield return elem;
        }
    }
    private async IAsyncEnumerable<CaseCaseParties> ReadFathersRightsViolationCasePlacementParties()
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
        await foreach (var elem in ReadCaseCaseRelations(sql, await _nodeIdReader.ReadAsync(new NodeIdReaderByUrlId.Request {
            UrlId = Constants.PLACEMENT_CASE_TYPE,
            TenantId = Constants.PPL
        }))) {
            yield return elem;
        }
    }
    private async IAsyncEnumerable<CaseCaseParties> ReadWrongfulRemovalCaseAuthorityParties()
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
        await foreach (var elem in ReadCaseCaseRelations(sql, await _nodeIdReader.ReadAsync(new NodeIdReaderByUrlId.Request {
            UrlId = Constants.AUTHORITIES_CASE_TYPE,
            TenantId = Constants.PPL
        }))) {
            yield return elem;
        }
    }
    private async IAsyncEnumerable<CaseCaseParties> ReadWrongfulMedicationCaseAuthorityParties()
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
        await foreach (var elem in ReadCaseCaseRelations(sql, await _nodeIdReader.ReadAsync(new NodeIdReaderByUrlId.Request {
            UrlId = Constants.AUTHORITIES_CASE_TYPE,
            TenantId = Constants.PPL
        }))) {
            yield return elem;
        }
    }

}
