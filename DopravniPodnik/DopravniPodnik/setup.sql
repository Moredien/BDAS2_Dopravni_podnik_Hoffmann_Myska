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