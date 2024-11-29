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

