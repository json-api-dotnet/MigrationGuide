using System.Collections.Generic;
using System.Threading.Tasks;
using Example.Api.Resources;
using JsonApiDotNetCore.Data;
using JsonApiDotNetCore.Services;
using Microsoft.Extensions.Logging;

namespace Example.Api.Services
{
    public class BookService : EntityResourceService<Book>
    {
        private readonly ILogger<BookService> _logger;

        public BookService(IJsonApiContext jsonApiContext, IEntityRepository<Book> entityRepository,
            ILoggerFactory loggerFactory = null)
            : base(jsonApiContext, entityRepository, loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<BookService>();
        }

        public override async Task<IEnumerable<Book>> GetAsync()
        {
            _logger.LogDebug("Entering BookService.GetAsync");

            return await base.GetAsync();
        }
    }
}
