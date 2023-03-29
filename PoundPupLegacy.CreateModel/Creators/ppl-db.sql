--
-- PostgreSQL database dump
--

-- Dumped from database version 14.7 (Ubuntu 14.7-1.pgdg20.04+1)
-- Dumped by pg_dump version 15.1

-- Started on 2023-02-28 16:09:55

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 6 (class 2615 OID 2200)
-- Name: public; Type: SCHEMA; Schema: -; Owner: postgres
--

-- *not* creating schema, since initdb creates it


ALTER SCHEMA public OWNER TO postgres;

--
-- TOC entry 3 (class 3079 OID 188353)
-- Name: btree_gist; Type: EXTENSION; Schema: -; Owner: -
--

CREATE EXTENSION IF NOT EXISTS btree_gist WITH SCHEMA public;


--
-- TOC entry 5106 (class 0 OID 0)
-- Dependencies: 3
-- Name: EXTENSION btree_gist; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION btree_gist IS 'support for indexing common datatypes in GiST';


--
-- TOC entry 2 (class 3079 OID 1291374)
-- Name: pg_trgm; Type: EXTENSION; Schema: -; Owner: -
--

CREATE EXTENSION IF NOT EXISTS pg_trgm WITH SCHEMA public;


--
-- TOC entry 5107 (class 0 OID 0)
-- Dependencies: 2
-- Name: EXTENSION pg_trgm; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION pg_trgm IS 'text similarity measurement and index searching based on trigrams';


--
-- TOC entry 577 (class 1255 OID 787796)
-- Name: authenticated_node(integer, integer, integer); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.authenticated_node(tenant_id integer, url_id integer, user_id integer) RETURNS TABLE(id integer, title text, node_type_id integer, tenant_id integer, node_id integer, publisher_id integer, created_date_time timestamp without time zone, changed_date_time timestamp without time zone, url_id integer, url_path text, subgroup_id integer, publication_status_id integer, has_been_published boolean)
    LANGUAGE sql
    AS $_$

select
	id,
	title,
	node_type_id,
	tenant_id,
	node_id,
	publisher_id,
	created_date_time,
	changed_date_time,
	url_id,
	url_path,
	subgroup_id,
	publication_status_id,
	case 
		when status = 0 then false
		else true
	end has_been_published
from(
	select
	tn.id,
	n.title,
	n.node_type_id,
	tn.tenant_id,
	tn.node_id,
	n.publisher_id,
	n.created_date_time,
	n.changed_date_time,
	tn.url_id,
	case 
		when tn.url_path is null then '/node/' || tn.url_id
		else '/' || url_path
	end url_path,
	tn.subgroup_id,
	tn.publication_status_id,
	case
		when tn.publication_status_id = 0 then (
			select
				case 
					when count(*) > 0 then 0
					else -1
				end status
			from user_group_user_role_user ugu
			WHERE ugu.user_group_id = 
			case
				when tn.subgroup_id is null then tn.tenant_id 
				else tn.subgroup_id 
			end 
			AND ugu.user_role_id = 6
			AND ugu.user_id = $3
		)
		when tn.publication_status_id = 1 then 1
		when tn.publication_status_id = 2 then (
			select
				case 
					when count(*) > 0 then 1
					else -1
				end status
			from user_group_user_role_user ugu
			WHERE ugu.user_group_id = 
				case
					when tn.subgroup_id is null then tn.tenant_id 
					else tn.subgroup_id 
				end
				AND ugu.user_id = $3
			)
		end status	
		from
		tenant_node tn
		join node n on n.id = tn.node_id
		WHERE tn.tenant_id = $1 and tn.url_id = $2
	) an
	where an.status <> -1
$_$;


ALTER FUNCTION public.authenticated_node(tenant_id integer, url_id integer, user_id integer) OWNER TO postgres;

--
-- TOC entry 576 (class 1255 OID 787795)
-- Name: f_comment_tree(integer); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.f_comment_tree(_comment_id integer) RETURNS jsonb
    LANGUAGE sql STABLE PARALLEL SAFE
    AS $$
SELECT jsonb_agg(sub)
FROM  (
   SELECT 
		c.id AS "Id", 
		c.node_status_id AS "NodeStatusId",
		json_build_object(
			'Id', p.id, 
			'Name', p.name,
			'CreatedDateTime', c.created_date_time
        ) AS "Authoring",
		c.title AS "Title", 
		c.text AS "Text", 
		f_comment_tree(c.id) AS "Comments"
	FROM comment c
	JOIN publisher p on p.id = c.publisher_id
	WHERE c.comment_id_parent = _comment_id
) sub
$$;


ALTER FUNCTION public.f_comment_tree(_comment_id integer) OWNER TO postgres;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 267 (class 1259 OID 69114)
-- Name: abuse_case; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.abuse_case (
    id integer NOT NULL,
    child_placement_type_id integer NOT NULL,
    family_size_id integer,
    home_schooling_involved boolean,
    fundamental_faith_involved boolean,
    disabilities_involved boolean
);


ALTER TABLE public.abuse_case OWNER TO postgres;

--
-- TOC entry 303 (class 1259 OID 189679)
-- Name: access_role; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.access_role (
    id integer NOT NULL
);


ALTER TABLE public.access_role OWNER TO postgres;

--
-- TOC entry 306 (class 1259 OID 189725)
-- Name: access_role_privilege; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.access_role_privilege (
    access_role_id integer NOT NULL,
    action_id integer NOT NULL
);


ALTER TABLE public.access_role_privilege OWNER TO postgres;

--
-- TOC entry 281 (class 1259 OID 187857)
-- Name: act; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.act (
    id integer NOT NULL,
    enactment_date date
);


ALTER TABLE public.act OWNER TO postgres;

--
-- TOC entry 338 (class 1259 OID 660709)
-- Name: action; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.action (
    id integer NOT NULL
);


ALTER TABLE public.action OWNER TO postgres;

--
-- TOC entry 339 (class 1259 OID 660714)
-- Name: action_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.action ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.action_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 342 (class 1259 OID 717670)
-- Name: action_menu_item; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.action_menu_item (
    id integer NOT NULL,
    action_id integer NOT NULL,
    name character varying NOT NULL
);


ALTER TABLE public.action_menu_item OWNER TO postgres;

--
-- TOC entry 354 (class 1259 OID 1855260)
-- Name: administrator_role; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.administrator_role (
    id integer NOT NULL,
    user_group_id integer NOT NULL
);


ALTER TABLE public.administrator_role OWNER TO postgres;

--
-- TOC entry 289 (class 1259 OID 189114)
-- Name: adoption_lawyer; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.adoption_lawyer (
    id integer NOT NULL
);


ALTER TABLE public.adoption_lawyer OWNER TO postgres;

--
-- TOC entry 279 (class 1259 OID 160186)
-- Name: article; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.article (
    id integer NOT NULL
);


ALTER TABLE public.article OWNER TO postgres;

--
-- TOC entry 263 (class 1259 OID 68333)
-- Name: attachment_therapist; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.attachment_therapist (
    id integer NOT NULL
);


ALTER TABLE public.attachment_therapist OWNER TO postgres;

--
-- TOC entry 335 (class 1259 OID 660655)
-- Name: basic_action; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.basic_action (
    id integer NOT NULL,
    path character varying(255) NOT NULL,
    description character varying NOT NULL
);


ALTER TABLE public.basic_action OWNER TO postgres;

--
-- TOC entry 248 (class 1259 OID 48035)
-- Name: basic_country; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.basic_country (
    id integer NOT NULL
);


ALTER TABLE public.basic_country OWNER TO postgres;

--
-- TOC entry 256 (class 1259 OID 48193)
-- Name: basic_first_and_second_level_subdivision; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.basic_first_and_second_level_subdivision (
    id integer NOT NULL
);


ALTER TABLE public.basic_first_and_second_level_subdivision OWNER TO postgres;

--
-- TOC entry 216 (class 1259 OID 32825)
-- Name: basic_nameable; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.basic_nameable (
    id integer NOT NULL
);


ALTER TABLE public.basic_nameable OWNER TO postgres;

--
-- TOC entry 250 (class 1259 OID 48104)
-- Name: basic_second_level_subdivision; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.basic_second_level_subdivision (
    id integer NOT NULL,
    intermediate_level_subdivision_id integer NOT NULL
);


ALTER TABLE public.basic_second_level_subdivision OWNER TO postgres;

--
-- TOC entry 333 (class 1259 OID 636047)
-- Name: bill; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.bill (
    id integer NOT NULL,
    introduction_date date
);


ALTER TABLE public.bill OWNER TO postgres;

--
-- TOC entry 329 (class 1259 OID 575880)
-- Name: bill_action_type; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.bill_action_type (
    id integer NOT NULL
);


ALTER TABLE public.bill_action_type OWNER TO postgres;

--
-- TOC entry 245 (class 1259 OID 47820)
-- Name: binding_country; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.binding_country (
    id integer NOT NULL
);


ALTER TABLE public.binding_country OWNER TO postgres;

--
-- TOC entry 278 (class 1259 OID 160173)
-- Name: blog_post; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.blog_post (
    id integer NOT NULL
);


ALTER TABLE public.blog_post OWNER TO postgres;

--
-- TOC entry 259 (class 1259 OID 56894)
-- Name: bottom_level_subdivision; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.bottom_level_subdivision (
    id integer NOT NULL
);


ALTER TABLE public.bottom_level_subdivision OWNER TO postgres;

--
-- TOC entry 238 (class 1259 OID 35152)
-- Name: bound_country; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.bound_country (
    id integer NOT NULL,
    binding_country_id integer NOT NULL
);


ALTER TABLE public.bound_country OWNER TO postgres;

--
-- TOC entry 265 (class 1259 OID 69098)
-- Name: case; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."case" (
    id integer NOT NULL,
    description character varying NOT NULL,
    date_range daterange,
    date date
);


ALTER TABLE public."case" OWNER TO postgres;

--
-- TOC entry 362 (class 1259 OID 2015918)
-- Name: case_case_parties; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.case_case_parties (
    case_id integer NOT NULL,
    case_parties_id integer NOT NULL,
    case_party_type_id integer NOT NULL
);


ALTER TABLE public.case_case_parties OWNER TO postgres;

--
-- TOC entry 357 (class 1259 OID 2015362)
-- Name: case_parties; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.case_parties (
    id integer NOT NULL,
    organizations character varying,
    persons character varying
);


ALTER TABLE public.case_parties OWNER TO postgres;

--
-- TOC entry 359 (class 1259 OID 2015370)
-- Name: case_parties_organization; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.case_parties_organization (
    case_parties_id integer NOT NULL,
    organization_id integer NOT NULL
);


ALTER TABLE public.case_parties_organization OWNER TO postgres;

--
-- TOC entry 360 (class 1259 OID 2015375)
-- Name: case_parties_person; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.case_parties_person (
    case_parties_id integer NOT NULL,
    person_id integer NOT NULL
);


ALTER TABLE public.case_parties_person OWNER TO postgres;

--
-- TOC entry 361 (class 1259 OID 2015909)
-- Name: case_party_type; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.case_party_type (
    id integer NOT NULL
);


ALTER TABLE public.case_party_type OWNER TO postgres;

--
-- TOC entry 358 (class 1259 OID 2015369)
-- Name: case_relations_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.case_parties ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.case_relations_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 363 (class 1259 OID 2015941)
-- Name: case_type; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.case_type (
    id integer NOT NULL
);


ALTER TABLE public.case_type OWNER TO postgres;

--
-- TOC entry 364 (class 1259 OID 2015952)
-- Name: case_type_case_party_type; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.case_type_case_party_type (
    case_type_id integer NOT NULL,
    case_party_type_id integer NOT NULL
);


ALTER TABLE public.case_type_case_party_type OWNER TO postgres;

--
-- TOC entry 275 (class 1259 OID 116009)
-- Name: child_placement_type; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.child_placement_type (
    id integer NOT NULL
);


ALTER TABLE public.child_placement_type OWNER TO postgres;

--
-- TOC entry 268 (class 1259 OID 69125)
-- Name: child_trafficking_case; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.child_trafficking_case (
    id integer NOT NULL,
    number_of_children_involved integer,
    country_id_from integer NOT NULL
);


ALTER TABLE public.child_trafficking_case OWNER TO postgres;

--
-- TOC entry 271 (class 1259 OID 69157)
-- Name: coerced_adoption_case; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.coerced_adoption_case (
    id integer NOT NULL
);


ALTER TABLE public.coerced_adoption_case OWNER TO postgres;

--
-- TOC entry 320 (class 1259 OID 545086)
-- Name: collective; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.collective (
    id integer NOT NULL
);


ALTER TABLE public.collective OWNER TO postgres;

--
-- TOC entry 321 (class 1259 OID 545097)
-- Name: collective_user; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.collective_user (
    collective_id integer NOT NULL,
    user_id integer NOT NULL
);


ALTER TABLE public.collective_user OWNER TO postgres;

--
-- TOC entry 214 (class 1259 OID 32793)
-- Name: comment; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.comment (
    id integer NOT NULL,
    node_id integer NOT NULL,
    comment_id_parent integer,
    text character varying NOT NULL,
    publisher_id integer NOT NULL,
    node_status_id integer NOT NULL,
    created_date_time timestamp without time zone NOT NULL,
    ip_address character varying(15) NOT NULL,
    title character varying(64)
);


ALTER TABLE public.comment OWNER TO postgres;

--
-- TOC entry 309 (class 1259 OID 403137)
-- Name: comment_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.comment ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.comment_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 381 (class 1259 OID 3684061)
-- Name: congressional_term; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.congressional_term (
    id integer NOT NULL
);


ALTER TABLE public.congressional_term OWNER TO postgres;

--
-- TOC entry 382 (class 1259 OID 3684072)
-- Name: congressional_term_political_party_affiliation; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.congressional_term_political_party_affiliation (
    id integer NOT NULL,
    congressional_term_id integer NOT NULL,
    united_states_political_party_affiliation_id integer NOT NULL,
    date_range daterange NOT NULL
);


ALTER TABLE public.congressional_term_political_party_affiliation OWNER TO postgres;

--
-- TOC entry 323 (class 1259 OID 545507)
-- Name: content_sharing_group; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.content_sharing_group (
    id integer NOT NULL
);


ALTER TABLE public.content_sharing_group OWNER TO postgres;

--
-- TOC entry 219 (class 1259 OID 32858)
-- Name: country; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.country (
    id integer NOT NULL,
    hague_status_id integer NOT NULL,
    residency_requirements character varying,
    age_requirements character varying,
    marriage_requirements character varying,
    income_requirements character varying,
    health_requirements character varying,
    other_requirements character varying
);


ALTER TABLE public.country OWNER TO postgres;

--
-- TOC entry 257 (class 1259 OID 48204)
-- Name: country_and_first_and_bottom_level_subdivision; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.country_and_first_and_bottom_level_subdivision (
    id integer NOT NULL
);


ALTER TABLE public.country_and_first_and_bottom_level_subdivision OWNER TO postgres;

--
-- TOC entry 255 (class 1259 OID 48176)
-- Name: country_and_first_and_second_level_subdivision; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.country_and_first_and_second_level_subdivision (
    id integer NOT NULL
);


ALTER TABLE public.country_and_first_and_second_level_subdivision OWNER TO postgres;

--
-- TOC entry 251 (class 1259 OID 48120)
-- Name: country_and_first_level_subdivision; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.country_and_first_level_subdivision (
    id integer NOT NULL
);


ALTER TABLE public.country_and_first_level_subdivision OWNER TO postgres;

--
-- TOC entry 261 (class 1259 OID 58172)
-- Name: country_and_intermediate_level_subdivision; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.country_and_intermediate_level_subdivision (
    id integer NOT NULL
);


ALTER TABLE public.country_and_intermediate_level_subdivision OWNER TO postgres;

--
-- TOC entry 348 (class 1259 OID 878710)
-- Name: country_report; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.country_report (
    country_id integer NOT NULL,
    date_range daterange NOT NULL,
    number_of_children_imported integer NOT NULL,
    number_of_children_imported_of_unknown_origin integer NOT NULL
);


ALTER TABLE public.country_report OWNER TO postgres;

--
-- TOC entry 351 (class 1259 OID 960317)
-- Name: country_subdivision_type; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.country_subdivision_type (
    country_id integer NOT NULL,
    subdivision_type_id integer NOT NULL
);


ALTER TABLE public.country_subdivision_type OWNER TO postgres;

--
-- TOC entry 334 (class 1259 OID 660636)
-- Name: create_node_action; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.create_node_action (
    id integer NOT NULL,
    node_type_id integer NOT NULL
);


ALTER TABLE public.create_node_action OWNER TO postgres;

--
-- TOC entry 336 (class 1259 OID 660675)
-- Name: delete_node_action; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.delete_node_action (
    id integer NOT NULL,
    node_type_id integer NOT NULL
);


ALTER TABLE public.delete_node_action OWNER TO postgres;

--
-- TOC entry 225 (class 1259 OID 32950)
-- Name: denomination; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.denomination (
    id integer NOT NULL
);


ALTER TABLE public.denomination OWNER TO postgres;

--
-- TOC entry 277 (class 1259 OID 144382)
-- Name: deportation_case; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.deportation_case (
    id integer NOT NULL,
    subdivision_id_from integer,
    country_id_to integer
);


ALTER TABLE public.deportation_case OWNER TO postgres;

--
-- TOC entry 280 (class 1259 OID 160199)
-- Name: discussion; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.discussion (
    id integer NOT NULL
);


ALTER TABLE public.discussion OWNER TO postgres;

--
-- TOC entry 273 (class 1259 OID 69178)
-- Name: disrupted_placement_case; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.disrupted_placement_case (
    id integer NOT NULL
);


ALTER TABLE public.disrupted_placement_case OWNER TO postgres;

--
-- TOC entry 232 (class 1259 OID 33040)
-- Name: document; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.document (
    id integer NOT NULL,
    source_url character varying(255),
    text character varying NOT NULL,
    document_type_id integer,
    publication_date_range daterange,
    publication_date date,
    teaser character varying NOT NULL,
    CONSTRAINT chk_document CHECK ((NOT ((publication_date IS NOT NULL) AND (publication_date_range IS NOT NULL))))
);


ALTER TABLE public.document OWNER TO postgres;

--
-- TOC entry 227 (class 1259 OID 32972)
-- Name: document_type; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.document_type (
    id integer NOT NULL
);


ALTER TABLE public.document_type OWNER TO postgres;

--
-- TOC entry 262 (class 1259 OID 67710)
-- Name: documentable; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.documentable (
    id integer NOT NULL
);


ALTER TABLE public.documentable OWNER TO postgres;

--
-- TOC entry 345 (class 1259 OID 787797)
-- Name: documentable_document; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.documentable_document (
    documentable_id integer NOT NULL,
    document_id integer NOT NULL
);


ALTER TABLE public.documentable_document OWNER TO postgres;

--
-- TOC entry 337 (class 1259 OID 660692)
-- Name: edit_node_action; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.edit_node_action (
    id integer NOT NULL,
    node_type_id integer NOT NULL
);


ALTER TABLE public.edit_node_action OWNER TO postgres;

--
-- TOC entry 296 (class 1259 OID 189196)
-- Name: facilitator; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.facilitator (
    id integer NOT NULL
);


ALTER TABLE public.facilitator OWNER TO postgres;

--
-- TOC entry 276 (class 1259 OID 116020)
-- Name: family_size; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.family_size (
    id integer NOT NULL
);


ALTER TABLE public.family_size OWNER TO postgres;

--
-- TOC entry 272 (class 1259 OID 69167)
-- Name: fathers_rights_violation_case; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.fathers_rights_violation_case (
    id integer NOT NULL
);


ALTER TABLE public.fathers_rights_violation_case OWNER TO postgres;

--
-- TOC entry 213 (class 1259 OID 32786)
-- Name: file; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.file (
    id integer NOT NULL,
    path character varying(255) NOT NULL,
    name character varying(255) NOT NULL,
    mime_type character varying(255) NOT NULL,
    size integer NOT NULL
);


ALTER TABLE public.file OWNER TO postgres;

--
-- TOC entry 366 (class 1259 OID 2384539)
-- Name: file_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.file ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.file_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 260 (class 1259 OID 56926)
-- Name: first_and_bottom_level_subdivision; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.first_and_bottom_level_subdivision (
    id integer NOT NULL
);


ALTER TABLE public.first_and_bottom_level_subdivision OWNER TO postgres;

--
-- TOC entry 252 (class 1259 OID 48137)
-- Name: first_and_second_level_subdivision; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.first_and_second_level_subdivision (
    id integer NOT NULL
);


ALTER TABLE public.first_and_second_level_subdivision OWNER TO postgres;

--
-- TOC entry 240 (class 1259 OID 35180)
-- Name: first_level_global_region; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.first_level_global_region (
    id integer NOT NULL
);


ALTER TABLE public.first_level_global_region OWNER TO postgres;

--
-- TOC entry 241 (class 1259 OID 35767)
-- Name: first_level_subdivision; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.first_level_subdivision (
    id integer NOT NULL
);


ALTER TABLE public.first_level_subdivision OWNER TO postgres;

--
-- TOC entry 253 (class 1259 OID 48154)
-- Name: formal_intermediate_level_subdivision; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.formal_intermediate_level_subdivision (
    id integer NOT NULL
);


ALTER TABLE public.formal_intermediate_level_subdivision OWNER TO postgres;

--
-- TOC entry 246 (class 1259 OID 47992)
-- Name: geographical_entity; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.geographical_entity (
    id integer NOT NULL
);


ALTER TABLE public.geographical_entity OWNER TO postgres;

--
-- TOC entry 247 (class 1259 OID 48007)
-- Name: global_region; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.global_region (
    id integer NOT NULL
);


ALTER TABLE public.global_region OWNER TO postgres;

--
-- TOC entry 226 (class 1259 OID 32961)
-- Name: hague_status; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.hague_status (
    id integer NOT NULL
);


ALTER TABLE public.hague_status OWNER TO postgres;

--
-- TOC entry 298 (class 1259 OID 189218)
-- Name: home_study_agency; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.home_study_agency (
    id integer NOT NULL
);


ALTER TABLE public.home_study_agency OWNER TO postgres;

--
-- TOC entry 324 (class 1259 OID 575819)
-- Name: house_bill; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.house_bill (
    id integer NOT NULL
);


ALTER TABLE public.house_bill OWNER TO postgres;

--
-- TOC entry 379 (class 1259 OID 3684013)
-- Name: house_term; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.house_term (
    id integer NOT NULL,
    representative_id integer NOT NULL,
    subdivision_id integer NOT NULL,
    district integer,
    date_range daterange NOT NULL
);


ALTER TABLE public.house_term OWNER TO postgres;

--
-- TOC entry 254 (class 1259 OID 48165)
-- Name: informal_intermediate_level_subdivision; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.informal_intermediate_level_subdivision (
    id integer NOT NULL
);


ALTER TABLE public.informal_intermediate_level_subdivision OWNER TO postgres;

--
-- TOC entry 299 (class 1259 OID 189229)
-- Name: institution; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.institution (
    id integer NOT NULL
);


ALTER TABLE public.institution OWNER TO postgres;

--
-- TOC entry 347 (class 1259 OID 860241)
-- Name: inter_country_relation; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.inter_country_relation (
    id integer NOT NULL,
    country_id_from integer NOT NULL,
    country_id_to integer NOT NULL,
    date_range daterange,
    inter_country_relation_type_id integer NOT NULL,
    number_of_children_involved integer,
    money_involved numeric,
    document_id_proof integer
);


ALTER TABLE public.inter_country_relation OWNER TO postgres;

--
-- TOC entry 346 (class 1259 OID 860230)
-- Name: inter_country_relation_type; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.inter_country_relation_type (
    id integer NOT NULL,
    is_symmetric boolean NOT NULL
);


ALTER TABLE public.inter_country_relation_type OWNER TO postgres;

--
-- TOC entry 231 (class 1259 OID 33029)
-- Name: inter_organizational_relation; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.inter_organizational_relation (
    id integer NOT NULL,
    organization_id_from integer NOT NULL,
    organization_id_to integer NOT NULL,
    document_id_proof integer,
    inter_organizational_relation_type_id integer,
    geographical_entity_id integer,
    money_involved numeric,
    number_of_children_involved integer,
    date_range daterange,
    description character varying
);


ALTER TABLE public.inter_organizational_relation OWNER TO postgres;

--
-- TOC entry 218 (class 1259 OID 32847)
-- Name: inter_organizational_relation_type; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.inter_organizational_relation_type (
    id integer NOT NULL,
    is_symmetric boolean NOT NULL
);


ALTER TABLE public.inter_organizational_relation_type OWNER TO postgres;

--
-- TOC entry 233 (class 1259 OID 33069)
-- Name: inter_personal_relation; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.inter_personal_relation (
    id integer NOT NULL,
    person_id_from integer NOT NULL,
    person_id_to integer NOT NULL,
    inter_personal_relation_type_id integer NOT NULL,
    document_id_proof integer,
    date_range daterange
);


ALTER TABLE public.inter_personal_relation OWNER TO postgres;

--
-- TOC entry 292 (class 1259 OID 189157)
-- Name: inter_personal_relation_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.inter_personal_relation ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.inter_personal_relation_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 223 (class 1259 OID 32928)
-- Name: inter_personal_relation_type; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.inter_personal_relation_type (
    id integer NOT NULL,
    is_symmetric boolean NOT NULL
);


ALTER TABLE public.inter_personal_relation_type OWNER TO postgres;

--
-- TOC entry 258 (class 1259 OID 56889)
-- Name: intermediate_level_subdivision; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.intermediate_level_subdivision (
    id integer NOT NULL
);


ALTER TABLE public.intermediate_level_subdivision OWNER TO postgres;

--
-- TOC entry 249 (class 1259 OID 48087)
-- Name: iso_coded_first_level_subdivision; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.iso_coded_first_level_subdivision (
    id integer NOT NULL
);


ALTER TABLE public.iso_coded_first_level_subdivision OWNER TO postgres;

--
-- TOC entry 242 (class 1259 OID 35772)
-- Name: iso_coded_subdivision; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.iso_coded_subdivision (
    id integer NOT NULL,
    iso_3166_2_code character varying(10) NOT NULL
);


ALTER TABLE public.iso_coded_subdivision OWNER TO postgres;

--
-- TOC entry 297 (class 1259 OID 189207)
-- Name: law_firm; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.law_firm (
    id integer NOT NULL
);


ALTER TABLE public.law_firm OWNER TO postgres;

--
-- TOC entry 353 (class 1259 OID 1797645)
-- Name: layout; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.layout (
    id integer NOT NULL,
    file_name character varying(255) NOT NULL
);


ALTER TABLE public.layout OWNER TO postgres;

--
-- TOC entry 264 (class 1259 OID 69082)
-- Name: locatable; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.locatable (
    id integer NOT NULL
);


ALTER TABLE public.locatable OWNER TO postgres;

--
-- TOC entry 236 (class 1259 OID 33750)
-- Name: location; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.location (
    id integer NOT NULL,
    country_id integer NOT NULL,
    subdivision_id integer,
    street character varying(255),
    additional character varying(255),
    city character varying(255),
    postal_code character varying(16),
    latitude numeric(10,6),
    longitude numeric(10,6)
);


ALTER TABLE public.location OWNER TO postgres;

--
-- TOC entry 274 (class 1259 OID 69403)
-- Name: location_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.location ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.location_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 266 (class 1259 OID 69109)
-- Name: location_locatable; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.location_locatable (
    location_id integer NOT NULL,
    locatable_id integer NOT NULL
);


ALTER TABLE public.location_locatable OWNER TO postgres;

--
-- TOC entry 326 (class 1259 OID 575841)
-- Name: member_of_congress; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.member_of_congress (
    id integer NOT NULL
);


ALTER TABLE public.member_of_congress OWNER TO postgres;

--
-- TOC entry 340 (class 1259 OID 717663)
-- Name: menu_item; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.menu_item (
    id integer NOT NULL,
    weight double precision
);


ALTER TABLE public.menu_item OWNER TO postgres;

--
-- TOC entry 341 (class 1259 OID 717666)
-- Name: menu_item_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.menu_item ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.menu_item_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 374 (class 1259 OID 2708794)
-- Name: multi_question_poll; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.multi_question_poll (
    id integer NOT NULL
);


ALTER TABLE public.multi_question_poll OWNER TO postgres;

--
-- TOC entry 376 (class 1259 OID 2708839)
-- Name: multi_question_poll_poll_question; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.multi_question_poll_poll_question (
    multi_question_poll_id integer NOT NULL,
    poll_question_id integer NOT NULL,
    delta integer NOT NULL
);


ALTER TABLE public.multi_question_poll_poll_question OWNER TO postgres;

--
-- TOC entry 282 (class 1259 OID 187876)
-- Name: nameable; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.nameable (
    id integer NOT NULL,
    description character varying,
    file_id_tile_image integer
);


ALTER TABLE public.nameable OWNER TO postgres;

--
-- TOC entry 212 (class 1259 OID 32773)
-- Name: node; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.node (
    id integer NOT NULL,
    publisher_id integer,
    title character varying(128) NOT NULL,
    created_date_time timestamp without time zone NOT NULL,
    changed_date_time timestamp without time zone NOT NULL,
    node_type_id integer NOT NULL,
    owner_id integer NOT NULL
);


ALTER TABLE public.node OWNER TO postgres;

--
-- TOC entry 367 (class 1259 OID 2403967)
-- Name: node_file; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.node_file (
    node_id integer NOT NULL,
    file_id integer NOT NULL
);


ALTER TABLE public.node_file OWNER TO postgres;

--
-- TOC entry 235 (class 1259 OID 33136)
-- Name: node_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.node ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.node_id_seq
    START WITH 100000
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 308 (class 1259 OID 403130)
-- Name: node_term; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.node_term (
    node_id integer NOT NULL,
    term_id integer NOT NULL
);


ALTER TABLE public.node_term OWNER TO postgres;

--
-- TOC entry 215 (class 1259 OID 32812)
-- Name: node_type; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.node_type (
    id integer NOT NULL,
    name character varying(255) NOT NULL,
    description character varying NOT NULL
);


ALTER TABLE public.node_type OWNER TO postgres;

--
-- TOC entry 228 (class 1259 OID 32996)
-- Name: organization; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.organization (
    id integer NOT NULL,
    website_url character varying(255),
    email_address character varying(255),
    description character varying,
    established timestamp without time zone,
    terminated timestamp without time zone
);


ALTER TABLE public.organization OWNER TO postgres;

--
-- TOC entry 332 (class 1259 OID 575944)
-- Name: organization_act_relation_type; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.organization_act_relation_type (
    id integer NOT NULL
);


ALTER TABLE public.organization_act_relation_type OWNER TO postgres;

--
-- TOC entry 349 (class 1259 OID 899648)
-- Name: organization_organization_type; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.organization_organization_type (
    organization_id integer NOT NULL,
    organization_type_id integer NOT NULL
);


ALTER TABLE public.organization_organization_type OWNER TO postgres;

--
-- TOC entry 217 (class 1259 OID 32836)
-- Name: organization_type; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.organization_type (
    id integer NOT NULL,
    has_concrete_subtype boolean NOT NULL
);


ALTER TABLE public.organization_type OWNER TO postgres;

--
-- TOC entry 293 (class 1259 OID 189158)
-- Name: organizational_role; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.organizational_role (
    id integer NOT NULL,
    organization_id integer NOT NULL,
    organization_type_id integer NOT NULL,
    daterange daterange
);


ALTER TABLE public.organizational_role OWNER TO postgres;

--
-- TOC entry 294 (class 1259 OID 189165)
-- Name: organizational_role_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.organizational_role ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.organizational_role_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 322 (class 1259 OID 545102)
-- Name: owner; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.owner (
    id integer NOT NULL
);


ALTER TABLE public.owner OWNER TO postgres;

--
-- TOC entry 310 (class 1259 OID 403143)
-- Name: page; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.page (
    id integer NOT NULL
);


ALTER TABLE public.page OWNER TO postgres;

--
-- TOC entry 229 (class 1259 OID 33001)
-- Name: party; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.party (
    id integer NOT NULL
);


ALTER TABLE public.party OWNER TO postgres;

--
-- TOC entry 285 (class 1259 OID 188346)
-- Name: party_political_entity_relation; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.party_political_entity_relation (
    id integer NOT NULL,
    political_entity_id integer NOT NULL,
    party_id integer NOT NULL,
    party_political_entity_relation_type_id integer NOT NULL,
    date_range daterange,
    document_id_proof integer
);


ALTER TABLE public.party_political_entity_relation OWNER TO postgres;

--
-- TOC entry 221 (class 1259 OID 32902)
-- Name: party_political_entity_relation_type; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.party_political_entity_relation_type (
    id integer NOT NULL,
    has_concrete_subtype boolean NOT NULL
);


ALTER TABLE public.party_political_entity_relation_type OWNER TO postgres;

--
-- TOC entry 230 (class 1259 OID 33018)
-- Name: person; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.person (
    id integer NOT NULL,
    date_of_birth timestamp without time zone,
    date_of_death timestamp without time zone,
    file_id_portrait integer,
    first_name character varying(100),
    middle_name character varying(100),
    last_name character varying(100),
    full_name character varying(100),
    suffix character varying(100),
    nick_name character varying(100),
    govtrack_id integer,
    bioguide character varying(50)
);


ALTER TABLE public.person OWNER TO postgres;

--
-- TOC entry 286 (class 1259 OID 188978)
-- Name: person_organization_relation; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.person_organization_relation (
    id integer NOT NULL,
    person_id integer NOT NULL,
    organization_id integer NOT NULL,
    person_organization_relation_type_id integer NOT NULL,
    date_range daterange,
    document_id_proof integer,
    description character varying,
    geographical_entity_id integer
);


ALTER TABLE public.person_organization_relation OWNER TO postgres;

--
-- TOC entry 222 (class 1259 OID 32907)
-- Name: person_organization_relation_type; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.person_organization_relation_type (
    id integer NOT NULL,
    has_concrete_subtype boolean NOT NULL
);


ALTER TABLE public.person_organization_relation_type OWNER TO postgres;

--
-- TOC entry 295 (class 1259 OID 189185)
-- Name: placement_agency; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.placement_agency (
    id integer NOT NULL
);


ALTER TABLE public.placement_agency OWNER TO postgres;

--
-- TOC entry 234 (class 1259 OID 33104)
-- Name: political_entity; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.political_entity (
    id integer NOT NULL,
    file_id_flag integer
);


ALTER TABLE public.political_entity OWNER TO postgres;

--
-- TOC entry 368 (class 1259 OID 2653879)
-- Name: poll; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.poll (
    id integer NOT NULL,
    poll_status_id integer NOT NULL,
    date_time_closure timestamp without time zone NOT NULL
);


ALTER TABLE public.poll OWNER TO postgres;

--
-- TOC entry 369 (class 1259 OID 2653892)
-- Name: poll_option; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.poll_option (
    poll_question_id integer NOT NULL,
    delta integer NOT NULL,
    text character varying NOT NULL,
    number_of_votes integer NOT NULL
);


ALTER TABLE public.poll_option OWNER TO postgres;

--
-- TOC entry 375 (class 1259 OID 2708805)
-- Name: poll_question; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.poll_question (
    id integer NOT NULL,
    question character varying NOT NULL
);


ALTER TABLE public.poll_question OWNER TO postgres;

--
-- TOC entry 371 (class 1259 OID 2653927)
-- Name: poll_status; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.poll_status (
    id integer NOT NULL,
    name character varying(255) NOT NULL
);


ALTER TABLE public.poll_status OWNER TO postgres;

--
-- TOC entry 370 (class 1259 OID 2653899)
-- Name: poll_vote; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.poll_vote (
    id integer NOT NULL,
    poll_id integer NOT NULL,
    delta integer NOT NULL,
    user_id integer,
    ip_address character varying
);


ALTER TABLE public.poll_vote OWNER TO postgres;

--
-- TOC entry 372 (class 1259 OID 2653956)
-- Name: poll_vote_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.poll_vote ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.poll_vote_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 300 (class 1259 OID 189240)
-- Name: post_placement_agency; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.post_placement_agency (
    id integer NOT NULL
);


ALTER TABLE public.post_placement_agency OWNER TO postgres;

--
-- TOC entry 317 (class 1259 OID 545055)
-- Name: principal; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.principal (
    id integer NOT NULL
);


ALTER TABLE public.principal OWNER TO postgres;

--
-- TOC entry 318 (class 1259 OID 545062)
-- Name: principal_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.principal ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.principal_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 224 (class 1259 OID 32939)
-- Name: profession; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.profession (
    id integer NOT NULL,
    has_concrete_subtype boolean NOT NULL
);


ALTER TABLE public.profession OWNER TO postgres;

--
-- TOC entry 290 (class 1259 OID 189119)
-- Name: professional_role; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.professional_role (
    id integer NOT NULL,
    person_id integer NOT NULL,
    daterange daterange,
    profession_id integer NOT NULL
);


ALTER TABLE public.professional_role OWNER TO postgres;

--
-- TOC entry 291 (class 1259 OID 189155)
-- Name: professional_role_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.professional_role ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.professional_role_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 287 (class 1259 OID 189046)
-- Name: publication_status; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.publication_status (
    id integer NOT NULL,
    name character varying NOT NULL
);


ALTER TABLE public.publication_status OWNER TO postgres;

--
-- TOC entry 319 (class 1259 OID 545069)
-- Name: publisher; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.publisher (
    id integer NOT NULL,
    name character varying(100) NOT NULL
);


ALTER TABLE public.publisher OWNER TO postgres;

--
-- TOC entry 327 (class 1259 OID 575852)
-- Name: representative; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.representative (
    id integer NOT NULL
);


ALTER TABLE public.representative OWNER TO postgres;

--
-- TOC entry 330 (class 1259 OID 575889)
-- Name: representative_house_bill_action; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.representative_house_bill_action (
    id integer NOT NULL,
    representative_id integer NOT NULL,
    house_bill_id integer NOT NULL,
    bill_action_type_id integer NOT NULL,
    date date NOT NULL
);


ALTER TABLE public.representative_house_bill_action OWNER TO postgres;

--
-- TOC entry 311 (class 1259 OID 403705)
-- Name: review; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.review (
    id integer NOT NULL
);


ALTER TABLE public.review OWNER TO postgres;

--
-- TOC entry 356 (class 1259 OID 1910327)
-- Name: searchable; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.searchable (
    id integer NOT NULL,
    tsvector tsvector
);


ALTER TABLE public.searchable OWNER TO postgres;

--
-- TOC entry 220 (class 1259 OID 32863)
-- Name: second_level_global_region; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.second_level_global_region (
    id integer NOT NULL,
    first_level_global_region_id integer NOT NULL
);


ALTER TABLE public.second_level_global_region OWNER TO postgres;

--
-- TOC entry 239 (class 1259 OID 35163)
-- Name: second_level_subdivision; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.second_level_subdivision (
    id integer NOT NULL
);


ALTER TABLE public.second_level_subdivision OWNER TO postgres;

--
-- TOC entry 325 (class 1259 OID 575830)
-- Name: senate_bill; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.senate_bill (
    id integer NOT NULL
);


ALTER TABLE public.senate_bill OWNER TO postgres;

--
-- TOC entry 378 (class 1259 OID 3683980)
-- Name: senate_term; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.senate_term (
    id integer NOT NULL,
    senator_id integer NOT NULL,
    subdivision_id integer NOT NULL,
    date_range daterange NOT NULL
);


ALTER TABLE public.senate_term OWNER TO postgres;

--
-- TOC entry 328 (class 1259 OID 575863)
-- Name: senator; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.senator (
    id integer NOT NULL
);


ALTER TABLE public.senator OWNER TO postgres;

--
-- TOC entry 331 (class 1259 OID 575914)
-- Name: senator_senate_bill_action; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.senator_senate_bill_action (
    id integer NOT NULL,
    senator_id integer NOT NULL,
    senate_bill_id integer NOT NULL,
    bill_action_type_id integer NOT NULL,
    date date NOT NULL
);


ALTER TABLE public.senator_senate_bill_action OWNER TO postgres;

--
-- TOC entry 288 (class 1259 OID 189061)
-- Name: simple_text_node; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.simple_text_node (
    id integer NOT NULL,
    text character varying NOT NULL,
    teaser character varying NOT NULL
);


ALTER TABLE public.simple_text_node OWNER TO postgres;

--
-- TOC entry 373 (class 1259 OID 2708781)
-- Name: single_question_poll; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.single_question_poll (
    id integer NOT NULL
);


ALTER TABLE public.single_question_poll OWNER TO postgres;

--
-- TOC entry 244 (class 1259 OID 43482)
-- Name: subdivision; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.subdivision (
    id integer NOT NULL,
    country_id integer NOT NULL,
    name character varying(100) NOT NULL,
    subdivision_type_id integer NOT NULL
);


ALTER TABLE public.subdivision OWNER TO postgres;

--
-- TOC entry 350 (class 1259 OID 958470)
-- Name: subdivision_type; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.subdivision_type (
    id integer NOT NULL
);


ALTER TABLE public.subdivision_type OWNER TO postgres;

--
-- TOC entry 314 (class 1259 OID 544973)
-- Name: subgroup; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.subgroup (
    id integer NOT NULL,
    tenant_id integer NOT NULL
);


ALTER TABLE public.subgroup OWNER TO postgres;

--
-- TOC entry 355 (class 1259 OID 1875508)
-- Name: system_group; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.system_group (
    id integer NOT NULL
);


ALTER TABLE public.system_group OWNER TO postgres;

--
-- TOC entry 313 (class 1259 OID 544940)
-- Name: tenant; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.tenant (
    id integer NOT NULL,
    vocabulary_id_tagging integer,
    domain_name character varying(255) NOT NULL,
    access_role_id_not_logged_in integer NOT NULL
);


ALTER TABLE public.tenant OWNER TO postgres;

--
-- TOC entry 365 (class 1259 OID 2383872)
-- Name: tenant_file; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.tenant_file (
    tenant_id integer NOT NULL,
    file_id integer NOT NULL,
    tenant_file_id integer NOT NULL
);


ALTER TABLE public.tenant_file OWNER TO postgres;

--
-- TOC entry 316 (class 1259 OID 544995)
-- Name: tenant_node; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.tenant_node (
    tenant_id integer NOT NULL,
    url_id integer NOT NULL,
    url_path character varying(255),
    node_id integer NOT NULL,
    subgroup_id integer,
    publication_status_id integer NOT NULL,
    id integer NOT NULL
);


ALTER TABLE public.tenant_node OWNER TO postgres;

--
-- TOC entry 343 (class 1259 OID 717700)
-- Name: tenant_node_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.tenant_node ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.tenant_node_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 344 (class 1259 OID 717701)
-- Name: tenant_node_menu_item; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.tenant_node_menu_item (
    id integer NOT NULL,
    tenant_node_id integer NOT NULL,
    name character varying(255) NOT NULL
);


ALTER TABLE public.tenant_node_menu_item OWNER TO postgres;

--
-- TOC entry 284 (class 1259 OID 188216)
-- Name: term; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.term (
    id integer NOT NULL,
    vocabulary_id integer NOT NULL,
    name character varying NOT NULL,
    nameable_id integer NOT NULL
);


ALTER TABLE public.term OWNER TO postgres;

--
-- TOC entry 237 (class 1259 OID 33765)
-- Name: term_hierarchy; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.term_hierarchy (
    term_id_parent integer NOT NULL,
    term_id_child integer NOT NULL
);


ALTER TABLE public.term_hierarchy OWNER TO postgres;

--
-- TOC entry 307 (class 1259 OID 195677)
-- Name: term_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.term ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.term_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 243 (class 1259 OID 37399)
-- Name: top_level_country; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.top_level_country (
    id integer NOT NULL,
    iso_3166_1_code character(2) NOT NULL,
    global_region_id integer NOT NULL
);


ALTER TABLE public.top_level_country OWNER TO postgres;

--
-- TOC entry 301 (class 1259 OID 189251)
-- Name: type_of_abuse; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.type_of_abuse (
    id integer NOT NULL
);


ALTER TABLE public.type_of_abuse OWNER TO postgres;

--
-- TOC entry 302 (class 1259 OID 189272)
-- Name: type_of_abuser; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.type_of_abuser (
    id integer NOT NULL
);


ALTER TABLE public.type_of_abuser OWNER TO postgres;

--
-- TOC entry 352 (class 1259 OID 1003372)
-- Name: united_states_congressional_meeting; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.united_states_congressional_meeting (
    id integer NOT NULL,
    date_range daterange NOT NULL,
    number integer NOT NULL
);


ALTER TABLE public.united_states_congressional_meeting OWNER TO postgres;

--
-- TOC entry 380 (class 1259 OID 3684044)
-- Name: united_states_political_party; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.united_states_political_party (
    id integer NOT NULL
);


ALTER TABLE public.united_states_political_party OWNER TO postgres;

--
-- TOC entry 377 (class 1259 OID 3683972)
-- Name: united_states_political_party_affiliation; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.united_states_political_party_affiliation (
    id integer NOT NULL,
    united_states_political_party_id integer
);


ALTER TABLE public.united_states_political_party_affiliation OWNER TO postgres;

--
-- TOC entry 211 (class 1259 OID 32769)
-- Name: user; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."user" (
    id integer NOT NULL,
    created_date_time timestamp without time zone NOT NULL,
    about_me character varying,
    animal_within character varying,
    relation_to_child_placement character varying(55) NOT NULL,
    avatar character varying(255),
    email character varying(64) NOT NULL,
    password character varying(32),
    user_status_id integer NOT NULL
);


ALTER TABLE public."user" OWNER TO postgres;

--
-- TOC entry 312 (class 1259 OID 544920)
-- Name: user_group; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.user_group (
    id integer NOT NULL,
    name character varying(100) NOT NULL,
    description character varying NOT NULL,
    administrator_role_id integer NOT NULL
);


ALTER TABLE public.user_group OWNER TO postgres;

--
-- TOC entry 315 (class 1259 OID 544978)
-- Name: user_group_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.user_group ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.user_group_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 305 (class 1259 OID 189705)
-- Name: user_group_user_role_user; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.user_group_user_role_user (
    user_role_id integer NOT NULL,
    user_id integer NOT NULL,
    user_group_id integer NOT NULL
);


ALTER TABLE public.user_group_user_role_user OWNER TO postgres;

--
-- TOC entry 304 (class 1259 OID 189694)
-- Name: user_role; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.user_role (
    id integer NOT NULL,
    user_group_id integer NOT NULL,
    name character varying(100) NOT NULL
);


ALTER TABLE public.user_role OWNER TO postgres;

--
-- TOC entry 283 (class 1259 OID 187881)
-- Name: vocabulary; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.vocabulary (
    id integer NOT NULL,
    description character varying NOT NULL,
    name character varying NOT NULL,
    owner_id integer NOT NULL
);


ALTER TABLE public.vocabulary OWNER TO postgres;

--
-- TOC entry 269 (class 1259 OID 69135)
-- Name: wrongful_medication_case; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.wrongful_medication_case (
    id integer NOT NULL
);


ALTER TABLE public.wrongful_medication_case OWNER TO postgres;

--
-- TOC entry 270 (class 1259 OID 69146)
-- Name: wrongful_removal_case; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.wrongful_removal_case (
    id integer NOT NULL
);


ALTER TABLE public.wrongful_removal_case OWNER TO postgres;

--
-- TOC entry 4281 (class 2606 OID 67714)
-- Name: documentable Documentable_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.documentable
    ADD CONSTRAINT "Documentable_pkey" PRIMARY KEY (id);


--
-- TOC entry 4296 (class 2606 OID 69118)
-- Name: abuse_case abuse_case_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.abuse_case
    ADD CONSTRAINT abuse_case_pkey PRIMARY KEY (id);


--
-- TOC entry 4416 (class 2606 OID 189685)
-- Name: access_role access_role_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.access_role
    ADD CONSTRAINT access_role_pkey PRIMARY KEY (id);


--
-- TOC entry 4430 (class 2606 OID 189729)
-- Name: access_role_privilege access_role_privilege_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.access_role_privilege
    ADD CONSTRAINT access_role_privilege_pkey PRIMARY KEY (access_role_id, action_id);


--
-- TOC entry 4338 (class 2606 OID 187861)
-- Name: act act_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.act
    ADD CONSTRAINT act_pkey PRIMARY KEY (id);


--
-- TOC entry 4549 (class 2606 OID 717674)
-- Name: action_menu_item action_menu_item_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.action_menu_item
    ADD CONSTRAINT action_menu_item_pkey PRIMARY KEY (id);


--
-- TOC entry 4545 (class 2606 OID 660713)
-- Name: action action_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.action
    ADD CONSTRAINT action_pkey PRIMARY KEY (id);


--
-- TOC entry 4601 (class 2606 OID 1855264)
-- Name: administrator_role administrator_role_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.administrator_role
    ADD CONSTRAINT administrator_role_pkey PRIMARY KEY (id);


--
-- TOC entry 4378 (class 2606 OID 189118)
-- Name: adoption_lawyer adoption_lawyer_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.adoption_lawyer
    ADD CONSTRAINT adoption_lawyer_pkey PRIMARY KEY (id);


--
-- TOC entry 4159 (class 2606 OID 33033)
-- Name: inter_organizational_relation affiliation_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.inter_organizational_relation
    ADD CONSTRAINT affiliation_pkey PRIMARY KEY (id);


--
-- TOC entry 4332 (class 2606 OID 160192)
-- Name: article article_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.article
    ADD CONSTRAINT article_pkey PRIMARY KEY (id);


--
-- TOC entry 4283 (class 2606 OID 68339)
-- Name: attachment_therapist attachment_therapist_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.attachment_therapist
    ADD CONSTRAINT attachment_therapist_pkey PRIMARY KEY (id);


--
-- TOC entry 4531 (class 2606 OID 660661)
-- Name: basic_action basic_action_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.basic_action
    ADD CONSTRAINT basic_action_pkey PRIMARY KEY (id);


--
-- TOC entry 4234 (class 2606 OID 48039)
-- Name: basic_country basic_country_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.basic_country
    ADD CONSTRAINT basic_country_pkey PRIMARY KEY (id);


--
-- TOC entry 4262 (class 2606 OID 48197)
-- Name: basic_first_and_second_level_subdivision basic_first_and_second_level_subdivision_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.basic_first_and_second_level_subdivision
    ADD CONSTRAINT basic_first_and_second_level_subdivision_pkey PRIMARY KEY (id);


--
-- TOC entry 4108 (class 2606 OID 32829)
-- Name: basic_nameable basic_nameable_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.basic_nameable
    ADD CONSTRAINT basic_nameable_pkey PRIMARY KEY (id);


--
-- TOC entry 4241 (class 2606 OID 48108)
-- Name: basic_second_level_subdivision basic_second_level_subdivision_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.basic_second_level_subdivision
    ADD CONSTRAINT basic_second_level_subdivision_pkey PRIMARY KEY (id);


--
-- TOC entry 4504 (class 2606 OID 575886)
-- Name: bill_action_type bill_action_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.bill_action_type
    ADD CONSTRAINT bill_action_pkey PRIMARY KEY (id);


--
-- TOC entry 4524 (class 2606 OID 636051)
-- Name: bill bill_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.bill
    ADD CONSTRAINT bill_pkey PRIMARY KEY (id);


--
-- TOC entry 4225 (class 2606 OID 47824)
-- Name: binding_country binding_country_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.binding_country
    ADD CONSTRAINT binding_country_pkey PRIMARY KEY (id);


--
-- TOC entry 4329 (class 2606 OID 160179)
-- Name: blog_post blog_post_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.blog_post
    ADD CONSTRAINT blog_post_pkey PRIMARY KEY (id);


--
-- TOC entry 4271 (class 2606 OID 56898)
-- Name: bottom_level_subdivision bottom_level_subdivision_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.bottom_level_subdivision
    ADD CONSTRAINT bottom_level_subdivision_pkey PRIMARY KEY (id);


--
-- TOC entry 4192 (class 2606 OID 35156)
-- Name: bound_country bound_country_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.bound_country
    ADD CONSTRAINT bound_country_pkey PRIMARY KEY (id);


--
-- TOC entry 4627 (class 2606 OID 2015922)
-- Name: case_case_parties case_case_parties_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.case_case_parties
    ADD CONSTRAINT case_case_parties_pkey PRIMARY KEY (case_id, case_parties_id);


--
-- TOC entry 4616 (class 2606 OID 2015374)
-- Name: case_parties_organization case_parties_organization_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.case_parties_organization
    ADD CONSTRAINT case_parties_organization_pkey PRIMARY KEY (case_parties_id, organization_id);


--
-- TOC entry 4620 (class 2606 OID 2015379)
-- Name: case_parties_person case_parties_person_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.case_parties_person
    ADD CONSTRAINT case_parties_person_pkey PRIMARY KEY (case_parties_id, person_id);


--
-- TOC entry 4614 (class 2606 OID 2015368)
-- Name: case_parties case_parties_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.case_parties
    ADD CONSTRAINT case_parties_pkey PRIMARY KEY (id);


--
-- TOC entry 4289 (class 2606 OID 69102)
-- Name: case case_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."case"
    ADD CONSTRAINT case_pkey PRIMARY KEY (id);


--
-- TOC entry 4624 (class 2606 OID 2015915)
-- Name: case_party_type case_relation_type_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.case_party_type
    ADD CONSTRAINT case_relation_type_pkey PRIMARY KEY (id);


--
-- TOC entry 4635 (class 2606 OID 2015956)
-- Name: case_type_case_party_type case_type_case_party_type_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.case_type_case_party_type
    ADD CONSTRAINT case_type_case_party_type_pkey PRIMARY KEY (case_type_id, case_party_type_id);


--
-- TOC entry 4632 (class 2606 OID 2015945)
-- Name: case_type case_type_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.case_type
    ADD CONSTRAINT case_type_pkey PRIMARY KEY (id);


--
-- TOC entry 4084 (class 2606 OID 1875519)
-- Name: system_group check_system_group_id_equals_0; Type: CHECK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE public.system_group
    ADD CONSTRAINT check_system_group_id_equals_0 CHECK ((id = 0)) NOT VALID;


--
-- TOC entry 4318 (class 2606 OID 116013)
-- Name: child_placement_type child_placement_type_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.child_placement_type
    ADD CONSTRAINT child_placement_type_pkey PRIMARY KEY (id);


--
-- TOC entry 4301 (class 2606 OID 69129)
-- Name: child_trafficking_case child_trafficking_case_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.child_trafficking_case
    ADD CONSTRAINT child_trafficking_case_pkey PRIMARY KEY (id);


--
-- TOC entry 4083 (class 2606 OID 116008)
-- Name: case chk_case; Type: CHECK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE public."case"
    ADD CONSTRAINT chk_case CHECK ((NOT ((date IS NOT NULL) AND (date_range IS NOT NULL)))) NOT VALID;


--
-- TOC entry 4310 (class 2606 OID 69161)
-- Name: coerced_adoption_case coerced_adoption_case_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.coerced_adoption_case
    ADD CONSTRAINT coerced_adoption_case_pkey PRIMARY KEY (id);


--
-- TOC entry 4476 (class 2606 OID 545090)
-- Name: collective collective_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.collective
    ADD CONSTRAINT collective_pkey PRIMARY KEY (id);


--
-- TOC entry 4479 (class 2606 OID 545101)
-- Name: collective_user collective_user_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.collective_user
    ADD CONSTRAINT collective_user_pkey PRIMARY KEY (collective_id, user_id);


--
-- TOC entry 4101 (class 2606 OID 32799)
-- Name: comment comment_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.comment
    ADD CONSTRAINT comment_pkey PRIMARY KEY (id);


--
-- TOC entry 4694 (class 2606 OID 3684065)
-- Name: congressional_term congressional_term_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.congressional_term
    ADD CONSTRAINT congressional_term_pkey PRIMARY KEY (id);


--
-- TOC entry 4697 (class 2606 OID 3684078)
-- Name: congressional_term_political_party_affiliation congressional_term_political_party_affiliation_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.congressional_term_political_party_affiliation
    ADD CONSTRAINT congressional_term_political_party_affiliation_pkey PRIMARY KEY (id);


--
-- TOC entry 4485 (class 2606 OID 545511)
-- Name: content_sharing_group content_sharing_group_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.content_sharing_group
    ADD CONSTRAINT content_sharing_group_pkey PRIMARY KEY (id);


--
-- TOC entry 4258 (class 2606 OID 48180)
-- Name: country_and_first_and_second_level_subdivision count_and_first_and_second_level_subdivision_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.country_and_first_and_second_level_subdivision
    ADD CONSTRAINT count_and_first_and_second_level_subdivision_pkey PRIMARY KEY (id);


--
-- TOC entry 4265 (class 2606 OID 48208)
-- Name: country_and_first_and_bottom_level_subdivision country_and_first_and_bottom_level_subdivision_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.country_and_first_and_bottom_level_subdivision
    ADD CONSTRAINT country_and_first_and_bottom_level_subdivision_pkey PRIMARY KEY (id);


--
-- TOC entry 4244 (class 2606 OID 48124)
-- Name: country_and_first_level_subdivision country_and_first_level_subdivision_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.country_and_first_level_subdivision
    ADD CONSTRAINT country_and_first_level_subdivision_pkey PRIMARY KEY (id);


--
-- TOC entry 4278 (class 2606 OID 58176)
-- Name: country_and_intermediate_level_subdivision country_and_intermediate_level_subdivision_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.country_and_intermediate_level_subdivision
    ADD CONSTRAINT country_and_intermediate_level_subdivision_pkey PRIMARY KEY (id);


--
-- TOC entry 4117 (class 2606 OID 32862)
-- Name: country country_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.country
    ADD CONSTRAINT country_pkey PRIMARY KEY (id);


--
-- TOC entry 4577 (class 2606 OID 878716)
-- Name: country_report country_report_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.country_report
    ADD CONSTRAINT country_report_pkey PRIMARY KEY (country_id, date_range);


--
-- TOC entry 4207 (class 2606 OID 35776)
-- Name: iso_coded_subdivision country_subdivision_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.iso_coded_subdivision
    ADD CONSTRAINT country_subdivision_pkey PRIMARY KEY (id);


--
-- TOC entry 4587 (class 2606 OID 960321)
-- Name: country_subdivision_type country_subdivision_type_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.country_subdivision_type
    ADD CONSTRAINT country_subdivision_type_pkey PRIMARY KEY (country_id, subdivision_type_id);


--
-- TOC entry 4527 (class 2606 OID 660640)
-- Name: create_node_action create_node_action_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.create_node_action
    ADD CONSTRAINT create_node_action_pkey PRIMARY KEY (id);


--
-- TOC entry 4537 (class 2606 OID 660679)
-- Name: delete_node_action delete_node_action_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.delete_node_action
    ADD CONSTRAINT delete_node_action_pkey PRIMARY KEY (id);


--
-- TOC entry 4136 (class 2606 OID 32954)
-- Name: denomination denomination_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.denomination
    ADD CONSTRAINT denomination_pkey PRIMARY KEY (id);


--
-- TOC entry 4324 (class 2606 OID 144386)
-- Name: deportation_case deportation_case_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.deportation_case
    ADD CONSTRAINT deportation_case_pkey PRIMARY KEY (id);


--
-- TOC entry 4335 (class 2606 OID 160205)
-- Name: discussion discussion_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.discussion
    ADD CONSTRAINT discussion_pkey PRIMARY KEY (id);


--
-- TOC entry 4315 (class 2606 OID 69182)
-- Name: disrupted_placement_case disrupted_placement_case_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.disrupted_placement_case
    ADD CONSTRAINT disrupted_placement_case_pkey PRIMARY KEY (id);


--
-- TOC entry 4169 (class 2606 OID 33044)
-- Name: document document_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.document
    ADD CONSTRAINT document_pkey PRIMARY KEY (id);


--
-- TOC entry 4142 (class 2606 OID 32976)
-- Name: document_type document_type_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.document_type
    ADD CONSTRAINT document_type_pkey PRIMARY KEY (id);


--
-- TOC entry 4561 (class 2606 OID 787801)
-- Name: documentable_document documentable_document_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.documentable_document
    ADD CONSTRAINT documentable_document_pkey PRIMARY KEY (documentable_id, document_id);


--
-- TOC entry 4541 (class 2606 OID 660696)
-- Name: edit_node_action edit_node_action_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.edit_node_action
    ADD CONSTRAINT edit_node_action_pkey PRIMARY KEY (id);


--
-- TOC entry 4395 (class 2606 OID 189200)
-- Name: facilitator facilitator_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.facilitator
    ADD CONSTRAINT facilitator_pkey PRIMARY KEY (id);


--
-- TOC entry 4321 (class 2606 OID 116024)
-- Name: family_size family_size_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.family_size
    ADD CONSTRAINT family_size_pkey PRIMARY KEY (id);


--
-- TOC entry 4312 (class 2606 OID 69171)
-- Name: fathers_rights_violation_case fathers_rights_violations_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.fathers_rights_violation_case
    ADD CONSTRAINT fathers_rights_violations_pkey PRIMARY KEY (id);


--
-- TOC entry 4099 (class 2606 OID 32792)
-- Name: file file_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.file
    ADD CONSTRAINT file_pkey PRIMARY KEY (id);


--
-- TOC entry 4274 (class 2606 OID 56930)
-- Name: first_and_bottom_level_subdivision first_and_bottom_level_subdivision_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.first_and_bottom_level_subdivision
    ADD CONSTRAINT first_and_bottom_level_subdivision_pkey PRIMARY KEY (id);


--
-- TOC entry 4248 (class 2606 OID 48141)
-- Name: first_and_second_level_subdivision first_and_second_level_subdivision_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.first_and_second_level_subdivision
    ADD CONSTRAINT first_and_second_level_subdivision_pkey PRIMARY KEY (id);


--
-- TOC entry 4199 (class 2606 OID 35184)
-- Name: first_level_global_region first_level_global_region_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.first_level_global_region
    ADD CONSTRAINT first_level_global_region_pkey PRIMARY KEY (id);


--
-- TOC entry 4203 (class 2606 OID 35771)
-- Name: first_level_subdivision first_level_subdivision_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.first_level_subdivision
    ADD CONSTRAINT first_level_subdivision_pkey PRIMARY KEY (id);


--
-- TOC entry 4253 (class 2606 OID 48158)
-- Name: formal_intermediate_level_subdivision formal_intermediate_level_subdivision_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.formal_intermediate_level_subdivision
    ADD CONSTRAINT formal_intermediate_level_subdivision_pkey PRIMARY KEY (id);


--
-- TOC entry 4229 (class 2606 OID 47996)
-- Name: geographical_entity geographical_entity_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.geographical_entity
    ADD CONSTRAINT geographical_entity_pkey PRIMARY KEY (id);


--
-- TOC entry 4232 (class 2606 OID 48011)
-- Name: global_region global_region_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.global_region
    ADD CONSTRAINT global_region_pkey PRIMARY KEY (id);


--
-- TOC entry 4140 (class 2606 OID 32965)
-- Name: hague_status hague_status_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.hague_status
    ADD CONSTRAINT hague_status_pkey PRIMARY KEY (id);


--
-- TOC entry 4402 (class 2606 OID 189222)
-- Name: home_study_agency home_study_agency_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.home_study_agency
    ADD CONSTRAINT home_study_agency_pkey PRIMARY KEY (id);


--
-- TOC entry 4489 (class 2606 OID 575823)
-- Name: house_bill house_bill_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.house_bill
    ADD CONSTRAINT house_bill_pkey PRIMARY KEY (id);


--
-- TOC entry 4689 (class 2606 OID 3684019)
-- Name: house_term house_term_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.house_term
    ADD CONSTRAINT house_term_pkey PRIMARY KEY (id);


--
-- TOC entry 4292 (class 2606 OID 69709)
-- Name: location_locatable idx_locatable_location; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.location_locatable
    ADD CONSTRAINT idx_locatable_location UNIQUE (locatable_id, location_id);


--
-- TOC entry 4256 (class 2606 OID 48169)
-- Name: informal_intermediate_level_subdivision informal_intermediate_level_subdivision_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.informal_intermediate_level_subdivision
    ADD CONSTRAINT informal_intermediate_level_subdivision_pkey PRIMARY KEY (id);


--
-- TOC entry 4405 (class 2606 OID 189233)
-- Name: institution institution_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.institution
    ADD CONSTRAINT institution_pkey PRIMARY KEY (id);


--
-- TOC entry 4573 (class 2606 OID 860247)
-- Name: inter_country_relation inter_country_relation_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.inter_country_relation
    ADD CONSTRAINT inter_country_relation_pkey PRIMARY KEY (id);


--
-- TOC entry 4566 (class 2606 OID 860234)
-- Name: inter_country_relation_type inter_country_relation_type_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.inter_country_relation_type
    ADD CONSTRAINT inter_country_relation_type_pkey PRIMARY KEY (id);


--
-- TOC entry 4115 (class 2606 OID 32851)
-- Name: inter_organizational_relation_type inter_organizational_relation_type_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.inter_organizational_relation_type
    ADD CONSTRAINT inter_organizational_relation_type_pkey PRIMARY KEY (id);


--
-- TOC entry 4179 (class 2606 OID 33073)
-- Name: inter_personal_relation inter_personal_relation_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.inter_personal_relation
    ADD CONSTRAINT inter_personal_relation_pkey PRIMARY KEY (id);


--
-- TOC entry 4131 (class 2606 OID 32932)
-- Name: inter_personal_relation_type inter_personal_relation_type_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.inter_personal_relation_type
    ADD CONSTRAINT inter_personal_relation_type_pkey PRIMARY KEY (id);


--
-- TOC entry 4269 (class 2606 OID 56893)
-- Name: intermediate_level_subdivision intermediate_level_subdivision_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.intermediate_level_subdivision
    ADD CONSTRAINT intermediate_level_subdivision_pkey PRIMARY KEY (id);


--
-- TOC entry 4239 (class 2606 OID 48091)
-- Name: iso_coded_first_level_subdivision iso_coded_first_level_subdivision_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.iso_coded_first_level_subdivision
    ADD CONSTRAINT iso_coded_first_level_subdivision_pkey PRIMARY KEY (id);


--
-- TOC entry 4399 (class 2606 OID 189211)
-- Name: law_firm law_firm_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.law_firm
    ADD CONSTRAINT law_firm_pkey PRIMARY KEY (id);


--
-- TOC entry 4599 (class 2606 OID 1797649)
-- Name: layout layout_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.layout
    ADD CONSTRAINT layout_pkey PRIMARY KEY (id);


--
-- TOC entry 4294 (class 2606 OID 69113)
-- Name: location_locatable locatable_location_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.location_locatable
    ADD CONSTRAINT locatable_location_pkey PRIMARY KEY (location_id, locatable_id);


--
-- TOC entry 4287 (class 2606 OID 69086)
-- Name: locatable locatable_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.locatable
    ADD CONSTRAINT locatable_pkey PRIMARY KEY (id);


--
-- TOC entry 4188 (class 2606 OID 33756)
-- Name: location location_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.location
    ADD CONSTRAINT location_pkey PRIMARY KEY (id);


--
-- TOC entry 4496 (class 2606 OID 575845)
-- Name: member_of_congress member_of_congress_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.member_of_congress
    ADD CONSTRAINT member_of_congress_pkey PRIMARY KEY (id);


--
-- TOC entry 4547 (class 2606 OID 717676)
-- Name: menu_item menu_item_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.menu_item
    ADD CONSTRAINT menu_item_pkey PRIMARY KEY (id);


--
-- TOC entry 4666 (class 2606 OID 2708798)
-- Name: multi_question_poll multi_question_poll_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.multi_question_poll
    ADD CONSTRAINT multi_question_poll_pkey PRIMARY KEY (id);


--
-- TOC entry 4673 (class 2606 OID 2708843)
-- Name: multi_question_poll_poll_question multi_question_poll_question_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.multi_question_poll_poll_question
    ADD CONSTRAINT multi_question_poll_question_pkey PRIMARY KEY (multi_question_poll_id, poll_question_id);


--
-- TOC entry 4343 (class 2606 OID 187880)
-- Name: nameable nameable_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.nameable
    ADD CONSTRAINT nameable_pkey PRIMARY KEY (id);


--
-- TOC entry 4701 (class 2606 OID 3684080)
-- Name: congressional_term_political_party_affiliation no_overlap_congressional_term_political_party_affiliation; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.congressional_term_political_party_affiliation
    ADD CONSTRAINT no_overlap_congressional_term_political_party_affiliation EXCLUDE USING gist (congressional_term_id WITH =, united_states_political_party_affiliation_id WITH =, date_range WITH &&);


--
-- TOC entry 4575 (class 2606 OID 860249)
-- Name: inter_country_relation no_overlap_inter_country_relation; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.inter_country_relation
    ADD CONSTRAINT no_overlap_inter_country_relation EXCLUDE USING gist (country_id_to WITH =, date_range WITH &&, inter_country_relation_type_id WITH =, country_id_from WITH =);


--
-- TOC entry 4167 (class 2606 OID 189045)
-- Name: inter_organizational_relation no_overlap_inter_organizational_relation; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.inter_organizational_relation
    ADD CONSTRAINT no_overlap_inter_organizational_relation EXCLUDE USING gist (date_range WITH &&, organization_id_from WITH =, organization_id_to WITH =, geographical_entity_id WITH =, inter_organizational_relation_type_id WITH =);


--
-- TOC entry 4388 (class 2606 OID 189179)
-- Name: organizational_role no_overlap_organizational_role; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.organizational_role
    ADD CONSTRAINT no_overlap_organizational_role EXCLUDE USING gist (daterange WITH &&, organization_id WITH =, organization_type_id WITH =);


--
-- TOC entry 4382 (class 2606 OID 189138)
-- Name: professional_role no_overlap_professional_role; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.professional_role
    ADD CONSTRAINT no_overlap_professional_role EXCLUDE USING gist (daterange WITH &&, person_id WITH =, profession_id WITH =);


--
-- TOC entry 4593 (class 2606 OID 1003392)
-- Name: united_states_congressional_meeting no_overlap_united_states_congressional_meeting; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.united_states_congressional_meeting
    ADD CONSTRAINT no_overlap_united_states_congressional_meeting EXCLUDE USING gist (date_range WITH &&);


--
-- TOC entry 4645 (class 2606 OID 2403971)
-- Name: node_file node_file_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.node_file
    ADD CONSTRAINT node_file_pkey PRIMARY KEY (node_id, file_id);


--
-- TOC entry 4096 (class 2606 OID 32777)
-- Name: node node_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.node
    ADD CONSTRAINT node_pkey PRIMARY KEY (id);


--
-- TOC entry 4371 (class 2606 OID 189052)
-- Name: publication_status node_status_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.publication_status
    ADD CONSTRAINT node_status_pkey PRIMARY KEY (id);


--
-- TOC entry 4436 (class 2606 OID 403134)
-- Name: node_term node_term_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.node_term
    ADD CONSTRAINT node_term_pkey PRIMARY KEY (node_id, term_id);


--
-- TOC entry 4106 (class 2606 OID 32818)
-- Name: node_type node_type_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.node_type
    ADD CONSTRAINT node_type_pkey PRIMARY KEY (id);


--
-- TOC entry 4522 (class 2606 OID 575948)
-- Name: organization_act_relation_type organization_act_relation_type_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.organization_act_relation_type
    ADD CONSTRAINT organization_act_relation_type_pkey PRIMARY KEY (id);


--
-- TOC entry 4583 (class 2606 OID 899652)
-- Name: organization_organization_type organization_organization_type_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.organization_organization_type
    ADD CONSTRAINT organization_organization_type_pkey PRIMARY KEY (organization_id, organization_type_id);


--
-- TOC entry 4148 (class 2606 OID 33000)
-- Name: organization organization_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.organization
    ADD CONSTRAINT organization_pkey PRIMARY KEY (id);


--
-- TOC entry 4112 (class 2606 OID 32840)
-- Name: organization_type organization_type_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.organization_type
    ADD CONSTRAINT organization_type_pkey PRIMARY KEY (id);


--
-- TOC entry 4390 (class 2606 OID 189164)
-- Name: organizational_role organizational_role_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.organizational_role
    ADD CONSTRAINT organizational_role_pkey PRIMARY KEY (id);


--
-- TOC entry 4483 (class 2606 OID 545106)
-- Name: owner owner_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.owner
    ADD CONSTRAINT owner_pkey PRIMARY KEY (id);


--
-- TOC entry 4441 (class 2606 OID 403147)
-- Name: page page_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.page
    ADD CONSTRAINT page_pkey PRIMARY KEY (id);


--
-- TOC entry 4153 (class 2606 OID 33005)
-- Name: party party_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.party
    ADD CONSTRAINT party_pkey PRIMARY KEY (id);


--
-- TOC entry 4362 (class 2606 OID 188352)
-- Name: party_political_entity_relation party_political_entity_relation_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.party_political_entity_relation
    ADD CONSTRAINT party_political_entity_relation_pkey PRIMARY KEY (id);


--
-- TOC entry 4125 (class 2606 OID 32906)
-- Name: party_political_entity_relation_type party_political_entity_relation_type_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.party_political_entity_relation_type
    ADD CONSTRAINT party_political_entity_relation_type_pkey PRIMARY KEY (id);


--
-- TOC entry 4369 (class 2606 OID 188984)
-- Name: person_organization_relation person_organization_relation_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.person_organization_relation
    ADD CONSTRAINT person_organization_relation_pkey PRIMARY KEY (id);


--
-- TOC entry 4128 (class 2606 OID 32911)
-- Name: person_organization_relation_type person_organization_relation_type_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.person_organization_relation_type
    ADD CONSTRAINT person_organization_relation_type_pkey PRIMARY KEY (id);


--
-- TOC entry 4157 (class 2606 OID 33022)
-- Name: person person_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.person
    ADD CONSTRAINT person_pkey PRIMARY KEY (id);


--
-- TOC entry 4393 (class 2606 OID 189189)
-- Name: placement_agency placement_agency_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.placement_agency
    ADD CONSTRAINT placement_agency_pkey PRIMARY KEY (id);


--
-- TOC entry 4183 (class 2606 OID 33108)
-- Name: political_entity political_entity_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.political_entity
    ADD CONSTRAINT political_entity_pkey PRIMARY KEY (id);


--
-- TOC entry 4651 (class 2606 OID 2653905)
-- Name: poll_option poll_option_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.poll_option
    ADD CONSTRAINT poll_option_pkey PRIMARY KEY (delta, poll_question_id);


--
-- TOC entry 4648 (class 2606 OID 2653883)
-- Name: poll poll_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.poll
    ADD CONSTRAINT poll_pkey PRIMARY KEY (id);


--
-- TOC entry 4669 (class 2606 OID 2708811)
-- Name: poll_question poll_question_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.poll_question
    ADD CONSTRAINT poll_question_pkey PRIMARY KEY (id);


--
-- TOC entry 4657 (class 2606 OID 2653931)
-- Name: poll_status poll_status_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.poll_status
    ADD CONSTRAINT poll_status_pkey PRIMARY KEY (id);


--
-- TOC entry 4085 (class 2606 OID 2653920)
-- Name: poll_vote poll_vote_check; Type: CHECK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE public.poll_vote
    ADD CONSTRAINT poll_vote_check CHECK ((NOT ((user_id IS NULL) AND (ip_address IS NULL)))) NOT VALID;


--
-- TOC entry 4655 (class 2606 OID 2653903)
-- Name: poll_vote poll_vote_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.poll_vote
    ADD CONSTRAINT poll_vote_pkey PRIMARY KEY (id);


--
-- TOC entry 4408 (class 2606 OID 189244)
-- Name: post_placement_agency post_placement_agency_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.post_placement_agency
    ADD CONSTRAINT post_placement_agency_pkey PRIMARY KEY (id);


--
-- TOC entry 4469 (class 2606 OID 545059)
-- Name: principal principal_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.principal
    ADD CONSTRAINT principal_pkey PRIMARY KEY (id);


--
-- TOC entry 4134 (class 2606 OID 32943)
-- Name: profession profession_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.profession
    ADD CONSTRAINT profession_pkey PRIMARY KEY (id);


--
-- TOC entry 4384 (class 2606 OID 189125)
-- Name: professional_role professional_role_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.professional_role
    ADD CONSTRAINT professional_role_pkey PRIMARY KEY (id);


--
-- TOC entry 4472 (class 2606 OID 545073)
-- Name: publisher publisher_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.publisher
    ADD CONSTRAINT publisher_pkey PRIMARY KEY (id);


--
-- TOC entry 4510 (class 2606 OID 575893)
-- Name: representative_house_bill_action representative_house_bill_action_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.representative_house_bill_action
    ADD CONSTRAINT representative_house_bill_action_pkey PRIMARY KEY (id);


--
-- TOC entry 4499 (class 2606 OID 575856)
-- Name: representative representative_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.representative
    ADD CONSTRAINT representative_pkey PRIMARY KEY (id);


--
-- TOC entry 4444 (class 2606 OID 403709)
-- Name: review review_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.review
    ADD CONSTRAINT review_pkey PRIMARY KEY (id);


--
-- TOC entry 4611 (class 2606 OID 1910333)
-- Name: searchable searchable_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.searchable
    ADD CONSTRAINT searchable_pkey PRIMARY KEY (id);


--
-- TOC entry 4123 (class 2606 OID 32879)
-- Name: second_level_global_region second_level_global_region_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.second_level_global_region
    ADD CONSTRAINT second_level_global_region_pkey PRIMARY KEY (id);


--
-- TOC entry 4197 (class 2606 OID 35167)
-- Name: second_level_subdivision second_level_subdivision_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.second_level_subdivision
    ADD CONSTRAINT second_level_subdivision_pkey PRIMARY KEY (id);


--
-- TOC entry 4493 (class 2606 OID 575834)
-- Name: senate_bill senate_bill_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.senate_bill
    ADD CONSTRAINT senate_bill_pkey PRIMARY KEY (id);


--
-- TOC entry 4684 (class 2606 OID 3683986)
-- Name: senate_term senate_term_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.senate_term
    ADD CONSTRAINT senate_term_pkey PRIMARY KEY (id);


--
-- TOC entry 4502 (class 2606 OID 575867)
-- Name: senator senator_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.senator
    ADD CONSTRAINT senator_pkey PRIMARY KEY (id);


--
-- TOC entry 4517 (class 2606 OID 575918)
-- Name: senator_senate_bill_action senator_senate_bill_action_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.senator_senate_bill_action
    ADD CONSTRAINT senator_senate_bill_action_pkey PRIMARY KEY (id);


--
-- TOC entry 4376 (class 2606 OID 189067)
-- Name: simple_text_node simple_text_node_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.simple_text_node
    ADD CONSTRAINT simple_text_node_pkey PRIMARY KEY (id);


--
-- TOC entry 4663 (class 2606 OID 2708787)
-- Name: single_question_poll single_question_poll_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.single_question_poll
    ADD CONSTRAINT single_question_poll_pkey PRIMARY KEY (id);


--
-- TOC entry 4221 (class 2606 OID 43486)
-- Name: subdivision subdivision_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.subdivision
    ADD CONSTRAINT subdivision_pkey PRIMARY KEY (id);


--
-- TOC entry 4585 (class 2606 OID 958474)
-- Name: subdivision_type subdivision_type_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.subdivision_type
    ADD CONSTRAINT subdivision_type_pkey PRIMARY KEY (id);


--
-- TOC entry 4457 (class 2606 OID 544977)
-- Name: subgroup subgroup_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.subgroup
    ADD CONSTRAINT subgroup_pkey PRIMARY KEY (id);


--
-- TOC entry 4608 (class 2606 OID 1875512)
-- Name: system_group system_group_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.system_group
    ADD CONSTRAINT system_group_pkey PRIMARY KEY (id);


--
-- TOC entry 4641 (class 2606 OID 2383876)
-- Name: tenant_file tenant_file_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tenant_file
    ADD CONSTRAINT tenant_file_pkey PRIMARY KEY (tenant_id, file_id);


--
-- TOC entry 4557 (class 2606 OID 717705)
-- Name: tenant_node_menu_item tenant_node_menu_item_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tenant_node_menu_item
    ADD CONSTRAINT tenant_node_menu_item_pkey PRIMARY KEY (id);


--
-- TOC entry 4463 (class 2606 OID 717697)
-- Name: tenant_node tenant_node_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tenant_node
    ADD CONSTRAINT tenant_node_pkey PRIMARY KEY (id);


--
-- TOC entry 4452 (class 2606 OID 544944)
-- Name: tenant tenant_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tenant
    ADD CONSTRAINT tenant_pkey PRIMARY KEY (id);


--
-- TOC entry 4351 (class 2606 OID 188224)
-- Name: term term_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.term
    ADD CONSTRAINT term_pkey PRIMARY KEY (id);


--
-- TOC entry 4214 (class 2606 OID 37403)
-- Name: top_level_country top_level_country_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.top_level_country
    ADD CONSTRAINT top_level_country_pkey PRIMARY KEY (id);


--
-- TOC entry 4411 (class 2606 OID 189255)
-- Name: type_of_abuse type_of_abuse_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.type_of_abuse
    ADD CONSTRAINT type_of_abuse_pkey PRIMARY KEY (id);


--
-- TOC entry 4414 (class 2606 OID 189276)
-- Name: type_of_abuser type_of_abuser_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.type_of_abuser
    ADD CONSTRAINT type_of_abuser_pkey PRIMARY KEY (id);


--
-- TOC entry 4535 (class 2606 OID 717748)
-- Name: basic_action unique_action_access_privilege_action; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.basic_action
    ADD CONSTRAINT unique_action_access_privilege_action UNIQUE (path) INCLUDE (id);


--
-- TOC entry 4553 (class 2606 OID 717738)
-- Name: action_menu_item unique_action_menu_item_name; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.action_menu_item
    ADD CONSTRAINT unique_action_menu_item_name UNIQUE (name) INCLUDE (action_id, id);


--
-- TOC entry 4605 (class 2606 OID 1855272)
-- Name: administrator_role unique_administrator_role_user_group; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.administrator_role
    ADD CONSTRAINT unique_administrator_role_user_group UNIQUE (user_group_id) INCLUDE (id);


--
-- TOC entry 4595 (class 2606 OID 3523722)
-- Name: united_states_congressional_meeting unique_congressional_meeting_number; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.united_states_congressional_meeting
    ADD CONSTRAINT unique_congressional_meeting_number UNIQUE (number) INCLUDE (id, date_range);


--
-- TOC entry 4210 (class 2606 OID 717752)
-- Name: iso_coded_subdivision unique_iso_3166_2_code; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.iso_coded_subdivision
    ADD CONSTRAINT unique_iso_3166_2_code UNIQUE (iso_3166_2_code) INCLUDE (id);


--
-- TOC entry 4216 (class 2606 OID 717766)
-- Name: top_level_country unique_iso_3166_code; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.top_level_country
    ADD CONSTRAINT unique_iso_3166_code UNIQUE (iso_3166_1_code) INCLUDE (id, global_region_id);


--
-- TOC entry 4675 (class 2606 OID 2708845)
-- Name: multi_question_poll_poll_question unique_multi_question_poll_question_multi_question_poll_delta; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.multi_question_poll_poll_question
    ADD CONSTRAINT unique_multi_question_poll_question_multi_question_poll_delta UNIQUE (multi_question_poll_id, delta) INCLUDE (poll_question_id);


--
-- TOC entry 4373 (class 2606 OID 189060)
-- Name: publication_status unique_node_status_name; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.publication_status
    ADD CONSTRAINT unique_node_status_name UNIQUE (name);


--
-- TOC entry 4438 (class 2606 OID 403136)
-- Name: node_term unique_node_term_term_id_node_id; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.node_term
    ADD CONSTRAINT unique_node_term_term_id_node_id UNIQUE (term_id, node_id);


--
-- TOC entry 4659 (class 2606 OID 2653933)
-- Name: poll_status unique_poll_status_name; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.poll_status
    ADD CONSTRAINT unique_poll_status_name UNIQUE (name) INCLUDE (id);


--
-- TOC entry 4474 (class 2606 OID 1768617)
-- Name: publisher unique_publisher_name; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.publisher
    ADD CONSTRAINT unique_publisher_name UNIQUE (name) INCLUDE (id);


--
-- TOC entry 4512 (class 2606 OID 575895)
-- Name: representative_house_bill_action unique_representative_house_bill_bill_action_type; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.representative_house_bill_action
    ADD CONSTRAINT unique_representative_house_bill_bill_action_type UNIQUE (representative_id, house_bill_id, bill_action_type_id);


--
-- TOC entry 4519 (class 2606 OID 575920)
-- Name: senator_senate_bill_action unique_senator_senate_bill_bill_action_type; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.senator_senate_bill_action
    ADD CONSTRAINT unique_senator_senate_bill_bill_action_type UNIQUE (senator_id, senate_bill_id, bill_action_type_id);


--
-- TOC entry 4223 (class 2606 OID 717750)
-- Name: subdivision unique_subdivision_name; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.subdivision
    ADD CONSTRAINT unique_subdivision_name UNIQUE (country_id, name) INCLUDE (id);


--
-- TOC entry 4454 (class 2606 OID 546321)
-- Name: tenant unique_tenant_domain_name; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tenant
    ADD CONSTRAINT unique_tenant_domain_name UNIQUE (domain_name);


--
-- TOC entry 4465 (class 2606 OID 717740)
-- Name: tenant_node unique_tenant_id_url_id; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tenant_node
    ADD CONSTRAINT unique_tenant_id_url_id UNIQUE (tenant_id, url_id) INCLUDE (node_id, id, publication_status_id, subgroup_id, url_path);


--
-- TOC entry 4467 (class 2606 OID 717742)
-- Name: tenant_node unique_tenant_id_url_path; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tenant_node
    ADD CONSTRAINT unique_tenant_id_url_path UNIQUE (tenant_id, url_path) INCLUDE (id, url_id, node_id, subgroup_id, publication_status_id);


--
-- TOC entry 4559 (class 2606 OID 717734)
-- Name: tenant_node_menu_item unique_tenant_node_menu_item_tenant_node_name; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tenant_node_menu_item
    ADD CONSTRAINT unique_tenant_node_menu_item_tenant_node_name UNIQUE (tenant_node_id, name) INCLUDE (id);


--
-- TOC entry 4353 (class 2606 OID 717744)
-- Name: term unique_term_vocabulary_name; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.term
    ADD CONSTRAINT unique_term_vocabulary_name UNIQUE (vocabulary_id, name) INCLUDE (id, nameable_id);


--
-- TOC entry 4355 (class 2606 OID 717746)
-- Name: term unique_term_vocabulary_nameable; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.term
    ADD CONSTRAINT unique_term_vocabulary_nameable UNIQUE (vocabulary_id, nameable_id) INCLUDE (id, name);


--
-- TOC entry 4089 (class 2606 OID 34357)
-- Name: user unique_user_email; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."user"
    ADD CONSTRAINT unique_user_email UNIQUE (email);


--
-- TOC entry 4421 (class 2606 OID 1768621)
-- Name: user_role unique_user_role_user_group_name; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.user_role
    ADD CONSTRAINT unique_user_role_user_group_name UNIQUE (user_group_id, name) INCLUDE (id);


--
-- TOC entry 4346 (class 2606 OID 717754)
-- Name: vocabulary unique_vocabulary_name_per_owner; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.vocabulary
    ADD CONSTRAINT unique_vocabulary_name_per_owner UNIQUE (name, owner_id) INCLUDE (id);


--
-- TOC entry 4597 (class 2606 OID 1003378)
-- Name: united_states_congressional_meeting united_states_congressional_meeting_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.united_states_congressional_meeting
    ADD CONSTRAINT united_states_congressional_meeting_pkey PRIMARY KEY (id);


--
-- TOC entry 4679 (class 2606 OID 3683978)
-- Name: united_states_political_party_affiliation united_states_political_party_affiliation_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.united_states_political_party_affiliation
    ADD CONSTRAINT united_states_political_party_affiliation_pkey PRIMARY KEY (id);


--
-- TOC entry 4692 (class 2606 OID 3684048)
-- Name: united_states_political_party united_states_political_party_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.united_states_political_party
    ADD CONSTRAINT united_states_political_party_pkey PRIMARY KEY (id);


--
-- TOC entry 4446 (class 2606 OID 544926)
-- Name: user_group user_group_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.user_group
    ADD CONSTRAINT user_group_pkey PRIMARY KEY (id);


--
-- TOC entry 4428 (class 2606 OID 545519)
-- Name: user_group_user_role_user user_group_user_role_user_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.user_group_user_role_user
    ADD CONSTRAINT user_group_user_role_user_pkey PRIMARY KEY (user_group_id, user_role_id, user_id);


--
-- TOC entry 4091 (class 2606 OID 32779)
-- Name: user user_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."user"
    ADD CONSTRAINT user_pkey PRIMARY KEY (id);


--
-- TOC entry 4423 (class 2606 OID 189698)
-- Name: user_role user_role_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.user_role
    ADD CONSTRAINT user_role_pkey PRIMARY KEY (id);


--
-- TOC entry 4348 (class 2606 OID 187885)
-- Name: vocabulary vocabulary_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.vocabulary
    ADD CONSTRAINT vocabulary_pkey PRIMARY KEY (id);


--
-- TOC entry 4305 (class 2606 OID 69139)
-- Name: wrongful_medication_case wrongful_medication_case_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.wrongful_medication_case
    ADD CONSTRAINT wrongful_medication_case_pkey PRIMARY KEY (id);


--
-- TOC entry 4308 (class 2606 OID 69150)
-- Name: wrongful_removal_case wrongful_removal_case_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.wrongful_removal_case
    ADD CONSTRAINT wrongful_removal_case_pkey PRIMARY KEY (id);


--
-- TOC entry 4184 (class 1259 OID 152275)
-- Name: fki_.; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "fki_." ON public.location USING btree (country_id);


--
-- TOC entry 4550 (class 1259 OID 717688)
-- Name: fki_a; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_a ON public.action_menu_item USING btree (action_id);


--
-- TOC entry 4272 (class 1259 OID 56904)
-- Name: fki_bottom_level_subdivision; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_bottom_level_subdivision ON public.bottom_level_subdivision USING btree (id);


--
-- TOC entry 4302 (class 1259 OID 152287)
-- Name: fki_c; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_c ON public.child_trafficking_case USING btree (country_id_from);


--
-- TOC entry 4208 (class 1259 OID 35782)
-- Name: fki_country_subdivision; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_country_subdivision ON public.iso_coded_subdivision USING btree (id);


--
-- TOC entry 4194 (class 1259 OID 35179)
-- Name: fki_country_subdivision_country_id_2; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_country_subdivision_country_id_2 ON public.second_level_subdivision USING btree (id);


--
-- TOC entry 4109 (class 1259 OID 32835)
-- Name: fki_d; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_d ON public.basic_nameable USING btree (id);


--
-- TOC entry 4297 (class 1259 OID 116036)
-- Name: fki_fk_abuse_case_child_placement_type; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_abuse_case_child_placement_type ON public.abuse_case USING btree (child_placement_type_id);


--
-- TOC entry 4298 (class 1259 OID 116042)
-- Name: fki_fk_abuse_case_family_size; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_abuse_case_family_size ON public.abuse_case USING btree (id);


--
-- TOC entry 4299 (class 1259 OID 69124)
-- Name: fki_fk_abuse_case_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_abuse_case_id ON public.abuse_case USING btree (id);


--
-- TOC entry 4412 (class 1259 OID 189282)
-- Name: fki_fk_abusers_relation_to_abused_id_nameable; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_abusers_relation_to_abused_id_nameable ON public.type_of_abuser USING btree (id);


--
-- TOC entry 4417 (class 1259 OID 545068)
-- Name: fki_fk_access_role_id_principal; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_access_role_id_principal ON public.access_role USING btree (id);


--
-- TOC entry 4431 (class 1259 OID 189735)
-- Name: fki_fk_access_role_privilege_access_role; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_access_role_privilege_access_role ON public.access_role_privilege USING btree (access_role_id);


--
-- TOC entry 4432 (class 1259 OID 189741)
-- Name: fki_fk_access_role_privilege_action; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_access_role_privilege_action ON public.access_role_privilege USING btree (action_id);


--
-- TOC entry 4339 (class 1259 OID 188328)
-- Name: fki_fk_act_id_collective; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_act_id_collective ON public.act USING btree (id);


--
-- TOC entry 4532 (class 1259 OID 660669)
-- Name: fki_fk_action_access_privilege_id_access_privilege; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_action_access_privilege_id_access_privilege ON public.basic_action USING btree (id);


--
-- TOC entry 4551 (class 1259 OID 717682)
-- Name: fki_fk_action_menu_item_id_menu_item; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_action_menu_item_id_menu_item ON public.action_menu_item USING btree (id);


--
-- TOC entry 4602 (class 1259 OID 1855278)
-- Name: fki_fk_administor_role_tenant; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_administor_role_tenant ON public.administrator_role USING btree (user_group_id);


--
-- TOC entry 4603 (class 1259 OID 1855270)
-- Name: fki_fk_administrator_role_user_role; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_administrator_role_user_role ON public.administrator_role USING btree (id);


--
-- TOC entry 4379 (class 1259 OID 189136)
-- Name: fki_fk_adoption_lawyer_id_professional_role; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_adoption_lawyer_id_professional_role ON public.adoption_lawyer USING btree (id);


--
-- TOC entry 4160 (class 1259 OID 33039)
-- Name: fki_fk_affiliation_organization_from; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_affiliation_organization_from ON public.inter_organizational_relation USING btree (organization_id_from);


--
-- TOC entry 4161 (class 1259 OID 33056)
-- Name: fki_fk_affiliation_organization_to; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_affiliation_organization_to ON public.inter_organizational_relation USING btree (id);


--
-- TOC entry 4162 (class 1259 OID 33062)
-- Name: fki_fk_affiliation_proof; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_affiliation_proof ON public.inter_organizational_relation USING btree (id);


--
-- TOC entry 4333 (class 1259 OID 160198)
-- Name: fki_fk_article_node; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_article_node ON public.article USING btree (id);


--
-- TOC entry 4284 (class 1259 OID 68345)
-- Name: fki_fk_attachment_therapist_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_attachment_therapist_id ON public.attachment_therapist USING btree (id);


--
-- TOC entry 4533 (class 1259 OID 660720)
-- Name: fki_fk_basic_action_id_action; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_basic_action_id_action ON public.basic_action USING btree (id);


--
-- TOC entry 4235 (class 1259 OID 48045)
-- Name: fki_fk_basic_country_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_basic_country_id ON public.basic_country USING btree (id);


--
-- TOC entry 4263 (class 1259 OID 48203)
-- Name: fki_fk_basic_first_and_second_level_subdivision_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_basic_first_and_second_level_subdivision_id ON public.basic_first_and_second_level_subdivision USING btree (id);


--
-- TOC entry 4242 (class 1259 OID 48114)
-- Name: fki_fk_basic_secondary_subdivision_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_basic_secondary_subdivision_id ON public.basic_second_level_subdivision USING btree (id);


--
-- TOC entry 4505 (class 1259 OID 575960)
-- Name: fki_fk_bill_action_nameable; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_bill_action_nameable ON public.bill_action_type USING btree (id);


--
-- TOC entry 4525 (class 1259 OID 636062)
-- Name: fki_fk_bill_id_collective; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_bill_id_collective ON public.bill USING btree (id);


--
-- TOC entry 4447 (class 1259 OID 1745709)
-- Name: fki_fk_bla; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_bla ON public.tenant USING btree (access_role_id_not_logged_in);


--
-- TOC entry 4330 (class 1259 OID 160185)
-- Name: fki_fk_blog_post_node; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_blog_post_node ON public.blog_post USING btree (id);


--
-- TOC entry 4193 (class 1259 OID 35162)
-- Name: fki_fk_bound_country_top_level_country; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_bound_country_top_level_country ON public.bound_country USING btree (binding_country_id);


--
-- TOC entry 4226 (class 1259 OID 47830)
-- Name: fki_fk_bounding_country_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_bounding_country_id ON public.binding_country USING btree (id);


--
-- TOC entry 4628 (class 1259 OID 2015928)
-- Name: fki_fk_case_case_parties_case; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_case_case_parties_case ON public.case_case_parties USING btree (case_id);


--
-- TOC entry 4629 (class 1259 OID 2015934)
-- Name: fki_fk_case_case_parties_case_parties; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_case_case_parties_case_parties ON public.case_case_parties USING btree (case_parties_id);


--
-- TOC entry 4630 (class 1259 OID 2015940)
-- Name: fki_fk_case_case_parties_case_party_type; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_case_case_parties_case_party_type ON public.case_case_parties USING btree (case_party_type_id);


--
-- TOC entry 4290 (class 1259 OID 69108)
-- Name: fki_fk_case_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_case_id ON public."case" USING btree (id);


--
-- TOC entry 4617 (class 1259 OID 2015385)
-- Name: fki_fk_case_parties_organization_case_parties; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_case_parties_organization_case_parties ON public.case_parties_organization USING btree (case_parties_id);


--
-- TOC entry 4618 (class 1259 OID 2015391)
-- Name: fki_fk_case_parties_organization_organization; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_case_parties_organization_organization ON public.case_parties_organization USING btree (organization_id);


--
-- TOC entry 4621 (class 1259 OID 2015397)
-- Name: fki_fk_case_parties_person; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_case_parties_person ON public.case_parties_person USING btree (case_parties_id);


--
-- TOC entry 4622 (class 1259 OID 2015403)
-- Name: fki_fk_case_parties_person_person; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_case_parties_person_person ON public.case_parties_person USING btree (person_id);


--
-- TOC entry 4625 (class 1259 OID 3701207)
-- Name: fki_fk_case_party_type_id_nameable; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_case_party_type_id_nameable ON public.case_party_type USING btree (id);


--
-- TOC entry 4636 (class 1259 OID 2015968)
-- Name: fki_fk_case_type_case_party_type_case_party_type; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_case_type_case_party_type_case_party_type ON public.case_type_case_party_type USING btree (case_party_type_id);


--
-- TOC entry 4637 (class 1259 OID 2015962)
-- Name: fki_fk_case_type_case_party_type_case_type; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_case_type_case_party_type_case_type ON public.case_type_case_party_type USING btree (case_type_id);


--
-- TOC entry 4633 (class 1259 OID 2015951)
-- Name: fki_fk_case_type_id_node_type; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_case_type_id_node_type ON public.case_type USING btree (id);


--
-- TOC entry 4344 (class 1259 OID 187891)
-- Name: fki_fk_category_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_category_id ON public.vocabulary USING btree (id);


--
-- TOC entry 4319 (class 1259 OID 116019)
-- Name: fki_fk_child_placement_type_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_child_placement_type_id ON public.child_placement_type USING btree (id);


--
-- TOC entry 4477 (class 1259 OID 545096)
-- Name: fki_fk_collective_id_published; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_collective_id_published ON public.collective USING btree (id);


--
-- TOC entry 4480 (class 1259 OID 547167)
-- Name: fki_fk_collective_user_user; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_collective_user_user ON public.collective_user USING btree (user_id);


--
-- TOC entry 4102 (class 1259 OID 32805)
-- Name: fki_fk_comment_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_comment_id ON public.comment USING btree (id);


--
-- TOC entry 4103 (class 1259 OID 32811)
-- Name: fki_fk_comment_node; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_comment_node ON public.comment USING btree (node_id);


--
-- TOC entry 4104 (class 1259 OID 787793)
-- Name: fki_fk_comment_publisher; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_comment_publisher ON public.comment USING btree (publisher_id);


--
-- TOC entry 4695 (class 1259 OID 3684071)
-- Name: fki_fk_congressional_term_documentable; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_congressional_term_documentable ON public.congressional_term USING btree (id);


--
-- TOC entry 4698 (class 1259 OID 3745464)
-- Name: fki_fk_congressional_term_political_party_affiliation_id_docume; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_congressional_term_political_party_affiliation_id_docume ON public.congressional_term_political_party_affiliation USING btree (id);


--
-- TOC entry 4699 (class 1259 OID 3745470)
-- Name: fki_fk_congressional_term_political_party_affiliation_united_st; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_congressional_term_political_party_affiliation_united_st ON public.congressional_term_political_party_affiliation USING btree (united_states_political_party_affiliation_id);


--
-- TOC entry 4486 (class 1259 OID 545517)
-- Name: fki_fk_content_sharing_group_id_owner; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_content_sharing_group_id_owner ON public.content_sharing_group USING btree (id);


--
-- TOC entry 4200 (class 1259 OID 35221)
-- Name: fki_fk_continent_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_continent_id ON public.first_level_global_region USING btree (id);


--
-- TOC entry 4259 (class 1259 OID 48186)
-- Name: fki_fk_country_and_first_and_second_level_subdivision_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_country_and_first_and_second_level_subdivision_id ON public.country_and_first_and_second_level_subdivision USING btree (id);


--
-- TOC entry 4260 (class 1259 OID 48192)
-- Name: fki_fk_country_and_first_and_second_level_subdivision_id_2; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_country_and_first_and_second_level_subdivision_id_2 ON public.country_and_first_and_second_level_subdivision USING btree (id);


--
-- TOC entry 4245 (class 1259 OID 48130)
-- Name: fki_fk_country_and_first_level_subdivision_1; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_country_and_first_level_subdivision_1 ON public.country_and_first_level_subdivision USING btree (id);


--
-- TOC entry 4266 (class 1259 OID 48214)
-- Name: fki_fk_country_and_first_level_subdivision_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_country_and_first_level_subdivision_id ON public.country_and_first_and_bottom_level_subdivision USING btree (id);


--
-- TOC entry 4246 (class 1259 OID 48136)
-- Name: fki_fk_country_and_first_level_subdivision_id_2; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_country_and_first_level_subdivision_id_2 ON public.country_and_first_level_subdivision USING btree (id);


--
-- TOC entry 4279 (class 1259 OID 58182)
-- Name: fki_fk_country_and_intermediate_level_subdivision_1; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_country_and_intermediate_level_subdivision_1 ON public.country_and_intermediate_level_subdivision USING btree (id);


--
-- TOC entry 4118 (class 1259 OID 32877)
-- Name: fki_fk_country_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_country_id ON public.country USING btree (id);


--
-- TOC entry 4217 (class 1259 OID 43498)
-- Name: fki_fk_country_part_name_country; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_country_part_name_country ON public.subdivision USING btree (country_id);


--
-- TOC entry 4218 (class 1259 OID 43492)
-- Name: fki_fk_country_part_name_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_country_part_name_id ON public.subdivision USING btree (id);


--
-- TOC entry 4204 (class 1259 OID 40657)
-- Name: fki_fk_country_region_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_country_region_id ON public.first_level_subdivision USING btree (id);


--
-- TOC entry 4205 (class 1259 OID 43518)
-- Name: fki_fk_country_region_id_2; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_country_region_id_2 ON public.first_level_subdivision USING btree (id);


--
-- TOC entry 4578 (class 1259 OID 904092)
-- Name: fki_fk_country_report_country; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_country_report_country ON public.country_report USING btree (country_id);


--
-- TOC entry 4195 (class 1259 OID 35173)
-- Name: fki_fk_country_subdivision_country_id_1; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_country_subdivision_country_id_1 ON public.second_level_subdivision USING btree (id);


--
-- TOC entry 4588 (class 1259 OID 960327)
-- Name: fki_fk_country_subdivision_type; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_country_subdivision_type ON public.country_subdivision_type USING btree (country_id);


--
-- TOC entry 4589 (class 1259 OID 960333)
-- Name: fki_fk_country_subdivision_type_subdivision_type; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_country_subdivision_type_subdivision_type ON public.country_subdivision_type USING btree (subdivision_type_id);


--
-- TOC entry 4528 (class 1259 OID 660653)
-- Name: fki_fk_create_node_action_id_access_privilege; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_create_node_action_id_access_privilege ON public.create_node_action USING btree (id);


--
-- TOC entry 4529 (class 1259 OID 660647)
-- Name: fki_fk_create_node_action_node_type; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_create_node_action_node_type ON public.create_node_action USING btree (node_type_id);


--
-- TOC entry 4538 (class 1259 OID 660690)
-- Name: fki_fk_delete_node_action_id_access_privilege; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_delete_node_action_id_access_privilege ON public.delete_node_action USING btree (id);


--
-- TOC entry 4539 (class 1259 OID 660691)
-- Name: fki_fk_delete_node_action_node_type; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_delete_node_action_node_type ON public.delete_node_action USING btree (node_type_id);


--
-- TOC entry 4137 (class 1259 OID 32960)
-- Name: fki_fk_denomination_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_denomination_id ON public.denomination USING btree (id);


--
-- TOC entry 4325 (class 1259 OID 144404)
-- Name: fki_fk_deportation_case_country_id_to; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_deportation_case_country_id_to ON public.deportation_case USING btree (country_id_to);


--
-- TOC entry 4326 (class 1259 OID 144398)
-- Name: fki_fk_deportation_case_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_deportation_case_id ON public.deportation_case USING btree (id);


--
-- TOC entry 4327 (class 1259 OID 144392)
-- Name: fki_fk_deportation_case_subdivision_id_from; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_deportation_case_subdivision_id_from ON public.deportation_case USING btree (subdivision_id_from);


--
-- TOC entry 4336 (class 1259 OID 160211)
-- Name: fki_fk_discussion_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_discussion_id ON public.discussion USING btree (id);


--
-- TOC entry 4316 (class 1259 OID 69188)
-- Name: fki_fk_disrupted_placement_case_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_disrupted_placement_case_id ON public.disrupted_placement_case USING btree (id);


--
-- TOC entry 4170 (class 1259 OID 70732)
-- Name: fki_fk_document_document_type_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_document_document_type_id ON public.document USING btree (id);


--
-- TOC entry 4171 (class 1259 OID 33050)
-- Name: fki_fk_document_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_document_id ON public.document USING btree (id);


--
-- TOC entry 4143 (class 1259 OID 32982)
-- Name: fki_fk_document_type_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_document_type_id ON public.document_type USING btree (id);


--
-- TOC entry 4562 (class 1259 OID 787813)
-- Name: fki_fk_documentable_document_document; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_documentable_document_document ON public.documentable_document USING btree (document_id);


--
-- TOC entry 4563 (class 1259 OID 787807)
-- Name: fki_fk_documentable_document_documentable; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_documentable_document_documentable ON public.documentable_document USING btree (documentable_id);


--
-- TOC entry 4542 (class 1259 OID 660707)
-- Name: fki_fk_edit_node_action_id_action; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_edit_node_action_id_action ON public.edit_node_action USING btree (id);


--
-- TOC entry 4543 (class 1259 OID 660708)
-- Name: fki_fk_edit_node_action_node_type; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_edit_node_action_node_type ON public.edit_node_action USING btree (node_type_id);


--
-- TOC entry 4396 (class 1259 OID 189206)
-- Name: fki_fk_facilitator_id_organizational_role; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_facilitator_id_organizational_role ON public.facilitator USING btree (id);


--
-- TOC entry 4322 (class 1259 OID 116030)
-- Name: fki_fk_family_size_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_family_size_id ON public.family_size USING btree (id);


--
-- TOC entry 4313 (class 1259 OID 69177)
-- Name: fki_fk_fathers_rights_violations_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_fathers_rights_violations_id ON public.fathers_rights_violation_case USING btree (id);


--
-- TOC entry 4154 (class 1259 OID 67000)
-- Name: fki_fk_file_id_file_portrait; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_file_id_file_portrait ON public.person USING btree (file_id_portrait);


--
-- TOC entry 4275 (class 1259 OID 56936)
-- Name: fki_fk_first_and_bottom_level_subdivision_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_first_and_bottom_level_subdivision_id ON public.first_and_bottom_level_subdivision USING btree (id);


--
-- TOC entry 4276 (class 1259 OID 56942)
-- Name: fki_fk_first_and_bottom_level_subdivision_id_02; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_first_and_bottom_level_subdivision_id_02 ON public.first_and_bottom_level_subdivision USING btree (id);


--
-- TOC entry 4249 (class 1259 OID 48147)
-- Name: fki_fk_first_and_second_level_subdivision_id_1; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_first_and_second_level_subdivision_id_1 ON public.first_and_second_level_subdivision USING btree (id);


--
-- TOC entry 4250 (class 1259 OID 48153)
-- Name: fki_fk_first_and_second_level_subdivision_id_2; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_first_and_second_level_subdivision_id_2 ON public.first_and_second_level_subdivision USING btree (id);


--
-- TOC entry 4201 (class 1259 OID 48023)
-- Name: fki_fk_first_level_global_region_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_first_level_global_region_id ON public.first_level_global_region USING btree (id);


--
-- TOC entry 4251 (class 1259 OID 48164)
-- Name: fki_fk_formal_first_level_subdivision_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_formal_first_level_subdivision_id ON public.formal_intermediate_level_subdivision USING btree (id);


--
-- TOC entry 4180 (class 1259 OID 33114)
-- Name: fki_fk_geographical_entity_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_geographical_entity_id ON public.political_entity USING btree (id);


--
-- TOC entry 4227 (class 1259 OID 188204)
-- Name: fki_fk_geographical_entity_id_2; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_geographical_entity_id_2 ON public.geographical_entity USING btree (id);


--
-- TOC entry 4230 (class 1259 OID 48017)
-- Name: fki_fk_global_region_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_global_region_id ON public.global_region USING btree (id);


--
-- TOC entry 4138 (class 1259 OID 32971)
-- Name: fki_fk_hague_status_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_hague_status_id ON public.hague_status USING btree (id);


--
-- TOC entry 4400 (class 1259 OID 189228)
-- Name: fki_fk_home_study_agency_id_organization_role; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_home_study_agency_id_organization_role ON public.home_study_agency USING btree (id);


--
-- TOC entry 4487 (class 1259 OID 575829)
-- Name: fki_fk_house_bill_bill; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_house_bill_bill ON public.house_bill USING btree (id);


--
-- TOC entry 4685 (class 1259 OID 3684040)
-- Name: fki_fk_house_term_id_node; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_house_term_id_node ON public.house_term USING btree (id);


--
-- TOC entry 4686 (class 1259 OID 3684041)
-- Name: fki_fk_house_term_representative; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_house_term_representative ON public.house_term USING btree (representative_id);


--
-- TOC entry 4687 (class 1259 OID 3684042)
-- Name: fki_fk_house_term_subdivision; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_house_term_subdivision ON public.house_term USING btree (subdivision_id);


--
-- TOC entry 4254 (class 1259 OID 48175)
-- Name: fki_fk_informal_first_level_subdivision_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_informal_first_level_subdivision_id ON public.informal_intermediate_level_subdivision USING btree (id);


--
-- TOC entry 4403 (class 1259 OID 189239)
-- Name: fki_fk_institution_id_organizational_role; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_institution_id_organizational_role ON public.institution USING btree (id);


--
-- TOC entry 4163 (class 1259 OID 189039)
-- Name: fki_fk_inter_collective_relation_political_entity; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_inter_collective_relation_political_entity ON public.inter_organizational_relation USING btree (geographical_entity_id);


--
-- TOC entry 4567 (class 1259 OID 860255)
-- Name: fki_fk_inter_country_relation_country_from; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_inter_country_relation_country_from ON public.inter_country_relation USING btree (country_id_from);


--
-- TOC entry 4568 (class 1259 OID 860261)
-- Name: fki_fk_inter_country_relation_country_to; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_inter_country_relation_country_to ON public.inter_country_relation USING btree (country_id_to);


--
-- TOC entry 4569 (class 1259 OID 860273)
-- Name: fki_fk_inter_country_relation_document_id_proof; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_inter_country_relation_document_id_proof ON public.inter_country_relation USING btree (document_id_proof);


--
-- TOC entry 4570 (class 1259 OID 860279)
-- Name: fki_fk_inter_country_relation_id_node; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_inter_country_relation_id_node ON public.inter_country_relation USING btree (id);


--
-- TOC entry 4571 (class 1259 OID 860267)
-- Name: fki_fk_inter_country_relation_inter_country_relation_type; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_inter_country_relation_inter_country_relation_type ON public.inter_country_relation USING btree (inter_country_relation_type_id);


--
-- TOC entry 4564 (class 1259 OID 860240)
-- Name: fki_fk_inter_country_relation_type_id_nameable; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_inter_country_relation_type_id_nameable ON public.inter_country_relation_type USING btree (id);


--
-- TOC entry 4172 (class 1259 OID 860291)
-- Name: fki_fk_inter_personal_relation_id_documentable; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_inter_personal_relation_id_documentable ON public.inter_personal_relation USING btree (id);


--
-- TOC entry 4267 (class 1259 OID 56910)
-- Name: fki_fk_intermediate_level_subdivision_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_intermediate_level_subdivision_id ON public.intermediate_level_subdivision USING btree (id);


--
-- TOC entry 4236 (class 1259 OID 48097)
-- Name: fki_fk_iso_coded_first_level_subdivision_1; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_iso_coded_first_level_subdivision_1 ON public.iso_coded_first_level_subdivision USING btree (id);


--
-- TOC entry 4237 (class 1259 OID 48103)
-- Name: fki_fk_iso_coded_first_level_subdivision_2; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_iso_coded_first_level_subdivision_2 ON public.iso_coded_first_level_subdivision USING btree (id);


--
-- TOC entry 4397 (class 1259 OID 189217)
-- Name: fki_fk_law_firm_organizational_role; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_law_firm_organizational_role ON public.law_firm USING btree (id);


--
-- TOC entry 4185 (class 1259 OID 152281)
-- Name: fki_fk_location_subdivision; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_location_subdivision ON public.location USING btree (subdivision_id);


--
-- TOC entry 4186 (class 1259 OID 152269)
-- Name: fki_fk_location_subdivision_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_location_subdivision_id ON public.location USING btree (subdivision_id);


--
-- TOC entry 4494 (class 1259 OID 575851)
-- Name: fki_fk_member_of_congress_political_entity_relation; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_member_of_congress_political_entity_relation ON public.member_of_congress USING btree (id);


--
-- TOC entry 4664 (class 1259 OID 2708804)
-- Name: fki_fk_multi_question_poll_id_poll; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_multi_question_poll_id_poll ON public.multi_question_poll USING btree (id);


--
-- TOC entry 4670 (class 1259 OID 2708851)
-- Name: fki_fk_multi_question_poll_question_multi_question_poll; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_multi_question_poll_question_multi_question_poll ON public.multi_question_poll_poll_question USING btree (multi_question_poll_id);


--
-- TOC entry 4671 (class 1259 OID 2708857)
-- Name: fki_fk_multi_question_poll_question_poll_question; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_multi_question_poll_question_poll_question ON public.multi_question_poll_poll_question USING btree (poll_question_id);


--
-- TOC entry 4340 (class 1259 OID 196613)
-- Name: fki_fk_nameable_file_tile_image; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_nameable_file_tile_image ON public.nameable USING btree (file_id_tile_image);


--
-- TOC entry 4642 (class 1259 OID 2403983)
-- Name: fki_fk_node_file_file; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_node_file_file ON public.node_file USING btree (file_id);


--
-- TOC entry 4643 (class 1259 OID 2403977)
-- Name: fki_fk_node_file_node; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_node_file_node ON public.node_file USING btree (node_id);


--
-- TOC entry 4433 (class 1259 OID 611544)
-- Name: fki_fk_node_term_node; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_node_term_node ON public.node_term USING btree (node_id);


--
-- TOC entry 4434 (class 1259 OID 611550)
-- Name: fki_fk_node_term_term; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_node_term_term ON public.node_term USING btree (term_id);


--
-- TOC entry 4092 (class 1259 OID 32785)
-- Name: fki_fk_node_user; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_node_user ON public.node USING btree (publisher_id);


--
-- TOC entry 4093 (class 1259 OID 544932)
-- Name: fki_fk_node_user_group; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_node_user_group ON public.node USING btree (owner_id);


--
-- TOC entry 4520 (class 1259 OID 575954)
-- Name: fki_fk_organization_act_relation_type; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_organization_act_relation_type ON public.organization_act_relation_type USING btree (id);


--
-- TOC entry 4144 (class 1259 OID 33017)
-- Name: fki_fk_organization_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_organization_id ON public.organization USING btree (id);


--
-- TOC entry 4145 (class 1259 OID 188322)
-- Name: fki_fk_organization_id_collective; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_organization_id_collective ON public.organization USING btree (id);


--
-- TOC entry 4146 (class 1259 OID 899647)
-- Name: fki_fk_organization_organization_type; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_organization_organization_type ON public.organization USING btree (id);


--
-- TOC entry 4580 (class 1259 OID 899658)
-- Name: fki_fk_organization_organization_type_organization; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_organization_organization_type_organization ON public.organization_organization_type USING btree (organization_id);


--
-- TOC entry 4581 (class 1259 OID 899664)
-- Name: fki_fk_organization_organization_type_organization_type; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_organization_organization_type_organization_type ON public.organization_organization_type USING btree (organization_type_id);


--
-- TOC entry 4110 (class 1259 OID 32846)
-- Name: fki_fk_organization_type_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_organization_type_id ON public.organization_type USING btree (id);


--
-- TOC entry 4385 (class 1259 OID 189171)
-- Name: fki_fk_organizational_role_organization; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_organizational_role_organization ON public.organizational_role USING btree (organization_id);


--
-- TOC entry 4386 (class 1259 OID 189177)
-- Name: fki_fk_organizational_role_organization_type; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_organizational_role_organization_type ON public.organizational_role USING btree (organization_type_id);


--
-- TOC entry 4439 (class 1259 OID 403153)
-- Name: fki_fk_page_id_simple_text_node; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_page_id_simple_text_node ON public.page USING btree (id);


--
-- TOC entry 4149 (class 1259 OID 33011)
-- Name: fki_fk_party; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_party ON public.party USING btree (id);


--
-- TOC entry 4150 (class 1259 OID 188300)
-- Name: fki_fk_party_id_nameable; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_party_id_nameable ON public.party USING btree (id);


--
-- TOC entry 4151 (class 1259 OID 33762)
-- Name: fki_fk_party_location; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_party_location ON public.party USING btree (id);


--
-- TOC entry 4356 (class 1259 OID 189013)
-- Name: fki_fk_party_political_entity_relation_document_proof; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_party_political_entity_relation_document_proof ON public.party_political_entity_relation USING btree (document_id_proof);


--
-- TOC entry 4357 (class 1259 OID 189019)
-- Name: fki_fk_party_political_entity_relation_political_entity; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_party_political_entity_relation_political_entity ON public.party_political_entity_relation USING btree (political_entity_id);


--
-- TOC entry 4358 (class 1259 OID 189025)
-- Name: fki_fk_party_political_entity_relation_political_entity_relatab; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_party_political_entity_relation_political_entity_relatab ON public.party_political_entity_relation USING btree (party_id);


--
-- TOC entry 4359 (class 1259 OID 189031)
-- Name: fki_fk_party_political_entity_relation_political_entity_relatio; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_party_political_entity_relation_political_entity_relatio ON public.party_political_entity_relation USING btree (party_political_entity_relation_type_id);


--
-- TOC entry 4360 (class 1259 OID 860313)
-- Name: fki_fk_party_politicial_entity_relation_id_documentable; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_party_politicial_entity_relation_id_documentable ON public.party_political_entity_relation USING btree (id);


--
-- TOC entry 4363 (class 1259 OID 188990)
-- Name: fki_fk_person_collective_relation_person; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_person_collective_relation_person ON public.person_organization_relation USING btree (person_id);


--
-- TOC entry 4364 (class 1259 OID 189002)
-- Name: fki_fk_person_collective_relation_person_collective_relation_ty; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_person_collective_relation_person_collective_relation_ty ON public.person_organization_relation USING btree (person_organization_relation_type_id);


--
-- TOC entry 4155 (class 1259 OID 33028)
-- Name: fki_fk_person_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_person_id ON public.person USING btree (id);


--
-- TOC entry 4365 (class 1259 OID 860307)
-- Name: fki_fk_person_organization_relation_id_documentable; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_person_organization_relation_id_documentable ON public.person_organization_relation USING btree (id);


--
-- TOC entry 4173 (class 1259 OID 33079)
-- Name: fki_fk_personal_relationship_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_personal_relationship_id ON public.inter_personal_relation USING btree (id);


--
-- TOC entry 4174 (class 1259 OID 33085)
-- Name: fki_fk_personal_relationship_person_from; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_personal_relationship_person_from ON public.inter_personal_relation USING btree (person_id_from);


--
-- TOC entry 4175 (class 1259 OID 33091)
-- Name: fki_fk_personal_relationship_person_to; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_personal_relationship_person_to ON public.inter_personal_relation USING btree (person_id_to);


--
-- TOC entry 4176 (class 1259 OID 33097)
-- Name: fki_fk_personal_relationship_personal_relationship_type; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_personal_relationship_personal_relationship_type ON public.inter_personal_relation USING btree (inter_personal_relation_type_id);


--
-- TOC entry 4177 (class 1259 OID 33103)
-- Name: fki_fk_personal_relationship_proof; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_personal_relationship_proof ON public.inter_personal_relation USING btree (document_id_proof);


--
-- TOC entry 4129 (class 1259 OID 32938)
-- Name: fki_fk_personal_relationship_type_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_personal_relationship_type_id ON public.inter_personal_relation_type USING btree (id);


--
-- TOC entry 4391 (class 1259 OID 189195)
-- Name: fki_fk_placement_agency_id_organizational_role; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_placement_agency_id_organizational_role ON public.placement_agency USING btree (id);


--
-- TOC entry 4181 (class 1259 OID 66753)
-- Name: fki_fk_political_entity_file_flag; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_political_entity_file_flag ON public.political_entity USING btree (file_id_flag);


--
-- TOC entry 4649 (class 1259 OID 2653911)
-- Name: fki_fk_poll_option_pole; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_poll_option_pole ON public.poll_option USING btree (poll_question_id);


--
-- TOC entry 4646 (class 1259 OID 2653889)
-- Name: fki_fk_poll_simple_text_node; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_poll_simple_text_node ON public.poll USING btree (id);


--
-- TOC entry 4652 (class 1259 OID 2653926)
-- Name: fki_fk_poll_vote_user; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_poll_vote_user ON public.poll_vote USING btree (user_id);


--
-- TOC entry 4406 (class 1259 OID 189250)
-- Name: fki_fk_post_placement_agency_id_organizational_role; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_post_placement_agency_id_organizational_role ON public.post_placement_agency USING btree (id);


--
-- TOC entry 4132 (class 1259 OID 32949)
-- Name: fki_fk_profession_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_profession_id ON public.profession USING btree (id);


--
-- TOC entry 4380 (class 1259 OID 189149)
-- Name: fki_fk_professional_role_profession; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_professional_role_profession ON public.professional_role USING btree (profession_id);


--
-- TOC entry 4470 (class 1259 OID 545079)
-- Name: fki_fk_publisher_id_principal; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_publisher_id_principal ON public.publisher USING btree (id);


--
-- TOC entry 4120 (class 1259 OID 35227)
-- Name: fki_fk_region_continent; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_region_continent ON public.second_level_global_region USING btree (first_level_global_region_id);


--
-- TOC entry 4121 (class 1259 OID 32871)
-- Name: fki_fk_region_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_region_id ON public.second_level_global_region USING btree (id);


--
-- TOC entry 4506 (class 1259 OID 575913)
-- Name: fki_fk_representative_house_bill_bill_action_bill_action; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_representative_house_bill_bill_action_bill_action ON public.representative_house_bill_action USING btree (bill_action_type_id);


--
-- TOC entry 4507 (class 1259 OID 575907)
-- Name: fki_fk_representative_house_bill_bill_action_house_bill; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_representative_house_bill_bill_action_house_bill ON public.representative_house_bill_action USING btree (house_bill_id);


--
-- TOC entry 4508 (class 1259 OID 575901)
-- Name: fki_fk_representative_house_bill_bill_action_representative; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_representative_house_bill_bill_action_representative ON public.representative_house_bill_action USING btree (representative_id);


--
-- TOC entry 4497 (class 1259 OID 575862)
-- Name: fki_fk_representative_member_of_congress; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_representative_member_of_congress ON public.representative USING btree (id);


--
-- TOC entry 4442 (class 1259 OID 403715)
-- Name: fki_fk_review_id_simple_text_node; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_review_id_simple_text_node ON public.review USING btree (id);


--
-- TOC entry 4609 (class 1259 OID 1910340)
-- Name: fki_fk_searchable_node; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_searchable_node ON public.searchable USING btree (id);


--
-- TOC entry 4490 (class 1259 OID 575840)
-- Name: fki_fk_senate_bill; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_senate_bill ON public.senate_bill USING btree (id);


--
-- TOC entry 4491 (class 1259 OID 636073)
-- Name: fki_fk_senate_bill_id_bill; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_senate_bill_id_bill ON public.senate_bill USING btree (id);


--
-- TOC entry 4680 (class 1259 OID 3683992)
-- Name: fki_fk_senate_term_id_node; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_senate_term_id_node ON public.senate_term USING btree (id);


--
-- TOC entry 4681 (class 1259 OID 3684004)
-- Name: fki_fk_senate_term_senator; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_senate_term_senator ON public.senate_term USING btree (senator_id);


--
-- TOC entry 4682 (class 1259 OID 3684010)
-- Name: fki_fk_senate_term_subdivision; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_senate_term_subdivision ON public.senate_term USING btree (subdivision_id);


--
-- TOC entry 4500 (class 1259 OID 575873)
-- Name: fki_fk_senator_member_of_congress; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_senator_member_of_congress ON public.senator USING btree (id);


--
-- TOC entry 4513 (class 1259 OID 575936)
-- Name: fki_fk_senator_senate_bill_bill_action_bill_action; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_senator_senate_bill_bill_action_bill_action ON public.senator_senate_bill_action USING btree (bill_action_type_id);


--
-- TOC entry 4514 (class 1259 OID 575937)
-- Name: fki_fk_senator_senate_bill_bill_action_senate_bill; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_senator_senate_bill_bill_action_senate_bill ON public.senator_senate_bill_action USING btree (senate_bill_id);


--
-- TOC entry 4515 (class 1259 OID 575938)
-- Name: fki_fk_senator_senate_bill_bill_action_senator; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_senator_senate_bill_bill_action_senator ON public.senator_senate_bill_action USING btree (senator_id);


--
-- TOC entry 4374 (class 1259 OID 189073)
-- Name: fki_fk_simple_text_node_id_node; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_simple_text_node_id_node ON public.simple_text_node USING btree (id);


--
-- TOC entry 4660 (class 1259 OID 2708793)
-- Name: fki_fk_single_question_poll_id_poll; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_single_question_poll_id_poll ON public.single_question_poll USING btree (id);


--
-- TOC entry 4661 (class 1259 OID 2708817)
-- Name: fki_fk_single_question_poll_id_poll_question; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_single_question_poll_id_poll_question ON public.single_question_poll USING btree (id);


--
-- TOC entry 4219 (class 1259 OID 960339)
-- Name: fki_fk_subdivision_country_subdivision; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_subdivision_country_subdivision ON public.subdivision USING btree (country_id, subdivision_type_id);


--
-- TOC entry 4455 (class 1259 OID 544989)
-- Name: fki_fk_subgroup_tenant; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_subgroup_tenant ON public.subgroup USING btree (tenant_id);


--
-- TOC entry 4606 (class 1259 OID 1875518)
-- Name: fki_fk_system_group_user_group; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_system_group_user_group ON public.system_group USING btree (id);


--
-- TOC entry 4638 (class 1259 OID 2383888)
-- Name: fki_fk_tenant_file_file; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_tenant_file_file ON public.tenant_file USING btree (file_id);


--
-- TOC entry 4639 (class 1259 OID 2383882)
-- Name: fki_fk_tenant_file_tenant; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_tenant_file_tenant ON public.tenant_file USING btree (tenant_id);


--
-- TOC entry 4554 (class 1259 OID 717711)
-- Name: fki_fk_tenant_node_menu_item_id_menu_item; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_tenant_node_menu_item_id_menu_item ON public.tenant_node_menu_item USING btree (id);


--
-- TOC entry 4555 (class 1259 OID 717717)
-- Name: fki_fk_tenant_node_menu_item_tenant_node; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_tenant_node_menu_item_tenant_node ON public.tenant_node_menu_item USING btree (tenant_node_id);


--
-- TOC entry 4458 (class 1259 OID 545046)
-- Name: fki_fk_tenant_node_publication_status; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_tenant_node_publication_status ON public.tenant_node USING btree (publication_status_id);


--
-- TOC entry 4459 (class 1259 OID 545040)
-- Name: fki_fk_tenant_node_subgroup; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_tenant_node_subgroup ON public.tenant_node USING btree (subgroup_id);


--
-- TOC entry 4460 (class 1259 OID 545028)
-- Name: fki_fk_tenant_node_tenant; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_tenant_node_tenant ON public.tenant_node USING btree (tenant_id);


--
-- TOC entry 4448 (class 1259 OID 544950)
-- Name: fki_fk_tenant_user_group; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_tenant_user_group ON public.tenant USING btree (id);


--
-- TOC entry 4449 (class 1259 OID 1669501)
-- Name: fki_fk_tenant_user_role_id_not_logged_in; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_tenant_user_role_id_not_logged_in ON public.tenant USING btree (access_role_id_not_logged_in);


--
-- TOC entry 4450 (class 1259 OID 545054)
-- Name: fki_fk_tenant_vocabulary_tagging; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_tenant_vocabulary_tagging ON public.tenant USING btree (vocabulary_id_tagging);


--
-- TOC entry 4189 (class 1259 OID 35195)
-- Name: fki_fk_term_hierarchy_parent; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_term_hierarchy_parent ON public.term_hierarchy USING btree (term_id_parent);


--
-- TOC entry 4341 (class 1259 OID 187902)
-- Name: fki_fk_term_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_term_id ON public.nameable USING btree (id);


--
-- TOC entry 4349 (class 1259 OID 188284)
-- Name: fki_fk_term_nameable; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_term_nameable ON public.term USING btree (nameable_id);


--
-- TOC entry 4211 (class 1259 OID 37409)
-- Name: fki_fk_top_level_country_country; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_top_level_country_country ON public.top_level_country USING btree (id);


--
-- TOC entry 4212 (class 1259 OID 51562)
-- Name: fki_fk_top_level_country_global_region; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_top_level_country_global_region ON public.top_level_country USING btree (global_region_id);


--
-- TOC entry 4409 (class 1259 OID 189261)
-- Name: fki_fk_type_of_abuse_id_nameable; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_type_of_abuse_id_nameable ON public.type_of_abuse USING btree (id);


--
-- TOC entry 4590 (class 1259 OID 1003384)
-- Name: fki_fk_united_states_congressional_meetings_documentable; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_united_states_congressional_meetings_documentable ON public.united_states_congressional_meeting USING btree (id);


--
-- TOC entry 4591 (class 1259 OID 1003390)
-- Name: fki_fk_united_states_congressional_meetings_nameable; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_united_states_congressional_meetings_nameable ON public.united_states_congressional_meeting USING btree (id);


--
-- TOC entry 4676 (class 1259 OID 3684060)
-- Name: fki_fk_united_states_politcal_party_affiliation_united_states_p; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_united_states_politcal_party_affiliation_united_states_p ON public.united_states_political_party_affiliation USING btree (united_states_political_party_id);


--
-- TOC entry 4677 (class 1259 OID 3684086)
-- Name: fki_fk_united_states_political_party_affiliation_id_nameable; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_united_states_political_party_affiliation_id_nameable ON public.united_states_political_party_affiliation USING btree (id);


--
-- TOC entry 4690 (class 1259 OID 3684054)
-- Name: fki_fk_united_states_political_party_id_organization; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_united_states_political_party_id_organization ON public.united_states_political_party USING btree (id);


--
-- TOC entry 4086 (class 1259 OID 189691)
-- Name: fki_fk_user_id_access_role; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_user_id_access_role ON public."user" USING btree (id);


--
-- TOC entry 4418 (class 1259 OID 1745751)
-- Name: fki_fk_user_role_user_group; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_user_role_user_group ON public.user_role USING btree (user_group_id);


--
-- TOC entry 4424 (class 1259 OID 545014)
-- Name: fki_fk_user_role_user_user; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_user_role_user_user ON public.user_group_user_role_user USING btree (user_id);


--
-- TOC entry 4425 (class 1259 OID 545020)
-- Name: fki_fk_user_role_user_user_group; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_user_role_user_user_group ON public.user_group_user_role_user USING btree (user_group_id);


--
-- TOC entry 4426 (class 1259 OID 545008)
-- Name: fki_fk_user_role_user_user_role; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_user_role_user_user_role ON public.user_group_user_role_user USING btree (user_role_id);


--
-- TOC entry 4303 (class 1259 OID 69145)
-- Name: fki_fk_wrongful_medication_case_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_wrongful_medication_case_id ON public.wrongful_medication_case USING btree (id);


--
-- TOC entry 4306 (class 1259 OID 69156)
-- Name: fki_fk_wrongful_removal_case_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_wrongful_removal_case_id ON public.wrongful_removal_case USING btree (id);


--
-- TOC entry 4094 (class 1259 OID 32824)
-- Name: fki_g; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_g ON public.node USING btree (node_type_id);


--
-- TOC entry 4126 (class 1259 OID 32917)
-- Name: fki_h; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_h ON public.person_organization_relation_type USING btree (id);


--
-- TOC entry 4164 (class 1259 OID 860285)
-- Name: fki_inter_organizational_relation_id_node; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_inter_organizational_relation_id_node ON public.inter_organizational_relation USING btree (id);


--
-- TOC entry 4113 (class 1259 OID 32857)
-- Name: fki_j; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_j ON public.inter_organizational_relation_type USING btree (id);


--
-- TOC entry 4165 (class 1259 OID 33068)
-- Name: fki_k; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_k ON public.inter_organizational_relation USING btree (id);


--
-- TOC entry 4285 (class 1259 OID 69092)
-- Name: fki_l; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_l ON public.locatable USING btree (id);


--
-- TOC entry 4481 (class 1259 OID 545112)
-- Name: fki_o; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_o ON public.owner USING btree (id);


--
-- TOC entry 4653 (class 1259 OID 2653919)
-- Name: fki_p; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_p ON public.poll_vote USING btree (poll_id, delta);


--
-- TOC entry 4366 (class 1259 OID 188996)
-- Name: fki_person_collective_relation_collective; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_person_collective_relation_collective ON public.person_organization_relation USING btree (organization_id);


--
-- TOC entry 4367 (class 1259 OID 1254862)
-- Name: fki_person_organization_relation_political_entity; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_person_organization_relation_political_entity ON public.person_organization_relation USING btree (geographical_entity_id);


--
-- TOC entry 4461 (class 1259 OID 545034)
-- Name: fki_r; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_r ON public.tenant_node USING btree (node_id);


--
-- TOC entry 4667 (class 1259 OID 2708838)
-- Name: fki_tk_poll_question_id_simple_text_node; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_tk_poll_question_id_simple_text_node ON public.poll_question USING btree (id);


--
-- TOC entry 4087 (class 1259 OID 545085)
-- Name: fki_u; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_u ON public."user" USING btree (id);


--
-- TOC entry 4419 (class 1259 OID 189704)
-- Name: fki_user_role_access_role; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_user_role_access_role ON public.user_role USING btree (id);


--
-- TOC entry 4119 (class 1259 OID 177593)
-- Name: fki_v; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_v ON public.country USING btree (hague_status_id);


--
-- TOC entry 4579 (class 1259 OID 878717)
-- Name: idx_country_year; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_country_year ON public.country_report USING btree (country_id, date_range);


--
-- TOC entry 4190 (class 1259 OID 33770)
-- Name: idx_term_id_child; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_term_id_child ON public.term_hierarchy USING btree (term_id_child);


--
-- TOC entry 4097 (class 1259 OID 1302712)
-- Name: node_trgm_idx; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX node_trgm_idx ON public.node USING gist (title public.gist_trgm_ops);


--
-- TOC entry 4612 (class 1259 OID 1910334)
-- Name: searchable_tsvector_idx; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX searchable_tsvector_idx ON public.searchable USING gin (tsvector);


--
-- TOC entry 4798 (class 2606 OID 116031)
-- Name: abuse_case fk_abuse_case_child_placement_type; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.abuse_case
    ADD CONSTRAINT fk_abuse_case_child_placement_type FOREIGN KEY (child_placement_type_id) REFERENCES public.child_placement_type(id) NOT VALID;


--
-- TOC entry 4799 (class 2606 OID 118283)
-- Name: abuse_case fk_abuse_case_family_size; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.abuse_case
    ADD CONSTRAINT fk_abuse_case_family_size FOREIGN KEY (family_size_id) REFERENCES public.family_size(id) NOT VALID;


--
-- TOC entry 4800 (class 2606 OID 69119)
-- Name: abuse_case fk_abuse_case_id_case; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.abuse_case
    ADD CONSTRAINT fk_abuse_case_id_case FOREIGN KEY (id) REFERENCES public."case"(id) NOT VALID;


--
-- TOC entry 4854 (class 2606 OID 189730)
-- Name: access_role_privilege fk_access_role_privilege_access_role; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.access_role_privilege
    ADD CONSTRAINT fk_access_role_privilege_access_role FOREIGN KEY (access_role_id) REFERENCES public.access_role(id) NOT VALID;


--
-- TOC entry 4855 (class 2606 OID 660736)
-- Name: access_role_privilege fk_access_role_privilege_action; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.access_role_privilege
    ADD CONSTRAINT fk_access_role_privilege_action FOREIGN KEY (action_id) REFERENCES public.action(id) NOT VALID;


--
-- TOC entry 4848 (class 2606 OID 1855255)
-- Name: access_role fk_access_role_user_role; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.access_role
    ADD CONSTRAINT fk_access_role_user_role FOREIGN KEY (id) REFERENCES public.user_role(id) NOT VALID;


--
-- TOC entry 4816 (class 2606 OID 189089)
-- Name: act fk_act_id_documentable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.act
    ADD CONSTRAINT fk_act_id_documentable FOREIGN KEY (id) REFERENCES public.documentable(id) NOT VALID;


--
-- TOC entry 4817 (class 2606 OID 189094)
-- Name: act fk_act_id_nameable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.act
    ADD CONSTRAINT fk_act_id_nameable FOREIGN KEY (id) REFERENCES public.nameable(id) NOT VALID;


--
-- TOC entry 4898 (class 2606 OID 717683)
-- Name: action_menu_item fk_action_menu_item_action; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.action_menu_item
    ADD CONSTRAINT fk_action_menu_item_action FOREIGN KEY (action_id) REFERENCES public.action(id) NOT VALID;


--
-- TOC entry 4899 (class 2606 OID 717677)
-- Name: action_menu_item fk_action_menu_item_id_menu_item; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.action_menu_item
    ADD CONSTRAINT fk_action_menu_item_id_menu_item FOREIGN KEY (id) REFERENCES public.menu_item(id) NOT VALID;


--
-- TOC entry 4918 (class 2606 OID 1875503)
-- Name: administrator_role fk_administor_role_user_group; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.administrator_role
    ADD CONSTRAINT fk_administor_role_user_group FOREIGN KEY (user_group_id) REFERENCES public.user_group(id) NOT VALID;


--
-- TOC entry 4919 (class 2606 OID 1855265)
-- Name: administrator_role fk_administrator_role_user_role; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.administrator_role
    ADD CONSTRAINT fk_administrator_role_user_role FOREIGN KEY (id) REFERENCES public.user_role(id) NOT VALID;


--
-- TOC entry 4835 (class 2606 OID 189131)
-- Name: adoption_lawyer fk_adoption_lawyer_id_professional_role; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.adoption_lawyer
    ADD CONSTRAINT fk_adoption_lawyer_id_professional_role FOREIGN KEY (id) REFERENCES public.professional_role(id) NOT VALID;


--
-- TOC entry 4814 (class 2606 OID 189084)
-- Name: article fk_article_node_id_simple_text_node; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.article
    ADD CONSTRAINT fk_article_node_id_simple_text_node FOREIGN KEY (id) REFERENCES public.simple_text_node(id) NOT VALID;


--
-- TOC entry 4791 (class 2606 OID 189126)
-- Name: attachment_therapist fk_attachment_therapist_id_professional_role; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.attachment_therapist
    ADD CONSTRAINT fk_attachment_therapist_id_professional_role FOREIGN KEY (id) REFERENCES public.professional_role(id) NOT VALID;


--
-- TOC entry 4893 (class 2606 OID 660715)
-- Name: basic_action fk_basic_action_id_action; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.basic_action
    ADD CONSTRAINT fk_basic_action_id_action FOREIGN KEY (id) REFERENCES public.action(id) NOT VALID;


--
-- TOC entry 4767 (class 2606 OID 717760)
-- Name: basic_country fk_basic_country_id_top_level_country; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.basic_country
    ADD CONSTRAINT fk_basic_country_id_top_level_country FOREIGN KEY (id) REFERENCES public.top_level_country(id) NOT VALID;


--
-- TOC entry 4781 (class 2606 OID 48198)
-- Name: basic_first_and_second_level_subdivision fk_basic_first_and_second_level_subdivision_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.basic_first_and_second_level_subdivision
    ADD CONSTRAINT fk_basic_first_and_second_level_subdivision_id FOREIGN KEY (id) REFERENCES public.first_and_second_level_subdivision(id) NOT VALID;


--
-- TOC entry 4709 (class 2606 OID 189313)
-- Name: basic_nameable fk_basic_nameable_id_nameable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.basic_nameable
    ADD CONSTRAINT fk_basic_nameable_id_nameable FOREIGN KEY (id) REFERENCES public.nameable(id) ON UPDATE RESTRICT ON DELETE RESTRICT NOT VALID;


--
-- TOC entry 4770 (class 2606 OID 48109)
-- Name: basic_second_level_subdivision fk_basic_second_level_subdivision_id_1; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.basic_second_level_subdivision
    ADD CONSTRAINT fk_basic_second_level_subdivision_id_1 FOREIGN KEY (id) REFERENCES public.second_level_subdivision(id) NOT VALID;


--
-- TOC entry 4771 (class 2606 OID 56948)
-- Name: basic_second_level_subdivision fk_basic_second_level_subdivision_intermediate_level_subdivisio; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.basic_second_level_subdivision
    ADD CONSTRAINT fk_basic_second_level_subdivision_intermediate_level_subdivisio FOREIGN KEY (intermediate_level_subdivision_id) REFERENCES public.intermediate_level_subdivision(id) NOT VALID;


--
-- TOC entry 4881 (class 2606 OID 575955)
-- Name: bill_action_type fk_bill_action_nameable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.bill_action_type
    ADD CONSTRAINT fk_bill_action_nameable FOREIGN KEY (id) REFERENCES public.nameable(id) NOT VALID;


--
-- TOC entry 4889 (class 2606 OID 636052)
-- Name: bill fk_bill_id_documentable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.bill
    ADD CONSTRAINT fk_bill_id_documentable FOREIGN KEY (id) REFERENCES public.documentable(id);


--
-- TOC entry 4890 (class 2606 OID 636057)
-- Name: bill fk_bill_id_nameable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.bill
    ADD CONSTRAINT fk_bill_id_nameable FOREIGN KEY (id) REFERENCES public.nameable(id);


--
-- TOC entry 4763 (class 2606 OID 48030)
-- Name: binding_country fk_binding_country_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.binding_country
    ADD CONSTRAINT fk_binding_country_id FOREIGN KEY (id) REFERENCES public.top_level_country(id) NOT VALID;


--
-- TOC entry 4813 (class 2606 OID 189074)
-- Name: blog_post fk_blog_post_id_simple_text_node; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.blog_post
    ADD CONSTRAINT fk_blog_post_id_simple_text_node FOREIGN KEY (id) REFERENCES public.simple_text_node(id) NOT VALID;


--
-- TOC entry 4785 (class 2606 OID 56899)
-- Name: bottom_level_subdivision fk_bottom_level_subdivision_subdivision; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.bottom_level_subdivision
    ADD CONSTRAINT fk_bottom_level_subdivision_subdivision FOREIGN KEY (id) REFERENCES public.subdivision(id) NOT VALID;


--
-- TOC entry 4748 (class 2606 OID 47976)
-- Name: bound_country fk_bound_country_binding_country; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.bound_country
    ADD CONSTRAINT fk_bound_country_binding_country FOREIGN KEY (binding_country_id) REFERENCES public.binding_country(id) NOT VALID;


--
-- TOC entry 4749 (class 2606 OID 2974160)
-- Name: bound_country fk_bound_country_id_country; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.bound_country
    ADD CONSTRAINT fk_bound_country_id_country FOREIGN KEY (id) REFERENCES public.country(id) NOT VALID;


--
-- TOC entry 4750 (class 2606 OID 2974165)
-- Name: bound_country fk_bound_country_id_iso_coded_subdivision; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.bound_country
    ADD CONSTRAINT fk_bound_country_id_iso_coded_subdivision FOREIGN KEY (id) REFERENCES public.iso_coded_subdivision(id) NOT VALID;


--
-- TOC entry 4927 (class 2606 OID 2015923)
-- Name: case_case_parties fk_case_case_parties_case; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.case_case_parties
    ADD CONSTRAINT fk_case_case_parties_case FOREIGN KEY (case_id) REFERENCES public."case"(id) NOT VALID;


--
-- TOC entry 4928 (class 2606 OID 2015929)
-- Name: case_case_parties fk_case_case_parties_case_parties; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.case_case_parties
    ADD CONSTRAINT fk_case_case_parties_case_parties FOREIGN KEY (case_parties_id) REFERENCES public.case_parties(id) NOT VALID;


--
-- TOC entry 4929 (class 2606 OID 2015935)
-- Name: case_case_parties fk_case_case_parties_case_party_type; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.case_case_parties
    ADD CONSTRAINT fk_case_case_parties_case_party_type FOREIGN KEY (case_party_type_id) REFERENCES public.case_party_type(id) NOT VALID;


--
-- TOC entry 4793 (class 2606 OID 69189)
-- Name: case fk_case_id_documentable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."case"
    ADD CONSTRAINT fk_case_id_documentable FOREIGN KEY (id) REFERENCES public.documentable(id) NOT VALID;


--
-- TOC entry 4794 (class 2606 OID 69103)
-- Name: case fk_case_id_locatable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."case"
    ADD CONSTRAINT fk_case_id_locatable FOREIGN KEY (id) REFERENCES public.locatable(id) NOT VALID;


--
-- TOC entry 4795 (class 2606 OID 188290)
-- Name: case fk_case_id_nameable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."case"
    ADD CONSTRAINT fk_case_id_nameable FOREIGN KEY (id) REFERENCES public.nameable(id) NOT VALID;


--
-- TOC entry 4922 (class 2606 OID 2015380)
-- Name: case_parties_organization fk_case_parties_organization_case_parties; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.case_parties_organization
    ADD CONSTRAINT fk_case_parties_organization_case_parties FOREIGN KEY (case_parties_id) REFERENCES public.case_parties(id) NOT VALID;


--
-- TOC entry 4923 (class 2606 OID 2015386)
-- Name: case_parties_organization fk_case_parties_organization_organization; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.case_parties_organization
    ADD CONSTRAINT fk_case_parties_organization_organization FOREIGN KEY (organization_id) REFERENCES public.organization(id) NOT VALID;


--
-- TOC entry 4924 (class 2606 OID 2015392)
-- Name: case_parties_person fk_case_parties_person_case_parties; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.case_parties_person
    ADD CONSTRAINT fk_case_parties_person_case_parties FOREIGN KEY (case_parties_id) REFERENCES public.case_parties(id) NOT VALID;


--
-- TOC entry 4925 (class 2606 OID 2015398)
-- Name: case_parties_person fk_case_parties_person_person; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.case_parties_person
    ADD CONSTRAINT fk_case_parties_person_person FOREIGN KEY (person_id) REFERENCES public.person(id) NOT VALID;


--
-- TOC entry 4926 (class 2606 OID 3701202)
-- Name: case_party_type fk_case_party_type_id_nameable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.case_party_type
    ADD CONSTRAINT fk_case_party_type_id_nameable FOREIGN KEY (id) REFERENCES public.nameable(id) NOT VALID;


--
-- TOC entry 4931 (class 2606 OID 2015963)
-- Name: case_type_case_party_type fk_case_type_case_party_type_case_party_type; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.case_type_case_party_type
    ADD CONSTRAINT fk_case_type_case_party_type_case_party_type FOREIGN KEY (case_party_type_id) REFERENCES public.case_party_type(id) NOT VALID;


--
-- TOC entry 4932 (class 2606 OID 2015957)
-- Name: case_type_case_party_type fk_case_type_case_party_type_case_type; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.case_type_case_party_type
    ADD CONSTRAINT fk_case_type_case_party_type_case_type FOREIGN KEY (case_type_id) REFERENCES public.case_type(id) NOT VALID;


--
-- TOC entry 4930 (class 2606 OID 2015946)
-- Name: case_type fk_case_type_id_node_type; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.case_type
    ADD CONSTRAINT fk_case_type_id_node_type FOREIGN KEY (id) REFERENCES public.node_type(id) NOT VALID;


--
-- TOC entry 4808 (class 2606 OID 189262)
-- Name: child_placement_type fk_child_placement_type_id_nameable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.child_placement_type
    ADD CONSTRAINT fk_child_placement_type_id_nameable FOREIGN KEY (id) REFERENCES public.nameable(id) NOT VALID;


--
-- TOC entry 4801 (class 2606 OID 69130)
-- Name: child_trafficking_case fk_child_trafficking_case_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.child_trafficking_case
    ADD CONSTRAINT fk_child_trafficking_case_id FOREIGN KEY (id) REFERENCES public."case"(id) NOT VALID;


--
-- TOC entry 4802 (class 2606 OID 152282)
-- Name: child_trafficking_case fk_childtrafficking_case_country_from; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.child_trafficking_case
    ADD CONSTRAINT fk_childtrafficking_case_country_from FOREIGN KEY (country_id_from) REFERENCES public.country(id) NOT VALID;


--
-- TOC entry 4805 (class 2606 OID 69162)
-- Name: coerced_adoption_case fk_coerced_adoption_case_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.coerced_adoption_case
    ADD CONSTRAINT fk_coerced_adoption_case_id FOREIGN KEY (id) REFERENCES public."case"(id) NOT VALID;


--
-- TOC entry 4871 (class 2606 OID 545091)
-- Name: collective fk_collective_id_publisher; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.collective
    ADD CONSTRAINT fk_collective_id_publisher FOREIGN KEY (id) REFERENCES public.publisher(id) NOT VALID;


--
-- TOC entry 4872 (class 2606 OID 547157)
-- Name: collective_user fk_collective_user_collective; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.collective_user
    ADD CONSTRAINT fk_collective_user_collective FOREIGN KEY (collective_id) REFERENCES public.collective(id) NOT VALID;


--
-- TOC entry 4873 (class 2606 OID 547162)
-- Name: collective_user fk_collective_user_user; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.collective_user
    ADD CONSTRAINT fk_collective_user_user FOREIGN KEY (user_id) REFERENCES public."user"(id) NOT VALID;


--
-- TOC entry 4706 (class 2606 OID 403138)
-- Name: comment fk_comment_comment_parent; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.comment
    ADD CONSTRAINT fk_comment_comment_parent FOREIGN KEY (comment_id_parent) REFERENCES public.comment(id) NOT VALID;


--
-- TOC entry 4707 (class 2606 OID 32806)
-- Name: comment fk_comment_node; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.comment
    ADD CONSTRAINT fk_comment_node FOREIGN KEY (node_id) REFERENCES public.node(id) ON UPDATE RESTRICT ON DELETE RESTRICT NOT VALID;


--
-- TOC entry 4708 (class 2606 OID 787788)
-- Name: comment fk_comment_publisher; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.comment
    ADD CONSTRAINT fk_comment_publisher FOREIGN KEY (publisher_id) REFERENCES public.publisher(id) NOT VALID;


--
-- TOC entry 4958 (class 2606 OID 3684066)
-- Name: congressional_term fk_congressional_term_documentable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.congressional_term
    ADD CONSTRAINT fk_congressional_term_documentable FOREIGN KEY (id) REFERENCES public.documentable(id) NOT VALID;


--
-- TOC entry 4959 (class 2606 OID 3745459)
-- Name: congressional_term_political_party_affiliation fk_congressional_term_political_party_affiliation_id_documentab; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.congressional_term_political_party_affiliation
    ADD CONSTRAINT fk_congressional_term_political_party_affiliation_id_documentab FOREIGN KEY (id) REFERENCES public.documentable(id) NOT VALID;


--
-- TOC entry 4960 (class 2606 OID 3745465)
-- Name: congressional_term_political_party_affiliation fk_congressional_term_political_party_affiliation_united_states; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.congressional_term_political_party_affiliation
    ADD CONSTRAINT fk_congressional_term_political_party_affiliation_united_states FOREIGN KEY (united_states_political_party_affiliation_id) REFERENCES public.united_states_political_party_affiliation(id) NOT VALID;


--
-- TOC entry 4875 (class 2606 OID 545512)
-- Name: content_sharing_group fk_content_sharing_group_id_owner; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.content_sharing_group
    ADD CONSTRAINT fk_content_sharing_group_id_owner FOREIGN KEY (id) REFERENCES public.owner(id) NOT VALID;


--
-- TOC entry 4782 (class 2606 OID 48209)
-- Name: country_and_first_and_bottom_level_subdivision fk_country_and_first_and_bottom_level_subdivision_id_1; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.country_and_first_and_bottom_level_subdivision
    ADD CONSTRAINT fk_country_and_first_and_bottom_level_subdivision_id_1 FOREIGN KEY (id) REFERENCES public.country_and_first_level_subdivision(id) NOT VALID;


--
-- TOC entry 4783 (class 2606 OID 58167)
-- Name: country_and_first_and_bottom_level_subdivision fk_country_and_first_and_bottom_level_subdivision_id_2; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.country_and_first_and_bottom_level_subdivision
    ADD CONSTRAINT fk_country_and_first_and_bottom_level_subdivision_id_2 FOREIGN KEY (id) REFERENCES public.country_and_first_and_bottom_level_subdivision(id) NOT VALID;


--
-- TOC entry 4779 (class 2606 OID 2974140)
-- Name: country_and_first_and_second_level_subdivision fk_country_and_first_and_second_level_subdivision_id_country_an; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.country_and_first_and_second_level_subdivision
    ADD CONSTRAINT fk_country_and_first_and_second_level_subdivision_id_country_an FOREIGN KEY (id) REFERENCES public.country_and_first_level_subdivision(id) NOT VALID;


--
-- TOC entry 4780 (class 2606 OID 2974145)
-- Name: country_and_first_and_second_level_subdivision fk_country_and_first_and_second_level_subdivision_id_first_and_; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.country_and_first_and_second_level_subdivision
    ADD CONSTRAINT fk_country_and_first_and_second_level_subdivision_id_first_and_ FOREIGN KEY (id) REFERENCES public.first_and_second_level_subdivision(id) NOT VALID;


--
-- TOC entry 4772 (class 2606 OID 2974155)
-- Name: country_and_first_level_subdivision fk_country_and_first_level_subdivision_id_iso_coded_first_level; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.country_and_first_level_subdivision
    ADD CONSTRAINT fk_country_and_first_level_subdivision_id_iso_coded_first_level FOREIGN KEY (id) REFERENCES public.iso_coded_first_level_subdivision(id) NOT VALID;


--
-- TOC entry 4773 (class 2606 OID 2974150)
-- Name: country_and_first_level_subdivision fk_country_and_first_level_subdivision_id_top_level_country; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.country_and_first_level_subdivision
    ADD CONSTRAINT fk_country_and_first_level_subdivision_id_top_level_country FOREIGN KEY (id) REFERENCES public.top_level_country(id) NOT VALID;


--
-- TOC entry 4788 (class 2606 OID 58177)
-- Name: country_and_intermediate_level_subdivision fk_country_and_intermediate_level_subdivision_1; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.country_and_intermediate_level_subdivision
    ADD CONSTRAINT fk_country_and_intermediate_level_subdivision_1 FOREIGN KEY (id) REFERENCES public.country_and_first_level_subdivision(id) NOT VALID;


--
-- TOC entry 4789 (class 2606 OID 58183)
-- Name: country_and_intermediate_level_subdivision fk_country_and_intermediate_level_subdivision_2; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.country_and_intermediate_level_subdivision
    ADD CONSTRAINT fk_country_and_intermediate_level_subdivision_2 FOREIGN KEY (id) REFERENCES public.intermediate_level_subdivision(id) NOT VALID;


--
-- TOC entry 4712 (class 2606 OID 177588)
-- Name: country fk_country_hague_status; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.country
    ADD CONSTRAINT fk_country_hague_status FOREIGN KEY (hague_status_id) REFERENCES public.hague_status(id) NOT VALID;


--
-- TOC entry 4713 (class 2606 OID 717755)
-- Name: country fk_country_id_political_entity; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.country
    ADD CONSTRAINT fk_country_id_political_entity FOREIGN KEY (id) REFERENCES public.political_entity(id) NOT VALID;


--
-- TOC entry 4910 (class 2606 OID 904087)
-- Name: country_report fk_country_report_country; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.country_report
    ADD CONSTRAINT fk_country_report_country FOREIGN KEY (country_id) REFERENCES public.country(id) NOT VALID;


--
-- TOC entry 4914 (class 2606 OID 960322)
-- Name: country_subdivision_type fk_country_subdivision_type_country; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.country_subdivision_type
    ADD CONSTRAINT fk_country_subdivision_type_country FOREIGN KEY (country_id) REFERENCES public.country(id) NOT VALID;


--
-- TOC entry 4915 (class 2606 OID 966319)
-- Name: country_subdivision_type fk_country_subdivision_type_subdivision_type; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.country_subdivision_type
    ADD CONSTRAINT fk_country_subdivision_type_subdivision_type FOREIGN KEY (subdivision_type_id) REFERENCES public.subdivision_type(id) NOT VALID;


--
-- TOC entry 4891 (class 2606 OID 660721)
-- Name: create_node_action fk_create_node_action_id_action; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.create_node_action
    ADD CONSTRAINT fk_create_node_action_id_action FOREIGN KEY (id) REFERENCES public.action(id) NOT VALID;


--
-- TOC entry 4892 (class 2606 OID 660642)
-- Name: create_node_action fk_create_node_action_node_type; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.create_node_action
    ADD CONSTRAINT fk_create_node_action_node_type FOREIGN KEY (node_type_id) REFERENCES public.node_type(id) NOT VALID;


--
-- TOC entry 4894 (class 2606 OID 660726)
-- Name: delete_node_action fk_delete_node_action_id_access_privilege; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.delete_node_action
    ADD CONSTRAINT fk_delete_node_action_id_access_privilege FOREIGN KEY (id) REFERENCES public.action(id);


--
-- TOC entry 4895 (class 2606 OID 660685)
-- Name: delete_node_action fk_delete_node_action_node_type; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.delete_node_action
    ADD CONSTRAINT fk_delete_node_action_node_type FOREIGN KEY (node_type_id) REFERENCES public.node_type(id);


--
-- TOC entry 4720 (class 2606 OID 189283)
-- Name: denomination fk_denomination_id_nameable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.denomination
    ADD CONSTRAINT fk_denomination_id_nameable FOREIGN KEY (id) REFERENCES public.nameable(id) NOT VALID;


--
-- TOC entry 4810 (class 2606 OID 144399)
-- Name: deportation_case fk_deportation_case_country_id_to; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.deportation_case
    ADD CONSTRAINT fk_deportation_case_country_id_to FOREIGN KEY (country_id_to) REFERENCES public.country(id) NOT VALID;


--
-- TOC entry 4811 (class 2606 OID 144393)
-- Name: deportation_case fk_deportation_case_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.deportation_case
    ADD CONSTRAINT fk_deportation_case_id FOREIGN KEY (id) REFERENCES public."case"(id) NOT VALID;


--
-- TOC entry 4812 (class 2606 OID 144387)
-- Name: deportation_case fk_deportation_case_subdivision_id_from; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.deportation_case
    ADD CONSTRAINT fk_deportation_case_subdivision_id_from FOREIGN KEY (subdivision_id_from) REFERENCES public.subdivision(id) NOT VALID;


--
-- TOC entry 4815 (class 2606 OID 189079)
-- Name: discussion fk_discussion_id_simple_text_node; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.discussion
    ADD CONSTRAINT fk_discussion_id_simple_text_node FOREIGN KEY (id) REFERENCES public.simple_text_node(id) NOT VALID;


--
-- TOC entry 4807 (class 2606 OID 69183)
-- Name: disrupted_placement_case fk_disrupted_placement_case_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.disrupted_placement_case
    ADD CONSTRAINT fk_disrupted_placement_case_id FOREIGN KEY (id) REFERENCES public."case"(id) NOT VALID;


--
-- TOC entry 4735 (class 2606 OID 71830)
-- Name: document fk_document_document_type_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.document
    ADD CONSTRAINT fk_document_document_type_id FOREIGN KEY (document_type_id) REFERENCES public.document_type(id) NOT VALID;


--
-- TOC entry 4736 (class 2606 OID 1910817)
-- Name: document fk_document_id_searchable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.document
    ADD CONSTRAINT fk_document_id_searchable FOREIGN KEY (id) REFERENCES public.searchable(id) NOT VALID;


--
-- TOC entry 4722 (class 2606 OID 189288)
-- Name: document_type fk_document_type_id_nameable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.document_type
    ADD CONSTRAINT fk_document_type_id_nameable FOREIGN KEY (id) REFERENCES public.nameable(id) NOT VALID;


--
-- TOC entry 4902 (class 2606 OID 787808)
-- Name: documentable_document fk_documentable_document_document; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.documentable_document
    ADD CONSTRAINT fk_documentable_document_document FOREIGN KEY (document_id) REFERENCES public.document(id) NOT VALID;


--
-- TOC entry 4903 (class 2606 OID 787802)
-- Name: documentable_document fk_documentable_document_documentable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.documentable_document
    ADD CONSTRAINT fk_documentable_document_documentable FOREIGN KEY (documentable_id) REFERENCES public.documentable(id) NOT VALID;


--
-- TOC entry 4790 (class 2606 OID 1910822)
-- Name: documentable fk_documentable_id_searchable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.documentable
    ADD CONSTRAINT fk_documentable_id_searchable FOREIGN KEY (id) REFERENCES public.searchable(id) NOT VALID;


--
-- TOC entry 4896 (class 2606 OID 660731)
-- Name: edit_node_action fk_edit_node_action_id_action; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.edit_node_action
    ADD CONSTRAINT fk_edit_node_action_id_action FOREIGN KEY (id) REFERENCES public.action(id);


--
-- TOC entry 4897 (class 2606 OID 660702)
-- Name: edit_node_action fk_edit_node_action_node_type; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.edit_node_action
    ADD CONSTRAINT fk_edit_node_action_node_type FOREIGN KEY (node_type_id) REFERENCES public.node_type(id);


--
-- TOC entry 4841 (class 2606 OID 189201)
-- Name: facilitator fk_facilitator_id_organizational_role; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.facilitator
    ADD CONSTRAINT fk_facilitator_id_organizational_role FOREIGN KEY (id) REFERENCES public.organizational_role(id) NOT VALID;


--
-- TOC entry 4809 (class 2606 OID 189267)
-- Name: family_size fk_family_size_id_nameable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.family_size
    ADD CONSTRAINT fk_family_size_id_nameable FOREIGN KEY (id) REFERENCES public.nameable(id) NOT VALID;


--
-- TOC entry 4806 (class 2606 OID 69172)
-- Name: fathers_rights_violation_case fk_fathers_rights_violation_case_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.fathers_rights_violation_case
    ADD CONSTRAINT fk_fathers_rights_violation_case_id FOREIGN KEY (id) REFERENCES public."case"(id) NOT VALID;


--
-- TOC entry 4727 (class 2606 OID 66995)
-- Name: person fk_file_id_file_portrait; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.person
    ADD CONSTRAINT fk_file_id_file_portrait FOREIGN KEY (file_id_portrait) REFERENCES public.file(id) NOT VALID;


--
-- TOC entry 4786 (class 2606 OID 56931)
-- Name: first_and_bottom_level_subdivision fk_first_and_bottom_level_subdivision_id_1; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.first_and_bottom_level_subdivision
    ADD CONSTRAINT fk_first_and_bottom_level_subdivision_id_1 FOREIGN KEY (id) REFERENCES public.iso_coded_first_level_subdivision(id) NOT VALID;


--
-- TOC entry 4787 (class 2606 OID 56937)
-- Name: first_and_bottom_level_subdivision fk_first_and_bottom_level_subdivision_id_2; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.first_and_bottom_level_subdivision
    ADD CONSTRAINT fk_first_and_bottom_level_subdivision_id_2 FOREIGN KEY (id) REFERENCES public.bottom_level_subdivision(id) NOT VALID;


--
-- TOC entry 4774 (class 2606 OID 48142)
-- Name: first_and_second_level_subdivision fk_first_and_second_level_subdivision_id_1; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.first_and_second_level_subdivision
    ADD CONSTRAINT fk_first_and_second_level_subdivision_id_1 FOREIGN KEY (id) REFERENCES public.iso_coded_first_level_subdivision(id) NOT VALID;


--
-- TOC entry 4775 (class 2606 OID 48148)
-- Name: first_and_second_level_subdivision fk_first_and_second_level_subdivision_id_2; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.first_and_second_level_subdivision
    ADD CONSTRAINT fk_first_and_second_level_subdivision_id_2 FOREIGN KEY (id) REFERENCES public.second_level_subdivision(id) NOT VALID;


--
-- TOC entry 4753 (class 2606 OID 48018)
-- Name: first_level_global_region fk_first_level_global_region_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.first_level_global_region
    ADD CONSTRAINT fk_first_level_global_region_id FOREIGN KEY (id) REFERENCES public.global_region(id) NOT VALID;


--
-- TOC entry 4754 (class 2606 OID 48077)
-- Name: first_level_subdivision fk_first_level_subdivision_id_subdivision; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.first_level_subdivision
    ADD CONSTRAINT fk_first_level_subdivision_id_subdivision FOREIGN KEY (id) REFERENCES public.subdivision(id) NOT VALID;


--
-- TOC entry 4776 (class 2606 OID 56916)
-- Name: formal_intermediate_level_subdivision fk_formal_intermediate_level_subdivision_id_1; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.formal_intermediate_level_subdivision
    ADD CONSTRAINT fk_formal_intermediate_level_subdivision_id_1 FOREIGN KEY (id) REFERENCES public.iso_coded_first_level_subdivision(id) NOT VALID;


--
-- TOC entry 4777 (class 2606 OID 56921)
-- Name: formal_intermediate_level_subdivision fk_formal_intermediate_level_subdivision_id_2; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.formal_intermediate_level_subdivision
    ADD CONSTRAINT fk_formal_intermediate_level_subdivision_id_2 FOREIGN KEY (id) REFERENCES public.intermediate_level_subdivision(id) NOT VALID;


--
-- TOC entry 4764 (class 2606 OID 67904)
-- Name: geographical_entity fk_geographical_entity_id_documentable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.geographical_entity
    ADD CONSTRAINT fk_geographical_entity_id_documentable FOREIGN KEY (id) REFERENCES public.documentable(id) NOT VALID;


--
-- TOC entry 4765 (class 2606 OID 188199)
-- Name: geographical_entity fk_geographical_entity_id_nameable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.geographical_entity
    ADD CONSTRAINT fk_geographical_entity_id_nameable FOREIGN KEY (id) REFERENCES public.nameable(id) NOT VALID;


--
-- TOC entry 4766 (class 2606 OID 48012)
-- Name: global_region fk_global_region_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.global_region
    ADD CONSTRAINT fk_global_region_id FOREIGN KEY (id) REFERENCES public.geographical_entity(id) NOT VALID;


--
-- TOC entry 4721 (class 2606 OID 189293)
-- Name: hague_status fk_hague_status_id_nameable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.hague_status
    ADD CONSTRAINT fk_hague_status_id_nameable FOREIGN KEY (id) REFERENCES public.nameable(id);


--
-- TOC entry 4843 (class 2606 OID 189223)
-- Name: home_study_agency fk_home_study_agency_id_organization_role; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.home_study_agency
    ADD CONSTRAINT fk_home_study_agency_id_organization_role FOREIGN KEY (id) REFERENCES public.organizational_role(id) NOT VALID;


--
-- TOC entry 4876 (class 2606 OID 636063)
-- Name: house_bill fk_house_bill_id_bill; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.house_bill
    ADD CONSTRAINT fk_house_bill_id_bill FOREIGN KEY (id) REFERENCES public.bill(id) NOT VALID;


--
-- TOC entry 4954 (class 2606 OID 3745454)
-- Name: house_term fk_house_term_id_congressional_term; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.house_term
    ADD CONSTRAINT fk_house_term_id_congressional_term FOREIGN KEY (id) REFERENCES public.congressional_term(id);


--
-- TOC entry 4955 (class 2606 OID 3684025)
-- Name: house_term fk_house_term_representative; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.house_term
    ADD CONSTRAINT fk_house_term_representative FOREIGN KEY (representative_id) REFERENCES public.representative(id);


--
-- TOC entry 4956 (class 2606 OID 3684030)
-- Name: house_term fk_house_term_subdivision; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.house_term
    ADD CONSTRAINT fk_house_term_subdivision FOREIGN KEY (subdivision_id) REFERENCES public.subdivision(id);


--
-- TOC entry 4778 (class 2606 OID 56911)
-- Name: informal_intermediate_level_subdivision fk_informal_intermediate_level_subdivision_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.informal_intermediate_level_subdivision
    ADD CONSTRAINT fk_informal_intermediate_level_subdivision_id FOREIGN KEY (id) REFERENCES public.intermediate_level_subdivision(id) NOT VALID;


--
-- TOC entry 4844 (class 2606 OID 189234)
-- Name: institution fk_institution_id_organizational_role; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.institution
    ADD CONSTRAINT fk_institution_id_organizational_role FOREIGN KEY (id) REFERENCES public.organizational_role(id) NOT VALID;


--
-- TOC entry 4905 (class 2606 OID 860250)
-- Name: inter_country_relation fk_inter_country_relation_country_from; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.inter_country_relation
    ADD CONSTRAINT fk_inter_country_relation_country_from FOREIGN KEY (country_id_from) REFERENCES public.country(id) NOT VALID;


--
-- TOC entry 4906 (class 2606 OID 860256)
-- Name: inter_country_relation fk_inter_country_relation_country_to; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.inter_country_relation
    ADD CONSTRAINT fk_inter_country_relation_country_to FOREIGN KEY (country_id_to) REFERENCES public.country(id) NOT VALID;


--
-- TOC entry 4907 (class 2606 OID 860268)
-- Name: inter_country_relation fk_inter_country_relation_document_id_proof; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.inter_country_relation
    ADD CONSTRAINT fk_inter_country_relation_document_id_proof FOREIGN KEY (document_id_proof) REFERENCES public.document(id) NOT VALID;


--
-- TOC entry 4908 (class 2606 OID 1910857)
-- Name: inter_country_relation fk_inter_country_relation_id_node; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.inter_country_relation
    ADD CONSTRAINT fk_inter_country_relation_id_node FOREIGN KEY (id) REFERENCES public.node(id) NOT VALID;


--
-- TOC entry 4909 (class 2606 OID 860262)
-- Name: inter_country_relation fk_inter_country_relation_inter_country_relation_type; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.inter_country_relation
    ADD CONSTRAINT fk_inter_country_relation_inter_country_relation_type FOREIGN KEY (inter_country_relation_type_id) REFERENCES public.inter_country_relation_type(id) NOT VALID;


--
-- TOC entry 4904 (class 2606 OID 860235)
-- Name: inter_country_relation_type fk_inter_country_relation_type_id_nameable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.inter_country_relation_type
    ADD CONSTRAINT fk_inter_country_relation_type_id_nameable FOREIGN KEY (id) REFERENCES public.nameable(id) NOT VALID;


--
-- TOC entry 4729 (class 2606 OID 1254951)
-- Name: inter_organizational_relation fk_inter_organizational_relation_document_proof; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.inter_organizational_relation
    ADD CONSTRAINT fk_inter_organizational_relation_document_proof FOREIGN KEY (document_id_proof) REFERENCES public.document(id) NOT VALID;


--
-- TOC entry 4730 (class 2606 OID 1254931)
-- Name: inter_organizational_relation fk_inter_organizational_relation_geographical_entity; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.inter_organizational_relation
    ADD CONSTRAINT fk_inter_organizational_relation_geographical_entity FOREIGN KEY (geographical_entity_id) REFERENCES public.geographical_entity(id) NOT VALID;


--
-- TOC entry 4731 (class 2606 OID 33034)
-- Name: inter_organizational_relation fk_inter_organizational_relation_organizational_from; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.inter_organizational_relation
    ADD CONSTRAINT fk_inter_organizational_relation_organizational_from FOREIGN KEY (organization_id_from) REFERENCES public.organization(id) NOT VALID;


--
-- TOC entry 4732 (class 2606 OID 1254946)
-- Name: inter_organizational_relation fk_inter_organizational_relation_organizational_to; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.inter_organizational_relation
    ADD CONSTRAINT fk_inter_organizational_relation_organizational_to FOREIGN KEY (organization_id_to) REFERENCES public.organization(id) NOT VALID;


--
-- TOC entry 4711 (class 2606 OID 188239)
-- Name: inter_organizational_relation_type fk_inter_organizational_relation_type_id_nameable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.inter_organizational_relation_type
    ADD CONSTRAINT fk_inter_organizational_relation_type_id_nameable FOREIGN KEY (id) REFERENCES public.nameable(id) NOT VALID;


--
-- TOC entry 4733 (class 2606 OID 1254941)
-- Name: inter_organizational_relation fk_inter_organizationale_relation_inter_organizational_relation; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.inter_organizational_relation
    ADD CONSTRAINT fk_inter_organizationale_relation_inter_organizational_relation FOREIGN KEY (inter_organizational_relation_type_id) REFERENCES public.inter_organizational_relation_type(id) NOT VALID;


--
-- TOC entry 4737 (class 2606 OID 33098)
-- Name: inter_personal_relation fk_inter_personal_relation_document_proof; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.inter_personal_relation
    ADD CONSTRAINT fk_inter_personal_relation_document_proof FOREIGN KEY (document_id_proof) REFERENCES public.document(id) NOT VALID;


--
-- TOC entry 4738 (class 2606 OID 1910842)
-- Name: inter_personal_relation fk_inter_personal_relation_id_node; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.inter_personal_relation
    ADD CONSTRAINT fk_inter_personal_relation_id_node FOREIGN KEY (id) REFERENCES public.node(id) NOT VALID;


--
-- TOC entry 4739 (class 2606 OID 33092)
-- Name: inter_personal_relation fk_inter_personal_relation_inter_personal_relation_type; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.inter_personal_relation
    ADD CONSTRAINT fk_inter_personal_relation_inter_personal_relation_type FOREIGN KEY (inter_personal_relation_type_id) REFERENCES public.inter_personal_relation_type(id) NOT VALID;


--
-- TOC entry 4740 (class 2606 OID 33080)
-- Name: inter_personal_relation fk_inter_personal_relation_person_from; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.inter_personal_relation
    ADD CONSTRAINT fk_inter_personal_relation_person_from FOREIGN KEY (person_id_from) REFERENCES public.person(id) NOT VALID;


--
-- TOC entry 4741 (class 2606 OID 33086)
-- Name: inter_personal_relation fk_inter_personal_relation_person_to; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.inter_personal_relation
    ADD CONSTRAINT fk_inter_personal_relation_person_to FOREIGN KEY (person_id_to) REFERENCES public.person(id) NOT VALID;


--
-- TOC entry 4718 (class 2606 OID 189298)
-- Name: inter_personal_relation_type fk_inter_personal_relation_type_id_nameable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.inter_personal_relation_type
    ADD CONSTRAINT fk_inter_personal_relation_type_id_nameable FOREIGN KEY (id) REFERENCES public.nameable(id) NOT VALID;


--
-- TOC entry 4784 (class 2606 OID 56905)
-- Name: intermediate_level_subdivision fk_intermediate_level_subdivision_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.intermediate_level_subdivision
    ADD CONSTRAINT fk_intermediate_level_subdivision_id FOREIGN KEY (id) REFERENCES public.first_level_subdivision(id) NOT VALID;


--
-- TOC entry 4768 (class 2606 OID 48092)
-- Name: iso_coded_first_level_subdivision fk_iso_coded_first_level_subdivision_1; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.iso_coded_first_level_subdivision
    ADD CONSTRAINT fk_iso_coded_first_level_subdivision_1 FOREIGN KEY (id) REFERENCES public.first_level_subdivision(id) NOT VALID;


--
-- TOC entry 4769 (class 2606 OID 48098)
-- Name: iso_coded_first_level_subdivision fk_iso_coded_first_level_subdivision_2; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.iso_coded_first_level_subdivision
    ADD CONSTRAINT fk_iso_coded_first_level_subdivision_2 FOREIGN KEY (id) REFERENCES public.iso_coded_subdivision(id) NOT VALID;


--
-- TOC entry 4755 (class 2606 OID 35777)
-- Name: iso_coded_subdivision fk_iso_coded_subdivision_id_political_entity; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.iso_coded_subdivision
    ADD CONSTRAINT fk_iso_coded_subdivision_id_political_entity FOREIGN KEY (id) REFERENCES public.political_entity(id) NOT VALID;


--
-- TOC entry 4756 (class 2606 OID 48062)
-- Name: iso_coded_subdivision fk_iso_coded_subdivision_id_subdivision; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.iso_coded_subdivision
    ADD CONSTRAINT fk_iso_coded_subdivision_id_subdivision FOREIGN KEY (id) REFERENCES public.subdivision(id) NOT VALID;


--
-- TOC entry 4842 (class 2606 OID 189212)
-- Name: law_firm fk_law_firm_id_organizational_role; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.law_firm
    ADD CONSTRAINT fk_law_firm_id_organizational_role FOREIGN KEY (id) REFERENCES public.organizational_role(id) NOT VALID;


--
-- TOC entry 4792 (class 2606 OID 1910832)
-- Name: locatable fk_locatable_id_searchable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.locatable
    ADD CONSTRAINT fk_locatable_id_searchable FOREIGN KEY (id) REFERENCES public.searchable(id) NOT VALID;


--
-- TOC entry 4744 (class 2606 OID 152604)
-- Name: location fk_location_country; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.location
    ADD CONSTRAINT fk_location_country FOREIGN KEY (country_id) REFERENCES public.country(id) NOT VALID;


--
-- TOC entry 4796 (class 2606 OID 69715)
-- Name: location_locatable fk_location_locatable_locatable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.location_locatable
    ADD CONSTRAINT fk_location_locatable_locatable FOREIGN KEY (locatable_id) REFERENCES public.locatable(id) NOT VALID;


--
-- TOC entry 4797 (class 2606 OID 4013439)
-- Name: location_locatable fk_location_locatable_location; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.location_locatable
    ADD CONSTRAINT fk_location_locatable_location FOREIGN KEY (location_id) REFERENCES public.location(id) NOT VALID;


--
-- TOC entry 4745 (class 2606 OID 152276)
-- Name: location fk_location_subdivision; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.location
    ADD CONSTRAINT fk_location_subdivision FOREIGN KEY (subdivision_id) REFERENCES public.subdivision(id) NOT VALID;


--
-- TOC entry 4878 (class 2606 OID 3027626)
-- Name: member_of_congress fk_member_of_congress_professional_role; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.member_of_congress
    ADD CONSTRAINT fk_member_of_congress_professional_role FOREIGN KEY (id) REFERENCES public.professional_role(id) NOT VALID;


--
-- TOC entry 4944 (class 2606 OID 2708799)
-- Name: multi_question_poll fk_multi_question_poll_id_poll; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.multi_question_poll
    ADD CONSTRAINT fk_multi_question_poll_id_poll FOREIGN KEY (id) REFERENCES public.poll(id) NOT VALID;


--
-- TOC entry 4945 (class 2606 OID 2708828)
-- Name: multi_question_poll fk_multi_question_poll_id_simpe_text_node; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.multi_question_poll
    ADD CONSTRAINT fk_multi_question_poll_id_simpe_text_node FOREIGN KEY (id) REFERENCES public.simple_text_node(id) NOT VALID;


--
-- TOC entry 4947 (class 2606 OID 2708846)
-- Name: multi_question_poll_poll_question fk_multi_question_poll_question_multi_question_poll; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.multi_question_poll_poll_question
    ADD CONSTRAINT fk_multi_question_poll_question_multi_question_poll FOREIGN KEY (multi_question_poll_id) REFERENCES public.multi_question_poll(id) NOT VALID;


--
-- TOC entry 4948 (class 2606 OID 2708852)
-- Name: multi_question_poll_poll_question fk_multi_question_poll_question_poll_question; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.multi_question_poll_poll_question
    ADD CONSTRAINT fk_multi_question_poll_question_poll_question FOREIGN KEY (poll_question_id) REFERENCES public.poll_question(id) NOT VALID;


--
-- TOC entry 4818 (class 2606 OID 196608)
-- Name: nameable fk_nameable_file_tile_image; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.nameable
    ADD CONSTRAINT fk_nameable_file_tile_image FOREIGN KEY (file_id_tile_image) REFERENCES public.file(id) NOT VALID;


--
-- TOC entry 4819 (class 2606 OID 1910827)
-- Name: nameable fk_nameable_id_searchable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.nameable
    ADD CONSTRAINT fk_nameable_id_searchable FOREIGN KEY (id) REFERENCES public.searchable(id) NOT VALID;


--
-- TOC entry 4935 (class 2606 OID 2403978)
-- Name: node_file fk_node_file_file; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.node_file
    ADD CONSTRAINT fk_node_file_file FOREIGN KEY (file_id) REFERENCES public.file(id) NOT VALID;


--
-- TOC entry 4936 (class 2606 OID 2403972)
-- Name: node_file fk_node_file_node; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.node_file
    ADD CONSTRAINT fk_node_file_node FOREIGN KEY (node_id) REFERENCES public.node(id) NOT VALID;


--
-- TOC entry 4703 (class 2606 OID 32819)
-- Name: node fk_node_node_type; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.node
    ADD CONSTRAINT fk_node_node_type FOREIGN KEY (node_type_id) REFERENCES public.node_type(id) NOT VALID;


--
-- TOC entry 4704 (class 2606 OID 545502)
-- Name: node fk_node_owner; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.node
    ADD CONSTRAINT fk_node_owner FOREIGN KEY (owner_id) REFERENCES public.owner(id) NOT VALID;


--
-- TOC entry 4705 (class 2606 OID 547180)
-- Name: node fk_node_publisher; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.node
    ADD CONSTRAINT fk_node_publisher FOREIGN KEY (publisher_id) REFERENCES public.publisher(id) ON UPDATE RESTRICT ON DELETE RESTRICT NOT VALID;


--
-- TOC entry 4856 (class 2606 OID 611539)
-- Name: node_term fk_node_term_node; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.node_term
    ADD CONSTRAINT fk_node_term_node FOREIGN KEY (node_id) REFERENCES public.node(id) NOT VALID;


--
-- TOC entry 4857 (class 2606 OID 611545)
-- Name: node_term fk_node_term_term; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.node_term
    ADD CONSTRAINT fk_node_term_term FOREIGN KEY (term_id) REFERENCES public.term(id) NOT VALID;


--
-- TOC entry 4888 (class 2606 OID 575949)
-- Name: organization_act_relation_type fk_organization_act_relation_type; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.organization_act_relation_type
    ADD CONSTRAINT fk_organization_act_relation_type FOREIGN KEY (id) REFERENCES public.nameable(id) NOT VALID;


--
-- TOC entry 4723 (class 2606 OID 33012)
-- Name: organization fk_organization_id_party; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.organization
    ADD CONSTRAINT fk_organization_id_party FOREIGN KEY (id) REFERENCES public.party(id) NOT VALID;


--
-- TOC entry 4911 (class 2606 OID 899653)
-- Name: organization_organization_type fk_organization_organization_type_organization; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.organization_organization_type
    ADD CONSTRAINT fk_organization_organization_type_organization FOREIGN KEY (organization_id) REFERENCES public.organization(id) NOT VALID;


--
-- TOC entry 4912 (class 2606 OID 899659)
-- Name: organization_organization_type fk_organization_organization_type_organization_type; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.organization_organization_type
    ADD CONSTRAINT fk_organization_organization_type_organization_type FOREIGN KEY (organization_type_id) REFERENCES public.organization_type(id) NOT VALID;


--
-- TOC entry 4710 (class 2606 OID 189303)
-- Name: organization_type fk_organization_type_id_nameable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.organization_type
    ADD CONSTRAINT fk_organization_type_id_nameable FOREIGN KEY (id) REFERENCES public.nameable(id) NOT VALID;


--
-- TOC entry 4838 (class 2606 OID 189166)
-- Name: organizational_role fk_organizational_role_organization; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.organizational_role
    ADD CONSTRAINT fk_organizational_role_organization FOREIGN KEY (organization_id) REFERENCES public.organization(id) NOT VALID;


--
-- TOC entry 4839 (class 2606 OID 189172)
-- Name: organizational_role fk_organizational_role_organization_type; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.organizational_role
    ADD CONSTRAINT fk_organizational_role_organization_type FOREIGN KEY (organization_type_id) REFERENCES public.organization_type(id) NOT VALID;


--
-- TOC entry 4874 (class 2606 OID 545107)
-- Name: owner fk_owner_id_user_group; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.owner
    ADD CONSTRAINT fk_owner_id_user_group FOREIGN KEY (id) REFERENCES public.user_group(id) NOT VALID;


--
-- TOC entry 4858 (class 2606 OID 403148)
-- Name: page fk_page_id_simple_text_node; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.page
    ADD CONSTRAINT fk_page_id_simple_text_node FOREIGN KEY (id) REFERENCES public.simple_text_node(id) NOT VALID;


--
-- TOC entry 4724 (class 2606 OID 67899)
-- Name: party fk_party_id_documentable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.party
    ADD CONSTRAINT fk_party_id_documentable FOREIGN KEY (id) REFERENCES public.documentable(id) NOT VALID;


--
-- TOC entry 4725 (class 2606 OID 69093)
-- Name: party fk_party_id_locatable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.party
    ADD CONSTRAINT fk_party_id_locatable FOREIGN KEY (id) REFERENCES public.locatable(id) NOT VALID;


--
-- TOC entry 4726 (class 2606 OID 188295)
-- Name: party fk_party_id_nameable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.party
    ADD CONSTRAINT fk_party_id_nameable FOREIGN KEY (id) REFERENCES public.nameable(id) NOT VALID;


--
-- TOC entry 4823 (class 2606 OID 189008)
-- Name: party_political_entity_relation fk_party_political_entity_relation_document_proof; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.party_political_entity_relation
    ADD CONSTRAINT fk_party_political_entity_relation_document_proof FOREIGN KEY (document_id_proof) REFERENCES public.document(id) NOT VALID;


--
-- TOC entry 4824 (class 2606 OID 189180)
-- Name: party_political_entity_relation fk_party_political_entity_relation_party; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.party_political_entity_relation
    ADD CONSTRAINT fk_party_political_entity_relation_party FOREIGN KEY (party_id) REFERENCES public.party(id) NOT VALID;


--
-- TOC entry 4825 (class 2606 OID 189014)
-- Name: party_political_entity_relation fk_party_political_entity_relation_political_entity; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.party_political_entity_relation
    ADD CONSTRAINT fk_party_political_entity_relation_political_entity FOREIGN KEY (political_entity_id) REFERENCES public.political_entity(id) NOT VALID;


--
-- TOC entry 4826 (class 2606 OID 189026)
-- Name: party_political_entity_relation fk_party_political_entity_relation_political_entity_relation_ty; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.party_political_entity_relation
    ADD CONSTRAINT fk_party_political_entity_relation_political_entity_relation_ty FOREIGN KEY (party_political_entity_relation_type_id) REFERENCES public.party_political_entity_relation_type(id) NOT VALID;


--
-- TOC entry 4716 (class 2606 OID 188264)
-- Name: party_political_entity_relation_type fk_party_political_entity_relation_type_id_nameable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.party_political_entity_relation_type
    ADD CONSTRAINT fk_party_political_entity_relation_type_id_nameable FOREIGN KEY (id) REFERENCES public.nameable(id) NOT VALID;


--
-- TOC entry 4827 (class 2606 OID 1910847)
-- Name: party_political_entity_relation fk_party_politicial_entity_relation_id_node; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.party_political_entity_relation
    ADD CONSTRAINT fk_party_politicial_entity_relation_id_node FOREIGN KEY (id) REFERENCES public.node(id) NOT VALID;


--
-- TOC entry 4828 (class 2606 OID 188997)
-- Name: person_organization_relation fk_person_collective_relation_person_collective_relation_type; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.person_organization_relation
    ADD CONSTRAINT fk_person_collective_relation_person_collective_relation_type FOREIGN KEY (person_organization_relation_type_id) REFERENCES public.person_organization_relation_type(id) NOT VALID;


--
-- TOC entry 4728 (class 2606 OID 33023)
-- Name: person fk_person_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.person
    ADD CONSTRAINT fk_person_id FOREIGN KEY (id) REFERENCES public.party(id) NOT VALID;


--
-- TOC entry 4829 (class 2606 OID 189003)
-- Name: person_organization_relation fk_person_organization_relation_document; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.person_organization_relation
    ADD CONSTRAINT fk_person_organization_relation_document FOREIGN KEY (document_id_proof) REFERENCES public.document(id) NOT VALID;


--
-- TOC entry 4830 (class 2606 OID 1910852)
-- Name: person_organization_relation fk_person_organization_relation_id_node; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.person_organization_relation
    ADD CONSTRAINT fk_person_organization_relation_id_node FOREIGN KEY (id) REFERENCES public.node(id) NOT VALID;


--
-- TOC entry 4831 (class 2606 OID 188985)
-- Name: person_organization_relation fk_person_organization_relation_person; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.person_organization_relation
    ADD CONSTRAINT fk_person_organization_relation_person FOREIGN KEY (person_id) REFERENCES public.person(id) NOT VALID;


--
-- TOC entry 4717 (class 2606 OID 188285)
-- Name: person_organization_relation_type fk_person_organization_relation_type_id_nameable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.person_organization_relation_type
    ADD CONSTRAINT fk_person_organization_relation_type_id_nameable FOREIGN KEY (id) REFERENCES public.nameable(id) NOT VALID;


--
-- TOC entry 4840 (class 2606 OID 189190)
-- Name: placement_agency fk_placement_agency_id_organizational_role; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.placement_agency
    ADD CONSTRAINT fk_placement_agency_id_organizational_role FOREIGN KEY (id) REFERENCES public.organizational_role(id) NOT VALID;


--
-- TOC entry 4742 (class 2606 OID 66748)
-- Name: political_entity fk_political_entity_file_flag; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.political_entity
    ADD CONSTRAINT fk_political_entity_file_flag FOREIGN KEY (file_id_flag) REFERENCES public.file(id) NOT VALID;


--
-- TOC entry 4743 (class 2606 OID 48002)
-- Name: political_entity fk_political_entity_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.political_entity
    ADD CONSTRAINT fk_political_entity_id FOREIGN KEY (id) REFERENCES public.geographical_entity(id) NOT VALID;


--
-- TOC entry 4937 (class 2606 OID 2708823)
-- Name: poll fk_poll_node; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.poll
    ADD CONSTRAINT fk_poll_node FOREIGN KEY (id) REFERENCES public.node(id) NOT VALID;


--
-- TOC entry 4939 (class 2606 OID 2708818)
-- Name: poll_option fk_poll_option_pole_question; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.poll_option
    ADD CONSTRAINT fk_poll_option_pole_question FOREIGN KEY (poll_question_id) REFERENCES public.poll_question(id) NOT VALID;


--
-- TOC entry 4938 (class 2606 OID 2653950)
-- Name: poll fk_poll_poll_status; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.poll
    ADD CONSTRAINT fk_poll_poll_status FOREIGN KEY (poll_status_id) REFERENCES public.poll_status(id) NOT VALID;


--
-- TOC entry 4940 (class 2606 OID 2653914)
-- Name: poll_vote fk_poll_vote_poll_option; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.poll_vote
    ADD CONSTRAINT fk_poll_vote_poll_option FOREIGN KEY (poll_id, delta) REFERENCES public.poll_option(poll_question_id, delta) NOT VALID;


--
-- TOC entry 4941 (class 2606 OID 2653921)
-- Name: poll_vote fk_poll_vote_user; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.poll_vote
    ADD CONSTRAINT fk_poll_vote_user FOREIGN KEY (user_id) REFERENCES public."user"(id) NOT VALID;


--
-- TOC entry 4845 (class 2606 OID 189245)
-- Name: post_placement_agency fk_post_placement_agency_id_organizational_role; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.post_placement_agency
    ADD CONSTRAINT fk_post_placement_agency_id_organizational_role FOREIGN KEY (id) REFERENCES public.organizational_role(id) NOT VALID;


--
-- TOC entry 4719 (class 2606 OID 189308)
-- Name: profession fk_profession_id_nameable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.profession
    ADD CONSTRAINT fk_profession_id_nameable FOREIGN KEY (id) REFERENCES public.nameable(id) NOT VALID;


--
-- TOC entry 4836 (class 2606 OID 3044746)
-- Name: professional_role fk_professional_role_person; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.professional_role
    ADD CONSTRAINT fk_professional_role_person FOREIGN KEY (person_id) REFERENCES public.person(id) NOT VALID;


--
-- TOC entry 4837 (class 2606 OID 189144)
-- Name: professional_role fk_professional_role_profession; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.professional_role
    ADD CONSTRAINT fk_professional_role_profession FOREIGN KEY (profession_id) REFERENCES public.profession(id) NOT VALID;


--
-- TOC entry 4870 (class 2606 OID 545074)
-- Name: publisher fk_publisher_id_principal; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.publisher
    ADD CONSTRAINT fk_publisher_id_principal FOREIGN KEY (id) REFERENCES public.principal(id) NOT VALID;


--
-- TOC entry 4882 (class 2606 OID 575908)
-- Name: representative_house_bill_action fk_representative_house_bill_action_bill_action_type; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.representative_house_bill_action
    ADD CONSTRAINT fk_representative_house_bill_action_bill_action_type FOREIGN KEY (bill_action_type_id) REFERENCES public.bill_action_type(id) NOT VALID;


--
-- TOC entry 4883 (class 2606 OID 575902)
-- Name: representative_house_bill_action fk_representative_house_bill_action_house_bill; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.representative_house_bill_action
    ADD CONSTRAINT fk_representative_house_bill_action_house_bill FOREIGN KEY (house_bill_id) REFERENCES public.house_bill(id) NOT VALID;


--
-- TOC entry 4884 (class 2606 OID 575896)
-- Name: representative_house_bill_action fk_representative_house_bill_action_representative; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.representative_house_bill_action
    ADD CONSTRAINT fk_representative_house_bill_action_representative FOREIGN KEY (representative_id) REFERENCES public.representative(id) NOT VALID;


--
-- TOC entry 4879 (class 2606 OID 575857)
-- Name: representative fk_representative_member_of_congress; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.representative
    ADD CONSTRAINT fk_representative_member_of_congress FOREIGN KEY (id) REFERENCES public.member_of_congress(id) NOT VALID;


--
-- TOC entry 4859 (class 2606 OID 403710)
-- Name: review fk_review_id_simple_text_node; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.review
    ADD CONSTRAINT fk_review_id_simple_text_node FOREIGN KEY (id) REFERENCES public.simple_text_node(id) NOT VALID;


--
-- TOC entry 4921 (class 2606 OID 1910335)
-- Name: searchable fk_searchable_node; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.searchable
    ADD CONSTRAINT fk_searchable_node FOREIGN KEY (id) REFERENCES public.node(id) NOT VALID;


--
-- TOC entry 4714 (class 2606 OID 35222)
-- Name: second_level_global_region fk_second_level_global_region_first_level_global_region; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.second_level_global_region
    ADD CONSTRAINT fk_second_level_global_region_first_level_global_region FOREIGN KEY (first_level_global_region_id) REFERENCES public.first_level_global_region(id) NOT VALID;


--
-- TOC entry 4715 (class 2606 OID 48025)
-- Name: second_level_global_region fk_second_level_global_region_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.second_level_global_region
    ADD CONSTRAINT fk_second_level_global_region_id FOREIGN KEY (id) REFERENCES public.global_region(id) NOT VALID;


--
-- TOC entry 4751 (class 2606 OID 58162)
-- Name: second_level_subdivision fk_second_level_subdivision_id_bottom_level_subdivision; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.second_level_subdivision
    ADD CONSTRAINT fk_second_level_subdivision_id_bottom_level_subdivision FOREIGN KEY (id) REFERENCES public.bottom_level_subdivision(id) NOT VALID;


--
-- TOC entry 4752 (class 2606 OID 48082)
-- Name: second_level_subdivision fk_second_level_subdivision_id_iso_coded_subdivision; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.second_level_subdivision
    ADD CONSTRAINT fk_second_level_subdivision_id_iso_coded_subdivision FOREIGN KEY (id) REFERENCES public.iso_coded_subdivision(id) NOT VALID;


--
-- TOC entry 4877 (class 2606 OID 636068)
-- Name: senate_bill fk_senate_bill_id_bill; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.senate_bill
    ADD CONSTRAINT fk_senate_bill_id_bill FOREIGN KEY (id) REFERENCES public.bill(id) NOT VALID;


--
-- TOC entry 4951 (class 2606 OID 3745449)
-- Name: senate_term fk_senate_term_id_congressional_term; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.senate_term
    ADD CONSTRAINT fk_senate_term_id_congressional_term FOREIGN KEY (id) REFERENCES public.congressional_term(id) NOT VALID;


--
-- TOC entry 4952 (class 2606 OID 3683999)
-- Name: senate_term fk_senate_term_senator; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.senate_term
    ADD CONSTRAINT fk_senate_term_senator FOREIGN KEY (senator_id) REFERENCES public.senator(id) NOT VALID;


--
-- TOC entry 4953 (class 2606 OID 3684005)
-- Name: senate_term fk_senate_term_subdivision; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.senate_term
    ADD CONSTRAINT fk_senate_term_subdivision FOREIGN KEY (subdivision_id) REFERENCES public.subdivision(id) NOT VALID;


--
-- TOC entry 4880 (class 2606 OID 575868)
-- Name: senator fk_senator_member_of_congress; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.senator
    ADD CONSTRAINT fk_senator_member_of_congress FOREIGN KEY (id) REFERENCES public.member_of_congress(id) NOT VALID;


--
-- TOC entry 4885 (class 2606 OID 3081063)
-- Name: senator_senate_bill_action fk_senator_senate_bill_action_bill_action_type; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.senator_senate_bill_action
    ADD CONSTRAINT fk_senator_senate_bill_action_bill_action_type FOREIGN KEY (bill_action_type_id) REFERENCES public.bill_action_type(id);


--
-- TOC entry 4886 (class 2606 OID 575926)
-- Name: senator_senate_bill_action fk_senator_senate_bill_action_senate_bill; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.senator_senate_bill_action
    ADD CONSTRAINT fk_senator_senate_bill_action_senate_bill FOREIGN KEY (senate_bill_id) REFERENCES public.senate_bill(id);


--
-- TOC entry 4887 (class 2606 OID 575931)
-- Name: senator_senate_bill_action fk_senator_senate_bill_action_senator; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.senator_senate_bill_action
    ADD CONSTRAINT fk_senator_senate_bill_action_senator FOREIGN KEY (senator_id) REFERENCES public.senator(id);


--
-- TOC entry 4834 (class 2606 OID 1910812)
-- Name: simple_text_node fk_simple_text_node_id_searchable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.simple_text_node
    ADD CONSTRAINT fk_simple_text_node_id_searchable FOREIGN KEY (id) REFERENCES public.searchable(id) NOT VALID;


--
-- TOC entry 4942 (class 2606 OID 2708788)
-- Name: single_question_poll fk_single_question_poll_id_poll; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.single_question_poll
    ADD CONSTRAINT fk_single_question_poll_id_poll FOREIGN KEY (id) REFERENCES public.poll(id) NOT VALID;


--
-- TOC entry 4943 (class 2606 OID 2708812)
-- Name: single_question_poll fk_single_question_poll_id_poll_question; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.single_question_poll
    ADD CONSTRAINT fk_single_question_poll_id_poll_question FOREIGN KEY (id) REFERENCES public.poll_question(id) NOT VALID;


--
-- TOC entry 4759 (class 2606 OID 43493)
-- Name: subdivision fk_subdivision_country; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.subdivision
    ADD CONSTRAINT fk_subdivision_country FOREIGN KEY (country_id) REFERENCES public.country(id) NOT VALID;


--
-- TOC entry 4760 (class 2606 OID 960334)
-- Name: subdivision fk_subdivision_country_subdivision; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.subdivision
    ADD CONSTRAINT fk_subdivision_country_subdivision FOREIGN KEY (country_id, subdivision_type_id) REFERENCES public.country_subdivision_type(country_id, subdivision_type_id) NOT VALID;


--
-- TOC entry 4761 (class 2606 OID 920804)
-- Name: subdivision fk_subdivision_id_geographical_entity; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.subdivision
    ADD CONSTRAINT fk_subdivision_id_geographical_entity FOREIGN KEY (id) REFERENCES public.geographical_entity(id) NOT VALID;


--
-- TOC entry 4762 (class 2606 OID 964518)
-- Name: subdivision fk_subdivision_subdivision_type; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.subdivision
    ADD CONSTRAINT fk_subdivision_subdivision_type FOREIGN KEY (subdivision_type_id) REFERENCES public.subdivision_type(id) NOT VALID;


--
-- TOC entry 4913 (class 2606 OID 958475)
-- Name: subdivision_type fk_subdivision_type_nameable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.subdivision_type
    ADD CONSTRAINT fk_subdivision_type_nameable FOREIGN KEY (id) REFERENCES public.nameable(id) NOT VALID;


--
-- TOC entry 4864 (class 2606 OID 544979)
-- Name: subgroup fk_subgroup_id_user_group; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.subgroup
    ADD CONSTRAINT fk_subgroup_id_user_group FOREIGN KEY (id) REFERENCES public.user_group(id) NOT VALID;


--
-- TOC entry 4865 (class 2606 OID 544984)
-- Name: subgroup fk_subgroup_tenant; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.subgroup
    ADD CONSTRAINT fk_subgroup_tenant FOREIGN KEY (tenant_id) REFERENCES public.tenant(id) NOT VALID;


--
-- TOC entry 4920 (class 2606 OID 1875513)
-- Name: system_group fk_system_group_user_group; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.system_group
    ADD CONSTRAINT fk_system_group_user_group FOREIGN KEY (id) REFERENCES public.user_group(id) NOT VALID;


--
-- TOC entry 4861 (class 2606 OID 1855313)
-- Name: tenant fk_tenant_access_role_id_not_logged_in; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tenant
    ADD CONSTRAINT fk_tenant_access_role_id_not_logged_in FOREIGN KEY (access_role_id_not_logged_in) REFERENCES public.access_role(id) DEFERRABLE INITIALLY DEFERRED NOT VALID;


--
-- TOC entry 4933 (class 2606 OID 2383883)
-- Name: tenant_file fk_tenant_file_file; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tenant_file
    ADD CONSTRAINT fk_tenant_file_file FOREIGN KEY (file_id) REFERENCES public.file(id) NOT VALID;


--
-- TOC entry 4934 (class 2606 OID 2383877)
-- Name: tenant_file fk_tenant_file_tenant; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tenant_file
    ADD CONSTRAINT fk_tenant_file_tenant FOREIGN KEY (tenant_id) REFERENCES public.tenant(id) NOT VALID;


--
-- TOC entry 4862 (class 2606 OID 545113)
-- Name: tenant fk_tenant_id_owner; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tenant
    ADD CONSTRAINT fk_tenant_id_owner FOREIGN KEY (id) REFERENCES public.owner(id) NOT VALID;


--
-- TOC entry 4900 (class 2606 OID 717706)
-- Name: tenant_node_menu_item fk_tenant_node_menu_item_id_menu_item; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tenant_node_menu_item
    ADD CONSTRAINT fk_tenant_node_menu_item_id_menu_item FOREIGN KEY (id) REFERENCES public.menu_item(id) NOT VALID;


--
-- TOC entry 4901 (class 2606 OID 717712)
-- Name: tenant_node_menu_item fk_tenant_node_menu_item_tenant_node; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tenant_node_menu_item
    ADD CONSTRAINT fk_tenant_node_menu_item_tenant_node FOREIGN KEY (tenant_node_id) REFERENCES public.tenant_node(id) NOT VALID;


--
-- TOC entry 4866 (class 2606 OID 545029)
-- Name: tenant_node fk_tenant_node_node; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tenant_node
    ADD CONSTRAINT fk_tenant_node_node FOREIGN KEY (node_id) REFERENCES public.node(id) NOT VALID;


--
-- TOC entry 4867 (class 2606 OID 545041)
-- Name: tenant_node fk_tenant_node_publication_status; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tenant_node
    ADD CONSTRAINT fk_tenant_node_publication_status FOREIGN KEY (publication_status_id) REFERENCES public.publication_status(id) NOT VALID;


--
-- TOC entry 4868 (class 2606 OID 545035)
-- Name: tenant_node fk_tenant_node_subgroup; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tenant_node
    ADD CONSTRAINT fk_tenant_node_subgroup FOREIGN KEY (subgroup_id) REFERENCES public.subgroup(id) NOT VALID;


--
-- TOC entry 4869 (class 2606 OID 545023)
-- Name: tenant_node fk_tenant_node_tenant; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tenant_node
    ADD CONSTRAINT fk_tenant_node_tenant FOREIGN KEY (tenant_id) REFERENCES public.tenant(id) NOT VALID;


--
-- TOC entry 4863 (class 2606 OID 545049)
-- Name: tenant fk_tenant_vocabulary_tagging; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tenant
    ADD CONSTRAINT fk_tenant_vocabulary_tagging FOREIGN KEY (vocabulary_id_tagging) REFERENCES public.vocabulary(id) NOT VALID;


--
-- TOC entry 4746 (class 2606 OID 188229)
-- Name: term_hierarchy fk_term_hierarchy_child; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.term_hierarchy
    ADD CONSTRAINT fk_term_hierarchy_child FOREIGN KEY (term_id_child) REFERENCES public.term(id) NOT VALID;


--
-- TOC entry 4747 (class 2606 OID 188234)
-- Name: term_hierarchy fk_term_hierarchy_parent; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.term_hierarchy
    ADD CONSTRAINT fk_term_hierarchy_parent FOREIGN KEY (term_id_parent) REFERENCES public.term(id) NOT VALID;


--
-- TOC entry 4821 (class 2606 OID 188279)
-- Name: term fk_term_nameable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.term
    ADD CONSTRAINT fk_term_nameable FOREIGN KEY (nameable_id) REFERENCES public.nameable(id) NOT VALID;


--
-- TOC entry 4822 (class 2606 OID 188274)
-- Name: term fk_term_vocabulary; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.term
    ADD CONSTRAINT fk_term_vocabulary FOREIGN KEY (vocabulary_id) REFERENCES public.vocabulary(id) NOT VALID;


--
-- TOC entry 4757 (class 2606 OID 51557)
-- Name: top_level_country fk_top_level_country_global_region; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.top_level_country
    ADD CONSTRAINT fk_top_level_country_global_region FOREIGN KEY (global_region_id) REFERENCES public.global_region(id) NOT VALID;


--
-- TOC entry 4758 (class 2606 OID 37404)
-- Name: top_level_country fk_top_level_country_id_country; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.top_level_country
    ADD CONSTRAINT fk_top_level_country_id_country FOREIGN KEY (id) REFERENCES public.country(id) NOT VALID;


--
-- TOC entry 4846 (class 2606 OID 189256)
-- Name: type_of_abuse fk_type_of_abuse_id_nameable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.type_of_abuse
    ADD CONSTRAINT fk_type_of_abuse_id_nameable FOREIGN KEY (id) REFERENCES public.nameable(id) NOT VALID;


--
-- TOC entry 4847 (class 2606 OID 189277)
-- Name: type_of_abuser fk_type_of_abuser_id_nameable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.type_of_abuser
    ADD CONSTRAINT fk_type_of_abuser_id_nameable FOREIGN KEY (id) REFERENCES public.nameable(id) NOT VALID;


--
-- TOC entry 4916 (class 2606 OID 1003379)
-- Name: united_states_congressional_meeting fk_united_states_congressional_meeting_documentable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.united_states_congressional_meeting
    ADD CONSTRAINT fk_united_states_congressional_meeting_documentable FOREIGN KEY (id) REFERENCES public.documentable(id) NOT VALID;


--
-- TOC entry 4917 (class 2606 OID 1003385)
-- Name: united_states_congressional_meeting fk_united_states_congressional_meeting_nameable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.united_states_congressional_meeting
    ADD CONSTRAINT fk_united_states_congressional_meeting_nameable FOREIGN KEY (id) REFERENCES public.nameable(id) NOT VALID;


--
-- TOC entry 4949 (class 2606 OID 3684055)
-- Name: united_states_political_party_affiliation fk_united_states_politcal_party_affiliation_united_states_polit; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.united_states_political_party_affiliation
    ADD CONSTRAINT fk_united_states_politcal_party_affiliation_united_states_polit FOREIGN KEY (united_states_political_party_id) REFERENCES public.united_states_political_party(id) NOT VALID;


--
-- TOC entry 4950 (class 2606 OID 3684081)
-- Name: united_states_political_party_affiliation fk_united_states_political_party_affiliation_id_nameable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.united_states_political_party_affiliation
    ADD CONSTRAINT fk_united_states_political_party_affiliation_id_nameable FOREIGN KEY (id) REFERENCES public.nameable(id) NOT VALID;


--
-- TOC entry 4957 (class 2606 OID 3684049)
-- Name: united_states_political_party fk_united_states_political_party_id_organization; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.united_states_political_party
    ADD CONSTRAINT fk_united_states_political_party_id_organization FOREIGN KEY (id) REFERENCES public.organization(id) NOT VALID;


--
-- TOC entry 4860 (class 2606 OID 1875001)
-- Name: user_group fk_user_group_administrator_role_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.user_group
    ADD CONSTRAINT fk_user_group_administrator_role_id FOREIGN KEY (administrator_role_id) REFERENCES public.administrator_role(id) DEFERRABLE INITIALLY DEFERRED NOT VALID;


--
-- TOC entry 4851 (class 2606 OID 545009)
-- Name: user_group_user_role_user fk_user_group_user_role_user_user; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.user_group_user_role_user
    ADD CONSTRAINT fk_user_group_user_role_user_user FOREIGN KEY (user_id) REFERENCES public."user"(id) NOT VALID;


--
-- TOC entry 4852 (class 2606 OID 545015)
-- Name: user_group_user_role_user fk_user_group_user_role_user_user_group; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.user_group_user_role_user
    ADD CONSTRAINT fk_user_group_user_role_user_user_group FOREIGN KEY (user_group_id) REFERENCES public.user_group(id) NOT VALID;


--
-- TOC entry 4853 (class 2606 OID 545003)
-- Name: user_group_user_role_user fk_user_group_user_role_user_user_role; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.user_group_user_role_user
    ADD CONSTRAINT fk_user_group_user_role_user_user_role FOREIGN KEY (user_role_id) REFERENCES public.user_role(id) NOT VALID;


--
-- TOC entry 4702 (class 2606 OID 545080)
-- Name: user fk_user_id_publisher; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."user"
    ADD CONSTRAINT fk_user_id_publisher FOREIGN KEY (id) REFERENCES public.publisher(id) NOT VALID;


--
-- TOC entry 4849 (class 2606 OID 1855250)
-- Name: user_role fk_user_role_id_principal; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.user_role
    ADD CONSTRAINT fk_user_role_id_principal FOREIGN KEY (id) REFERENCES public.principal(id) NOT VALID;


--
-- TOC entry 4850 (class 2606 OID 1745746)
-- Name: user_role fk_user_role_user_group; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.user_role
    ADD CONSTRAINT fk_user_role_user_group FOREIGN KEY (user_group_id) REFERENCES public.user_group(id) NOT VALID;


--
-- TOC entry 4820 (class 2606 OID 188205)
-- Name: vocabulary fk_vocabulary_id_node; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.vocabulary
    ADD CONSTRAINT fk_vocabulary_id_node FOREIGN KEY (id) REFERENCES public.node(id) NOT VALID;


--
-- TOC entry 4803 (class 2606 OID 69140)
-- Name: wrongful_medication_case fk_wrongful_medication_case_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.wrongful_medication_case
    ADD CONSTRAINT fk_wrongful_medication_case_id FOREIGN KEY (id) REFERENCES public."case"(id) NOT VALID;


--
-- TOC entry 4804 (class 2606 OID 69151)
-- Name: wrongful_removal_case fk_wrongful_removal_case_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.wrongful_removal_case
    ADD CONSTRAINT fk_wrongful_removal_case_id FOREIGN KEY (id) REFERENCES public."case"(id) NOT VALID;


--
-- TOC entry 4734 (class 2606 OID 1910837)
-- Name: inter_organizational_relation inter_organizational_relation_id_node; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.inter_organizational_relation
    ADD CONSTRAINT inter_organizational_relation_id_node FOREIGN KEY (id) REFERENCES public.node(id) NOT VALID;


--
-- TOC entry 4832 (class 2606 OID 1254867)
-- Name: person_organization_relation person_organization_relation_geographical_entity; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.person_organization_relation
    ADD CONSTRAINT person_organization_relation_geographical_entity FOREIGN KEY (geographical_entity_id) REFERENCES public.geographical_entity(id) NOT VALID;


--
-- TOC entry 4833 (class 2606 OID 189109)
-- Name: person_organization_relation person_organization_relation_organization; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.person_organization_relation
    ADD CONSTRAINT person_organization_relation_organization FOREIGN KEY (organization_id) REFERENCES public.organization(id) NOT VALID;


--
-- TOC entry 4946 (class 2606 OID 2708833)
-- Name: poll_question tk_poll_question_id_simple_text_node; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.poll_question
    ADD CONSTRAINT tk_poll_question_id_simple_text_node FOREIGN KEY (id) REFERENCES public.simple_text_node(id) NOT VALID;


--
-- TOC entry 5105 (class 0 OID 0)
-- Dependencies: 6
-- Name: SCHEMA public; Type: ACL; Schema: -; Owner: postgres
--

REVOKE USAGE ON SCHEMA public FROM PUBLIC;
GRANT ALL ON SCHEMA public TO PUBLIC;


-- Completed on 2023-02-28 16:09:56

--
-- PostgreSQL database dump complete
--

