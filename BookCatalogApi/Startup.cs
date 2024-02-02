using Microsoft.EntityFrameworkCore;
using BookCatalogApi.Data;
using BookCatalogApi.Repositories;
using MediatR;
using Microsoft.OpenApi.Models;
using BookCatalogApi.Request.Books.Commands.CreateBook;
using BookCatalogApi.Commands.CreateBook;

namespace BookCatalogApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase("InMemoryDatabase"));

            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<IRequestHandler<CreateBookCommand, int>, CreateBookCommandHandler>();
            services.AddMediatR(typeof(Startup).Assembly);
            services.AddMemoryCache();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Book Catalog API", Version = "v1" });
            });

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Book Catalog API v1"));
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
