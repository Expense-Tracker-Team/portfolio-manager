FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["src/microservices/users/src/Api/Api.csproj", "src/microservices/users/src/Api/"]
COPY ["src/microservices/users/src/Application/Application.csproj", "src/microservices/users/src/Application/"]
COPY ["src/microservices/users/src/Domain/Domain.csproj", "src/microservices/users/src/Domain/"]
COPY ["src/microservices/users/src/Infrastructure/Infrastructure.csproj", "src/microservices/users/src/Infrastructure/"]
COPY ["src/microservices/users/src/Persistence/Persistence.csproj", "src/microservices/users/src/Persistence/"]
RUN dotnet restore "src/microservices/users/src/Api/Api.csproj"
COPY . .
WORKDIR /src/src/microservices/users/src/Api
RUN dotnet build "Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Api.dll"]