# Logistics & Shipment Management API System

A scalable, production-ready backend API built for a logistics platform to manage shipment lifecycles, real-time tracking history, warehouse operations, and delivery confirmations. 

This project was built during the **Teyzix Core Internship (June Batch)**.

## 🏗️ Architectural Pattern: 7-Layer Onion Architecture
The solution is strictly structured using the **Onion Architecture** pattern to achieve maximum separation of concerns, maintainability, and testability:
1. **Domain**: Contains core entities, enums, and rules (Shipment, Warehouse, ApplicationUser).
2. **Persistence**: EF Core implementation, `ApplicationDbContext`, Migrations, and configurations.
3. **Abstraction**: Holds interfaces and contracts for services (`IShipmentService`, `IAuthService`).
4. **Service**: Core business logic implementation (`ShipmentService`, `AuthService`).
5. **Shared**: Common DTOs (`RegisterDto`, `CreateShipmentDto`) and constants shared across layers.
6. **Presentation**: Decoupled Web API Controllers (`ShipmentsController`, `AuthController`).
7. **Web**: The entry point hosting `Program.cs`, `appsettings.json`, and dependency injection setup.

## 🚀 Tech Stack
* **Framework:** .NET 9 (C# 13 utilizing primary constructors and file-scoped namespaces)
* **Database:** SQL Server via Entity Framework Core
* **Authentication:** ASP.NET Core Identity + JWT (JSON Web Tokens) with Refresh Token capabilities
* **Documentation:** Swashbuckle (Swagger UI)

## 📌 Main API Endpoints Implemented

### 🔑 Authentication Module
* `POST /api/auth/register` - Registers a new user (Default role: Customer).
* `POST /api/auth/login` - Authenticates user and issues JWT + Refresh Token.
* `POST /api/auth/refresh-token` - Renews an expired JWT using a valid Refresh Token.
* `POST /api/auth/logout` - Revokes user session and invalidates tokens.

### 📦 Shipment Management
* `POST /api/shipments` - Book a new shipment (Customers).
* `GET /api/shipments/my-shipments` - View logged-in customer's shipments.
* `GET /api/shipments/{id}` - Fetch shipment details by ID.
* `GET /api/shipments/track/{trackingId}` - Anonymous endpoint to track shipment journey by Guid.
* `GET /api/shipments/assigned` - View shipments assigned to a specific Delivery Agent.
* `PUT /api/shipments/{id}/status` - Update status (Created ➔ Picked Up ➔ In Warehouse ➔ Out For Delivery ➔ Delivered).
* `POST /api/shipments/{id}/proof` - Upload/Submit delivery confirmation proof image.

### 🏢 Warehouse & Admin Operations
* `GET /api/admin/shipments` - View all shipments globally (Admin only).
* `PUT /api/admin/shipments/{id}/assign-agent` - Dispatch an agent to a shipment.
* `GET /api/warehouses` - Manage and monitor warehouse capacity and utilization.

### 📊 Reports & Notifications
* `GET /api/reports/daily-shipments` - Core performance reports for daily operations.
* `GET /api/reports/warehouse-capacity` - Real-time statistics on warehouse space availability.
* `GET /api/notifications` - Retrieve status change alert triggers for users.

## ⚙️ How to Run Locally

1. **Clone the repository:**
   ```bash
   git clone <YOUR_GITHUB_REPOSITORY_LINK>
   cd LogisticsSystem
