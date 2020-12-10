using System.Collections.Generic;
using System.Linq;
using Example.Api.Resources;
using JsonApiDotNetCore.Configuration;
using JsonApiDotNetCore.Queries;
using JsonApiDotNetCore.Repositories;
using JsonApiDotNetCore.Resources;
using Microsoft.Extensions.Logging;

namespace Example.Api.Repositories
{
    public class PersonRepository : EntityFrameworkCoreRepository<Person>
    {
        private readonly ILogger<PersonRepository> _logger;

        public PersonRepository(ITargetedFields targetedFields, IDbContextResolver contextResolver,
            IResourceGraph resourceGraph, IResourceFactory resourceFactory,
            IEnumerable<IQueryConstraintProvider> constraintProviders, ILoggerFactory loggerFactory)
            : base(targetedFields, contextResolver, resourceGraph, resourceFactory, constraintProviders, loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<PersonRepository>();
        }

        protected override IQueryable<Person> GetAll()
        {
            _logger.LogDebug("Entering PersonRepository.GetAll");

            return base.GetAll();
        }
    }
}
