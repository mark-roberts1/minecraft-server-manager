using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
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
using ServerManager.Rest.IO;
using ServerManager.Rest.Logging;
using ServerManager.Rest.Management;
using ServerManager.Rest.Utility;

namespace ServerManager.Rest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private readonly string CORS_POLICY = "AllowAny";

        public IConfiguration Configuration { get; }
        public static LoggerFactory LoggerFactory { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddCors(c =>
            {
                c.AddPolicy(CORS_POLICY, builder =>
                {
                    builder.AllowAnyOrigin();
                    //builder.WithOrigins("https://api.marksgamedomain.net", "https://marksgamedomain.net", "https://www.marksgamedomain.net", "https://localhost:3000", "https://localhost:44345");
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                    //builder.AllowCredentials();
                });
            });

            services.AddMvc(options => options.EnableEndpointRouting = false);

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

            services.AddTransient<IUserData, UserData>();
            services.AddTransient<ILinkGenerator, LinkGenerator>();
            services.AddTransient<IDbConnectionFactory, SqliteConnectionFactory>();
            services.AddTransient<IDbCommandFactory, SqliteCommandFactory>();
            services.AddTransient<ICommandExecutor, DatabaseCommandExecutor>();
            services.AddTransient<IDataMapper, DataMapper>();
            services.AddTransient<IServerManager, Management.ServerManager>();
            services.AddTransient<IServerData, ServerData>();
            services.AddTransient<IDiskOperator, DiskOperator>();

            var loggerConfig = LoggerConfiguration.Default;
            
            loggerConfig.MinFileLogLevel = NLog.LogLevel.FromString(Configuration.GetValue<string>("InternalLogger:ToFile"));
            loggerConfig.MinConsoleLogLevel = NLog.LogLevel.FromString(Configuration.GetValue<string>("InternalLogger:ToConsole"));
            loggerConfig.LogToFile = loggerConfig.MinFileLogLevel != NLog.LogLevel.Off;
            loggerConfig.LogToConsole = loggerConfig.MinConsoleLogLevel != NLog.LogLevel.Off;
            loggerConfig.LogPath = Configuration.GetValue<string>("InternalLogger:LogPath");
            loggerConfig.ArchiveDirectory = Configuration.GetValue<string>("InternalLogger:ArchivePath");
            loggerConfig.UseArchive = true;

            LoggerFactory = new LoggerFactory(loggerConfig);

            services.AddSingleton<ILoggerFactory, LoggerFactory>((provider) =>
            {
                return LoggerFactory;
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

            app.UseCors(CORS_POLICY);

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

            var dbStartup = new DatabaseStartupRoutine(Configuration, LoggerFactory);

            var startupTask = dbStartup.Start();
            startupTask.Wait();
        }
    }
}
