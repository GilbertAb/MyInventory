#!/bin/bash

set -e

SERVER="sqlserver"
USER="sa"
PASSWORD="$MSSQL_SA_PASSWORD"

echo "Waiting for SQL Server..."

until /opt/mssql-tools/bin/sqlcmd \
    -S "$SERVER" \
    -U "$USER" \
    -P "$PASSWORD" \
    -C \
    -Q "SELECT 1" >/dev/null 2>&1
do
    sleep 5
done

echo "SQL Server is ready."

DATABASE_EXISTS=$(
/opt/mssql-tools/bin/sqlcmd \
    -S "$SERVER" \
    -U "$USER" \
    -P "$PASSWORD" \
    -C \
    -h -1 \
    -Q "SET NOCOUNT ON; SELECT COUNT(*) FROM sys.databases WHERE name = 'MyInventoryDb';" \
| tr -d '[:space:]'
)

if [ "$DATABASE_EXISTS" = "1" ]; then
    echo "Database already exists. Skipping initialization."
    exit 0
fi

echo "Initializing database..."

execute_scripts() {
    local folder=$1

    if [ -d "$folder" ]; then
        for file in $(find "$folder" -type f -name "*.sql" | sort); do
            echo "Executing $file"

            /opt/mssql-tools/bin/sqlcmd \
                -S "$SERVER" \
                -U "$USER" \
                -P "$PASSWORD" \
                -C \
                -b \
                -i "$file"
        done
    fi
}

execute_scripts "/scripts/0_database"
execute_scripts "/scripts/1_schemas"
execute_scripts "/scripts/2_tables"
execute_scripts "/scripts/3_functions"
execute_scripts "/scripts/4_procedures"
execute_scripts "/scripts/5_triggers"
execute_scripts "/scripts/6_seeds"

echo "Database initialization completed successfully."