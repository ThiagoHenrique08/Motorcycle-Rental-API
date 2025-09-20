using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Motorcycle_Rental_Application.DTOs.LoginDTO;
using Motorcycle_Rental_Application.Interfaces.Services;
using Motorcycle_Rental_Domain.Models;
using Motorcycle_Rental_Infrastructure.Interfaces;
using Motorcycle_Rental_Infrastructure.Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Motorcycle_Rental_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;
        private readonly IApplicationUserRoleRepository _applicationUserRoleRepository;
        private readonly IUserApplicationRepository _userApplicationRepository;
        private readonly IApplicationRoleRepository _applicationRoleRepository;

        public AuthController(ITokenService tokenService,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IConfiguration configuration, ILogger<AuthController> logger,
            IApplicationUserRoleRepository applicationUserRoleRepository,
            IUserApplicationRepository userApplicationRepository,
            IApplicationRoleRepository applicationRoleRepository
            )
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _logger = logger;
            _applicationUserRoleRepository = applicationUserRoleRepository;
            _userApplicationRepository = userApplicationRepository;
            _applicationRoleRepository = applicationRoleRepository;
        }

        [HttpPost]
        [Route("CreateRole")]
        [Authorize(Policy = "ADMIN")]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            var roleExist = await _roleManager.RoleExistsAsync(roleName);

            ApplicationRole role = new()
            {
                Name = roleName,
                NormalizedName = roleName.ToUpper(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
            };

            if (!roleExist)
            {
                var roleResult = await _roleManager.CreateAsync(role);
                if (roleResult.Succeeded)
                {
                    _logger.LogInformation(1, "Roles Added");
                    return StatusCode(StatusCodes.Status200OK, new ResponseDTO { Status = "Success", Message = $"Role {roleName} added successfully" });
                }
                else
                {
                    _logger.LogInformation(2, "Error");
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseDTO { Status = "Error", Message = $"Issue adding the new {roleName} role" });

                }
            }
            return StatusCode(StatusCodes.Status400BadRequest, new ResponseDTO { Status = "Error", Message = "Role already exist." });

        }

        [HttpPost]
        [Route("AddUserToRole")]
        [Authorize(Policy = "ADMIN")]
        public async Task<IActionResult> AddUserToRole(string email, string roleName)
        {


            var user = await _userManager.FindByEmailAsync(email);
            var role = await _roleManager.FindByNameAsync(roleName);

            if (user != null)
            {

                var roleToUserObject = new ApplicationUserRole
                {
                    UserId = user.Id,
                    User = user,
                    RoleId = role!.Id,
                    Role = role,
                };
                await _userManager.AddToRoleAsync(user, role.Name!);
                var result = await _applicationUserRoleRepository.RegisterAsync(roleToUserObject);

                if (result is not null)

                {
                    _logger.LogInformation(1, $"User {user.Email} Added to the {roleName} role");
                    return StatusCode(StatusCodes.Status200OK,
                    new ResponseDTO { Status = "Success", Message = $"User {user.Email} to the {roleName} role" });
                }

                else
                {
                    _logger.LogInformation(1, $"Error: Unable to add user {user.Email} to the {roleName} role");
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseDTO { Status = "Error", Message = $"Error: Unable to add user {user.Email} to the {roleName} role" });
                }
            }
            return BadRequest(new { error = "Unable to find user" });
        }

        [HttpPost]
        [Route("Login")]

        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username!);

            if (user is not null && await _userManager.CheckPasswordAsync(user, model.Password!))

            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName!),
                        new Claim(ClaimTypes.Email, user.Email!),
                        new Claim("id", user.UserName!),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var token = _tokenService.GenerateAccessToken(authClaims, _configuration);

                var refreshToken = _tokenService.GenerateRefreshToken();

                _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInMinutes"], out int refreshTokenValidityInMinutes);

                user.RefreshToken = refreshToken;


                user.RefreshTokenExpirytime = DateTime.UtcNow.AddMinutes(refreshTokenValidityInMinutes);

                await _userManager.UpdateAsync(user);

                return Ok(new
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    RefreshToken = refreshToken,
                    Expiration = token.ValidTo,
                    user.Id
                });
            }
            return Unauthorized();
        }
        [HttpPost]
        [Route("register-User")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            try
            {
                //CRIA UM NOVO USUARIO CASO ELE NÃO EXISTA
                //============================================================================
                var userExists = await _userManager.FindByNameAsync(model.Username!);

                if (userExists != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        new ResponseDTO { Status = "Error", Message = "User already exists!" });
                }

                ApplicationUser user = new()
                {
                    Email = model.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = model.Username,

                };
                var result = await _userManager.CreateAsync(user, model.Password!);
                var createdUser = await _userManager.FindByEmailAsync(user.Email!);
                if (!result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDTO { Status = "Error", Message = "User creation failed!" });
                }


                //CRIA A ROLE ADMIN CASO ELA NÃO EXISTA
                //============================================================================
                var roleExist = await _roleManager.RoleExistsAsync("ADMIN");

                if (!roleExist)
                {
                    await CreateRole("ADMIN");
                }
                //============================================================================


                //ADICIONA A ROLE DO USUÁRIO NA CRIAÇÃO e CRIA O RELACIONAMENTO USUARIO,EMPRESA,ROLE E TENANT
                //============================================================================
                var recoverUser = await _userManager.FindByIdAsync(createdUser!.Id!);
                var recoverRole = await _roleManager.FindByNameAsync("ADMIN");

                if (recoverUser != null)
                {
                    //var result = await _userManager.AddToRoleAsync(user, roleName);
                    var roleToUserObject = new ApplicationUserRole
                    {
                        UserId = recoverUser!.Id,
                        User = recoverUser,
                        RoleId = recoverRole!.Id,
                        Role = recoverRole,
                    };
                    await _userManager.AddToRoleAsync(recoverUser, recoverRole.Name!);                    
                    await _applicationUserRoleRepository.RegisterAsync(roleToUserObject);

                    //=========================================================================

                }
                return Ok(new ResponseDTO { Status = "Success", Message = "User created successfully!" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost]
        [Route("refresh-token")]
        [Authorize(Policy = "ADMIN")]
        public async Task<IActionResult> RefreshToken(TokenModel tokenModel)
        {
            if (tokenModel is null)
            {
                return BadRequest("Invalid client request");
            }

            string? accessToken = tokenModel.AcessToken ?? throw new ArgumentException(nameof(tokenModel));

            string? refreshToken = tokenModel.RefreshToken ?? throw new ArgumentException(nameof(tokenModel));

            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken!, _configuration);

            if (principal == null)
            {
                return BadRequest("Invalid access token/refresh token");
            }
            string username = principal.Identity!.Name!;

            var user = await _userManager.FindByNameAsync(username!);

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpirytime <= DateTime.Now)
            {
                return BadRequest("Invalid access token/refresh token");
            }

            var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims.ToList(), _configuration);

            var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;

            await _userManager.UpdateAsync(user);

            return new ObjectResult(new
            {
                accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                refreshToken = newRefreshToken
            });
        }

        [HttpPost]
        [Route("revoke/{username}")]
        [Authorize(Policy = "ADMIN")]
        public async Task<IActionResult> Revoke(string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null) return BadRequest("Invalid user name");

            user.RefreshToken = null;

            await _userManager.UpdateAsync(user);

            return NoContent();
        }

        [HttpPost]
        [Route("RevokeRoleToUser")]
        [Authorize(Policy = "ADMIN")]
        public async Task<IActionResult> RevokeRole(string name, string role)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("User name must be provided.");
            if (string.IsNullOrWhiteSpace(role))
                return BadRequest("Role name must be provided.");
            // Busca usuário
            var user = await _userManager.FindByNameAsync(name);
            if (user is null)
                return NotFound($"User with name '{name}' not found.");

            // Busca role
            var auxRole = await _roleManager.FindByNameAsync(role);
            if (auxRole is null)
                return NotFound($"Role '{role}' not found.");

            // Remove do Identity
            var identityResult = await _userManager.RemoveFromRoleAsync(user, role);

            // Se deu certo, remove também dos relacionamentos customizados
            if (identityResult.Succeeded)
            {
                var userRoles = await _applicationUserRoleRepository
                    .SearchForAsync(u => u.UserId == user.Id && u.RoleId == auxRole.Id);

                foreach (var auxUser in userRoles.ToList())
                {
                    await _applicationUserRoleRepository.DeleteAsync(auxUser);
                }

                return Ok($"Role '{role}' has been revoked from user '{user.UserName}'.");
            }

            // Se falhou no Identity, devolve erro
            return BadRequest($"Failed to revoke role '{role}' from user '{user.UserName}'.");


        }

        [HttpGet]
        [Route("getUsers")]
        [OutputCache(Duration = 400)]
        [Authorize(Policy = "ADMIN")]
        public async Task<ActionResult<IEnumerable<GetUsersDTO>>> GetUsers()
        {

            // Carrega todos os usuários
            var users = await _userApplicationRepository.ToListAsync();

            if (users is null || !users.Any())
                return NotFound("No users found.");

            var listUsersDTO = new List<GetUsersDTO>();

            foreach (var user in users)
            {
                // Pega todas as roles do Identity (já retorna nomes)
                var roles = await _userManager.GetRolesAsync(user);

                listUsersDTO.Add(new GetUsersDTO
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Roles = roles.ToList() // já vem como lista de strings
                });
            }

            return Ok(listUsersDTO);

        }

       
        
        [HttpGet]
        [Route("getUserById")]
        [OutputCache(Duration = 400)]
        [Authorize(Policy = "ADMIN")]
        public async Task<ActionResult<GetUsersDTO>> GetUserPerId([FromQuery] string UserId)
        {
            var user = await _userApplicationRepository.RecoverByAsync(u => u.Id == UserId);
            var auxUser = await _applicationUserRoleRepository.RecoverByAsync(r => r.UserId == user.Id);
            var role = await _applicationRoleRepository.RecoverByAsync(r => r.Id == auxUser!.RoleId);
            //var RoleName = await _roleManager.GetRoleNameAsync(role!);
            var roles = await _userManager.GetRolesAsync(user);

            if (user is null)
            {
                return NotFound();
            }

            var userDTO = new GetUsersDTO
            {
                UserId = user.Id,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Roles = roles.ToList()
            };

            return Ok(userDTO);
        }

    }
}

