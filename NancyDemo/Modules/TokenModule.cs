using System;
using System.Text;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Validation;
using NancyDemo.Models;

namespace NancyDemo.Modules
{
    public sealed class TokenModule : NancyModule
    {
        public TokenModule() : base("token")
        {
            Post("/", args =>
            {
                var userTokenReqModel = this.Bind<UserTokenReqModel>();
                var results = this.Validate(userTokenReqModel);
                if (!results.IsValid)
                {
                    return Negotiate.WithModel(results).WithStatusCode(HttpStatusCode.BadRequest);
                }
                var jwtToken = new JwtToken()
                {
                    UserId = 1,
                    Sub = userTokenReqModel.UserName,
                    Exp = DateTime.UtcNow.AddHours(2).ToBinary()
                };
                var token = Jose.JWT.Encode(jwtToken, Encoding.UTF8.GetBytes(AppSettings.SecretKey), Jose.JwsAlgorithm.HS256);
                return Response.AsJson(new {access_token = token});

            });
        }
    }
}