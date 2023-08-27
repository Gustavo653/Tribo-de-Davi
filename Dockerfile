#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["TriboDavi.API/TriboDavi.API.csproj", "TriboDavi.API/"]
COPY ["Common.DTO/Common.DTO.csproj", "Common.DTO/"]
COPY ["Common.Functions/Common.Functions.csproj", "Common.Functions/"]
COPY ["TriboDavi.DTO/TriboDavi.DTO.csproj", "TriboDavi.DTO/"]
COPY ["TriboDavi.Domain/TriboDavi.Domain.csproj", "TriboDavi.Domain/"]
COPY ["TriboDavi.Persistence/TriboDavi.Persistence.csproj", "TriboDavi.Persistence/"]
COPY ["TriboDavi.Service/TriboDavi.Service.csproj", "TriboDavi.Service/"]
COPY ["TriboDavi.DataAccess/TriboDavi.DataAccess.csproj", "TriboDavi.DataAccess/"]
COPY ["Common.DataAccess/Common.DataAccess.csproj", "Common.DataAccess/"]
COPY ["Common.Infrastructure/Common.Infrastructure.csproj", "Common.Infrastructure/"]
RUN dotnet restore "TriboDavi.API/TriboDavi.API.csproj"
COPY . .
WORKDIR "/src/TriboDavi.API"
RUN dotnet build "TriboDavi.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TriboDavi.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TriboDavi.API.dll"]