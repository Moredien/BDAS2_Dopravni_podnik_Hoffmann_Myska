create PROCEDURE CheckUserExists (
    p_username IN VARCHAR2,
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
/

CREATE OR REPLACE VIEW UZIVATEL_VIEW AS
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
    f.DATUM_PRIDANI AS FOTO_DATUM_PRIDANI
FROM ST67028.UZIVATELE u
         LEFT JOIN ST67028.TYPY_UZIVATELE tu ON u.ID_TYP_UZIVATELE = tu.ID_TYP_UZIVATELE
         LEFT JOIN ST67028.ADRESY a ON u.ID_ADRESY = a.ID_ADRESY
         LEFT JOIN ST67028.FOTO f ON u.ID_UZIVATELE = f.ID_UZIVATELE;


CREATE OR REPLACE PROCEDURE InsertUzivatel(
    p_uzivatelske_jmeno IN VARCHAR2,
    p_heslo IN VARCHAR2,
    p_jmeno IN VARCHAR2,
    p_prijmeni IN VARCHAR2,
    p_cas_zalozeni IN DATE,
    p_datum_narozeni IN DATE,
    p_typ_uzivatele_nazev IN VARCHAR2,
    p_mesto IN VARCHAR2,
    p_ulice IN VARCHAR2,
    p_cislo_popisne IN NUMBER,
    p_foto_jmeno_souboru IN VARCHAR2 DEFAULT NULL,
    p_foto_data IN BLOB DEFAULT NULL,
    p_foto_datum_pridani IN DATE DEFAULT NULL,
    p_error_message OUT VARCHAR2
) AS
    user_exists NUMBER := 0;
    new_id_adresy NUMBER;
    new_id_uzivatele NUMBER;
    resolved_id_typ_uzivatele NUMBER;
BEGIN
    p_error_message := NULL;

    -- Validate input
    IF p_uzivatelske_jmeno IS NULL OR LENGTH(p_uzivatelske_jmeno) < 3 THEN
        p_error_message := 'Uzivatelske_jmeno must be at least 3 characters long.';
        RETURN;
    END IF;

    IF p_jmeno IS NULL THEN
        p_error_message := 'Jmeno cannot be NULL.';
        RETURN;
    END IF;

    IF p_prijmeni IS NULL THEN
        p_error_message := 'Prijmeni cannot be NULL.';
        RETURN;
    END IF;

    IF p_cas_zalozeni IS NULL OR p_datum_narozeni IS NULL THEN
        p_error_message := 'Cas zalozeni and Datum narozeni cannot be NULL.';
        RETURN;
    END IF;

    IF p_typ_uzivatele_nazev IS NULL THEN
        p_error_message := 'Typ uzivatele nazev cannot be NULL.';
        RETURN;
    END IF;

    BEGIN
        CheckUserExists(p_username => p_uzivatelske_jmeno, p_exists => user_exists);
        IF user_exists > 0 THEN
            p_error_message := 'User with this Uzivatelske_jmeno already exists.';
            RETURN;
        END IF;
    EXCEPTION
        WHEN OTHERS THEN
            p_error_message := 'Error while checking user existence: ' || SQLERRM;
            RETURN;
    END;

    -- Resolve ID_TYP_UZIVATELE
    BEGIN
        SELECT ID_TYP_UZIVATELE
        INTO resolved_id_typ_uzivatele
        FROM ST67028.TYPY_UZIVATELE
        WHERE NAZEV = p_typ_uzivatele_nazev;

        IF resolved_id_typ_uzivatele IS NULL THEN
            p_error_message := 'No matching Typ uzivatele found for the provided NAZEV.';
            RETURN;
        END IF;
    EXCEPTION
        WHEN NO_DATA_FOUND THEN
            p_error_message := 'No matching Typ uzivatele found for the provided NAZEV.';
            RETURN;
        WHEN OTHERS THEN
            p_error_message := 'Error while resolving Typ uzivatele: ' || SQLERRM;
            RETURN;
    END;

    --Insert
    BEGIN
        IF p_mesto IS NOT NULL AND p_ulice IS NOT NULL AND p_cislo_popisne IS NOT NULL THEN
            INSERT INTO ADRESY (MESTO, ULICE, CISLO_POPISNE)
            VALUES (p_mesto, p_ulice, p_cislo_popisne)
            RETURNING ID_ADRESY INTO new_id_adresy;
        ELSE
            new_id_adresy := NULL;
        END IF;
    EXCEPTION
        WHEN OTHERS THEN
            p_error_message := 'Error while inserting address: ' || SQLERRM;
            RETURN;
    END;

    BEGIN
        INSERT INTO UZIVATELE (UZIVATELSKE_JMENO, HESLO, JMENO, PRIJMENI, CAS_ZALOZENI, DATUM_NAROZENI, ID_TYP_UZIVATELE, ID_ADRESY)
        VALUES (p_uzivatelske_jmeno, p_heslo, p_jmeno, p_prijmeni, p_cas_zalozeni, p_datum_narozeni, resolved_id_typ_uzivatele, new_id_adresy)
        RETURNING ID_UZIVATELE INTO new_id_uzivatele;
    EXCEPTION
        WHEN OTHERS THEN
            p_error_message := 'Error while inserting user: ' || SQLERRM;
            RETURN;
    END;

    BEGIN
        IF p_foto_jmeno_souboru IS NOT NULL AND p_foto_data IS NOT NULL AND p_foto_datum_pridani IS NOT NULL THEN
            INSERT INTO FOTO (JMENO_SOUBORU, DATA, DATUM_PRIDANI, ID_UZIVATELE)
            VALUES (p_foto_jmeno_souboru, p_foto_data, p_foto_datum_pridani, new_id_uzivatele);
        END IF;
    EXCEPTION
        WHEN OTHERS THEN
            p_error_message := 'Error while inserting photo: ' || SQLERRM;
            RETURN;
    END;
END;
/