# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY . .

RUN dotnet restore "External.MyInventoryApi.Worker/External.MyInventoryApi.Worker.csproj"

RUN dotnet publish "External.MyInventoryApi.Worker/External.MyInventoryApi.Worker.csproj" -c Release -o /app/publish /p:UseAppHost=false --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS final

WORKDIR /app

RUN addgroup --system appgroup && adduser --system --ingroup appgroup appuser

COPY --from=build /app/publish .

RUN chown -R appuser:appgroup /app

USER appuser

ENV DOTNET_RUNNING_IN_CONTAINER=true

ENTRYPOINT ["dotnet", "External.MyInventoryApi.Worker.dll"]