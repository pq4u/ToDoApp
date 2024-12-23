FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ToDoApp.API/ToDoApp.API.csproj", "ToDoApp.API/"]
COPY ["ToDoApp.Common/ToDoApp.Common.csproj", "ToDoApp.Common/"]
COPY ["ToDoApp.DB/ToDoApp.DB.csproj", "ToDoApp.DB/"]
RUN dotnet restore "./ToDoApp.API/ToDoApp.API.csproj"
COPY . .
WORKDIR "/src/ToDoApp.API"
RUN dotnet build "./ToDoApp.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=ReleaseS
RUN dotnet publish "./ToDoApp.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ToDoApp.API.dll"]