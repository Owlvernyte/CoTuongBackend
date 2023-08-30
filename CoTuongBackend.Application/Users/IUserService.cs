namespace CoTuongBackend.Application.Users;

public interface IUserService
{
    AccountDTO Register(string userName, string email, string password, string confirmPassword);
    AccountDTO Login(string userNameOrEmail, string password);
}
