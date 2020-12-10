using System.Linq;
using Example.Api.Resources;
using JsonApiDotNetCore.Data;
using JsonApiDotNetCore.Models;
using JsonApiDotNetCore.Services;
using Microsoft.Extensions.Logging;

namespace Example.Api.Repositories
{
    public class PersonRepository : DefaultEntityRepository<Person>
    {
        private readonly ILogger<PersonRepository> _logger;

        public PersonRepository(ILoggerFactory loggerFactory, IJsonApiContext jsonApiContext,
            IDbContextResolver contextResolver, ResourceDefinition<Person> resourceDefinition = null)
            : base(loggerFactory, jsonApiContext, contextResolver, resourceDefinition)
        {
            _logger = loggerFactory.CreateLogger<PersonRepository>();
        }

        public override IQueryable<Person> Get()
        {
            _logger.LogDebug("Entering PersonRepository.Get");

            return base.Get();
        }
    }
}
