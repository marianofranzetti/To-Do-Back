using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ToDoAPI;
using ToDoAPI.Constantes;
using ToDoAPI.DTOs;
using ToDoAPI.Servicios;
using ToDoAPI.Utilidades;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ToDoAPI.Controllers
{
    [Route("api/usuarios")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = Roles.esAdmin)]
    public class UsuariosController: ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IConfiguration configuration;
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly ILogger<TareaServicio> _logger;

        public UsuariosController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,
            IConfiguration configuration, ApplicationDbContext context, IMapper mapper, ILogger<TareaServicio> logger)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            this.context = context;
            this.mapper = mapper;
            _logger = logger;
        }

        [HttpGet("ListadoUsuarios")]
        public async Task<ActionResult<List<UsuarioDTO>>> ListadoUsuarios([FromQuery] PaginacionDTO paginacionDTO)
        {
            try
            {
                _logger.LogInformation("obteniendo listado de usuarios");

                var queryable = context.Users.AsQueryable();
                await HttpContext.InsertarParametrosPaginacionEnCabecera(queryable);
                var usuarios = await queryable.ProjectTo<UsuarioDTO>(mapper.ConfigurationProvider)
                    .OrderBy(x => x.Email).Paginar(paginacionDTO).ToListAsync();

                _logger.LogInformation($"obteniendo listado usuarios {usuarios.Count}");

                return usuarios;
            }
            catch (Exception e)
            {
                _logger.LogError($"error obteniendo listados de usuarios {e}");
                throw new InvalidOperationException($"error obteniendo listados de usuarios {e}");
            }

        }

        [HttpPost("registrar")]
        [AllowAnonymous]
        public async Task<ActionResult<RespuestaAutenticacionDTO>> Registrar(CredencialesUsuarioDTO credencialesUsuarioDTO)
        {
            try
            {
                _logger.LogInformation("registrando usuario");

                var usuario = new IdentityUser
                {
                    Email = credencialesUsuarioDTO.Email,
                    UserName = credencialesUsuarioDTO.Email
                };

                var resultado = await userManager.CreateAsync(usuario, credencialesUsuarioDTO.Password);

                if (resultado.Succeeded)
                {
                    _logger.LogInformation("registrando usuario con exito");
                    return await ConstruirToken(usuario);
                }

                _logger.LogError("error registrando usuario");
                return BadRequest(resultado.Errors);
            }
            catch (Exception e)
            {
                _logger.LogError($"error obteniendo listados de usuarios {e}");
                throw new InvalidOperationException($"error obteniendo listados de usuarios {e}");
            }

        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<RespuestaAutenticacionDTO>> Login(CredencialesUsuarioDTO credencialesUsuarioDTO)
        {
            try
            {
                _logger.LogInformation("Login usuario");
                var usuario = await userManager.FindByEmailAsync(credencialesUsuarioDTO.Email);

                if (usuario is null)
                {
                    var errores = ConstruirLoginIncorrecto();
                    return BadRequest(errores);
                }

                var resultado = await signInManager.CheckPasswordSignInAsync(usuario,
                    credencialesUsuarioDTO.Password, lockoutOnFailure: false);

                if (resultado.Succeeded)
                {
                    _logger.LogInformation("Login exitoso");
                    return await ConstruirToken(usuario);
                }
                else
                {
                    var errores = ConstruirLoginIncorrecto();
                    _logger.LogError($"Login erroneo {errores.First()}");
                    return BadRequest(errores);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"error realizando login {e}");
                throw new InvalidOperationException($"error realizando login {e}");
            }

        }

        [HttpPost("HacerAdmin")]
        public async Task<IActionResult> HacerAdmin(EditarClaimDTO editarClaimDTO)
        {
            try
            {
                _logger.LogInformation("creando nuevo admin");
                var usuario = await userManager.FindByEmailAsync(editarClaimDTO.Email);

                if (usuario is null)
                {
                    return NotFound();
                }

                await userManager.AddClaimAsync(usuario, new Claim("esadmin", "true"));
                _logger.LogInformation($"nuevo admin asignado con exito {editarClaimDTO.Email}");  
                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError($"error creando nuevo admin {e}");
                throw new InvalidOperationException($"error creando nuevo admin {e}");
            }

        }

        [HttpPost("RemoverAdmin")]
        public async Task<IActionResult> RemoverAdmin(EditarClaimDTO editarClaimDTO)
        {
            try
            {
                _logger.LogInformation("removiendo admin");

                var usuario = await userManager.FindByEmailAsync(editarClaimDTO.Email);

                if (usuario is null)
                {
                    return NotFound();
                }

                await userManager.RemoveClaimAsync(usuario, new Claim("esadmin", "true"));

                _logger.LogInformation($"admin {editarClaimDTO.Email} removido con exito");

                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError($"error removiendo admin {e}");
                throw new InvalidOperationException($"error removiendo admin {e}");
            }

        }

        private IEnumerable<IdentityError> ConstruirLoginIncorrecto()
        {
            try
            {
                _logger.LogInformation("construyendo login incorrecto");
                var identityError = new IdentityError() { Description = "Login incorrecto" };
                var errores = new List<IdentityError>();
                errores.Add(identityError);
                return errores;
            }
            catch (Exception e)
            {
                _logger.LogError($"error construyendo login incorrecto {e}");
                throw new InvalidOperationException($"error construyendo login incorrecto {e}");
            }

        }

        private async Task<RespuestaAutenticacionDTO> ConstruirToken(IdentityUser identityUser)
        {
            try
            {
                _logger.LogInformation("construyendo token");

                var claims = new List<Claim>{new Claim("email", identityUser.Email!) };

                var claimsDB = await userManager.GetClaimsAsync(identityUser);

                claims.AddRange(claimsDB);

                var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["llavejwt"]!));
                var creds = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);

                var expiracion = DateTime.UtcNow.AddYears(1);

                var tokenDeSeguridad = new JwtSecurityToken(issuer: null, audience: null, claims: claims,
                    expires: expiracion, signingCredentials: creds);

                var token = new JwtSecurityTokenHandler().WriteToken(tokenDeSeguridad);

                return new RespuestaAutenticacionDTO
                {
                    Token = token,
                    Expiracion = expiracion
                };
            }
            catch (Exception e)
            {
                _logger.LogError($"error construyendo token {e}");
                throw new InvalidOperationException($"error construyendo token {e}");
            }

        }
    }
}
