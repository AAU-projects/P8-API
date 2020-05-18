using Microsoft.AspNetCore.Http;

namespace P8_API.Utility
{
    public interface IUtility
    {
        string GetToken(HttpRequest request);
        bool IsEmailValid(string email);
    }
}
