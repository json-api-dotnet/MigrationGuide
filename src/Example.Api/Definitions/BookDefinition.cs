using System.Collections.Generic;
using System.Linq;
using Example.Api.Resources;
using JsonApiDotNetCore.Internal;
using JsonApiDotNetCore.Internal.Query;
using JsonApiDotNetCore.Models;

namespace Example.Api.Definitions
{
    public class BookDefinition : ResourceDefinition<Book>
    {
        private readonly IResourceGraph _resourceGraph;

        public BookDefinition(IResourceGraph resourceGraph)
        {
            _resourceGraph = resourceGraph;
        }

        protected override PropertySortOrder GetDefaultSortOrder()
        {
            return new PropertySortOrder
            {
                (book => book.Id, SortDirection.Ascending)
            };
        }

        public override QueryFilters GetQueryFilters()
        {
            return new QueryFilters
            {
                {"hide", (bookQuery, parameterValue) => GetBooksFilter(bookQuery, parameterValue)}
            };
        }

        private IQueryable<Book> GetBooksFilter(IQueryable<Book> bookQuery, string parameterValue)
        {
            return parameterValue == "all" ? bookQuery.Where(_ => false) : bookQuery;
        }

        protected override List<AttrAttribute> OutputAttrs(Book instance)
        {
            var contextEntity = _resourceGraph.GetContextEntity(typeof(Book));

            var visibleAttributes = contextEntity.Attributes
                .Where(attr => !attr.InternalAttributeName.StartsWith("Hidden"))
                .ToList();

            return visibleAttributes;
        }
    }
}
