FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["App/Api/FairDraw.Listings.App.Api.csproj", "App/Api/"]
COPY ["App/Application/FairDraw.Listings.App.Application.csproj", "App/Application/"]
COPY ["App/Domain/FairDraw.Listings.App.Domain.csproj", "App/Domain/"]
COPY ["App/Infrastructure/FairDraw.Listings.App.Infrastructure.csproj", "App/Infrastructure/"]

# Restore packages
RUN dotnet restore "App/Api/FairDraw.Listings.App.Api.csproj"

# Copy all source code
COPY . .

# Build the application
RUN dotnet build "App/Api/FairDraw.Listings.App.Api.csproj" -c Release -o /app/build

# Publish the application
RUN dotnet publish "App/Api/FairDraw.Listings.App.Api.csproj" -c Release -o /app/publish

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Create non-root user for security
RUN adduser --disabled-password --gecos "" appuser && chown -R appuser:appuser /app
USER appuser

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Expose the port
EXPOSE 8080

# Start the application
ENTRYPOINT ["dotnet", "FairDraw.Listings.App.Api.dll"]
