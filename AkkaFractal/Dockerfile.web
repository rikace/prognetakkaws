FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ./AkkaFractal.Web/AkkaFractal.Web.csproj ./src/AkkaFractal.Web/AkkaFractal.Web.csproj
COPY ./AkkaFractal.Core/AkkaFractal.Core.csproj ./src/AkkaFractal.Core/AkkaFractal.Core.csproj

RUN dotnet restore ./src/AkkaFractal.Core/AkkaFractal.Core.csproj 
RUN dotnet restore ./src/AkkaFractal.Web/AkkaFractal.Web.csproj 

# Copy all source code to image.
COPY ./AkkaFractal.Web ./src/AkkaFractal.Web
COPY ./AkkaFractal.Core ./src/AkkaFractal.Core

WORKDIR ./src/AkkaFractal.Web/
RUN dotnet build  AkkaFractal.Web.csproj -c Release
RUN dotnet publish AkkaFractal.Web.csproj -c Release -o publish

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
WORKDIR /app
COPY --from=build-env /app/out .

EXPOSE 5000

ENTRYPOINT ["dotnet", "AkkaFractal.Web.dll"]
