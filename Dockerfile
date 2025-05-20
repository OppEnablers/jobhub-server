FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app
COPY . ./
COPY FireBaseServiceKey.json ./FireBaseServiceKey.json
RUN dotnet publish -c Release -o out

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/out ./
COPY --from=build /app/FireBaseServiceKey.json ./FireBaseServiceKey.json
EXPOSE 8080
ENTRYPOINT ["dotnet", "JobHubServer.dll"]