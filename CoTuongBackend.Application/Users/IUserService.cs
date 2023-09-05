namespace CoTuongBackend.Application.Users;

public interface IUserService
{
    Task<AccountDTO> Register(string userName, string email, string password, string confirmPassword);
    Task<AccountDTO> Login(string userNameOrEmail, string password);
    Task<AccountDTO> ChangePassword(string userNameOrEmail, string newPassword, string confirmPassword, string oldPassword);
}
