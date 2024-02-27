FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine3.19 as prepare
WORKDIR /app
RUN adduser -u 1000 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine3.19 AS aspnetbuild
WORKDIR /src
COPY HostingAzureOpenAiSample.WebApp/HostingAzureOpenAiSample.WebApp.csproj ./HostingAzureOpenAiSample.WebApp.csproj
RUN dotnet restore
COPY HostingAzureOpenAiSample.WebApp .
RUN dotnet publish "HostingAzureOpenAiSample.WebApp.csproj" \
    -c Release \
    -o /app \
    --no-restore

FROM node:20-alpine3.18 AS angularbuild
WORKDIR /src
COPY hosting-azure-open-ai-frontend/package*.json ./
RUN npm install -g @angular/cli && npm ci
COPY hosting-azure-open-ai-frontend .
RUN ng build --configuration production

FROM prepare AS final
COPY --chown=appuser --from=aspnetbuild /app .
COPY --chown=appuser --from=angularbuild /src/dist/hosting-azure-open-ai-frontend/browser /app/bundled-angular-client
ENTRYPOINT ["dotnet", "HostingAzureOpenAiSample.WebApp.dll"]
