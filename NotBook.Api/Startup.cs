using System.Collections.Generic;
using System.Text;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NotBook.Api.Filters;
using NotBook.Core.Constants;
using NotBook.Core.Settings;
using NotBook.Data;
using NotBook.Service;
using Swashbuckle.AspNetCore.Swagger;

namespace NotBook.Api
{
    public class Startup
    {
        private IConfiguration Configuration { get; }
        
        private IHostingEnvironment Environment { get; }
        
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("MySQLProductionConnection");
            if (Environment.IsDevelopment())
            {
                connectionString = Configuration.GetConnectionString("MySQLDevelopmentConnection");
            }
            
            services.AddDbContext<Context>(options => options.UseMySql(connectionString));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(Constants.Version, new Info {Title = "NotBook", Version = Constants.Version});
                
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    In = "header",
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = "apiKey"
                });

                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    { "Bearer", new string[] { } }
                });
            });

            // configure strongly typed settings objects
            var settingsSection = Configuration.GetSection("Settings");
            services.Configure<Settings>(settingsSection);
            services.Configure<FormOptions>(options => options.MultipartBodyLengthLimit = long.MaxValue); // or other given limit

            
            // configure cors
            // TODO: For production change allowed origins
            services.AddCors();

            // configure jwt authentication
            var settings = settingsSection.Get<Settings>();
            var key = Encoding.ASCII.GetBytes(settings.Secret);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateLifetime = true,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    };
                });
            
            services.AddMvc(options =>
                {   
                    // Filter bad requests
                    options.Filters.Add(typeof(ModelStateValidationFilter));
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddFluentValidation(fvc => fvc.RegisterValidatorsFromAssemblyContaining<Startup>());
            
            services.ConfigureDataServices();
            services.ConfigureServiceServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                // Enable middleware to serve generated Swagger as a JSON endpoint.
                app.UseSwagger();

                // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "NotBook Api" + Constants.Version);
                });
            }
            else
                app.UseHsts();

            app.UseAuthentication();
            
            // Enable cors
            app.UseCors(builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
            );
                        
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}