FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /source

COPY *.sln .
COPY MovieAPI/*.csproj ./MovieAPI/
RUN dotnet restore  

COPY MovieAPI/. ./MovieAPI/
WORKDIR /source/MovieAPI/
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "MovieAPI.dll"]

