# Multi-stage Dockerfile for production deployment
# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["src/LunarCalendar.Api/LunarCalendar.Api.csproj", "src/LunarCalendar.Api/"]

# Restore dependencies
RUN dotnet restore "src/LunarCalendar.Api/LunarCalendar.Api.csproj"

# Copy all source files
COPY . .

# Build the application
WORKDIR "/src/src/LunarCalendar.Api"
RUN dotnet build "LunarCalendar.Api.csproj" -c Release -o /app/build

# Stage 2: Publish the application
FROM build AS publish
RUN dotnet publish "LunarCalendar.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 3: Create final runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

# Install curl for health checks (useful for DigitalOcean)
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

WORKDIR /app

# Copy published application
COPY --from=publish /app/publish .

# Create non-root user for security
RUN useradd -m -u 1001 appuser && chown -R appuser:appuser /app
USER appuser

# Expose port 8080 (DigitalOcean App Platform default)
EXPOSE 8080

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
  CMD curl -f http://localhost:8080/health || exit 1

# Run the application
ENTRYPOINT ["dotnet", "LunarCalendar.Api.dll"]
