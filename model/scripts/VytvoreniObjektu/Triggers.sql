CREATE OR REPLACE trigger PREVENT_ADMIN_DELETE
    before delete
    on UZIVATELE
    for each row
DECLARE
    v_typ_uzivatele VARCHAR2(20);
BEGIN
    SELECT NAZEV
    INTO v_typ_uzivatele
    FROM TYPY_UZIVATELE
    WHERE ID_TYP_UZIVATELE = :OLD.ID_TYP_UZIVATELE;

    -- Kontrola, zda se jedná o uživatele 'Admin' s typem 'Admin'
    IF :OLD.UZIVATELSKE_JMENO = 'admin' AND v_typ_uzivatele = 'Admin' THEN
        RAISE_APPLICATION_ERROR(-20001, 'Nelze smazat uživatele Admin s typem Admin.');
    END IF;
END;
/

CREATE OR REPLACE TRIGGER UPDATE_ZUSTATEK_TGR
    AFTER INSERT ON ST67028.PLATBY
    FOR EACH ROW
DECLARE
    v_id_karty NUMBER;
    v_exists NUMBER;
BEGIN
    -- Získání ID karty spojené s platbou, pokud existuje
    SELECT k.ID_KARTY
    INTO v_id_karty
    FROM KARTY_MHD k
             JOIN ZAKAZNICI z ON k.ID_ZAKAZNIKA = z.ID_ZAKAZNIKA
    WHERE z.ID_ZAKAZNIKA = :NEW.ID_ZAKAZNIKA
      AND ROWNUM = 1;

    -- Pokud karta existuje v tabulce PREDPLATNE, jedná se kartu, u které se platí za období a neslouží k placení lístků
    SELECT COUNT(*)
    INTO v_exists
    FROM PREDPLATNE
    WHERE ID_KARTY = v_id_karty;

    IF v_exists = 0 THEN
        UPDATE KARTY_MHD
        SET ZUSTATEK = ZUSTATEK + :NEW.VYSE_PLATBY
        WHERE ID_KARTY = v_id_karty;
    END IF;

EXCEPTION
    WHEN NO_DATA_FOUND THEN
        NULL;
    WHEN OTHERS THEN
        RAISE;
END UPDATE_ZUSTATEK_TGR;
/

CREATE OR REPLACE TRIGGER UPDATE_UZIVATEL_ON_DELETE_ZAMESTNANEC_TGR
    AFTER DELETE ON ST67028.ZAMESTNANCI
    FOR EACH ROW
DECLARE
    v_id_typ_zakaznika NUMBER;
BEGIN
    
    SELECT ID_TYP_UZIVATELE
    INTO v_id_typ_zakaznika
    FROM ST67028.TYPY_UZIVATELE
    WHERE NAZEV = 'Zákazník';

    UPDATE ST67028.UZIVATELE
    SET ID_TYP_UZIVATELE = v_id_typ_zakaznika
    WHERE ID_UZIVATELE = :OLD.ID_UZIVATELE;

EXCEPTION
    WHEN NO_DATA_FOUND THEN
        RAISE_APPLICATION_ERROR(-20001, 'Typ uživatele Zákazník nebyl nalezen.');
    WHEN OTHERS THEN
        RAISE_APPLICATION_ERROR(-20002, 'Chyba při aktualizaci uživatele na typ Zákazník.');
END;
/


CREATE OR REPLACE TRIGGER DELETE_OLD_PHOTO_TRG
    BEFORE INSERT
    ON FOTO
    FOR EACH ROW
DECLARE
    v_old_id_foto NUMBER;
BEGIN
    -- Pokud je fotka spojena s uživatelem
    IF :NEW.ID_UZIVATELE IS NOT NULL THEN
        BEGIN
            SELECT ID_FOTO
            INTO v_old_id_foto
            FROM ST67028.FOTO
            WHERE ID_UZIVATELE = :NEW.ID_UZIVATELE
              AND ID_KARTY IS NULL
              FETCH FIRST 1 ROW ONLY;

            DELETE FROM ST67028.FOTO
            WHERE ID_FOTO = v_old_id_foto;
        EXCEPTION
            WHEN NO_DATA_FOUND THEN
                NULL; -- Pokud záznam neexistuje, nic se nestane
        END;

        -- Pokud je fotka spojena s kartou MHD
    ELSIF :NEW.ID_KARTY IS NOT NULL THEN
        BEGIN
            SELECT ID_FOTO
            INTO v_old_id_foto
            FROM ST67028.FOTO
            WHERE ID_KARTY = :NEW.ID_KARTY
              AND ID_UZIVATELE IS NULL
              FETCH FIRST 1 ROW ONLY;

            DELETE FROM ST67028.FOTO
            WHERE ID_FOTO = v_old_id_foto;
        EXCEPTION
            WHEN NO_DATA_FOUND THEN
                NULL; -- To stejné tady
        END;
    END IF;
END;
/
