using Microsoft.AspNetCore.Builder;
using Nancy.Owin;
using NancyDemo.Bootstrappers;

namespace NancyDemo
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app)
        {
            app.UseOwin(x => x.UseNancy(action: y =>
            {
                y.Bootstrapper = new ApiAutofacBootstrapper();
            }));
        }
    }
}