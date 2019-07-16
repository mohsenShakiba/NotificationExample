FROM dotnet:2.2-sdk AS build
COPY CassandraExample.API/*.csproj /CassandraExample.API/
WORKDIR /CassandraExample.API
RUN dotnet restore
COPY . /app/
WORKDIR /app/CassandraExample.API
RUN mv appsettings.docker.json appsettings.json
RUN dotnet publish -c Release -o /output

FROM dotnet:2.2-aspnetcore-runtime AS final
WORKDIR /app
COPY --from=build /output .
ENTRYPOINT ["dotnet", "CassandraExample.API.dll"]