# TaskManager API

**TaskManager API** is a simple but production‑ready REST API built with **.NET 8**, following Clean Architecture principles.  
It demonstrates best practices for building maintainable, scalable, and testable backend services.

---

## 🚀 Features
- Full CRUD for tasks (`TaskItem`) with pagination and search.
- Unified API response format (`ApiResponse<T>`) including `traceId` and `timestamp`.
- Global exception handling with structured logging via **Serilog**.
- DTOs and object mapping with **Mapster**.
- Input validation using **FluentValidation**.
- EF Core with PostgreSQL (or InMemory provider for tests).
- Unit tests with **xUnit** for domain and repository layers.

---

## 🏗 Architecture
The solution is organized according to **Clean Architecture**:
src/ 
├── TaskManager.Domain # Domain model and business logic 
├── TaskManager.Application # DTOs, validation, mapping 
├── TaskManager.Infrastructure # EF Core, repositories, migrations 
├── TaskManager.Api # Controllers, middleware, configuration tests/ 
├── TaskManager.Tests # xUnit tests (domain + repository)

---

## 🛠 Tech Stack
- **.NET 8**
- **Entity Framework Core**
- **PostgreSQL** (InMemory for tests)
- **Serilog**
- **Mapster**
- **FluentValidation**
- **xUnit**

---

## ⚙️ Getting Started

### 1. Clone the repository
```bash
git clone https://github.com/Munestdol/TaskManager.Api.git
cd TaskManager
```

### 2. Configure the connection string in appsettings.json
```json
"ConnectionStrings": {
  "Default": "Host=db;Port=5432;Database=task_manager;Username=postgres;Password=postgres"
}
```

### 3. Run with docker-compose
```bash
docker-compose up --build
```

The API will be available at: http://localhost:5000/swagger
