using System.ComponentModel.DataAnnotations;
using JsonApiDotNetCore.Models;

namespace Example.Api.Resources
{
    public class Book : Identifiable
    {
        [Attr]
        [Required]
        public string Title { get; set; }

        [Attr("synopsis", isImmutable: true, isSortable: false)]
        [MinLength(3)]
        public string Summary { get; set; }

        [HasOne]
        public Person Author { get; set; }
    }
}
