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
    f_user.ID_FOTO AS ID_FOTO
FROM ST67028.UZIVATELE u
         LEFT JOIN ST67028.TYPY_UZIVATELE tu ON u.ID_TYP_UZIVATELE = tu.ID_TYP_UZIVATELE
         LEFT JOIN ST67028.ADRESY a ON u.ID_ADRESY = a.ID_ADRESY
         LEFT JOIN (
    SELECT
        ID_UZIVATELE,
        ID_FOTO,
        JMENO_SOUBORU,
        DATA,
        DATUM_PRIDANI
    FROM ST67028.FOTO
    WHERE ID_UZIVATELE IS NOT NULL
) f_user ON u.ID_UZIVATELE = f_user.ID_UZIVATELE;
/


CREATE OR REPLACE VIEW ST67028.ZASTAVKY_LINKY_VIEW AS
SELECT
    l.ID_LINKY,
    l.CISLO_LINKY,
    z.ID_ZASTAVKY,
    z.JMENO AS JMENO_ZASTAVKY,
    zs.ODJEZD,
    zs.ITERACE,
    zs.SMER,
    zs.ID_ZASTAVENI
FROM
    ST67028.LINKY l
        JOIN
    ST67028.ZASTAVENI zs ON l.ID_LINKY = zs.ID_LINKY
        JOIN
    ST67028.ZASTAVKY z ON zs.ID_ZASTAVKY = z.ID_ZASTAVKY
/


CREATE OR REPLACE VIEW PLATBY_VIEW AS
SELECT
    p.ID_PLATBY,
    p.CAS_PLATBY,
    p.VYSE_PLATBY,
    p.TYP_PLATBY,
    CASE
        WHEN p.TYP_PLATBY = 0 THEN 'Platba kartou'
        WHEN p.TYP_PLATBY = 1 THEN 'Platba převodem'
        ELSE 'Neznámý typ platby'
        END AS TYP_PLATBY_TEXT,
    z.ID_ZAKAZNIKA,
    u.JMENO || ' ' || u.PRIJMENI AS JMENO_ZAKAZNIKA,
    CASE
        WHEN p.TYP_PLATBY = 0 THEN pk.CISLO_KARTY
        WHEN p.TYP_PLATBY = 1 THEN pp.CISLO_UCTU
        ELSE NULL
        END AS DETAIL_PLATBY
FROM
    ST67028.PLATBY p
        JOIN ST67028.ZAKAZNICI z ON p.ID_ZAKAZNIKA = z.ID_ZAKAZNIKA
        JOIN ST67028.UZIVATELE u ON z.ID_UZIVATELE = u.ID_UZIVATELE
        LEFT JOIN ST67028.PLATBY_KARTOU pk ON p.ID_PLATBY = pk.ID_PLATBY
        LEFT JOIN ST67028.PLATBY_PREVODEM pp ON p.ID_PLATBY = pp.ID_PLATBY
/


CREATE OR REPLACE VIEW ST67028.ZASTAVKY_LINKY_VIEW AS
SELECT
    l.ID_LINKY,
    l.CISLO_LINKY,
    z.ID_ZASTAVKY,
    z.JMENO AS JMENO_ZASTAVKY,
    zs.ODJEZD,
    zs.ITERACE,
    zs.SMER,
    zs.ID_ZASTAVENI
FROM
    ST67028.LINKY l
        JOIN
    ST67028.ZASTAVENI zs ON l.ID_LINKY = zs.ID_LINKY
        JOIN
    ST67028.ZASTAVKY z ON zs.ID_ZASTAVKY = z.ID_ZASTAVKY
/


CREATE OR REPLACE VIEW DOSTUPNA_VOZIDLA_VIEW AS
SELECT
    v.ID_VOZIDLA,
    tv.NAZEV AS TYP_VOZIDLA,
    v.ZNACKA AS ZNACKA,
    NVL(l.CISLO_LINKY, 0) AS JEZDI_NA_LINCE
FROM
    ST67028.VOZIDLA v
        JOIN ST67028.TYPY_VOZIDEL tv ON v.ID_TYP_VOZIDLA = tv.ID_TYP_VOZIDLA
        LEFT JOIN ST67028.JIZDY j ON v.ID_VOZIDLA = j.ID_VOZIDLA
        LEFT JOIN ST67028.LINKY l ON j.ID_LINKY = l.ID_LINKY
/


CREATE OR REPLACE VIEW ST67028.ZAMESTNANCI_VIEW AS
SELECT
    z.ID_ZAMESTNANCE,
    u.JMENO AS JMENO,
    u.PRIJMENI AS PRIJMENI,
    u.UZIVATELSKE_JMENO AS UZIVATELSKE_JMENO,
    z.PLAT,
    z.PLATNOST_UVAZKU_DO,
    z.ID_NADRIZENEHO,
    n.JMENO AS JMENO_NADRIZENEHO,
    n.PRIJMENI AS PRIJMENI_NADRIZENEHO,
    u.ID_UZIVATELE AS ID_UZIVATELE
FROM
    ST67028.ZAMESTNANCI z
        JOIN ST67028.UZIVATELE u ON z.ID_UZIVATELE = u.ID_UZIVATELE
        LEFT JOIN ST67028.ZAMESTNANCI zn ON z.ID_NADRIZENEHO = zn.ID_ZAMESTNANCE
        LEFT JOIN ST67028.UZIVATELE n ON zn.ID_UZIVATELE = n.ID_UZIVATELE;
/  


CREATE OR REPLACE VIEW ST67028.FOTO_VIEW AS
SELECT
    f.ID_FOTO,
    f.JMENO_SOUBORU,
    f.DATUM_PRIDANI,
    COALESCE(u1.UZIVATELSKE_JMENO, u2.UZIVATELSKE_JMENO) AS UZIVATELSKE_JMENO,
    CASE
        WHEN f.ID_UZIVATELE IS NOT NULL THEN 'Uživatel'
        WHEN f.ID_KARTY IS NOT NULL THEN 'Karta MHD'
        ELSE 'Neznámé'
        END AS TYP_ASOCIACE
FROM
    ST67028.FOTO f
        LEFT JOIN ST67028.UZIVATELE u1 ON f.ID_UZIVATELE = u1.ID_UZIVATELE 
        LEFT JOIN ST67028.KARTY_MHD k ON f.ID_KARTY = k.ID_KARTY 
        LEFT JOIN ST67028.ZAKAZNICI z ON k.ID_ZAKAZNIKA = z.ID_ZAKAZNIKA 
        LEFT JOIN ST67028.UZIVATELE u2 ON z.ID_UZIVATELE = u2.ID_UZIVATELE; 
/


CREATE OR REPLACE VIEW ST67028.JIZDY_VIEW AS
SELECT
    j.ID_JIZDY,
    l.ID_LINKY,
    v.ID_VOZIDLA,
    r.ID_RIDICE,
    j.ZACATEK,
    j.KONEC,
    l.CISLO_LINKY,
    l.JMENO AS JMENO_LINKY,
    v.ZNACKA AS ZNACKA_VOZIDLA,
    tv.NAZEV AS TYP_VOZIDLA,
    r.JMENO AS JMENO_RIDICE,
    r.PRIJMENI AS PRIJMENI_RIDICE
FROM
    ST67028.JIZDY j
        JOIN ST67028.LINKY l ON j.ID_LINKY = l.ID_LINKY
        JOIN ST67028.VOZIDLA v ON j.ID_VOZIDLA = v.ID_VOZIDLA
        JOIN ST67028.TYPY_VOZIDEL tv ON v.ID_TYP_VOZIDLA = tv.ID_TYP_VOZIDLA
        JOIN ST67028.RIDICI r ON j.ID_RIDICE = r.ID_RIDICE;
/


CREATE OR REPLACE VIEW ST67028.KARTY_MHD_VIEW AS
SELECT
    k.ID_KARTY,
    k.ZUSTATEK,
    k.PLATNOST_OD,
    k.PLATNOST_DO,
    k.ID_FOTO,
    u.UZIVATELSKE_JMENO,
    u.JMENO,
    u.PRIJMENI
FROM
    ST67028.KARTY_MHD k
        JOIN ST67028.ZAKAZNICI z ON k.ID_ZAKAZNIKA = z.ID_ZAKAZNIKA
        JOIN ST67028.UZIVATELE u ON z.ID_UZIVATELE = u.ID_UZIVATELE;
/


CREATE OR REPLACE VIEW ST67028.PREDPLATNE_VIEW AS
SELECT
    p.ID_PREDPLATNEHO,
    p.OD,
    p.DO,
    k.ID_KARTY,
    tp.JMENO AS TYP_PREDPLATNEHO,
    u.ID_UZIVATELE,
    u.UZIVATELSKE_JMENO
FROM
    ST67028.PREDPLATNE p
        JOIN ST67028.KARTY_MHD k ON p.ID_KARTY = k.ID_KARTY
        JOIN ST67028.ZAKAZNICI z ON k.ID_ZAKAZNIKA = z.ID_ZAKAZNIKA
        JOIN ST67028.UZIVATELE u ON z.ID_UZIVATELE = u.ID_UZIVATELE
        JOIN ST67028.TYPY_PREDPLATNEHO tp ON p.ID_TYP_PREDPLATNEHO = tp.ID_TYP_PREDPLATNEHO;
/
