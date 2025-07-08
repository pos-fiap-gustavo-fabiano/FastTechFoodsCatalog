FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY src/FastTechFoods.Api/*.csproj ./
RUN dotnet restore

COPY . ./
WORKDIR /app/src/FastTechFoods.Api

RUN dotnet publish -c Release -o /out

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

COPY --from=build /out .

EXPOSE 80
EXPOSE 443

ENTRYPOINT ["dotnet", "FastTechFoods.Api.dll"]