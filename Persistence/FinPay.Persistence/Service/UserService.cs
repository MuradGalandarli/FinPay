using FinPay.Application.Service;
using FinPay.Domain.Identity;
using FinPay.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinPay.Application.DTOs.User;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using FinPay.Persistence.Context;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Security.Authentication;
using FinPay.Application.Exceptions;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Diagnostics;
using FinPay.Domain.Entity;
using FinPay.Persistence.Repositoryes.Endpoint;
using FinPay.Application.Repositoryes.Endpoint;
using FinPay.Application.DTOs;


namespace FinPay.Persistence.Service
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ILogger<UserService> _logger;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _appDbContext;
        private readonly IEndpointReadRepository _endpointReadRepository;


        public UserService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager = null, ILogger<UserService> logger = null, IConfiguration configuration = null, AppDbContext appDbContext = null, IEndpointReadRepository endpointReadRepository = null)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
            _configuration = configuration;
            _appDbContext = appDbContext;
            _endpointReadRepository = endpointReadRepository;
        }


        public async Task<bool> HasRolePermissionToEndpointAsync(string name,string code)
        {
            var userRoles = await GetRolsToUserAsync(name);
          
            if (!userRoles.Any())
                return false;

            Domain.Entity.Endpoint? endpoint = await _endpointReadRepository.Table
                     .Include(e => e.ApplicationRoles)
                     .FirstOrDefaultAsync(e => e.Code == code);

            if (endpoint == null)
                return false;

            var hasRole = false;
            var endpointRoles = endpoint.ApplicationRoles.Select(r => r.Name);

           
            foreach (var userRole in userRoles)
            {
                foreach (var endpointRole in endpointRoles)
                    if (userRole == endpointRole)
                        return true;
            }

            return false;
        }

        public async Task<string[]> GetRolsToUserAsync(string name)
        {
            var user = await _userManager.FindByNameAsync(name);
            if (user != null)
            {
                var userRole = await _userManager.GetRolesAsync(user);
                return userRole.ToArray();
            }
            return new string[] { };
        }



        public async Task AssignRoleToUser(string id, string[] roles)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id);
            if(user != null)
            {
               
                var userRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, userRoles);

                await _userManager.AddToRolesAsync(user, roles);
            }


        }

        public async Task<List<UserResponseDto>> GetAllUserAsync(int page, int size)
        {
            List<UserResponseDto> user = await _userManager.Users.Skip(page * size).Take(size).Select(x => new UserResponseDto
            {
                Id = x.Id,
                Name = x.Name,
                Email = x.Email,

            }).ToListAsync();

            return user;
        }

        public async Task<CreateUserResponse> Signup(SignupDto signup)
        {
            CreateUserResponse userResponse = new();
            try
            {
                var existingUser = await _userManager.FindByNameAsync(signup.Email);
                if (existingUser != null)
                {
                    userResponse.Message = "User already exists"; userResponse.Succeeded = false;
                    return userResponse;
                }


                if ((await _roleManager.RoleExistsAsync(Role.User)) == false)
                {
                    var roleResult = await _roleManager
    .CreateAsync(new ApplicationRole { Name = "User" ,Id = Guid.NewGuid().ToString()});

                    if (roleResult.Succeeded == false)
                    {
                        userResponse.Succeeded = roleResult.Succeeded;
                        var roleErros = roleResult.Errors.Select(e => e.Description);
                        _logger.LogError(userResponse.Message);
                        throw new SignupErrorException($"Failed to create user role. Errors : {string.Join(",", roleErros)}");
                    }
                }

                ApplicationUser user = new()
                {
                    Email = signup.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = signup.Email,
                    Name = signup.Name,
                    EmailConfirmed = true
                };


                var createUserResult = await _userManager.CreateAsync(user, signup.Password);

                if (createUserResult.Succeeded == false)
                {
                    userResponse.Succeeded = createUserResult.Succeeded;
                    var errors = createUserResult.Errors.Select(e => e.Description);
                    _logger.LogError(userResponse.Message);
                    throw new SignupErrorException($"Failed to create user. Errors: {string.Join(", ", errors)}");

                }

                var addUserToRoleResult = await _userManager.AddToRoleAsync(user: user, role: Role.User);

                if (addUserToRoleResult.Succeeded == false)
                {
                    var errors = addUserToRoleResult.Errors.Select(e => e.Description);
                    _logger.LogError(userResponse.Message);
                    throw new SignupErrorException($"Failed to add role to the user. Errors : {string.Join(",", errors)}");

                }
                userResponse.Succeeded = true;
                userResponse.Message = "User created";
                return userResponse;
            }
            catch (Exception ex)
            {
                throw new SignupErrorException("StatusCodes.Status500InternalServerError", ex);
            }
        }

        public async Task<TokenDto> Login(string email, string password, int accessTokenLifeTime, int refreshTokenLifeTime)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(email);
                if (user == null)
                {
                    throw new AuthenticationException("User with this username is not registered with us.");
                }
                bool isValidPassword = await _userManager.CheckPasswordAsync(user, password);
                if (isValidPassword == false)
                {
                    throw new AuthenticationException("401");
                }

                // creating the necessary claims
                List<Claim> authClaims = [
                        new (ClaimTypes.Name, user.UserName),
                new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),];

                var userRoles = await _userManager.GetRolesAsync(user);

                // adding roles to the claims. So that we can get the user role from the token.
                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                // generating access token
                var token = GenerateAccessToken(authClaims, accessTokenLifeTime);

                string refreshToken = GenerateRefreshToken();

                //save refreshToken with exp date in the database
                var tokenInfo = _appDbContext.TokenInfos.
                            FirstOrDefault(a => a.Username == user.UserName);

                // If tokenInfo is null for the user, create a new one
                DateTime dateTime = DateTime.UtcNow.AddMinutes(accessTokenLifeTime + refreshTokenLifeTime);
                if (tokenInfo == null)
                {
                    var ti = new Tokeninfo
                    {
                        Username = user.UserName,
                        RefreshToken = refreshToken,
                        ExpiredAt = dateTime
                    };
                    _appDbContext.TokenInfos.Add(ti);
                }
                // Else, update the refresh token and expiration

                else
                {
                    tokenInfo.RefreshToken = refreshToken;
                    tokenInfo.ExpiredAt = dateTime;
                }

                await _appDbContext.SaveChangesAsync();

                return (new TokenDto
                {
                    AccessToken = token,
                    RefreshToken = refreshToken,
                    RefreshTokenExpiresAt = dateTime

                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new AuthenticationException("401", ex);
            }

        }
        public async Task<TokenDto> RefreshTokenAsync(string accessToken, string refreshToken, int accessTokenLifeTime, int refreshTokenLifeTime)
        {
            try
            {
                var principal = GetPrincipalFromExpiredToken(accessToken);
                var username = principal.Identity.Name;

                var tokenInfo = _appDbContext.TokenInfos.SingleOrDefault(u => u.Username == username);
                if (tokenInfo == null
                    || tokenInfo.RefreshToken != refreshToken
                    || tokenInfo.ExpiredAt <= DateTime.UtcNow)
                {
                    throw new RefreshTokenException("Invalid refresh token. Please login again.");
                }

                var newAccessToken = GenerateAccessToken(principal.Claims, accessTokenLifeTime);
                var newRefreshToken = GenerateRefreshToken();
                DateTime dateToken = DateTime.UtcNow.AddMinutes(refreshTokenLifeTime + accessTokenLifeTime);
                tokenInfo.RefreshToken = newRefreshToken; // rotating the refresh token
                tokenInfo.ExpiredAt = dateToken; // rotating the refresh token
                await _appDbContext.SaveChangesAsync();

                return (new TokenDto
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken,
                    RefreshTokenExpiresAt = dateToken
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new RefreshTokenException("500", ex);
            }
        }


        public string GenerateAccessToken(IEnumerable<Claim> claims, int minutes)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            // Create a symmetric security key using the secret key from the configuration.
            var authSigningKey = new SymmetricSecurityKey
                            (Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration["JWT:ValidIssuer"],
                Audience = _configuration["JWT:ValidAudience"],
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(minutes),
                SigningCredentials = new SigningCredentials
                              (authSigningKey, SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            // Create a 32-byte array to hold cryptographically secure random bytes
            var randomNumber = new byte[32];

            // Use a cryptographically secure random number generator 
            // to fill the byte array with random values
            using var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(randomNumber);

            // Convert the random bytes to a base64 encoded string 
            return Convert.ToBase64String(randomNumber);
        }
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string accessToken)
        {
            // Define the token validation parameters used to validate the token.
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = _configuration["JWT:ValidAudience"],
                ValidIssuer = _configuration["JWT:ValidIssuer"],
                ValidateLifetime = false,
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = new SymmetricSecurityKey
                           (Encoding.UTF8.GetBytes(_configuration["JWT:secret"]))
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            // Validate the token and extract the claims principal and the security token.
            var principal = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out SecurityToken securityToken);

            // Cast the security token to a JwtSecurityToken for further validation.
            var jwtSecurityToken = securityToken as JwtSecurityToken;

            // Ensure the token is a valid JWT and uses the HmacSha256 signing algorithm.
            // If no throw new SecurityTokenException
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals
            (SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            // return the principal
            return principal;
        }

      
    }
}
