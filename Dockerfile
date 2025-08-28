# Use the official .NET 6.0 runtime as base image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Use the SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["src/services/Integration.Api/Integration.Api.csproj", "src/services/Integration.Api/"]
COPY ["src/building blocks/Integration.Service/Integration.Service.csproj", "src/building blocks/Integration.Service/"]
COPY ["src/building blocks/Integration.Infrastructure/Integration.Infrastructure.csproj", "src/building blocks/Integration.Infrastructure/"]
COPY ["src/building blocks/Integration.Domain/Integration.Domain.csproj", "src/building blocks/Integration.Domain/"]

# Restore dependencies
RUN dotnet restore "src/services/Integration.Api/Integration.Api.csproj"

# Copy all source code
COPY . .

# Build the application
WORKDIR "/src/src/services/Integration.Api"
RUN dotnet build "Integration.Api.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "Integration.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage/image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Set environment variables
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:80
ENV PORT=80

ENTRYPOINT ["dotnet", "Integration.Api.dll"]
