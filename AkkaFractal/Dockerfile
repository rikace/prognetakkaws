FROM microsoft/dotnet:2.1-sdk AS builder

WORKDIR /app
# TODO (a) 
# copy both project files "AkkaFractal.Remote.csproj" and "AkkaFractal.Core.csproj" 
# from the current solution into the docker "app" folder

# COPY ./AkkaFractal.Remote/AkkaFractal.Remote.csproj << .. more here .. >>

# TODO (b)
# restore both project files just copied (use dotnet core command)
#   reference command here https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-restore?tabs=netcore2x
# RUN  << .. more here .. >>

WORKDIR ./src/AkkaFractal.Remote/
RUN dotnet restore
WORKDIR /app

# TODO (c)
# Copy all source code from both projects folder "AkkaFractal.Remote" and "AkkaFractal.Core" to the image.
# COPY << .. more here .. >>


WORKDIR ./src/AkkaFractal.Remote/
RUN dotnet build  AkkaFractal.Remote.csproj -c Release
RUN dotnet publish AkkaFractal.Remote.csproj -c Release -o publish

FROM microsoft/dotnet:2.1-sdk AS runner
WORKDIR /app

COPY --from=builder /app/src/AkkaFractal.Remote/publish/ .

EXPOSE 8090

CMD ["dotnet", "AkkaFractal.Remote.dll"]
