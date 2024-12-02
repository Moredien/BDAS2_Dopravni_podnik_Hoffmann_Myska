CREATE OR REPLACE FUNCTION FIND_LINKA_FOR_STOPS (
    p_start_zastavka IN VARCHAR2,
    p_end_zastavka IN VARCHAR2
) RETURN SYS_REFCURSOR
AS
    v_result SYS_REFCURSOR;
BEGIN
    OPEN v_result FOR
        SELECT DISTINCT
            v1.CISLO_LINKY AS LINKA,
            v1.JMENO_ZASTAVKY AS START_STOP,
            v2.JMENO_ZASTAVKY AS END_STOP,
            v1.ODJEZD AS START_TIME,
            v2.ODJEZD AS END_TIME,
            v1.ITERACE AS TRASA,
            v1.SMER AS DIRECTION
        FROM
            ST67028.ZASTAVKY_LINKY_VIEW v1
                JOIN
            ST67028.ZASTAVKY_LINKY_VIEW v2 ON v1.ID_LINKY = v2.ID_LINKY
                AND v1.ITERACE = v2.ITERACE
                AND v1.SMER = v2.SMER
        WHERE
            v1.JMENO_ZASTAVKY = p_start_zastavka
          AND v2.JMENO_ZASTAVKY = p_end_zastavka
          AND v1.ODJEZD <= v2.ODJEZD
        ORDER BY
            v1.ODJEZD ASC;

    RETURN v_result;
END;
/


CREATE OR REPLACE FUNCTION FIND_ODJEZDY_ZE_ZASTAVKY (
    p_jmeno_zastavky IN VARCHAR2,
    p_cas_odjezdu IN DATE DEFAULT NULL
) RETURN SYS_REFCURSOR
AS
    v_result SYS_REFCURSOR;
BEGIN
    OPEN v_result FOR
        SELECT
            l.CISLO_LINKY AS LINKA,
            l.JMENO AS JMENO_LINKY,
            zs.ODJEZD AS CAS_ODJEZDU,
            (SELECT z_end.JMENO
             FROM ST67028.ZASTAVKY z_end
                      JOIN ST67028.ZASTAVENI zs_end
                           ON z_end.ID_ZASTAVKY = zs_end.ID_ZASTAVKY
             WHERE zs_end.ID_LINKY = l.ID_LINKY
               AND zs_end.ITERACE = zs.ITERACE
               AND zs_end.SMER = zs.SMER
               AND zs_end.ODJEZD = (SELECT MAX(zs_sub.ODJEZD)
                                    FROM ST67028.ZASTAVENI zs_sub
                                    WHERE zs_sub.ID_LINKY = l.ID_LINKY
                                      AND zs_sub.ITERACE = zs.ITERACE
                                      AND zs_sub.SMER = zs.SMER)
                 FETCH FIRST 1 ROW ONLY) AS JMENO_POSLEDNI_ZASTAVKY
        FROM
            ST67028.LINKY l
                JOIN ST67028.ZASTAVENI zs ON l.ID_LINKY = zs.ID_LINKY
                JOIN ST67028.ZASTAVKY z ON zs.ID_ZASTAVKY = z.ID_ZASTAVKY
        WHERE
            z.JMENO = p_jmeno_zastavky
          AND (p_cas_odjezdu IS NULL OR zs.ODJEZD >= p_cas_odjezdu)
          AND z.JMENO != (SELECT z_end.JMENO
                          FROM ST67028.ZASTAVKY z_end
                                   JOIN ST67028.ZASTAVENI zs_end
                                        ON z_end.ID_ZASTAVKY = zs_end.ID_ZASTAVKY
                          WHERE zs_end.ID_LINKY = l.ID_LINKY
                            AND zs_end.ITERACE = zs.ITERACE
                            AND zs_end.SMER = zs.SMER
                            AND zs_end.ODJEZD = (SELECT MAX(zs_sub.ODJEZD)
                                                 FROM ST67028.ZASTAVENI zs_sub
                                                 WHERE zs_sub.ID_LINKY = l.ID_LINKY
                                                   AND zs_sub.ITERACE = zs.ITERACE
                                                   AND zs_sub.SMER = zs.SMER)
                              FETCH FIRST 1 ROW ONLY)
        ORDER BY
            zs.ODJEZD;

    RETURN v_result;
END;
/


CREATE OR REPLACE FUNCTION GET_EMPLOYEE_HIERARCHY
    RETURN SYS_REFCURSOR
AS
    employee_hierarchy SYS_REFCURSOR;
BEGIN
    OPEN employee_hierarchy FOR
        SELECT
            LEVEL AS HLADINA,
            z.ID_ZAMESTNANCE,
            u.JMENO || ' ' || u.PRIJMENI AS JMENO,
            z.PLAT AS PLAT,
            z.PLATNOST_UVAZKU_DO AS PLATNOST_UVAZKU_DO,
            SYS_CONNECT_BY_PATH(u.JMENO || ' ' || u.PRIJMENI, ' -> ') AS HIERARCHIE
        FROM
            ST67028.ZAMESTNANCI z
                JOIN
            ST67028.UZIVATELE u ON z.ID_UZIVATELE = u.ID_UZIVATELE
        START WITH
            z.ID_NADRIZENEHO IS NULL -- Nejvyšší nadřízený
        CONNECT BY
            PRIOR z.ID_ZAMESTNANCE = z.ID_NADRIZENEHO;
    RETURN employee_hierarchy;
END;
/


CREATE OR REPLACE FUNCTION FIND_PLATBY_BY_ZAKAZNIK (
    p_id_zakaznika IN NUMBER
) RETURN SYS_REFCURSOR
AS
    v_result SYS_REFCURSOR;
BEGIN
    OPEN v_result FOR
        SELECT
            ID_PLATBY,
            CAS_PLATBY,
            VYSE_PLATBY,
            TYP_PLATBY,
            DETAIL_PLATBY
        FROM
            PLATBY_VIEW
        WHERE
            ID_ZAKAZNIKA = p_id_zakaznika
        ORDER BY CAS_PLATBY;

    RETURN v_result;
END;
