using CrewDemandService.Api.CommandHandler;
using CrewDemandService.Api.QueryHandler;
using CrewDemandService.Infrastructure.Data;
using CrewDemandService.Infrastructure.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CrewDemandService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<PilotDbContext>(options =>
                 options.UseInMemoryDatabase(databaseName: "CrewDemandDb"));
            services.AddScoped<IPilotRepository, PilotRepository>();
            services.AddScoped<IWorkDayRepository, WorkDayRepository>();
            services.AddScoped<IPilotBookingRepository, PilotBookingRepository>();
            services.AddScoped<IPilotQueryHandler, PilotQueryHandler>();
            services.AddScoped<IPilotBookingCommandHandler, PilotBookingCommandHandler>();

            services.AddControllers();
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CrewDemandService", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CrewDemandService v1"));
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
