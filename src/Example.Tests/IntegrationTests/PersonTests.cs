using System.Threading.Tasks;
using Example.Api;
using FluentAssertions;
using JsonApiDotNetCore.Serialization.Objects;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;

namespace Example.Tests.IntegrationTests
{
    public class PersonTests : ExampleTestFixture
    {
        public PersonTests(WebApplicationFactory<Startup> factory)
            : base(factory)
        {
        }

        [Fact]
        public async Task Can_get_people()
        {
            // Act
            var response = await ExecuteGetRequestAsync("/api/people");

            // Assert
            response.EnsureSuccessStatus();

            var responseBody = await response.Content.ReadAsStringAsync();

            responseBody.Should().Be(@"{
  ""meta"": {
    ""total-resources"": 1
  },
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
        public async Task Can_filter_people_by_last_name()
        {
            // Act
            var response = await ExecuteGetRequestAsync("/api/people?filter[last-name]=Doe");

            // Assert
            response.EnsureSuccessStatus();

            var responseBody = await response.Content.ReadAsStringAsync();

            responseBody.Should().Be(@"{
  ""meta"": {
    ""total-resources"": 1
  },
  ""links"": {
    ""first"": ""/api/people?filter[last-name]=Doe"",
    ""last"": ""/api/people?filter[last-name]=Doe""
  },
  ""data"": [
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
        public async Task Can_get_person_by_ID()
        {
            // Act
            var response = await ExecuteGetRequestAsync("/api/people/1");

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
        public async Task Can_get_person_books()
        {
            // Act
            var response = await ExecuteGetRequestAsync("/api/people/1/books");

            // Assert
            response.EnsureSuccessStatus();

            var responseBody = await response.Content.ReadAsStringAsync();

            responseBody.Should().Be(@"{
  ""links"": {
    ""first"": ""/api/people/1/books""
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
        public async Task Can_get_person_books_relationship()
        {
            // Act
            var response = await ExecuteGetRequestAsync("/api/people/1/relationships/books");

            // Assert
            response.EnsureSuccessStatus();

            var responseBody = await response.Content.ReadAsStringAsync();

            responseBody.Should().Be(@"{
  ""links"": {
    ""first"": ""/api/people/1/relationships/books""
  },
  ""data"": [
    {
      ""type"": ""books"",
      ""id"": ""1""
    }
  ]
}");
        }

        [Fact]
        public async Task Can_get_person_including_books()
        {
            // Act
            var response = await ExecuteGetRequestAsync("/api/people/1?include=books");

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
            var response = await ExecuteGetRequestAsync("/api/people/1?include=books&fields[books]=title");

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
            var response = await ExecuteGetRequestAsync("/api/people/1?fields[people]=last-name");

            // Assert
            response.EnsureSuccessStatus();

            var responseBody = await response.Content.ReadAsStringAsync();

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
            var response = await ExecuteGetRequestAsync("/api/people/99999999");

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
      ""detail"": ""Resource of type 'people' with ID '99999999' does not exist.""
    }
  ]
}");
        }
    }
}
