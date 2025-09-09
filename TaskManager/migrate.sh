#!/bin/sh
set -e

echo "===> Installing dotnet-ef..."
dotnet tool install --global dotnet-ef
export PATH="$PATH:/root/.dotnet/tools"

echo "===> Applying EF Core migrations..."
dotnet ef database update \
  --project src/TaskManager.Infrastructure/TaskManager.Infrastructure.csproj \
  --startup-project src/TaskManager.Api/TaskManager.Api.csproj

echo "===> Seeding test data..."
# Подключаемся к Postgres и вставляем тестовые задачи
PGPASSWORD=postgres psql -h db -U postgres -d task_manager <<EOF
INSERT INTO "Tasks" ("Id", "Title", "Description", "Status", "CreatedAtUtc")
VALUES
  (gen_random_uuid(), 'First test task', 'Discription for first task', 0, NOW()),
  (gen_random_uuid(), 'Second test task', 'Discription for second task', 0, NOW()),
  (gen_random_uuid(), 'Third test task', 'Discription for third task', 2, NOW())
ON CONFLICT DO NOTHING;
EOF

echo "===> Migration and seeding completed!"
