using System.Net;
using Example.Api.Data;
using FluentAssertions;
using JsonApiDotNetCore.Serialization.Objects;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;

namespace Example.Tests.IntegrationTests;

public sealed class PersonTests : ExampleTestFixture
{
    public PersonTests(WebApplicationFactory<AppDbContext> factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Can_get_people()
    {
        // Act
        HttpResponseMessage response = await ExecuteGetRequestAsync("/api/people");

        // Assert
        response.EnsureSuccessStatus();

        string responseBody = await response.Content.ReadAsStringAsync();

        responseBody.Should().Be(@"{
  ""links"": {
    ""first"": ""/api/people"",
    ""last"": ""/api/people""
  },
  ""data"": [
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
  ],
  ""meta"": {
    ""total"": 1
  }
}");
    }

    [Fact]
    public async Task Can_filter_people_by_last_name()
    {
        // Act
        HttpResponseMessage response = await ExecuteGetRequestAsync("/api/people?filter[last-name]=Doe");

        // Assert
        response.EnsureSuccessStatus();

        string responseBody = await response.Content.ReadAsStringAsync();

        responseBody.Should().Be(@"{
  ""links"": {
    ""first"": ""/api/people?filter%5Blast-name%5D=Doe"",
    ""last"": ""/api/people?filter%5Blast-name%5D=Doe""
  },
  ""data"": [
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
  ],
  ""meta"": {
    ""total"": 1
  }
}");
    }

    [Fact]
    public async Task Can_get_person_by_ID()
    {
        // Act
        HttpResponseMessage response = await ExecuteGetRequestAsync("/api/people/1");

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
    public async Task Can_get_person_books()
    {
        // Act
        HttpResponseMessage response = await ExecuteGetRequestAsync("/api/people/1/books");

        // Assert
        response.EnsureSuccessStatus();

        string responseBody = await response.Content.ReadAsStringAsync();

        responseBody.Should().Be(@"{
  ""links"": {
    ""first"": ""/api/people/1/books"",
    ""last"": ""/api/people/1/books""
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
    public async Task Can_get_person_books_relationship()
    {
        // Act
        HttpResponseMessage response = await ExecuteGetRequestAsync("/api/people/1/relationships/books");

        // Assert
        response.EnsureSuccessStatus();

        string responseBody = await response.Content.ReadAsStringAsync();

        responseBody.Should().Be(@"{
  ""links"": {
    ""first"": ""/api/people/1/relationships/books"",
    ""last"": ""/api/people/1/relationships/books""
  },
  ""data"": [
    {
      ""type"": ""books"",
      ""id"": ""1""
    }
  ],
  ""meta"": {
    ""total"": 1
  }
}");
    }

    [Fact]
    public async Task Can_get_person_including_books()
    {
        // Act
        HttpResponseMessage response = await ExecuteGetRequestAsync("/api/people/1?include=books");

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
        },
        ""data"": [
          {
            ""type"": ""books"",
            ""id"": ""1""
          }
        ]
      }
    }
  },
  ""included"": [
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
    public async Task Can_get_person_including_book_titles()
    {
        // Act
        HttpResponseMessage response = await ExecuteGetRequestAsync("/api/people/1?include=books&fields[books]=title");

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
        },
        ""data"": [
          {
            ""type"": ""books"",
            ""id"": ""1""
          }
        ]
      }
    }
  },
  ""included"": [
    {
      ""type"": ""books"",
      ""id"": ""1"",
      ""attributes"": {
        ""title"": ""Gulliver's Travels""
      }
    }
  ]
}");
    }

    [Fact]
    public async Task Can_get_person_last_name()
    {
        // Act
        HttpResponseMessage response = await ExecuteGetRequestAsync("/api/people/1?fields[people]=last-name");

        // Assert
        response.EnsureSuccessStatus();

        string responseBody = await response.Content.ReadAsStringAsync();

        responseBody.Should().Be(@"{
  ""data"": {
    ""type"": ""people"",
    ""id"": ""1"",
    ""attributes"": {
      ""last-name"": ""Doe""
    }
  }
}");
    }

    [Fact]
    public async Task Cannot_get_missing_person()
    {
        // Act
        HttpResponseMessage response = await ExecuteGetRequestAsync("/api/people/99999999");

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
      ""detail"": ""Resource of type 'people' with ID '99999999' does not exist.""
    }
  ]
}");
    }
}
