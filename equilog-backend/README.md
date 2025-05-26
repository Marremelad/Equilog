# Equilog Backend API

A comprehensive horse stable management system built with ASP.NET Core 8.0, featuring user management, stable operations, horse tracking, calendar events, and more.

## Table of Contents

- [Overview](#overview)
- [Architecture](#architecture)
- [Getting Started](#getting-started)
- [Project Structure](#project-structure)
- [Data Models](#data-models)
- [API Endpoints](#api-endpoints)
- [Authentication & Security](#authentication--security)
- [Features](#features)
- [Development](#development)
- [Testing](#testing)
- [Deployment](#deployment)

## Overview

Equilog is a horse stable management platform that enables:
- **User Management**: Registration, authentication, and profile management
- **Stable Operations**: Create and manage horse stables with role-based access
- **Horse Management**: Track horses, their details, and ownership
- **Social Features**: Stable posts, comments, and communication
- **Calendar System**: Schedule and manage stable events
- **File Storage**: Profile pictures and media management via Azure Blob Storage
- **Email Notifications**: Welcome emails and password reset functionality

## Architecture

### Technology Stack
- **Framework**: ASP.NET Core 8.0 (Minimal APIs)
- **Database**: SQL Server with Entity Framework Core
- **Authentication**: JWT Bearer tokens with refresh token support
- **Validation**: FluentValidation
- **Mapping**: AutoMapper
- **Email**: SendGrid integration
- **Storage**: Azure Blob Storage (Azurite emulator for local development)
- **Testing**: xUnit with integration and unit tests

### Design Patterns
- **Repository Pattern**: Data access through Entity Framework DbContext
- **Service Layer**: Business logic separated from controllers
- **Composition Pattern**: Complex operations broken into smaller services
- **Generic Response Pattern**: Standardized API responses with `ApiResponse<T>`
- **Filter Pattern**: Validation filters for automatic DTO validation

## Getting Started

### Prerequisites
- .NET 8.0 SDK
- SQL Server (LocalDB for development)
- Docker (for containerized deployment)
- Azurite (for local blob storage emulation)

### Local Development Setup

1. **Clone the repository**
```bash
git clone <repository-url>
cd equilog-backend
```

2. **Start Azurite Storage Emulator**
```bash
# Using Docker
docker run -p 10000:10000 -p 10001:10001 -p 10002:10002 mcr.microsoft.com/azure-storage/azurite

# Or install globally with npm
npm install -g azurite
azurite --silent --location c:\azurite --debug c:\azurite\debug.log
```

3. **Configure User Secrets Or use appsettings.Development.json file**
```bash
cd equilog-backend
dotnet user-secrets init
dotnet user-secrets set "JwtSettings:Key" "your-secret-key-here"
dotnet user-secrets set "SendGridSettings:ApiKey" "your-sendgrid-api-key"
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "your-connection-string"
dotnet user-secrets set "ConnectionStrings:LocalEquilogStorage" "DefaultEndpointsProtocol=http;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;"
```

4. **Run Database Migrations**
```bash
dotnet ef database update
```

5. **Start the Application**
```bash
dotnet run
```

The API will be available at the URLs shown in the console output (typically https://localhost:7018 or https://localhost:44337 depending on your configuration) with Swagger documentation at  `/swagger`.

### Blob Storage Configuration

The application uses **Azurite** for local development blob storage emulation:

- **Local Development**: Azurite emulator running on `http://127.0.0.1:10000`
- **Production**: Azure Blob Storage with proper connection strings
- **Container**: `equilog-media` for storing profile pictures and media files
- **CORS Configuration**: Automatically configured for frontend access

The `AzuriteStartupFilter` automatically:
- Configures CORS policies for blob access
- Creates the required container with public blob access
- Sets up proper permissions for file operations

## Project Structure

```
equilog-backend/
├── Common/                 # Shared utilities and helpers
│   ├── ApiResponse.cs     # Standardized API response wrapper
│   ├── ValidationFilter.cs # Endpoint validation filter
│   ├── Result.cs          # HTTP response generator
│   └── MappingProfile.cs  # AutoMapper configuration
├── Compositions/          # Complex operation orchestrators
│   ├── HorseCompositions.cs
│   ├── StableCompositions.cs
│   └── CommentCompositions.cs
├── Data/                  # Database context and configuration
│   └── EquilogDbContext.cs
├── DTOs/                  # Data Transfer Objects
│   ├── AuthDTOs/         # Authentication related DTOs
│   ├── HorseDTOs/        # Horse management DTOs
│   ├── StableDTOs/       # Stable management DTOs
│   ├── UserDTOs/         # User management DTOs
│   └── ...               # Feature-specific DTO folders
├── Endpoints/             # Minimal API endpoint definitions
│   ├── AuthEndpoints.cs
│   ├── HorseEndpoints.cs
│   ├── StableEndpoints.cs
│   └── ...
├── Interfaces/            # Service contracts
├── Models/               # Entity Framework models
├── Services/             # Business logic implementation
├── Validators/           # FluentValidation validators
├── Security/             # Security-related configurations
│   └── AzuriteStartupFilter.cs # Blob storage initialization
├── Startup/              # Application configuration
│   ├── AppConfiguration.cs
│   └── PipelineInitialization.cs
└── Migrations/           # Entity Framework migrations
```

## API Endpoints

### Authentication (`/api/auth`)

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| POST | `/api/auth/register` | Register new user | No |
| POST | `/api/auth/login` | User login | No |
| POST | `/api/auth/validate-password` | Validate user password | No |
| POST | `/api/auth/refresh-token` | Refresh access token | No |
| POST | `/api/auth/revoke-token` | Revoke refresh token | No |

### User Management (`/api/user`)

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| GET | `/api/user/{userId}` | Get user details | Yes           |
| GET | `/api/user/{userId}/stable/{stableId}` | Get user profile in stable context | Yes           |
| PUT | `/api/user/update` | Update user information | Yes           |
| DELETE | `/api/user/delete/{userId}` | Delete user | Yes           |
| POST | `/api/user/set-profile-picture` | Set user profile picture | Yes           |

### Horse Management (`/api/horse`)

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| GET | `/api/horse/{horseId}/profile` | Get horse profile with owners | Yes           |
| POST | `/api/horse/create` | Create new horse | Yes           |
| PUT | `/api/horse/update` | Update horse details | Yes           |
| DELETE | `/api/horse/delete/{horseId}` | Delete horse | Yes           |
| POST | `/api/horse/create/composition` | Create horse with relationships | Yes           |
| GET | `/api/horse` | Get all horses (testing) | Yes           |

### Stable Management (`/api/stable`)

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| GET | `/api/stable/{stableId}` | Get stable details | Yes           |
| GET | `/api/stable/search` | Search stables by criteria | Yes           |
| PUT | `/api/stable/update` | Update stable information | Yes           |
| DELETE | `/api/stable/delete/{stableId}` | Delete stable | Yes           |
| POST | `/api/stable/create` | Create stable with relationships | Yes           |

### Stable Posts (`/api/stable-post`)

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| GET | `/api/stable-post-by-stable-id/{stableId}` | Get posts by stable | Yes           |
| GET | `/api/stable-post/{stablePostId}` | Get specific post | Yes           |
| POST | `/api/stable-post/create` | Create new post | Yes           |
| PUT | `/api/stable-post/update` | Update post | Yes           |
| PATCH | `/api/stable-post/is-pinned/change/{stablePostId}` | Toggle pin status | Yes           |
| DELETE | `/api/stable-post/delete/{stablePostId}` | Delete post | Yes           |

### Calendar Events (`/api/calendar-events`)

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| GET | `/api/calendar-events/{stableId}` | Get events by stable | Yes           |
| GET | `/api/calendar-event/{calendarEventId}` | Get specific event | Yes           |
| POST | `/api/calendar-event/create` | Create new event | Yes           |
| PUT | `/api/calendar-event/update` | Update event | Yes           |
| DELETE | `/api/calendar-event/delete/{calendarEventId}` | Delete event | Yes           |

### Comments (`/api/comment`)

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| GET | `/api/comment/{stablePostId}` | Get comments for post | Yes           |
| POST | `/api/comment/create` | Create comment | Yes           |
| DELETE | `/api/comment/delete/{commentId}` | Delete comment | Yes           |
| POST | `/api/comment/create/composition` | Create comment with relationships | Yes           |

### Password Management (`/api`)

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| POST | `/api/reset-password` | Reset password with token | Yes           |
| POST | `/api/change-password` | Change user password | Yes           |

### Email Services (`/api/email-send`)

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| POST | `/api/email-send/welcome/` | Send welcome email | Yes           |
| POST | `/api/password-reset-email/send` | Send password reset email | Yes           |

### Blob Storage (`/api/blob-storage`)

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| GET | `/api/blob-storage/get-upload-uri` | Get SAS upload URI for blob | Yes           |
| GET | `/api/blob-storage/get-read-uri` | Get SAS read URI for blob | Yes           |
| DELETE | `/api/blob-storage/delete-blob` | Delete blob from storage | Yes           |

## Authentication & Security

### JWT Authentication
The application uses JWT (JSON Web Tokens) for authentication with the following features:
- **Access Tokens**: Short-lived tokens (configurable duration) for API access
- **Refresh Tokens**: Long-lived tokens (7 days) for getting new access tokens
- **Token Revocation**: Ability to revoke refresh tokens for security

### Security Configuration
```csharp
// JWT Configuration
"JwtSettings": {
    "Key": "your-secret-key",
    "Issuer": "equilog-api",
    "Audience": "equilog-client",
    "DurationInMinutes": 60
}
```

### Password Security
- Passwords are hashed using BCrypt with generated salts
- Password complexity requirements enforced via validation
- Password reset functionality with time-limited tokens

## Features

### User Management Flow
1. **Registration**: User registers with email/password
2. **Profile Management**: Users can update personal information and profile pictures
3. **Role-Based Access**: Different permissions based on stable roles

### Stable Management Flow
1. **Stable Creation**: Users create stables and become owners
2. **Member Invitation**: Owners can invite users or approve join requests
3. **Role Management**: Assign roles (Owner, Admin, User) to members
4. **Content Management**: Create posts, events, and manage horses

### Horse Management Flow
1. **Horse Registration**: Add horses to stables with detailed information
2. **Ownership Tracking**: Multiple users can have different roles with horses
3. **Profile Management**: Track horse details, health information, profile pictures

### File Storage Flow
1. **Upload Request**: Client requests upload URI with filename
2. **SAS Generation**: Server generates time-limited SAS URI with write permissions
3. **Direct Upload**: Client uploads file directly to Azurite/Azure Blob Storage
4. **Reference Update**: Client updates entity with blob name for future retrieval

### Composition Pattern
Complex operations use the composition pattern to orchestrate multiple services:

```csharp
// Example: Creating a horse with all relationships
public async Task<ApiResponse<Unit>> CreateHorseCompositionAsync(HorseCompositionCreateDto dto)
{
    // 1. Create horse
    var horseResponse = await horseService.CreateHorseAsync(dto.Horse);
    
    // 2. Link to stable
    var stableHorseResponse = await stableHorseService.CreateConnectionAsync(dto.StableId, horseId);
    
    // 3. Link to user (owner)
    var userHorseResponse = await userHorseService.CreateConnectionAsync(dto.UserId, horseId);
    
    // Handle rollback on failures...
}
```

## Development

### Adding New Features

1. **Create Models**: Define Entity Framework models in `/Models`
2. **Create DTOs**: Define request/response DTOs in `/DTOs`
3. **Add Validators**: Create FluentValidation validators in `/Validators`
4. **Implement Services**: Create service interfaces and implementations
5. **Add Endpoints**: Define API endpoints in `/Endpoints`
6. **Update Mappings**: Add AutoMapper configurations
7. **Write Tests**: Add unit and integration tests

### Validation Example
```csharp
public class HorseCreateDtoValidator : AbstractValidator<HorseCreateDto>
{
    public HorseCreateDtoValidator()
    {
        RuleFor(h => h.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(50).WithMessage("Name cannot exceed 50 characters.");
            
        RuleFor(h => h.Age)
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
            .When(h => h.Age.HasValue)
            .WithMessage("Age must be today or a past date.");
    }
}
```

### Database Migrations
```bash
# Add new migration
dotnet ef migrations add MigrationName

# Update database
dotnet ef database update

# Generate migration bundle for deployment
dotnet ef migrations bundle
```

### Working with Blob Storage

```csharp
// Service implementation for blob operations
public async Task<ApiResponse<Uri?>> GetUploadUriAsync(string blobName)
{
    var expiresOn = DateTimeOffset.UtcNow.Add(TimeSpan.FromMinutes(5));
    var blobClient = containerClient.GetBlobClient(blobName);
    
    var sasUri = blobClient.GenerateSasUri(
        BlobSasPermissions.Create | BlobSasPermissions.Write,
        expiresOn);
        
    return ApiResponse<Uri>.Success(HttpStatusCode.OK, sasUri, "Upload URI generated");
}
```

## Testing

### Test Structure
- **Unit Tests**: Service layer testing with mocked dependencies
- **Integration Tests**: Full API testing with in-memory database and Azurite
- **Validator Tests**: FluentValidation rule testing

### Running Tests
```bash
# Run all tests
dotnet test

# Run specific test project
dotnet test equilog-backend-test-unit
dotnet test equilog-backend-test-integration

# Run with coverage
dotnet test --collect:"XPlat Code Coverage"
```

### Test Examples
```csharp
[Fact]
public async Task CreateHorse_ValidData_ReturnsSuccess()
{
    // Arrange
    var dto = new HorseCreateDto { Name = "Thunder", Color = "Black" };
    
    // Act
    var result = await horseService.CreateHorseAsync(dto);
    
    // Assert
    Assert.True(result.IsSuccess);
    Assert.Equal(HttpStatusCode.Created, result.StatusCode);
}
```

## Deployment

### Docker Deployment
The application includes Docker configuration for containerized deployment:

```dockerfile
# Multi-stage build with EF migrations
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
# ... build and migration steps

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
# ... runtime configuration
```

### Environment Configuration
```yaml
# docker-compose.yml example
services:
  equilog-backend:
    image: equilog-backend:latest
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - ConnectionStrings__DefaultConnection=Server=db;...
      - JwtSettings__Key=${JWT_SECRET}
    depends_on:
      - database
      - azurite
      
  azurite:
    image: mcr.microsoft.com/azure-storage/azurite
    command: azurite --blobHost 0.0.0.0 --queueHost 0.0.0.0 --tableHost 0.0.0.0
    ports:
      - "10000:10000"
      - "10001:10001"
      - "10002:10002"
```

### CI/CD Pipeline
The project includes GitLab CI configuration with:
- **Build Stage**: Docker image creation for backend, database, and blob storage
- **Test Stage**: Automated testing with coverage reporting
- **Deploy Stage**: Environment deployment with Azurite integration
- **Cleanup Stage**: Resource management and environment teardown

## Error Handling

### Standardized API Responses
All endpoints return standardized responses using `ApiResponse<T>`:

```csharp
public class ApiResponse<T>
{
    public bool IsSuccess { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public T? Value { get; set; }
    public string? Message { get; set; }
}
```

### Exception Handling
- Services return `ApiResponse<T>` instead of throwing exceptions
- Global exception handling converts unhandled exceptions to proper HTTP responses
- Validation errors automatically converted to `ValidationProblem` responses
- Blob storage errors properly wrapped in `ApiResponse<T>` format

## Contributing

1. Follow the established architecture patterns
2. Add comprehensive tests for new features
3. Use FluentValidation for all input validation
4. Follow RESTful API conventions
5. Document new endpoints and features
6. Ensure proper error handling and logging
7. Test blob storage functionality with Azurite locally

## License

This project is developed for educational purposes.