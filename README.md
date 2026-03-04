# Event Booking System

A comprehensive event booking and ticketing platform built with .NET 10 and Clean Architecture principles. The system enables users to register, manage their wallets, browse events, and book tickets seamlessly.

## 🏗️ Architecture

The project follows **Clean Architecture** with clear separation of concerns across four layers:

```
EventBookingSystem/
├── EventBookingSystem.Domain/          # Core business entities and enums
├── EventBookingSystem.Application/     # Business logic, DTOs, and service interfaces
├── EventBookingSystem.Infrastructure/  # Data access, repositories, and external integrations
└── EventBookingSystem.API/            # Web API controllers and entry point
```

## 🚀 Features

- **User Management**
  - Customer registration with email/password authentication
  - JWT-based authentication with refresh tokens
  - Role-based authorization (Customer & Admin)

- **Event Management**
  - Create, view, and manage events (Admin)
  - Browse available events
  - Real-time ticket availability tracking

- **Booking System**
  - Book event tickets with quantity selection
  - Atomic transaction processing
  - Booking history tracking

- **Wallet System**
  - Individual user wallets with balance management
  - Top-up via payment gateway integration
  - Transaction history with idempotent design
  - Automatic debit on successful bookings

## 🛠️ Tech Stack

- **.NET 10** with C# 14.0
- **Entity Framework Core** for database operations
- **MySQL** database
- **JWT** for authentication
- **Swagger/OpenAPI** for API documentation
- **CORS** enabled for cross-origin requests

## 📋 Prerequisites

Before running the application, ensure you have the following installed:

1. **.NET 10 SDK** - [Download here](https://dotnet.microsoft.com/download/dotnet/10.0)
2. **MySQL Server** (v8.0 or higher) - [Download here](https://dev.mysql.com/downloads/mysql/)
3. **Visual Studio 2022** (v17.12+) or **Visual Studio Code** with C# extension
4. **Git** (optional, for cloning the repository)

## ⚙️ Local Setup Instructions

### 1. Clone the Repository (if applicable)

```bash
git clone <repository-url>
cd EventBookingSystem
```

### 2. Configure Database Connection

Update the connection string in `EventBookingSystem.API/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "server=localhost;port=3306;database=event_booking;user=YOUR_MYSQL_USER;password=YOUR_MYSQL_PASSWORD"
  }
}
```

**Important:** Replace `YOUR_MYSQL_USER` and `YOUR_MYSQL_PASSWORD` with your actual MySQL credentials.

### 3. Set Up the Database

#### Option A: Using EF Core Migrations (Recommended)

Open a terminal in the solution directory and run:

```bash
# Navigate to the Infrastructure project
cd EventBookingSystem.Infrastructure

# Apply migrations
dotnet ef database update --startup-project ../EventBookingSystem.API

# Or from the solution root:
dotnet ef database update --project EventBookingSystem.Infrastructure --startup-project EventBookingSystem.API
```

#### Option B: Using Visual Studio Package Manager Console

1. Open **Tools > NuGet Package Manager > Package Manager Console**
2. Set **Default Project** to `EventBookingSystem.Infrastructure`
3. Run:

```powershell
Update-Database
```

The database will be automatically created with the schema and seeded with initial data.

### 4. Verify Seed Data

The application automatically seeds the following test accounts:

**Admin Accounts:**
- Email: `admin@eventhub.com` | Password: `AdminSecure123!`
- Email: `support@eventhub.com` | Password: `AdminSecure123!`
- Email: `operations@eventhub.com` | Password: `AdminSecure123!`

**Customer Accounts:**
- Email: `abdul.salaudeen@gmail.com` | Password: `StrongPass123!`
- Email: `chinedu.okafor@gmail.com` | Password: `StrongPass123!`
- Email: `amina.bello@gmail.com` | Password: `StrongPass123!`
- Email: `tunde.adeyemi@gmail.com` | Password: `StrongPass123!`

### 5. Run the Application

#### Using Visual Studio:

1. Open `EventBookingSystem.sln`
2. Set `EventBookingSystem.API` as the startup project (right-click > Set as Startup Project)
3. Press `F5` or click **Run**

#### Using .NET CLI:

```bash
cd EventBookingSystem.API
dotnet run
```

The application will start and display:

```
Now listening on: https://localhost:7172
Now listening on: http://localhost:5172
```

### 6. Access Swagger UI

Once the application is running, open your browser and navigate to:

```
https://localhost:7172/swagger
```

This provides an interactive API documentation interface where you can test all endpoints.

## 📚 API Endpoints

### Authentication

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| POST | `/api/Authentication/customer/login` | Customer login | No |
| POST | `/api/Authentication/admin/login` | Admin login | No |

### Customer Management

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| POST | `/api/Customer/CreateUser` | Register new customer | No |
| GET | `/api/Customer/GetUserProfile` | Get current user profile | Yes (Customer) |

### Event Management

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| GET | `/api/Events/GetAllEvent` | Get all events | No |
| GET | `/api/Events/GetAvailableEvents` | Get available events | No |
| GET | `/api/Events/{id}` | Get event by ID | No |
| POST | `/api/Events/CreateEvent` | Create new event | Yes (Admin) |
| POST | `/api/Events/BookEvent` | Book event tickets | Yes (Customer) |

### Wallet Management

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| POST | `/api/Wallets/InitiateTopUp` | Initiate wallet top-up | Yes (Customer) |
| POST | `/api/Wallets/ConfirmTopUp/{reference}` | Confirm payment | Yes (Customer) |

## 🧪 Testing the Application

### 1. Register a New User

```bash
POST https://localhost:7172/api/Customer/CreateUser
Content-Type: application/json

{
  "email": "test@example.com",
  "password": "Test123!",
  "firstName": "Test",
  "lastName": "User"
}
```

### 2. Login as Customer

```bash
POST https://localhost:7172/api/Authentication/customer/login
Content-Type: application/json

{
  "email": "test@example.com",
  "password": "Test123!"
}
```

Copy the returned `token` for use in subsequent requests.

### 3. Create an Event (Admin)

First, login as admin, then:

```bash
POST https://localhost:7172/api/Events/CreateEvent
Content-Type: application/json
Authorization: Bearer YOUR_ADMIN_TOKEN

{
  "title": "Tech Conference 2025",
  "description": "Annual technology conference",
  "eventDate": "2025-06-15T10:00:00Z",
  "ticketPrice": 5000.00,
  "totalTickets": 500
}
```

### 4. Top Up Wallet

```bash
POST https://localhost:7172/api/Wallets/InitiateTopUp
Content-Type: application/json
Authorization: Bearer YOUR_CUSTOMER_TOKEN

{
  "amount": 10000.00,
  "idempotencyKey": "unique-key-12345"
}
```

### 5. Book a Ticket

```bash
POST https://localhost:7172/api/Events/BookEvent
Content-Type: application/json
Authorization: Bearer YOUR_CUSTOMER_TOKEN

{
  "eventId": "EVENT_ID",
  "quantity": 2
}
```

## 🔐 Authentication

The API uses **JWT Bearer Token** authentication. After logging in, include the token in the Authorization header:

```
Authorization: Bearer YOUR_JWT_TOKEN
```

In Swagger UI:
1. Click the **Authorize** button (top right)
2. Enter: `Bearer YOUR_JWT_TOKEN`
3. Click **Authorize**

## 📊 Database Schema

Key entities and relationships:

- **User** (1) → (1) **Wallet** → (many) **WalletTransaction**
- **User** (1) → (many) **Booking**
- **Event** (1) → (many) **Booking**
- **Admin** (separate authentication table)

All entities include audit fields (CreatedOn, ModifiedOn, etc.) and soft delete support.

## 🐛 Troubleshooting

### Database Connection Issues

- Verify MySQL is running: `sudo systemctl status mysql` (Linux) or check Services (Windows)
- Test connection using MySQL Workbench or command line
- Ensure the user has proper permissions on the database

### Migration Issues

If migrations fail, try:

```bash
# Remove the database
dotnet ef database drop --startup-project EventBookingSystem.API --project EventBookingSystem.Infrastructure

# Re-apply migrations
dotnet ef database update --startup-project EventBookingSystem.API --project EventBookingSystem.Infrastructure
```

### Port Conflicts

If ports 5172 or 7172 are already in use, modify `launchSettings.json` in the API project.

### Seed Data Issues

The application attempts to seed data automatically on startup. If this fails, check the console output for error messages.

## 📝 Configuration

### JWT Settings

JWT configuration is in `appsettings.json`. For production, use environment variables or Azure Key Vault:

```json
{
  "Jwt": {
    "Issuer": "eventManagementSyetem",
    "Audience": "eventManagementSyetem",
    "PublicKey": "...",
    "secretKey": "..."
  }
}
```

### CORS Policy

Currently set to `AllowAnyOrigin` for development. For production, restrict to specific origins in `Program.cs`.

## 🚀 Deployment

For production deployment:

1. Update `appsettings.Production.json` with production connection strings
2. Set `ASPNETCORE_ENVIRONMENT=Production`
3. Use secure JWT keys stored in environment variables
4. Configure proper CORS policy
5. Enable HTTPS enforcement
6. Use a reverse proxy (Nginx/IIS)

## 📄 Project Structure

```
EventBookingSystem/
├── EventBookingSystem.API/
│   ├── Controllers/              # API endpoints
│   ├── Program.cs               # Application entry point
│   └── appsettings.json         # Configuration
├── EventBookingSystem.Application/
│   ├── Dtos/                    # Data transfer objects
│   ├── Services/                # Business logic
│   └── Extensions/              # Service registration
├── EventBookingSystem.Domain/
│   ├── Models/                  # Core entities
│   ├── Enums/                   # Enumerations
│   └── Constant/                # Constants
└── EventBookingSystem.Infrastructure/
    ├── Data/                    # DbContext and migrations
    ├── Repositories/            # Data access layer
    └── Migrations/              # EF Core migrations
```

## 🤝 Contributing

1. Create a feature branch
2. Make your changes
3. Write/update tests
4. Submit a pull request

## ⚠️ Security Notes

- The current implementation uses SHA256 for password hashing. For production, consider migrating to **BCrypt** or **PBKDF2**.
- Never commit `appsettings.json` with real credentials to version control
- Use environment variables or secure vaults for sensitive configuration
- Implement rate limiting for API endpoints
- Add input validation and sanitization

## 📞 Support

For issues or questions:
- Check the Swagger documentation at `/swagger`
- Review the troubleshooting section
- Check application logs in the console output

## 📜 License

[Specify your license here]

---

**Built with ❤️ using .NET 10 and Clean Architecture**
