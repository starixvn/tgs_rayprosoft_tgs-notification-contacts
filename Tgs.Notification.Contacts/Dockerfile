FROM hub.lumigame.com/base/dotnet/core/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["Tgs.Notification.Contacts/Tgs.Notification.Contacts.csproj", "Tgs.Notification.Contacts/"]
COPY ["Tgs.Notification.Contacts.Common/Tgs.Notification.Contacts.Common.csproj", "Tgs.Notification.Contacts.Common/"]
COPY ["Tgs.Notification.Contacts.Core/Tgs.Notification.Contacts.Core.csproj", "Tgs.Notification.Contacts.Core/"]
COPY ["Tgs.Notification.Contacts.Repositories/Tgs.Notification.Contacts.Repositories.csproj", "Tgs.Notification.Contacts.Repositories/"]
RUN dotnet restore "./Tgs.Notification.Contacts/./Tgs.Notification.Contacts.csproj"
COPY . .
WORKDIR "/src/Tgs.Notification.Contacts"
RUN dotnet build "./Tgs.Notification.Contacts.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Tgs.Notification.Contacts.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM hub.lumigame.com/base/dotnet/core/aspnet:8.0-alpine-24h1.1 AS base
USER app
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tgs.Notification.Contacts.dll"]