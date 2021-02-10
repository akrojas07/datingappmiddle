using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using ChatsManagement.Domain.Services;
using ChatsManagement.Domain.Services.Interfaces;
using ChatsManagement.Infrastructure.Persistence.Repository;
using ChatsManagement.Infrastructure.Persistence.Repository.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ChatsManagement.Infrastructure.UserManagementAPI.Services;
using ChatsManagement.Infrastructure.UserManagementAPI.Interfaces;
using ChatsManagement.Infrastructure.MatchesManagementAPI.Interfaces;
using ChatsManagement.Infrastructure.MatchesManagementAPI.Services;
using ChatsManagement.Infrastructure.HTTPClient;

namespace ChatsManagement.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration.GetSection("Jwt:Issuer").Value,
                        ValidAudience = Configuration.GetSection("Jwt:Issuer").Value,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("Jwt:Key").Value))
                    };
                });

            services.AddControllers();
            services.AddMvc();
            services.AddCors();

            services.AddSingleton<IChatRepository, ChatRepository>();

            services.AddTransient<IChatServices, ChatServices>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IMatchServices, MatchServices>();
            services.AddTransient<IHttpClientService, HttpClientService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseRouting();
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
