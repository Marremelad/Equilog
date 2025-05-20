#!/bin/bash

# Start sqlserver
/opt/mssql/bin/sqlservr &

# Wait for server to start
echo "Waiting for SQL server to start..."

TOOL=/opt/mssql-tools18/bin/sqlcmd

# Wait for server to start, attempt to run basic query, check exit status
sleep 10s
while ! "$TOOL" -No -U sa -P "$MSSQL_SA_PASSWORD" -Q "SELECT 1"; do
    echo "Still waiting..."
    sleep 20s
done

# Edgecase needed maybe
sleep 20s

echo "Server started, loading init data"
$TOOL -No -U sa -P "$MSSQL_SA_PASSWORD" -i db-init.sql

if [ $? -eq 0 ];then
    echo "Successfully initialized DB"
else
    echo "DB initialization failed"
    exit 1
fi

# Think this is needed to not exit container?
sleep infinity
