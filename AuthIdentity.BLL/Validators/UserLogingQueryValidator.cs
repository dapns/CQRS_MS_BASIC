using AuthIdentity.BLL.Queries;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AuthIdentity.BLL.Validators
{
    public class UserLogingQueryValidator : AbstractValidator<UserLoginQuery>
    {
        public UserLogingQueryValidator()
        {
            RuleFor(c => c.UserLoginId).NotEmpty();
            RuleFor(c => c.UserLoginId).MustAsync(BeValidUserLoginIdAsync).WithMessage("Please enter valid emailid or phone number");
        }
        private Task<bool> BeValidUserLoginIdAsync(string loginid, CancellationToken cancellationToken)
        {
            var isPhone = Regex.IsMatch(loginid, @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", RegexOptions.IgnoreCase);
            bool isEmail = Regex.IsMatch(loginid, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
            if (isPhone || isEmail)
                return Task.FromResult(true);
            else
                return Task.FromResult(false);
        }
    }
}
