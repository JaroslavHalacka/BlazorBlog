using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace BlazorBlog.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<ServiceResponse<bool>> Register(UserRegisterDto user)
        {
            ServiceResponse<bool> response = new ServiceResponse<bool>();

            try
            {
                if (await UserExists(user.Email))
                    throw new Exception("Email has already been used");

                if (user.Password != user.ConfirmPasword)
                    throw new Exception("The password do not match");

                User newUser = new User();
                CreatePasswordHash(user.Password, out byte[] passwordHash, out byte[] passwordSolt);
                newUser.Email= user.Email;
                newUser.PasswordHash= passwordHash;
                newUser.PasswordSalt= passwordSolt;
                newUser.DateCreated= DateTime.Now;

                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                response.Data = true;
                response.Success = true;
                response.Message = "Registration Successful";

            }
            catch (Exception ex)
            {
                response.Success= false;
                response.Message= ex.Message;
            }

            return response;
        }

        public async Task<ServiceResponse<string>> Login(string email, string password)
        {
            var response = new ServiceResponse<string>();
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());
            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found.";
            }
            else if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                response.Success = false;
                response.Message = "Wrong password.";
            }
            else
            {
                response.Data = CreateToken(user);
                response.Success = true;
                response.Message = "User was Login";
            }
            return response;
        }

        public ServiceResponse<int?> VerifyJWT(string jwtToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value!));

            try
            {
                //jwtToken = jwtToken.Replace("\"", "");
                tokenHandler.ValidateToken((jwtToken.Replace("\"", "")), new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)

                }, out SecurityToken validatedToken);
                var validjwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(validjwtToken.Claims.First(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value);

                return new ServiceResponse<int?> { Data = userId, Success = true };
            }
            catch (Exception)
            {

                return new ServiceResponse<int?> { Data = null, Success = false };
            }
        }

        public async Task<bool> UserExists(string email)
        {
            if (await _context.Users.AnyAsync(user => user.Email.ToLower().Equals(email.ToLower())))
            {
                return true;
            }
            return false;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSolt)
        {
            using (var hmac = new HMACSHA512(passwordSolt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
