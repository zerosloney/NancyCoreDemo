using System.Security.Principal;

namespace NancyDemo.Principals
{
    public interface IAuthenticatedIdentity: IIdentity
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        long Id { get;}
    }
}