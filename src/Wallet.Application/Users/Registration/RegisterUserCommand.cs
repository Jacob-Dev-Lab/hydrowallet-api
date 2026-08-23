namespace Wallet.Application.Users.Registration
{
    public record RegisterUserCommand(
        string FirstName,
        string OtherNames,
        string DateOfBirth,
        string Email,
        string MobileNumber,
        string Password,
        string ConfirmPassword
    );
}
