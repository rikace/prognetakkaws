# Summary of branches 
- Branch `Step 1` contains implementation of the basic project with Actor commuinication
- Branch `Step 2` contains implementation of Actor Supervision and Actor Routing 
- Branch `Step 3` contains implementation of Actor Remoting (and Docker container to run the Web and Remote projects)
- Branch `Step 4` contains implementation of Actor Slustering (see compose.yml file to start the cluster)
- Branch `Step 5` contains implementation of Actor Persistent

# Install .NET SDK

Verify the .NET SDK version installed on your PC. Open a command prompt and run the following command...

```dotnet --version```

The version should be **2.1.403** or higher.

1. If you don't have installed .NET Core SDK, or you require a newer version, you can download and install it from [.NET SDK for Visual Studio](https://www.microsoft.com/net/download/visual-studio-sdks)

2. You can view additional information with the following command...

```dotnet --info```

This command lists all of the SDK and runtime versions installed on your PC.


 Running in a Docker container

This is a getting started workshop for using Docker, and focuses on using it for development since that is a logical and no-risk place to start. We will use an ASP.NET Core app for the exercise, but any app on most stacks that can run on Linux should work. You will be guided through running a container from an image from Docker Hub. Then you'll create your own image & container, in order to build and run an app inside the container, without having to install anything locally except Docker.



## Install Docker

Please go through these steps before the workshop, to ensure you don't spend time with setup, and downloading a large Docker image.

1. **Install Docker & Run a Container** Follow Docker's **installation instructions** for your platform, including the section for **testing it** to ensure it's working.

    - [Install & set up for Windows](https://store.docker.com/editions/community/docker-ce-desktop-windows?tab=description)

    - [Install & set up for Mac](https://store.docker.com/editions/community/docker-ce-desktop-mac?tab=description)

1. **On Windows only**, you will have to share your drive. You can do this by:

    - Right-clicking the docker tray icon
    - Selecting `Settings...`
    - On the left-hand side, select `Shared Drives`
    - Check the appropriate drive and click `Apply`
    - Enter your user account's password and click `OK`.

- **Test the installation by running the offical MongoDb container**. It will download the image from Docker Hub if it doesn't find it already in your local Docker repo:

    `docker run --rm -p 27017:27017 --name mongodb -d mongo`
 Once that starts you have a working Docker installation and you can stop the container with 
    `docker stop mongodb`

## Install Robomongo (Optional)

Robomongo (Robo3T) is a tool that allows you to connect, visualize and interact with an instance of MongoDb running. To download and install locally this tool click this [link](https://robomongo.org/download) abd follow the instruction

# How to run the application 
- Start the Web Application, open http://localhost:5000
- Start the Remote application
- In the web page http://localhost:5000 click "Start drawing"

# Know issue
- if the : http://localhost:5000 is blank see this references: 
	- https://github.com/aspnet/AspNetCore/issues/4587 
	- https://docs.microsoft.com/en-us/aspnet/core/security/enforcing-ssl?view=aspnetcore-2.2&tabs=visual-studio#trust-the-aspnet-core-https-development-certificate-on-windows-and-macos
- Run this command  `dotnet dev-certs https --trust`

