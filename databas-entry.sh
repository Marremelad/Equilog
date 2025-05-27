#!/bin/bash

# Starta SQL Server
/opt/mssql/bin/sqlservr &

# Vänta på att servern ska starta
echo "Väntar på att SQL Server ska starta..."

TOOL=/opt/mssql-tools18/bin/sqlcmd

# Vänta på att servern ska starta, försök köra en grundläggande fråga, kontrollera avslutningsstatus
sleep 10s
while ! "$TOOL" -No -U sa -P "$MSSQL_SA_PASSWORD" -Q "SELECT 1"; do
    echo "Väntar fortfarande..."
    sleep 20s
done

# Edgefall som kanske behövs
sleep 20s

echo "Server startad, laddar initieringsdata"
$TOOL -No -U sa -P "$MSSQL_SA_PASSWORD" -i db-init.sql

if [ $? -eq 0 ]; then
    echo "Databas initierad framgångsrikt"
else
    echo "Databasinitiering misslyckades"
    exit 1
fi

# Tror detta behövs för att inte avsluta containern?
sleep infinity
