using CentennialTalk.Persistence;
using CentennialTalk.Persistence.Repositories;
using CentennialTalk.PersistenceContract;
using CentennialTalk.Service;
using CentennialTalk.ServiceContract;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using System;

namespace CentennialTalk.Main
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public IApplicationBuilder Application;

        public void ConfigureServices(IServiceCollection services)
        {
            string connString = Configuration.GetConnectionString("chatConnection");

            services.AddSignalR();

            AddServicePackages(services);
            AddRepositoryPackages(services);

            services.AddDbContext<ChatDBContext>(options =>
                options.UseSqlServer(connString));

            ConfigureIdentity(services);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                            .AddJsonOptions(y => y.SerializerSettings.ReferenceLoopHandling
                                            = Newtonsoft.Json.ReferenceLoopHandling.Ignore); ;

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        private void ConfigureIdentity(IServiceCollection services)
        {
            services.AddIdentity<IdentityUser, IdentityRole>()
                                .AddEntityFrameworkStores<ChatDBContext>()
                                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                //options.Password.RequireDigit = true;
                //options.Password.RequireLowercase = true;
                //options.Password.RequireNonAlphanumeric = true;
                //options.Password.RequireUppercase = true;
                //options.Password.RequiredLength = 6;
                //options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                options.Lockout.MaxFailedAccessAttempts = 15;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
            });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(10);

                //options.LoginPath = "/Identity/Account/Login";
                //options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                //options.SlidingExpiration = true;
            });
        }

        private void AddServicePackages(IServiceCollection services)
        {
            services.AddScoped<IChatService, ChatService>();
            services.AddScoped<IMemberService, MemberService>();
            services.AddScoped<IMessageService, MessageService>();

            services.AddScoped<IUnitOfWorkService, UnitOfWorkService>();
        }

        private void AddRepositoryPackages(IServiceCollection services)
        {
            services.AddScoped<IChatRepository, ChatRepository>();
            services.AddScoped<IMemberRepository, MemberRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory logger)
        {
            Application = app;

            InitDatabase();

            Log.Logger = new LoggerConfiguration()
                            .MinimumLevel.Information()
                            .WriteTo.RollingFile("./Logs/log-{Date}.txt", LogEventLevel.Information)
                            .CreateLogger();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                logger.AddConsole();
                logger.AddDebug(LogLevel.Information);
                logger.AddSerilog(Log.Logger);
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
                logger.AddSerilog(Log.Logger);
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseSignalR(routes =>
            {
                routes.MapHub<ChatHub>("/discussion");
            });

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }

        public void InitDatabase()
        {
            using (var serviceScope = Application.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                ChatDBContext context = serviceScope.ServiceProvider.GetRequiredService<ChatDBContext>();

                string connectionString = context.Database.GetDbConnection().ConnectionString;

                context.Database.Migrate();
            }
        }
    }
}