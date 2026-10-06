# HRMS - macOS Development Setup

This repository contains the backend and frontend source code for the Enterprise HRMS system, configured and tested for macOS Apple Silicon (arm64).

---

## Prerequisites (Configured)

1. **.NET 10 SDK**: Installed at `~/.dotnet` (configured in `~/.zshrc` and `~/.zprofile`).
2. **Node.js (v22 LTS) & npm (v10)**: Installed at `~/.nodejs` (configured in `~/.zshrc` and `~/.zprofile`).
3. **Microsoft SqlPackage Tool**: Installed at `~/.dotnet/tools/sqlpackage`.
4. **Docker SQL Server**: Container `sql_server` running on `localhost:1433` (Password: `Root@123#`).

---

## Database Status

- **Host**: `127.0.0.1:1433`
- **User**: `sa`
- **Password**: `Root@123#`
- **Databases**: `HRMS` and `HRMS_DB` (both initialized with 185 tables and 355 stored procedures/functions).
- **Schema Deployment Command** (to re-publish after SQL project updates):
  ```bash
  dotnet build API/HRMS.DatabaseCore/HRMS.DatabaseCore.sqlproj
  sqlpackage /Action:Publish /SourceFile:"API/HRMS.DatabaseCore/bin/Debug/HRMS.DatabaseCore.dacpac" /TargetConnectionString:"Server=127.0.0.1,1433;Database=HRMS;User Id=sa;Password=Root@123#;TrustServerCertificate=True;"
  ```

---

## Running the Application

### 1. Run the Backend API (.NET 10)
```bash
cd "API/HRMS.API"
dotnet run --launch-profile http
```
- API Base URL: `http://localhost:5143`
- OpenAPI Specification: `http://localhost:5143/openapi/v1.json`
- SignalR Notification Hub: `http://localhost:5143/hubs/notifications`

### 2. Run the Frontend (Angular 21)
```bash
cd "UI/hrms-client"
npm start
```
- Web Application: `http://localhost:4200`
- On initial launch with an empty database, the app automatically detects fresh installation and routes to `/auth/setup-admin` for first-time tenant and administrator provisioning.

---

## Running Tests

- **Unit Tests**:
  ```bash
  dotnet test API/HRMS.UnitTests/HRMS.UnitTests.csproj
  ```
- **Integration Tests**:
  ```bash
  dotnet test API/HRMS.IntegrationTests/HRMS.IntegrationTests.csproj
  ```
- **Angular Client Production Build**:
  ```bash
  cd UI/hrms-client && npm run build
  ```
