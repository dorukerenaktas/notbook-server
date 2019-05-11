FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.sln .
COPY NotBook.Api/*.csproj ./NotBook.Api/
COPY NotBook.Core/*.csproj ./NotBook.Core/
COPY NotBook.Data/*.csproj ./NotBook.Data/
COPY NotBook.Service/*.csproj ./NotBook.Service/
RUN dotnet restore

# copy everything else and build app
COPY NotBook.Api/. ./NotBook.Api/
COPY NotBook.Core/. ./NotBook.Core/
COPY NotBook.Data/. ./NotBook.Data/
COPY NotBook.Service/. ./NotBook.Service/

RUN dotnet publish -c Release -o "../out"

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

# copy assets to root
COPY NotBook.Api/Assets/. ./Assets/
ENTRYPOINT ["dotnet", "NotBook.Api.dll"]