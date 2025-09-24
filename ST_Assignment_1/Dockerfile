FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ST_Assignment_1/ST_Assignment_1.csproj ./ST_Assignment_1/
RUN dotnet restore "./ST_Assignment_1/ST_Assignment_1.csproj"

COPY . .
RUN dotnet publish "./ST_Assignment_1/ST_Assignment_1.csproj" -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:${PORT:-8080}
ENTRYPOINT ["dotnet", "ST_Assignment_1.dll"]

