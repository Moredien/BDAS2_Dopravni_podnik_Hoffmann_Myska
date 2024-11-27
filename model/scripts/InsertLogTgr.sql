DECLARE
    v_sql   CLOB;
    v_table_name VARCHAR2(200);
    v_column_list CLOB;
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
            
            v_column_list := NULL;
            FOR col IN (
                SELECT column_name
                FROM all_tab_columns
                WHERE table_name = v_table_name
                  AND owner = 'ST67028' 
                ) LOOP
                    IF v_column_list IS NOT NULL THEN
                        v_column_list := v_column_list || ' || '', '' || ';
                    END IF;
                    v_column_list := v_column_list || '''' || col.column_name || ': '' || :NEW.' || col.column_name;
                END LOOP;

            v_sql := 'CREATE OR REPLACE TRIGGER LOG_INSERT_' || v_table_name || '_TGR ' ||
                     'AFTER INSERT ON ' || 'ST67028.' || v_table_name || ' ' ||
                     'FOR EACH ROW ' ||
                     'BEGIN ' ||
                     '   INSERT INTO ST67028.LOGY (' ||
                     '       ID_LOGU, CAS, UZIVATEL, TABULKA, OPERACE, STARA_HODNOTA, NOVA_HODNOTA ' ||
                     '   ) VALUES ( ' ||
                     '       ST67028.LOGY_ID_LOGU_SEQ.NEXTVAL, ' ||
                     '       SYSDATE, ' ||
                     '       USER, ' ||
                     '       ''' || v_table_name || ''', ' ||
                     '       ''INSERT'', ' ||
                     '       '''', ' ||
                     '       ' || v_column_list || ' ' ||
                     '   ); ' ||
                     'END;';

            EXECUTE IMMEDIATE v_sql;
        END LOOP;
END;