using Example.Api.Resources;
using JsonApiDotNetCore.Configuration;
using JsonApiDotNetCore.Middleware;
using JsonApiDotNetCore.Queries;
using JsonApiDotNetCore.Repositories;
using JsonApiDotNetCore.Resources;
using JsonApiDotNetCore.Services;

namespace Example.Api.Services;

public sealed class BookService : JsonApiResourceService<Book, int>
{
    private readonly ILogger<BookService> _logger;

    public BookService(IResourceRepositoryAccessor repositoryAccessor, IQueryLayerComposer queryLayerComposer, IPaginationContext paginationContext,
        IJsonApiOptions options, ILoggerFactory loggerFactory, IJsonApiRequest request, IResourceChangeTracker<Book> resourceChangeTracker,
        IResourceDefinitionAccessor resourceDefinitionAccessor)
        : base(repositoryAccessor, queryLayerComposer, paginationContext, options, loggerFactory, request, resourceChangeTracker, resourceDefinitionAccessor)
    {
        _logger = loggerFactory.CreateLogger<BookService>();
    }

    public override async Task<IReadOnlyCollection<Book>> GetAsync(CancellationToken cancellationToken)
    {
        _logger.LogDebug("Entering BookService.GetAsync");

        return await base.GetAsync(cancellationToken);
    }
}
