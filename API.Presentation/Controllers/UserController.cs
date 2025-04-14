using System.Security.Cryptography;
using System.Text;
using API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Presentation.Controllers
{
    [Route("api/User")]
    [ApiController]

    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly AuthService _authService;

        public UserController(AppDbContext context, AuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password) || string.IsNullOrEmpty(request.Email))
            {
                return BadRequest("Todos los campos son obligatorios.");
            }

            if (await _context.Usuarios.AnyAsync(u => u.Username == request.Username))
            {
                return BadRequest("El nombre de usuario ya está en uso.");
            }

            if (await _context.Usuarios.AnyAsync(u => u.Email == request.Email))
            {
                return BadRequest("El email ya está registrado.");
            }

            var usuario = new Usuario
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = HashPassword(request.Password),
                Rol = "Usuario",
                FechaRegistro = DateTime.UtcNow
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Usuario registrado exitosamente.",
                user = new
                {
                    usuario.Id,
                    usuario.Username,
                    usuario.Email,
                    usuario.Rol,
                    usuario.FechaRegistro
                }
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Email y contraseña son obligatorios.");
            }

            var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
            {
                return Unauthorized("El usuario no existe.");
            }

            var isPasswordValid = VerifyPassword(request.Password, user.PasswordHash!);
            if (!isPasswordValid)
            {
                return Unauthorized("Credenciales incorrectas.");
            }

            var token = _authService.GenerateJwtToken(user);

            return Ok(new
            {
                message = "Inicio de sesión exitoso.",
                token = token,
                user = new
                {
                    user.Id,
                    user.Username,
                    user.Email,
                    user.Rol,
                    user.FechaRegistro
                }
            });

        }

        private bool VerifyPassword(string inputPassword, string storedPasswordHash)
        {
            string hashedInput = HashPassword(inputPassword);
            return hashedInput == storedPasswordHash;
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

    }
}