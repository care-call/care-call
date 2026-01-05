FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/CC.PractitionerService/CC.PractitionerService.csproj", "src/CC.PractitionerService/"]
RUN dotnet restore "src/CC.PractitionerService/CC.PractitionerService.csproj"
COPY . .
WORKDIR "/src/src/CC.PractitionerService"
RUN dotnet build "./CC.PractitionerService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CC.PractitionerService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CC.PractitionerService.dll"]
