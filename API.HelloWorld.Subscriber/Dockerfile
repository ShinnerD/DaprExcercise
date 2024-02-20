FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["API.HelloWorld.Subscriber/API.HelloWorld.Subscriber.csproj", "API.HelloWorld.Subscriber/"]
RUN dotnet restore "API.HelloWorld.Subscriber/API.HelloWorld.Subscriber.csproj"
COPY . .
WORKDIR "/src/API.HelloWorld.Subscriber"
RUN dotnet build "API.HelloWorld.Subscriber.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "API.HelloWorld.Subscriber.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "API.HelloWorld.Subscriber.dll"]
