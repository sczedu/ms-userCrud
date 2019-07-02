using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ms_userCrud._01Api.Model;
using ms_userCrud._01Api.Validator;
using ms_userCrud._02Service;
using ms_userCrud._02Service.Security;
using ms_userCrud._03Data;
using ms_userCrud._03Data.Entity;
using ms_userCrud._05Helper;
using System;

namespace ms_userCrud
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
            //DbConfiguration
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddDbContext<DBContext>(opt => opt.UseInMemoryDatabase("DataUserMemory"));
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IAuthHelper, AuthHelper>();
            services.AddScoped<UserAuthValidator, UserAuthValidator>();
            services.AddScoped<IHelper, Helper>();
            services.AddScoped<IUserContext<UserDTO>, UserContext<UserDTO>>();
            services.AddScoped<UserValidator, UserValidator>();
            services.AddScoped<DBContext, DBContext>();

            //tokenGeneration
            var signingConfigurations = new SigningConfigurations();
            services.AddSingleton(signingConfigurations);
            var tokenConfigurations = new TokenConfigurations();
            new ConfigureFromConfigurationOptions<TokenConfigurations>(
                Configuration.GetSection("TokenConfigurations"))
                    .Configure(tokenConfigurations);
            services.AddSingleton(tokenConfigurations);
            services.AddJwtSecurity(
                signingConfigurations, tokenConfigurations);

            services.AddCors();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            Environment.SetEnvironmentVariable("CryptographyKey", "CryptographySecretKeyValue");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
