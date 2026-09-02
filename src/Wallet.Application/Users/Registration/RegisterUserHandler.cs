using FluentValidation;
using Wallet.Application.Common.Interfaces;
using Wallet.Application.Common.Results;
using Wallet.Domain.Entities;
using Wallet.Domain.ValueObjects;

namespace Wallet.Application.Users.Registration
{
    public class RegisterUserHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<RegisterUserCommand> _validator;

        public RegisterUserHandler(
            IUserRepository userRepository, 
            IPasswordHasher passwordHasher,
            IUnitOfWork unitOfWork,
            IValidator<RegisterUserCommand> validator)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<Result<RegisterUserResponse>> Handle(
            RegisterUserCommand command, 
            CancellationToken cancellationToken)
        {
            var validationResult = await _validator
                .ValidateAsync(
                    command, 
                    cancellationToken);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .Select(e => e.ErrorMessage)
                    .ToArray();

                return Result<RegisterUserResponse>
                    .Failure(errors);
            }

            var dateOfBirth = DateOfBirth.Create(command.DateOfBirth);
            var email = Email.Create(command.Email);
            var mobileNumber = MobileNumber.Create(command.MobileNumber);

            // Check if the user already exists
            var existingUser = await _userRepository
                .ExistsByEmailAsync(email, cancellationToken);

            if (existingUser)
                return Result<RegisterUserResponse>
                    .Failure("User already exists.");

            if (command.Password != command.ConfirmPassword)
                return Result<RegisterUserResponse>
                    .Failure("Passwords do not match.");

            // Hash the password
            var hashedPassword = _passwordHasher.Hash(command.Password);

            // Create a new user entity
            var newUser = User.Create(
                command.FirstName,
                command.OtherNames,
                dateOfBirth,
                email,
                mobileNumber,
                hashedPassword);

            // Save the new user to the repository
            _userRepository.Add(newUser);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var response = new RegisterUserResponse(
                newUser.Id,
                newUser.FirstName,
                newUser.OtherNames,
                newUser.Email.ToString(),
                newUser.RegisteredAt);

            return Result<RegisterUserResponse>.Success(response);
        }
    }
}
