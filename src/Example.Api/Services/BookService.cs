using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Example.Api.Resources;
using JsonApiDotNetCore.Configuration;
using JsonApiDotNetCore.Hooks;
using JsonApiDotNetCore.Middleware;
using JsonApiDotNetCore.Queries;
using JsonApiDotNetCore.Repositories;
using JsonApiDotNetCore.Resources;
using JsonApiDotNetCore.Services;
using Microsoft.Extensions.Logging;

namespace Example.Api.Services
{
    public class BookService : JsonApiResourceService<Book>
    {
        private readonly ILogger<BookService> _logger;

        public BookService(IResourceRepositoryAccessor repositoryAccessor, IQueryLayerComposer queryLayerComposer,
            IPaginationContext paginationContext, IJsonApiOptions options, ILoggerFactory loggerFactory,
            IJsonApiRequest request, IResourceChangeTracker<Book> resourceChangeTracker,
            IResourceHookExecutorFacade hookExecutor)
            : base(repositoryAccessor, queryLayerComposer, paginationContext, options, loggerFactory, request,
                resourceChangeTracker, hookExecutor)
        {
            _logger = loggerFactory.CreateLogger<BookService>();
        }

        public override async Task<IReadOnlyCollection<Book>> GetAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug("Entering BookService.GetAsync");

            return await base.GetAsync(cancellationToken);
        }
    }
}
