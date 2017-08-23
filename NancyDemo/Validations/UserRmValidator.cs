using FluentValidation;
using  NancyDemo.Models;

namespace NancyDemo.Validations
{
    public class UserRmValidator:AbstractValidator<UserReqModel>
    {
        public UserRmValidator()
        {
            RuleFor(request => request.Name).NotEmpty().WithMessage("姓名不能为空");
            RuleFor(request => request.Age).ExclusiveBetween(20, 50).WithMessage("年龄必须在20-50");
        }

        
    }
}