using Moq;
using Wallet.Application.Common.Interfaces;
using Wallet.Application.Users.Registration;
using Wallet.Domain.Entities;
using Wallet.Domain.ValueObjects;

namespace Wallet.Application.Tests.Users.Registration
{
    public class RegisterUserHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<IPasswordHasher> _passwordHasher;
        private readonly Mock<IUnitOfWork> _unitOfWork;

        private readonly RegisterUserHandler _handler;

        public RegisterUserHandlerTests()
        {
            _userRepository = new Mock<IUserRepository>();
            _passwordHasher = new Mock<IPasswordHasher>();
            _unitOfWork = new Mock<IUnitOfWork>();

            _handler = new RegisterUserHandler(
                _userRepository.Object,
                _passwordHasher.Object,
                _unitOfWork.Object);
        }

        [Fact]
        public async Task Handle_ShouldRegisterUser_WhenCommandIsValid()
        {
            // Arrange
            var command = new RegisterUserCommand(
                "Amos",
                "Daniel",
                "1999-05-16",
                "amos.daniel@example.com",
                "07512345674",
                "Password123!",
                "Password123!"
            );

            _userRepository.Setup(repo => repo.ExistsByEmailAsync(
                    It.IsAny<Email>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _passwordHasher.Setup(hasher => hasher.Hash(
                    It.IsAny<string>()))
                .Returns("hashedPassword");

            // Act
            var result = await _handler.Handle(
                command,
                CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.NotEqual(Guid.Empty, result.Value.Id);

            Assert.Equal(command.FirstName, result.Value.FirstName);
            Assert.Equal(command.Email, result.Value.Email);

            _userRepository.Verify(repo => repo.Add(
                It.IsAny<User>()), 
                Times.Once);

            _passwordHasher.Verify(hasher => hasher.Hash(
                It.IsAny<string>()), 
                Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenEmailAlreadyExists()
        {
            // Arrange
            var command = new RegisterUserCommand(
                "Amos",
                "Daniel",
                "1999-05-16",
                "amos.daniel@example.com",
                "07512345674",
                "Password123!",
                "Password123!"
            );

            _userRepository.Setup(repo => repo.ExistsByEmailAsync(
                    It.IsAny<Email>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(
                command,
                CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("User already exists.", result.ErrorMessage);
            Assert.Null(result.Value);

            _userRepository.Verify(repo => repo.Add(
                It.IsAny<User>()), 
                Times.Never);

            _passwordHasher.Verify(hasher => hasher.Hash(
                It.IsAny<string>()),
                Times.Never);
        }
    }
}