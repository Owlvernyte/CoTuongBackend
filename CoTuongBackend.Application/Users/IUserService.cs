namespace CoTuongBackend.Application.Users;

public interface IUserService
{
    Task<AccountDTO> Register(string userName, string email, string password, string confirmPassword);
    Task<AccountDTO> Login(string userNameOrEmail, string password);
}
