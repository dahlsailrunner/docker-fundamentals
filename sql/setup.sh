# Wait for SQL Server to be started and then run the sql script
./wait-for-it.sh globosql:1433 --timeout=0 --strict -- sleep 5s && \
/opt/mssql-tools/bin/sqlcmd -S localhost -i InitializeGlobomanticsDb.sql -U sa -P "$SA_PASSWORD"