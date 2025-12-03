# Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
# ASP.NET Core in containers defaults to port 8080
EXPOSE 8080

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project files (Dockerfile is inside Feed.API → so use ..)
COPY ["Feed.API/Feed.API.csproj", "Feed.API/"]
COPY ["Feed.Application/Feed.Application.csproj", "Feed.Application/"]
COPY ["Feed.Domain/Feed.Domain.csproj", "Feed.Domain/"]

RUN dotnet restore "Feed.API/Feed.API.csproj"

# Copy the rest of the code
COPY . .

WORKDIR "/src/Feed.API"
RUN dotnet build "Feed.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Feed.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Feed.API.dll"]
