namespace Wallet.Application.Users.Registration
{
    public record RegisterUserResponse(
        Guid Id,
        string FirstName,
        string OtherNames,
        string Email,
        DateTimeOffset RegisteredAt
    );
}
