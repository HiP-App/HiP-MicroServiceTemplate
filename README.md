 # HiP-MicroServiceTemplate
A custom "dotnet new"-template for creating backend services that make use of event sourcing.

## How to Use
1. Clone the repository or download as ZIP-file
1. Install the template: `dotnet new --install /path/to/downloaded/repo/Template`
1. Create a new solution using the template: `dotnet new hipapi [--MakeSdk --NpmPackageName MyNewService] -o MyNewService`

**Switches**:
* `--MakeSdk` (optional): Including it enables automatic REST client generation via NSwag.
* `--NpmPackageName` (optional): Provides the name for the generated NPM package. This switch should be used together with the `--MakeSdk` switch. 

## Features
✔ **Like DataStore**: Creates a service with a structure similar to [HiP-DataStore](https://github.com/HiP-App/HiP-DataStore) and other services!  
✔ **Includes Auth-stuff**: Authentication and authorization work right out of the box!  
✔ **Ready to Run**: The generated service is immediately executable with no further configuration (*1)!  
✔ **Sample Code**: Includes a sample resource type *Foo* with all the event classes, DTO classes and controller methods  
✔ **Swagger**: Sets up Swagger and Swagger UI  
✔ **Auto-Generated SDK**: Sets up [NSwag](https://github.com/RSuter/NSwag) for auto-generation of REST client code (*2)
✔ **Integration Tests**: Scaffolds an xUnit test project to verify the correctness of the service
✔ **Ready for Docker**: Includes a Dockerfile so the generated service can be easily deployed to our Docker cloud  
✔ **Ready for Git**: Includes gitignore-file and gitattributes-file

(*1) Note: Before running the generated service, make sure MongoDB and EventStore are running.  
(*2) Note: This is optional.

## Links
The following topics are recommended to understand the [.NET template engine](https://github.com/dotnet/templating):
* ["dotnet new"-Command](https://docs.microsoft.com/dotnet/core/tools/dotnet-new)
* [Custom Templates for "dotnet new"](https://docs.microsoft.com/en-us/dotnet/core/tools/custom-templates)
* [Create a Custom Template for "dotnet new"](https://docs.microsoft.com/dotnet/core/tutorials/create-custom-template)
* [How to Create Your Own Templates for "dotnet new"](https://blogs.msdn.microsoft.com/dotnet/2017/04/02/how-to-create-your-own-templates-for-dotnet-new/)
