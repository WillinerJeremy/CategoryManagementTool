#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Client/CategoryManagementTool.Client.csproj", "Client/"]
COPY ["Shared/CategoryManagementTool.Shared.csproj", "Shared/"]
RUN dotnet restore "Client/CategoryManagementTool.Client.csproj"
COPY . .
WORKDIR "/src/Client"
RUN dotnet build "CategoryManagementTool.Client.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CategoryManagementTool.Client.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CategoryManagementTool.Client.dll"]
