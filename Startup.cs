using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ef_key_problem.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ef_key_problem
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
            services.AddMvc();
            services.AddDbContext<EfKeyContext>(opt => opt.UseInMemoryDatabase("EfKey"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, EfKeyContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            // Test with `dotnet run` and go to `/api/values` to check data
            var scenario = 3;
            switch (scenario) {
                case 1: {
                    // SCENARIO 1: Adding only with dynamic keys:
                    context.Todos.Add(new Todo() { Name = "First" });
                    context.SaveChanges();
                    context.Todos.Add(new Todo() { Name = "Seconds" });
                    context.SaveChanges();
                    // This successfully assigns keys 1 and 2 because `Id`
                    // is recognized as a primary-key by EF convention.
                    break;
                }

                case 2: {
                    // SCENARIO 2: Adding only with static keys:
                    context.Todos.Add(new Todo() { Id = 1, Name = "First" });
                    context.SaveChanges();
                    context.Todos.Add(new Todo() { Id = 2, Name = "Seconds" });
                    context.SaveChanges();
                    // This works okay - but if I were to add an item without an `Id`
                    // next it would fail - see scenario 3.
                    break;
                }

                case 3: {
                    // SCENARIO 2: Adding only with static keys:
                    context.Todos.Add(new Todo() { Id = 1, Name = "First" });
                    context.SaveChanges();
                    context.Todos.Add(new Todo() { Name = "Seconds" });
                    context.SaveChanges();
                    // This fails before even launching.
                    break;
                }
            }
        }
    }
}
