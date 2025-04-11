# XAUBOT - C# Backend Trading Platform

This project is a robust trading platform built with a C# backend, focusing on gold (XAU) and other financial instruments trading. The frontend UI components are excluded from version control via .gitignore settings.

## Project Architecture

This application follows a clean architecture pattern with the following components:

### Domain Layer
- **User Management**: Complete user model with role-based permissions
- **Trading Operations**: Comprehensive trading activity models with business logic
- **Data Models**: Fully typed domain entities with validation

### Data Access Layer
- **Repository Pattern**: Clean separation of data access concerns
- **In-Memory Repositories**: Rapid development with sample data
- **Extensible Design**: Easy to swap with database implementations

### Service Layer
- **Business Logic**: Encapsulated in service classes
- **Transaction Management**: Proper handling of trading operations
- **Authentication**: User credential validation and security

### API Layer
- **RESTful Controllers**: Well-designed API endpoints
- **Input Validation**: Robust request validation
- **Error Handling**: Comprehensive exception management

## API Endpoints

### User Management
- `GET /api/User` - Get all users
- `GET /api/User/{id}` - Get user by ID
- `POST /api/User` - Create new user
- `PUT /api/User/{id}` - Update user
- `DELETE /api/User/{id}` - Delete user
- `POST /api/User/authenticate` - Authenticate user
- `POST /api/User/change-password` - Change password

### Trading Operations
- `GET /api/Trading/user/{userId}` - Get user's trading activities
- `GET /api/Trading/{id}` - Get trading activity by ID
- `POST /api/Trading/open` - Open a new trading position
- `POST /api/Trading/{id}/close` - Close a trading position
- `POST /api/Trading/{id}/cancel` - Cancel a trading position
- `PUT /api/Trading/{id}/stop-loss` - Update stop loss
- `PUT /api/Trading/{id}/take-profit` - Update take profit
- `GET /api/Trading/open` - Get all open positions
- `GET /api/Trading/user/{userId}/profit-loss` - Get user's total profit/loss
- `GET /api/Trading/user/{userId}/exposure` - Get user's current exposure

## C# Project Structure

```
/csharp
├── Api                     # API Controllers
│   ├── ExampleController.cs
│   ├── UserController.cs
│   └── TradingController.cs
├── Data                    # Data Access Layer
│   ├── IDataRepository.cs
│   ├── MemoryDataRepository.cs
│   ├── IUserRepository.cs
│   ├── MemoryUserRepository.cs
│   ├── ITradingRepository.cs
│   └── MemoryTradingRepository.cs
├── Domain                  # Domain Models
│   ├── User.cs
│   ├── UserRole.cs
│   ├── UserPreference.cs
│   ├── TradingActivity.cs
│   ├── TradingSymbol.cs
│   ├── TradingPosition.cs
│   └── TradingStatus.cs
├── Services                # Business Logic
│   ├── DataService.cs
│   ├── UserService.cs
│   └── TradingService.cs
└── DependencyInjection.cs  # Service Registration
```

## Version Control Strategy

This project focuses on the C# backend implementation. To maintain this focus, we've configured the `.gitignore` file to exclude frontend-related code and assets:

- HTML, CSS, and frontend JavaScript/TypeScript files
- Frontend component directories
- Frontend asset files
- Style files

This approach ensures that the repository primarily contains the C# backend code, making it easier to review, maintain, and collaborate on the backend architecture.

## Getting Started

### Prerequisites
- .NET 7.0 or higher
- Visual Studio 2022 or Visual Studio Code

### Running the Backend

```bash
dotnet build
dotnet run
```

## Learning Resources

To learn more about the technologies used in this project:

- [ASP.NET Core Documentation](https://docs.microsoft.com/en-us/aspnet/core)
- [C# Programming Guide](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/)
- [Repository Pattern](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/infrastructure-persistence-layer-implementation-entity-framework-core)
- [Dependency Injection in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection)
