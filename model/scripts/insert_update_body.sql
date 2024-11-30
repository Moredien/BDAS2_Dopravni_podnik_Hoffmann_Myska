CREATE OR REPLACE PACKAGE BODY INSERT_UPDATE AS
    PROCEDURE edit_adresy(
    p_id_adresy NUMBER, 
    p_mesto VARCHAR2, 
    p_ulice VARCHAR2, 
    p_cislo_popisne VARCHAR2
    )AS
    BEGIN
        IF p_id_adresy IS NULL THEN
            INSERT INTO adresy(
                mesto, 
                ulice, 
                cislo_popisne
                ) VALUES (
                p_mesto,
                p_ulice,
                p_cislo_popisne
                );
        ELSE
            UPDATE adresy
            SET
                mesto = p_mesto,
                ulice = p_ulice,
                cislo_popisne = cislo_popisne
            WHERE
                id_adresy = p_id_adresy;
        END IF;
        COMMIT;
    END edit_adresy;
    
PROCEDURE edit_foto(
    p_id_foto       NUMBER, 
    p_jmeno_souboru VARCHAR2, 
    p_data          BLOB, 
    p_datum_pridani DATE, 
    p_id_karty      NUMBER, 
    p_id_uzivatele  NUMBER
) AS
BEGIN
    IF p_id_foto IS NULL THEN
        INSERT INTO foto (
            jmeno_souboru, 
            data, 
            datum_pridani, 
            id_karty, 
            id_uzivatele
        ) VALUES (
            p_jmeno_souboru,
            p_data,
            p_datum_pridani,
            p_id_karty,
            p_id_uzivatele
        );
    ELSE
        UPDATE foto
        SET
            jmeno_souboru = p_jmeno_souboru,
            data = p_data,
            datum_pridani = p_datum_pridani,
            id_karty = p_id_karty,
            id_uzivatele = p_id_uzivatele
        WHERE
            id_foto = p_id_foto;
    END IF;
    COMMIT;
END edit_foto;

PROCEDURE edit_jizdy(
    p_id_jizdy NUMBER,
    p_zacatek DATE,
    p_konec DATE,
    p_id_vozidla NUMBER,
    p_id_linky NUMBER,
    p_id_ridice NUMBER
) AS
BEGIN
    IF p_id_jizdy IS NULL THEN
        INSERT INTO jizdy (
            zacatek,
            konec,
            id_vozidla,
            id_linky,
	    id_ridice
        ) VALUES (
            p_zacatek,
            p_konec,
            p_id_vozidla,
            p_id_linky,
	    p_id_ridice 
        );
    ELSE
        UPDATE jizdy
        SET
            zacatek = p_zacatek,
            konec = p_konec,
            id_vozidla = p_id_vozidla,
            id_linky = p_id_linky,
	    id_ridice = p_id_ridice
        WHERE
            id_jizdy = p_id_jizdy;
    END IF;
    COMMIT;
END edit_jizdy;

PROCEDURE edit_karty_mhd(
    p_id_karty NUMBER,
    p_zustatek NUMBER,
    p_platnost_od DATE,
    p_platnost_do DATE,
    p_id_zakaznika NUMBER,
    p_id_foto NUMBER
) AS
BEGIN
    IF p_id_karty IS NULL THEN
        INSERT INTO karty_mhd (
            zustatek,
            platnost_od,
            platnost_do,
            id_zakaznika,
            id_foto
        ) VALUES (
            p_zustatek,
            p_platnost_od,
            p_platnost_do,
            p_id_zakaznika,
            p_id_foto
        );
    ELSE
        UPDATE karty_mhd
        SET
            zustatek = p_zustatek,
            platnost_od = p_platnost_od,
            platnost_do = p_platnost_do,
            id_zakaznika = p_id_zakaznika,
            id_foto = p_id_foto
        WHERE
            id_karty = p_id_karty;
    END IF;
    COMMIT;
END edit_karty_mhd;

PROCEDURE edit_linky(
    p_id_linky NUMBER,
    p_cislo_linky NUMBER,
    p_jmeno VARCHAR2
) AS
BEGIN
    IF p_id_linky IS NULL THEN
        INSERT INTO linky (
            cislo_linky,
            jmeno
        ) VALUES (
            p_cislo_linky,
            p_jmeno
        );
    ELSE
        UPDATE linky
        SET
            cislo_linky = p_cislo_linky,
            jmeno = p_jmeno
        WHERE
            id_linky = p_id_linky;
    END IF;
    COMMIT;
END edit_linky;

PROCEDURE edit_logy(
    p_id_logu NUMBER,
    p_cas DATE,
    p_tabulka VARCHAR2,
    p_operace VARCHAR2,
    p_stara_hodnota VARCHAR2,
    p_nova_hodnota VARCHAR2
) AS
BEGIN
    IF p_id_logu IS NULL THEN
        INSERT INTO logy (
            cas,
            tabulka,
            operace,
            stara_hodnota,
            nova_hodnota
        ) VALUES (
            p_cas,
            p_tabulka,
            p_operace,
            p_stara_hodnota,
            p_nova_hodnota
        );
    ELSE
        UPDATE logy
        SET
            cas = p_cas,
            tabulka = p_tabulka,
            operace = p_operace,
            stara_hodnota = p_stara_hodnota,
            nova_hodnota = p_nova_hodnota
        WHERE
            id_logu = p_id_logu;
    END IF;
    COMMIT;
END edit_logy;
PROCEDURE edit_platby(
    p_id_platby NUMBER,
    p_cas_platby DATE,
    p_vyse_platby NUMBER,
    p_id_zakaznika NUMBER,
    p_typ_platby NUMBER
) AS
BEGIN
    IF p_id_platby IS NULL THEN
        INSERT INTO platby (
            cas_platby,
            vyse_platby,
            id_zakaznika,
            typ_platby
        ) VALUES (
            p_cas_platby,
            p_vyse_platby,
            p_id_zakaznika,
            p_typ_platby
        );
    ELSE
        UPDATE platby
        SET
            cas_platby = p_cas_platby,
            vyse_platby = p_vyse_platby,
            id_zakaznika = p_id_zakaznika,
            typ_platby = p_typ_platby
        WHERE
            id_platby = p_id_platby;
    END IF;
    COMMIT;
END edit_platby;

PROCEDURE edit_platby_kartou(
    p_id_platby NUMBER,
    p_cislo_karty VARCHAR2,
    p_jmeno_majitele VARCHAR2
) AS
BEGIN
    IF p_id_platby IS NULL THEN
        INSERT INTO platby_kartou (
            cislo_karty,
            jmeno_majitele
        ) VALUES (
            p_cislo_karty,
            p_jmeno_majitele
        );
    ELSE
        UPDATE platby_kartou
        SET
            cislo_karty = p_cislo_karty,
            jmeno_majitele = p_jmeno_majitele
        WHERE
            id_platby = p_id_platby;
    END IF;
    COMMIT;
END edit_platby_kartou;

PROCEDURE edit_platby_prevodem(
    p_id_platby NUMBER,
    p_cislo_uctu VARCHAR2
) AS
BEGIN
    IF p_id_platby IS NULL THEN
        INSERT INTO platby_prevodem (
            cislo_uctu
        ) VALUES (
            p_cislo_uctu
        );
    ELSE
        UPDATE platby_prevodem
        SET
            cislo_uctu = p_cislo_uctu
        WHERE
            id_platby = p_id_platby;
    END IF;
    COMMIT;
END edit_platby_prevodem;

PROCEDURE edit_predplatne(
    p_id_predplatneho NUMBER,
    p_od DATE,
    p_do DATE,
    p_id_karty NUMBER,
    p_id_typ_predplatneho NUMBER
) AS
BEGIN
    IF p_id_predplatneho IS NULL THEN
        INSERT INTO predplatne (
            od,
            do,
            id_karty,
            id_typ_predplatneho
        ) VALUES (
            p_od,
            p_do,
            p_id_karty,
            p_id_typ_predplatneho
        );
    ELSE
        UPDATE predplatne
        SET
            od = p_od,
            do = p_do,
            id_karty = p_id_karty,
            id_typ_predplatneho = p_id_typ_predplatneho
        WHERE
            id_predplatneho = p_id_predplatneho;
    END IF;
    COMMIT;
END edit_predplatne;

PROCEDURE edit_ridici(
    p_id_ridice NUMBER,
    p_jmeno VARCHAR2,
    p_prijmeni VARCHAR2
) AS
BEGIN
    IF p_id_ridice IS NULL THEN
        INSERT INTO ridici (
            jmeno,
            prijmeni
        ) VALUES (
            p_jmeno,
            p_prijmeni
        );
    ELSE
        UPDATE ridici
        SET
            jmeno = p_jmeno,
            prijmeni = p_prijmeni
        WHERE
            id_ridice = p_id_ridice;
    END IF;
    COMMIT;
END edit_ridici;
PROCEDURE edit_typy_predplatneho(
    p_id_typ_predplatneho NUMBER,
    p_jmeno VARCHAR2,
    p_cena NUMBER
) AS
BEGIN
    IF p_id_typ_predplatneho IS NULL THEN
        INSERT INTO typy_predplatneho (
            jmeno,
            cena
        ) VALUES (
            p_jmeno,
            p_cena
        );
    ELSE
        UPDATE typy_predplatneho
        SET
            jmeno = p_jmeno,
            cena = p_cena
        WHERE
            id_typ_predplatneho = p_id_typ_predplatneho;
    END IF;
    COMMIT;
END edit_typy_predplatneho;
PROCEDURE edit_typy_uzivatele(
    p_id_typ_uzivatele NUMBER,
    p_nazev VARCHAR2
) AS
BEGIN
    IF p_id_typ_uzivatele IS NULL THEN
        INSERT INTO typy_uzivatele (
            nazev
        ) VALUES (
            p_nazev
        );
    ELSE
        UPDATE typy_uzivatele
        SET
            nazev = p_nazev
        WHERE
            id_typ_uzivatele = p_id_typ_uzivatele;
    END IF;
    COMMIT;
END edit_typy_uzivatele;
PROCEDURE edit_typy_vozidel(
    p_id_typ_vozidla NUMBER,
    p_nazev VARCHAR2
) AS
BEGIN
    IF p_id_typ_vozidla IS NULL THEN
        INSERT INTO typy_vozidel (
            nazev
        ) VALUES (
            p_nazev
        );
    ELSE
        UPDATE typy_vozidel
        SET
            nazev = p_nazev
        WHERE
            id_typ_vozidla = p_id_typ_vozidla;
    END IF;
    COMMIT;
END edit_typy_vozidel;

PROCEDURE edit_uzivatele(
    p_id_uzivatele NUMBER,
    p_uzivatelske_jmeno VARCHAR2,
    p_heslo VARCHAR2,
    p_jmeno VARCHAR2,
    p_prijmeni VARCHAR2,
    p_cas_zalozeni DATE,
    p_datum_narozeni DATE,
    p_id_typ_uzivatele NUMBER,
    p_id_adresy NUMBER
) AS
BEGIN
    IF p_id_uzivatele IS NULL THEN
        INSERT INTO uzivatele (
            uzivatelske_jmeno,
            heslo,
            jmeno,
            prijmeni,
            cas_zalozeni,
            datum_narozeni,
            id_typ_uzivatele,
            id_adresy
        ) VALUES (
            p_uzivatelske_jmeno,
            p_heslo,
            p_jmeno,
            p_prijmeni,
            p_cas_zalozeni,
            p_datum_narozeni,
            p_id_typ_uzivatele,
            p_id_adresy
        );
    ELSE
        UPDATE uzivatele
        SET
            uzivatelske_jmeno = p_uzivatelske_jmeno,
            heslo = p_heslo,
            jmeno = p_jmeno,
            prijmeni = p_prijmeni,
            cas_zalozeni = p_cas_zalozeni,
            datum_narozeni = p_datum_narozeni,
            id_typ_uzivatele = p_id_typ_uzivatele,
            id_adresy = p_id_adresy
        WHERE
            id_uzivatele = p_id_uzivatele;
    END IF;
    COMMIT;
END edit_uzivatele;

PROCEDURE edit_vozidla(
    p_id_vozidla NUMBER,
    p_id_typ_vozidla NUMBER,
    p_znacka VARCHAR2
) AS
BEGIN
    IF p_id_vozidla IS NULL THEN
        INSERT INTO vozidla (
            id_typ_vozidla,
            znacka
        ) VALUES (
            p_id_typ_vozidla,
            p_znacka
        );
    ELSE
        UPDATE vozidla
        SET
            id_typ_vozidla = p_id_typ_vozidla,
            znacka = p_znacka
        WHERE
            id_vozidla = p_id_vozidla;
    END IF;
    COMMIT;
END edit_vozidla;

PROCEDURE edit_zakaznici(
    p_id_zakaznika NUMBER,
    p_id_uzivatele NUMBER
) AS
BEGIN
    IF p_id_zakaznika IS NULL THEN
        INSERT INTO zakaznici (
            id_uzivatele
        ) VALUES (
            p_id_uzivatele
        );
    ELSE
        UPDATE zakaznici
        SET
            id_uzivatele = p_id_uzivatele
        WHERE
            id_zakaznika = p_id_zakaznika;
    END IF;
    COMMIT;
END edit_zakaznici;

PROCEDURE edit_zamestnanci(
    p_id_zamestnance NUMBER,
    p_plat NUMBER,
    p_platnost_uvazku_do DATE,
    p_id_nadrizeneho NUMBER,
    p_id_uzivatele NUMBER
) AS
BEGIN
    IF p_id_zamestnance IS NULL THEN
        INSERT INTO zamestnanci (
            plat,
            platnost_uvazku_do,
            id_nadrizeneho,
            id_uzivatele
        ) VALUES (
            p_plat,
            p_platnost_uvazku_do,
            p_id_nadrizeneho,
            p_id_uzivatele
        );
    ELSE
        UPDATE zamestnanci
        SET
            plat = p_plat,
            platnost_uvazku_do = p_platnost_uvazku_do,
            id_nadrizeneho = p_id_nadrizeneho,
            id_uzivatele = p_id_uzivatele
        WHERE
            id_zamestnance = p_id_zamestnance;
    END IF;
    COMMIT;
END edit_zamestnanci;

PROCEDURE edit_zastaveni(
    p_id_zastaveni NUMBER,
    p_odjezd DATE,
    p_id_linky NUMBER,
    p_id_zastavky NUMBER,
    p_iterace NUMBER,
    p_smer NUMBER
) AS
BEGIN
    IF p_id_zastaveni IS NULL THEN
        INSERT INTO zastaveni (
            odjezd,
            id_linky,
            id_zastavky,
            iterace,
            smer
        ) VALUES (
            p_odjezd,
            p_id_linky,
            p_id_zastavky,
            p_iterace,
            p_smer
        );
    ELSE
        UPDATE zastaveni
        SET
            odjezd = p_odjezd,
            id_linky = p_id_linky,
            id_zastavky = p_id_zastavky,
            iterace = p_iterace,
            smer = p_smer
        WHERE
            id_zastaveni = p_id_zastaveni;
    END IF;
    COMMIT;
END edit_zastaveni;

PROCEDURE edit_zastavky(
    p_id_zastavky NUMBER,
    p_jmeno VARCHAR2
) AS
BEGIN
    IF p_id_zastavky IS NULL THEN
        INSERT INTO zastavky (
            jmeno
        ) VALUES (
            p_jmeno
        );
    ELSE
        UPDATE zastavky
        SET
            jmeno = p_jmeno
        WHERE
            id_zastavky = p_id_zastavky;
    END IF;
    COMMIT;
END edit_zastavky;

PROCEDURE edit_uzivatel_view(
        p_id_uzivatele IN OUT NUMBER,
        p_uzivatelske_jmeno VARCHAR2,
        p_heslo VARCHAR2,
        p_jmeno VARCHAR2,
        p_prijmeni VARCHAR2,
        p_cas_zalozeni DATE,
        p_datum_narozeni DATE,
        p_typ_uzivatele_nazev VARCHAR2,
        p_mesto VARCHAR2,
        p_ulice VARCHAR2,
        p_cislo_popisne NUMBER,
        p_foto_jmeno_souboru VARCHAR2 DEFAULT NULL,
        p_foto_data BLOB DEFAULT NULL,
        p_foto_datum_pridani DATE DEFAULT NULL
    ) AS
        new_id_adresy NUMBER;
resolved_id_typ_uzivatele NUMBER;
v_id_uzivatele NUMBER;
BEGIN
    SELECT ID_TYP_UZIVATELE
    INTO resolved_id_typ_uzivatele
    FROM ST67028.TYPY_UZIVATELE
    WHERE NAZEV = p_typ_uzivatele_nazev;

    IF p_id_uzivatele IS NOT NULL THEN
        UPDATE ADRESY
        SET MESTO = p_mesto,
            ULICE = p_ulice,
            CISLO_POPISNE = p_cislo_popisne
        WHERE ID_ADRESY = (
            SELECT ID_ADRESY FROM UZIVATELE WHERE ID_UZIVATELE = p_id_uzivatele
        );
    ELSE
        INSERT INTO ADRESY (MESTO, ULICE, CISLO_POPISNE)
        VALUES (p_mesto, p_ulice, p_cislo_popisne)
        RETURNING ID_ADRESY INTO new_id_adresy;
    END IF;

    IF p_id_uzivatele IS NOT NULL THEN
        UPDATE UZIVATELE
        SET UZIVATELSKE_JMENO = p_uzivatelske_jmeno,
            HESLO = p_heslo,
            JMENO = p_jmeno,
            PRIJMENI = p_prijmeni,
            CAS_ZALOZENI = p_cas_zalozeni,
            DATUM_NAROZENI = p_datum_narozeni,
            ID_TYP_UZIVATELE = resolved_id_typ_uzivatele,
            ID_ADRESY = COALESCE(new_id_adresy, ID_ADRESY)
        WHERE ID_UZIVATELE = p_id_uzivatele;
    ELSE
        INSERT INTO UZIVATELE (UZIVATELSKE_JMENO, HESLO, JMENO, PRIJMENI, CAS_ZALOZENI, DATUM_NAROZENI, ID_TYP_UZIVATELE, ID_ADRESY)
        VALUES (p_uzivatelske_jmeno, p_heslo, p_jmeno, p_prijmeni, p_cas_zalozeni, p_datum_narozeni, resolved_id_typ_uzivatele, new_id_adresy)
        RETURNING ID_UZIVATELE INTO v_id_uzivatele;

        p_id_uzivatele := v_id_uzivatele;
    END IF;

    IF p_foto_jmeno_souboru IS NOT NULL AND p_foto_data IS NOT NULL AND p_foto_datum_pridani IS NOT NULL THEN
        IF p_id_uzivatele IS NOT NULL THEN
            UPDATE FOTO
            SET JMENO_SOUBORU = p_foto_jmeno_souboru,
                DATA = p_foto_data,
                DATUM_PRIDANI = p_foto_datum_pridani
            WHERE ID_UZIVATELE = p_id_uzivatele;

            IF SQL%ROWCOUNT = 0 THEN
                INSERT INTO FOTO (JMENO_SOUBORU, DATA, DATUM_PRIDANI, ID_UZIVATELE)
                VALUES (p_foto_jmeno_souboru, p_foto_data, p_foto_datum_pridani, p_id_uzivatele);
            END IF;
        END IF;
    END IF;

    COMMIT;
END edit_uzivatel_view;

END INSERT_UPDATE;
/
commit;
