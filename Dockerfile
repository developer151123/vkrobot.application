#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["vkrobot.application.webapi/vkrobot.application.webapi.csproj", "vkrobot.application.webapi/"]
RUN dotnet restore "vkrobot.application.webapi/vkrobot.application.webapi.csproj"
COPY . .
WORKDIR "/src/vkrobot.application.webapi"
RUN dotnet build "vkrobot.application.webapi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "vkrobot.application.webapi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN chmod +x wait-for-it.sh

ENV TZ="Europe/Berlin"

ENTRYPOINT ["dotnet", "vkrobot.application.webapi.dll"]