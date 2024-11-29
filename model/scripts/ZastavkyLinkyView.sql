CREATE OR REPLACE VIEW ST67028.ZASTAVKY_LINKY_VIEW AS
SELECT
    l.ID_LINKY,
    l.CISLO_LINKY,
    z.ID_ZASTAVKY,
    z.JMENO AS JMENO_ZASTAVKY,
    zs.ODJEZD
FROM
    ST67028.LINKY l
        JOIN
    ST67028.ZASTAVENI zs ON l.ID_LINKY = zs.ID_LINKY
        JOIN
    ST67028.ZASTAVKY z ON zs.ID_ZASTAVKY = z.ID_ZASTAVKY;
/


CREATE OR REPLACE PROCEDURE FIND_LINKA_FOR_STOPS (
    p_start_zastavka IN VARCHAR2,
    p_end_zastavka IN VARCHAR2,
    p_result OUT SYS_REFCURSOR
)
AS
BEGIN
    OPEN p_result FOR
        SELECT DISTINCT
            v1.CISLO_LINKY AS LINKA,
            v1.JMENO_ZASTAVKY AS START_STOP,
            v2.JMENO_ZASTAVKY AS END_STOP,
            v1.ODJEZD AS START_TIME,
            v2.ODJEZD AS END_TIME
        FROM
            ST67028.ZASTAVKY_LINKY_VIEW v1
                JOIN
            ST67028.ZASTAVKY_LINKY_VIEW v2 ON v1.ID_LINKY = v2.ID_LINKY
        WHERE
            v1.JMENO_ZASTAVKY = p_start_zastavka
          AND v2.JMENO_ZASTAVKY = p_end_zastavka
          AND v1.ODJEZD <= v2.ODJEZD
        ORDER BY
            v1.ODJEZD ASC; 
END;
/

CREATE OR REPLACE PROCEDURE FIND_ODJEZDY_ZE_ZASTAVKY (
    p_jmeno_zastavky IN VARCHAR2,
    p_cas_odjezdu IN DATE DEFAULT NULL,
    p_result OUT SYS_REFCURSOR
)
AS
BEGIN
    OPEN p_result FOR
        SELECT
            l.CISLO_LINKY AS LINKA,
            l.JMENO AS JMENO_LINKY,
            z.JMENO AS JMENO_ZASTAVKY,
            zs.ODJEZD AS CAS_ODJEZDU
        FROM
            ST67028.LINKY l
                JOIN ST67028.ZASTAVENI zs ON l.ID_LINKY = zs.ID_LINKY
                JOIN ST67028.ZASTAVKY z ON zs.ID_ZASTAVKY = z.ID_ZASTAVKY
        WHERE
            z.JMENO = p_jmeno_zastavky
          AND (p_cas_odjezdu IS NULL OR zs.ODJEZD >= p_cas_odjezdu)
        ORDER BY
            zs.ODJEZD;
END;
/