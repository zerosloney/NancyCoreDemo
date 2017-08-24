using Autofac;
using Nancy;
using Nancy.Responses;
using Nancy.Bootstrapper;
using Nancy.Bootstrappers.Autofac;
using System;
using System.Text;
using System.Security.Claims;
using NancyDemo.Principals;

namespace NancyDemo.Bootstrappers
{
    public class ApiAutofacBootstrapper : AutofacNancyBootstrapper
    {
        private readonly Func<NancyContext, Response> _delegateResponse = (ctx) =>
        {   
            var env = ctx.Environment;
            var jwtToken = ctx.Request.Headers.Authorization;
            try
            {
                if (string.IsNullOrEmpty(jwtToken))
                {
                    ctx.Response = new JsonResponse(new {code = 40001, msg = "无权限"}, new DefaultJsonSerializer(env),
                        env);
                }
                else
                {
                    var payload = Jose.JWT.Decode<JwtToken>(jwtToken, Encoding.UTF8.GetBytes(AppSettings.SecretKey));
                    var tokenExpires = DateTime.FromBinary(payload.Exp);
                    if (tokenExpires > DateTime.UtcNow)
                    {
                        ctx.CurrentUser = new ClaimsPrincipal(new AuthenticatedIdentity(payload.UserId, payload.Sub));
                        return null;
                    }
                    ctx.Response = new JsonResponse(new {code = 40002, msg = "已过期"}, new DefaultJsonSerializer(env),
                        env);
                }
            }
            catch (Exception e)
            {
                ctx.Response = new JsonResponse(new {code = 50001, msg = e.Message}, new DefaultJsonSerializer(env),
                    env);
            }
            return ctx.Response;
        };

        protected override void RequestStartup(ILifetimeScope container, IPipelines pipelines, NancyContext context)
        {
            var path = context.Request.Path;
            if (path == "/" || path=="/token") return;

            pipelines.BeforeRequest += _delegateResponse;
        }

        protected override void ConfigureApplicationContainer(ILifetimeScope existingContainer)
        {
            base.ConfigureApplicationContainer(existingContainer);
        }
    }

    
}