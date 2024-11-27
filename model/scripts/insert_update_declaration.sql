CREATE OR REPLACE PACKAGE INSERT_UPDATE AS
PROCEDURE edit_adresy(
    p_id_adresy NUMBER, 
    p_mesto VARCHAR2, 
    p_ulice VARCHAR2, 
    p_cislo_popisne VARCHAR2
    ); 
PROCEDURE edit_foto(
    p_id_foto       NUMBER, 
    p_jmeno_souboru VARCHAR2, 
    p_data          BLOB, 
    p_datum_pridani DATE, 
    p_id_karty      NUMBER, 
    p_id_uzivatele  NUMBER
);
PROCEDURE edit_jizdy(
    p_id_jizdy NUMBER,
    p_zacatek DATE,
    p_konec DATE,
    p_id_vozidla NUMBER,
    p_id_linky NUMBER
);
PROCEDURE edit_karty_mhd(
    p_id_karty NUMBER,
    p_zustatek NUMBER,
    p_platnost_od DATE,
    p_platnost_do DATE,
    p_id_zakaznika NUMBER,
    p_id_foto NUMBER
);
PROCEDURE edit_linky(
    p_id_linky NUMBER,
    p_cislo_linky NUMBER,
    p_jmeno VARCHAR2
);
PROCEDURE edit_logy(
    p_id_logu NUMBER,
    p_cas DATE,
    p_uzivatel VARCHAR2,
    p_tabulka VARCHAR2,
    p_operace VARCHAR2,
    p_stara_hodnota VARCHAR2,
    p_nova_hodnota VARCHAR2
);
PROCEDURE edit_platby(
    p_id_platby NUMBER,
    p_cas_platby DATE,
    p_vyse_platby NUMBER,
    p_id_zakaznika NUMBER,
    p_typ_platby NUMBER
);
PROCEDURE edit_platby_kartou(
    p_id_platby NUMBER,
    p_cislo_karty VARCHAR2,
    p_jmeno_majitele VARCHAR2
);
PROCEDURE edit_platby_prevodem(
    p_id_platby NUMBER,
    p_cislo_uctu VARCHAR2
);
PROCEDURE edit_predplatne(
    p_id_predplatneho NUMBER,
    p_od DATE,
    p_do DATE,
    p_id_karty NUMBER,
    p_id_typ_predplatneho NUMBER
);
PROCEDURE edit_ridici(
    p_id_ridice NUMBER,
    p_jmeno VARCHAR2,
    p_prijmeni VARCHAR2
);
PROCEDURE edit_typy_predplatneho(
    p_id_typ_predplatneho NUMBER,
    p_jmeno VARCHAR2,
    p_cena NUMBER
);
PROCEDURE edit_typy_uzivatele(
    p_id_typ_uzivatele NUMBER,
    p_nazev VARCHAR2
);
PROCEDURE edit_typy_vozidel(
    p_id_typ_vozidla NUMBER,
    p_nazev VARCHAR2,
    p_znacka VARCHAR2
);
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
);
PROCEDURE edit_vozidla(
    p_id_vozidla NUMBER,
    p_id_typ_vozidla NUMBER
);
PROCEDURE edit_zakaznici(
    p_id_zakaznika NUMBER,
    p_id_uzivatele NUMBER
);
PROCEDURE edit_zamestnanci(
    p_id_zamestnance NUMBER,
    p_plat NUMBER,
    p_platnost_uvazku_do DATE,
    p_id_nadrizeneho NUMBER,
    p_id_uzivatele NUMBER
);
PROCEDURE edit_zastaveni(
    p_id_zastaveni NUMBER,
    p_odjezd DATE,
    p_id_linky NUMBER,
    p_id_zastavky NUMBER
);
PROCEDURE edit_zastavky(
    p_id_zastavky NUMBER,
    p_jmeno VARCHAR2
);
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
);

END INSERT_UPDATE;
/
commit;