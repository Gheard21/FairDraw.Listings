# FairDraw.Listings

FairDraw.Listings is a .NET-based application for managing listings in a fair draw system.

## Local Development Setup

Follow these steps to set up the project locally:

### Prerequisites

-   [.NET 9.0 SDK](https://dotnet.microsoft.com/download)
-   [Docker](https://www.docker.com/products/docker-desktop)
-   [Docker Compose](https://docs.docker.com/compose/install/) (included with Docker Desktop)

### Database Setup

1. Create a `.env` file in the root directory with the following content:

```
# PostgreSQL Environment Variables
POSTGRES_USER=postgres
POSTGRES_PASSWORD=postgres
POSTGRES_DB=FairDrawListings
POSTGRES_PORT=5432
```

2. Start the PostgreSQL database container:

```bash
docker compose up -d
```

3. Verify the database is running:

```bash
docker ps
```

### Application Configuration

1. Add the database connection string as a user secret for the API project:

```bash
cd App/Api
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Port=5432;Database=FairDrawListings;User Id=postgres;Password=postgres;"
```

2. Add the same user secrets to the Infrastructure project for migrations:

```bash
cd ../Infrastructure
dotnet user-secrets init --id <same-id-as-api-project>
```

### Running Migrations

To create and apply database migrations:

```bash
cd App/Infrastructure
dotnet ef migrations add <MigrationName>
dotnet ef database update
```

### Running the Application

```bash
cd App/Api
dotnet run
```

The API will be available at `https://localhost:5001` and `http://localhost:5000` (or as configured in launchSettings.json).

## Project Structure

-   **App/Api**: Web API controllers and configuration
-   **App/Application**: Application layer with business logic
-   **App/Domain**: Domain entities and interfaces
-   **App/Infrastructure**: Data access and external services implementation
-   **Tests**: Unit and integration tests

## Development

When making changes to the database schema:

1. Update the relevant entity classes in the Domain project
2. Run migration commands from the Infrastructure project
3. Test the changes thoroughly
