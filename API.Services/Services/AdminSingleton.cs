using API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class AdminSingleton
{
    private static AdminSingleton? _instance;
    private static readonly object _lock = new();
    private readonly AppDbContext _context;
    private readonly ILogger<AdminSingleton> _logger;
    private readonly AuthService _authService;

    private AdminSingleton(AppDbContext context, ILogger<AdminSingleton> logger, AuthService authService)
    {
        _context = context;
        _logger = logger;
        _authService = authService;
    }

    public static AdminSingleton GetInstance(AppDbContext context, ILogger<AdminSingleton> logger, AuthService authService)
    {
        if (_instance == null)
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new AdminSingleton(context, logger, authService);
                }
            }
        }
        return _instance;
    }

    public async Task InitializeAdminAsync()
    {
        try
        {
            if (_context.Database.CanConnect())
            {
                var adminExistente = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Rol == "Administrador");

                if (adminExistente == null)
                {
                    _logger.LogWarning("No se encontró un administrador en la base de datos.");
                }
                else
                {
                    _logger.LogInformation("Administrador encontrado.");
                }
            }
            else
            {
                _logger.LogError("No se pudo conectar a la base de datos.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error al verificar o crear el administrador: {ex.Message}");
        }
    }
}

