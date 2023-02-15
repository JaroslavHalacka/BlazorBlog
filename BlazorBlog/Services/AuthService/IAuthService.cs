namespace BlazorBlog.Services.AuthService
{
    public interface IAuthService
    {
        Task<ServiceResponse<bool>> Register(UserRegisterDto user);
        Task<ServiceResponse<string>> Login(string email, string password);
        ServiceResponse<int?> VerifyJWT(string jwtToken);
        Task<bool> UserExists(string email);

    }
}
