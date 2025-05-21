#!/bin/bash
set -e

echo "Väntar på att databasen ska bli tillgänglig..."

until ./migrations -v; do
  echo "Databasen är inte redo ännu – försöker igen om 5 sekunder..."
  sleep 5
done

echo "Databas uppdaterad – startar applikationen..."
exec dotnet equilog-backend.dll