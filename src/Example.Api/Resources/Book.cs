using System.ComponentModel.DataAnnotations;
using JsonApiDotNetCore.Resources;
using JsonApiDotNetCore.Resources.Annotations;

namespace Example.Api.Resources;

[Resource]
public sealed class Book : Identifiable<int>
{
    [Attr]
    public string Title { get; set; } = null!;

    [Attr(PublicName = "synopsis", Capabilities = AttrCapabilities.AllowView | AttrCapabilities.AllowFilter)]
    [MinLength(3)]
    public string? Summary { get; set; }

    [HasOne]
    public Person? Author { get; set; }
}
