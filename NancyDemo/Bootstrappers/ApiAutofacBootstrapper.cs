using Autofac;
using Nancy;
using Nancy.Responses;
using Nancy.Bootstrapper;
using Nancy.Bootstrappers.Autofac;
using Nancy.Authentication.Stateless;
using System;
using System.Security.Claims;
using System.Security.Principal;


namespace NancyDemo.Bootstrappers
{

    public class ApiAutofacBootstrapper : AutofacNancyBootstrapper
    {
        private const string SecretKey = "aba3adcfef78b0c10e2acb59da98b75f";

        protected override void RequestStartup(ILifetimeScope container, IPipelines pipelines, NancyContext context)
        {
            var statelessAuthConfiguration = new StatelessAuthenticationConfiguration(ctx =>
            {
                var env = ctx.Environment;
                var jwtToken = ctx.Request.Headers.Authorization;
                try
                {
                    if (string.IsNullOrEmpty(jwtToken))
                    {
                        ctx.Response = new JsonResponse(new { code = 40001, msg = "无权限" }, new DefaultJsonSerializer(env), env);
                        
                        return null;
                    }
                    var payload = Jose.JWT.Decode<JwtToken>(jwtToken, SecretKey);
                    var tokenExpires = DateTime.FromBinary(payload.Exp);
                    if (tokenExpires > DateTime.UtcNow)
                    {
                        return new ClaimsPrincipal(new GenericIdentity(payload.Sub));
                    }
                    ctx.Response = new JsonResponse(new { code = 40002, msg = "已过期" }, new DefaultJsonSerializer(env), env);
                    return null;
                }
                catch (Exception e)
                {
                    ctx.Response = new JsonResponse(new { code = 50001, msg = e.Message }, new DefaultJsonSerializer(env), env);
                    return null;
                }
            });
            StatelessAuthentication.Enable(pipelines, statelessAuthConfiguration);

            var b = statelessAuthConfiguration.IsValid;
            //var path = context.Request.Path;
            //if (path == "/") return;




        }

        protected override void ConfigureApplicationContainer(ILifetimeScope existingContainer)
        {
            base.ConfigureApplicationContainer(existingContainer);
        }
    }

    public class JwtToken
    {
        public string Sub;
        public long Exp;
    }

   
}