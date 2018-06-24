using CentennialTalk.Persistence;
using CentennialTalk.Persistence.Contracts;
using CentennialTalk.Persistence.Repositories;
using CentennialTalk.Service.Contracts;
using CentennialTalk.Service.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        private void AddServicePackages(IServiceCollection services)
        {
            services.AddScoped<IChatService, ChatService>();
        }

        private void AddRepositoryPackages(IServiceCollection services)
        {
            services.AddScoped<IChatRepository, ChatRepository>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            Application = app;

            InitDatabase();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseSignalR(routes =>
            {
                routes.MapHub<ChatHub>("/discussion");
            });

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