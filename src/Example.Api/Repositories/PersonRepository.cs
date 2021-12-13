using Example.Api.Resources;
using JsonApiDotNetCore.Configuration;
using JsonApiDotNetCore.Queries;
using JsonApiDotNetCore.Repositories;
using JsonApiDotNetCore.Resources;

namespace Example.Api.Repositories;

public sealed class PersonRepository : EntityFrameworkCoreRepository<Person, int>
{
    private readonly ILogger<PersonRepository> _logger;

    public PersonRepository(ITargetedFields targetedFields, IDbContextResolver dbContextResolver, IResourceGraph resourceGraph,
        IResourceFactory resourceFactory, IEnumerable<IQueryConstraintProvider> constraintProviders, ILoggerFactory loggerFactory,
        IResourceDefinitionAccessor resourceDefinitionAccessor)
        : base(targetedFields, dbContextResolver, resourceGraph, resourceFactory, constraintProviders, loggerFactory, resourceDefinitionAccessor)
    {
        _logger = loggerFactory.CreateLogger<PersonRepository>();
    }

    protected override IQueryable<Person> GetAll()
    {
        _logger.LogDebug("Entering PersonRepository.GetAll");

        return base.GetAll();
    }
}
