using ms_userCrud.Api.Model;

namespace ms_userCrud.Helper
{
    public interface IAuthHelper
    {
        string GetHash(string password);
        Token GenerateToken(User user);
    }
}
