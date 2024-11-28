INSERT INTO ST67028.ZASTAVKY (ID_ZASTAVKY, JMENO)
VALUES (1, 'Stop A');
INSERT INTO ST67028.ZASTAVKY (ID_ZASTAVKY, JMENO)
VALUES (2, 'Stop B');
INSERT INTO ST67028.ZASTAVKY (ID_ZASTAVKY, JMENO)
VALUES (3, 'Stop C');
INSERT INTO ST67028.ZASTAVKY (ID_ZASTAVKY, JMENO)
VALUES (4, 'Stop D');

INSERT INTO ST67028.LINKY (ID_LINKY, CISLO_LINKY, JMENO)
VALUES (1, 101, 'Line 1');
INSERT INTO ST67028.LINKY (ID_LINKY, CISLO_LINKY, JMENO)
VALUES (2, 102, 'Line 2');

INSERT INTO ST67028.ZASTAVENI (ID_ZASTAVENI, ODJEZD, ID_LINKY, ID_ZASTAVKY)
VALUES (1, TO_DATE('2024-11-27 08:00:00', 'YYYY-MM-DD HH24:MI:SS'), 1, 1);
INSERT INTO ST67028.ZASTAVENI (ID_ZASTAVENI, ODJEZD, ID_LINKY, ID_ZASTAVKY)
VALUES (2, TO_DATE('2024-11-27 08:10:00', 'YYYY-MM-DD HH24:MI:SS'), 1, 2);
INSERT INTO ST67028.ZASTAVENI (ID_ZASTAVENI, ODJEZD, ID_LINKY, ID_ZASTAVKY)
VALUES (3, TO_DATE('2024-11-27 08:20:00', 'YYYY-MM-DD HH24:MI:SS'), 1, 3);

INSERT INTO ST67028.ZASTAVENI (ID_ZASTAVENI, ODJEZD, ID_LINKY, ID_ZASTAVKY)
VALUES (4, TO_DATE('2024-11-27 09:00:00', 'YYYY-MM-DD HH24:MI:SS'), 2, 2);
INSERT INTO ST67028.ZASTAVENI (ID_ZASTAVENI, ODJEZD, ID_LINKY, ID_ZASTAVKY)
VALUES (5, TO_DATE('2024-11-27 09:10:00', 'YYYY-MM-DD HH24:MI:SS'), 2, 3);
INSERT INTO ST67028.ZASTAVENI (ID_ZASTAVENI, ODJEZD, ID_LINKY, ID_ZASTAVKY)
VALUES (6, TO_DATE('2024-11-27 09:20:00', 'YYYY-MM-DD HH24:MI:SS'), 2, 4);
