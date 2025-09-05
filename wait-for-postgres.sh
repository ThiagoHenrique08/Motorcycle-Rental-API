#!/bin/sh

echo "Esperando o Postgres iniciar..."
until pg_isready -h postgres -p 5432 -U admin; do
  sleep 1
done

echo "Postgres iniciado, aplicando migrations..."
exec "$@"
