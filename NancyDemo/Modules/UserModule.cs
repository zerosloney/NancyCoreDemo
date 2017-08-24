using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Validation;
using NancyDemo.ViewModels;
using NancyDemo.Models;

namespace NancyDemo.Modules
{
    public sealed class UserModule : NancyModule
    {
        public UserModule() : base("users")
        {
            Get("/", args => Response.AsJson(new {code = 1, msg = "成功", data = GetList()}));

    
            Post("/", args =>
            {
                var userReqModel = this.Bind<UserReqModel>();
                var results = this.Validate(userReqModel);
                if (!results.IsValid)
                {
                    return Negotiate.WithModel(results).WithStatusCode(HttpStatusCode.BadRequest);
                }
                return Response.AsJson(userReqModel);
            });
        }

        private static IEnumerable<UserViewModel> GetList()
        {
            return Enumerable.Range(1, 10)
                .Select(x => new UserViewModel() {UserId = x, UserName = x.ToString()});
        }
    }
}