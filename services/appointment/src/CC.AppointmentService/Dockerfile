FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/CC.AppointmentService/CC.AppointmentService.csproj", "src/CC.AppointmentService/"]
RUN dotnet restore "src/CC.AppointmentService/CC.AppointmentService.csproj"
COPY . .
WORKDIR "/src/src/CC.AppointmentService"
RUN dotnet build "./CC.AppointmentService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CC.AppointmentService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CC.AppointmentService.dll"]
