using System.Net;
using Example.Api.Data;
using FluentAssertions;
using JsonApiDotNetCore.Serialization.Objects;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;

namespace Example.Tests.IntegrationTests;

public sealed class BookTests : ExampleTestFixture
{
    public BookTests(WebApplicationFactory<AppDbContext> factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Can_get_books()
    {
        // Act
        HttpResponseMessage response = await ExecuteGetRequestAsync("/api/books");

        // Assert
        response.EnsureSuccessStatus();

        string responseBody = await response.Content.ReadAsStringAsync();

        responseBody.Should().Be(@"{
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
  ],
  ""meta"": {
    ""total"": 1
  }
}");
    }

    [Fact]
    public async Task Can_filter_books_by_title()
    {
        // Act
        HttpResponseMessage response = await ExecuteGetRequestAsync("/api/books?filter[title]=like:Gulliver");

        // Assert
        response.EnsureSuccessStatus();

        string responseBody = await response.Content.ReadAsStringAsync();

        responseBody.Should().Be(@"{
  ""links"": {
    ""first"": ""/api/books?filter%5Btitle%5D=like%3AGulliver"",
    ""last"": ""/api/books?filter%5Btitle%5D=like%3AGulliver""
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
  ],
  ""meta"": {
    ""total"": 1
  }
}");
    }

    [Fact]
    public async Task Can_filter_books_by_custom_query()
    {
        // Act
        HttpResponseMessage response = await ExecuteGetRequestAsync("/api/books?hide=all");

        // Assert
        response.EnsureSuccessStatus();

        string responseBody = await response.Content.ReadAsStringAsync();

        responseBody.Should().Be(@"{
  ""data"": [],
  ""meta"": {
    ""total"": 0
  }
}");
    }

    [Fact]
    public async Task Can_get_book_by_ID()
    {
        // Act
        HttpResponseMessage response = await ExecuteGetRequestAsync("/api/books/1");

        // Assert
        response.EnsureSuccessStatus();

        string responseBody = await response.Content.ReadAsStringAsync();

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
        HttpResponseMessage response = await ExecuteGetRequestAsync("/api/books/1/author");

        // Assert
        response.EnsureSuccessStatus();

        string responseBody = await response.Content.ReadAsStringAsync();

        responseBody.Should().Be(@"{
  ""data"": {
    ""type"": ""people"",
    ""id"": ""1"",
    ""attributes"": {
      ""first-name"": ""John"",
      ""last-name"": ""Doe"",
      ""born-at"": ""1993-03-29T00:00:00+00:00""
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
        HttpResponseMessage response = await ExecuteGetRequestAsync("/api/books/1/relationships/author");

        // Assert
        response.EnsureSuccessStatus();

        string responseBody = await response.Content.ReadAsStringAsync();

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
        HttpResponseMessage response = await ExecuteGetRequestAsync("/api/books/1?include=author");

        // Assert
        response.EnsureSuccessStatus();

        string responseBody = await response.Content.ReadAsStringAsync();

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
        ""born-at"": ""1993-03-29T00:00:00+00:00""
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
        HttpResponseMessage response = await ExecuteGetRequestAsync("/api/books/1?include=author&fields[people]=last-name");

        // Assert
        response.EnsureSuccessStatus();

        string responseBody = await response.Content.ReadAsStringAsync();

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
        HttpResponseMessage response = await ExecuteGetRequestAsync("/api/books/1?fields[books]=title");

        // Assert
        response.EnsureSuccessStatus();

        string responseBody = await response.Content.ReadAsStringAsync();

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
        HttpResponseMessage response = await ExecuteGetRequestAsync("/api/books/99999999");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        string responseBody = await response.Content.ReadAsStringAsync();

        var responseDocument = JsonConvert.DeserializeObject<Document>(responseBody);
        string? errorId = responseDocument?.Errors?[0].Id;

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
