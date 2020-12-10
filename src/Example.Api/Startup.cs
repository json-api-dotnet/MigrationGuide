using System;
using Example.Api.Data;
using Example.Api.Definitions;
using Example.Api.Repositories;
using Example.Api.Resources;
using Example.Api.Services;
using JsonApiDotNetCore.Configuration;
using JsonApiDotNetCore.Resources;
using JsonApiDotNetCore.Resources.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Example.Api
{
    public class Startup
    {
        private const string DbConnectionString =
            "Host=localhost;Port=5432;Database=JsonApiDotNetCoreMigrationExample;User ID=postgres;Password=postgres";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(DbConnectionString);
            });

            services.AddJsonApi<AppDbContext>(options =>
            {
                options.IncludeTotalResourceCount = true;
                options.Namespace = "api";
                options.UseRelativeLinks = true;
                options.SerializerSettings.Formatting = Formatting.Indented;
                options.DefaultPageSize = new PageSize(10);
                options.EnableLegacyFilterNotation = true;
                options.TopLevelLinks = LinkTypes.Paging;
                options.ResourceLinks = LinkTypes.None;

                options.SerializerSettings.ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new KebabCaseNamingStrategy()
                };
            });

            services.AddResourceRepository<PersonRepository>();
            services.AddResourceService<BookService>();
            services.AddScoped<IResourceDefinition<Book>, BookDefinition>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment environment, AppDbContext appDbContext)
        {
            SeedSampleData(appDbContext);

            if (environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseJsonApi();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }

        private static void SeedSampleData(AppDbContext appDbContext)
        {
            appDbContext.Database.EnsureDeleted();
            appDbContext.Database.EnsureCreated();

            appDbContext.Books.Add(new Book
            {
                Title = "Gulliver's Travels",
                Summary = "This book is about...",
                Author = new Person
                {
                    FirstName = "John",
                    LastName = "Doe",
                    BornAt = new DateTimeOffset(new DateTime(1993, 3, 29), TimeSpan.FromHours(3))
                }
            });

            appDbContext.SaveChanges();
        }
    }
}
