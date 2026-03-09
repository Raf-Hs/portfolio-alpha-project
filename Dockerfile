# Etapa 1: Build (SDK completo para compilar)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiamos solo los archivos de proyecto primero para aprovechar el caché de Docker
COPY ["AlphaProject.Api/AlphaProject.Api.csproj", "AlphaProject.Api/"]
COPY ["AlphaProject.Application/AlphaProject.Application.csproj", "AlphaProject.Application/"]
COPY ["AlphaProject.Domain/AlphaProject.Domain.csproj", "AlphaProject.Domain/"]
COPY ["AlphaProject.Infrastructure/AlphaProject.Infrastructure.csproj", "AlphaProject.Infrastructure/"]

# Restauramos dependencias
RUN dotnet restore "AlphaProject.Api/AlphaProject.Api.csproj"

# Copiamos el resto del código y publicamos
COPY . .
WORKDIR "/src/AlphaProject.Api"
RUN dotnet publish "AlphaProject.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa 2: Runtime (Imagen ultra ligera solo para ejecutar)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
# Render expone los servicios web en el puerto 8080 por defecto
EXPOSE 8080 
ENV ASPNETCORE_URLS=http://+:8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "AlphaProject.Api.dll"]