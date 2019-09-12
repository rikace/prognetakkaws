FROM microsoft/dotnet:2.1-sdk AS base

# TODO lab (4) (a)
# generate the dotnet artefacts of the "AkkaFractal.Remote" project
#   - use the "dotnet" build to publish the artefacts in RELEASE mode
#       see these references
#       https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-publish?tabs=netcore21
#       https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-build

# TODO lab (4) (b) 
#   - change the working directory to "app" 
#       (this directory will be used to copy the artifacts of the "AkkaFractal.Remote")

#   - Copy the artefacts of the "AkkaFractal.Remote" project into the docker "app" directory 

#   - expose the port to make the Actor System reachable from outside

CMD ["dotnet", "AkkaFractal.Remote.dll"]


# TODO lab (4)
#   - build the docker image using this Dockerfile
#   - run the docker image just created
#   - run the "AkkaFractal.Web" project