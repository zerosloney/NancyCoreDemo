using Nancy;

namespace NancyDemo.Modules
{
    public sealed class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get("/", args => "Welcome to Api.");
        }
    }
}