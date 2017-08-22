using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Nancy;
using NancyDemo.ViewModels;

namespace NancyDemo.Modules
{
    public sealed class UserModule : NancyModule
    {
        public UserModule() : base("users")
        {
            Get("/", args => Response.AsJson(new {code = 1, msg = "成功", data = GetList()}));

            Post("/", args => Response.AsJson(new {code = 1, msg = "成功", data = new { username =  args.name } }));
        }

        private static IEnumerable<UserViewModel> GetList()
        {
            return Enumerable.Range(1, 10)
                .Select(x => new UserViewModel() {UserId = x, UserName = x.ToString()});
        }
    }
}