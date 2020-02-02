using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ServerManager.Rest.Data;
using ServerManager.Rest.Database;
using ServerManager.Rest.Database.Sqlite;
using ServerManager.Rest.Logging;
using ServerManager.Rest.Utility;

namespace ServerManager.Rest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private LoggerFactory loggerFactory;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddMvcCore(options => options.EnableEndpointRouting = false);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "Minecraft Server Manager API",
                        Description = "This is the list of actions you can take on the management api",
                        Version = "v1"
                    });

                //c.IncludeXmlComments($"ServerManager.Rest.xml");
            });

            services.AddTransient<IDataAccessLayer, DataAccessLayer>();
            services.AddTransient<ILinkGenerator, LinkGenerator>();
            services.AddTransient<IDbConnectionFactory, SqliteConnectionFactory>();
            services.AddTransient<IDbCommandFactory, SqliteCommandFactory>();
            services.AddTransient<ICommandExecutor, DatabaseCommandExecutor>();
            services.AddTransient<IDataMapper, DataMapper>();

            var loggerConfig = LoggerConfiguration.Default;

            loggerFactory = new Logging.LoggerFactory(loggerConfig);

            services.AddSingleton<Logging.ILoggerFactory, Logging.LoggerFactory>((provider) =>
            {
                return loggerFactory;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                c.RoutePrefix = string.Empty;
            });

            app.UseMvc();

            var dbStartup = new DatabaseStartupRoutine(Configuration, loggerFactory);

            var startupTask = dbStartup.Start();
            startupTask.Wait();
        }
    }
}
