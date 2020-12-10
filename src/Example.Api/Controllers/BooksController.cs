using Example.Api.Resources;
using JsonApiDotNetCore.Controllers;
using JsonApiDotNetCore.Services;

namespace Example.Api.Controllers
{
    public class BooksController : JsonApiController<Book>
    {
        public BooksController(IJsonApiContext jsonApiContext, IResourceService<Book> resourceService)
            : base(jsonApiContext, resourceService)
        {
        }
    }
}
