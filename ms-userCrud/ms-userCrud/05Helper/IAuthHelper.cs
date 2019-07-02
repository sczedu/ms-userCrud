using ms_userCrud._01Api.Model;

namespace ms_userCrud._05Helper
{
    public interface IAuthHelper
    {
        string GetHash(string password);
        Token GenerateToken(User user);
    }
}
