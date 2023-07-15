--
-- PostgreSQL database dump
--

-- Dumped from database version 15.2 (Ubuntu 15.2-1.pgdg22.04+1)
-- Dumped by pg_dump version 15.2

-- Started on 2023-05-07 12:28:24

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
-- TOC entry 7 (class 2615 OID 2200)
-- Name: public; Type: SCHEMA; Schema: -; Owner: postgres
--

-- *not* creating schema, since initdb creates it


ALTER SCHEMA public OWNER TO postgres;

--
-- TOC entry 3 (class 3079 OID 16390)
-- Name: btree_gist; Type: EXTENSION; Schema: -; Owner: -
--

CREATE EXTENSION IF NOT EXISTS btree_gist WITH SCHEMA public;


--
-- TOC entry 5242 (class 0 OID 0)
-- Dependencies: 3
-- Name: EXTENSION btree_gist; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION btree_gist IS 'support for indexing common datatypes in GiST';


--
-- TOC entry 2 (class 3079 OID 17040)
-- Name: pg_trgm; Type: EXTENSION; Schema: -; Owner: -
--

CREATE EXTENSION IF NOT EXISTS pg_trgm WITH SCHEMA public;


--
-- TOC entry 5243 (class 0 OID 0)
-- Dependencies: 2
-- Name: EXTENSION pg_trgm; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION pg_trgm IS 'text similarity measurement and index searching based on trigrams';


--
-- TOC entry 1345 (class 1247 OID 570396)
-- Name: fuzzy_date; Type: DOMAIN; Schema: public; Owner: niels
--

CREATE DOMAIN public.fuzzy_date AS tsrange
	CONSTRAINT fuzzy_date_is_valid CHECK (((VALUE IS NULL) OR ((date_part('hour'::text, lower(VALUE)) = (0)::double precision) AND (date_part('minute'::text, lower(VALUE)) = (0)::double precision) AND (date_part('second'::text, lower(VALUE)) = (0)::double precision) AND (date_part('hour'::text, upper(VALUE)) = (23)::double precision) AND (date_part('minute'::text, upper(VALUE)) = (59)::double precision) AND (date_part('second'::text, upper(VALUE)) = (59.999)::double precision) AND ((upper(VALUE) = ((lower(VALUE) + '1 day'::interval) - '00:00:00.001'::interval)) OR (upper(VALUE) = ((lower(VALUE) + '1 mon'::interval) - '00:00:00.001'::interval)) OR (upper(VALUE) = ((lower(VALUE) + '1 year'::interval) - '00:00:00.001'::interval))))));


ALTER DOMAIN public.fuzzy_date OWNER TO niels;

--
-- TOC entry 624 (class 1255 OID 17121)
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
-- TOC entry 625 (class 1255 OID 17122)
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
-- TOC entry 216 (class 1259 OID 17123)
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
-- TOC entry 393 (class 1259 OID 2047145)
-- Name: abuse_case_type_of_abuse; Type: TABLE; Schema: public; Owner: niels
--

CREATE TABLE public.abuse_case_type_of_abuse (
    abuse_case_id integer NOT NULL,
    type_of_abuse_id integer NOT NULL
);


ALTER TABLE public.abuse_case_type_of_abuse OWNER TO niels;

--
-- TOC entry 392 (class 1259 OID 2047128)
-- Name: abuse_case_type_of_abuser; Type: TABLE; Schema: public; Owner: niels
--

CREATE TABLE public.abuse_case_type_of_abuser (
    abuse_case_id integer NOT NULL,
    type_of_abuser_id integer NOT NULL
);


ALTER TABLE public.abuse_case_type_of_abuser OWNER TO niels;

--
-- TOC entry 217 (class 1259 OID 17126)
-- Name: access_role; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.access_role (
    id integer NOT NULL
);


ALTER TABLE public.access_role OWNER TO postgres;

--
-- TOC entry 218 (class 1259 OID 17129)
-- Name: access_role_privilege; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.access_role_privilege (
    access_role_id integer NOT NULL,
    action_id integer NOT NULL
);


ALTER TABLE public.access_role_privilege OWNER TO postgres;

--
-- TOC entry 219 (class 1259 OID 17132)
-- Name: act; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.act (
    id integer NOT NULL,
    enactment_date date
);


ALTER TABLE public.act OWNER TO postgres;

--
-- TOC entry 220 (class 1259 OID 17135)
-- Name: action; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.action (
    id integer NOT NULL
);


ALTER TABLE public.action OWNER TO postgres;

--
-- TOC entry 221 (class 1259 OID 17138)
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
-- TOC entry 222 (class 1259 OID 17139)
-- Name: action_menu_item; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.action_menu_item (
    id integer NOT NULL,
    action_id integer NOT NULL,
    name character varying NOT NULL
);


ALTER TABLE public.action_menu_item OWNER TO postgres;

--
-- TOC entry 223 (class 1259 OID 17144)
-- Name: administrator_role; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.administrator_role (
    id integer NOT NULL,
    user_group_id integer NOT NULL
);


ALTER TABLE public.administrator_role OWNER TO postgres;

--
-- TOC entry 224 (class 1259 OID 17147)
-- Name: adoption_lawyer; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.adoption_lawyer (
    id integer NOT NULL
);


ALTER TABLE public.adoption_lawyer OWNER TO postgres;

--
-- TOC entry 225 (class 1259 OID 17153)
-- Name: attachment_therapist; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.attachment_therapist (
    id integer NOT NULL
);


ALTER TABLE public.attachment_therapist OWNER TO postgres;

--
-- TOC entry 226 (class 1259 OID 17156)
-- Name: basic_action; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.basic_action (
    id integer NOT NULL,
    path character varying(255) NOT NULL,
    description character varying NOT NULL
);


ALTER TABLE public.basic_action OWNER TO postgres;

--
-- TOC entry 227 (class 1259 OID 17161)
-- Name: basic_country; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.basic_country (
    id integer NOT NULL
);


ALTER TABLE public.basic_country OWNER TO postgres;

--
-- TOC entry 228 (class 1259 OID 17164)
-- Name: basic_first_and_second_level_subdivision; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.basic_first_and_second_level_subdivision (
    id integer NOT NULL
);


ALTER TABLE public.basic_first_and_second_level_subdivision OWNER TO postgres;

--
-- TOC entry 229 (class 1259 OID 17167)
-- Name: basic_nameable; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.basic_nameable (
    id integer NOT NULL
);


ALTER TABLE public.basic_nameable OWNER TO postgres;

--
-- TOC entry 230 (class 1259 OID 17170)
-- Name: basic_second_level_subdivision; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.basic_second_level_subdivision (
    id integer NOT NULL,
    intermediate_level_subdivision_id integer NOT NULL
);


ALTER TABLE public.basic_second_level_subdivision OWNER TO postgres;

--
-- TOC entry 231 (class 1259 OID 17173)
-- Name: bill; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.bill (
    id integer NOT NULL,
    introduction_date date,
    act_id integer
);


ALTER TABLE public.bill OWNER TO postgres;

--
-- TOC entry 232 (class 1259 OID 17176)
-- Name: bill_action_type; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.bill_action_type (
    id integer NOT NULL
);


ALTER TABLE public.bill_action_type OWNER TO postgres;

--
-- TOC entry 233 (class 1259 OID 17179)
-- Name: binding_country; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.binding_country (
    id integer NOT NULL
);


ALTER TABLE public.binding_country OWNER TO postgres;

--
-- TOC entry 234 (class 1259 OID 17182)
-- Name: blog_post; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.blog_post (
    id integer NOT NULL
);


ALTER TABLE public.blog_post OWNER TO postgres;

--
-- TOC entry 235 (class 1259 OID 17185)
-- Name: bottom_level_subdivision; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.bottom_level_subdivision (
    id integer NOT NULL
);


ALTER TABLE public.bottom_level_subdivision OWNER TO postgres;

--
-- TOC entry 236 (class 1259 OID 17188)
-- Name: bound_country; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.bound_country (
    id integer NOT NULL,
    binding_country_id integer NOT NULL
);


ALTER TABLE public.bound_country OWNER TO postgres;

--
-- TOC entry 237 (class 1259 OID 17191)
-- Name: case; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."case" (
    id integer NOT NULL,
    description character varying NOT NULL,
    fuzzy_date public.fuzzy_date
);


ALTER TABLE public."case" OWNER TO postgres;

--
-- TOC entry 238 (class 1259 OID 17196)
-- Name: case_case_parties; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.case_case_parties (
    case_id integer NOT NULL,
    case_parties_id integer NOT NULL,
    case_party_type_id integer NOT NULL
);


ALTER TABLE public.case_case_parties OWNER TO postgres;

--
-- TOC entry 239 (class 1259 OID 17199)
-- Name: case_parties; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.case_parties (
    id integer NOT NULL,
    organizations character varying,
    persons character varying
);


ALTER TABLE public.case_parties OWNER TO postgres;

--
-- TOC entry 240 (class 1259 OID 17204)
-- Name: case_parties_organization; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.case_parties_organization (
    case_parties_id integer NOT NULL,
    organization_id integer NOT NULL
);


ALTER TABLE public.case_parties_organization OWNER TO postgres;

--
-- TOC entry 241 (class 1259 OID 17207)
-- Name: case_parties_person; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.case_parties_person (
    case_parties_id integer NOT NULL,
    person_id integer NOT NULL
);


ALTER TABLE public.case_parties_person OWNER TO postgres;

--
-- TOC entry 242 (class 1259 OID 17210)
-- Name: case_party_type; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.case_party_type (
    id integer NOT NULL
);


ALTER TABLE public.case_party_type OWNER TO postgres;

--
-- TOC entry 243 (class 1259 OID 17213)
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
-- TOC entry 244 (class 1259 OID 17214)
-- Name: case_type; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.case_type (
    id integer NOT NULL,
    text character varying NOT NULL
);


ALTER TABLE public.case_type OWNER TO postgres;

--
-- TOC entry 245 (class 1259 OID 17217)
-- Name: case_type_case_party_type; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.case_type_case_party_type (
    case_type_id integer NOT NULL,
    case_party_type_id integer NOT NULL
);


ALTER TABLE public.case_type_case_party_type OWNER TO postgres;

--
-- TOC entry 246 (class 1259 OID 17220)
-- Name: child_placement_type; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.child_placement_type (
    id integer NOT NULL
);


ALTER TABLE public.child_placement_type OWNER TO postgres;

--
-- TOC entry 247 (class 1259 OID 17223)
-- Name: child_trafficking_case; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.child_trafficking_case (
    id integer NOT NULL,
    number_of_children_involved integer,
    country_id_from integer NOT NULL
);


ALTER TABLE public.child_trafficking_case OWNER TO postgres;

--
-- TOC entry 248 (class 1259 OID 17226)
-- Name: coerced_adoption_case; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.coerced_adoption_case (
    id integer NOT NULL
);


ALTER TABLE public.coerced_adoption_case OWNER TO postgres;

--
-- TOC entry 249 (class 1259 OID 17229)
-- Name: collective; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.collective (
    id integer NOT NULL
);


ALTER TABLE public.collective OWNER TO postgres;

--
-- TOC entry 250 (class 1259 OID 17232)
-- Name: collective_user; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.collective_user (
    collective_id integer NOT NULL,
    user_id integer NOT NULL
);


ALTER TABLE public.collective_user OWNER TO postgres;

--
-- TOC entry 251 (class 1259 OID 17235)
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
-- TOC entry 252 (class 1259 OID 17240)
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
-- TOC entry 253 (class 1259 OID 17241)
-- Name: congressional_term; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.congressional_term (
    id integer NOT NULL
);


ALTER TABLE public.congressional_term OWNER TO postgres;

--
-- TOC entry 254 (class 1259 OID 17244)
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
-- TOC entry 255 (class 1259 OID 17249)
-- Name: content_sharing_group; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.content_sharing_group (
    id integer NOT NULL
);


ALTER TABLE public.content_sharing_group OWNER TO postgres;

--
-- TOC entry 256 (class 1259 OID 17252)
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
    other_requirements character varying,
    vocabulary_id_subdivisions integer NOT NULL
);


ALTER TABLE public.country OWNER TO postgres;

--
-- TOC entry 257 (class 1259 OID 17257)
-- Name: country_and_first_and_bottom_level_subdivision; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.country_and_first_and_bottom_level_subdivision (
    id integer NOT NULL
);


ALTER TABLE public.country_and_first_and_bottom_level_subdivision OWNER TO postgres;

--
-- TOC entry 258 (class 1259 OID 17260)
-- Name: country_and_first_and_second_level_subdivision; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.country_and_first_and_second_level_subdivision (
    id integer NOT NULL
);


ALTER TABLE public.country_and_first_and_second_level_subdivision OWNER TO postgres;

--
-- TOC entry 259 (class 1259 OID 17263)
-- Name: country_and_first_level_subdivision; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.country_and_first_level_subdivision (
    id integer NOT NULL
);


ALTER TABLE public.country_and_first_level_subdivision OWNER TO postgres;

--
-- TOC entry 260 (class 1259 OID 17266)
-- Name: country_and_intermediate_level_subdivision; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.country_and_intermediate_level_subdivision (
    id integer NOT NULL
);


ALTER TABLE public.country_and_intermediate_level_subdivision OWNER TO postgres;

--
-- TOC entry 261 (class 1259 OID 17269)
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
-- TOC entry 262 (class 1259 OID 17274)
-- Name: country_subdivision_type; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.country_subdivision_type (
    country_id integer NOT NULL,
    subdivision_type_id integer NOT NULL
);


ALTER TABLE public.country_subdivision_type OWNER TO postgres;

--
-- TOC entry 263 (class 1259 OID 17277)
-- Name: create_node_action; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.create_node_action (
    id integer NOT NULL,
    node_type_id integer NOT NULL
);


ALTER TABLE public.create_node_action OWNER TO postgres;

--
-- TOC entry 264 (class 1259 OID 17280)
-- Name: delete_node_action; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.delete_node_action (
    id integer NOT NULL,
    node_type_id integer NOT NULL
);


ALTER TABLE public.delete_node_action OWNER TO postgres;

--
-- TOC entry 265 (class 1259 OID 17283)
-- Name: denomination; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.denomination (
    id integer NOT NULL
);


ALTER TABLE public.denomination OWNER TO postgres;

--
-- TOC entry 266 (class 1259 OID 17286)
-- Name: deportation_case; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.deportation_case (
    id integer NOT NULL,
    subdivision_id_from integer,
    country_id_to integer
);


ALTER TABLE public.deportation_case OWNER TO postgres;

--
-- TOC entry 267 (class 1259 OID 17289)
-- Name: discussion; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.discussion (
    id integer NOT NULL
);


ALTER TABLE public.discussion OWNER TO postgres;

--
-- TOC entry 268 (class 1259 OID 17292)
-- Name: disrupted_placement_case; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.disrupted_placement_case (
    id integer NOT NULL
);


ALTER TABLE public.disrupted_placement_case OWNER TO postgres;

--
-- TOC entry 269 (class 1259 OID 17295)
-- Name: document; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.document (
    id integer NOT NULL,
    source_url character varying(255),
    document_type_id integer,
    published public.fuzzy_date
);


ALTER TABLE public.document OWNER TO postgres;

--
-- TOC entry 270 (class 1259 OID 17301)
-- Name: document_type; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.document_type (
    id integer NOT NULL
);


ALTER TABLE public.document_type OWNER TO postgres;

--
-- TOC entry 271 (class 1259 OID 17304)
-- Name: documentable; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.documentable (
    id integer NOT NULL
);


ALTER TABLE public.documentable OWNER TO postgres;

--
-- TOC entry 272 (class 1259 OID 17310)
-- Name: edit_node_action; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.edit_node_action (
    id integer NOT NULL,
    node_type_id integer NOT NULL
);


ALTER TABLE public.edit_node_action OWNER TO postgres;

--
-- TOC entry 387 (class 1259 OID 192731)
-- Name: edit_own_node_action; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.edit_own_node_action (
    id integer NOT NULL,
    node_type_id integer NOT NULL
);


ALTER TABLE public.edit_own_node_action OWNER TO postgres;

--
-- TOC entry 273 (class 1259 OID 17313)
-- Name: facilitator; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.facilitator (
    id integer NOT NULL
);


ALTER TABLE public.facilitator OWNER TO postgres;

--
-- TOC entry 274 (class 1259 OID 17316)
-- Name: family_size; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.family_size (
    id integer NOT NULL
);


ALTER TABLE public.family_size OWNER TO postgres;

--
-- TOC entry 275 (class 1259 OID 17319)
-- Name: fathers_rights_violation_case; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.fathers_rights_violation_case (
    id integer NOT NULL
);


ALTER TABLE public.fathers_rights_violation_case OWNER TO postgres;

--
-- TOC entry 276 (class 1259 OID 17322)
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
-- TOC entry 277 (class 1259 OID 17327)
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
-- TOC entry 278 (class 1259 OID 17328)
-- Name: first_and_bottom_level_subdivision; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.first_and_bottom_level_subdivision (
    id integer NOT NULL
);


ALTER TABLE public.first_and_bottom_level_subdivision OWNER TO postgres;

--
-- TOC entry 279 (class 1259 OID 17331)
-- Name: first_and_second_level_subdivision; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.first_and_second_level_subdivision (
    id integer NOT NULL
);


ALTER TABLE public.first_and_second_level_subdivision OWNER TO postgres;

--
-- TOC entry 280 (class 1259 OID 17334)
-- Name: first_level_global_region; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.first_level_global_region (
    id integer NOT NULL
);


ALTER TABLE public.first_level_global_region OWNER TO postgres;

--
-- TOC entry 281 (class 1259 OID 17337)
-- Name: first_level_subdivision; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.first_level_subdivision (
    id integer NOT NULL
);


ALTER TABLE public.first_level_subdivision OWNER TO postgres;

--
-- TOC entry 282 (class 1259 OID 17340)
-- Name: formal_intermediate_level_subdivision; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.formal_intermediate_level_subdivision (
    id integer NOT NULL
);


ALTER TABLE public.formal_intermediate_level_subdivision OWNER TO postgres;

--
-- TOC entry 283 (class 1259 OID 17343)
-- Name: geographical_entity; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.geographical_entity (
    id integer NOT NULL
);


ALTER TABLE public.geographical_entity OWNER TO postgres;

--
-- TOC entry 284 (class 1259 OID 17346)
-- Name: global_region; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.global_region (
    id integer NOT NULL
);


ALTER TABLE public.global_region OWNER TO postgres;

--
-- TOC entry 285 (class 1259 OID 17349)
-- Name: hague_status; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.hague_status (
    id integer NOT NULL
);


ALTER TABLE public.hague_status OWNER TO postgres;

--
-- TOC entry 286 (class 1259 OID 17352)
-- Name: home_study_agency; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.home_study_agency (
    id integer NOT NULL
);


ALTER TABLE public.home_study_agency OWNER TO postgres;

--
-- TOC entry 287 (class 1259 OID 17355)
-- Name: house_bill; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.house_bill (
    id integer NOT NULL
);


ALTER TABLE public.house_bill OWNER TO postgres;

--
-- TOC entry 288 (class 1259 OID 17358)
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
-- TOC entry 289 (class 1259 OID 17363)
-- Name: informal_intermediate_level_subdivision; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.informal_intermediate_level_subdivision (
    id integer NOT NULL
);


ALTER TABLE public.informal_intermediate_level_subdivision OWNER TO postgres;

--
-- TOC entry 290 (class 1259 OID 17366)
-- Name: institution; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.institution (
    id integer NOT NULL
);


ALTER TABLE public.institution OWNER TO postgres;

--
-- TOC entry 291 (class 1259 OID 17369)
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
-- TOC entry 292 (class 1259 OID 17374)
-- Name: inter_country_relation_type; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.inter_country_relation_type (
    id integer NOT NULL,
    is_symmetric boolean NOT NULL
);


ALTER TABLE public.inter_country_relation_type OWNER TO postgres;

--
-- TOC entry 293 (class 1259 OID 17377)
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
-- TOC entry 294 (class 1259 OID 17382)
-- Name: inter_organizational_relation_type; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.inter_organizational_relation_type (
    id integer NOT NULL,
    is_symmetric boolean NOT NULL
);


ALTER TABLE public.inter_organizational_relation_type OWNER TO postgres;

--
-- TOC entry 295 (class 1259 OID 17385)
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
-- TOC entry 296 (class 1259 OID 17390)
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
-- TOC entry 297 (class 1259 OID 17391)
-- Name: inter_personal_relation_type; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.inter_personal_relation_type (
    id integer NOT NULL,
    is_symmetric boolean NOT NULL
);


ALTER TABLE public.inter_personal_relation_type OWNER TO postgres;

--
-- TOC entry 298 (class 1259 OID 17394)
-- Name: intermediate_level_subdivision; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.intermediate_level_subdivision (
    id integer NOT NULL
);


ALTER TABLE public.intermediate_level_subdivision OWNER TO postgres;

--
-- TOC entry 299 (class 1259 OID 17397)
-- Name: iso_coded_first_level_subdivision; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.iso_coded_first_level_subdivision (
    id integer NOT NULL
);


ALTER TABLE public.iso_coded_first_level_subdivision OWNER TO postgres;

--
-- TOC entry 300 (class 1259 OID 17400)
-- Name: iso_coded_subdivision; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.iso_coded_subdivision (
    id integer NOT NULL,
    iso_3166_2_code character varying(10) NOT NULL
);


ALTER TABLE public.iso_coded_subdivision OWNER TO postgres;

--
-- TOC entry 301 (class 1259 OID 17403)
-- Name: law_firm; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.law_firm (
    id integer NOT NULL
);


ALTER TABLE public.law_firm OWNER TO postgres;

--
-- TOC entry 302 (class 1259 OID 17406)
-- Name: layout; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.layout (
    id integer NOT NULL,
    file_name character varying(255) NOT NULL
);


ALTER TABLE public.layout OWNER TO postgres;

--
-- TOC entry 303 (class 1259 OID 17409)
-- Name: locatable; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.locatable (
    id integer NOT NULL
);


ALTER TABLE public.locatable OWNER TO postgres;

--
-- TOC entry 304 (class 1259 OID 17412)
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
-- TOC entry 305 (class 1259 OID 17417)
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
-- TOC entry 306 (class 1259 OID 17418)
-- Name: location_locatable; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.location_locatable (
    location_id integer NOT NULL,
    locatable_id integer NOT NULL
);


ALTER TABLE public.location_locatable OWNER TO postgres;

--
-- TOC entry 307 (class 1259 OID 17421)
-- Name: member_of_congress; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.member_of_congress (
    id integer NOT NULL
);


ALTER TABLE public.member_of_congress OWNER TO postgres;

--
-- TOC entry 308 (class 1259 OID 17424)
-- Name: menu_item; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.menu_item (
    id integer NOT NULL,
    weight double precision
);


ALTER TABLE public.menu_item OWNER TO postgres;

--
-- TOC entry 309 (class 1259 OID 17427)
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
-- TOC entry 310 (class 1259 OID 17428)
-- Name: multi_question_poll; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.multi_question_poll (
    id integer NOT NULL
);


ALTER TABLE public.multi_question_poll OWNER TO postgres;

--
-- TOC entry 311 (class 1259 OID 17431)
-- Name: multi_question_poll_poll_question; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.multi_question_poll_poll_question (
    multi_question_poll_id integer NOT NULL,
    poll_question_id integer NOT NULL,
    delta integer NOT NULL
);


ALTER TABLE public.multi_question_poll_poll_question OWNER TO postgres;

--
-- TOC entry 312 (class 1259 OID 17434)
-- Name: nameable; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.nameable (
    id integer NOT NULL,
    description character varying,
    file_id_tile_image integer
);


ALTER TABLE public.nameable OWNER TO postgres;

--
-- TOC entry 390 (class 1259 OID 1933312)
-- Name: nameable_type; Type: TABLE; Schema: public; Owner: niels
--

CREATE TABLE public.nameable_type (
    id integer NOT NULL,
    tag_label_name character varying NOT NULL
);


ALTER TABLE public.nameable_type OWNER TO niels;

--
-- TOC entry 313 (class 1259 OID 17439)
-- Name: node; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.node (
    id integer NOT NULL,
    publisher_id integer NOT NULL,
    title character varying(128) NOT NULL,
    created_date_time timestamp without time zone NOT NULL,
    changed_date_time timestamp without time zone NOT NULL,
    node_type_id integer NOT NULL,
    owner_id integer NOT NULL
);


ALTER TABLE public.node OWNER TO postgres;

--
-- TOC entry 314 (class 1259 OID 17442)
-- Name: node_file; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.node_file (
    node_id integer NOT NULL,
    file_id integer NOT NULL
);


ALTER TABLE public.node_file OWNER TO postgres;

--
-- TOC entry 315 (class 1259 OID 17445)
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
-- TOC entry 316 (class 1259 OID 17446)
-- Name: node_term; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.node_term (
    node_id integer NOT NULL,
    term_id integer NOT NULL
);


ALTER TABLE public.node_term OWNER TO postgres;

--
-- TOC entry 317 (class 1259 OID 17449)
-- Name: node_type; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.node_type (
    id integer NOT NULL,
    name character varying(255) NOT NULL,
    description character varying NOT NULL,
    author_specific boolean NOT NULL
);


ALTER TABLE public.node_type OWNER TO postgres;

--
-- TOC entry 318 (class 1259 OID 17454)
-- Name: organization; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.organization (
    id integer NOT NULL,
    website_url character varying(255),
    email_address character varying(255),
    established public.fuzzy_date,
    terminated public.fuzzy_date
);


ALTER TABLE public.organization OWNER TO postgres;

--
-- TOC entry 319 (class 1259 OID 17459)
-- Name: organization_act_relation_type; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.organization_act_relation_type (
    id integer NOT NULL
);


ALTER TABLE public.organization_act_relation_type OWNER TO postgres;

--
-- TOC entry 320 (class 1259 OID 17462)
-- Name: organization_organization_type; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.organization_organization_type (
    organization_id integer NOT NULL,
    organization_type_id integer NOT NULL
);


ALTER TABLE public.organization_organization_type OWNER TO postgres;

--
-- TOC entry 321 (class 1259 OID 17465)
-- Name: organization_type; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.organization_type (
    id integer NOT NULL,
    has_concrete_subtype boolean NOT NULL
);


ALTER TABLE public.organization_type OWNER TO postgres;

--
-- TOC entry 322 (class 1259 OID 17468)
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
-- TOC entry 323 (class 1259 OID 17473)
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
-- TOC entry 324 (class 1259 OID 17474)
-- Name: owner; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.owner (
    id integer NOT NULL
);


ALTER TABLE public.owner OWNER TO postgres;

--
-- TOC entry 325 (class 1259 OID 17477)
-- Name: page; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.page (
    id integer NOT NULL
);


ALTER TABLE public.page OWNER TO postgres;

--
-- TOC entry 326 (class 1259 OID 17480)
-- Name: party; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.party (
    id integer NOT NULL
);


ALTER TABLE public.party OWNER TO postgres;

--
-- TOC entry 389 (class 1259 OID 1263429)
-- Name: party_act_relation; Type: TABLE; Schema: public; Owner: niels
--

CREATE TABLE public.party_act_relation (
    id integer NOT NULL,
    act_id integer NOT NULL,
    party_id integer NOT NULL,
    party_act_relation_type_id integer NOT NULL,
    date_range daterange,
    document_id_proof integer
);


ALTER TABLE public.party_act_relation OWNER TO niels;

--
-- TOC entry 388 (class 1259 OID 1263418)
-- Name: party_act_relation_type; Type: TABLE; Schema: public; Owner: niels
--

CREATE TABLE public.party_act_relation_type (
    id integer NOT NULL
);


ALTER TABLE public.party_act_relation_type OWNER TO niels;

--
-- TOC entry 327 (class 1259 OID 17483)
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
-- TOC entry 328 (class 1259 OID 17488)
-- Name: party_political_entity_relation_type; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.party_political_entity_relation_type (
    id integer NOT NULL,
    has_concrete_subtype boolean NOT NULL
);


ALTER TABLE public.party_political_entity_relation_type OWNER TO postgres;

--
-- TOC entry 329 (class 1259 OID 17491)
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
-- TOC entry 330 (class 1259 OID 17496)
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
-- TOC entry 331 (class 1259 OID 17501)
-- Name: person_organization_relation_type; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.person_organization_relation_type (
    id integer NOT NULL,
    has_concrete_subtype boolean NOT NULL
);


ALTER TABLE public.person_organization_relation_type OWNER TO postgres;

--
-- TOC entry 332 (class 1259 OID 17504)
-- Name: placement_agency; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.placement_agency (
    id integer NOT NULL
);


ALTER TABLE public.placement_agency OWNER TO postgres;

--
-- TOC entry 333 (class 1259 OID 17507)
-- Name: political_entity; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.political_entity (
    id integer NOT NULL,
    file_id_flag integer
);


ALTER TABLE public.political_entity OWNER TO postgres;

--
-- TOC entry 334 (class 1259 OID 17510)
-- Name: poll; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.poll (
    id integer NOT NULL,
    poll_status_id integer NOT NULL,
    date_time_closure timestamp without time zone NOT NULL
);


ALTER TABLE public.poll OWNER TO postgres;

--
-- TOC entry 335 (class 1259 OID 17513)
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
-- TOC entry 336 (class 1259 OID 17518)
-- Name: poll_question; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.poll_question (
    id integer NOT NULL,
    question character varying NOT NULL
);


ALTER TABLE public.poll_question OWNER TO postgres;

--
-- TOC entry 337 (class 1259 OID 17523)
-- Name: poll_status; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.poll_status (
    id integer NOT NULL,
    name character varying(255) NOT NULL
);


ALTER TABLE public.poll_status OWNER TO postgres;

--
-- TOC entry 338 (class 1259 OID 17526)
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
-- TOC entry 339 (class 1259 OID 17531)
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
-- TOC entry 340 (class 1259 OID 17532)
-- Name: post_placement_agency; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.post_placement_agency (
    id integer NOT NULL
);


ALTER TABLE public.post_placement_agency OWNER TO postgres;

--
-- TOC entry 341 (class 1259 OID 17535)
-- Name: principal; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.principal (
    id integer NOT NULL
);


ALTER TABLE public.principal OWNER TO postgres;

--
-- TOC entry 342 (class 1259 OID 17538)
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
-- TOC entry 343 (class 1259 OID 17539)
-- Name: profession; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.profession (
    id integer NOT NULL,
    has_concrete_subtype boolean NOT NULL
);


ALTER TABLE public.profession OWNER TO postgres;

--
-- TOC entry 344 (class 1259 OID 17542)
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
-- TOC entry 345 (class 1259 OID 17547)
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
-- TOC entry 346 (class 1259 OID 17548)
-- Name: publication_status; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.publication_status (
    id integer NOT NULL,
    name character varying NOT NULL
);


ALTER TABLE public.publication_status OWNER TO postgres;

--
-- TOC entry 347 (class 1259 OID 17553)
-- Name: publisher; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.publisher (
    id integer NOT NULL,
    name character varying(100) NOT NULL
);


ALTER TABLE public.publisher OWNER TO postgres;

--
-- TOC entry 386 (class 1259 OID 139264)
-- Name: publishing_user_group; Type: TABLE; Schema: public; Owner: niels
--

CREATE TABLE public.publishing_user_group (
    id integer NOT NULL,
    publication_status_id_default integer NOT NULL
);


ALTER TABLE public.publishing_user_group OWNER TO niels;

--
-- TOC entry 348 (class 1259 OID 17556)
-- Name: representative; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.representative (
    id integer NOT NULL
);


ALTER TABLE public.representative OWNER TO postgres;

--
-- TOC entry 349 (class 1259 OID 17559)
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
-- TOC entry 350 (class 1259 OID 17562)
-- Name: review; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.review (
    id integer NOT NULL
);


ALTER TABLE public.review OWNER TO postgres;

--
-- TOC entry 351 (class 1259 OID 17565)
-- Name: searchable; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.searchable (
    id integer NOT NULL,
    tsvector tsvector
);


ALTER TABLE public.searchable OWNER TO postgres;

--
-- TOC entry 352 (class 1259 OID 17570)
-- Name: second_level_global_region; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.second_level_global_region (
    id integer NOT NULL,
    first_level_global_region_id integer NOT NULL
);


ALTER TABLE public.second_level_global_region OWNER TO postgres;

--
-- TOC entry 353 (class 1259 OID 17573)
-- Name: second_level_subdivision; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.second_level_subdivision (
    id integer NOT NULL
);


ALTER TABLE public.second_level_subdivision OWNER TO postgres;

--
-- TOC entry 354 (class 1259 OID 17576)
-- Name: senate_bill; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.senate_bill (
    id integer NOT NULL
);


ALTER TABLE public.senate_bill OWNER TO postgres;

--
-- TOC entry 355 (class 1259 OID 17579)
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
-- TOC entry 356 (class 1259 OID 17584)
-- Name: senator; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.senator (
    id integer NOT NULL
);


ALTER TABLE public.senator OWNER TO postgres;

--
-- TOC entry 357 (class 1259 OID 17587)
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
-- TOC entry 358 (class 1259 OID 17590)
-- Name: simple_text_node; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.simple_text_node (
    id integer NOT NULL,
    text character varying NOT NULL,
    teaser character varying NOT NULL
);


ALTER TABLE public.simple_text_node OWNER TO postgres;

--
-- TOC entry 359 (class 1259 OID 17595)
-- Name: single_question_poll; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.single_question_poll (
    id integer NOT NULL
);


ALTER TABLE public.single_question_poll OWNER TO postgres;

--
-- TOC entry 360 (class 1259 OID 17598)
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
-- TOC entry 361 (class 1259 OID 17601)
-- Name: subdivision_type; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.subdivision_type (
    id integer NOT NULL
);


ALTER TABLE public.subdivision_type OWNER TO postgres;

--
-- TOC entry 362 (class 1259 OID 17604)
-- Name: subgroup; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.subgroup (
    id integer NOT NULL,
    tenant_id integer NOT NULL
);


ALTER TABLE public.subgroup OWNER TO postgres;

--
-- TOC entry 363 (class 1259 OID 17607)
-- Name: system_group; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.system_group (
    id integer NOT NULL
);


ALTER TABLE public.system_group OWNER TO postgres;

--
-- TOC entry 364 (class 1259 OID 17610)
-- Name: tenant; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.tenant (
    id integer NOT NULL,
    vocabulary_id_tagging integer,
    domain_name character varying(255) NOT NULL,
    access_role_id_not_logged_in integer NOT NULL,
    country_id_default integer NOT NULL,
    front_page_text character varying,
    logo character varying,
    sub_title character varying,
    footer_text character varying,
    css_file character varying
);


ALTER TABLE public.tenant OWNER TO postgres;

--
-- TOC entry 365 (class 1259 OID 17613)
-- Name: tenant_file; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.tenant_file (
    tenant_id integer NOT NULL,
    file_id integer NOT NULL,
    tenant_file_id integer NOT NULL
);


ALTER TABLE public.tenant_file OWNER TO postgres;

--
-- TOC entry 366 (class 1259 OID 17616)
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
-- TOC entry 367 (class 1259 OID 17619)
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
-- TOC entry 368 (class 1259 OID 17620)
-- Name: tenant_node_menu_item; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.tenant_node_menu_item (
    id integer NOT NULL,
    tenant_node_id integer NOT NULL,
    name character varying(255) NOT NULL
);


ALTER TABLE public.tenant_node_menu_item OWNER TO postgres;

--
-- TOC entry 369 (class 1259 OID 17623)
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
-- TOC entry 370 (class 1259 OID 17628)
-- Name: term_hierarchy; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.term_hierarchy (
    term_id_parent integer NOT NULL,
    term_id_child integer NOT NULL
);


ALTER TABLE public.term_hierarchy OWNER TO postgres;

--
-- TOC entry 371 (class 1259 OID 17631)
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
-- TOC entry 372 (class 1259 OID 17632)
-- Name: top_level_country; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.top_level_country (
    id integer NOT NULL,
    iso_3166_1_code character(2) NOT NULL,
    global_region_id integer NOT NULL
);


ALTER TABLE public.top_level_country OWNER TO postgres;

--
-- TOC entry 373 (class 1259 OID 17635)
-- Name: type_of_abuse; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.type_of_abuse (
    id integer NOT NULL
);


ALTER TABLE public.type_of_abuse OWNER TO postgres;

--
-- TOC entry 374 (class 1259 OID 17638)
-- Name: type_of_abuser; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.type_of_abuser (
    id integer NOT NULL
);


ALTER TABLE public.type_of_abuser OWNER TO postgres;

--
-- TOC entry 375 (class 1259 OID 17641)
-- Name: united_states_congressional_meeting; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.united_states_congressional_meeting (
    id integer NOT NULL,
    date_range daterange NOT NULL,
    number integer NOT NULL
);


ALTER TABLE public.united_states_congressional_meeting OWNER TO postgres;

--
-- TOC entry 376 (class 1259 OID 17646)
-- Name: united_states_political_party; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.united_states_political_party (
    id integer NOT NULL
);


ALTER TABLE public.united_states_political_party OWNER TO postgres;

--
-- TOC entry 377 (class 1259 OID 17649)
-- Name: united_states_political_party_affiliation; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.united_states_political_party_affiliation (
    id integer NOT NULL,
    united_states_political_party_id integer
);


ALTER TABLE public.united_states_political_party_affiliation OWNER TO postgres;

--
-- TOC entry 378 (class 1259 OID 17652)
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
-- TOC entry 379 (class 1259 OID 17657)
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
-- TOC entry 380 (class 1259 OID 17662)
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
-- TOC entry 381 (class 1259 OID 17663)
-- Name: user_group_user_role_user; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.user_group_user_role_user (
    user_role_id integer NOT NULL,
    user_id integer NOT NULL,
    user_group_id integer NOT NULL
);


ALTER TABLE public.user_group_user_role_user OWNER TO postgres;

--
-- TOC entry 382 (class 1259 OID 17666)
-- Name: user_role; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.user_role (
    id integer NOT NULL,
    user_group_id integer NOT NULL,
    name character varying(100) NOT NULL
);


ALTER TABLE public.user_role OWNER TO postgres;

--
-- TOC entry 391 (class 1259 OID 1992403)
-- Name: view_node_type_list_action; Type: TABLE; Schema: public; Owner: niels
--

CREATE TABLE public.view_node_type_list_action (
    basic_action_id integer NOT NULL,
    node_type_id integer NOT NULL
);


ALTER TABLE public.view_node_type_list_action OWNER TO niels;

--
-- TOC entry 383 (class 1259 OID 17669)
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
-- TOC entry 384 (class 1259 OID 17674)
-- Name: wrongful_medication_case; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.wrongful_medication_case (
    id integer NOT NULL
);


ALTER TABLE public.wrongful_medication_case OWNER TO postgres;

--
-- TOC entry 385 (class 1259 OID 17677)
-- Name: wrongful_removal_case; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.wrongful_removal_case (
    id integer NOT NULL
);


ALTER TABLE public.wrongful_removal_case OWNER TO postgres;

--
-- TOC entry 4357 (class 2606 OID 17681)
-- Name: documentable Documentable_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.documentable
    ADD CONSTRAINT "Documentable_pkey" PRIMARY KEY (id);


--
-- TOC entry 4169 (class 2606 OID 17683)
-- Name: abuse_case abuse_case_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.abuse_case
    ADD CONSTRAINT abuse_case_pkey PRIMARY KEY (id);


--
-- TOC entry 4814 (class 2606 OID 2047149)
-- Name: abuse_case_type_of_abuse abuse_case_type_of_abuse_pkey; Type: CONSTRAINT; Schema: public; Owner: niels
--

ALTER TABLE ONLY public.abuse_case_type_of_abuse
    ADD CONSTRAINT abuse_case_type_of_abuse_pkey PRIMARY KEY (abuse_case_id, type_of_abuse_id);


--
-- TOC entry 4810 (class 2606 OID 2047132)
-- Name: abuse_case_type_of_abuser abuse_case_type_of_abuser_pkey; Type: CONSTRAINT; Schema: public; Owner: niels
--

ALTER TABLE ONLY public.abuse_case_type_of_abuser
    ADD CONSTRAINT abuse_case_type_of_abuser_pkey PRIMARY KEY (abuse_case_id, type_of_abuser_id);


--
-- TOC entry 4174 (class 2606 OID 17685)
-- Name: access_role access_role_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.access_role
    ADD CONSTRAINT access_role_pkey PRIMARY KEY (id);


--
-- TOC entry 4177 (class 2606 OID 17687)
-- Name: access_role_privilege access_role_privilege_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.access_role_privilege
    ADD CONSTRAINT access_role_privilege_pkey PRIMARY KEY (access_role_id, action_id);


--
-- TOC entry 4181 (class 2606 OID 17689)
-- Name: act act_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.act
    ADD CONSTRAINT act_pkey PRIMARY KEY (id);


--
-- TOC entry 4186 (class 2606 OID 17691)
-- Name: action_menu_item action_menu_item_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.action_menu_item
    ADD CONSTRAINT action_menu_item_pkey PRIMARY KEY (id);


--
-- TOC entry 4184 (class 2606 OID 17693)
-- Name: action action_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.action
    ADD CONSTRAINT action_pkey PRIMARY KEY (id);


--
-- TOC entry 4192 (class 2606 OID 17695)
-- Name: administrator_role administrator_role_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.administrator_role
    ADD CONSTRAINT administrator_role_pkey PRIMARY KEY (id);


--
-- TOC entry 4198 (class 2606 OID 17697)
-- Name: adoption_lawyer adoption_lawyer_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.adoption_lawyer
    ADD CONSTRAINT adoption_lawyer_pkey PRIMARY KEY (id);


--
-- TOC entry 4431 (class 2606 OID 17699)
-- Name: inter_organizational_relation affiliation_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.inter_organizational_relation
    ADD CONSTRAINT affiliation_pkey PRIMARY KEY (id);


--
-- TOC entry 4201 (class 2606 OID 17703)
-- Name: attachment_therapist attachment_therapist_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.attachment_therapist
    ADD CONSTRAINT attachment_therapist_pkey PRIMARY KEY (id);


--
-- TOC entry 4204 (class 2606 OID 17705)
-- Name: basic_action basic_action_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.basic_action
    ADD CONSTRAINT basic_action_pkey PRIMARY KEY (id);


--
-- TOC entry 4210 (class 2606 OID 17707)
-- Name: basic_country basic_country_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.basic_country
    ADD CONSTRAINT basic_country_pkey PRIMARY KEY (id);


--
-- TOC entry 4213 (class 2606 OID 17709)
-- Name: basic_first_and_second_level_subdivision basic_first_and_second_level_subdivision_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.basic_first_and_second_level_subdivision
    ADD CONSTRAINT basic_first_and_second_level_subdivision_pkey PRIMARY KEY (id);


--
-- TOC entry 4216 (class 2606 OID 17711)
-- Name: basic_nameable basic_nameable_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.basic_nameable
    ADD CONSTRAINT basic_nameable_pkey PRIMARY KEY (id);


--
-- TOC entry 4219 (class 2606 OID 17713)
-- Name: basic_second_level_subdivision basic_second_level_subdivision_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.basic_second_level_subdivision
    ADD CONSTRAINT basic_second_level_subdivision_pkey PRIMARY KEY (id);


--
-- TOC entry 4226 (class 2606 OID 17715)
-- Name: bill_action_type bill_action_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.bill_action_type
    ADD CONSTRAINT bill_action_pkey PRIMARY KEY (id);


--
-- TOC entry 4222 (class 2606 OID 17717)
-- Name: bill bill_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.bill
    ADD CONSTRAINT bill_pkey PRIMARY KEY (id);


--
-- TOC entry 4229 (class 2606 OID 17719)
-- Name: binding_country binding_country_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.binding_country
    ADD CONSTRAINT binding_country_pkey PRIMARY KEY (id);


--
-- TOC entry 4232 (class 2606 OID 17721)
-- Name: blog_post blog_post_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.blog_post
    ADD CONSTRAINT blog_post_pkey PRIMARY KEY (id);


--
-- TOC entry 4235 (class 2606 OID 17723)
-- Name: bottom_level_subdivision bottom_level_subdivision_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.bottom_level_subdivision
    ADD CONSTRAINT bottom_level_subdivision_pkey PRIMARY KEY (id);


--
-- TOC entry 4238 (class 2606 OID 17725)
-- Name: bound_country bound_country_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.bound_country
    ADD CONSTRAINT bound_country_pkey PRIMARY KEY (id);


--
-- TOC entry 4244 (class 2606 OID 17727)
-- Name: case_case_parties case_case_parties_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.case_case_parties
    ADD CONSTRAINT case_case_parties_pkey PRIMARY KEY (case_id, case_parties_id);


--
-- TOC entry 4251 (class 2606 OID 17729)
-- Name: case_parties_organization case_parties_organization_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.case_parties_organization
    ADD CONSTRAINT case_parties_organization_pkey PRIMARY KEY (case_parties_id, organization_id);


--
-- TOC entry 4255 (class 2606 OID 17731)
-- Name: case_parties_person case_parties_person_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.case_parties_person
    ADD CONSTRAINT case_parties_person_pkey PRIMARY KEY (case_parties_id, person_id);


--
-- TOC entry 4249 (class 2606 OID 17733)
-- Name: case_parties case_parties_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.case_parties
    ADD CONSTRAINT case_parties_pkey PRIMARY KEY (id);


--
-- TOC entry 4241 (class 2606 OID 17735)
-- Name: case case_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."case"
    ADD CONSTRAINT case_pkey PRIMARY KEY (id);


--
-- TOC entry 4259 (class 2606 OID 17737)
-- Name: case_party_type case_relation_type_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.case_party_type
    ADD CONSTRAINT case_relation_type_pkey PRIMARY KEY (id);


--
-- TOC entry 4265 (class 2606 OID 17739)
-- Name: case_type_case_party_type case_type_case_party_type_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.case_type_case_party_type
    ADD CONSTRAINT case_type_case_party_type_pkey PRIMARY KEY (case_type_id, case_party_type_id);


--
-- TOC entry 4262 (class 2606 OID 17741)
-- Name: case_type case_type_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.case_type
    ADD CONSTRAINT case_type_pkey PRIMARY KEY (id);


--
-- TOC entry 4167 (class 2606 OID 17742)
-- Name: system_group check_system_group_id_equals_0; Type: CHECK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE public.system_group
    ADD CONSTRAINT check_system_group_id_equals_0 CHECK ((id = 0)) NOT VALID;


--
-- TOC entry 4269 (class 2606 OID 17744)
-- Name: child_placement_type child_placement_type_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.child_placement_type
    ADD CONSTRAINT child_placement_type_pkey PRIMARY KEY (id);


--
-- TOC entry 4272 (class 2606 OID 17746)
-- Name: child_trafficking_case child_trafficking_case_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.child_trafficking_case
    ADD CONSTRAINT child_trafficking_case_pkey PRIMARY KEY (id);


--
-- TOC entry 4275 (class 2606 OID 17749)
-- Name: coerced_adoption_case coerced_adoption_case_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.coerced_adoption_case
    ADD CONSTRAINT coerced_adoption_case_pkey PRIMARY KEY (id);


--
-- TOC entry 4277 (class 2606 OID 17751)
-- Name: collective collective_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.collective
    ADD CONSTRAINT collective_pkey PRIMARY KEY (id);


--
-- TOC entry 4280 (class 2606 OID 17753)
-- Name: collective_user collective_user_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.collective_user
    ADD CONSTRAINT collective_user_pkey PRIMARY KEY (collective_id, user_id);


--
-- TOC entry 4283 (class 2606 OID 17755)
-- Name: comment comment_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.comment
    ADD CONSTRAINT comment_pkey PRIMARY KEY (id);


--
-- TOC entry 4288 (class 2606 OID 17757)
-- Name: congressional_term congressional_term_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.congressional_term
    ADD CONSTRAINT congressional_term_pkey PRIMARY KEY (id);


--
-- TOC entry 4291 (class 2606 OID 17759)
-- Name: congressional_term_political_party_affiliation congressional_term_political_party_affiliation_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.congressional_term_political_party_affiliation
    ADD CONSTRAINT congressional_term_political_party_affiliation_pkey PRIMARY KEY (id);


--
-- TOC entry 4297 (class 2606 OID 17761)
-- Name: content_sharing_group content_sharing_group_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.content_sharing_group
    ADD CONSTRAINT content_sharing_group_pkey PRIMARY KEY (id);


--
-- TOC entry 4308 (class 2606 OID 17763)
-- Name: country_and_first_and_second_level_subdivision count_and_first_and_second_level_subdivision_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.country_and_first_and_second_level_subdivision
    ADD CONSTRAINT count_and_first_and_second_level_subdivision_pkey PRIMARY KEY (id);


--
-- TOC entry 4305 (class 2606 OID 17765)
-- Name: country_and_first_and_bottom_level_subdivision country_and_first_and_bottom_level_subdivision_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.country_and_first_and_bottom_level_subdivision
    ADD CONSTRAINT country_and_first_and_bottom_level_subdivision_pkey PRIMARY KEY (id);


--
-- TOC entry 4312 (class 2606 OID 17767)
-- Name: country_and_first_level_subdivision country_and_first_level_subdivision_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.country_and_first_level_subdivision
    ADD CONSTRAINT country_and_first_level_subdivision_pkey PRIMARY KEY (id);


--
-- TOC entry 4316 (class 2606 OID 17769)
-- Name: country_and_intermediate_level_subdivision country_and_intermediate_level_subdivision_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.country_and_intermediate_level_subdivision
    ADD CONSTRAINT country_and_intermediate_level_subdivision_pkey PRIMARY KEY (id);


--
-- TOC entry 4300 (class 2606 OID 17771)
-- Name: country country_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.country
    ADD CONSTRAINT country_pkey PRIMARY KEY (id);


--
-- TOC entry 4319 (class 2606 OID 17773)
-- Name: country_report country_report_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.country_report
    ADD CONSTRAINT country_report_pkey PRIMARY KEY (country_id, date_range);


--
-- TOC entry 4462 (class 2606 OID 17775)
-- Name: iso_coded_subdivision country_subdivision_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.iso_coded_subdivision
    ADD CONSTRAINT country_subdivision_pkey PRIMARY KEY (id);


--
-- TOC entry 4323 (class 2606 OID 17777)
-- Name: country_subdivision_type country_subdivision_type_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.country_subdivision_type
    ADD CONSTRAINT country_subdivision_type_pkey PRIMARY KEY (country_id, subdivision_type_id);


--
-- TOC entry 4327 (class 2606 OID 17779)
-- Name: create_node_action create_node_action_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.create_node_action
    ADD CONSTRAINT create_node_action_pkey PRIMARY KEY (id);


--
-- TOC entry 4331 (class 2606 OID 17781)
-- Name: delete_node_action delete_node_action_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.delete_node_action
    ADD CONSTRAINT delete_node_action_pkey PRIMARY KEY (id);


--
-- TOC entry 4335 (class 2606 OID 17783)
-- Name: denomination denomination_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.denomination
    ADD CONSTRAINT denomination_pkey PRIMARY KEY (id);


--
-- TOC entry 4338 (class 2606 OID 17785)
-- Name: deportation_case deportation_case_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.deportation_case
    ADD CONSTRAINT deportation_case_pkey PRIMARY KEY (id);


--
-- TOC entry 4343 (class 2606 OID 17787)
-- Name: discussion discussion_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.discussion
    ADD CONSTRAINT discussion_pkey PRIMARY KEY (id);


--
-- TOC entry 4346 (class 2606 OID 17789)
-- Name: disrupted_placement_case disrupted_placement_case_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.disrupted_placement_case
    ADD CONSTRAINT disrupted_placement_case_pkey PRIMARY KEY (id);


--
-- TOC entry 4349 (class 2606 OID 17791)
-- Name: document document_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.document
    ADD CONSTRAINT document_pkey PRIMARY KEY (id);


--
-- TOC entry 4354 (class 2606 OID 17793)
-- Name: document_type document_type_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.document_type
    ADD CONSTRAINT document_type_pkey PRIMARY KEY (id);


--
-- TOC entry 4359 (class 2606 OID 17797)
-- Name: edit_node_action edit_node_action_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.edit_node_action
    ADD CONSTRAINT edit_node_action_pkey PRIMARY KEY (id);


--
-- TOC entry 4787 (class 2606 OID 192735)
-- Name: edit_own_node_action edit_own_node_action_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.edit_own_node_action
    ADD CONSTRAINT edit_own_node_action_pkey PRIMARY KEY (id);


--
-- TOC entry 4363 (class 2606 OID 17799)
-- Name: facilitator facilitator_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.facilitator
    ADD CONSTRAINT facilitator_pkey PRIMARY KEY (id);


--
-- TOC entry 4366 (class 2606 OID 17801)
-- Name: family_size family_size_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.family_size
    ADD CONSTRAINT family_size_pkey PRIMARY KEY (id);


--
-- TOC entry 4369 (class 2606 OID 17803)
-- Name: fathers_rights_violation_case fathers_rights_violations_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.fathers_rights_violation_case
    ADD CONSTRAINT fathers_rights_violations_pkey PRIMARY KEY (id);


--
-- TOC entry 4372 (class 2606 OID 17805)
-- Name: file file_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.file
    ADD CONSTRAINT file_pkey PRIMARY KEY (id);


--
-- TOC entry 4374 (class 2606 OID 17807)
-- Name: first_and_bottom_level_subdivision first_and_bottom_level_subdivision_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.first_and_bottom_level_subdivision
    ADD CONSTRAINT first_and_bottom_level_subdivision_pkey PRIMARY KEY (id);


--
-- TOC entry 4378 (class 2606 OID 17809)
-- Name: first_and_second_level_subdivision first_and_second_level_subdivision_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.first_and_second_level_subdivision
    ADD CONSTRAINT first_and_second_level_subdivision_pkey PRIMARY KEY (id);


--
-- TOC entry 4382 (class 2606 OID 17811)
-- Name: first_level_global_region first_level_global_region_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.first_level_global_region
    ADD CONSTRAINT first_level_global_region_pkey PRIMARY KEY (id);


--
-- TOC entry 4386 (class 2606 OID 17813)
-- Name: first_level_subdivision first_level_subdivision_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.first_level_subdivision
    ADD CONSTRAINT first_level_subdivision_pkey PRIMARY KEY (id);


--
-- TOC entry 4391 (class 2606 OID 17815)
-- Name: formal_intermediate_level_subdivision formal_intermediate_level_subdivision_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.formal_intermediate_level_subdivision
    ADD CONSTRAINT formal_intermediate_level_subdivision_pkey PRIMARY KEY (id);


--
-- TOC entry 4394 (class 2606 OID 17817)
-- Name: geographical_entity geographical_entity_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.geographical_entity
    ADD CONSTRAINT geographical_entity_pkey PRIMARY KEY (id);


--
-- TOC entry 4397 (class 2606 OID 17819)
-- Name: global_region global_region_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.global_region
    ADD CONSTRAINT global_region_pkey PRIMARY KEY (id);


--
-- TOC entry 4400 (class 2606 OID 17821)
-- Name: hague_status hague_status_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.hague_status
    ADD CONSTRAINT hague_status_pkey PRIMARY KEY (id);


--
-- TOC entry 4403 (class 2606 OID 17823)
-- Name: home_study_agency home_study_agency_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.home_study_agency
    ADD CONSTRAINT home_study_agency_pkey PRIMARY KEY (id);


--
-- TOC entry 4406 (class 2606 OID 17825)
-- Name: house_bill house_bill_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.house_bill
    ADD CONSTRAINT house_bill_pkey PRIMARY KEY (id);


--
-- TOC entry 4411 (class 2606 OID 17827)
-- Name: house_term house_term_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.house_term
    ADD CONSTRAINT house_term_pkey PRIMARY KEY (id);


--
-- TOC entry 4480 (class 2606 OID 17829)
-- Name: location_locatable idx_locatable_location; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.location_locatable
    ADD CONSTRAINT idx_locatable_location UNIQUE (locatable_id, location_id);


--
-- TOC entry 4414 (class 2606 OID 17831)
-- Name: informal_intermediate_level_subdivision informal_intermediate_level_subdivision_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.informal_intermediate_level_subdivision
    ADD CONSTRAINT informal_intermediate_level_subdivision_pkey PRIMARY KEY (id);


--
-- TOC entry 4417 (class 2606 OID 17833)
-- Name: institution institution_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.institution
    ADD CONSTRAINT institution_pkey PRIMARY KEY (id);


--
-- TOC entry 4424 (class 2606 OID 17835)
-- Name: inter_country_relation inter_country_relation_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.inter_country_relation
    ADD CONSTRAINT inter_country_relation_pkey PRIMARY KEY (id);


--
-- TOC entry 4429 (class 2606 OID 17837)
-- Name: inter_country_relation_type inter_country_relation_type_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.inter_country_relation_type
    ADD CONSTRAINT inter_country_relation_type_pkey PRIMARY KEY (id);


--
-- TOC entry 4442 (class 2606 OID 17839)
-- Name: inter_organizational_relation_type inter_organizational_relation_type_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.inter_organizational_relation_type
    ADD CONSTRAINT inter_organizational_relation_type_pkey PRIMARY KEY (id);


--
-- TOC entry 4450 (class 2606 OID 17841)
-- Name: inter_personal_relation inter_personal_relation_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.inter_personal_relation
    ADD CONSTRAINT inter_personal_relation_pkey PRIMARY KEY (id);


--
-- TOC entry 4453 (class 2606 OID 17843)
-- Name: inter_personal_relation_type inter_personal_relation_type_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.inter_personal_relation_type
    ADD CONSTRAINT inter_personal_relation_type_pkey PRIMARY KEY (id);


--
-- TOC entry 4456 (class 2606 OID 17845)
-- Name: intermediate_level_subdivision intermediate_level_subdivision_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.intermediate_level_subdivision
    ADD CONSTRAINT intermediate_level_subdivision_pkey PRIMARY KEY (id);


--
-- TOC entry 4460 (class 2606 OID 17847)
-- Name: iso_coded_first_level_subdivision iso_coded_first_level_subdivision_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.iso_coded_first_level_subdivision
    ADD CONSTRAINT iso_coded_first_level_subdivision_pkey PRIMARY KEY (id);


--
-- TOC entry 4468 (class 2606 OID 17849)
-- Name: law_firm law_firm_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.law_firm
    ADD CONSTRAINT law_firm_pkey PRIMARY KEY (id);


--
-- TOC entry 4470 (class 2606 OID 17851)
-- Name: layout layout_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.layout
    ADD CONSTRAINT layout_pkey PRIMARY KEY (id);


--
-- TOC entry 4482 (class 2606 OID 17853)
-- Name: location_locatable locatable_location_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.location_locatable
    ADD CONSTRAINT locatable_location_pkey PRIMARY KEY (location_id, locatable_id);


--
-- TOC entry 4473 (class 2606 OID 17855)
-- Name: locatable locatable_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.locatable
    ADD CONSTRAINT locatable_pkey PRIMARY KEY (id);


--
-- TOC entry 4478 (class 2606 OID 17857)
-- Name: location location_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.location
    ADD CONSTRAINT location_pkey PRIMARY KEY (id);


--
-- TOC entry 4485 (class 2606 OID 17859)
-- Name: member_of_congress member_of_congress_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.member_of_congress
    ADD CONSTRAINT member_of_congress_pkey PRIMARY KEY (id);


--
-- TOC entry 4487 (class 2606 OID 17861)
-- Name: menu_item menu_item_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.menu_item
    ADD CONSTRAINT menu_item_pkey PRIMARY KEY (id);


--
-- TOC entry 4490 (class 2606 OID 17863)
-- Name: multi_question_poll multi_question_poll_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.multi_question_poll
    ADD CONSTRAINT multi_question_poll_pkey PRIMARY KEY (id);


--
-- TOC entry 4494 (class 2606 OID 17865)
-- Name: multi_question_poll_poll_question multi_question_poll_question_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.multi_question_poll_poll_question
    ADD CONSTRAINT multi_question_poll_question_pkey PRIMARY KEY (multi_question_poll_id, poll_question_id);


--
-- TOC entry 4500 (class 2606 OID 17867)
-- Name: nameable nameable_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.nameable
    ADD CONSTRAINT nameable_pkey PRIMARY KEY (id);


--
-- TOC entry 4802 (class 2606 OID 1933316)
-- Name: nameable_type nameable_type_pkey; Type: CONSTRAINT; Schema: public; Owner: niels
--

ALTER TABLE ONLY public.nameable_type
    ADD CONSTRAINT nameable_type_pkey PRIMARY KEY (id);


--
-- TOC entry 4295 (class 2606 OID 17869)
-- Name: congressional_term_political_party_affiliation no_overlap_congressional_term_political_party_affiliation; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.congressional_term_political_party_affiliation
    ADD CONSTRAINT no_overlap_congressional_term_political_party_affiliation EXCLUDE USING gist (congressional_term_id WITH =, united_states_political_party_affiliation_id WITH =, date_range WITH &&);


--
-- TOC entry 4426 (class 2606 OID 17871)
-- Name: inter_country_relation no_overlap_inter_country_relation; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.inter_country_relation
    ADD CONSTRAINT no_overlap_inter_country_relation EXCLUDE USING gist (country_id_to WITH =, date_range WITH &&, inter_country_relation_type_id WITH =, country_id_from WITH =);


--
-- TOC entry 4439 (class 2606 OID 17873)
-- Name: inter_organizational_relation no_overlap_inter_organizational_relation; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.inter_organizational_relation
    ADD CONSTRAINT no_overlap_inter_organizational_relation EXCLUDE USING gist (date_range WITH &&, organization_id_from WITH =, organization_id_to WITH =, geographical_entity_id WITH =, inter_organizational_relation_type_id WITH =);


--
-- TOC entry 4537 (class 2606 OID 17875)
-- Name: organizational_role no_overlap_organizational_role; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.organizational_role
    ADD CONSTRAINT no_overlap_organizational_role EXCLUDE USING gist (daterange WITH &&, organization_id WITH =, organization_type_id WITH =);


--
-- TOC entry 4608 (class 2606 OID 17877)
-- Name: professional_role no_overlap_professional_role; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.professional_role
    ADD CONSTRAINT no_overlap_professional_role EXCLUDE USING gist (daterange WITH &&, person_id WITH =, profession_id WITH =);


--
-- TOC entry 4740 (class 2606 OID 17879)
-- Name: united_states_congressional_meeting no_overlap_united_states_congressional_meeting; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.united_states_congressional_meeting
    ADD CONSTRAINT no_overlap_united_states_congressional_meeting EXCLUDE USING gist (date_range WITH &&);


--
-- TOC entry 4510 (class 2606 OID 17881)
-- Name: node_file node_file_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.node_file
    ADD CONSTRAINT node_file_pkey PRIMARY KEY (node_id, file_id);


--
-- TOC entry 4505 (class 2606 OID 17883)
-- Name: node node_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.node
    ADD CONSTRAINT node_pkey PRIMARY KEY (id);


--
-- TOC entry 4612 (class 2606 OID 17885)
-- Name: publication_status node_status_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.publication_status
    ADD CONSTRAINT node_status_pkey PRIMARY KEY (id);


--
-- TOC entry 4514 (class 2606 OID 17887)
-- Name: node_term node_term_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.node_term
    ADD CONSTRAINT node_term_pkey PRIMARY KEY (node_id, term_id);


--
-- TOC entry 4518 (class 2606 OID 17889)
-- Name: node_type node_type_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.node_type
    ADD CONSTRAINT node_type_pkey PRIMARY KEY (id);


--
-- TOC entry 4526 (class 2606 OID 17891)
-- Name: organization_act_relation_type organization_act_relation_type_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.organization_act_relation_type
    ADD CONSTRAINT organization_act_relation_type_pkey PRIMARY KEY (id);


--
-- TOC entry 4530 (class 2606 OID 17893)
-- Name: organization_organization_type organization_organization_type_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.organization_organization_type
    ADD CONSTRAINT organization_organization_type_pkey PRIMARY KEY (organization_id, organization_type_id);


--
-- TOC entry 4523 (class 2606 OID 17895)
-- Name: organization organization_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.organization
    ADD CONSTRAINT organization_pkey PRIMARY KEY (id);


--
-- TOC entry 4533 (class 2606 OID 17897)
-- Name: organization_type organization_type_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.organization_type
    ADD CONSTRAINT organization_type_pkey PRIMARY KEY (id);


--
-- TOC entry 4539 (class 2606 OID 17899)
-- Name: organizational_role organizational_role_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.organizational_role
    ADD CONSTRAINT organizational_role_pkey PRIMARY KEY (id);


--
-- TOC entry 4542 (class 2606 OID 17901)
-- Name: owner owner_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.owner
    ADD CONSTRAINT owner_pkey PRIMARY KEY (id);


--
-- TOC entry 4545 (class 2606 OID 17903)
-- Name: page page_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.page
    ADD CONSTRAINT page_pkey PRIMARY KEY (id);


--
-- TOC entry 4799 (class 2606 OID 1263435)
-- Name: party_act_relation party_act_relation_pkey; Type: CONSTRAINT; Schema: public; Owner: niels
--

ALTER TABLE ONLY public.party_act_relation
    ADD CONSTRAINT party_act_relation_pkey PRIMARY KEY (id);


--
-- TOC entry 4792 (class 2606 OID 1263422)
-- Name: party_act_relation_type party_act_relation_type_pkey; Type: CONSTRAINT; Schema: public; Owner: niels
--

ALTER TABLE ONLY public.party_act_relation_type
    ADD CONSTRAINT party_act_relation_type_pkey PRIMARY KEY (id);


--
-- TOC entry 4550 (class 2606 OID 17905)
-- Name: party party_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.party
    ADD CONSTRAINT party_pkey PRIMARY KEY (id);


--
-- TOC entry 4557 (class 2606 OID 17907)
-- Name: party_political_entity_relation party_political_entity_relation_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.party_political_entity_relation
    ADD CONSTRAINT party_political_entity_relation_pkey PRIMARY KEY (id);


--
-- TOC entry 4559 (class 2606 OID 17909)
-- Name: party_political_entity_relation_type party_political_entity_relation_type_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.party_political_entity_relation_type
    ADD CONSTRAINT party_political_entity_relation_type_pkey PRIMARY KEY (id);


--
-- TOC entry 4570 (class 2606 OID 17911)
-- Name: person_organization_relation person_organization_relation_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.person_organization_relation
    ADD CONSTRAINT person_organization_relation_pkey PRIMARY KEY (id);


--
-- TOC entry 4573 (class 2606 OID 17913)
-- Name: person_organization_relation_type person_organization_relation_type_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.person_organization_relation_type
    ADD CONSTRAINT person_organization_relation_type_pkey PRIMARY KEY (id);


--
-- TOC entry 4563 (class 2606 OID 17915)
-- Name: person person_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.person
    ADD CONSTRAINT person_pkey PRIMARY KEY (id);


--
-- TOC entry 4576 (class 2606 OID 17917)
-- Name: placement_agency placement_agency_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.placement_agency
    ADD CONSTRAINT placement_agency_pkey PRIMARY KEY (id);


--
-- TOC entry 4580 (class 2606 OID 17919)
-- Name: political_entity political_entity_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.political_entity
    ADD CONSTRAINT political_entity_pkey PRIMARY KEY (id);


--
-- TOC entry 4586 (class 2606 OID 17921)
-- Name: poll_option poll_option_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.poll_option
    ADD CONSTRAINT poll_option_pkey PRIMARY KEY (delta, poll_question_id);


--
-- TOC entry 4583 (class 2606 OID 17923)
-- Name: poll poll_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.poll
    ADD CONSTRAINT poll_pkey PRIMARY KEY (id);


--
-- TOC entry 4589 (class 2606 OID 17925)
-- Name: poll_question poll_question_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.poll_question
    ADD CONSTRAINT poll_question_pkey PRIMARY KEY (id);


--
-- TOC entry 4591 (class 2606 OID 17927)
-- Name: poll_status poll_status_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.poll_status
    ADD CONSTRAINT poll_status_pkey PRIMARY KEY (id);


--
-- TOC entry 4166 (class 2606 OID 17928)
-- Name: poll_vote poll_vote_check; Type: CHECK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE public.poll_vote
    ADD CONSTRAINT poll_vote_check CHECK ((NOT ((user_id IS NULL) AND (ip_address IS NULL)))) NOT VALID;


--
-- TOC entry 4597 (class 2606 OID 17930)
-- Name: poll_vote poll_vote_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.poll_vote
    ADD CONSTRAINT poll_vote_pkey PRIMARY KEY (id);


--
-- TOC entry 4600 (class 2606 OID 17932)
-- Name: post_placement_agency post_placement_agency_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.post_placement_agency
    ADD CONSTRAINT post_placement_agency_pkey PRIMARY KEY (id);


--
-- TOC entry 4602 (class 2606 OID 17934)
-- Name: principal principal_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.principal
    ADD CONSTRAINT principal_pkey PRIMARY KEY (id);


--
-- TOC entry 4605 (class 2606 OID 17936)
-- Name: profession profession_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.profession
    ADD CONSTRAINT profession_pkey PRIMARY KEY (id);


--
-- TOC entry 4610 (class 2606 OID 17938)
-- Name: professional_role professional_role_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.professional_role
    ADD CONSTRAINT professional_role_pkey PRIMARY KEY (id);


--
-- TOC entry 4617 (class 2606 OID 17940)
-- Name: publisher publisher_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.publisher
    ADD CONSTRAINT publisher_pkey PRIMARY KEY (id);


--
-- TOC entry 4785 (class 2606 OID 139268)
-- Name: publishing_user_group publishing_user_group_pkey; Type: CONSTRAINT; Schema: public; Owner: niels
--

ALTER TABLE ONLY public.publishing_user_group
    ADD CONSTRAINT publishing_user_group_pkey PRIMARY KEY (id);


--
-- TOC entry 4627 (class 2606 OID 17942)
-- Name: representative_house_bill_action representative_house_bill_action_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.representative_house_bill_action
    ADD CONSTRAINT representative_house_bill_action_pkey PRIMARY KEY (id);


--
-- TOC entry 4622 (class 2606 OID 17944)
-- Name: representative representative_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.representative
    ADD CONSTRAINT representative_pkey PRIMARY KEY (id);


--
-- TOC entry 4632 (class 2606 OID 17946)
-- Name: review review_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.review
    ADD CONSTRAINT review_pkey PRIMARY KEY (id);


--
-- TOC entry 4635 (class 2606 OID 17948)
-- Name: searchable searchable_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.searchable
    ADD CONSTRAINT searchable_pkey PRIMARY KEY (id);


--
-- TOC entry 4640 (class 2606 OID 17950)
-- Name: second_level_global_region second_level_global_region_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.second_level_global_region
    ADD CONSTRAINT second_level_global_region_pkey PRIMARY KEY (id);


--
-- TOC entry 4644 (class 2606 OID 17952)
-- Name: second_level_subdivision second_level_subdivision_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.second_level_subdivision
    ADD CONSTRAINT second_level_subdivision_pkey PRIMARY KEY (id);


--
-- TOC entry 4648 (class 2606 OID 17954)
-- Name: senate_bill senate_bill_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.senate_bill
    ADD CONSTRAINT senate_bill_pkey PRIMARY KEY (id);


--
-- TOC entry 4653 (class 2606 OID 17956)
-- Name: senate_term senate_term_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.senate_term
    ADD CONSTRAINT senate_term_pkey PRIMARY KEY (id);


--
-- TOC entry 4656 (class 2606 OID 17958)
-- Name: senator senator_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.senator
    ADD CONSTRAINT senator_pkey PRIMARY KEY (id);


--
-- TOC entry 4661 (class 2606 OID 17960)
-- Name: senator_senate_bill_action senator_senate_bill_action_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.senator_senate_bill_action
    ADD CONSTRAINT senator_senate_bill_action_pkey PRIMARY KEY (id);


--
-- TOC entry 4666 (class 2606 OID 17962)
-- Name: simple_text_node simple_text_node_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.simple_text_node
    ADD CONSTRAINT simple_text_node_pkey PRIMARY KEY (id);


--
-- TOC entry 4670 (class 2606 OID 17964)
-- Name: single_question_poll single_question_poll_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.single_question_poll
    ADD CONSTRAINT single_question_poll_pkey PRIMARY KEY (id);


--
-- TOC entry 4675 (class 2606 OID 17966)
-- Name: subdivision subdivision_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.subdivision
    ADD CONSTRAINT subdivision_pkey PRIMARY KEY (id);


--
-- TOC entry 4679 (class 2606 OID 17968)
-- Name: subdivision_type subdivision_type_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.subdivision_type
    ADD CONSTRAINT subdivision_type_pkey PRIMARY KEY (id);


--
-- TOC entry 4682 (class 2606 OID 17970)
-- Name: subgroup subgroup_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.subgroup
    ADD CONSTRAINT subgroup_pkey PRIMARY KEY (id);


--
-- TOC entry 4685 (class 2606 OID 17972)
-- Name: system_group system_group_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.system_group
    ADD CONSTRAINT system_group_pkey PRIMARY KEY (id);


--
-- TOC entry 4699 (class 2606 OID 17974)
-- Name: tenant_file tenant_file_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tenant_file
    ADD CONSTRAINT tenant_file_pkey PRIMARY KEY (tenant_id, file_id);


--
-- TOC entry 4713 (class 2606 OID 17976)
-- Name: tenant_node_menu_item tenant_node_menu_item_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tenant_node_menu_item
    ADD CONSTRAINT tenant_node_menu_item_pkey PRIMARY KEY (id);


--
-- TOC entry 4705 (class 2606 OID 17978)
-- Name: tenant_node tenant_node_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tenant_node
    ADD CONSTRAINT tenant_node_pkey PRIMARY KEY (id);


--
-- TOC entry 4693 (class 2606 OID 17980)
-- Name: tenant tenant_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tenant
    ADD CONSTRAINT tenant_pkey PRIMARY KEY (id);


--
-- TOC entry 4718 (class 2606 OID 17982)
-- Name: term term_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.term
    ADD CONSTRAINT term_pkey PRIMARY KEY (id);


--
-- TOC entry 4728 (class 2606 OID 17984)
-- Name: top_level_country top_level_country_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.top_level_country
    ADD CONSTRAINT top_level_country_pkey PRIMARY KEY (id);


--
-- TOC entry 4733 (class 2606 OID 17986)
-- Name: type_of_abuse type_of_abuse_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.type_of_abuse
    ADD CONSTRAINT type_of_abuse_pkey PRIMARY KEY (id);


--
-- TOC entry 4736 (class 2606 OID 17988)
-- Name: type_of_abuser type_of_abuser_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.type_of_abuser
    ADD CONSTRAINT type_of_abuser_pkey PRIMARY KEY (id);


--
-- TOC entry 4208 (class 2606 OID 17990)
-- Name: basic_action unique_action_access_privilege_action; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.basic_action
    ADD CONSTRAINT unique_action_access_privilege_action UNIQUE (path) INCLUDE (id);


--
-- TOC entry 4190 (class 2606 OID 17992)
-- Name: action_menu_item unique_action_menu_item_name; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.action_menu_item
    ADD CONSTRAINT unique_action_menu_item_name UNIQUE (name) INCLUDE (action_id, id);


--
-- TOC entry 4196 (class 2606 OID 17994)
-- Name: administrator_role unique_administrator_role_user_group; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.administrator_role
    ADD CONSTRAINT unique_administrator_role_user_group UNIQUE (user_group_id) INCLUDE (id);


--
-- TOC entry 4742 (class 2606 OID 17996)
-- Name: united_states_congressional_meeting unique_congressional_meeting_number; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.united_states_congressional_meeting
    ADD CONSTRAINT unique_congressional_meeting_number UNIQUE (number) INCLUDE (id, date_range);


--
-- TOC entry 4465 (class 2606 OID 17998)
-- Name: iso_coded_subdivision unique_iso_3166_2_code; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.iso_coded_subdivision
    ADD CONSTRAINT unique_iso_3166_2_code UNIQUE (iso_3166_2_code) INCLUDE (id);


--
-- TOC entry 4730 (class 2606 OID 18000)
-- Name: top_level_country unique_iso_3166_code; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.top_level_country
    ADD CONSTRAINT unique_iso_3166_code UNIQUE (iso_3166_1_code) INCLUDE (id, global_region_id);


--
-- TOC entry 4496 (class 2606 OID 18002)
-- Name: multi_question_poll_poll_question unique_multi_question_poll_question_multi_question_poll_delta; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.multi_question_poll_poll_question
    ADD CONSTRAINT unique_multi_question_poll_question_multi_question_poll_delta UNIQUE (multi_question_poll_id, delta) INCLUDE (poll_question_id);


--
-- TOC entry 4614 (class 2606 OID 18004)
-- Name: publication_status unique_node_status_name; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.publication_status
    ADD CONSTRAINT unique_node_status_name UNIQUE (name);


--
-- TOC entry 4516 (class 2606 OID 18006)
-- Name: node_term unique_node_term_term_id_node_id; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.node_term
    ADD CONSTRAINT unique_node_term_term_id_node_id UNIQUE (term_id, node_id);


--
-- TOC entry 4593 (class 2606 OID 18008)
-- Name: poll_status unique_poll_status_name; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.poll_status
    ADD CONSTRAINT unique_poll_status_name UNIQUE (name) INCLUDE (id);


--
-- TOC entry 4619 (class 2606 OID 18010)
-- Name: publisher unique_publisher_name; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.publisher
    ADD CONSTRAINT unique_publisher_name UNIQUE (name) INCLUDE (id);


--
-- TOC entry 4629 (class 2606 OID 18012)
-- Name: representative_house_bill_action unique_representative_house_bill_bill_action_type; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.representative_house_bill_action
    ADD CONSTRAINT unique_representative_house_bill_bill_action_type UNIQUE (representative_id, house_bill_id, bill_action_type_id);


--
-- TOC entry 4663 (class 2606 OID 18014)
-- Name: senator_senate_bill_action unique_senator_senate_bill_bill_action_type; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.senator_senate_bill_action
    ADD CONSTRAINT unique_senator_senate_bill_bill_action_type UNIQUE (senator_id, senate_bill_id, bill_action_type_id);


--
-- TOC entry 4677 (class 2606 OID 18016)
-- Name: subdivision unique_subdivision_name; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.subdivision
    ADD CONSTRAINT unique_subdivision_name UNIQUE (country_id, name) INCLUDE (id);


--
-- TOC entry 4695 (class 2606 OID 18018)
-- Name: tenant unique_tenant_domain_name; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tenant
    ADD CONSTRAINT unique_tenant_domain_name UNIQUE (domain_name);


--
-- TOC entry 4707 (class 2606 OID 18020)
-- Name: tenant_node unique_tenant_id_url_id; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tenant_node
    ADD CONSTRAINT unique_tenant_id_url_id UNIQUE (tenant_id, url_id) INCLUDE (node_id, id, publication_status_id, subgroup_id, url_path);


--
-- TOC entry 4709 (class 2606 OID 18022)
-- Name: tenant_node unique_tenant_id_url_path; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tenant_node
    ADD CONSTRAINT unique_tenant_id_url_path UNIQUE (tenant_id, url_path) INCLUDE (id, url_id, node_id, subgroup_id, publication_status_id);


--
-- TOC entry 4715 (class 2606 OID 18024)
-- Name: tenant_node_menu_item unique_tenant_node_menu_item_tenant_node_name; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tenant_node_menu_item
    ADD CONSTRAINT unique_tenant_node_menu_item_tenant_node_name UNIQUE (tenant_node_id, name) INCLUDE (id);


--
-- TOC entry 4720 (class 2606 OID 18026)
-- Name: term unique_term_vocabulary_name; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.term
    ADD CONSTRAINT unique_term_vocabulary_name UNIQUE (vocabulary_id, name) INCLUDE (id, nameable_id);


--
-- TOC entry 4722 (class 2606 OID 18028)
-- Name: term unique_term_vocabulary_nameable; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.term
    ADD CONSTRAINT unique_term_vocabulary_nameable UNIQUE (vocabulary_id, nameable_id) INCLUDE (id, name);


--
-- TOC entry 4755 (class 2606 OID 18030)
-- Name: user unique_user_email; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."user"
    ADD CONSTRAINT unique_user_email UNIQUE (email);


--
-- TOC entry 4768 (class 2606 OID 18032)
-- Name: user_role unique_user_role_user_group_name; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.user_role
    ADD CONSTRAINT unique_user_role_user_group_name UNIQUE (user_group_id, name) INCLUDE (id);


--
-- TOC entry 4806 (class 2606 OID 1992409)
-- Name: view_node_type_list_action unique_view_node_type_list_action_node_type; Type: CONSTRAINT; Schema: public; Owner: niels
--

ALTER TABLE ONLY public.view_node_type_list_action
    ADD CONSTRAINT unique_view_node_type_list_action_node_type UNIQUE (node_type_id) INCLUDE (basic_action_id);


--
-- TOC entry 4773 (class 2606 OID 18034)
-- Name: vocabulary unique_vocabulary_name_per_owner; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.vocabulary
    ADD CONSTRAINT unique_vocabulary_name_per_owner UNIQUE (name, owner_id) INCLUDE (id);


--
-- TOC entry 4744 (class 2606 OID 18036)
-- Name: united_states_congressional_meeting united_states_congressional_meeting_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.united_states_congressional_meeting
    ADD CONSTRAINT united_states_congressional_meeting_pkey PRIMARY KEY (id);


--
-- TOC entry 4751 (class 2606 OID 18038)
-- Name: united_states_political_party_affiliation united_states_political_party_affiliation_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.united_states_political_party_affiliation
    ADD CONSTRAINT united_states_political_party_affiliation_pkey PRIMARY KEY (id);


--
-- TOC entry 4747 (class 2606 OID 18040)
-- Name: united_states_political_party united_states_political_party_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.united_states_political_party
    ADD CONSTRAINT united_states_political_party_pkey PRIMARY KEY (id);


--
-- TOC entry 4759 (class 2606 OID 18042)
-- Name: user_group user_group_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.user_group
    ADD CONSTRAINT user_group_pkey PRIMARY KEY (id);


--
-- TOC entry 4764 (class 2606 OID 18044)
-- Name: user_group_user_role_user user_group_user_role_user_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.user_group_user_role_user
    ADD CONSTRAINT user_group_user_role_user_pkey PRIMARY KEY (user_group_id, user_role_id, user_id);


--
-- TOC entry 4757 (class 2606 OID 18046)
-- Name: user user_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."user"
    ADD CONSTRAINT user_pkey PRIMARY KEY (id);


--
-- TOC entry 4770 (class 2606 OID 18048)
-- Name: user_role user_role_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.user_role
    ADD CONSTRAINT user_role_pkey PRIMARY KEY (id);


--
-- TOC entry 4808 (class 2606 OID 1993825)
-- Name: view_node_type_list_action view_node_type_list_action_pkey; Type: CONSTRAINT; Schema: public; Owner: niels
--

ALTER TABLE ONLY public.view_node_type_list_action
    ADD CONSTRAINT view_node_type_list_action_pkey PRIMARY KEY (basic_action_id, node_type_id);


--
-- TOC entry 4775 (class 2606 OID 18050)
-- Name: vocabulary vocabulary_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.vocabulary
    ADD CONSTRAINT vocabulary_pkey PRIMARY KEY (id);


--
-- TOC entry 4778 (class 2606 OID 18052)
-- Name: wrongful_medication_case wrongful_medication_case_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.wrongful_medication_case
    ADD CONSTRAINT wrongful_medication_case_pkey PRIMARY KEY (id);


--
-- TOC entry 4781 (class 2606 OID 18054)
-- Name: wrongful_removal_case wrongful_removal_case_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.wrongful_removal_case
    ADD CONSTRAINT wrongful_removal_case_pkey PRIMARY KEY (id);


--
-- TOC entry 4474 (class 1259 OID 18055)
-- Name: fki_.; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "fki_." ON public.location USING btree (country_id);


--
-- TOC entry 4187 (class 1259 OID 18056)
-- Name: fki_a; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_a ON public.action_menu_item USING btree (action_id);


--
-- TOC entry 4236 (class 1259 OID 18057)
-- Name: fki_bottom_level_subdivision; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_bottom_level_subdivision ON public.bottom_level_subdivision USING btree (id);


--
-- TOC entry 4273 (class 1259 OID 18058)
-- Name: fki_c; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_c ON public.child_trafficking_case USING btree (country_id_from);


--
-- TOC entry 4463 (class 1259 OID 18059)
-- Name: fki_country_subdivision; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_country_subdivision ON public.iso_coded_subdivision USING btree (id);


--
-- TOC entry 4641 (class 1259 OID 18060)
-- Name: fki_country_subdivision_country_id_2; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_country_subdivision_country_id_2 ON public.second_level_subdivision USING btree (id);


--
-- TOC entry 4217 (class 1259 OID 18061)
-- Name: fki_d; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_d ON public.basic_nameable USING btree (id);


--
-- TOC entry 4170 (class 1259 OID 18062)
-- Name: fki_fk_abuse_case_child_placement_type; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_abuse_case_child_placement_type ON public.abuse_case USING btree (child_placement_type_id);


--
-- TOC entry 4171 (class 1259 OID 18063)
-- Name: fki_fk_abuse_case_family_size; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_abuse_case_family_size ON public.abuse_case USING btree (id);


--
-- TOC entry 4172 (class 1259 OID 18064)
-- Name: fki_fk_abuse_case_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_abuse_case_id ON public.abuse_case USING btree (id);


--
-- TOC entry 4815 (class 1259 OID 2047160)
-- Name: fki_fk_abuse_case_type_of_abuse_abuse_case; Type: INDEX; Schema: public; Owner: niels
--

CREATE INDEX fki_fk_abuse_case_type_of_abuse_abuse_case ON public.abuse_case_type_of_abuse USING btree (abuse_case_id);


--
-- TOC entry 4816 (class 1259 OID 2047161)
-- Name: fki_fk_abuse_case_type_of_abuse_type_of_abuse; Type: INDEX; Schema: public; Owner: niels
--

CREATE INDEX fki_fk_abuse_case_type_of_abuse_type_of_abuse ON public.abuse_case_type_of_abuse USING btree (type_of_abuse_id);


--
-- TOC entry 4811 (class 1259 OID 2047138)
-- Name: fki_fk_abuse_case_type_of_abuser_abuse_case; Type: INDEX; Schema: public; Owner: niels
--

CREATE INDEX fki_fk_abuse_case_type_of_abuser_abuse_case ON public.abuse_case_type_of_abuser USING btree (abuse_case_id);


--
-- TOC entry 4812 (class 1259 OID 2047144)
-- Name: fki_fk_abuse_case_type_of_abuser_type_of_abuser; Type: INDEX; Schema: public; Owner: niels
--

CREATE INDEX fki_fk_abuse_case_type_of_abuser_type_of_abuser ON public.abuse_case_type_of_abuser USING btree (type_of_abuser_id);


--
-- TOC entry 4734 (class 1259 OID 18065)
-- Name: fki_fk_abusers_relation_to_abused_id_nameable; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_abusers_relation_to_abused_id_nameable ON public.type_of_abuser USING btree (id);


--
-- TOC entry 4175 (class 1259 OID 18066)
-- Name: fki_fk_access_role_id_principal; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_access_role_id_principal ON public.access_role USING btree (id);


--
-- TOC entry 4178 (class 1259 OID 18067)
-- Name: fki_fk_access_role_privilege_access_role; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_access_role_privilege_access_role ON public.access_role_privilege USING btree (access_role_id);


--
-- TOC entry 4179 (class 1259 OID 18068)
-- Name: fki_fk_access_role_privilege_action; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_access_role_privilege_action ON public.access_role_privilege USING btree (action_id);


--
-- TOC entry 4182 (class 1259 OID 18069)
-- Name: fki_fk_act_id_collective; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_act_id_collective ON public.act USING btree (id);


--
-- TOC entry 4205 (class 1259 OID 18070)
-- Name: fki_fk_action_access_privilege_id_access_privilege; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_action_access_privilege_id_access_privilege ON public.basic_action USING btree (id);


--
-- TOC entry 4188 (class 1259 OID 18071)
-- Name: fki_fk_action_menu_item_id_menu_item; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_action_menu_item_id_menu_item ON public.action_menu_item USING btree (id);


--
-- TOC entry 4193 (class 1259 OID 18072)
-- Name: fki_fk_administor_role_tenant; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_administor_role_tenant ON public.administrator_role USING btree (user_group_id);


--
-- TOC entry 4194 (class 1259 OID 18073)
-- Name: fki_fk_administrator_role_user_role; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_administrator_role_user_role ON public.administrator_role USING btree (id);


--
-- TOC entry 4199 (class 1259 OID 18074)
-- Name: fki_fk_adoption_lawyer_id_professional_role; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_adoption_lawyer_id_professional_role ON public.adoption_lawyer USING btree (id);


--
-- TOC entry 4432 (class 1259 OID 18075)
-- Name: fki_fk_affiliation_organization_from; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_affiliation_organization_from ON public.inter_organizational_relation USING btree (organization_id_from);


--
-- TOC entry 4433 (class 1259 OID 18076)
-- Name: fki_fk_affiliation_organization_to; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_affiliation_organization_to ON public.inter_organizational_relation USING btree (id);


--
-- TOC entry 4434 (class 1259 OID 18077)
-- Name: fki_fk_affiliation_proof; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_affiliation_proof ON public.inter_organizational_relation USING btree (id);


--
-- TOC entry 4202 (class 1259 OID 18079)
-- Name: fki_fk_attachment_therapist_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_attachment_therapist_id ON public.attachment_therapist USING btree (id);


--
-- TOC entry 4206 (class 1259 OID 18080)
-- Name: fki_fk_basic_action_id_action; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_basic_action_id_action ON public.basic_action USING btree (id);


--
-- TOC entry 4211 (class 1259 OID 18081)
-- Name: fki_fk_basic_country_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_basic_country_id ON public.basic_country USING btree (id);


--
-- TOC entry 4214 (class 1259 OID 18082)
-- Name: fki_fk_basic_first_and_second_level_subdivision_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_basic_first_and_second_level_subdivision_id ON public.basic_first_and_second_level_subdivision USING btree (id);


--
-- TOC entry 4220 (class 1259 OID 18083)
-- Name: fki_fk_basic_secondary_subdivision_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_basic_secondary_subdivision_id ON public.basic_second_level_subdivision USING btree (id);


--
-- TOC entry 4223 (class 1259 OID 1209181)
-- Name: fki_fk_bill_act; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_bill_act ON public.bill USING btree (act_id);


--
-- TOC entry 4227 (class 1259 OID 18084)
-- Name: fki_fk_bill_action_nameable; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_bill_action_nameable ON public.bill_action_type USING btree (id);


--
-- TOC entry 4224 (class 1259 OID 18085)
-- Name: fki_fk_bill_id_collective; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_bill_id_collective ON public.bill USING btree (id);


--
-- TOC entry 4686 (class 1259 OID 18086)
-- Name: fki_fk_bla; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_bla ON public.tenant USING btree (access_role_id_not_logged_in);


--
-- TOC entry 4233 (class 1259 OID 18087)
-- Name: fki_fk_blog_post_node; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_blog_post_node ON public.blog_post USING btree (id);


--
-- TOC entry 4239 (class 1259 OID 18088)
-- Name: fki_fk_bound_country_top_level_country; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_bound_country_top_level_country ON public.bound_country USING btree (binding_country_id);


--
-- TOC entry 4230 (class 1259 OID 18089)
-- Name: fki_fk_bounding_country_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_bounding_country_id ON public.binding_country USING btree (id);


--
-- TOC entry 4245 (class 1259 OID 18090)
-- Name: fki_fk_case_case_parties_case; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_case_case_parties_case ON public.case_case_parties USING btree (case_id);


--
-- TOC entry 4246 (class 1259 OID 18091)
-- Name: fki_fk_case_case_parties_case_parties; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_case_case_parties_case_parties ON public.case_case_parties USING btree (case_parties_id);


--
-- TOC entry 4247 (class 1259 OID 18092)
-- Name: fki_fk_case_case_parties_case_party_type; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_case_case_parties_case_party_type ON public.case_case_parties USING btree (case_party_type_id);


--
-- TOC entry 4242 (class 1259 OID 18093)
-- Name: fki_fk_case_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_case_id ON public."case" USING btree (id);


--
-- TOC entry 4252 (class 1259 OID 18094)
-- Name: fki_fk_case_parties_organization_case_parties; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_case_parties_organization_case_parties ON public.case_parties_organization USING btree (case_parties_id);


--
-- TOC entry 4253 (class 1259 OID 18095)
-- Name: fki_fk_case_parties_organization_organization; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_case_parties_organization_organization ON public.case_parties_organization USING btree (organization_id);


--
-- TOC entry 4256 (class 1259 OID 18096)
-- Name: fki_fk_case_parties_person; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_case_parties_person ON public.case_parties_person USING btree (case_parties_id);


--
-- TOC entry 4257 (class 1259 OID 18097)
-- Name: fki_fk_case_parties_person_person; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_case_parties_person_person ON public.case_parties_person USING btree (person_id);


--
-- TOC entry 4260 (class 1259 OID 18098)
-- Name: fki_fk_case_party_type_id_nameable; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_case_party_type_id_nameable ON public.case_party_type USING btree (id);


--
-- TOC entry 4266 (class 1259 OID 18099)
-- Name: fki_fk_case_type_case_party_type_case_party_type; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_case_type_case_party_type_case_party_type ON public.case_type_case_party_type USING btree (case_party_type_id);


--
-- TOC entry 4267 (class 1259 OID 18100)
-- Name: fki_fk_case_type_case_party_type_case_type; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_case_type_case_party_type_case_type ON public.case_type_case_party_type USING btree (case_type_id);


--
-- TOC entry 4263 (class 1259 OID 18101)
-- Name: fki_fk_case_type_id_node_type; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_case_type_id_node_type ON public.case_type USING btree (id);


--
-- TOC entry 4771 (class 1259 OID 18102)
-- Name: fki_fk_category_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_category_id ON public.vocabulary USING btree (id);


--
-- TOC entry 4270 (class 1259 OID 18103)
-- Name: fki_fk_child_placement_type_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_child_placement_type_id ON public.child_placement_type USING btree (id);


--
-- TOC entry 4278 (class 1259 OID 18104)
-- Name: fki_fk_collective_id_published; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_collective_id_published ON public.collective USING btree (id);


--
-- TOC entry 4281 (class 1259 OID 18105)
-- Name: fki_fk_collective_user_user; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_collective_user_user ON public.collective_user USING btree (user_id);


--
-- TOC entry 4284 (class 1259 OID 18106)
-- Name: fki_fk_comment_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_comment_id ON public.comment USING btree (id);


--
-- TOC entry 4285 (class 1259 OID 18107)
-- Name: fki_fk_comment_node; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_comment_node ON public.comment USING btree (node_id);


--
-- TOC entry 4286 (class 1259 OID 18108)
-- Name: fki_fk_comment_publisher; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_comment_publisher ON public.comment USING btree (publisher_id);


--
-- TOC entry 4289 (class 1259 OID 18109)
-- Name: fki_fk_congressional_term_documentable; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_congressional_term_documentable ON public.congressional_term USING btree (id);


--
-- TOC entry 4292 (class 1259 OID 18110)
-- Name: fki_fk_congressional_term_political_party_affiliation_id_docume; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_congressional_term_political_party_affiliation_id_docume ON public.congressional_term_political_party_affiliation USING btree (id);


--
-- TOC entry 4293 (class 1259 OID 18111)
-- Name: fki_fk_congressional_term_political_party_affiliation_united_st; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_congressional_term_political_party_affiliation_united_st ON public.congressional_term_political_party_affiliation USING btree (united_states_political_party_affiliation_id);


--
-- TOC entry 4298 (class 1259 OID 18112)
-- Name: fki_fk_content_sharing_group_id_owner; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_content_sharing_group_id_owner ON public.content_sharing_group USING btree (id);


--
-- TOC entry 4383 (class 1259 OID 18113)
-- Name: fki_fk_continent_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_continent_id ON public.first_level_global_region USING btree (id);


--
-- TOC entry 4309 (class 1259 OID 18114)
-- Name: fki_fk_country_and_first_and_second_level_subdivision_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_country_and_first_and_second_level_subdivision_id ON public.country_and_first_and_second_level_subdivision USING btree (id);


--
-- TOC entry 4310 (class 1259 OID 18115)
-- Name: fki_fk_country_and_first_and_second_level_subdivision_id_2; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_country_and_first_and_second_level_subdivision_id_2 ON public.country_and_first_and_second_level_subdivision USING btree (id);


--
-- TOC entry 4313 (class 1259 OID 18116)
-- Name: fki_fk_country_and_first_level_subdivision_1; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_country_and_first_level_subdivision_1 ON public.country_and_first_level_subdivision USING btree (id);


--
-- TOC entry 4306 (class 1259 OID 18117)
-- Name: fki_fk_country_and_first_level_subdivision_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_country_and_first_level_subdivision_id ON public.country_and_first_and_bottom_level_subdivision USING btree (id);


--
-- TOC entry 4314 (class 1259 OID 18118)
-- Name: fki_fk_country_and_first_level_subdivision_id_2; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_country_and_first_level_subdivision_id_2 ON public.country_and_first_level_subdivision USING btree (id);


--
-- TOC entry 4317 (class 1259 OID 18119)
-- Name: fki_fk_country_and_intermediate_level_subdivision_1; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_country_and_intermediate_level_subdivision_1 ON public.country_and_intermediate_level_subdivision USING btree (id);


--
-- TOC entry 4301 (class 1259 OID 18120)
-- Name: fki_fk_country_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_country_id ON public.country USING btree (id);


--
-- TOC entry 4671 (class 1259 OID 18121)
-- Name: fki_fk_country_part_name_country; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_country_part_name_country ON public.subdivision USING btree (country_id);


--
-- TOC entry 4672 (class 1259 OID 18122)
-- Name: fki_fk_country_part_name_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_country_part_name_id ON public.subdivision USING btree (id);


--
-- TOC entry 4387 (class 1259 OID 18123)
-- Name: fki_fk_country_region_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_country_region_id ON public.first_level_subdivision USING btree (id);


--
-- TOC entry 4388 (class 1259 OID 18124)
-- Name: fki_fk_country_region_id_2; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_country_region_id_2 ON public.first_level_subdivision USING btree (id);


--
-- TOC entry 4320 (class 1259 OID 18125)
-- Name: fki_fk_country_report_country; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_country_report_country ON public.country_report USING btree (country_id);


--
-- TOC entry 4642 (class 1259 OID 18126)
-- Name: fki_fk_country_subdivision_country_id_1; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_country_subdivision_country_id_1 ON public.second_level_subdivision USING btree (id);


--
-- TOC entry 4324 (class 1259 OID 18127)
-- Name: fki_fk_country_subdivision_type; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_country_subdivision_type ON public.country_subdivision_type USING btree (country_id);


--
-- TOC entry 4325 (class 1259 OID 18128)
-- Name: fki_fk_country_subdivision_type_subdivision_type; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_country_subdivision_type_subdivision_type ON public.country_subdivision_type USING btree (subdivision_type_id);


--
-- TOC entry 4302 (class 1259 OID 1506301)
-- Name: fki_fk_country_vocabulary_subdivisions; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_country_vocabulary_subdivisions ON public.country USING btree (vocabulary_id_subdivisions);


--
-- TOC entry 4328 (class 1259 OID 18129)
-- Name: fki_fk_create_node_action_id_access_privilege; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_create_node_action_id_access_privilege ON public.create_node_action USING btree (id);


--
-- TOC entry 4329 (class 1259 OID 18130)
-- Name: fki_fk_create_node_action_node_type; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_create_node_action_node_type ON public.create_node_action USING btree (node_type_id);


--
-- TOC entry 4332 (class 1259 OID 18131)
-- Name: fki_fk_delete_node_action_id_access_privilege; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_delete_node_action_id_access_privilege ON public.delete_node_action USING btree (id);


--
-- TOC entry 4333 (class 1259 OID 18132)
-- Name: fki_fk_delete_node_action_node_type; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_delete_node_action_node_type ON public.delete_node_action USING btree (node_type_id);


--
-- TOC entry 4336 (class 1259 OID 18133)
-- Name: fki_fk_denomination_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_denomination_id ON public.denomination USING btree (id);


--
-- TOC entry 4339 (class 1259 OID 18134)
-- Name: fki_fk_deportation_case_country_id_to; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_deportation_case_country_id_to ON public.deportation_case USING btree (country_id_to);


--
-- TOC entry 4340 (class 1259 OID 18135)
-- Name: fki_fk_deportation_case_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_deportation_case_id ON public.deportation_case USING btree (id);


--
-- TOC entry 4341 (class 1259 OID 18136)
-- Name: fki_fk_deportation_case_subdivision_id_from; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_deportation_case_subdivision_id_from ON public.deportation_case USING btree (subdivision_id_from);


--
-- TOC entry 4344 (class 1259 OID 18137)
-- Name: fki_fk_discussion_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_discussion_id ON public.discussion USING btree (id);


--
-- TOC entry 4347 (class 1259 OID 18138)
-- Name: fki_fk_disrupted_placement_case_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_disrupted_placement_case_id ON public.disrupted_placement_case USING btree (id);


--
-- TOC entry 4350 (class 1259 OID 18139)
-- Name: fki_fk_document_document_type_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_document_document_type_id ON public.document USING btree (id);


--
-- TOC entry 4351 (class 1259 OID 18140)
-- Name: fki_fk_document_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_document_id ON public.document USING btree (id);


--
-- TOC entry 4352 (class 1259 OID 1317102)
-- Name: fki_fk_document_id_simple_text_node; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_document_id_simple_text_node ON public.document USING btree (id);


--
-- TOC entry 4355 (class 1259 OID 18141)
-- Name: fki_fk_document_type_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_document_type_id ON public.document_type USING btree (id);


--
-- TOC entry 4360 (class 1259 OID 18144)
-- Name: fki_fk_edit_node_action_id_action; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_edit_node_action_id_action ON public.edit_node_action USING btree (id);


--
-- TOC entry 4361 (class 1259 OID 18145)
-- Name: fki_fk_edit_node_action_node_type; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_edit_node_action_node_type ON public.edit_node_action USING btree (node_type_id);


--
-- TOC entry 4788 (class 1259 OID 192746)
-- Name: fki_fk_edit_own_node_action_id_action; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_edit_own_node_action_id_action ON public.edit_own_node_action USING btree (id);


--
-- TOC entry 4789 (class 1259 OID 192747)
-- Name: fki_fk_edit_own_node_action_node_type; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_edit_own_node_action_node_type ON public.edit_own_node_action USING btree (node_type_id);


--
-- TOC entry 4364 (class 1259 OID 18146)
-- Name: fki_fk_facilitator_id_organizational_role; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_facilitator_id_organizational_role ON public.facilitator USING btree (id);


--
-- TOC entry 4367 (class 1259 OID 18147)
-- Name: fki_fk_family_size_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_family_size_id ON public.family_size USING btree (id);


--
-- TOC entry 4370 (class 1259 OID 18148)
-- Name: fki_fk_fathers_rights_violations_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_fathers_rights_violations_id ON public.fathers_rights_violation_case USING btree (id);


--
-- TOC entry 4560 (class 1259 OID 18149)
-- Name: fki_fk_file_id_file_portrait; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_file_id_file_portrait ON public.person USING btree (file_id_portrait);


--
-- TOC entry 4375 (class 1259 OID 18150)
-- Name: fki_fk_first_and_bottom_level_subdivision_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_first_and_bottom_level_subdivision_id ON public.first_and_bottom_level_subdivision USING btree (id);


--
-- TOC entry 4376 (class 1259 OID 18151)
-- Name: fki_fk_first_and_bottom_level_subdivision_id_02; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_first_and_bottom_level_subdivision_id_02 ON public.first_and_bottom_level_subdivision USING btree (id);


--
-- TOC entry 4379 (class 1259 OID 18152)
-- Name: fki_fk_first_and_second_level_subdivision_id_1; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_first_and_second_level_subdivision_id_1 ON public.first_and_second_level_subdivision USING btree (id);


--
-- TOC entry 4380 (class 1259 OID 18153)
-- Name: fki_fk_first_and_second_level_subdivision_id_2; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_first_and_second_level_subdivision_id_2 ON public.first_and_second_level_subdivision USING btree (id);


--
-- TOC entry 4384 (class 1259 OID 18154)
-- Name: fki_fk_first_level_global_region_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_first_level_global_region_id ON public.first_level_global_region USING btree (id);


--
-- TOC entry 4389 (class 1259 OID 18155)
-- Name: fki_fk_formal_first_level_subdivision_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_formal_first_level_subdivision_id ON public.formal_intermediate_level_subdivision USING btree (id);


--
-- TOC entry 4577 (class 1259 OID 18156)
-- Name: fki_fk_geographical_entity_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_geographical_entity_id ON public.political_entity USING btree (id);


--
-- TOC entry 4392 (class 1259 OID 18157)
-- Name: fki_fk_geographical_entity_id_2; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_geographical_entity_id_2 ON public.geographical_entity USING btree (id);


--
-- TOC entry 4395 (class 1259 OID 18158)
-- Name: fki_fk_global_region_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_global_region_id ON public.global_region USING btree (id);


--
-- TOC entry 4398 (class 1259 OID 18159)
-- Name: fki_fk_hague_status_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_hague_status_id ON public.hague_status USING btree (id);


--
-- TOC entry 4401 (class 1259 OID 18160)
-- Name: fki_fk_home_study_agency_id_organization_role; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_home_study_agency_id_organization_role ON public.home_study_agency USING btree (id);


--
-- TOC entry 4404 (class 1259 OID 18161)
-- Name: fki_fk_house_bill_bill; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_house_bill_bill ON public.house_bill USING btree (id);


--
-- TOC entry 4407 (class 1259 OID 18162)
-- Name: fki_fk_house_term_id_node; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_house_term_id_node ON public.house_term USING btree (id);


--
-- TOC entry 4408 (class 1259 OID 18163)
-- Name: fki_fk_house_term_representative; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_house_term_representative ON public.house_term USING btree (representative_id);


--
-- TOC entry 4409 (class 1259 OID 18164)
-- Name: fki_fk_house_term_subdivision; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_house_term_subdivision ON public.house_term USING btree (subdivision_id);


--
-- TOC entry 4412 (class 1259 OID 18165)
-- Name: fki_fk_informal_first_level_subdivision_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_informal_first_level_subdivision_id ON public.informal_intermediate_level_subdivision USING btree (id);


--
-- TOC entry 4415 (class 1259 OID 18166)
-- Name: fki_fk_institution_id_organizational_role; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_institution_id_organizational_role ON public.institution USING btree (id);


--
-- TOC entry 4435 (class 1259 OID 18167)
-- Name: fki_fk_inter_collective_relation_political_entity; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_inter_collective_relation_political_entity ON public.inter_organizational_relation USING btree (geographical_entity_id);


--
-- TOC entry 4418 (class 1259 OID 18168)
-- Name: fki_fk_inter_country_relation_country_from; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_inter_country_relation_country_from ON public.inter_country_relation USING btree (country_id_from);


--
-- TOC entry 4419 (class 1259 OID 18169)
-- Name: fki_fk_inter_country_relation_country_to; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_inter_country_relation_country_to ON public.inter_country_relation USING btree (country_id_to);


--
-- TOC entry 4420 (class 1259 OID 18170)
-- Name: fki_fk_inter_country_relation_document_id_proof; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_inter_country_relation_document_id_proof ON public.inter_country_relation USING btree (document_id_proof);


--
-- TOC entry 4421 (class 1259 OID 18171)
-- Name: fki_fk_inter_country_relation_id_node; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_inter_country_relation_id_node ON public.inter_country_relation USING btree (id);


--
-- TOC entry 4422 (class 1259 OID 18172)
-- Name: fki_fk_inter_country_relation_inter_country_relation_type; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_inter_country_relation_inter_country_relation_type ON public.inter_country_relation USING btree (inter_country_relation_type_id);


--
-- TOC entry 4427 (class 1259 OID 18173)
-- Name: fki_fk_inter_country_relation_type_id_nameable; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_inter_country_relation_type_id_nameable ON public.inter_country_relation_type USING btree (id);


--
-- TOC entry 4443 (class 1259 OID 18174)
-- Name: fki_fk_inter_personal_relation_id_documentable; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_inter_personal_relation_id_documentable ON public.inter_personal_relation USING btree (id);


--
-- TOC entry 4454 (class 1259 OID 18175)
-- Name: fki_fk_intermediate_level_subdivision_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_intermediate_level_subdivision_id ON public.intermediate_level_subdivision USING btree (id);


--
-- TOC entry 4457 (class 1259 OID 18176)
-- Name: fki_fk_iso_coded_first_level_subdivision_1; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_iso_coded_first_level_subdivision_1 ON public.iso_coded_first_level_subdivision USING btree (id);


--
-- TOC entry 4458 (class 1259 OID 18177)
-- Name: fki_fk_iso_coded_first_level_subdivision_2; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_iso_coded_first_level_subdivision_2 ON public.iso_coded_first_level_subdivision USING btree (id);


--
-- TOC entry 4466 (class 1259 OID 18178)
-- Name: fki_fk_law_firm_organizational_role; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_law_firm_organizational_role ON public.law_firm USING btree (id);


--
-- TOC entry 4475 (class 1259 OID 18179)
-- Name: fki_fk_location_subdivision; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_location_subdivision ON public.location USING btree (subdivision_id);


--
-- TOC entry 4476 (class 1259 OID 18180)
-- Name: fki_fk_location_subdivision_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_location_subdivision_id ON public.location USING btree (subdivision_id);


--
-- TOC entry 4483 (class 1259 OID 18181)
-- Name: fki_fk_member_of_congress_political_entity_relation; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_member_of_congress_political_entity_relation ON public.member_of_congress USING btree (id);


--
-- TOC entry 4488 (class 1259 OID 18182)
-- Name: fki_fk_multi_question_poll_id_poll; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_multi_question_poll_id_poll ON public.multi_question_poll USING btree (id);


--
-- TOC entry 4491 (class 1259 OID 18183)
-- Name: fki_fk_multi_question_poll_question_multi_question_poll; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_multi_question_poll_question_multi_question_poll ON public.multi_question_poll_poll_question USING btree (multi_question_poll_id);


--
-- TOC entry 4492 (class 1259 OID 18184)
-- Name: fki_fk_multi_question_poll_question_poll_question; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_multi_question_poll_question_poll_question ON public.multi_question_poll_poll_question USING btree (poll_question_id);


--
-- TOC entry 4497 (class 1259 OID 18185)
-- Name: fki_fk_nameable_file_tile_image; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_nameable_file_tile_image ON public.nameable USING btree (file_id_tile_image);


--
-- TOC entry 4800 (class 1259 OID 1937575)
-- Name: fki_fk_nameable_type_id_node_type; Type: INDEX; Schema: public; Owner: niels
--

CREATE INDEX fki_fk_nameable_type_id_node_type ON public.nameable_type USING btree (id);


--
-- TOC entry 4507 (class 1259 OID 18186)
-- Name: fki_fk_node_file_file; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_node_file_file ON public.node_file USING btree (file_id);


--
-- TOC entry 4508 (class 1259 OID 18187)
-- Name: fki_fk_node_file_node; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_node_file_node ON public.node_file USING btree (node_id);


--
-- TOC entry 4511 (class 1259 OID 18188)
-- Name: fki_fk_node_term_node; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_node_term_node ON public.node_term USING btree (node_id);


--
-- TOC entry 4512 (class 1259 OID 18189)
-- Name: fki_fk_node_term_term; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_node_term_term ON public.node_term USING btree (term_id);


--
-- TOC entry 4501 (class 1259 OID 18190)
-- Name: fki_fk_node_user; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_node_user ON public.node USING btree (publisher_id);


--
-- TOC entry 4502 (class 1259 OID 18191)
-- Name: fki_fk_node_user_group; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_node_user_group ON public.node USING btree (owner_id);


--
-- TOC entry 4524 (class 1259 OID 18192)
-- Name: fki_fk_organization_act_relation_type; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_organization_act_relation_type ON public.organization_act_relation_type USING btree (id);


--
-- TOC entry 4519 (class 1259 OID 18193)
-- Name: fki_fk_organization_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_organization_id ON public.organization USING btree (id);


--
-- TOC entry 4520 (class 1259 OID 18194)
-- Name: fki_fk_organization_id_collective; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_organization_id_collective ON public.organization USING btree (id);


--
-- TOC entry 4521 (class 1259 OID 18195)
-- Name: fki_fk_organization_organization_type; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_organization_organization_type ON public.organization USING btree (id);


--
-- TOC entry 4527 (class 1259 OID 18196)
-- Name: fki_fk_organization_organization_type_organization; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_organization_organization_type_organization ON public.organization_organization_type USING btree (organization_id);


--
-- TOC entry 4528 (class 1259 OID 18197)
-- Name: fki_fk_organization_organization_type_organization_type; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_organization_organization_type_organization_type ON public.organization_organization_type USING btree (organization_type_id);


--
-- TOC entry 4531 (class 1259 OID 18198)
-- Name: fki_fk_organization_type_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_organization_type_id ON public.organization_type USING btree (id);


--
-- TOC entry 4534 (class 1259 OID 18199)
-- Name: fki_fk_organizational_role_organization; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_organizational_role_organization ON public.organizational_role USING btree (organization_id);


--
-- TOC entry 4535 (class 1259 OID 18200)
-- Name: fki_fk_organizational_role_organization_type; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_organizational_role_organization_type ON public.organizational_role USING btree (organization_type_id);


--
-- TOC entry 4543 (class 1259 OID 18201)
-- Name: fki_fk_page_id_simple_text_node; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_page_id_simple_text_node ON public.page USING btree (id);


--
-- TOC entry 4546 (class 1259 OID 18202)
-- Name: fki_fk_party; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_party ON public.party USING btree (id);


--
-- TOC entry 4793 (class 1259 OID 1263453)
-- Name: fki_fk_party_act_relation_act; Type: INDEX; Schema: public; Owner: niels
--

CREATE INDEX fki_fk_party_act_relation_act ON public.party_act_relation USING btree (act_id);


--
-- TOC entry 4794 (class 1259 OID 1263441)
-- Name: fki_fk_party_act_relation_id_node; Type: INDEX; Schema: public; Owner: niels
--

CREATE INDEX fki_fk_party_act_relation_id_node ON public.party_act_relation USING btree (id);


--
-- TOC entry 4795 (class 1259 OID 1263447)
-- Name: fki_fk_party_act_relation_party; Type: INDEX; Schema: public; Owner: niels
--

CREATE INDEX fki_fk_party_act_relation_party ON public.party_act_relation USING btree (party_id);


--
-- TOC entry 4796 (class 1259 OID 1263465)
-- Name: fki_fk_party_act_relation_party_document; Type: INDEX; Schema: public; Owner: niels
--

CREATE INDEX fki_fk_party_act_relation_party_document ON public.party_act_relation USING btree (document_id_proof);


--
-- TOC entry 4797 (class 1259 OID 1263459)
-- Name: fki_fk_party_act_relation_party_party_act_relation_party_type; Type: INDEX; Schema: public; Owner: niels
--

CREATE INDEX fki_fk_party_act_relation_party_party_act_relation_party_type ON public.party_act_relation USING btree (party_act_relation_type_id);


--
-- TOC entry 4547 (class 1259 OID 18203)
-- Name: fki_fk_party_id_nameable; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_party_id_nameable ON public.party USING btree (id);


--
-- TOC entry 4548 (class 1259 OID 18204)
-- Name: fki_fk_party_location; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_party_location ON public.party USING btree (id);


--
-- TOC entry 4551 (class 1259 OID 18205)
-- Name: fki_fk_party_political_entity_relation_document_proof; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_party_political_entity_relation_document_proof ON public.party_political_entity_relation USING btree (document_id_proof);


--
-- TOC entry 4552 (class 1259 OID 18206)
-- Name: fki_fk_party_political_entity_relation_political_entity; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_party_political_entity_relation_political_entity ON public.party_political_entity_relation USING btree (political_entity_id);


--
-- TOC entry 4553 (class 1259 OID 18207)
-- Name: fki_fk_party_political_entity_relation_political_entity_relatab; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_party_political_entity_relation_political_entity_relatab ON public.party_political_entity_relation USING btree (party_id);


--
-- TOC entry 4554 (class 1259 OID 18208)
-- Name: fki_fk_party_political_entity_relation_political_entity_relatio; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_party_political_entity_relation_political_entity_relatio ON public.party_political_entity_relation USING btree (party_political_entity_relation_type_id);


--
-- TOC entry 4555 (class 1259 OID 18209)
-- Name: fki_fk_party_politicial_entity_relation_id_documentable; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_party_politicial_entity_relation_id_documentable ON public.party_political_entity_relation USING btree (id);


--
-- TOC entry 4564 (class 1259 OID 18210)
-- Name: fki_fk_person_collective_relation_person; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_person_collective_relation_person ON public.person_organization_relation USING btree (person_id);


--
-- TOC entry 4565 (class 1259 OID 18211)
-- Name: fki_fk_person_collective_relation_person_collective_relation_ty; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_person_collective_relation_person_collective_relation_ty ON public.person_organization_relation USING btree (person_organization_relation_type_id);


--
-- TOC entry 4561 (class 1259 OID 18212)
-- Name: fki_fk_person_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_person_id ON public.person USING btree (id);


--
-- TOC entry 4566 (class 1259 OID 18213)
-- Name: fki_fk_person_organization_relation_id_documentable; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_person_organization_relation_id_documentable ON public.person_organization_relation USING btree (id);


--
-- TOC entry 4444 (class 1259 OID 18214)
-- Name: fki_fk_personal_relationship_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_personal_relationship_id ON public.inter_personal_relation USING btree (id);


--
-- TOC entry 4445 (class 1259 OID 18215)
-- Name: fki_fk_personal_relationship_person_from; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_personal_relationship_person_from ON public.inter_personal_relation USING btree (person_id_from);


--
-- TOC entry 4446 (class 1259 OID 18216)
-- Name: fki_fk_personal_relationship_person_to; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_personal_relationship_person_to ON public.inter_personal_relation USING btree (person_id_to);


--
-- TOC entry 4447 (class 1259 OID 18217)
-- Name: fki_fk_personal_relationship_personal_relationship_type; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_personal_relationship_personal_relationship_type ON public.inter_personal_relation USING btree (inter_personal_relation_type_id);


--
-- TOC entry 4448 (class 1259 OID 18218)
-- Name: fki_fk_personal_relationship_proof; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_personal_relationship_proof ON public.inter_personal_relation USING btree (document_id_proof);


--
-- TOC entry 4451 (class 1259 OID 18219)
-- Name: fki_fk_personal_relationship_type_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_personal_relationship_type_id ON public.inter_personal_relation_type USING btree (id);


--
-- TOC entry 4574 (class 1259 OID 18220)
-- Name: fki_fk_placement_agency_id_organizational_role; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_placement_agency_id_organizational_role ON public.placement_agency USING btree (id);


--
-- TOC entry 4578 (class 1259 OID 18221)
-- Name: fki_fk_political_entity_file_flag; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_political_entity_file_flag ON public.political_entity USING btree (file_id_flag);


--
-- TOC entry 4584 (class 1259 OID 18222)
-- Name: fki_fk_poll_option_pole; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_poll_option_pole ON public.poll_option USING btree (poll_question_id);


--
-- TOC entry 4581 (class 1259 OID 18223)
-- Name: fki_fk_poll_simple_text_node; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_poll_simple_text_node ON public.poll USING btree (id);


--
-- TOC entry 4594 (class 1259 OID 18224)
-- Name: fki_fk_poll_vote_user; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_poll_vote_user ON public.poll_vote USING btree (user_id);


--
-- TOC entry 4598 (class 1259 OID 18225)
-- Name: fki_fk_post_placement_agency_id_organizational_role; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_post_placement_agency_id_organizational_role ON public.post_placement_agency USING btree (id);


--
-- TOC entry 4603 (class 1259 OID 18226)
-- Name: fki_fk_profession_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_profession_id ON public.profession USING btree (id);


--
-- TOC entry 4606 (class 1259 OID 18227)
-- Name: fki_fk_professional_role_profession; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_professional_role_profession ON public.professional_role USING btree (profession_id);


--
-- TOC entry 4615 (class 1259 OID 18228)
-- Name: fki_fk_publisher_id_principal; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_publisher_id_principal ON public.publisher USING btree (id);


--
-- TOC entry 4782 (class 1259 OID 139274)
-- Name: fki_fk_publishing_user_group_id_user_group; Type: INDEX; Schema: public; Owner: niels
--

CREATE INDEX fki_fk_publishing_user_group_id_user_group ON public.publishing_user_group USING btree (id);


--
-- TOC entry 4783 (class 1259 OID 139280)
-- Name: fki_fk_publishing_user_group_publication_status_default; Type: INDEX; Schema: public; Owner: niels
--

CREATE INDEX fki_fk_publishing_user_group_publication_status_default ON public.publishing_user_group USING btree (publication_status_id_default);


--
-- TOC entry 4637 (class 1259 OID 18229)
-- Name: fki_fk_region_continent; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_region_continent ON public.second_level_global_region USING btree (first_level_global_region_id);


--
-- TOC entry 4638 (class 1259 OID 18230)
-- Name: fki_fk_region_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_region_id ON public.second_level_global_region USING btree (id);


--
-- TOC entry 4623 (class 1259 OID 18231)
-- Name: fki_fk_representative_house_bill_bill_action_bill_action; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_representative_house_bill_bill_action_bill_action ON public.representative_house_bill_action USING btree (bill_action_type_id);


--
-- TOC entry 4624 (class 1259 OID 18232)
-- Name: fki_fk_representative_house_bill_bill_action_house_bill; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_representative_house_bill_bill_action_house_bill ON public.representative_house_bill_action USING btree (house_bill_id);


--
-- TOC entry 4625 (class 1259 OID 18233)
-- Name: fki_fk_representative_house_bill_bill_action_representative; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_representative_house_bill_bill_action_representative ON public.representative_house_bill_action USING btree (representative_id);


--
-- TOC entry 4620 (class 1259 OID 18234)
-- Name: fki_fk_representative_member_of_congress; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_representative_member_of_congress ON public.representative USING btree (id);


--
-- TOC entry 4630 (class 1259 OID 18235)
-- Name: fki_fk_review_id_simple_text_node; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_review_id_simple_text_node ON public.review USING btree (id);


--
-- TOC entry 4633 (class 1259 OID 18236)
-- Name: fki_fk_searchable_node; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_searchable_node ON public.searchable USING btree (id);


--
-- TOC entry 4645 (class 1259 OID 18237)
-- Name: fki_fk_senate_bill; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_senate_bill ON public.senate_bill USING btree (id);


--
-- TOC entry 4646 (class 1259 OID 18238)
-- Name: fki_fk_senate_bill_id_bill; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_senate_bill_id_bill ON public.senate_bill USING btree (id);


--
-- TOC entry 4649 (class 1259 OID 18239)
-- Name: fki_fk_senate_term_id_node; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_senate_term_id_node ON public.senate_term USING btree (id);


--
-- TOC entry 4650 (class 1259 OID 18240)
-- Name: fki_fk_senate_term_senator; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_senate_term_senator ON public.senate_term USING btree (senator_id);


--
-- TOC entry 4651 (class 1259 OID 18241)
-- Name: fki_fk_senate_term_subdivision; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_senate_term_subdivision ON public.senate_term USING btree (subdivision_id);


--
-- TOC entry 4654 (class 1259 OID 18242)
-- Name: fki_fk_senator_member_of_congress; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_senator_member_of_congress ON public.senator USING btree (id);


--
-- TOC entry 4657 (class 1259 OID 18243)
-- Name: fki_fk_senator_senate_bill_bill_action_bill_action; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_senator_senate_bill_bill_action_bill_action ON public.senator_senate_bill_action USING btree (bill_action_type_id);


--
-- TOC entry 4658 (class 1259 OID 18244)
-- Name: fki_fk_senator_senate_bill_bill_action_senate_bill; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_senator_senate_bill_bill_action_senate_bill ON public.senator_senate_bill_action USING btree (senate_bill_id);


--
-- TOC entry 4659 (class 1259 OID 18245)
-- Name: fki_fk_senator_senate_bill_bill_action_senator; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_senator_senate_bill_bill_action_senator ON public.senator_senate_bill_action USING btree (senator_id);


--
-- TOC entry 4664 (class 1259 OID 18246)
-- Name: fki_fk_simple_text_node_id_node; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_simple_text_node_id_node ON public.simple_text_node USING btree (id);


--
-- TOC entry 4667 (class 1259 OID 18247)
-- Name: fki_fk_single_question_poll_id_poll; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_single_question_poll_id_poll ON public.single_question_poll USING btree (id);


--
-- TOC entry 4668 (class 1259 OID 18248)
-- Name: fki_fk_single_question_poll_id_poll_question; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_single_question_poll_id_poll_question ON public.single_question_poll USING btree (id);


--
-- TOC entry 4673 (class 1259 OID 18249)
-- Name: fki_fk_subdivision_country_subdivision; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_subdivision_country_subdivision ON public.subdivision USING btree (country_id, subdivision_type_id);


--
-- TOC entry 4680 (class 1259 OID 18250)
-- Name: fki_fk_subgroup_tenant; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_subgroup_tenant ON public.subgroup USING btree (tenant_id);


--
-- TOC entry 4683 (class 1259 OID 18251)
-- Name: fki_fk_system_group_user_group; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_system_group_user_group ON public.system_group USING btree (id);


--
-- TOC entry 4696 (class 1259 OID 18252)
-- Name: fki_fk_tenant_file_file; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_tenant_file_file ON public.tenant_file USING btree (file_id);


--
-- TOC entry 4697 (class 1259 OID 18253)
-- Name: fki_fk_tenant_file_tenant; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_tenant_file_tenant ON public.tenant_file USING btree (tenant_id);


--
-- TOC entry 4687 (class 1259 OID 139286)
-- Name: fki_fk_tenant_id_publishing_user_group; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_tenant_id_publishing_user_group ON public.tenant USING btree (id);


--
-- TOC entry 4710 (class 1259 OID 18254)
-- Name: fki_fk_tenant_node_menu_item_id_menu_item; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_tenant_node_menu_item_id_menu_item ON public.tenant_node_menu_item USING btree (id);


--
-- TOC entry 4711 (class 1259 OID 18255)
-- Name: fki_fk_tenant_node_menu_item_tenant_node; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_tenant_node_menu_item_tenant_node ON public.tenant_node_menu_item USING btree (tenant_node_id);


--
-- TOC entry 4700 (class 1259 OID 18256)
-- Name: fki_fk_tenant_node_publication_status; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_tenant_node_publication_status ON public.tenant_node USING btree (publication_status_id);


--
-- TOC entry 4701 (class 1259 OID 18257)
-- Name: fki_fk_tenant_node_subgroup; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_tenant_node_subgroup ON public.tenant_node USING btree (subgroup_id);


--
-- TOC entry 4702 (class 1259 OID 18258)
-- Name: fki_fk_tenant_node_tenant; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_tenant_node_tenant ON public.tenant_node USING btree (tenant_id);


--
-- TOC entry 4688 (class 1259 OID 18259)
-- Name: fki_fk_tenant_user_group; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_tenant_user_group ON public.tenant USING btree (id);


--
-- TOC entry 4689 (class 1259 OID 18260)
-- Name: fki_fk_tenant_user_role_id_not_logged_in; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_tenant_user_role_id_not_logged_in ON public.tenant USING btree (access_role_id_not_logged_in);


--
-- TOC entry 4690 (class 1259 OID 18261)
-- Name: fki_fk_tenant_vocabulary_tagging; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_tenant_vocabulary_tagging ON public.tenant USING btree (vocabulary_id_tagging);


--
-- TOC entry 4723 (class 1259 OID 18262)
-- Name: fki_fk_term_hierarchy_parent; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_term_hierarchy_parent ON public.term_hierarchy USING btree (term_id_parent);


--
-- TOC entry 4498 (class 1259 OID 18263)
-- Name: fki_fk_term_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_term_id ON public.nameable USING btree (id);


--
-- TOC entry 4716 (class 1259 OID 18264)
-- Name: fki_fk_term_nameable; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_term_nameable ON public.term USING btree (nameable_id);


--
-- TOC entry 4725 (class 1259 OID 18265)
-- Name: fki_fk_top_level_country_country; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_top_level_country_country ON public.top_level_country USING btree (id);


--
-- TOC entry 4726 (class 1259 OID 18266)
-- Name: fki_fk_top_level_country_global_region; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_top_level_country_global_region ON public.top_level_country USING btree (global_region_id);


--
-- TOC entry 4731 (class 1259 OID 18267)
-- Name: fki_fk_type_of_abuse_id_nameable; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_type_of_abuse_id_nameable ON public.type_of_abuse USING btree (id);


--
-- TOC entry 4737 (class 1259 OID 18268)
-- Name: fki_fk_united_states_congressional_meetings_documentable; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_united_states_congressional_meetings_documentable ON public.united_states_congressional_meeting USING btree (id);


--
-- TOC entry 4738 (class 1259 OID 18269)
-- Name: fki_fk_united_states_congressional_meetings_nameable; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_united_states_congressional_meetings_nameable ON public.united_states_congressional_meeting USING btree (id);


--
-- TOC entry 4748 (class 1259 OID 18270)
-- Name: fki_fk_united_states_politcal_party_affiliation_united_states_p; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_united_states_politcal_party_affiliation_united_states_p ON public.united_states_political_party_affiliation USING btree (united_states_political_party_id);


--
-- TOC entry 4749 (class 1259 OID 18271)
-- Name: fki_fk_united_states_political_party_affiliation_id_nameable; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_united_states_political_party_affiliation_id_nameable ON public.united_states_political_party_affiliation USING btree (id);


--
-- TOC entry 4745 (class 1259 OID 18272)
-- Name: fki_fk_united_states_political_party_id_organization; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_united_states_political_party_id_organization ON public.united_states_political_party USING btree (id);


--
-- TOC entry 4752 (class 1259 OID 18273)
-- Name: fki_fk_user_id_access_role; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_user_id_access_role ON public."user" USING btree (id);


--
-- TOC entry 4765 (class 1259 OID 18274)
-- Name: fki_fk_user_role_user_group; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_user_role_user_group ON public.user_role USING btree (user_group_id);


--
-- TOC entry 4760 (class 1259 OID 18275)
-- Name: fki_fk_user_role_user_user; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_user_role_user_user ON public.user_group_user_role_user USING btree (user_id);


--
-- TOC entry 4761 (class 1259 OID 18276)
-- Name: fki_fk_user_role_user_user_group; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_user_role_user_user_group ON public.user_group_user_role_user USING btree (user_group_id);


--
-- TOC entry 4762 (class 1259 OID 18277)
-- Name: fki_fk_user_role_user_user_role; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_user_role_user_user_role ON public.user_group_user_role_user USING btree (user_role_id);


--
-- TOC entry 4803 (class 1259 OID 1992415)
-- Name: fki_fk_view_node_type_list_action_id_basic_action; Type: INDEX; Schema: public; Owner: niels
--

CREATE INDEX fki_fk_view_node_type_list_action_id_basic_action ON public.view_node_type_list_action USING btree (basic_action_id);


--
-- TOC entry 4804 (class 1259 OID 1992421)
-- Name: fki_fk_view_node_type_list_action_node_type; Type: INDEX; Schema: public; Owner: niels
--

CREATE INDEX fki_fk_view_node_type_list_action_node_type ON public.view_node_type_list_action USING btree (node_type_id);


--
-- TOC entry 4776 (class 1259 OID 18278)
-- Name: fki_fk_wrongful_medication_case_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_wrongful_medication_case_id ON public.wrongful_medication_case USING btree (id);


--
-- TOC entry 4779 (class 1259 OID 18279)
-- Name: fki_fk_wrongful_removal_case_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_fk_wrongful_removal_case_id ON public.wrongful_removal_case USING btree (id);


--
-- TOC entry 4503 (class 1259 OID 18280)
-- Name: fki_g; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_g ON public.node USING btree (node_type_id);


--
-- TOC entry 4571 (class 1259 OID 18281)
-- Name: fki_h; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_h ON public.person_organization_relation_type USING btree (id);


--
-- TOC entry 4436 (class 1259 OID 18282)
-- Name: fki_inter_organizational_relation_id_node; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_inter_organizational_relation_id_node ON public.inter_organizational_relation USING btree (id);


--
-- TOC entry 4440 (class 1259 OID 18283)
-- Name: fki_j; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_j ON public.inter_organizational_relation_type USING btree (id);


--
-- TOC entry 4437 (class 1259 OID 18284)
-- Name: fki_k; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_k ON public.inter_organizational_relation USING btree (id);


--
-- TOC entry 4471 (class 1259 OID 18285)
-- Name: fki_l; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_l ON public.locatable USING btree (id);


--
-- TOC entry 4540 (class 1259 OID 18286)
-- Name: fki_o; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_o ON public.owner USING btree (id);


--
-- TOC entry 4595 (class 1259 OID 18287)
-- Name: fki_p; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_p ON public.poll_vote USING btree (poll_id, delta);


--
-- TOC entry 4790 (class 1259 OID 1263428)
-- Name: fki_party_act_relation_type_id_nameable; Type: INDEX; Schema: public; Owner: niels
--

CREATE INDEX fki_party_act_relation_type_id_nameable ON public.party_act_relation_type USING btree (id);


--
-- TOC entry 4567 (class 1259 OID 18288)
-- Name: fki_person_collective_relation_collective; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_person_collective_relation_collective ON public.person_organization_relation USING btree (organization_id);


--
-- TOC entry 4568 (class 1259 OID 18289)
-- Name: fki_person_organization_relation_political_entity; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_person_organization_relation_political_entity ON public.person_organization_relation USING btree (geographical_entity_id);


--
-- TOC entry 4703 (class 1259 OID 18290)
-- Name: fki_r; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_r ON public.tenant_node USING btree (node_id);


--
-- TOC entry 4691 (class 1259 OID 262150)
-- Name: fki_tenant_country_default; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_tenant_country_default ON public.tenant USING btree (country_id_default);


--
-- TOC entry 4587 (class 1259 OID 18291)
-- Name: fki_tk_poll_question_id_simple_text_node; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_tk_poll_question_id_simple_text_node ON public.poll_question USING btree (id);


--
-- TOC entry 4753 (class 1259 OID 18292)
-- Name: fki_u; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_u ON public."user" USING btree (id);


--
-- TOC entry 4766 (class 1259 OID 18293)
-- Name: fki_user_role_access_role; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_user_role_access_role ON public.user_role USING btree (id);


--
-- TOC entry 4303 (class 1259 OID 18294)
-- Name: fki_v; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX fki_v ON public.country USING btree (hague_status_id);


--
-- TOC entry 4321 (class 1259 OID 18295)
-- Name: idx_country_year; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_country_year ON public.country_report USING btree (country_id, date_range);


--
-- TOC entry 4724 (class 1259 OID 18296)
-- Name: idx_term_id_child; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_term_id_child ON public.term_hierarchy USING btree (term_id_child);


--
-- TOC entry 4506 (class 1259 OID 18297)
-- Name: node_trgm_idx; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX node_trgm_idx ON public.node USING gist (title public.gist_trgm_ops);


--
-- TOC entry 4636 (class 1259 OID 18298)
-- Name: searchable_tsvector_idx; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX searchable_tsvector_idx ON public.searchable USING gin (tsvector);


--
-- TOC entry 4817 (class 2606 OID 18299)
-- Name: abuse_case fk_abuse_case_child_placement_type; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.abuse_case
    ADD CONSTRAINT fk_abuse_case_child_placement_type FOREIGN KEY (child_placement_type_id) REFERENCES public.child_placement_type(id) NOT VALID;


--
-- TOC entry 4818 (class 2606 OID 18304)
-- Name: abuse_case fk_abuse_case_family_size; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.abuse_case
    ADD CONSTRAINT fk_abuse_case_family_size FOREIGN KEY (family_size_id) REFERENCES public.family_size(id) NOT VALID;


--
-- TOC entry 4819 (class 2606 OID 18309)
-- Name: abuse_case fk_abuse_case_id_case; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.abuse_case
    ADD CONSTRAINT fk_abuse_case_id_case FOREIGN KEY (id) REFERENCES public."case"(id) NOT VALID;


--
-- TOC entry 5092 (class 2606 OID 2047150)
-- Name: abuse_case_type_of_abuse fk_abuse_case_type_of_abuse_abuse_case; Type: FK CONSTRAINT; Schema: public; Owner: niels
--

ALTER TABLE ONLY public.abuse_case_type_of_abuse
    ADD CONSTRAINT fk_abuse_case_type_of_abuse_abuse_case FOREIGN KEY (abuse_case_id) REFERENCES public.abuse_case(id);


--
-- TOC entry 5093 (class 2606 OID 2047155)
-- Name: abuse_case_type_of_abuse fk_abuse_case_type_of_abuse_type_of_abuse; Type: FK CONSTRAINT; Schema: public; Owner: niels
--

ALTER TABLE ONLY public.abuse_case_type_of_abuse
    ADD CONSTRAINT fk_abuse_case_type_of_abuse_type_of_abuse FOREIGN KEY (type_of_abuse_id) REFERENCES public.type_of_abuse(id);


--
-- TOC entry 5090 (class 2606 OID 2047133)
-- Name: abuse_case_type_of_abuser fk_abuse_case_type_of_abuser_abuse_case; Type: FK CONSTRAINT; Schema: public; Owner: niels
--

ALTER TABLE ONLY public.abuse_case_type_of_abuser
    ADD CONSTRAINT fk_abuse_case_type_of_abuser_abuse_case FOREIGN KEY (abuse_case_id) REFERENCES public.abuse_case(id) NOT VALID;


--
-- TOC entry 5091 (class 2606 OID 2047139)
-- Name: abuse_case_type_of_abuser fk_abuse_case_type_of_abuser_type_of_abuser; Type: FK CONSTRAINT; Schema: public; Owner: niels
--

ALTER TABLE ONLY public.abuse_case_type_of_abuser
    ADD CONSTRAINT fk_abuse_case_type_of_abuser_type_of_abuser FOREIGN KEY (type_of_abuser_id) REFERENCES public.type_of_abuser(id) NOT VALID;


--
-- TOC entry 4821 (class 2606 OID 18314)
-- Name: access_role_privilege fk_access_role_privilege_access_role; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.access_role_privilege
    ADD CONSTRAINT fk_access_role_privilege_access_role FOREIGN KEY (access_role_id) REFERENCES public.access_role(id) NOT VALID;


--
-- TOC entry 4822 (class 2606 OID 18319)
-- Name: access_role_privilege fk_access_role_privilege_action; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.access_role_privilege
    ADD CONSTRAINT fk_access_role_privilege_action FOREIGN KEY (action_id) REFERENCES public.action(id) NOT VALID;


--
-- TOC entry 4820 (class 2606 OID 18324)
-- Name: access_role fk_access_role_user_role; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.access_role
    ADD CONSTRAINT fk_access_role_user_role FOREIGN KEY (id) REFERENCES public.user_role(id) NOT VALID;


--
-- TOC entry 4823 (class 2606 OID 18329)
-- Name: act fk_act_id_documentable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.act
    ADD CONSTRAINT fk_act_id_documentable FOREIGN KEY (id) REFERENCES public.documentable(id);


--
-- TOC entry 4824 (class 2606 OID 18334)
-- Name: act fk_act_id_nameable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.act
    ADD CONSTRAINT fk_act_id_nameable FOREIGN KEY (id) REFERENCES public.nameable(id);


--
-- TOC entry 4825 (class 2606 OID 18339)
-- Name: action_menu_item fk_action_menu_item_action; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.action_menu_item
    ADD CONSTRAINT fk_action_menu_item_action FOREIGN KEY (action_id) REFERENCES public.action(id) NOT VALID;


--
-- TOC entry 4826 (class 2606 OID 18344)
-- Name: action_menu_item fk_action_menu_item_id_menu_item; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.action_menu_item
    ADD CONSTRAINT fk_action_menu_item_id_menu_item FOREIGN KEY (id) REFERENCES public.menu_item(id) NOT VALID;


--
-- TOC entry 4827 (class 2606 OID 18349)
-- Name: administrator_role fk_administor_role_user_group; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.administrator_role
    ADD CONSTRAINT fk_administor_role_user_group FOREIGN KEY (user_group_id) REFERENCES public.user_group(id) NOT VALID;


--
-- TOC entry 4828 (class 2606 OID 18354)
-- Name: administrator_role fk_administrator_role_user_role; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.administrator_role
    ADD CONSTRAINT fk_administrator_role_user_role FOREIGN KEY (id) REFERENCES public.user_role(id) NOT VALID;


--
-- TOC entry 4829 (class 2606 OID 18359)
-- Name: adoption_lawyer fk_adoption_lawyer_id_professional_role; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.adoption_lawyer
    ADD CONSTRAINT fk_adoption_lawyer_id_professional_role FOREIGN KEY (id) REFERENCES public.professional_role(id) NOT VALID;


--
-- TOC entry 4830 (class 2606 OID 18369)
-- Name: attachment_therapist fk_attachment_therapist_id_professional_role; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.attachment_therapist
    ADD CONSTRAINT fk_attachment_therapist_id_professional_role FOREIGN KEY (id) REFERENCES public.professional_role(id) NOT VALID;


--
-- TOC entry 4831 (class 2606 OID 18374)
-- Name: basic_action fk_basic_action_id_action; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.basic_action
    ADD CONSTRAINT fk_basic_action_id_action FOREIGN KEY (id) REFERENCES public.action(id) NOT VALID;


--
-- TOC entry 4832 (class 2606 OID 18379)
-- Name: basic_country fk_basic_country_id_top_level_country; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.basic_country
    ADD CONSTRAINT fk_basic_country_id_top_level_country FOREIGN KEY (id) REFERENCES public.top_level_country(id) NOT VALID;


--
-- TOC entry 4833 (class 2606 OID 18384)
-- Name: basic_first_and_second_level_subdivision fk_basic_first_and_second_level_subdivision_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.basic_first_and_second_level_subdivision
    ADD CONSTRAINT fk_basic_first_and_second_level_subdivision_id FOREIGN KEY (id) REFERENCES public.first_and_second_level_subdivision(id) NOT VALID;


--
-- TOC entry 4834 (class 2606 OID 18389)
-- Name: basic_nameable fk_basic_nameable_id_nameable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.basic_nameable
    ADD CONSTRAINT fk_basic_nameable_id_nameable FOREIGN KEY (id) REFERENCES public.nameable(id) ON UPDATE RESTRICT ON DELETE RESTRICT NOT VALID;


--
-- TOC entry 4835 (class 2606 OID 18394)
-- Name: basic_second_level_subdivision fk_basic_second_level_subdivision_id_1; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.basic_second_level_subdivision
    ADD CONSTRAINT fk_basic_second_level_subdivision_id_1 FOREIGN KEY (id) REFERENCES public.second_level_subdivision(id) NOT VALID;


--
-- TOC entry 4836 (class 2606 OID 18399)
-- Name: basic_second_level_subdivision fk_basic_second_level_subdivision_intermediate_level_subdivisio; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.basic_second_level_subdivision
    ADD CONSTRAINT fk_basic_second_level_subdivision_intermediate_level_subdivisio FOREIGN KEY (intermediate_level_subdivision_id) REFERENCES public.intermediate_level_subdivision(id) NOT VALID;


--
-- TOC entry 4837 (class 2606 OID 1209176)
-- Name: bill fk_bill_act; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.bill
    ADD CONSTRAINT fk_bill_act FOREIGN KEY (act_id) REFERENCES public.act(id) NOT VALID;


--
-- TOC entry 4840 (class 2606 OID 18404)
-- Name: bill_action_type fk_bill_action_nameable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.bill_action_type
    ADD CONSTRAINT fk_bill_action_nameable FOREIGN KEY (id) REFERENCES public.nameable(id) NOT VALID;


--
-- TOC entry 4838 (class 2606 OID 18409)
-- Name: bill fk_bill_id_documentable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.bill
    ADD CONSTRAINT fk_bill_id_documentable FOREIGN KEY (id) REFERENCES public.documentable(id);


--
-- TOC entry 4839 (class 2606 OID 18414)
-- Name: bill fk_bill_id_nameable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.bill
    ADD CONSTRAINT fk_bill_id_nameable FOREIGN KEY (id) REFERENCES public.nameable(id);


--
-- TOC entry 4841 (class 2606 OID 18419)
-- Name: binding_country fk_binding_country_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.binding_country
    ADD CONSTRAINT fk_binding_country_id FOREIGN KEY (id) REFERENCES public.top_level_country(id) NOT VALID;


--
-- TOC entry 4842 (class 2606 OID 18424)
-- Name: blog_post fk_blog_post_id_simple_text_node; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.blog_post
    ADD CONSTRAINT fk_blog_post_id_simple_text_node FOREIGN KEY (id) REFERENCES public.simple_text_node(id) NOT VALID;


--
-- TOC entry 4843 (class 2606 OID 18429)
-- Name: bottom_level_subdivision fk_bottom_level_subdivision_subdivision; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.bottom_level_subdivision
    ADD CONSTRAINT fk_bottom_level_subdivision_subdivision FOREIGN KEY (id) REFERENCES public.subdivision(id) NOT VALID;


--
-- TOC entry 4844 (class 2606 OID 18434)
-- Name: bound_country fk_bound_country_binding_country; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.bound_country
    ADD CONSTRAINT fk_bound_country_binding_country FOREIGN KEY (binding_country_id) REFERENCES public.binding_country(id) NOT VALID;


--
-- TOC entry 4845 (class 2606 OID 18439)
-- Name: bound_country fk_bound_country_id_country; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.bound_country
    ADD CONSTRAINT fk_bound_country_id_country FOREIGN KEY (id) REFERENCES public.country(id) NOT VALID;


--
-- TOC entry 4846 (class 2606 OID 18444)
-- Name: bound_country fk_bound_country_id_iso_coded_subdivision; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.bound_country
    ADD CONSTRAINT fk_bound_country_id_iso_coded_subdivision FOREIGN KEY (id) REFERENCES public.iso_coded_subdivision(id) NOT VALID;


--
-- TOC entry 4850 (class 2606 OID 18449)
-- Name: case_case_parties fk_case_case_parties_case; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.case_case_parties
    ADD CONSTRAINT fk_case_case_parties_case FOREIGN KEY (case_id) REFERENCES public."case"(id) NOT VALID;


--
-- TOC entry 4851 (class 2606 OID 18454)
-- Name: case_case_parties fk_case_case_parties_case_parties; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.case_case_parties
    ADD CONSTRAINT fk_case_case_parties_case_parties FOREIGN KEY (case_parties_id) REFERENCES public.case_parties(id) NOT VALID;


--
-- TOC entry 4852 (class 2606 OID 18459)
-- Name: case_case_parties fk_case_case_parties_case_party_type; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.case_case_parties
    ADD CONSTRAINT fk_case_case_parties_case_party_type FOREIGN KEY (case_party_type_id) REFERENCES public.case_party_type(id) NOT VALID;


--
-- TOC entry 4847 (class 2606 OID 18464)
-- Name: case fk_case_id_documentable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."case"
    ADD CONSTRAINT fk_case_id_documentable FOREIGN KEY (id) REFERENCES public.documentable(id) NOT VALID;


--
-- TOC entry 4848 (class 2606 OID 18469)
-- Name: case fk_case_id_locatable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."case"
    ADD CONSTRAINT fk_case_id_locatable FOREIGN KEY (id) REFERENCES public.locatable(id) NOT VALID;


--
-- TOC entry 4849 (class 2606 OID 18474)
-- Name: case fk_case_id_nameable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."case"
    ADD CONSTRAINT fk_case_id_nameable FOREIGN KEY (id) REFERENCES public.nameable(id) NOT VALID;


--
-- TOC entry 4853 (class 2606 OID 18479)
-- Name: case_parties_organization fk_case_parties_organization_case_parties; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.case_parties_organization
    ADD CONSTRAINT fk_case_parties_organization_case_parties FOREIGN KEY (case_parties_id) REFERENCES public.case_parties(id) NOT VALID;


--
-- TOC entry 4854 (class 2606 OID 18484)
-- Name: case_parties_organization fk_case_parties_organization_organization; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.case_parties_organization
    ADD CONSTRAINT fk_case_parties_organization_organization FOREIGN KEY (organization_id) REFERENCES public.organization(id) NOT VALID;


--
-- TOC entry 4855 (class 2606 OID 18489)
-- Name: case_parties_person fk_case_parties_person_case_parties; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.case_parties_person
    ADD CONSTRAINT fk_case_parties_person_case_parties FOREIGN KEY (case_parties_id) REFERENCES public.case_parties(id) NOT VALID;


--
-- TOC entry 4856 (class 2606 OID 18494)
-- Name: case_parties_person fk_case_parties_person_person; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.case_parties_person
    ADD CONSTRAINT fk_case_parties_person_person FOREIGN KEY (person_id) REFERENCES public.person(id) NOT VALID;


--
-- TOC entry 4857 (class 2606 OID 18499)
-- Name: case_party_type fk_case_party_type_id_nameable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.case_party_type
    ADD CONSTRAINT fk_case_party_type_id_nameable FOREIGN KEY (id) REFERENCES public.nameable(id) NOT VALID;


--
-- TOC entry 4859 (class 2606 OID 18504)
-- Name: case_type_case_party_type fk_case_type_case_party_type_case_party_type; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.case_type_case_party_type
    ADD CONSTRAINT fk_case_type_case_party_type_case_party_type FOREIGN KEY (case_party_type_id) REFERENCES public.case_party_type(id) NOT VALID;


--
-- TOC entry 4860 (class 2606 OID 18509)
-- Name: case_type_case_party_type fk_case_type_case_party_type_case_type; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.case_type_case_party_type
    ADD CONSTRAINT fk_case_type_case_party_type_case_type FOREIGN KEY (case_type_id) REFERENCES public.case_type(id) NOT VALID;


--
-- TOC entry 4858 (class 2606 OID 1933903)
-- Name: case_type fk_case_type_id_nameable_type; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.case_type
    ADD CONSTRAINT fk_case_type_id_nameable_type FOREIGN KEY (id) REFERENCES public.nameable_type(id) NOT VALID;


--
-- TOC entry 4861 (class 2606 OID 18519)
-- Name: child_placement_type fk_child_placement_type_id_nameable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.child_placement_type
    ADD CONSTRAINT fk_child_placement_type_id_nameable FOREIGN KEY (id) REFERENCES public.nameable(id) NOT VALID;


--
-- TOC entry 4862 (class 2606 OID 18524)
-- Name: child_trafficking_case fk_child_trafficking_case_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.child_trafficking_case
    ADD CONSTRAINT fk_child_trafficking_case_id FOREIGN KEY (id) REFERENCES public."case"(id) NOT VALID;


--
-- TOC entry 4863 (class 2606 OID 18529)
-- Name: child_trafficking_case fk_childtrafficking_case_country_from; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.child_trafficking_case
    ADD CONSTRAINT fk_childtrafficking_case_country_from FOREIGN KEY (country_id_from) REFERENCES public.country(id) NOT VALID;


--
-- TOC entry 4864 (class 2606 OID 18534)
-- Name: coerced_adoption_case fk_coerced_adoption_case_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.coerced_adoption_case
    ADD CONSTRAINT fk_coerced_adoption_case_id FOREIGN KEY (id) REFERENCES public."case"(id) NOT VALID;


--
-- TOC entry 4865 (class 2606 OID 18539)
-- Name: collective fk_collective_id_publisher; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.collective
    ADD CONSTRAINT fk_collective_id_publisher FOREIGN KEY (id) REFERENCES public.publisher(id) NOT VALID;


--
-- TOC entry 4866 (class 2606 OID 18544)
-- Name: collective_user fk_collective_user_collective; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.collective_user
    ADD CONSTRAINT fk_collective_user_collective FOREIGN KEY (collective_id) REFERENCES public.collective(id) NOT VALID;


--
-- TOC entry 4867 (class 2606 OID 18549)
-- Name: collective_user fk_collective_user_user; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.collective_user
    ADD CONSTRAINT fk_collective_user_user FOREIGN KEY (user_id) REFERENCES public."user"(id) NOT VALID;


--
-- TOC entry 4868 (class 2606 OID 18554)
-- Name: comment fk_comment_comment_parent; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.comment
    ADD CONSTRAINT fk_comment_comment_parent FOREIGN KEY (comment_id_parent) REFERENCES public.comment(id) NOT VALID;


--
-- TOC entry 4869 (class 2606 OID 18559)
-- Name: comment fk_comment_node; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.comment
    ADD CONSTRAINT fk_comment_node FOREIGN KEY (node_id) REFERENCES public.node(id) ON UPDATE RESTRICT ON DELETE RESTRICT NOT VALID;


--
-- TOC entry 4870 (class 2606 OID 18564)
-- Name: comment fk_comment_publisher; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.comment
    ADD CONSTRAINT fk_comment_publisher FOREIGN KEY (publisher_id) REFERENCES public.publisher(id) NOT VALID;


--
-- TOC entry 4871 (class 2606 OID 18569)
-- Name: congressional_term fk_congressional_term_documentable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.congressional_term
    ADD CONSTRAINT fk_congressional_term_documentable FOREIGN KEY (id) REFERENCES public.documentable(id) NOT VALID;


--
-- TOC entry 4872 (class 2606 OID 18574)
-- Name: congressional_term_political_party_affiliation fk_congressional_term_political_party_affiliation_id_documentab; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.congressional_term_political_party_affiliation
    ADD CONSTRAINT fk_congressional_term_political_party_affiliation_id_documentab FOREIGN KEY (id) REFERENCES public.documentable(id) NOT VALID;


--
-- TOC entry 4873 (class 2606 OID 18579)
-- Name: congressional_term_political_party_affiliation fk_congressional_term_political_party_affiliation_united_states; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.congressional_term_political_party_affiliation
    ADD CONSTRAINT fk_congressional_term_political_party_affiliation_united_states FOREIGN KEY (united_states_political_party_affiliation_id) REFERENCES public.united_states_political_party_affiliation(id) NOT VALID;


--
-- TOC entry 4874 (class 2606 OID 18584)
-- Name: content_sharing_group fk_content_sharing_group_id_owner; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.content_sharing_group
    ADD CONSTRAINT fk_content_sharing_group_id_owner FOREIGN KEY (id) REFERENCES public.owner(id) NOT VALID;


--
-- TOC entry 4878 (class 2606 OID 18589)
-- Name: country_and_first_and_bottom_level_subdivision fk_country_and_first_and_bottom_level_subdivision_id_1; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.country_and_first_and_bottom_level_subdivision
    ADD CONSTRAINT fk_country_and_first_and_bottom_level_subdivision_id_1 FOREIGN KEY (id) REFERENCES public.country_and_first_level_subdivision(id) NOT VALID;


--
-- TOC entry 4879 (class 2606 OID 18594)
-- Name: country_and_first_and_bottom_level_subdivision fk_country_and_first_and_bottom_level_subdivision_id_2; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.country_and_first_and_bottom_level_subdivision
    ADD CONSTRAINT fk_country_and_first_and_bottom_level_subdivision_id_2 FOREIGN KEY (id) REFERENCES public.country_and_first_and_bottom_level_subdivision(id) NOT VALID;


--
-- TOC entry 4880 (class 2606 OID 18599)
-- Name: country_and_first_and_second_level_subdivision fk_country_and_first_and_second_level_subdivision_id_country_an; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.country_and_first_and_second_level_subdivision
    ADD CONSTRAINT fk_country_and_first_and_second_level_subdivision_id_country_an FOREIGN KEY (id) REFERENCES public.country_and_first_level_subdivision(id) NOT VALID;


--
-- TOC entry 4881 (class 2606 OID 18604)
-- Name: country_and_first_and_second_level_subdivision fk_country_and_first_and_second_level_subdivision_id_first_and_; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.country_and_first_and_second_level_subdivision
    ADD CONSTRAINT fk_country_and_first_and_second_level_subdivision_id_first_and_ FOREIGN KEY (id) REFERENCES public.first_and_second_level_subdivision(id) NOT VALID;


--
-- TOC entry 4882 (class 2606 OID 18609)
-- Name: country_and_first_level_subdivision fk_country_and_first_level_subdivision_id_iso_coded_first_level; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.country_and_first_level_subdivision
    ADD CONSTRAINT fk_country_and_first_level_subdivision_id_iso_coded_first_level FOREIGN KEY (id) REFERENCES public.iso_coded_first_level_subdivision(id) NOT VALID;


--
-- TOC entry 4883 (class 2606 OID 18614)
-- Name: country_and_first_level_subdivision fk_country_and_first_level_subdivision_id_top_level_country; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.country_and_first_level_subdivision
    ADD CONSTRAINT fk_country_and_first_level_subdivision_id_top_level_country FOREIGN KEY (id) REFERENCES public.top_level_country(id) NOT VALID;


--
-- TOC entry 4884 (class 2606 OID 18619)
-- Name: country_and_intermediate_level_subdivision fk_country_and_intermediate_level_subdivision_1; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.country_and_intermediate_level_subdivision
    ADD CONSTRAINT fk_country_and_intermediate_level_subdivision_1 FOREIGN KEY (id) REFERENCES public.country_and_first_level_subdivision(id) NOT VALID;


--
-- TOC entry 4885 (class 2606 OID 18624)
-- Name: country_and_intermediate_level_subdivision fk_country_and_intermediate_level_subdivision_2; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.country_and_intermediate_level_subdivision
    ADD CONSTRAINT fk_country_and_intermediate_level_subdivision_2 FOREIGN KEY (id) REFERENCES public.intermediate_level_subdivision(id) NOT VALID;


--
-- TOC entry 4875 (class 2606 OID 18629)
-- Name: country fk_country_hague_status; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.country
    ADD CONSTRAINT fk_country_hague_status FOREIGN KEY (hague_status_id) REFERENCES public.hague_status(id) NOT VALID;


--
-- TOC entry 4876 (class 2606 OID 18634)
-- Name: country fk_country_id_political_entity; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.country
    ADD CONSTRAINT fk_country_id_political_entity FOREIGN KEY (id) REFERENCES public.political_entity(id) NOT VALID;


--
-- TOC entry 4886 (class 2606 OID 18639)
-- Name: country_report fk_country_report_country; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.country_report
    ADD CONSTRAINT fk_country_report_country FOREIGN KEY (country_id) REFERENCES public.country(id) NOT VALID;


--
-- TOC entry 4887 (class 2606 OID 18644)
-- Name: country_subdivision_type fk_country_subdivision_type_country; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.country_subdivision_type
    ADD CONSTRAINT fk_country_subdivision_type_country FOREIGN KEY (country_id) REFERENCES public.country(id) NOT VALID;


--
-- TOC entry 4888 (class 2606 OID 18649)
-- Name: country_subdivision_type fk_country_subdivision_type_subdivision_type; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.country_subdivision_type
    ADD CONSTRAINT fk_country_subdivision_type_subdivision_type FOREIGN KEY (subdivision_type_id) REFERENCES public.subdivision_type(id) NOT VALID;


--
-- TOC entry 4877 (class 2606 OID 1506296)
-- Name: country fk_country_vocabulary_subdivisions; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.country
    ADD CONSTRAINT fk_country_vocabulary_subdivisions FOREIGN KEY (vocabulary_id_subdivisions) REFERENCES public.vocabulary(id) NOT VALID;


--
-- TOC entry 4889 (class 2606 OID 18654)
-- Name: create_node_action fk_create_node_action_id_action; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.create_node_action
    ADD CONSTRAINT fk_create_node_action_id_action FOREIGN KEY (id) REFERENCES public.action(id) NOT VALID;


--
-- TOC entry 4890 (class 2606 OID 18659)
-- Name: create_node_action fk_create_node_action_node_type; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.create_node_action
    ADD CONSTRAINT fk_create_node_action_node_type FOREIGN KEY (node_type_id) REFERENCES public.node_type(id) NOT VALID;


--
-- TOC entry 4891 (class 2606 OID 18664)
-- Name: delete_node_action fk_delete_node_action_id_access_privilege; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.delete_node_action
    ADD CONSTRAINT fk_delete_node_action_id_access_privilege FOREIGN KEY (id) REFERENCES public.action(id);


--
-- TOC entry 4892 (class 2606 OID 18669)
-- Name: delete_node_action fk_delete_node_action_node_type; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.delete_node_action
    ADD CONSTRAINT fk_delete_node_action_node_type FOREIGN KEY (node_type_id) REFERENCES public.node_type(id);


--
-- TOC entry 4893 (class 2606 OID 18674)
-- Name: denomination fk_denomination_id_nameable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.denomination
    ADD CONSTRAINT fk_denomination_id_nameable FOREIGN KEY (id) REFERENCES public.nameable(id) NOT VALID;


--
-- TOC entry 4894 (class 2606 OID 18679)
-- Name: deportation_case fk_deportation_case_country_id_to; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.deportation_case
    ADD CONSTRAINT fk_deportation_case_country_id_to FOREIGN KEY (country_id_to) REFERENCES public.country(id) NOT VALID;


--
-- TOC entry 4895 (class 2606 OID 18684)
-- Name: deportation_case fk_deportation_case_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.deportation_case
    ADD CONSTRAINT fk_deportation_case_id FOREIGN KEY (id) REFERENCES public."case"(id) NOT VALID;


--
-- TOC entry 4896 (class 2606 OID 18689)
-- Name: deportation_case fk_deportation_case_subdivision_id_from; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.deportation_case
    ADD CONSTRAINT fk_deportation_case_subdivision_id_from FOREIGN KEY (subdivision_id_from) REFERENCES public.subdivision(id) NOT VALID;


--
-- TOC entry 4897 (class 2606 OID 18694)
-- Name: discussion fk_discussion_id_simple_text_node; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.discussion
    ADD CONSTRAINT fk_discussion_id_simple_text_node FOREIGN KEY (id) REFERENCES public.simple_text_node(id) NOT VALID;


--
-- TOC entry 4898 (class 2606 OID 18699)
-- Name: disrupted_placement_case fk_disrupted_placement_case_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.disrupted_placement_case
    ADD CONSTRAINT fk_disrupted_placement_case_id FOREIGN KEY (id) REFERENCES public."case"(id) NOT VALID;


--
-- TOC entry 4899 (class 2606 OID 18704)
-- Name: document fk_document_document_type_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.document
    ADD CONSTRAINT fk_document_document_type_id FOREIGN KEY (document_type_id) REFERENCES public.document_type(id) NOT VALID;


--
-- TOC entry 4900 (class 2606 OID 1317097)
-- Name: document fk_document_id_simple_text_node; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.document
    ADD CONSTRAINT fk_document_id_simple_text_node FOREIGN KEY (id) REFERENCES public.simple_text_node(id) NOT VALID;


--
-- TOC entry 4901 (class 2606 OID 18714)
-- Name: document_type fk_document_type_id_nameable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.document_type
    ADD CONSTRAINT fk_document_type_id_nameable FOREIGN KEY (id) REFERENCES public.nameable(id) NOT VALID;


--
-- TOC entry 4902 (class 2606 OID 18729)
-- Name: documentable fk_documentable_id_searchable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.documentable
    ADD CONSTRAINT fk_documentable_id_searchable FOREIGN KEY (id) REFERENCES public.searchable(id) NOT VALID;


--
-- TOC entry 4903 (class 2606 OID 18734)
-- Name: edit_node_action fk_edit_node_action_id_action; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.edit_node_action
    ADD CONSTRAINT fk_edit_node_action_id_action FOREIGN KEY (id) REFERENCES public.action(id);


--
-- TOC entry 4904 (class 2606 OID 18739)
-- Name: edit_node_action fk_edit_node_action_node_type; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.edit_node_action
    ADD CONSTRAINT fk_edit_node_action_node_type FOREIGN KEY (node_type_id) REFERENCES public.node_type(id);


--
-- TOC entry 5079 (class 2606 OID 192736)
-- Name: edit_own_node_action fk_edit_own_node_action_id_action; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.edit_own_node_action
    ADD CONSTRAINT fk_edit_own_node_action_id_action FOREIGN KEY (id) REFERENCES public.action(id);


--
-- TOC entry 5080 (class 2606 OID 192741)
-- Name: edit_own_node_action fk_edit_own_node_action_node_type; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.edit_own_node_action
    ADD CONSTRAINT fk_edit_own_node_action_node_type FOREIGN KEY (node_type_id) REFERENCES public.node_type(id);


--
-- TOC entry 4905 (class 2606 OID 18744)
-- Name: facilitator fk_facilitator_id_organizational_role; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.facilitator
    ADD CONSTRAINT fk_facilitator_id_organizational_role FOREIGN KEY (id) REFERENCES public.organizational_role(id) NOT VALID;


--
-- TOC entry 4906 (class 2606 OID 18749)
-- Name: family_size fk_family_size_id_nameable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.family_size
    ADD CONSTRAINT fk_family_size_id_nameable FOREIGN KEY (id) REFERENCES public.nameable(id) NOT VALID;


--
-- TOC entry 4907 (class 2606 OID 18754)
-- Name: fathers_rights_violation_case fk_fathers_rights_violation_case_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.fathers_rights_violation_case
    ADD CONSTRAINT fk_fathers_rights_violation_case_id FOREIGN KEY (id) REFERENCES public."case"(id) NOT VALID;


--
-- TOC entry 4989 (class 2606 OID 18759)
-- Name: person fk_file_id_file_portrait; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.person
    ADD CONSTRAINT fk_file_id_file_portrait FOREIGN KEY (file_id_portrait) REFERENCES public.file(id) NOT VALID;


--
-- TOC entry 4908 (class 2606 OID 18764)
-- Name: first_and_bottom_level_subdivision fk_first_and_bottom_level_subdivision_id_1; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.first_and_bottom_level_subdivision
    ADD CONSTRAINT fk_first_and_bottom_level_subdivision_id_1 FOREIGN KEY (id) REFERENCES public.iso_coded_first_level_subdivision(id) NOT VALID;


--
-- TOC entry 4909 (class 2606 OID 18769)
-- Name: first_and_bottom_level_subdivision fk_first_and_bottom_level_subdivision_id_2; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.first_and_bottom_level_subdivision
    ADD CONSTRAINT fk_first_and_bottom_level_subdivision_id_2 FOREIGN KEY (id) REFERENCES public.bottom_level_subdivision(id) NOT VALID;


--
-- TOC entry 4910 (class 2606 OID 18774)
-- Name: first_and_second_level_subdivision fk_first_and_second_level_subdivision_id_1; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.first_and_second_level_subdivision
    ADD CONSTRAINT fk_first_and_second_level_subdivision_id_1 FOREIGN KEY (id) REFERENCES public.iso_coded_first_level_subdivision(id) NOT VALID;


--
-- TOC entry 4911 (class 2606 OID 18779)
-- Name: first_and_second_level_subdivision fk_first_and_second_level_subdivision_id_2; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.first_and_second_level_subdivision
    ADD CONSTRAINT fk_first_and_second_level_subdivision_id_2 FOREIGN KEY (id) REFERENCES public.second_level_subdivision(id) NOT VALID;


--
-- TOC entry 4912 (class 2606 OID 18784)
-- Name: first_level_global_region fk_first_level_global_region_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.first_level_global_region
    ADD CONSTRAINT fk_first_level_global_region_id FOREIGN KEY (id) REFERENCES public.global_region(id) NOT VALID;


--
-- TOC entry 4913 (class 2606 OID 18789)
-- Name: first_level_subdivision fk_first_level_subdivision_id_subdivision; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.first_level_subdivision
    ADD CONSTRAINT fk_first_level_subdivision_id_subdivision FOREIGN KEY (id) REFERENCES public.subdivision(id) NOT VALID;


--
-- TOC entry 4914 (class 2606 OID 18794)
-- Name: formal_intermediate_level_subdivision fk_formal_intermediate_level_subdivision_id_1; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.formal_intermediate_level_subdivision
    ADD CONSTRAINT fk_formal_intermediate_level_subdivision_id_1 FOREIGN KEY (id) REFERENCES public.iso_coded_first_level_subdivision(id) NOT VALID;


--
-- TOC entry 4915 (class 2606 OID 18799)
-- Name: formal_intermediate_level_subdivision fk_formal_intermediate_level_subdivision_id_2; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.formal_intermediate_level_subdivision
    ADD CONSTRAINT fk_formal_intermediate_level_subdivision_id_2 FOREIGN KEY (id) REFERENCES public.intermediate_level_subdivision(id) NOT VALID;


--
-- TOC entry 4916 (class 2606 OID 18804)
-- Name: geographical_entity fk_geographical_entity_id_documentable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.geographical_entity
    ADD CONSTRAINT fk_geographical_entity_id_documentable FOREIGN KEY (id) REFERENCES public.documentable(id) NOT VALID;


--
-- TOC entry 4917 (class 2606 OID 18809)
-- Name: geographical_entity fk_geographical_entity_id_nameable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.geographical_entity
    ADD CONSTRAINT fk_geographical_entity_id_nameable FOREIGN KEY (id) REFERENCES public.nameable(id) NOT VALID;


--
-- TOC entry 4918 (class 2606 OID 18814)
-- Name: global_region fk_global_region_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.global_region
    ADD CONSTRAINT fk_global_region_id FOREIGN KEY (id) REFERENCES public.geographical_entity(id) NOT VALID;


--
-- TOC entry 4919 (class 2606 OID 18819)
-- Name: hague_status fk_hague_status_id_nameable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.hague_status
    ADD CONSTRAINT fk_hague_status_id_nameable FOREIGN KEY (id) REFERENCES public.nameable(id);


--
-- TOC entry 4920 (class 2606 OID 18824)
-- Name: home_study_agency fk_home_study_agency_id_organization_role; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.home_study_agency
    ADD CONSTRAINT fk_home_study_agency_id_organization_role FOREIGN KEY (id) REFERENCES public.organizational_role(id) NOT VALID;


--
-- TOC entry 4921 (class 2606 OID 18829)
-- Name: house_bill fk_house_bill_id_bill; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.house_bill
    ADD CONSTRAINT fk_house_bill_id_bill FOREIGN KEY (id) REFERENCES public.bill(id) NOT VALID;


--
-- TOC entry 4922 (class 2606 OID 18834)
-- Name: house_term fk_house_term_id_congressional_term; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.house_term
    ADD CONSTRAINT fk_house_term_id_congressional_term FOREIGN KEY (id) REFERENCES public.congressional_term(id);


--
-- TOC entry 4923 (class 2606 OID 18839)
-- Name: house_term fk_house_term_representative; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.house_term
    ADD CONSTRAINT fk_house_term_representative FOREIGN KEY (representative_id) REFERENCES public.representative(id);


--
-- TOC entry 4924 (class 2606 OID 18844)
-- Name: house_term fk_house_term_subdivision; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.house_term
    ADD CONSTRAINT fk_house_term_subdivision FOREIGN KEY (subdivision_id) REFERENCES public.subdivision(id);


--
-- TOC entry 4925 (class 2606 OID 18849)
-- Name: informal_intermediate_level_subdivision fk_informal_intermediate_level_subdivision_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.informal_intermediate_level_subdivision
    ADD CONSTRAINT fk_informal_intermediate_level_subdivision_id FOREIGN KEY (id) REFERENCES public.intermediate_level_subdivision(id) NOT VALID;


--
-- TOC entry 4926 (class 2606 OID 18854)
-- Name: institution fk_institution_id_organizational_role; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.institution
    ADD CONSTRAINT fk_institution_id_organizational_role FOREIGN KEY (id) REFERENCES public.organizational_role(id) NOT VALID;


--
-- TOC entry 4927 (class 2606 OID 18859)
-- Name: inter_country_relation fk_inter_country_relation_country_from; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.inter_country_relation
    ADD CONSTRAINT fk_inter_country_relation_country_from FOREIGN KEY (country_id_from) REFERENCES public.country(id) NOT VALID;


--
-- TOC entry 4928 (class 2606 OID 18864)
-- Name: inter_country_relation fk_inter_country_relation_country_to; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.inter_country_relation
    ADD CONSTRAINT fk_inter_country_relation_country_to FOREIGN KEY (country_id_to) REFERENCES public.country(id) NOT VALID;


--
-- TOC entry 4929 (class 2606 OID 18869)
-- Name: inter_country_relation fk_inter_country_relation_document_id_proof; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.inter_country_relation
    ADD CONSTRAINT fk_inter_country_relation_document_id_proof FOREIGN KEY (document_id_proof) REFERENCES public.document(id) NOT VALID;


--
-- TOC entry 4930 (class 2606 OID 18874)
-- Name: inter_country_relation fk_inter_country_relation_id_node; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.inter_country_relation
    ADD CONSTRAINT fk_inter_country_relation_id_node FOREIGN KEY (id) REFERENCES public.node(id) NOT VALID;


--
-- TOC entry 4931 (class 2606 OID 18879)
-- Name: inter_country_relation fk_inter_country_relation_inter_country_relation_type; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.inter_country_relation
    ADD CONSTRAINT fk_inter_country_relation_inter_country_relation_type FOREIGN KEY (inter_country_relation_type_id) REFERENCES public.inter_country_relation_type(id) NOT VALID;


--
-- TOC entry 4932 (class 2606 OID 18884)
-- Name: inter_country_relation_type fk_inter_country_relation_type_id_nameable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.inter_country_relation_type
    ADD CONSTRAINT fk_inter_country_relation_type_id_nameable FOREIGN KEY (id) REFERENCES public.nameable(id) NOT VALID;


--
-- TOC entry 4933 (class 2606 OID 18889)
-- Name: inter_organizational_relation fk_inter_organizational_relation_document_proof; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.inter_organizational_relation
    ADD CONSTRAINT fk_inter_organizational_relation_document_proof FOREIGN KEY (document_id_proof) REFERENCES public.document(id) NOT VALID;


--
-- TOC entry 4934 (class 2606 OID 18894)
-- Name: inter_organizational_relation fk_inter_organizational_relation_geographical_entity; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.inter_organizational_relation
    ADD CONSTRAINT fk_inter_organizational_relation_geographical_entity FOREIGN KEY (geographical_entity_id) REFERENCES public.geographical_entity(id) NOT VALID;


--
-- TOC entry 4935 (class 2606 OID 18899)
-- Name: inter_organizational_relation fk_inter_organizational_relation_organizational_from; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.inter_organizational_relation
    ADD CONSTRAINT fk_inter_organizational_relation_organizational_from FOREIGN KEY (organization_id_from) REFERENCES public.organization(id) NOT VALID;


--
-- TOC entry 4936 (class 2606 OID 18904)
-- Name: inter_organizational_relation fk_inter_organizational_relation_organizational_to; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.inter_organizational_relation
    ADD CONSTRAINT fk_inter_organizational_relation_organizational_to FOREIGN KEY (organization_id_to) REFERENCES public.organization(id) NOT VALID;


--
-- TOC entry 4939 (class 2606 OID 18909)
-- Name: inter_organizational_relation_type fk_inter_organizational_relation_type_id_nameable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.inter_organizational_relation_type
    ADD CONSTRAINT fk_inter_organizational_relation_type_id_nameable FOREIGN KEY (id) REFERENCES public.nameable(id) NOT VALID;


--
-- TOC entry 4937 (class 2606 OID 18914)
-- Name: inter_organizational_relation fk_inter_organizationale_relation_inter_organizational_relation; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.inter_organizational_relation
    ADD CONSTRAINT fk_inter_organizationale_relation_inter_organizational_relation FOREIGN KEY (inter_organizational_relation_type_id) REFERENCES public.inter_organizational_relation_type(id) NOT VALID;


--
-- TOC entry 4940 (class 2606 OID 18919)
-- Name: inter_personal_relation fk_inter_personal_relation_document_proof; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.inter_personal_relation
    ADD CONSTRAINT fk_inter_personal_relation_document_proof FOREIGN KEY (document_id_proof) REFERENCES public.document(id) NOT VALID;


--
-- TOC entry 4941 (class 2606 OID 18924)
-- Name: inter_personal_relation fk_inter_personal_relation_id_node; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.inter_personal_relation
    ADD CONSTRAINT fk_inter_personal_relation_id_node FOREIGN KEY (id) REFERENCES public.node(id) NOT VALID;


--
-- TOC entry 4942 (class 2606 OID 18929)
-- Name: inter_personal_relation fk_inter_personal_relation_inter_personal_relation_type; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.inter_personal_relation
    ADD CONSTRAINT fk_inter_personal_relation_inter_personal_relation_type FOREIGN KEY (inter_personal_relation_type_id) REFERENCES public.inter_personal_relation_type(id) NOT VALID;


--
-- TOC entry 4943 (class 2606 OID 18934)
-- Name: inter_personal_relation fk_inter_personal_relation_person_from; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.inter_personal_relation
    ADD CONSTRAINT fk_inter_personal_relation_person_from FOREIGN KEY (person_id_from) REFERENCES public.person(id) NOT VALID;


--
-- TOC entry 4944 (class 2606 OID 18939)
-- Name: inter_personal_relation fk_inter_personal_relation_person_to; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.inter_personal_relation
    ADD CONSTRAINT fk_inter_personal_relation_person_to FOREIGN KEY (person_id_to) REFERENCES public.person(id) NOT VALID;


--
-- TOC entry 4945 (class 2606 OID 18944)
-- Name: inter_personal_relation_type fk_inter_personal_relation_type_id_nameable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.inter_personal_relation_type
    ADD CONSTRAINT fk_inter_personal_relation_type_id_nameable FOREIGN KEY (id) REFERENCES public.nameable(id) NOT VALID;


--
-- TOC entry 4946 (class 2606 OID 18949)
-- Name: intermediate_level_subdivision fk_intermediate_level_subdivision_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.intermediate_level_subdivision
    ADD CONSTRAINT fk_intermediate_level_subdivision_id FOREIGN KEY (id) REFERENCES public.first_level_subdivision(id) NOT VALID;


--
-- TOC entry 4947 (class 2606 OID 18954)
-- Name: iso_coded_first_level_subdivision fk_iso_coded_first_level_subdivision_1; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.iso_coded_first_level_subdivision
    ADD CONSTRAINT fk_iso_coded_first_level_subdivision_1 FOREIGN KEY (id) REFERENCES public.first_level_subdivision(id) NOT VALID;


--
-- TOC entry 4948 (class 2606 OID 18959)
-- Name: iso_coded_first_level_subdivision fk_iso_coded_first_level_subdivision_2; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.iso_coded_first_level_subdivision
    ADD CONSTRAINT fk_iso_coded_first_level_subdivision_2 FOREIGN KEY (id) REFERENCES public.iso_coded_subdivision(id) NOT VALID;


--
-- TOC entry 4949 (class 2606 OID 18964)
-- Name: iso_coded_subdivision fk_iso_coded_subdivision_id_political_entity; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.iso_coded_subdivision
    ADD CONSTRAINT fk_iso_coded_subdivision_id_political_entity FOREIGN KEY (id) REFERENCES public.political_entity(id) NOT VALID;


--
-- TOC entry 4950 (class 2606 OID 18969)
-- Name: iso_coded_subdivision fk_iso_coded_subdivision_id_subdivision; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.iso_coded_subdivision
    ADD CONSTRAINT fk_iso_coded_subdivision_id_subdivision FOREIGN KEY (id) REFERENCES public.subdivision(id) NOT VALID;


--
-- TOC entry 4951 (class 2606 OID 18974)
-- Name: law_firm fk_law_firm_id_organizational_role; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.law_firm
    ADD CONSTRAINT fk_law_firm_id_organizational_role FOREIGN KEY (id) REFERENCES public.organizational_role(id) NOT VALID;


--
-- TOC entry 4952 (class 2606 OID 18979)
-- Name: locatable fk_locatable_id_searchable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.locatable
    ADD CONSTRAINT fk_locatable_id_searchable FOREIGN KEY (id) REFERENCES public.searchable(id) NOT VALID;


--
-- TOC entry 4953 (class 2606 OID 18984)
-- Name: location fk_location_country; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.location
    ADD CONSTRAINT fk_location_country FOREIGN KEY (country_id) REFERENCES public.country(id) NOT VALID;


--
-- TOC entry 4955 (class 2606 OID 18989)
-- Name: location_locatable fk_location_locatable_locatable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.location_locatable
    ADD CONSTRAINT fk_location_locatable_locatable FOREIGN KEY (locatable_id) REFERENCES public.locatable(id) NOT VALID;


--
-- TOC entry 4956 (class 2606 OID 18994)
-- Name: location_locatable fk_location_locatable_location; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.location_locatable
    ADD CONSTRAINT fk_location_locatable_location FOREIGN KEY (location_id) REFERENCES public.location(id) NOT VALID;


--
-- TOC entry 4954 (class 2606 OID 18999)
-- Name: location fk_location_subdivision; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.location
    ADD CONSTRAINT fk_location_subdivision FOREIGN KEY (subdivision_id) REFERENCES public.subdivision(id) NOT VALID;


--
-- TOC entry 4957 (class 2606 OID 19004)
-- Name: member_of_congress fk_member_of_congress_professional_role; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.member_of_congress
    ADD CONSTRAINT fk_member_of_congress_professional_role FOREIGN KEY (id) REFERENCES public.professional_role(id) NOT VALID;


--
-- TOC entry 4958 (class 2606 OID 19009)
-- Name: multi_question_poll fk_multi_question_poll_id_poll; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.multi_question_poll
    ADD CONSTRAINT fk_multi_question_poll_id_poll FOREIGN KEY (id) REFERENCES public.poll(id) NOT VALID;


--
-- TOC entry 4959 (class 2606 OID 19014)
-- Name: multi_question_poll fk_multi_question_poll_id_simpe_text_node; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.multi_question_poll
    ADD CONSTRAINT fk_multi_question_poll_id_simpe_text_node FOREIGN KEY (id) REFERENCES public.simple_text_node(id) NOT VALID;


--
-- TOC entry 4960 (class 2606 OID 19019)
-- Name: multi_question_poll_poll_question fk_multi_question_poll_question_multi_question_poll; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.multi_question_poll_poll_question
    ADD CONSTRAINT fk_multi_question_poll_question_multi_question_poll FOREIGN KEY (multi_question_poll_id) REFERENCES public.multi_question_poll(id) NOT VALID;


--
-- TOC entry 4961 (class 2606 OID 19024)
-- Name: multi_question_poll_poll_question fk_multi_question_poll_question_poll_question; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.multi_question_poll_poll_question
    ADD CONSTRAINT fk_multi_question_poll_question_poll_question FOREIGN KEY (poll_question_id) REFERENCES public.poll_question(id) NOT VALID;


--
-- TOC entry 4962 (class 2606 OID 19029)
-- Name: nameable fk_nameable_file_tile_image; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.nameable
    ADD CONSTRAINT fk_nameable_file_tile_image FOREIGN KEY (file_id_tile_image) REFERENCES public.file(id) NOT VALID;


--
-- TOC entry 4963 (class 2606 OID 19034)
-- Name: nameable fk_nameable_id_searchable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.nameable
    ADD CONSTRAINT fk_nameable_id_searchable FOREIGN KEY (id) REFERENCES public.searchable(id) NOT VALID;


--
-- TOC entry 5087 (class 2606 OID 1937570)
-- Name: nameable_type fk_nameable_type_id_node_type; Type: FK CONSTRAINT; Schema: public; Owner: niels
--

ALTER TABLE ONLY public.nameable_type
    ADD CONSTRAINT fk_nameable_type_id_node_type FOREIGN KEY (id) REFERENCES public.node_type(id) NOT VALID;


--
-- TOC entry 4967 (class 2606 OID 19039)
-- Name: node_file fk_node_file_file; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.node_file
    ADD CONSTRAINT fk_node_file_file FOREIGN KEY (file_id) REFERENCES public.file(id) NOT VALID;


--
-- TOC entry 4968 (class 2606 OID 19044)
-- Name: node_file fk_node_file_node; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.node_file
    ADD CONSTRAINT fk_node_file_node FOREIGN KEY (node_id) REFERENCES public.node(id) NOT VALID;


--
-- TOC entry 4964 (class 2606 OID 19049)
-- Name: node fk_node_node_type; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.node
    ADD CONSTRAINT fk_node_node_type FOREIGN KEY (node_type_id) REFERENCES public.node_type(id) NOT VALID;


--
-- TOC entry 4965 (class 2606 OID 19054)
-- Name: node fk_node_owner; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.node
    ADD CONSTRAINT fk_node_owner FOREIGN KEY (owner_id) REFERENCES public.owner(id) NOT VALID;


--
-- TOC entry 4966 (class 2606 OID 19059)
-- Name: node fk_node_publisher; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.node
    ADD CONSTRAINT fk_node_publisher FOREIGN KEY (publisher_id) REFERENCES public.publisher(id) ON UPDATE RESTRICT ON DELETE RESTRICT NOT VALID;


--
-- TOC entry 4969 (class 2606 OID 19064)
-- Name: node_term fk_node_term_node; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.node_term
    ADD CONSTRAINT fk_node_term_node FOREIGN KEY (node_id) REFERENCES public.node(id) NOT VALID;


--
-- TOC entry 4970 (class 2606 OID 19069)
-- Name: node_term fk_node_term_term; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.node_term
    ADD CONSTRAINT fk_node_term_term FOREIGN KEY (term_id) REFERENCES public.term(id) NOT VALID;


--
-- TOC entry 4972 (class 2606 OID 19074)
-- Name: organization_act_relation_type fk_organization_act_relation_type; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.organization_act_relation_type
    ADD CONSTRAINT fk_organization_act_relation_type FOREIGN KEY (id) REFERENCES public.nameable(id) NOT VALID;


--
-- TOC entry 4971 (class 2606 OID 19079)
-- Name: organization fk_organization_id_party; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.organization
    ADD CONSTRAINT fk_organization_id_party FOREIGN KEY (id) REFERENCES public.party(id);


--
-- TOC entry 4973 (class 2606 OID 19084)
-- Name: organization_organization_type fk_organization_organization_type_organization; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.organization_organization_type
    ADD CONSTRAINT fk_organization_organization_type_organization FOREIGN KEY (organization_id) REFERENCES public.organization(id) NOT VALID;


--
-- TOC entry 4974 (class 2606 OID 19089)
-- Name: organization_organization_type fk_organization_organization_type_organization_type; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.organization_organization_type
    ADD CONSTRAINT fk_organization_organization_type_organization_type FOREIGN KEY (organization_type_id) REFERENCES public.organization_type(id) NOT VALID;


--
-- TOC entry 4975 (class 2606 OID 19094)
-- Name: organization_type fk_organization_type_id_nameable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.organization_type
    ADD CONSTRAINT fk_organization_type_id_nameable FOREIGN KEY (id) REFERENCES public.nameable(id) NOT VALID;


--
-- TOC entry 4976 (class 2606 OID 19099)
-- Name: organizational_role fk_organizational_role_organization; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.organizational_role
    ADD CONSTRAINT fk_organizational_role_organization FOREIGN KEY (organization_id) REFERENCES public.organization(id) NOT VALID;


--
-- TOC entry 4977 (class 2606 OID 19104)
-- Name: organizational_role fk_organizational_role_organization_type; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.organizational_role
    ADD CONSTRAINT fk_organizational_role_organization_type FOREIGN KEY (organization_type_id) REFERENCES public.organization_type(id) NOT VALID;


--
-- TOC entry 4978 (class 2606 OID 19109)
-- Name: owner fk_owner_id_user_group; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.owner
    ADD CONSTRAINT fk_owner_id_user_group FOREIGN KEY (id) REFERENCES public.user_group(id) NOT VALID;


--
-- TOC entry 4979 (class 2606 OID 19114)
-- Name: page fk_page_id_simple_text_node; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.page
    ADD CONSTRAINT fk_page_id_simple_text_node FOREIGN KEY (id) REFERENCES public.simple_text_node(id) NOT VALID;


--
-- TOC entry 5082 (class 2606 OID 1263448)
-- Name: party_act_relation fk_party_act_relation_act; Type: FK CONSTRAINT; Schema: public; Owner: niels
--

ALTER TABLE ONLY public.party_act_relation
    ADD CONSTRAINT fk_party_act_relation_act FOREIGN KEY (act_id) REFERENCES public.act(id) NOT VALID;


--
-- TOC entry 5083 (class 2606 OID 1263436)
-- Name: party_act_relation fk_party_act_relation_id_node; Type: FK CONSTRAINT; Schema: public; Owner: niels
--

ALTER TABLE ONLY public.party_act_relation
    ADD CONSTRAINT fk_party_act_relation_id_node FOREIGN KEY (id) REFERENCES public.node(id) NOT VALID;


--
-- TOC entry 5084 (class 2606 OID 1263442)
-- Name: party_act_relation fk_party_act_relation_party; Type: FK CONSTRAINT; Schema: public; Owner: niels
--

ALTER TABLE ONLY public.party_act_relation
    ADD CONSTRAINT fk_party_act_relation_party FOREIGN KEY (party_id) REFERENCES public.party(id) NOT VALID;


--
-- TOC entry 5085 (class 2606 OID 1263460)
-- Name: party_act_relation fk_party_act_relation_party_document; Type: FK CONSTRAINT; Schema: public; Owner: niels
--

ALTER TABLE ONLY public.party_act_relation
    ADD CONSTRAINT fk_party_act_relation_party_document FOREIGN KEY (document_id_proof) REFERENCES public.document(id) NOT VALID;


--
-- TOC entry 5086 (class 2606 OID 1263454)
-- Name: party_act_relation fk_party_act_relation_party_party_act_relation_party_type; Type: FK CONSTRAINT; Schema: public; Owner: niels
--

ALTER TABLE ONLY public.party_act_relation
    ADD CONSTRAINT fk_party_act_relation_party_party_act_relation_party_type FOREIGN KEY (party_act_relation_type_id) REFERENCES public.party_political_entity_relation_type(id) NOT VALID;


--
-- TOC entry 4980 (class 2606 OID 19119)
-- Name: party fk_party_id_documentable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.party
    ADD CONSTRAINT fk_party_id_documentable FOREIGN KEY (id) REFERENCES public.documentable(id) NOT VALID;


--
-- TOC entry 4981 (class 2606 OID 19124)
-- Name: party fk_party_id_locatable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.party
    ADD CONSTRAINT fk_party_id_locatable FOREIGN KEY (id) REFERENCES public.locatable(id) NOT VALID;


--
-- TOC entry 4982 (class 2606 OID 19129)
-- Name: party fk_party_id_nameable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.party
    ADD CONSTRAINT fk_party_id_nameable FOREIGN KEY (id) REFERENCES public.nameable(id) NOT VALID;


--
-- TOC entry 4983 (class 2606 OID 19134)
-- Name: party_political_entity_relation fk_party_political_entity_relation_document_proof; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.party_political_entity_relation
    ADD CONSTRAINT fk_party_political_entity_relation_document_proof FOREIGN KEY (document_id_proof) REFERENCES public.document(id) NOT VALID;


--
-- TOC entry 4984 (class 2606 OID 19139)
-- Name: party_political_entity_relation fk_party_political_entity_relation_party; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.party_political_entity_relation
    ADD CONSTRAINT fk_party_political_entity_relation_party FOREIGN KEY (party_id) REFERENCES public.party(id) NOT VALID;


--
-- TOC entry 4985 (class 2606 OID 19144)
-- Name: party_political_entity_relation fk_party_political_entity_relation_political_entity; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.party_political_entity_relation
    ADD CONSTRAINT fk_party_political_entity_relation_political_entity FOREIGN KEY (political_entity_id) REFERENCES public.political_entity(id) NOT VALID;


--
-- TOC entry 4986 (class 2606 OID 19149)
-- Name: party_political_entity_relation fk_party_political_entity_relation_political_entity_relation_ty; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.party_political_entity_relation
    ADD CONSTRAINT fk_party_political_entity_relation_political_entity_relation_ty FOREIGN KEY (party_political_entity_relation_type_id) REFERENCES public.party_political_entity_relation_type(id) NOT VALID;


--
-- TOC entry 4988 (class 2606 OID 19154)
-- Name: party_political_entity_relation_type fk_party_political_entity_relation_type_id_nameable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.party_political_entity_relation_type
    ADD CONSTRAINT fk_party_political_entity_relation_type_id_nameable FOREIGN KEY (id) REFERENCES public.nameable(id) NOT VALID;


--
-- TOC entry 4987 (class 2606 OID 19159)
-- Name: party_political_entity_relation fk_party_politicial_entity_relation_id_node; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.party_political_entity_relation
    ADD CONSTRAINT fk_party_politicial_entity_relation_id_node FOREIGN KEY (id) REFERENCES public.node(id) NOT VALID;


--
-- TOC entry 4991 (class 2606 OID 19164)
-- Name: person_organization_relation fk_person_collective_relation_person_collective_relation_type; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.person_organization_relation
    ADD CONSTRAINT fk_person_collective_relation_person_collective_relation_type FOREIGN KEY (person_organization_relation_type_id) REFERENCES public.person_organization_relation_type(id) NOT VALID;


--
-- TOC entry 4990 (class 2606 OID 19169)
-- Name: person fk_person_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.person
    ADD CONSTRAINT fk_person_id FOREIGN KEY (id) REFERENCES public.party(id) NOT VALID;


--
-- TOC entry 4992 (class 2606 OID 19174)
-- Name: person_organization_relation fk_person_organization_relation_document; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.person_organization_relation
    ADD CONSTRAINT fk_person_organization_relation_document FOREIGN KEY (document_id_proof) REFERENCES public.document(id) NOT VALID;


--
-- TOC entry 4993 (class 2606 OID 19179)
-- Name: person_organization_relation fk_person_organization_relation_id_node; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.person_organization_relation
    ADD CONSTRAINT fk_person_organization_relation_id_node FOREIGN KEY (id) REFERENCES public.node(id) NOT VALID;


--
-- TOC entry 4994 (class 2606 OID 19184)
-- Name: person_organization_relation fk_person_organization_relation_person; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.person_organization_relation
    ADD CONSTRAINT fk_person_organization_relation_person FOREIGN KEY (person_id) REFERENCES public.person(id) NOT VALID;


--
-- TOC entry 4997 (class 2606 OID 19189)
-- Name: person_organization_relation_type fk_person_organization_relation_type_id_nameable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.person_organization_relation_type
    ADD CONSTRAINT fk_person_organization_relation_type_id_nameable FOREIGN KEY (id) REFERENCES public.nameable(id) NOT VALID;


--
-- TOC entry 4998 (class 2606 OID 19194)
-- Name: placement_agency fk_placement_agency_id_organizational_role; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.placement_agency
    ADD CONSTRAINT fk_placement_agency_id_organizational_role FOREIGN KEY (id) REFERENCES public.organizational_role(id) NOT VALID;


--
-- TOC entry 4999 (class 2606 OID 19199)
-- Name: political_entity fk_political_entity_file_flag; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.political_entity
    ADD CONSTRAINT fk_political_entity_file_flag FOREIGN KEY (file_id_flag) REFERENCES public.file(id) NOT VALID;


--
-- TOC entry 5000 (class 2606 OID 19204)
-- Name: political_entity fk_political_entity_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.political_entity
    ADD CONSTRAINT fk_political_entity_id FOREIGN KEY (id) REFERENCES public.geographical_entity(id) NOT VALID;


--
-- TOC entry 5001 (class 2606 OID 19209)
-- Name: poll fk_poll_node; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.poll
    ADD CONSTRAINT fk_poll_node FOREIGN KEY (id) REFERENCES public.node(id) NOT VALID;


--
-- TOC entry 5003 (class 2606 OID 19214)
-- Name: poll_option fk_poll_option_pole_question; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.poll_option
    ADD CONSTRAINT fk_poll_option_pole_question FOREIGN KEY (poll_question_id) REFERENCES public.poll_question(id) NOT VALID;


--
-- TOC entry 5002 (class 2606 OID 19219)
-- Name: poll fk_poll_poll_status; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.poll
    ADD CONSTRAINT fk_poll_poll_status FOREIGN KEY (poll_status_id) REFERENCES public.poll_status(id) NOT VALID;


--
-- TOC entry 5005 (class 2606 OID 19224)
-- Name: poll_vote fk_poll_vote_poll_option; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.poll_vote
    ADD CONSTRAINT fk_poll_vote_poll_option FOREIGN KEY (poll_id, delta) REFERENCES public.poll_option(poll_question_id, delta) NOT VALID;


--
-- TOC entry 5006 (class 2606 OID 19229)
-- Name: poll_vote fk_poll_vote_user; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.poll_vote
    ADD CONSTRAINT fk_poll_vote_user FOREIGN KEY (user_id) REFERENCES public."user"(id) NOT VALID;


--
-- TOC entry 5007 (class 2606 OID 19234)
-- Name: post_placement_agency fk_post_placement_agency_id_organizational_role; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.post_placement_agency
    ADD CONSTRAINT fk_post_placement_agency_id_organizational_role FOREIGN KEY (id) REFERENCES public.organizational_role(id) NOT VALID;


--
-- TOC entry 5008 (class 2606 OID 19239)
-- Name: profession fk_profession_id_nameable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.profession
    ADD CONSTRAINT fk_profession_id_nameable FOREIGN KEY (id) REFERENCES public.nameable(id) NOT VALID;


--
-- TOC entry 5009 (class 2606 OID 19244)
-- Name: professional_role fk_professional_role_person; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.professional_role
    ADD CONSTRAINT fk_professional_role_person FOREIGN KEY (person_id) REFERENCES public.person(id) NOT VALID;


--
-- TOC entry 5010 (class 2606 OID 19249)
-- Name: professional_role fk_professional_role_profession; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.professional_role
    ADD CONSTRAINT fk_professional_role_profession FOREIGN KEY (profession_id) REFERENCES public.profession(id) NOT VALID;


--
-- TOC entry 5011 (class 2606 OID 19254)
-- Name: publisher fk_publisher_id_principal; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.publisher
    ADD CONSTRAINT fk_publisher_id_principal FOREIGN KEY (id) REFERENCES public.principal(id) NOT VALID;


--
-- TOC entry 5077 (class 2606 OID 139269)
-- Name: publishing_user_group fk_publishing_user_group_id_user_group; Type: FK CONSTRAINT; Schema: public; Owner: niels
--

ALTER TABLE ONLY public.publishing_user_group
    ADD CONSTRAINT fk_publishing_user_group_id_user_group FOREIGN KEY (id) REFERENCES public.user_group(id) NOT VALID;


--
-- TOC entry 5078 (class 2606 OID 139275)
-- Name: publishing_user_group fk_publishing_user_group_publication_status_default; Type: FK CONSTRAINT; Schema: public; Owner: niels
--

ALTER TABLE ONLY public.publishing_user_group
    ADD CONSTRAINT fk_publishing_user_group_publication_status_default FOREIGN KEY (publication_status_id_default) REFERENCES public.publication_status(id) NOT VALID;


--
-- TOC entry 5013 (class 2606 OID 19259)
-- Name: representative_house_bill_action fk_representative_house_bill_action_bill_action_type; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.representative_house_bill_action
    ADD CONSTRAINT fk_representative_house_bill_action_bill_action_type FOREIGN KEY (bill_action_type_id) REFERENCES public.bill_action_type(id) NOT VALID;


--
-- TOC entry 5014 (class 2606 OID 19264)
-- Name: representative_house_bill_action fk_representative_house_bill_action_house_bill; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.representative_house_bill_action
    ADD CONSTRAINT fk_representative_house_bill_action_house_bill FOREIGN KEY (house_bill_id) REFERENCES public.house_bill(id) NOT VALID;


--
-- TOC entry 5015 (class 2606 OID 19269)
-- Name: representative_house_bill_action fk_representative_house_bill_action_representative; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.representative_house_bill_action
    ADD CONSTRAINT fk_representative_house_bill_action_representative FOREIGN KEY (representative_id) REFERENCES public.representative(id) NOT VALID;


--
-- TOC entry 5012 (class 2606 OID 19274)
-- Name: representative fk_representative_member_of_congress; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.representative
    ADD CONSTRAINT fk_representative_member_of_congress FOREIGN KEY (id) REFERENCES public.member_of_congress(id) NOT VALID;


--
-- TOC entry 5016 (class 2606 OID 19279)
-- Name: review fk_review_id_simple_text_node; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.review
    ADD CONSTRAINT fk_review_id_simple_text_node FOREIGN KEY (id) REFERENCES public.simple_text_node(id) NOT VALID;


--
-- TOC entry 5017 (class 2606 OID 19284)
-- Name: searchable fk_searchable_node; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.searchable
    ADD CONSTRAINT fk_searchable_node FOREIGN KEY (id) REFERENCES public.node(id) NOT VALID;


--
-- TOC entry 5018 (class 2606 OID 19289)
-- Name: second_level_global_region fk_second_level_global_region_first_level_global_region; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.second_level_global_region
    ADD CONSTRAINT fk_second_level_global_region_first_level_global_region FOREIGN KEY (first_level_global_region_id) REFERENCES public.first_level_global_region(id) NOT VALID;


--
-- TOC entry 5019 (class 2606 OID 19294)
-- Name: second_level_global_region fk_second_level_global_region_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.second_level_global_region
    ADD CONSTRAINT fk_second_level_global_region_id FOREIGN KEY (id) REFERENCES public.global_region(id) NOT VALID;


--
-- TOC entry 5020 (class 2606 OID 19299)
-- Name: second_level_subdivision fk_second_level_subdivision_id_bottom_level_subdivision; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.second_level_subdivision
    ADD CONSTRAINT fk_second_level_subdivision_id_bottom_level_subdivision FOREIGN KEY (id) REFERENCES public.bottom_level_subdivision(id) NOT VALID;


--
-- TOC entry 5021 (class 2606 OID 19304)
-- Name: second_level_subdivision fk_second_level_subdivision_id_iso_coded_subdivision; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.second_level_subdivision
    ADD CONSTRAINT fk_second_level_subdivision_id_iso_coded_subdivision FOREIGN KEY (id) REFERENCES public.iso_coded_subdivision(id) NOT VALID;


--
-- TOC entry 5022 (class 2606 OID 19309)
-- Name: senate_bill fk_senate_bill_id_bill; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.senate_bill
    ADD CONSTRAINT fk_senate_bill_id_bill FOREIGN KEY (id) REFERENCES public.bill(id) NOT VALID;


--
-- TOC entry 5023 (class 2606 OID 19314)
-- Name: senate_term fk_senate_term_id_congressional_term; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.senate_term
    ADD CONSTRAINT fk_senate_term_id_congressional_term FOREIGN KEY (id) REFERENCES public.congressional_term(id) NOT VALID;


--
-- TOC entry 5024 (class 2606 OID 19319)
-- Name: senate_term fk_senate_term_senator; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.senate_term
    ADD CONSTRAINT fk_senate_term_senator FOREIGN KEY (senator_id) REFERENCES public.senator(id) NOT VALID;


--
-- TOC entry 5025 (class 2606 OID 19324)
-- Name: senate_term fk_senate_term_subdivision; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.senate_term
    ADD CONSTRAINT fk_senate_term_subdivision FOREIGN KEY (subdivision_id) REFERENCES public.subdivision(id) NOT VALID;


--
-- TOC entry 5026 (class 2606 OID 19329)
-- Name: senator fk_senator_member_of_congress; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.senator
    ADD CONSTRAINT fk_senator_member_of_congress FOREIGN KEY (id) REFERENCES public.member_of_congress(id) NOT VALID;


--
-- TOC entry 5027 (class 2606 OID 19334)
-- Name: senator_senate_bill_action fk_senator_senate_bill_action_bill_action_type; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.senator_senate_bill_action
    ADD CONSTRAINT fk_senator_senate_bill_action_bill_action_type FOREIGN KEY (bill_action_type_id) REFERENCES public.bill_action_type(id);


--
-- TOC entry 5028 (class 2606 OID 19339)
-- Name: senator_senate_bill_action fk_senator_senate_bill_action_senate_bill; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.senator_senate_bill_action
    ADD CONSTRAINT fk_senator_senate_bill_action_senate_bill FOREIGN KEY (senate_bill_id) REFERENCES public.senate_bill(id);


--
-- TOC entry 5029 (class 2606 OID 19344)
-- Name: senator_senate_bill_action fk_senator_senate_bill_action_senator; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.senator_senate_bill_action
    ADD CONSTRAINT fk_senator_senate_bill_action_senator FOREIGN KEY (senator_id) REFERENCES public.senator(id);


--
-- TOC entry 5030 (class 2606 OID 19349)
-- Name: simple_text_node fk_simple_text_node_id_searchable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.simple_text_node
    ADD CONSTRAINT fk_simple_text_node_id_searchable FOREIGN KEY (id) REFERENCES public.searchable(id) NOT VALID;


--
-- TOC entry 5031 (class 2606 OID 19354)
-- Name: single_question_poll fk_single_question_poll_id_poll; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.single_question_poll
    ADD CONSTRAINT fk_single_question_poll_id_poll FOREIGN KEY (id) REFERENCES public.poll(id) NOT VALID;


--
-- TOC entry 5032 (class 2606 OID 19359)
-- Name: single_question_poll fk_single_question_poll_id_poll_question; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.single_question_poll
    ADD CONSTRAINT fk_single_question_poll_id_poll_question FOREIGN KEY (id) REFERENCES public.poll_question(id) NOT VALID;


--
-- TOC entry 5033 (class 2606 OID 19364)
-- Name: subdivision fk_subdivision_country; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.subdivision
    ADD CONSTRAINT fk_subdivision_country FOREIGN KEY (country_id) REFERENCES public.country(id) NOT VALID;


--
-- TOC entry 5034 (class 2606 OID 19369)
-- Name: subdivision fk_subdivision_country_subdivision; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.subdivision
    ADD CONSTRAINT fk_subdivision_country_subdivision FOREIGN KEY (country_id, subdivision_type_id) REFERENCES public.country_subdivision_type(country_id, subdivision_type_id) NOT VALID;


--
-- TOC entry 5035 (class 2606 OID 19374)
-- Name: subdivision fk_subdivision_id_geographical_entity; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.subdivision
    ADD CONSTRAINT fk_subdivision_id_geographical_entity FOREIGN KEY (id) REFERENCES public.geographical_entity(id) NOT VALID;


--
-- TOC entry 5036 (class 2606 OID 19379)
-- Name: subdivision fk_subdivision_subdivision_type; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.subdivision
    ADD CONSTRAINT fk_subdivision_subdivision_type FOREIGN KEY (subdivision_type_id) REFERENCES public.subdivision_type(id) NOT VALID;


--
-- TOC entry 5037 (class 2606 OID 19384)
-- Name: subdivision_type fk_subdivision_type_nameable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.subdivision_type
    ADD CONSTRAINT fk_subdivision_type_nameable FOREIGN KEY (id) REFERENCES public.nameable(id) NOT VALID;


--
-- TOC entry 5038 (class 2606 OID 139287)
-- Name: subgroup fk_subgroup_id_publishing_user_group; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.subgroup
    ADD CONSTRAINT fk_subgroup_id_publishing_user_group FOREIGN KEY (id) REFERENCES public.publishing_user_group(id) NOT VALID;


--
-- TOC entry 5039 (class 2606 OID 19394)
-- Name: subgroup fk_subgroup_tenant; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.subgroup
    ADD CONSTRAINT fk_subgroup_tenant FOREIGN KEY (tenant_id) REFERENCES public.tenant(id) NOT VALID;


--
-- TOC entry 5040 (class 2606 OID 19399)
-- Name: system_group fk_system_group_user_group; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.system_group
    ADD CONSTRAINT fk_system_group_user_group FOREIGN KEY (id) REFERENCES public.user_group(id) NOT VALID;


--
-- TOC entry 5041 (class 2606 OID 19404)
-- Name: tenant fk_tenant_access_role_id_not_logged_in; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tenant
    ADD CONSTRAINT fk_tenant_access_role_id_not_logged_in FOREIGN KEY (access_role_id_not_logged_in) REFERENCES public.access_role(id) DEFERRABLE INITIALLY DEFERRED NOT VALID;


--
-- TOC entry 5046 (class 2606 OID 19409)
-- Name: tenant_file fk_tenant_file_file; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tenant_file
    ADD CONSTRAINT fk_tenant_file_file FOREIGN KEY (file_id) REFERENCES public.file(id) NOT VALID;


--
-- TOC entry 5047 (class 2606 OID 19414)
-- Name: tenant_file fk_tenant_file_tenant; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tenant_file
    ADD CONSTRAINT fk_tenant_file_tenant FOREIGN KEY (tenant_id) REFERENCES public.tenant(id) NOT VALID;


--
-- TOC entry 5042 (class 2606 OID 19419)
-- Name: tenant fk_tenant_id_owner; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tenant
    ADD CONSTRAINT fk_tenant_id_owner FOREIGN KEY (id) REFERENCES public.owner(id) NOT VALID;


--
-- TOC entry 5043 (class 2606 OID 139281)
-- Name: tenant fk_tenant_id_publishing_user_group; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tenant
    ADD CONSTRAINT fk_tenant_id_publishing_user_group FOREIGN KEY (id) REFERENCES public.publishing_user_group(id) NOT VALID;


--
-- TOC entry 5052 (class 2606 OID 19424)
-- Name: tenant_node_menu_item fk_tenant_node_menu_item_id_menu_item; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tenant_node_menu_item
    ADD CONSTRAINT fk_tenant_node_menu_item_id_menu_item FOREIGN KEY (id) REFERENCES public.menu_item(id) NOT VALID;


--
-- TOC entry 5053 (class 2606 OID 19429)
-- Name: tenant_node_menu_item fk_tenant_node_menu_item_tenant_node; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tenant_node_menu_item
    ADD CONSTRAINT fk_tenant_node_menu_item_tenant_node FOREIGN KEY (tenant_node_id) REFERENCES public.tenant_node(id) NOT VALID;


--
-- TOC entry 5048 (class 2606 OID 19434)
-- Name: tenant_node fk_tenant_node_node; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tenant_node
    ADD CONSTRAINT fk_tenant_node_node FOREIGN KEY (node_id) REFERENCES public.node(id) NOT VALID;


--
-- TOC entry 5049 (class 2606 OID 19439)
-- Name: tenant_node fk_tenant_node_publication_status; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tenant_node
    ADD CONSTRAINT fk_tenant_node_publication_status FOREIGN KEY (publication_status_id) REFERENCES public.publication_status(id) NOT VALID;


--
-- TOC entry 5050 (class 2606 OID 19444)
-- Name: tenant_node fk_tenant_node_subgroup; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tenant_node
    ADD CONSTRAINT fk_tenant_node_subgroup FOREIGN KEY (subgroup_id) REFERENCES public.subgroup(id) NOT VALID;


--
-- TOC entry 5051 (class 2606 OID 19449)
-- Name: tenant_node fk_tenant_node_tenant; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tenant_node
    ADD CONSTRAINT fk_tenant_node_tenant FOREIGN KEY (tenant_id) REFERENCES public.tenant(id) NOT VALID;


--
-- TOC entry 5044 (class 2606 OID 19454)
-- Name: tenant fk_tenant_vocabulary_tagging; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tenant
    ADD CONSTRAINT fk_tenant_vocabulary_tagging FOREIGN KEY (vocabulary_id_tagging) REFERENCES public.vocabulary(id) NOT VALID;


--
-- TOC entry 5056 (class 2606 OID 19459)
-- Name: term_hierarchy fk_term_hierarchy_child; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.term_hierarchy
    ADD CONSTRAINT fk_term_hierarchy_child FOREIGN KEY (term_id_child) REFERENCES public.term(id) NOT VALID;


--
-- TOC entry 5057 (class 2606 OID 19464)
-- Name: term_hierarchy fk_term_hierarchy_parent; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.term_hierarchy
    ADD CONSTRAINT fk_term_hierarchy_parent FOREIGN KEY (term_id_parent) REFERENCES public.term(id) NOT VALID;


--
-- TOC entry 5054 (class 2606 OID 19469)
-- Name: term fk_term_nameable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.term
    ADD CONSTRAINT fk_term_nameable FOREIGN KEY (nameable_id) REFERENCES public.nameable(id) NOT VALID;


--
-- TOC entry 5055 (class 2606 OID 19474)
-- Name: term fk_term_vocabulary; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.term
    ADD CONSTRAINT fk_term_vocabulary FOREIGN KEY (vocabulary_id) REFERENCES public.vocabulary(id) NOT VALID;


--
-- TOC entry 5058 (class 2606 OID 19479)
-- Name: top_level_country fk_top_level_country_global_region; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.top_level_country
    ADD CONSTRAINT fk_top_level_country_global_region FOREIGN KEY (global_region_id) REFERENCES public.global_region(id) NOT VALID;


--
-- TOC entry 5059 (class 2606 OID 19484)
-- Name: top_level_country fk_top_level_country_id_country; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.top_level_country
    ADD CONSTRAINT fk_top_level_country_id_country FOREIGN KEY (id) REFERENCES public.country(id) NOT VALID;


--
-- TOC entry 5060 (class 2606 OID 19489)
-- Name: type_of_abuse fk_type_of_abuse_id_nameable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.type_of_abuse
    ADD CONSTRAINT fk_type_of_abuse_id_nameable FOREIGN KEY (id) REFERENCES public.nameable(id) NOT VALID;


--
-- TOC entry 5061 (class 2606 OID 19494)
-- Name: type_of_abuser fk_type_of_abuser_id_nameable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.type_of_abuser
    ADD CONSTRAINT fk_type_of_abuser_id_nameable FOREIGN KEY (id) REFERENCES public.nameable(id) NOT VALID;


--
-- TOC entry 5062 (class 2606 OID 19499)
-- Name: united_states_congressional_meeting fk_united_states_congressional_meeting_documentable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.united_states_congressional_meeting
    ADD CONSTRAINT fk_united_states_congressional_meeting_documentable FOREIGN KEY (id) REFERENCES public.documentable(id) NOT VALID;


--
-- TOC entry 5063 (class 2606 OID 19504)
-- Name: united_states_congressional_meeting fk_united_states_congressional_meeting_nameable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.united_states_congressional_meeting
    ADD CONSTRAINT fk_united_states_congressional_meeting_nameable FOREIGN KEY (id) REFERENCES public.nameable(id) NOT VALID;


--
-- TOC entry 5065 (class 2606 OID 19509)
-- Name: united_states_political_party_affiliation fk_united_states_politcal_party_affiliation_united_states_polit; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.united_states_political_party_affiliation
    ADD CONSTRAINT fk_united_states_politcal_party_affiliation_united_states_polit FOREIGN KEY (united_states_political_party_id) REFERENCES public.united_states_political_party(id) NOT VALID;


--
-- TOC entry 5066 (class 2606 OID 19514)
-- Name: united_states_political_party_affiliation fk_united_states_political_party_affiliation_id_nameable; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.united_states_political_party_affiliation
    ADD CONSTRAINT fk_united_states_political_party_affiliation_id_nameable FOREIGN KEY (id) REFERENCES public.nameable(id) NOT VALID;


--
-- TOC entry 5064 (class 2606 OID 19519)
-- Name: united_states_political_party fk_united_states_political_party_id_organization; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.united_states_political_party
    ADD CONSTRAINT fk_united_states_political_party_id_organization FOREIGN KEY (id) REFERENCES public.organization(id) NOT VALID;


--
-- TOC entry 5068 (class 2606 OID 19524)
-- Name: user_group fk_user_group_administrator_role_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.user_group
    ADD CONSTRAINT fk_user_group_administrator_role_id FOREIGN KEY (administrator_role_id) REFERENCES public.administrator_role(id) DEFERRABLE INITIALLY DEFERRED NOT VALID;


--
-- TOC entry 5069 (class 2606 OID 19529)
-- Name: user_group_user_role_user fk_user_group_user_role_user_user; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.user_group_user_role_user
    ADD CONSTRAINT fk_user_group_user_role_user_user FOREIGN KEY (user_id) REFERENCES public."user"(id) NOT VALID;


--
-- TOC entry 5070 (class 2606 OID 19534)
-- Name: user_group_user_role_user fk_user_group_user_role_user_user_group; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.user_group_user_role_user
    ADD CONSTRAINT fk_user_group_user_role_user_user_group FOREIGN KEY (user_group_id) REFERENCES public.user_group(id) NOT VALID;


--
-- TOC entry 5071 (class 2606 OID 19539)
-- Name: user_group_user_role_user fk_user_group_user_role_user_user_role; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.user_group_user_role_user
    ADD CONSTRAINT fk_user_group_user_role_user_user_role FOREIGN KEY (user_role_id) REFERENCES public.user_role(id) NOT VALID;


--
-- TOC entry 5067 (class 2606 OID 19544)
-- Name: user fk_user_id_publisher; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."user"
    ADD CONSTRAINT fk_user_id_publisher FOREIGN KEY (id) REFERENCES public.publisher(id) NOT VALID;


--
-- TOC entry 5072 (class 2606 OID 19549)
-- Name: user_role fk_user_role_id_principal; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.user_role
    ADD CONSTRAINT fk_user_role_id_principal FOREIGN KEY (id) REFERENCES public.principal(id) NOT VALID;


--
-- TOC entry 5073 (class 2606 OID 19554)
-- Name: user_role fk_user_role_user_group; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.user_role
    ADD CONSTRAINT fk_user_role_user_group FOREIGN KEY (user_group_id) REFERENCES public.user_group(id) NOT VALID;


--
-- TOC entry 5088 (class 2606 OID 1992410)
-- Name: view_node_type_list_action fk_view_node_type_list_action_id_basic_action; Type: FK CONSTRAINT; Schema: public; Owner: niels
--

ALTER TABLE ONLY public.view_node_type_list_action
    ADD CONSTRAINT fk_view_node_type_list_action_id_basic_action FOREIGN KEY (basic_action_id) REFERENCES public.basic_action(id) NOT VALID;


--
-- TOC entry 5089 (class 2606 OID 1992416)
-- Name: view_node_type_list_action fk_view_node_type_list_action_node_type; Type: FK CONSTRAINT; Schema: public; Owner: niels
--

ALTER TABLE ONLY public.view_node_type_list_action
    ADD CONSTRAINT fk_view_node_type_list_action_node_type FOREIGN KEY (node_type_id) REFERENCES public.node_type(id) NOT VALID;


--
-- TOC entry 5074 (class 2606 OID 19559)
-- Name: vocabulary fk_vocabulary_id_node; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.vocabulary
    ADD CONSTRAINT fk_vocabulary_id_node FOREIGN KEY (id) REFERENCES public.node(id) NOT VALID;


--
-- TOC entry 5075 (class 2606 OID 19564)
-- Name: wrongful_medication_case fk_wrongful_medication_case_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.wrongful_medication_case
    ADD CONSTRAINT fk_wrongful_medication_case_id FOREIGN KEY (id) REFERENCES public."case"(id) NOT VALID;


--
-- TOC entry 5076 (class 2606 OID 19569)
-- Name: wrongful_removal_case fk_wrongful_removal_case_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.wrongful_removal_case
    ADD CONSTRAINT fk_wrongful_removal_case_id FOREIGN KEY (id) REFERENCES public."case"(id) NOT VALID;


--
-- TOC entry 4938 (class 2606 OID 19574)
-- Name: inter_organizational_relation inter_organizational_relation_id_node; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.inter_organizational_relation
    ADD CONSTRAINT inter_organizational_relation_id_node FOREIGN KEY (id) REFERENCES public.node(id) NOT VALID;


--
-- TOC entry 5081 (class 2606 OID 1263423)
-- Name: party_act_relation_type party_act_relation_type_id_nameable; Type: FK CONSTRAINT; Schema: public; Owner: niels
--

ALTER TABLE ONLY public.party_act_relation_type
    ADD CONSTRAINT party_act_relation_type_id_nameable FOREIGN KEY (id) REFERENCES public.nameable(id) NOT VALID;


--
-- TOC entry 4995 (class 2606 OID 19579)
-- Name: person_organization_relation person_organization_relation_geographical_entity; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.person_organization_relation
    ADD CONSTRAINT person_organization_relation_geographical_entity FOREIGN KEY (geographical_entity_id) REFERENCES public.geographical_entity(id) NOT VALID;


--
-- TOC entry 4996 (class 2606 OID 19584)
-- Name: person_organization_relation person_organization_relation_organization; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.person_organization_relation
    ADD CONSTRAINT person_organization_relation_organization FOREIGN KEY (organization_id) REFERENCES public.organization(id) NOT VALID;


--
-- TOC entry 5045 (class 2606 OID 262145)
-- Name: tenant tenant_country_default; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tenant
    ADD CONSTRAINT tenant_country_default FOREIGN KEY (country_id_default) REFERENCES public.country(id) NOT VALID;


--
-- TOC entry 5004 (class 2606 OID 19589)
-- Name: poll_question tk_poll_question_id_simple_text_node; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.poll_question
    ADD CONSTRAINT tk_poll_question_id_simple_text_node FOREIGN KEY (id) REFERENCES public.simple_text_node(id) NOT VALID;


--
-- TOC entry 5241 (class 0 OID 0)
-- Dependencies: 7
-- Name: SCHEMA public; Type: ACL; Schema: -; Owner: postgres
--

REVOKE USAGE ON SCHEMA public FROM PUBLIC;
GRANT ALL ON SCHEMA public TO PUBLIC;


-- Completed on 2023-05-07 12:28:24

--
-- PostgreSQL database dump complete
--

