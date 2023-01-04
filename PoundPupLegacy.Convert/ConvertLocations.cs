﻿using MySqlConnector;
using Npgsql;
using PoundPupLegacy.Db;
using PoundPupLegacy.Model;
using System.Data;

namespace PoundPupLegacy.Convert;

internal partial class Program
{
    private static List<Location> GetLocations()
    {
        return new List<Location>
        {
            new Location
            {
                Id = 100001,
                Street = "8010 S County Road 5 Suite 205",
                Additional = null,
                PostalCode = "80528",
                City = "Fort Collins",
                SubdivisionId = 2951,
                CountryId = 3805,
                Latitude = decimal.Parse("40.47622551263952"),
                Longitude = decimal.Parse("-104.98063363798134")
            }
        };
    }
    private static List<LocationLocatable> GetLocationLocatables()
    {
        return new List<LocationLocatable>
        {
            new LocationLocatable
            {
                LocationId = 100001,
                LocatableId = 105
            }
        };
    }
    private static string? GetStreet(int id, string? street)
    {
        return id switch
        {
            1420 => "1511 Fredericksburg Rd",
            1137 => "38 Place du Commerce",
            2641 => "1a avenida “A” 2-44",
            2366 => "Bolivar Avenue",
            9618 => "Av Independencia 82",
            2615 => "12 Kurumbapalayam Rd",
            1138 => "Lambhvel Road",
            2699 => "",
            332 => null,
            _ => street
        };
    }
    private static string? GetAdditional(int id, string? additional)
    {
        return id switch
        {
            1137 => "porte # 11-616",
            2366 => "Villa Hermosa neighborhood",
            735 => "Colonia Centro, Centro, Cuauhtémoc",
            _ => additional
        };
    }
    private static string? GetPostalCode(int id, string? postalCode)
    {
        return id switch
        {
            1420 => "78201",
            1137 => "H3E 1T8",
            735 => "06000",
            _ => postalCode
        };
    }
    private static decimal? GetLatitude(int id, decimal? longitude)
    {
        return id switch
        {
            1420 => decimal.Parse("29.474257243949754"),
            2699 => decimal.Parse("9.905252"),
            735 => decimal.Parse("19.433611"),
            _ => longitude
        };
    }
    private static decimal? GetLongitude(int id, decimal? lattitude)
    {
        return id switch
        {
            1420 => decimal.Parse("-98.51049867541713"),
            2699 => decimal.Parse("-84.620304"),
            735 => decimal.Parse("-99.144167"),
            _ => lattitude
        };
    }
    private static string? GetCity(int id, string? city)
    {
        return id switch
        {
            231 => "Morristown",
            3937 => "Westville",
            2019 => "Guatemala City",
            686 => "Ouagadougou",
            2635 => "Monrovia",
            1673 => "Lagos",
            2738 => "Addis Ababa",
            476 => "Addis Ababa",
            1638 => "Addis Ababa",
            1838 => "Addis Ababa",
            1976 => "Addis Ababa",
            1600 => "Addis Ababa",
            1864 => "Addis Ababa",
            1546 => "Addis Ababa",
            3565 => "Addis Ababa",
            3217 => "Addis Ababa",
            1636 => "Addis Ababa",
            2787 => "Addis Ababa",
            1609 => "Addis Ababa",
            3535 => "Addis Ababa",
            1586 => "Addis Ababa",
            1547 => "Addis Ababa",
            3566 => "Addis Ababa",
            3562 => "Addis Ababa",
            1637 => "Addis Ababa",
            3698 => "Addis Ababa",
            1903 => "Addis Ababa",
            1118 => "Addis Ababa",
            1989 => "Addis Ababa",
            1642 => "Addis Ababa",
            1596 => "Addis Ababa",
            3533 => "Addis Ababa",
            3567 => "Addis Ababa",
            2676 => "Addis Ababa",
            2677 => "Addis Ababa",
            3660 => "Shashamane",
            2001 => "Wolaita Sodo",
            2699 => "La Ceiba Cascajal",
            2527 => null,
            1137 => "Verdun",
            2216 => "La Uruca",
            2641 => "Guatemala City",
            2582 => "Guatemala City",
            2455 => "Antigua",
            2366 => "Guatemala City",
            2645 => "Guatemala City",
            2679 => "Guatemala City",
            2436 => "Guatemala City",
            2634 => "Guatemala City",
            2462 => "Guatemala City",
            2626 => "Guatemala City",
            2778 => "Guatemala City",
            2583 => "Guatemala City",
            2646 => "Guatemala City",
            2680 => "Guatemala City",
            2441 => "Guatemala City",
            2640 => "Guatemala City",
            2424 => "Guatemala City",
            2501 => "Guatemala City",
            2459 => "Guatemala City",
            1556 => "Guatemala City",
            2310 => "Guatemala City",
            2454 => "Guatemala City",
            2365 => "Guatemala City",
            2681 => "Guatemala City",
            2644 => "Guatemala City",
            2581 => "Guatemala City",
            2438 => "Guatemala City",
            2507 => "Guatemala City",
            2426 => "Guatemala City",
            2502 => "Guatemala City",
            3651 => "Guatemala City",
            2651 => "Guatemala City",
            2315 => "Guatemala City",
            2211 => "Guatemala City",
            2298 => "Guatemala City",
            2314 => "Guatemala City",
            1933 => "Guatemala City",
            735 => "Mexico City",
            1631 => "Sao Bras",
            713 => "Bogota",
            1479 => "Bogota",
            1477 => "El Poblado",
            1481 => "Medellin",
            1478 => "Pance",
            1480 => "Bogota",
            978 => "Quito",
            2615 => "Coimbatore",
            1138 => "Gujarat",
            2534 => "Kathmandu",
            1693 => "Kathmandu",
            1121 => "Kathmandu",
            1694 => "Kathmandu",
            1615 => "Choudwar",
            1125 => "Dimapur",
            2325 => "Koteshwor",
            2326 => "Koteshwor",
            1067 => "Nugegoda",
            2632 => "Boeung Trabek Phnom Penh",
            2356 => "Phnom Penh",
            1720 => "Phnom Penh",
            1674 => "Phnom Penh",
            1169 => "Phnom Penh",
            3507 => "Phnom Penh",
            2174 => "Phnom Penh",
            2212 => "Phnom Penh",
            2340 => "Phnom Penh",
            1891 => "Phnom Penh",
            1892 => "Phnom Penh",
            2328 => "Cham Chao, Phnom Penh",
            2618 => "Cham Chao, Phnom Penh",
            2763 => null,
            2331 => null,
            3906 => "Siem Reap",
            2280 => "Siem Reap",
            2332 => "Siem Reap",
            2857 => "Ciputat, South Tangerang",
            1097 => "Tangerang",
            4088 => "Alor Setar",
            2841 => "Jala Jala",
            2836 => "Jala Jala",
            2840 => "Jala Jala",
            3526 => "Naklua",
            3621 => "Bảo Lộc",
            3597 => "Đồng Hới",
            3595 => "Đồng Hới",
            3596 => "Đồng Hới",
            2784 => "Ho Chi Minh City",
            2781 => "Ho Chi Minh City",
            2586 => null,
            2782 => null,
            2995 => "Bucharest",
            3356 => "Bucharest",
            287 => "Bucharest",
            773 => "Den Haag",
            247 => "Den Haag",
            336 => "Den Haag",
            475 => "Den Haag",
            1144 => "Madrid",
            342 => "London",
            1083 => "Lisboa",

            _ => city
        };
    }

    private static int? GetSubdivisionId(int id, int? stateId, NpgsqlConnection connection)
    {
        if (stateId == null)
        {
            var stateCode = id switch
            {
                1428 => "MX-TLA",
                2019 => "GT-01",
                2073 => "MH-MAJ",
                3838 => "EG-C",
                1632 => "MA-RAB",
                686 => "BF-KAD",
                3395 => "GH-AA",
                3600 => "GH-AA",
                2659 => "LR-MO",
                2635 => "LR-MO",
                2658 => "LR-MO",
                2389 => "NG-RI",
                1905 => "NG-LA",
                1673 => "NG-LA",
                2028 => "NG-LA",
                2770 => "NG-FC",
                2619 => "NG-LA",
                3637 => "CF-BGF",
                3563 => "ET-OR",
                2738 => "ET-AA",
                3601 => "ET-AA",
                2739 => "ET-AA",
                3602 => "ET-AA",
                2496 => "ET-AA",
                3599 => "ET-AA",
                3505 => "ET-AA",
                2497 => "ET-AA",
                476 => "ET-AA",
                1638 => "ET-AA",
                1838 => "ET-AA",
                1976 => "ET-AA",
                1600 => "ET-AA",
                1864 => "ET-AA",
                1546 => "ET-AA",
                3565 => "ET-AA",
                3217 => "ET-AA",
                1636 => "ET-AA",
                2787 => "ET-AA",
                1609 => "ET-AA",
                3535 => "ET-AA",
                1586 => "ET-AA",
                1547 => "ET-AA",
                3566 => "ET-AA",
                3562 => "ET-AA",
                1637 => "ET-AA",
                3698 => "ET-AA",
                1903 => "ET-AA",
                1118 => "ET-AA",
                1989 => "ET-AA",
                1642 => "ET-AA",
                1596 => "ET-AA",
                3533 => "ET-AA",
                3567 => "ET-AA",
                2676 => "ET-AA",
                2677 => "ET-AA",
                3608 => "ET-AM",
                1639 => "ET-OR",
                3561 => "ET-AM",
                3560 => "ET-SI",
                3480 => "ET-SN",
                3660 => "ET-OR",
                2001 => "ET-SN",
                2655 => "KE-17",
                2248 => "KE-28",
                2117 => "KE-30",
                1447 => "KE-30",
                1566 => "KE-30",
                1445 => "KE-30",
                2031 => "KE-30",
                1032 => "MW-BL",
                1971 => "UG-204",
                3929 => "UG-102",
                3900 => "UG-102",
                2701 => "ZM-09",
                2702 => "ZM-09",
                1704 => "ZM-08",
                1702 => "ZM-08",
                1705 => "LS-A",
                1706 => "LS-A",
                811 => "ZA-GP",
                1920 => "ZA-GP",
                1162 => "ZA-GP",
                1916 => "ZA-GP",
                1075 => "ZA-GP",
                1163 => "ZA-GP",
                1918 => "ZA-GP",
                1528 => "ZA-GP",
                1707 => "ZA-WC",
                1922 => "ZA-GP",
                1921 => "ZA-GP",
                1969 => "ZA-GP",
                1524 => "ZA-GP",
                1970 => "ZA-GP",
                1811 => "ZA-GP",
                1525 => "ZA-GP",
                1919 => "ZA-GP",
                1527 => "ZA-GP",
                1137 => "CA-QC",
                2860 => "CA-AB",
                2144 => "CA-AB",
                3776 => "CA-ON",
                3040 => "CA-QC",
                1803 => "CA-QC",
                1731 => "CA-ON",
                3638 => "CA-QC",
                234 => "CA-BC",
                2652 => "CA-AB",
                3724 => "CA-AB",
                3841 => "CA-AB",
                3569 => "CA-AB",
                2869 => "CA-ON",
                1983 => "CA-ON",
                2873 => "CA-ON",
                3835 => "CA-QC",
                1610 => "CA-QC",
                2859 => "CA-AB",
                3394 => "CA-ON",
                1841 => "CA-BC",
                1532 => "CA-NB",
                2059 => "CA-MB",
                3852 => "CA-ON",
                3848 => "CA-NS",
                2149 => "CA-AB",
                3556 => "CA-BC",
                4155 => "CA-QC",
                3266 => "CA-BC",
                4087 => "CA-BC",
                2152 => "CA-MB",
                1498 => "CA-ON",
                1611 => "CA-QC",
                831 => "CA-QC",
                829 => "CA-QC",
                3897 => "CA-BC",
                820 => "CA-ON",
                693 => "CA-BC",
                3521 => "CA-AB",
                827 => "CA-ON",
                3616 => "CA-ON",
                1972 => "CA-ON",
                3983 => "CA-ON",
                2146 => "CA-BC",
                3265 => "CA-AB",
                2858 => "CA-AB",
                818 => "CA-ON",
                3617 => "CA-QC",
                888 => "CA-QC",
                3856 => "CA-ON",
                3939 => "CA-ON",
                2768 => "CA-ON",
                3857 => "CA-ON",
                828 => "CA-ON",
                437 => "CA-ON",
                1448 => "CA-ON",
                819 => "CA-ON",
                3391 => "CA-ON",
                3855 => "CA-ON",
                3938 => "CA-ON",
                830 => "CA-QC",
                1866 => "CA-QC",
                3555 => "CA-BC",
                2503 => "CA-MB",
                2098 => "CA-MB",
                3817 => "BS-NP",
                31599 => "CR-A",
                31597 => "CR-A",
                2216 => "CR-SJ",
                2209 => "CR-SJ",
                1070 => "CR-SJ",
                3884 => "CR-H",
                2571 => "SV-CH",
                2569 => "SV-CH",
                2184 => "SV-SS",
                2517 => "SV-SS",
                2122 => "SV-SS",
                2293 => "SV-SS",
                2291 => "SV-SS",
                2115 => "SV-SS",
                2292 => "SV-SS",
                2641 => "GT-01",
                2487 => "GT-03",
                1934 => "GT-03",
                2490 => "GT-03",
                2455 => "GT-03",
                2366 => "GT-01",
                2638 => "GT-04",
                2792 => "GT-09",
                2582 => "GT-01",
                2645 => "GT-01",
                2679 => "GT-01",
                2436 => "GT-01",
                2634 => "GT-01",
                2462 => "GT-01",
                2626 => "GT-01",
                2778 => "GT-01",
                2583 => "GT-01",
                2646 => "GT-01",
                2680 => "GT-01",
                2441 => "GT-01",
                2640 => "GT-01",
                2424 => "GT-01",
                2501 => "GT-01",
                2459 => "GT-01",
                1556 => "GT-01",
                2310 => "GT-01",
                2454 => "GT-01",
                2365 => "GT-01",
                2681 => "GT-01",
                2644 => "GT-01",
                2581 => "GT-01",
                2438 => "GT-01",
                2507 => "GT-01",
                2426 => "GT-01",
                2502 => "GT-01",
                3651 => "GT-01",
                2651 => "GT-01",
                2315 => "GT-01",
                2211 => "GT-01",
                2298 => "GT-01",
                2314 => "GT-01",
                1933 => "GT-01",
                2299 => "GT-21",
                2300 => "GT-21",
                2439 => "GT-01",
                2437 => "GT-01",
                2039 => "GT-01",
                2578 => "GT-04",
                2478 => "GT-04",
                2666 => "GT-01",
                2461 => "GT-02",
                2662 => "GT-01",
                2463 => "GT-01",
                2464 => "GT-01",
                2440 => "GT-01",
                2637 => "GT-04",
                3532 => "HT-OU",
                3625 => "HT-ND",
                3623 => "HT-ND",
                3527 => "HT-OU",
                1100 => "HT-OU",
                2682 => "HT-GA",
                3416 => "HT-AR",
                3522 => "HT-AR",
                3930 => "HT-AR",
                1585 => "HT-OU",
                861 => "HT-OU",
                3383 => "HT-OU",
                1646 => "HT-OU",
                2064 => "HT-OU",
                3396 => "HT-OU",
                749 => "HT-OU",
                3398 => "HT-OU",
                3780 => "HT-NO",
                3778 => "HT-NO",
                834 => "HT-OU",
                2544 => "HN-FM",
                2513 => "HN-FM",
                2694 => "JM-11",
                735 => "MX-CMX",
                2213 => "MX-SON",
                3670 => "MX-MIC",
                2695 => "MX-BCN",
                3784 => "MX-JAL",
                3785 => "MX-JAL",
                3828 => "MX-JAL",
                3017 => "MX-SIN",
                1402 => "MX-CMX",
                2362 => "MX-VER",
                3088 => "MX-BCN",
                2538 => "AR-B",
                1626 => "BO-P",
                2796 => "BR-SP",
                712 => "BR-DF",
                2540 => "BR-DF",
                1111 => "BR-RJ",
                2539 => "BR-PR",
                2509 => "BR-SC",
                2541 => "BR-SC",
                2207 => "BR-SP",
                1723 => "BR-PE",
                1631 => "BR-PA",
                1722 => "BR-SP",
                1629 => "BR-SP",
                1630 => "BR-BA",
                723 => "CL-RM",
                2611 => "CL-RM",
                1476 => "CO-DC",
                2786 => "CO-DC",
                1475 => "CO-DC",
                1482 => "CO-DC",
                713 => "CO-DC",
                1477 => "CO-ANT",
                1481 => "CO-ANT",
                1478 => "CO-CAU",
                1480 => "CO-DC",
                978 => "EC-P",
                2202 => "PE-LIM",
                1066 => "PE-LIM",
                1557 => "AM-ER",
                802 => "IL-JM",
                3914 => "SA-01",
                1821 => "AE-DU",
                817 => "AE-SH",
                745 => "MN-1",
                2615 => "IN-TN",
                1138 => "IN-GJ",
                2347 => "IN-TN",
                2344 => "IN-TN",
                2403 => "IN-TN",
                2341 => "IN-TN",
                1126 => "IN-TN",
                2391 => "IN-TN",
                2219 => "IN-TN",
                844 => "IN-TN",
                2345 => "IN-TN",
                2342 => "IN-TN",
                2346 => "IN-TN",
                1134 => "IN-TN",
                2421 => "IN-TG",
                2384 => "IN-TG",
                1094 => "IN-TG",
                2364 => "IN-TG",
                1095 => "IN-TG",
                2035 => "IN-TG",
                905 => "IN-MH",
                2367 => "IN-MH",
                2453 => "IN-MH",
                546 => "IN-MH",
                2506 => "IN-MH",
                2253 => "NP-P3",
                3630 => "NP-P3",
                1026 => "NP-P3",
                2537 => "NP-P3",
                2532 => "NP-P3",
                2324 => "NP-P3",
                3631 => "NP-P3",
                985 => "NP-P3",
                2278 => "NP-P3",
                1024 => "NP-P3",
                2535 => "NP-P3",
                2533 => "NP-P3",
                2034 => "NP-P3",
                1025 => "NP-P3",
                2536 => "NP-P3",
                2534 => "NP-P3",
                1693 => "NP-P3",
                1121 => "NP-P3",
                1694 => "NP-P3",
                2399 => "IN-KA",
                2414 => "IN-MH",
                1695 => "IN-WB",
                1615 => "IN-OR",
                1125 => "IN-NL",
                3812 => "IN-MP",
                1644 => "IN-TN",
                4085 => "IN-KA",
                2420 => "IN-TG",
                1161 => "IN-MH",
                4086 => "IN-KA",
                601 => "IN-DL",
                2614 => "IN-TN",
                2283 => "IN-TN",
                1613 => "IN-OR",
                2187 => "IN-MH",
                1645 => "IN-AP",
                1935 => "IN-TN",
                3633 => "NP-P3",
                2531 => "NP-P3",
                2038 => "NP-P4",
                2325 => "NP-P3",
                2326 => "NP-P3",
                1023 => "NP-P3",
                1022 => "NP-P3",
                3626 => "PK-IS",
                906 => "PK-SD",
                3846 => "PK-SD",
                1067 => "LK-1",
                2632 => "KH-12",
                2356 => "KH-12",
                1720 => "KH-12",
                1674 => "KH-12",
                1169 => "KH-12",
                3507 => "KH-12",
                2174 => "KH-12",
                2212 => "KH-12",
                2340 => "KH-12",
                1891 => "KH-12",
                1892 => "KH-12",
                2328 => "KH-12",
                2618 => "KH-12",
                2763 => "KH-3",
                2331 => "KH-5",
                2624 => "KH-17",
                3598 => "KH-3",
                2623 => "KH-12",
                2355 => "KH-12",
                2625 => "KH-15",
                3906 => "KH-17",
                2280 => "KH-17",
                2332 => "KH-17",
                2330 => "KH-18",
                2857 => "ID-BT",
                1096 => "ID-YO",
                1097 => "ID-BT",
                4088 => "MY-02",
                2841 => "PH-RIZ",
                2836 => "PH-RIZ",
                2840 => "PH-RIZ",
                2842 => "PH-RIZ",
                1925 => "PH-00",
                464 => "PH-00",
                463 => "PH-CAV",
                3523 => "TH-10",
                3524 => "TH-10",
                3526 => "TH-20",
                1878 => "TH-20",
                3621 => "VN-35",
                3614 => "VN-HN",
                3597 => "VN-24",
                3595 => "VN-24",
                3596 => "VN-24",
                2266 => "VN-HN",
                2784 => "VN-SG",
                2781 => "VN-SG",
                2780 => "VN-SG",
                2275 => "VN-SG",
                2586 => "VN-66",
                2782 => "VN-47",
                2588 => "VN-44",
                2267 => "VN-44",
                2215 => "VN-13",
                3620 => "VN-67",
                3618 => "VN-67",
                3619 => "VN-67",
                2204 => "VN-18",
                281 => "RO-B",
                2470 => "RO-B",
                662 => "RO-B",
                2495 => "RO-B",
                283 => "RO-B",
                2488 => "RO-B",
                1962 => "RO-B",
                487 => "RO-B",
                1641 => "RO-B",
                285 => "RO-B",
                2251 => "RO-B",
                1634 => "RO-B",
                493 => "RO-B",
                483 => "RO-B",
                290 => "RO-B",
                1961 => "RO-B",
                1960 => "RO-B",
                2995 => "RO-B",
                3356 => "RO-B",
                287 => "RO-B",
                610 => "RU-MOW",
                2193 => "RU-MOW",
                2756 => "RU-MOW",
                2737 => "RU-MOW",
                3510 => "RU-MOW",
                3357 => "RU-MOW",
                1876 => "RU-MOW",
                3318 => "NL-ZH",
                773 => "NL-ZH",
                247 => "NL-ZH",
                336 => "NL-ZH",
                475 => "NL-ZH",
                573 => "NL-ZH",
                489 => "NL-ZH",
                457 => "NL-ZH",
                488 => "NL-ZH",
                993 => "FR-75C",
                1085 => "FR-75C",
                291 => "FR-75C",
                722 => "FR-75C",
                288 => "FR-75C",
                1325 => "FR-75C",
                737 => "FR-75C",
                574 => "FR-75C",
                923 => "FR-75C",
                1991 => "FR-75C",
                492 => "FR-75C",
                911 => "FR-75C",
                727 => "FR-75C",
                670 => "FR-75C",
                897 => "FR-75C",
                1893 => "FR-75C",
                320 => "FR-75C",
                1567 => "BE-BRU",
                1571 => "BE-BRU",
                1568 => "BE-BRU",
                494 => "BE-BRU",
                322 => "BE-BRU",
                1047 => "BE-BRU",
                289 => "BE-BRU",
                260 => "BE-BRU",
                759 => "BE-BRU",
                1148 => "ES-M",
                999 => "ES-M",
                901 => "ES-M",
                1145 => "ES-M",
                1159 => "ES-M",
                1156 => "ES-M",
                1006 => "ES-M",
                3557 => "ES-M",
                823 => "ES-M",
                3490 => "ES-M",
                1020 => "ES-M",
                1001 => "ES-M",
                1038 => "ES-M",
                812 => "ES-M",
                1144 => "ES-M",
                785 => "ES-M",
                1015 => "ES-B",
                1046 => "ES-B",
                841 => "ES-B",
                1009 => "ES-B",
                857 => "ES-B",
                814 => "ES-B",
                3554 => "ES-B",
                855 => "ES-B",
                824 => "ES-B",
                1008 => "ES-B",
                1005 => "ES-B",
                1621 => "IT-62",
                900 => "IT-62",
                918 => "IT-62",
                894 => "IT-62",
                808 => "IT-62",
                805 => "IT-62",
                770 => "IT-62",
                956 => "IT-62",
                936 => "IT-62",
                933 => "IT-62",
                940 => "IT-62",
                937 => "IT-62",
                986 => "IT-62",
                934 => "IT-62",
                922 => "IT-21",
                1617 => "IT-21",
                809 => "IT-21",
                896 => "IT-21",
                852 => "IT-21",
                912 => "IT-21",
                788 => "KR-11",
                789 => "KR-11",
                790 => "KR-11",
                3512 => "KR-11",
                2799 => "KR-11",
                2723 => "KR-11",
                2797 => "KR-11",
                2773 => "KR-11",
                3508 => "CN-BJ",
                2769 => "CN-BJ",
                2363 => "CN-BJ",
                1550 => "FI-18",
                764 => "FI-18",
                832 => "FI-18",
                1549 => "FI-18",
                1956 => "IE-D",
                1957 => "IE-D",
                3953 => "IE-D",
                976 => "SE-AB",
                778 => "SE-AB",
                1454 => "SE-AB",
                2062 => "GB-LND",
                3097 => "GB-LND",
                3992 => "GB-LND",
                342 => "GB-LND",
                2867 => "GB-LND",
                916 => "IT-82",
                951 => "IT-82",
                909 => "IT-82",
                928 => "IT-52",
                902 => "IT-52",
                1620 => "IT-52",
                927 => "IT-52",
                944 => "IT-25",
                1624 => "IT-25",
                771 => "IT-25",
                803 => "IT-25",
                2282 => "PT-11",
                2284 => "PT-11",
                1083 => "PT-11",
                1040 => "ES-MU",
                1010 => "ES-MU",
                1044 => "ES-MU",
                1003 => "ES-PM",
                1151 => "ES-PM",
                3549 => "ES-PM",
                1157 => "ES-SE",
                1140 => "ES-SE",
                3538 => "ES-SE",
                1037 => "ES-TO",
                1013 => "ES-TO",
                1153 => "ES-TO",
                1039 => "ES-V",
                1160 => "ES-V",
                1007 => "ES-V",
                1017 => "ES-V",
                845 => "ES-V",
                3536 => "ES-V",
                3537 => "ES-Z",
                3541 => "ES-Z",
                1150 => "ES-Z",
                1035 => "ES-Z",
                1011 => "ES-Z",
                729 => "BG-22",
                3414 => "BG-22",
                2348 => "BG-22",
                1628 => "BG-22",
                1928 => "BG-22",
                1120 => "DE-NW",
                1335 => "DE-NW",
                765 => "DE-NW",
                767 => "DE-BW",
                1115 => "DE-BW",
                1446 => "DE-BW",
                3727 => "CH-GE",
                1090 => "CH-GE",
                858 => "CH-GE",
                1084 => "CH-GE",
                1081 => "CH-GE",


                _ => null
            };
            if (stateCode == null)
            {
                return null;
            }
            return GetSubdivisionId(stateCode, connection);
        }
        else
        {
            return id switch
            {
                262 => 2954,
                1065 => 2954,
                296 => 2967,
                987 => 2800,
                294 => 2801,
                1896 => 2801,
                2118 => 2001,
                2599 => 2977,
                1420 => 2987,
                2527 => null,
                _ => stateId
            };
        }
    }
    private static int? GetCountryId(int id, int? countryId)
    {
        return id switch
        {
            664 => 4048,
            1428 => 3909,
            2019 => 3904,
            2073 => 4050,
            _ => countryId
        };
    }

    private static async Task MigrateLocations(MySqlConnection mysqlconnection, NpgsqlConnection connection)
    {
        await LocationCreator.CreateAsync(GetLocations().ToAsyncEnumerable(), connection);
        await LocationCreator.CreateAsync(ReadUSLocations(mysqlconnection, connection), connection);
        await LocationLocatableCreator.CreateAsync(GetLocationLocatables().ToAsyncEnumerable(), connection);
    }
    private static async IAsyncEnumerable<Location> ReadUSLocations(MySqlConnection mysqlconnection, NpgsqlConnection connection)
    {

        var sql = $"""
                SELECT 
                l.lid id,
                case when l.street = '' then NULL ELSE l.street END street,
                l.additional,
                case when l.city = '' then NULL ELSE l.city END city,
                l.postal_code,
                s.nid subdivision_id,
                c.nid country_id,
                l.latitude,
                l.longitude
                FROM location l
                JOIN node_revisions nr on nr.vid = l.eid
                JOIN node n ON n.nid = nr.nid AND n.vid = nr.vid
                JOIN content_type_country_type c ON lower(c.field_country_code_value) = lower(l.country)
                JOIN node n2 ON n2.nid = c.nid
                LEFT JOIN content_type_statefact s ON s.field_country_1_nid = c.nid AND s.field_statecode_value = l.province
                """;
        using var readCommand = mysqlconnection.CreateCommand();
        readCommand.CommandType = CommandType.Text;
        readCommand.CommandTimeout = 300;
        readCommand.CommandText = sql;


        var reader = await readCommand.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            yield return new Location
            {
                Id = reader.GetInt32("id"),
                Street = GetStreet(reader.GetInt32("id"), reader.IsDBNull("street") ? null : reader.GetString("street")),
                Additional = GetAdditional(reader.GetInt32("id"), reader.IsDBNull("additional") ? null : reader.GetString("additional")),
                City = GetCity(reader.GetInt32("id"), reader.IsDBNull("city") ? null : reader.GetString("city")),
                PostalCode = GetPostalCode(reader.GetInt32("id"), reader.IsDBNull("postal_code") ? null : reader.GetString("postal_code")),
                SubdivisionId = GetSubdivisionId(reader.GetInt32("id"), reader.IsDBNull("subdivision_id") ? null : reader.GetInt32("subdivision_id"), connection),
                CountryId = GetCountryId(reader.GetInt32("id"), reader.IsDBNull("country_id") ? null : reader.GetInt32("country_id")),
                Latitude = GetLatitude(reader.GetInt32("id"), reader.IsDBNull("latitude") ? null : reader.GetDecimal("latitude")),
                Longitude = GetLongitude(reader.GetInt32("id"), reader.IsDBNull("longitude") ? null : reader.GetDecimal("longitude")),
            };

        }
        await reader.CloseAsync();
    }
}
