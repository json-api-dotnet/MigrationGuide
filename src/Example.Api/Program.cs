using Example.Api;
using Example.Api.Data;
using Example.Api.Definitions;
using Example.Api.Repositories;
using Example.Api.Resources;
using Example.Api.Services;
using JsonApiDotNetCore.Configuration;
using JsonApiDotNetCore.Resources.Annotations;

const string dbConnectionString = "Host=localhost;Port=5432;Database=JsonApiDotNetCoreMigrationExample;User ID=postgres;Password=postgres";

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddNpgsql<AppDbContext>(dbConnectionString);

builder.Services.AddJsonApi<AppDbContext>(options =>
{
    options.IncludeTotalResourceCount = true;
    options.Namespace = "api";
    options.UseRelativeLinks = true;
    options.DefaultPageSize = new PageSize(10);
    options.EnableLegacyFilterNotation = true;
    options.TopLevelLinks = LinkTypes.Paging;
    options.ResourceLinks = LinkTypes.None;
    options.SerializerOptions.WriteIndented = true;
    options.SerializerOptions.PropertyNamingPolicy = JsonKebabCaseNamingPolicy.Instance;
    options.SerializerOptions.DictionaryKeyPolicy = JsonKebabCaseNamingPolicy.Instance;
    options.ValidateModelState = false;
    options.AllowUnknownFieldsInRequestBody = true;
    options.IncludeRequestBodyInErrors = true;
});

builder.Services.AddResourceRepository<PersonRepository>();
builder.Services.AddResourceService<BookService>();
builder.Services.AddResourceDefinition<BookDefinition>();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.

app.UseRouting();
app.UseJsonApi();
app.MapControllers();

await CreateDatabaseAsync(app.Services);

app.Run();

static async Task CreateDatabaseAsync(IServiceProvider serviceProvider)
{
    await using AsyncServiceScope scope = serviceProvider.CreateAsyncScope();

    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await dbContext.Database.EnsureDeletedAsync();
    await dbContext.Database.EnsureCreatedAsync();

    await SeedSampleDataAsync(dbContext);
}

static async Task SeedSampleDataAsync(AppDbContext dbContext)
{
    dbContext.Books.Add(new Book
    {
        Title = "Gulliver's Travels",
        Summary = "This book is about...",
        Author = new Person
        {
            FirstName = "John",
            LastName = "Doe",
            BornAt = new DateTime(1993, 3, 29, 0, 0, 0, DateTimeKind.Utc)
        }
    });

    await dbContext.SaveChangesAsync();
}
