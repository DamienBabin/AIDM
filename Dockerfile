# Dockerfile
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Copy csproj files and restore
COPY *.sln .
COPY DnDAdventure.API/*.csproj ./DnDAdventure.API/
COPY DnDAdventure.Core/*.csproj ./DnDAdventure.Core/
COPY DnDAdventure.Infrastructure/*.csproj ./DnDAdventure.Infrastructure/
COPY DnDAdventure.AI/*.csproj ./DnDAdventure.AI/
COPY DnDAdventure.Web/*.csproj ./DnDAdventure.Web/

# Restore packages
RUN dotnet restore

# Copy the rest of the code
COPY . .

# Build the API
RUN dotnet publish -c Release -o /app/out/api DnDAdventure.API/DnDAdventure.API.csproj

# Build the Web App
RUN dotnet publish -c Release -o /app/out/web DnDAdventure.Web/DnDAdventure.Web.csproj

# Runtime stage for API
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS api
WORKDIR /app
COPY --from=build /app/out/api .
ENTRYPOINT ["dotnet", "DnDAdventure.API.dll"]

# Runtime stage for Web
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS web
WORKDIR /app
COPY --from=build /app/out/web .
ENTRYPOINT ["dotnet", "DnDAdventure.Web.dll"]