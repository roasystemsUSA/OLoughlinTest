using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Repositories;
using Services;

namespace OLaughlinTestAPI
{
    public class Program
    {
        public class Environment
        {
            public string Name { get; set; }
            public string ConnectionString { get; set; }
        }

        private static string GetConnectionStringForEnvironment(IConfiguration configuration, string environmentMode)
        {
            var environments = configuration.GetSection("Environments").Get<List<Environment>>();
            var environment = environments?.FirstOrDefault(x => x.Name == environmentMode);

            if (environment == null || string.IsNullOrEmpty(environment.ConnectionString))
            {
                throw new Exception($"Connection string for environment '{environmentMode}' not found.");
            }

            return configuration.GetConnectionString(environment.ConnectionString);
        }

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // 🗃️ DbContexts
            var environmentMode = builder.Configuration["Environment"] ?? "Development";
            var connectionString = GetConnectionStringForEnvironment(builder.Configuration, environmentMode);
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
            // 💼 Application Services and Repositories
            builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
            builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            // Services
            builder.Services.AddScoped<ICustomerService, CustomerService>();
            builder.Services.AddScoped<IAppointmentService, AppointmentService>();

            // 🌐 CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            // 📘 Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "OLaughlin.API.Demo.Local", Version = "v1" });
            });

            // 📦 Controllers
            builder.Services.AddControllers();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("AllowAll");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}