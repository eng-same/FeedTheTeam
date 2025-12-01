# Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Feed.PL/Feed.PL.csproj", "Feed.PL/"]
RUN dotnet restore "./Feed.PL/Feed.PL.csproj"
COPY . .
WORKDIR "/src/Feed.PL"
RUN dotnet build "./Feed.PL.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Feed.PL.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage
FROM base AS final
ENV ASPNETCORE_HTTP_POSRTS=80
EXPOSE 80
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Feed.PL.dll"]
