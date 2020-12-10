using Example.Api.Resources;
using JsonApiDotNetCore.Controllers;
using JsonApiDotNetCore.Services;

namespace Example.Api.Controllers
{
    public class PeopleController : JsonApiController<Person>
    {
        public PeopleController(IJsonApiContext jsonApiContext, IResourceService<Person> resourceService)
            : base(jsonApiContext, resourceService)
        {
        }
    }
}
