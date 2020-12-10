using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using JsonApiDotNetCore.Models;

namespace Example.Api.Resources
{
    public class Person : Identifiable
    {
        [Attr]
        public string FirstName { get; set; }

        [Attr]
        [Required]
        public string LastName { get; set; }

        [Attr(isFilterable: false)]
        public DateTimeOffset BornAt { get; set; }

        [HasMany]
        public IList<Book> Books { get; set; }
    }
}
