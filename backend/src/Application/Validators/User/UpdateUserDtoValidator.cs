using System;
using FluentValidation;
using Application.DTOs.User;

namespace Application.Validators.User
{
    /// <summary>
    /// Validator for UpdateUserDto. Performs existence and uniqueness validations for user update.
    /// </summary>
    public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
    {
        private readonly Domain.Interfaces.IUserRepository _userRepo;
        private readonly Domain.Interfaces.IRoleRepository _roleRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateUserDtoValidator"/> class.
        /// </summary>
        /// <param name="userRepo">The user repository for existence and uniqueness checks.</param>
        /// <param name="roleRepo">The role repository for existence checks.</param>
        public UpdateUserDtoValidator(Domain.Interfaces.IUserRepository userRepo, Domain.Interfaces.IRoleRepository roleRepo)
        {
            _userRepo = userRepo;
            _roleRepo = roleRepo;

            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("User ID is required.")
                .NotEqual(Guid.Empty).WithMessage("User ID cannot be empty.")
                .MustAsync(UserExists).WithMessage("The specified user does not exist.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("User name is required.")
                .MaximumLength(100).WithMessage("User name cannot exceed 100 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email address is required.")
                .MaximumLength(150).WithMessage("Email cannot exceed 150 characters.")
                .MustAsync(EmailIsUnique).WithMessage("A user with this email already exists.");

            RuleFor(x => x.RoleId)
                .NotEmpty().WithMessage("Role is required.")
                .MustAsync(RoleExists).WithMessage("The specified role does not exist.");
        }

        /// <summary>
        /// Checks if the user exists in the database.
        /// </summary>
        private async System.Threading.Tasks.Task<bool> UserExists(System.Guid id, System.Threading.CancellationToken ct)
        {
            return await _userRepo.GetByIdAsync(id, ct) != null;
        }

        /// <summary>
        /// Checks if the user email is unique in the database (excluding the current ID).
        /// </summary>
        private async System.Threading.Tasks.Task<bool> EmailIsUnique(UpdateUserDto dto, string email, System.Threading.CancellationToken ct)
        {
            var user = await _userRepo.GetByEmailAsync(email, ct);
            return user == null || user.Id == dto.Id;
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