CREATE OR REPLACE PROCEDURE ZALOZIT_KARTU (
    p_zustatek NUMBER,
    p_platnost_od DATE,
    p_platnost_do DATE,
    p_id_zakaznika NUMBER,
    p_foto BLOB,
    p_jmeno_souboru VARCHAR2,
    p_typ_predplatneho VARCHAR2 DEFAULT NULL,
    p_konec_predplatneho DATE DEFAULT NULL
)
AS
    v_id_karty NUMBER;
    v_id_typ_predplatneho NUMBER;
    v_od DATE := SYSDATE;
BEGIN

    IF p_foto IS NULL OR p_jmeno_souboru IS NULL THEN
        RAISE_APPLICATION_ERROR(-20004, 'Chybí informace o fotce. Foto i jméno souboru musí být zadány.');
    END IF;

    INSERT INTO ST67028.KARTY_MHD (
        ZUSTATEK,
        PLATNOST_OD,
        PLATNOST_DO,
        ID_ZAKAZNIKA,
        ID_FOTO
    ) VALUES (
                 p_zustatek,
                 p_platnost_od,
                 p_platnost_do,
                 p_id_zakaznika,
                 NULL
             )
    RETURNING ID_KARTY INTO v_id_karty;

    INSERT INTO ST67028.FOTO (
        JMENO_SOUBORU,
        DATA,
        DATUM_PRIDANI,
        ID_KARTY,
        ID_UZIVATELE
    ) VALUES (
                 p_jmeno_souboru,
                 p_foto,
                 SYSDATE,
                 v_id_karty,
                 NULL
             );

    -- Pokud je zadán typ předplatného, přidáme záznam o předplatném
    IF p_typ_predplatneho IS NOT NULL THEN
        SELECT ID_TYP_PREDPLATNEHO
        INTO v_id_typ_predplatneho
        FROM ST67028.TYPY_PREDPLATNEHO
        WHERE JMENO = p_typ_predplatneho;

        IF p_konec_predplatneho IS NULL THEN
            RAISE_APPLICATION_ERROR(-20003, 'Konec předplatného musí být zadán, pokud je typ předplatného vyplněn.');
        END IF;

        INSERT INTO ST67028.PREDPLATNE (
            OD,
            DO,
            ID_KARTY,
            ID_TYP_PREDPLATNEHO
        ) VALUES (
                     v_od,
                     p_konec_predplatneho,
                     v_id_karty,
                     v_id_typ_predplatneho
                 );
    END IF;

    COMMIT;
    
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        RAISE_APPLICATION_ERROR(-20002, 'Typ předplatného nebyl nalezen: ' || p_typ_predplatneho);
    WHEN OTHERS THEN
        ROLLBACK;
        RAISE;
END;
/


CREATE OR REPLACE PROCEDURE KONTROLA_PLATEB AS
    CURSOR predplatne_cursor IS
        SELECT
            p.ID_PREDPLATNEHO,
            p.OD,
            p.ID_KARTY,
            tp.CENA,
            k.ID_ZAKAZNIKA
        FROM
            ST67028.PREDPLATNE p
                JOIN ST67028.KARTY_MHD k ON p.ID_KARTY = k.ID_KARTY
                JOIN ST67028.TYPY_PREDPLATNEHO tp ON p.ID_TYP_PREDPLATNEHO = tp.ID_TYP_PREDPLATNEHO
        WHERE
            p.OD <= SYSDATE
          AND p.OD > SYSDATE - 7;

    v_id_predplatneho NUMBER;
    v_od DATE;
    v_id_karty NUMBER;
    v_cena NUMBER;
    v_id_zakaznika NUMBER;
    v_pocet_plateb NUMBER;
BEGIN
    OPEN predplatne_cursor;
    LOOP
        FETCH predplatne_cursor INTO v_id_predplatneho, v_od, v_id_karty, v_cena, v_id_zakaznika;
        EXIT WHEN predplatne_cursor%NOTFOUND;

        -- Zkontrolujeme, zda zákazník provedl platbu ve výši ceny předplatného
        SELECT COUNT(*)
        INTO v_pocet_plateb
        FROM ST67028.PLATBY
        WHERE ID_ZAKAZNIKA = v_id_zakaznika
          AND VYSE_PLATBY = v_cena
          AND CAS_PLATBY BETWEEN v_od AND SYSDATE;

        IF v_pocet_plateb = 0 THEN
            DELETE FROM ST67028.PREDPLATNE WHERE ID_KARTY = v_id_karty;
            DELETE FROM ST67028.FOTO WHERE ID_KARTY = v_id_karty;
            DELETE FROM ST67028.KARTY_MHD WHERE ID_KARTY = v_id_karty;

            DBMS_OUTPUT.PUT_LINE('Smazána karta ID: ' || v_id_karty || ' pro zákazníka ID: ' || v_id_zakaznika);
        END IF;
    END LOOP;
    CLOSE predplatne_cursor;

    COMMIT;
END;
/


CREATE OR REPLACE PROCEDURE POVYSIT_ZAMESTNANCE (
    p_id_zamestnance IN NUMBER,
    p_novy_plat IN NUMBER,
    p_id_noveho_nadrizeneho IN NUMBER DEFAULT NULL,
    p_list_podrizenych IN VARCHAR2 DEFAULT NULL -- Seznam podřízených ID oddělených čárkou
) AS
BEGIN
    
    IF p_id_noveho_nadrizeneho IS NOT NULL THEN
        UPDATE ST67028.ZAMESTNANCI
        SET PLAT = p_novy_plat,
            ID_NADRIZENEHO = p_id_noveho_nadrizeneho
        WHERE ID_ZAMESTNANCE = p_id_zamestnance;
    ELSE
        UPDATE ST67028.ZAMESTNANCI
        SET PLAT = p_novy_plat
        WHERE ID_ZAMESTNANCE = p_id_zamestnance;
    END IF;

    -- Aktualizace nadřízeného pro podřízené, pokud je seznam poskytnut
    IF p_list_podrizenych IS NOT NULL THEN
        FOR podrizeny_id IN (
            SELECT TO_NUMBER(REGEXP_SUBSTR(p_list_podrizenych, '[^,]+', 1, LEVEL)) AS ID
            FROM DUAL
            CONNECT BY REGEXP_SUBSTR(p_list_podrizenych, '[^,]+', 1, LEVEL) IS NOT NULL
            ) LOOP
                UPDATE ST67028.ZAMESTNANCI
                SET ID_NADRIZENEHO = p_id_zamestnance
                WHERE ID_ZAMESTNANCE = podrizeny_id.ID;
            END LOOP;
    END IF;
    
    COMMIT;
EXCEPTION
    WHEN OTHERS THEN
        ROLLBACK;
        DBMS_OUTPUT.PUT_LINE('Chyba při povýšení zaměstnance: ' || SQLERRM);
        RAISE;
END POVYSIT_ZAMESTNANCE;
/