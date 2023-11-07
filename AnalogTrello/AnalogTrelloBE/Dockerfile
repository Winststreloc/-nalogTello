FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["AnalogTrelloBE/AnalogTrelloBE.csproj", "AnalogTrelloBE/"]
RUN dotnet restore "AnalogTrelloBE/AnalogTrelloBE.csproj"

WORKDIR "/src/AnalogTrelloBE"
COPY . .

RUN dotnet build "AnalogTrelloBE.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AnalogTrelloBE.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AnalogTrelloBE.dll"]
