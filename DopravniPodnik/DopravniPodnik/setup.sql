DECLARE
    v_count INTEGER;
BEGIN
    SELECT COUNT(*) INTO v_count
    FROM all_objects
    WHERE object_type = 'PROCEDURE'
      AND object_name = 'CHECKUSEREXISTS'
      AND owner = 'ST67028';

    IF v_count = 0 THEN
        EXECUTE IMMEDIATE '
        CREATE OR REPLACE PROCEDURE CheckUserExists (
            p_username IN VARCHAR2(30),
            p_exists OUT NUMBER
        ) AS
        BEGIN
            SELECT COUNT(*)
            INTO p_exists
            FROM ST67028.UZIVATELE
            WHERE UZIVATELSKE_JMENO = p_username;

            IF p_exists > 0 THEN
                p_exists := 1;
            ELSE
                p_exists := 0;
            END IF;
        END CheckUserExists;
        ';
    END IF;
END;

CREATE OR REPLACE VIEW ZakaznikView AS
SELECT
    u.ID_UZIVATELE,
    u.UZIVATELSKE_JMENO,
    u.HESLO,
    u.JMENO,
    u.PRIJMENI,
    u.CAS_ZALOZENI,
    u.DATUM_NAROZENI,
    tu.NAZEV AS TYP_UZIVATELE,
    a.ID_ADRESY,
    a.MESTO,
    a.ULICE,
    a.CISLO_POPISNE,
    f.ID_FOTO,
    f.JMENO_SOUBORU AS FOTO_JMENO_SOUBORU,
    f.DATA AS FOTO_DATA,
    f.DATUM_PRIDANI AS FOTO_DATUM_PRIDANI,
    z.ID_ZAKAZNIKA
FROM ZAKAZNICI z
         JOIN UZIVATELE u ON z.ID_UZIVATELE = u.ID_UZIVATELE
         JOIN TYPY_UZIVATELE tu ON u.ID_TYP_UZIVATELE = tu.ID_TYP_UZIVATELE
         JOIN ADRESY a ON u.ID_ADRESY = a.ID_ADRESY
         LEFT JOIN FOTO f ON u.ID_UZIVATELE = f.ID_UZIVATELE;
