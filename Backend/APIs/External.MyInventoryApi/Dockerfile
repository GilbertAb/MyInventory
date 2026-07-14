# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY . .

RUN dotnet restore "External.MyInventoryApi/External.MyInventoryApi.csproj"

RUN dotnet publish "External.MyInventoryApi/External.MyInventoryApi.csproj" -c Release -o /app/publish /p:UseAppHost=false --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

WORKDIR /app

ENV DOTNET_RUNNING_IN_CONTAINER=true

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "External.MyInventoryApi.dll"]