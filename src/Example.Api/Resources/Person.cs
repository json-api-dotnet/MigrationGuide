using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using JsonApiDotNetCore.Resources;
using JsonApiDotNetCore.Resources.Annotations;

namespace Example.Api.Resources
{
    public class Person : Identifiable
    {
        [Attr]
        public string FirstName { get; set; }

        [Attr]
        [Required]
        public string LastName { get; set; }

        [Attr(Capabilities = ~AttrCapabilities.AllowFilter)]
        public DateTimeOffset BornAt { get; set; }

        [HasMany]
        public IList<Book> Books { get; set; }
    }
}
