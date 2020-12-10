using System.Threading.Tasks;
using Example.Api;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
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
  ""data"": [
    {
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
      },
      ""type"": ""people"",
      ""id"": ""1""
    }
  ],
  ""links"": {
    ""last"": ""/api/people?page[size]=10&page[number]=1""
  },
  ""meta"": {
    ""total-records"": 1
  }
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
  ""data"": [
    {
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
      },
      ""type"": ""people"",
      ""id"": ""1""
    }
  ],
  ""links"": {
    ""last"": ""/api/people?page[size]=10&page[number]=1&filter[last-name]=Doe""
  },
  ""meta"": {
    ""total-records"": 1
  }
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
    },
    ""type"": ""people"",
    ""id"": ""1""
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
  ""data"": [
    {
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
      },
      ""type"": ""books"",
      ""id"": ""1""
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
    },
    ""type"": ""people"",
    ""id"": ""1""
  },
  ""included"": [
    {
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
      },
      ""type"": ""books"",
      ""id"": ""1""
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
    ""attributes"": {},
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
    },
    ""type"": ""people"",
    ""id"": ""1""
  },
  ""included"": [
    {
      ""attributes"": {
        ""title"": ""Gulliver's Travels""
      },
      ""relationships"": {
        ""author"": {
          ""links"": {
            ""self"": ""/api/books/1/relationships/author"",
            ""related"": ""/api/books/1/author""
          }
        }
      },
      ""type"": ""books"",
      ""id"": ""1""
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
    ""attributes"": {
      ""last-name"": ""Doe""
    },
    ""relationships"": {
      ""books"": {
        ""links"": {
          ""self"": ""/api/people/1/relationships/books"",
          ""related"": ""/api/people/1/books""
        }
      }
    },
    ""type"": ""people"",
    ""id"": ""1""
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

            responseBody.Should().Be("");
        }
    }
}
