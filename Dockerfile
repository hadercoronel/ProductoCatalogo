# ---- Build ----
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar solución y proyectos para aprovechar caché de restore
COPY ProductoCatalogo.sln ./
COPY ProductoCatalogo/ProductoCatalogo.csproj ProductoCatalogo/
COPY Aplicacion/Aplicacion.csproj Aplicacion/
COPY Domain/Domain.csproj Domain/
COPY Infraestructura/Infraestructura.csproj Infraestructura/
RUN dotnet restore ProductoCatalogo.sln

# Copiar el resto y publicar
COPY . .
RUN dotnet publish ProductoCatalogo/ProductoCatalogo.csproj -c Release -o /app/publish --no-restore /p:UseAppHost=false

# ---- Runtime ----
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Railway inyecta $PORT en runtime. Por defecto 8080 para local/docker.
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "ProductoCatalogo.dll"]
