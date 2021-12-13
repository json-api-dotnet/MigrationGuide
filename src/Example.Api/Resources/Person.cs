using JsonApiDotNetCore.Resources;
using JsonApiDotNetCore.Resources.Annotations;

namespace Example.Api.Resources;

[Resource]
public sealed class Person : Identifiable<int>
{
    [Attr]
    public string? FirstName { get; set; }

    [Attr]
    public string LastName { get; set; } = null!;

    [Attr(Capabilities = ~AttrCapabilities.AllowFilter)]
    public DateTimeOffset BornAt { get; set; }

    [HasMany]
    public ISet<Book> Books { get; set; } = new HashSet<Book>();
}
