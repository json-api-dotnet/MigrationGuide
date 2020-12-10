using System;
using Example.Api.Data;
using Example.Api.Definitions;
using Example.Api.Repositories;
using Example.Api.Resources;
using Example.Api.Services;
using JsonApiDotNetCore.Data;
using JsonApiDotNetCore.Extensions;
using JsonApiDotNetCore.Models;
using JsonApiDotNetCore.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

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
                options.IncludeTotalRecordCount = true;
                options.Namespace = "api";
                options.RelativeLinks = true;
                options.SerializerSettings.Formatting = Formatting.Indented;
                options.DefaultPageSize = 10;
            });

            services.AddScoped<IEntityRepository<Person>, PersonRepository>();
            services.AddScoped<IResourceService<Book>, BookService>();
            services.AddScoped<ResourceDefinition<Book>, BookDefinition>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, AppDbContext appDbContext)
        {
            SeedSampleData(appDbContext);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseJsonApi();
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
