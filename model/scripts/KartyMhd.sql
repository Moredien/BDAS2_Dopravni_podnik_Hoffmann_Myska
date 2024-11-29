CREATE OR REPLACE PROCEDURE ZALOZIT_KARTU (
    p_zustatek NUMBER,
    p_platnost_od DATE,
    p_platnost_do DATE,
    p_id_zakaznika NUMBER,
    p_foto BLOB,
    p_typ_predplatneho VARCHAR2 DEFAULT NULL,
    p_konec_predplatneho DATE DEFAULT NULL
)
AS
    v_id_karty NUMBER;
    v_id_typ_predplatneho NUMBER;
    v_od DATE := SYSDATE;
BEGIN
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

    DBMS_OUTPUT.PUT_LINE('Karta byla úspěšně vytvořena. ID karty: ' || v_id_karty);
    IF p_typ_predplatneho IS NOT NULL THEN
        DBMS_OUTPUT.PUT_LINE('Předplatné bylo úspěšně přidáno. Typ předplatného: ' || p_typ_predplatneho);
    END IF;
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        RAISE_APPLICATION_ERROR(-20002, 'Typ předplatného nebyl nalezen: ' || p_typ_predplatneho);
    WHEN OTHERS THEN
        ROLLBACK;
        RAISE;
END;
/
