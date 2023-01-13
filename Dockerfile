FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["CodingAssessment/CodingAssessment.csproj", "CodingAssessment/"]
RUN dotnet restore "CodingAssessment/CodingAssessment.csproj"
COPY . .
WORKDIR "/src/CodingAssessment"
RUN dotnet build "CodingAssessment.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CodingAssessment.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CodingAssessment.dll"]
