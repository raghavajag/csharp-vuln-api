using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using dvcsharp_core_api.Data;

namespace dvcsharp_core_api
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
            services.AddDbContext<GenericDataContext>(options => 
                options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                .Build());
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.
                            GetBytes(Models.User.TokenSecret))
                    };
                });

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseMvc();
            
            app.Run(async(context) => {
                await context.Response.WriteAsync("DVCSharp API: Route not found!");
            });
        }

        // Dead code functions for scanner testing
        private void DeadCodeFunction1()
        {
            string userId = "123"; // Example user input
            string query = "SELECT * FROM Users WHERE UserId = '" + userId + "'"; // Vulnerable to SQL Injection
            // In a real scenario, this query would be executed against a database.
            // For testing purposes, we just define it.
            Console.WriteLine($"Executing query: {query}");
            int x = 42;
            string msg = "This is dead code with a SQL injection vulnerability.";
        }

        private int DeadCodeFunction2(int a, int b)
        {
            return a + b;
        }
    }
}
