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

If you're using git, copy the `.gitignore` from the (dotnet core github)[https://github.com/dotnet/core/blob/master/.gitignore] and paste it in the root of the application. Having an incomplete gitignore-file will result in compiled binaries getting added to git.

### Initialize API
The naming convension for projects is `$"{NameOfTheApplication}.{Layer}"`.
Using this convension we will name our API-folder `MyApplication.API`.
The commands below will create a folder, initialize a .NET core webapi, and run the project on port 5000 (http) and 5001 (https). Browsing to (https://localhost:5001/api/values)[https://localhost:5001/api/values] will show a list of two values.
```
mkdir MyApplication.API && cd MyApplication.API
dotnet new webapi
dotnet run watch
```

### Creating a Data Model and CRUD actions
Inside our API-project we're creating a new folder `Models`.
This folder will hold all the models we use for data we're getting from the user and returning to the user.
For this example we will create a models for a `BlogPost`-entity. 

```csharp
namespace MyApplication.API.Models
{
    /// <summary>
    /// A blog post entity.
    /// </summary>
    public class BlogPost
    {
        /// <summary>
        /// Id of the blog post.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///  Title of the blog post.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The text of the blog post.
        /// </summary>
        public string Text { get; set; }
    }
}
```

For each entity we create a subfolder in `MyApplication.API.Models`.
Even though every model is in a seperate subfolder the namespace of the models should be `MyApplication.API.Models`.
Changing the namespace will greatly reduce the amount of usings we're going to need further down the road.

Now that we have a model, let's create a controller for it.
Inside `MyApplication.Controllers` we add a new file named `BlogPostsController`. Note the pluralisation of the name.

```csharp
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MyApplication.API.Models;

namespace MyApplication.API.Controllers
{
    /// <summary>
    /// Endpoint for blog posts.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class BlogPostsController : ControllerBase
    {
        private readonly List<BlogPost> _blogPosts = new List<BlogPost>()
        {
            new BlogPost() { Id = 1, Title = "Title 1", Text = "Text 1" },
            new BlogPost() { Id = 2, Title = "Title 2", Text = "Text 2" },
        };

        #region GET     /
        [HttpGet]
        public ActionResult<IEnumerable<BlogPost>> Get()
        {
            return _blogPosts;
        }
        #endregion
        
        #region GET     /{id}
        [HttpGet("{id}")]
        public ActionResult<BlogPost> Get(int id)
        {
            var blogPost = _blogPosts.FirstOrDefault(x => x.Id == id);
            if (blogPost == null) {
                return NotFound();
            }
            return blogPost;
        }
        #endregion
    }
}
```

For the create and update actions, we need need new models representing the user input. We create a `CreateBlogPost` and an `UpdateBlogPost`. For the sake of example we can't edit the title of a blog post after creation.

```csharp
namespace MyApplication.API.Models
{
    public class CreateBlogPost
    {
        [Required]
        [MaxLength(40)]
        public string Title { get; set; }

        [Required]
        public string Text { get; set; }
    }
    public class UpdateBlogPost
    {
        [Required]
        public string Text { get; set; }
    }
}
```

The create/update/delete functions look like this. Since we've added attributes on our CreateBlogPost and UpdateBlogPost there is no need to check validations here.

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MyApplication.API.Models;

namespace MyApplication.API.Controllers
{
    /// <summary>
    /// Endpoint for blog posts.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class BlogPostsController : ControllerBase
    {
        static int _blogPostId = 0;
        static List<BlogPost> _blogPosts = new List<BlogPost>()
        {
            new BlogPost() { Id = ++_blogPostId, Title = "Title 1", Text = "Text 1" },
            new BlogPost() { Id = ++_blogPostId, Title = "Title 2", Text = "Text 2" },
        };

        // ...

        #region POST    /
        [HttpPost]
        public void Create([FromBody] CreateBlogPost createBlogPost)
        {
            var newBlogPost = new BlogPost()
            {
                Id = ++_blogPostId,
                Title = createBlogPost.Title,
                Text = createBlogPost.Text,
            };
            _blogPosts.Add(newBlogPost);
        }
        #endregion

        #region PUT     /{id}
        [HttpPut("{id}")]
        public ActionResult<BlogPost> Update(
            int id,
            [FromBody] UpdateBlogPost updatedBlogPost
        )
        {
            try
            {
                var blogPost = _blogPosts.FirstOrDefault(x => x.Id == id);
                if (blogPost == null) { throw new Exception("NOT_FOUND"); }
                blogPost.Text = updatedBlogPost.Text;
                return blogPost;
            }
            catch (Exception ex)
            {
                if (ex.Message == "NOT_FOUND")
                {
                    return NotFound();
                }
                return BadRequest();
            }
        }
        #endregion

        #region DELETE  /{id}
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var blogPost = _blogPosts.FirstOrDefault(x => x.Id == id);
                if (blogPost == null) { throw new Exception("NOT_FOUND"); }
                _blogPosts.Remove(blogPost);
                return Ok();
            }
            catch (Exception ex)
            {
                if (ex.Message == "NOT_FOUND")
                {
                    return NotFound();
                }
                return BadRequest();
            }
        }
        #endregion
    }
}
```

The reason every call is wrapped in a `#region` is to help us list all API routes in this controller. When minimising every region, our controller looks like a nice overview of all routes.

<img src="./_guide/region-in-controllers.png" width="400">


### Adding Swagger
To finish of the API layer, we're going to add Swagger UI. Swagger is going to help us document our API layer and ables us to test calls inside the browser. Installing Swagger UI is easy using Nuget. Open a terminal window and paste

```dotnet add package Swashbuckle.AspNetCore```

After adding the package, Swagger is not yet configured. All info is bundled on (docs.microsoft.com)[https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-2.2&tabs=visual-studio-code], but for convineance the steps will be listed below.

Swagger isn't the only thing we are going to configure in this project. Inside `MyApplication.API` we're going to add a new folder named `Configurations`. This folder will hold static classes containing, you guessed it, configurations. Add a file `SwaggerConfiguration`.

```csharp
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Swashbuckle.AspNetCore.Swagger;
using System.Reflection;
using System.IO;
using System;

namespace MyApplication.API
{
    internal static class SwaggerConfiguration
    {
        internal static void Configure(IServiceCollection services)
        {
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My Application API", Version = "v1" });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        internal static void Configure(IApplicationBuilder app)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                
                // Our application consists only an API (every route is behind /api/).
                // Setting the RoutePrefix to empty servers Swagger at the root of our application.
                c.RoutePrefix = string.Empty;
            });
        }
    }
}

```

To generate an XML with documentation add the following to the `MyApplication.API.csproj`.

```xml
<PropertyGroup>
    <!-- Generate a documentation file for Swagger.  -->
    <GenerateDocumentationFile>true</GenerateDocumentationFile>
    <!-- Disable project wide warnings for missing documentation. -->
    <!-- <NoWarn>$(NoWarn);1591</NoWarn> -->
</PropertyGroup>
```

Enabling Swagger with the configuration above will throw warnings on every public property without XML documentation.
Generally we only want to force documentation on the controllers. To disable the warnings, wrap classes where you don't want to put documentation with

```csharp
#pragma warning disable CS1591
namespace ... {
    // ...
}
#pragma warning restore CS1591
```

The API documentation looks like the example below. Make sure every action is documentated so your Swagger is always up to date.
```csharp
/// <summary>
/// Update an existing blog post.
/// </summary>
/// <param name="id">Id of the blog post to update</param>
/// <param name="updatedBlogPost">New values for the blog post</param>
/// <returns>Returns the updated blog post.</returns>
/// <response code="200">Blog post succesfully updated</response>
/// <response code="400">Validation error</response>
/// <response code="404">No blog posts found with the given id</response>
public ActionResult<BlogPost> Update() {}
```