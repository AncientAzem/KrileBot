FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Discord/Discord.csproj", "Discord/"]
RUN dotnet restore "Discord/Discord.csproj"
COPY . Discord/
WORKDIR /src/Discord
RUN dotnet publish "Discord.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Discord.csproj" -c "${CONFIGURATION}" -o /app/publish

FROM base AS final
WORKDIR /app

LABEL org.opencontainers.image.source="https://github.com/AncientAzem/KrileBot"
LABEL org.opencontainers.image.authors="AncientAzem"

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Discord.dll"]