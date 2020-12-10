using System.ComponentModel.DataAnnotations;
using JsonApiDotNetCore.Resources;
using JsonApiDotNetCore.Resources.Annotations;

namespace Example.Api.Resources
{
    public class Book : Identifiable
    {
        [Attr]
        [Required]
        public string Title { get; set; }

        [Attr(PublicName = "synopsis", Capabilities = AttrCapabilities.AllowView | AttrCapabilities.AllowFilter)]
        [MinLength(3)]
        public string Summary { get; set; }

        [HasOne]
        public Person Author { get; set; }
    }
}
