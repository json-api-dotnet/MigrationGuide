using System.Collections.Immutable;
using System.ComponentModel;
using Example.Api.Resources;
using JsonApiDotNetCore.Configuration;
using JsonApiDotNetCore.Queries.Expressions;
using JsonApiDotNetCore.Resources;
using JsonApiDotNetCore.Resources.Annotations;

namespace Example.Api.Definitions;

public sealed class BookDefinition : JsonApiResourceDefinition<Book, int>
{
    public BookDefinition(IResourceGraph resourceGraph)
        : base(resourceGraph)
    {
    }

    public override SortExpression OnApplySort(SortExpression? existingSort)
    {
        if (existingSort != null)
        {
            return existingSort;
        }

        return CreateSortExpressionFromLambda(new PropertySortOrder
        {
            (book => book.Id, ListSortDirection.Ascending)
        });
    }

    public override QueryStringParameterHandlers<Book> OnRegisterQueryableHandlersForQueryStringParameters()
    {
        return new QueryStringParameterHandlers<Book>
        {
            { "hide", (bookQuery, parameterValue) => GetBooksFilter(bookQuery, parameterValue) }
        };
    }

    private IQueryable<Book> GetBooksFilter(IQueryable<Book> bookQuery, string parameterValue)
    {
        return parameterValue == "all" ? bookQuery.Where(_ => false) : bookQuery;
    }

    public override SparseFieldSetExpression? OnApplySparseFieldSet(SparseFieldSetExpression? existingSparseFieldSet)
    {
        if (existingSparseFieldSet != null)
        {
            ImmutableHashSet<ResourceFieldAttribute> visibleFields = existingSparseFieldSet.Fields
                .Where(field => !field.Property.Name.StartsWith("Hidden", StringComparison.Ordinal))
                .ToImmutableHashSet();

            if (visibleFields.Any())
            {
                return new SparseFieldSetExpression(visibleFields);
            }
        }

        return null;
    }
}
