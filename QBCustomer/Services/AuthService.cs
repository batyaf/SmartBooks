using Intuit.Ipp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using QBCustomer.Models;
using QBCustomer.Utils;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;

namespace QBCustomer.Services
{
    public class AuthService
    {
        private SmartBooksContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;


        public AuthService(SmartBooksContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
            _secretKey = AppConfig.AppSettings["Jwt:SecretKey"];
            _issuer = AppConfig.AppSettings["Jwt:Issuer"];
            _audience = AppConfig.AppSettings["Jwt:Audience"];
        }

        public async Task<SBUser> RegisterUser(UserRegisterRequest request)
        {
            var user = new IdentityUser { UserName = request.Username, Email = request.Email };
            var result = await _userManager.CreateAsync(user, request.Password);
            //Todo: throw  
            var sbUser = await AddNewUser(user.Id);
            return sbUser;
        }
        public async Task<SBUser> AddNewUser(string userId)
        {
            SBUser sbUser = new SBUser();
            sbUser.UserId = userId;
            _db.sbUsers.Add(sbUser);
            await _db.SaveChangesAsync();
            return sbUser;
        }

        public async Task<string> LoginUser(UserLoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.Username);

            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
            {
                throw new Exception("Invalid credentials");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                  new Claim(ClaimTypes.Name, user.UserName),
                  new Claim(ClaimTypes.NameIdentifier, user.Id)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _issuer,
                Audience = _audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


       


      

    }
}
