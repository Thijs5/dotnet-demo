# Building multilayered .NET applications the right way
Entire blogs are written about how to use a specific library or showcasing a new piece of technology.
Much less is written about how to structure your application. In this write-up, we begin at our web layer
and build up to a multilayered .NET-application.
When we're finished we'll be having an application consisting of three main archectural layers:
- Database
- Services
- Web API

If you are only interested in the result, by all means fetch the latest master branch. If you want to know how we get there, continue reading.

## Prerequisites
To get started with .NET core, you need to download it from [dotnet.microsoft.com](https://dotnet.microsoft.com/download).
After downloading and installing everything, verify the installation was succesful. Open to a terminal window and type `dotnet --version`.
On the time of writing, this returns `2.2.105`.

![version](./_guide/version.png)

## Creating an API
To start of we're creating a single API-project. When this step is finished we will have:
- A single project in our solution
- CRUD actions on a controller level for a data model
- Validation on these CRUD actions
- Installed Swagger UI so we call our controller within the browser
- Created unit tests for our controller

The naming convension for projects is `$"{NameOfTheApplication}.{Layer}"`.
Using this convension we will name our API-folder `MyApplication.API`.
The commands below will create a folder, initialize a .NET core webapi, and run the project.
```
mkdir MyApplication.API && cd MyApplication.API
dotnet new webapi
dotnet run watch
```
If you're using git, copy the `.gitignore` from the (dotnet core github)[https://github.com/dotnet/core/blob/master/.gitignore] and paste it in the root of the application.