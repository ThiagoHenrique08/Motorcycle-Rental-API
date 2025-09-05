# Fase base (runtime)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Fase de build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Motorcycle-Rental-API/Motorcycle-Rental-API.csproj", "Motorcycle-Rental-API/"]
COPY ["Motorcycle-Rental-Application/Motorcycle-Rental-Application.csproj", "Motorcycle-Rental-Application/"]
COPY ["Motorcycle-Rental-Domain/Motorcycle-Rental-Domain.csproj", "Motorcycle-Rental-Domain/"]
COPY ["Motorcycle-Rental-Infrastructure/Motorcycle-Rental-Infrastructure.csproj", "Motorcycle-Rental-Infrastructure/"]
COPY ["Motorcycle-Rental-Tests/Motorcycle-Rental-Tests.csproj", "Motorcycle-Rental-Tests/"]

# Restore packages
RUN dotnet restore "Motorcycle-Rental-API/Motorcycle-Rental-API.csproj" --no-cache


COPY . .
WORKDIR "/src/Motorcycle-Rental-API"
RUN dotnet build "Motorcycle-Rental-API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Fase publish
FROM build AS publish
RUN dotnet publish "Motorcycle-Rental-API.csproj" -c Release -o /app

# Fase final
FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Motorcycle-Rental-API.dll"]

