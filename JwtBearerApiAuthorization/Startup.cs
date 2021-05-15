using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using JwtBearerApiAuthorization.Config;
using JwtBearerApiAuthorization.Data;
using JwtBearerApiAuthorization.Model.Authorization.AutoMapperConfig;
using JwtBearerApiAuthorization.Repository;

using System.Text;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Mvc;

namespace JwtBearerApiAuthorization
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
            services.AddCors();

            services.AddDbContext<ApplicationDbContext>(config => config.UseInMemoryDatabase("test"));

            services.AddScoped<IDrinkRepository, DrinkRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddSingleton<JwtSettings>();

            services.AddAutoMapper(typeof(UserMapping));

            var appSettingsSection = Configuration.GetSection("AppSettings");
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services.Configure<AppSettings>(appSettingsSection);
            services.AddAuthentication(x =>
           {
               x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
               x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
           })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });


            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "JwtBearerApiAuthorization", Version = "v1" });
            });

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, SwaggerSettings>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "JwtBearerApiAuthorization v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(x =>
            x.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
