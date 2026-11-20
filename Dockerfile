FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copiamos toda la solucion y restauramos dependencias
COPY . .
RUN dotnet restore Lab13.API/Lab13.API.csproj
RUN dotnet publish Lab13.API/Lab13.API.csproj -c Release -o /app/publish -p:UseAppHost=false

# Imagen ligera de ejecucion
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Render expone la aplicacion en el puerto 8080 por defecto
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "Lab13.API.dll"]
