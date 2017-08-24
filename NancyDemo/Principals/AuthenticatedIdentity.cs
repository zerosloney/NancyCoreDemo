namespace NancyDemo.Principals
{
    public class AuthenticatedIdentity : IAuthenticatedIdentity
    {
        public AuthenticatedIdentity(long userid, string name)
        {
            Id = userid;
            Name = name;
        }

        public string AuthenticationType => "Bearer";
        public bool IsAuthenticated => true;
        public string Name { get; }
        public long Id { get; }
    }
}