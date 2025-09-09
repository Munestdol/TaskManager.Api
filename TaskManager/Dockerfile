FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY TaskManager.sln ./
COPY src/TaskManager.Api/TaskManager.Api.csproj src/TaskManager.Api/
COPY src/TaskManager.Application/TaskManager.Application.csproj src/TaskManager.Application/
COPY src/TaskManager.Domain/TaskManager.Domain.csproj src/TaskManager.Domain/
COPY src/TaskManager.Infrastructure/TaskManager.Infrastructure.csproj src/TaskManager.Infrastructure/
COPY tests/TaskManager.Tests/TaskManager.Tests.csproj tests/TaskManager.Tests/

RUN dotnet restore TaskManager.sln

COPY . .

RUN dotnet build TaskManager.sln -c Release --no-restore
RUN dotnet test tests/TaskManager.Tests/TaskManager.Tests.csproj -c Release --no-build --verbosity normal
RUN dotnet publish src/TaskManager.Api/TaskManager.Api.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "TaskManager.Api.dll"]
