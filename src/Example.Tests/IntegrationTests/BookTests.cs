using System.Threading.Tasks;
using Example.Api;
using FluentAssertions;
using JsonApiDotNetCore.Serialization.Objects;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;

namespace Example.Tests.IntegrationTests
{
    public class BookTests : ExampleTestFixture
    {
        public BookTests(WebApplicationFactory<Startup> factory)
            : base(factory)
        {
        }

        [Fact]
        public async Task Can_get_books()
        {
            // Act
            var response = await ExecuteGetRequestAsync("/api/books");

            // Assert
            response.EnsureSuccessStatus();

            var responseBody = await response.Content.ReadAsStringAsync();

            responseBody.Should().Be(@"{
  ""meta"": {
    ""total-resources"": 1
  },
  ""links"": {
    ""first"": ""/api/books"",
    ""last"": ""/api/books""
  },
  ""data"": [
    {
      ""type"": ""books"",
      ""id"": ""1"",
      ""attributes"": {
        ""title"": ""Gulliver's Travels"",
        ""synopsis"": ""This book is about...""
      },
      ""relationships"": {
        ""author"": {
          ""links"": {
            ""self"": ""/api/books/1/relationships/author"",
            ""related"": ""/api/books/1/author""
          }
        }
      }
    }
  ]
}");
        }

        [Fact]
        public async Task Can_filter_books_by_title()
        {
            // Act
            var response = await ExecuteGetRequestAsync("/api/books?filter[title]=like:Gulliver");

            // Assert
            response.EnsureSuccessStatus();

            var responseBody = await response.Content.ReadAsStringAsync();

            responseBody.Should().Be(@"{
  ""meta"": {
    ""total-resources"": 1
  },
  ""links"": {
    ""first"": ""/api/books?filter[title]=like:Gulliver"",
    ""last"": ""/api/books?filter[title]=like:Gulliver""
  },
  ""data"": [
    {
      ""type"": ""books"",
      ""id"": ""1"",
      ""attributes"": {
        ""title"": ""Gulliver's Travels"",
        ""synopsis"": ""This book is about...""
      },
      ""relationships"": {
        ""author"": {
          ""links"": {
            ""self"": ""/api/books/1/relationships/author"",
            ""related"": ""/api/books/1/author""
          }
        }
      }
    }
  ]
}");
        }

        [Fact]
        public async Task Can_filter_books_by_custom_query()
        {
            // Act
            var response = await ExecuteGetRequestAsync("/api/books?hide=all");

            // Assert
            response.EnsureSuccessStatus();

            var responseBody = await response.Content.ReadAsStringAsync();

            responseBody.Should().Be(@"{
  ""meta"": {
    ""total-resources"": 0
  },
  ""data"": []
}");
        }

        [Fact]
        public async Task Can_get_book_by_ID()
        {
            // Act
            var response = await ExecuteGetRequestAsync("/api/books/1");

            // Assert
            response.EnsureSuccessStatus();

            var responseBody = await response.Content.ReadAsStringAsync();

            responseBody.Should().Be(@"{
  ""data"": {
    ""type"": ""books"",
    ""id"": ""1"",
    ""attributes"": {
      ""title"": ""Gulliver's Travels"",
      ""synopsis"": ""This book is about...""
    },
    ""relationships"": {
      ""author"": {
        ""links"": {
          ""self"": ""/api/books/1/relationships/author"",
          ""related"": ""/api/books/1/author""
        }
      }
    }
  }
}");
        }

        [Fact]
        public async Task Can_get_book_author()
        {
            // Act
            var response = await ExecuteGetRequestAsync("/api/books/1/author");

            // Assert
            response.EnsureSuccessStatus();

            var responseBody = await response.Content.ReadAsStringAsync();

            responseBody.Should().Be(@"{
  ""data"": {
    ""type"": ""people"",
    ""id"": ""1"",
    ""attributes"": {
      ""first-name"": ""John"",
      ""last-name"": ""Doe"",
      ""born-at"": ""1993-03-28T23:00:00+02:00""
    },
    ""relationships"": {
      ""books"": {
        ""links"": {
          ""self"": ""/api/people/1/relationships/books"",
          ""related"": ""/api/people/1/books""
        }
      }
    }
  }
}");
        }

        [Fact]
        public async Task Can_get_book_author_relationship()
        {
            // Act
            var response = await ExecuteGetRequestAsync("/api/books/1/relationships/author");

            // Assert
            response.EnsureSuccessStatus();

            var responseBody = await response.Content.ReadAsStringAsync();

            responseBody.Should().Be(@"{
  ""data"": {
    ""type"": ""people"",
    ""id"": ""1""
  }
}");
        }

        [Fact]
        public async Task Can_get_book_including_author()
        {
            // Act
            var response = await ExecuteGetRequestAsync("/api/books/1?include=author");

            // Assert
            response.EnsureSuccessStatus();

            var responseBody = await response.Content.ReadAsStringAsync();

            responseBody.Should().Be(@"{
  ""data"": {
    ""type"": ""books"",
    ""id"": ""1"",
    ""attributes"": {
      ""title"": ""Gulliver's Travels"",
      ""synopsis"": ""This book is about...""
    },
    ""relationships"": {
      ""author"": {
        ""links"": {
          ""self"": ""/api/books/1/relationships/author"",
          ""related"": ""/api/books/1/author""
        },
        ""data"": {
          ""type"": ""people"",
          ""id"": ""1""
        }
      }
    }
  },
  ""included"": [
    {
      ""type"": ""people"",
      ""id"": ""1"",
      ""attributes"": {
        ""first-name"": ""John"",
        ""last-name"": ""Doe"",
        ""born-at"": ""1993-03-28T23:00:00+02:00""
      },
      ""relationships"": {
        ""books"": {
          ""links"": {
            ""self"": ""/api/people/1/relationships/books"",
            ""related"": ""/api/people/1/books""
          }
        }
      }
    }
  ]
}");
        }

        [Fact]
        public async Task Can_get_book_including_author_last_name()
        {
            // Act
            var response = await ExecuteGetRequestAsync("/api/books/1?include=author&fields[people]=last-name");

            // Assert
            response.EnsureSuccessStatus();

            var responseBody = await response.Content.ReadAsStringAsync();

            responseBody.Should().Be(@"{
  ""data"": {
    ""type"": ""books"",
    ""id"": ""1"",
    ""attributes"": {
      ""title"": ""Gulliver's Travels"",
      ""synopsis"": ""This book is about...""
    },
    ""relationships"": {
      ""author"": {
        ""links"": {
          ""self"": ""/api/books/1/relationships/author"",
          ""related"": ""/api/books/1/author""
        },
        ""data"": {
          ""type"": ""people"",
          ""id"": ""1""
        }
      }
    }
  },
  ""included"": [
    {
      ""type"": ""people"",
      ""id"": ""1"",
      ""attributes"": {
        ""last-name"": ""Doe""
      }
    }
  ]
}");
        }

        [Fact]
        public async Task Can_get_book_title()
        {
            // Act
            var response = await ExecuteGetRequestAsync("/api/books/1?fields[books]=title");

            // Assert
            response.EnsureSuccessStatus();

            var responseBody = await response.Content.ReadAsStringAsync();

            responseBody.Should().Be(@"{
  ""data"": {
    ""type"": ""books"",
    ""id"": ""1"",
    ""attributes"": {
      ""title"": ""Gulliver's Travels""
    }
  }
}");
        }

        [Fact]
        public async Task Cannot_get_missing_book()
        {
            // Act
            var response = await ExecuteGetRequestAsync("/api/books/99999999");

            // Assert
            response.StatusCode.Should().Be(404);

            var responseBody = await response.Content.ReadAsStringAsync();

            var responseDocument = JsonConvert.DeserializeObject<ErrorDocument>(responseBody);
            var errorId = responseDocument.Errors[0].Id;

            responseBody.Should().Be(@"{
  ""errors"": [
    {
      ""id"": """ + errorId + @""",
      ""status"": ""404"",
      ""title"": ""The requested resource does not exist."",
      ""detail"": ""Resource of type 'books' with ID '99999999' does not exist.""
    }
  ]
}");
        }
    }
}
