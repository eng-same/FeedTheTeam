# Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Feed.PL/Feed.API.csproj", "Feed.API/"]
COPY ["Feed.BLL/Feed.Aplication.csproj", "Feed.Aplication/"]
COPY ["Feed.DAL/Feed.Domain.csproj", "Feed.Domain/"]
RUN dotnet restore "./Feed.API/Feed.API.csproj"
COPY . .
WORKDIR "/src/Feed.PL"
RUN dotnet build "./Feed.PL.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Feed.PL.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Feed.PL.dll"]
