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
INSERT INTO "Tasks" ("Id", "Title", "Description", "Status", "CreatedAt")
VALUES
  (gen_random_uuid(), 'Первая тестовая задача', 'Описание первой задачи', 'New', NOW()),
  (gen_random_uuid(), 'Вторая тестовая задача', 'Описание второй задачи', 'InProgress', NOW()),
  (gen_random_uuid(), 'Третья тестовая задача', 'Описание третьей задачи', 'Done', NOW())
ON CONFLICT DO NOTHING;
EOF

echo "===> Migration and seeding completed!"
