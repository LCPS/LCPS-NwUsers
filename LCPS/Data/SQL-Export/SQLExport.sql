select 'bcp ' + SCHEMA_NAME(schema_id) + '.' + st.name + ' out C:\Lcps\NwUsers\LCPS-NwUsers\LCPS\Data\SQL-Export\' +SCHEMA_NAME(schema_id) + '.' + st.name + '.txt -t \t -c -S .\sqlexpress -U sa -P Sql-pw1 -d ' + DB_NAME() from sys.tables st