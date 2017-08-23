using Autofac;
using Nancy;
using Nancy.Responses;
using Nancy.Bootstrapper;
using Nancy.Bootstrappers.Autofac;
using System;
using System.Security.Claims;
using System.Security.Principal;

namespace NancyDemo.Bootstrappers
{
    public class ApiAutofacBootstrapper : AutofacNancyBootstrapper
    {
        private const string SecretKey = "aba3adcfef78b0c10e2acb59da98b75f";

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
                    var payload = Jose.JWT.Decode<JwtToken>(jwtToken, SecretKey);
                    var tokenExpires = DateTime.FromBinary(payload.Exp);
                    if (tokenExpires > DateTime.UtcNow)
                    {
                        ctx.CurrentUser = new ClaimsPrincipal(new GenericIdentity(payload.Sub));
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
            if (path == "/") return;

            pipelines.BeforeRequest += _delegateResponse;
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