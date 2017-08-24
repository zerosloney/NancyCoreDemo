using FluentValidation;
using NancyDemo.Models;

namespace NancyDemo.Validations
{
    public class UserTokenRmValidator:AbstractValidator<UserTokenReqModel>
    {
        public UserTokenRmValidator()
        {
            RuleFor(request => request.UserName).NotEmpty().WithMessage("用户名或者手机号不能为空");
            RuleFor(request => request.Password).NotEmpty().WithMessage("密码不能为空");
        }
    }
}
