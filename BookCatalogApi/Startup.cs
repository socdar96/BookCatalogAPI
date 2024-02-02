using Microsoft.EntityFrameworkCore;
using BookCatalogApi.Data;
using BookCatalogApi.Repositories;
using BookCatalogApi.Handlers;
using MediatR;
using Microsoft.OpenApi.Models;
using BookCatalogApi.Commands;

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

            services.AddTransient<IRequestHandler<CreateBookCommand, int>, CreateBookCommandHandler>();
            services.AddScoped<IBookRepository, BookRepository>();
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
