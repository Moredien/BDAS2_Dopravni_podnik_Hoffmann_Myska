DECLARE
    v_sql   CLOB;
    v_table_name VARCHAR2(200);
    v_column_list_old CLOB;
    v_column_list_new CLOB;
BEGIN
    FOR tbl IN (
        SELECT table_name
        FROM all_tables
        WHERE owner = 'ST67028'
        ) LOOP
            v_table_name := tbl.table_name;

            IF v_table_name = 'LOGY' THEN
                CONTINUE;
            END IF;

            v_column_list_old := NULL;
            v_column_list_new := NULL;

            FOR col IN (
                SELECT column_name
                FROM all_tab_columns
                WHERE table_name = v_table_name
                  AND owner = 'ST67028'
                ) LOOP
                    IF v_column_list_old IS NOT NULL THEN
                        v_column_list_old := v_column_list_old || ' || '', '' || ';
                        v_column_list_new := v_column_list_new || ' || '', '' || ';
                    END IF;

                    v_column_list_old := v_column_list_old || '''' || col.column_name || ': '' || :OLD.' || col.column_name;
                    v_column_list_new := v_column_list_new || '''' || col.column_name || ': '' || :NEW.' || col.column_name;
                END LOOP;

            v_sql := 'CREATE OR REPLACE TRIGGER LOG_UPDATE_' || v_table_name || '_TGR ' ||
                     'AFTER UPDATE ON ' || 'ST67028.' || v_table_name || ' ' ||
                     'FOR EACH ROW ' ||
                     'BEGIN ' ||
                     '   INSERT INTO ST67028.LOGY (' ||
                     '       ID_LOGU, CAS, UZIVATEL, TABULKA, OPERACE, STARA_HODNOTA, NOVA_HODNOTA ' ||
                     '   ) VALUES ( ' ||
                     '       ST67028.LOGY_ID_LOGU_SEQ.NEXTVAL, ' ||
                     '       SYSDATE, ' ||
                     '       USER, ' ||
                     '       ''' || v_table_name || ''', ' ||
                     '       ''UPDATE'', ' ||
                     '       ' || v_column_list_old || ', ' ||
                     '       ' || v_column_list_new || ' ' ||
                     '   ); ' ||
                     'END;';

            EXECUTE IMMEDIATE v_sql;
        END LOOP;
END;
/
