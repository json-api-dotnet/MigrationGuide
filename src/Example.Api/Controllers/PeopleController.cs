﻿using Example.Api.Resources;
using JsonApiDotNetCore.Configuration;
using JsonApiDotNetCore.Controllers;
using JsonApiDotNetCore.Services;
using Microsoft.Extensions.Logging;

namespace Example.Api.Controllers
{
    public class PeopleController : JsonApiController<Person>
    {
        public PeopleController(IJsonApiOptions options, ILoggerFactory loggerFactory,
            IResourceService<Person> resourceService)
            : base(options, loggerFactory, resourceService)
        {
        }
    }
}
