namespace CoTuongBackend.Application.Users;

public interface IUserService
{
    Task<AccountDto> Register(string userName, string email, string password, string confirmPassword);
    Task<AccountDto> Login(string userNameOrEmail, string password);
    Task<AccountDto> ChangePassword(string userNameOrEmail, string newPassword, string confirmPassword, string oldPassword);
}
