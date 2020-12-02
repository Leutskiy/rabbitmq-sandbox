#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["Demo.MainApp/Demo.MainApp.csproj", "Demo.MainApp/"]
RUN dotnet restore "Demo.MainApp/Demo.MainApp.csproj"
COPY . .
WORKDIR "/src/Demo.MainApp"
RUN dotnet build "Demo.MainApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Demo.MainApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Demo.MainApp.dll"]