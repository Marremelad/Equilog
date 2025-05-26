#!/bin/bash
set -e

echo "Väntar på att databasen ska bli tillgänglig..."

until ./migrationslnx64 -v \
  --connection "Server=prod-equilog-db;Database=EquilogDB;User=sa;Password=$DB_PASSWORD;TrustServerCertificate=True"
do
  echo "Databasen är inte redo ännu – försöker igen om 10 sekunder..."
  sleep 10
done

echo "Databas uppdaterad – startar applikationen..."
exec dotnet equilog-backend.dll