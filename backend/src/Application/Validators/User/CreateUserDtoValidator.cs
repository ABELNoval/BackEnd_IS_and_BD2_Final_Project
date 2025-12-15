using System;
using FluentValidation;
using Application.DTOs.User;

namespace Application.Validators.User
{
    /// <summary>
    /// Validator for CreateUserDto. Performs basic, uniqueness, and existence validations for user creation.
    /// </summary>
    public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
    {
        private readonly Domain.Interfaces.IUserRepository _userRepo;
        private readonly Domain.Interfaces.IRoleRepository _roleRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateUserDtoValidator"/> class.
        /// </summary>
        /// <param name="userRepo">The user repository for uniqueness checks.</param>
        /// <param name="roleRepo">The role repository for existence checks.</param>
        public CreateUserDtoValidator(Domain.Interfaces.IUserRepository userRepo, Domain.Interfaces.IRoleRepository roleRepo)
        {
            _userRepo = userRepo;
            _roleRepo = roleRepo;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("User name is required.")
                .MaximumLength(100).WithMessage("User name cannot exceed 100 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email address is required.")
                .MaximumLength(150).WithMessage("Email cannot exceed 150 characters.")
                .MustAsync(EmailIsUnique).WithMessage("A user with this email already exists.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters.");

            RuleFor(x => x.RoleId)
                .NotEmpty().WithMessage("Role is required.")
                .MustAsync(RoleExists).WithMessage("The specified role does not exist.");
        }

        /// <summary>
        /// Checks if the user email is unique in the database.
        /// </summary>
        private async System.Threading.Tasks.Task<bool> EmailIsUnique(string email, System.Threading.CancellationToken ct)
        {
            return !await _userRepo.ExistsByEmailAsync(email, ct);
        }

        /// <summary>
        /// Checks if the role exists in the database.
        /// </summary>
        private async System.Threading.Tasks.Task<bool> RoleExists(int roleId, System.Threading.CancellationToken ct)
        {
            var roles = await _roleRepo.GetByIdsAsync(new[] { roleId }, ct);
            return roles != null && roles.GetEnumerator().MoveNext();
        }
    }
}