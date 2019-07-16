#FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
FROM 5.9.211.60:5000/dotnet:2.2-sdk AS build
COPY CassandraExample.API/*.csproj /CassandraExample.API/
WORKDIR /CassandraExample.API
RUN dotnet restore
COPY . /app/
WORKDIR /app/CassandraExample.API
RUN mv appsettings.docker.json appsettings.json
RUN dotnet publish -c Release -o /output

#FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS final
FROM 5.9.211.60:5000/dotnet:2.2-aspnetcore-runtime AS final
WORKDIR /app
COPY --from=build /output .
ENTRYPOINT ["dotnet", "CassandraExample.API.dll"]