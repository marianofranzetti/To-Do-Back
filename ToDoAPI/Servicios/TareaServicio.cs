using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ToDoAPI.DTOs;
using ToDoAPI.Entidades;
using ToDoAPI.Utilidades;

namespace ToDoAPI.Servicios
{
    public class TareaServicio : ITareaServicio
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserManager<IdentityUser> userManager;
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly ILogger<TareaServicio> _logger;

        public TareaServicio(IHttpContextAccessor httpContextAccessor,
            UserManager<IdentityUser> userManager,
            ApplicationDbContext context,
            IMapper mapper,
            ILogger<TareaServicio> logger)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.userManager = userManager;
            this.context = context;
            this.mapper = mapper;
            _logger = logger;
        }

        public async Task ActualizarTarea(int id, TareaCreacionDTO creacionDTO)
        {

            try
            {
                _logger.LogInformation($"actuanlizando tarea {id}");
                var entidadExiste = await context.Tareas.AnyAsync(g => g.Id == id);

                if (!entidadExiste)
                    return;

                var entidad = mapper.Map<Tarea>(creacionDTO);
                entidad.Id = id;

                context.Update(entidad);
                await context.SaveChangesAsync();

                _logger.LogInformation($"tarea {id} actualizada con exito");

            }
            catch (Exception e)
            {
                _logger.LogError($"error actualizando tarea {e}");
                throw new InvalidOperationException($"error al actualizar tarea {e}");
            }
           
        }

        public async Task<TareaDTO> CrearTarea(TareaCreacionDTO creacionDTO)
        {
            try
            {
                _logger.LogInformation($"creando tarea nueva");

                var entidad = mapper.Map<Tarea>(creacionDTO);
                context.Add(entidad);
                await context.SaveChangesAsync();
                var entidadDTO = mapper.Map<TareaDTO>(entidad);

                _logger.LogInformation($"tarea nueva creada con exito {entidad.Id}");

                return entidadDTO;
            }
            catch (Exception e)
            {
                _logger.LogError($"error creando tarea {e}");
                throw new InvalidOperationException($"error al creando tarea {e}");
            }
            
        }

        public async Task eliminarTarea(int id)
        {
            try
            {
                _logger.LogInformation($"eliminando tarea");

                var tarea = await context.Tareas.FirstOrDefaultAsync(x => x.Id == id);
                context.Tareas.Remove(tarea!);
                await context.SaveChangesAsync();

                _logger.LogInformation($"tarea eliminada con exito");
            }
            catch (Exception e)
            {
                _logger.LogError($"error eliminando tarea {e}");
                throw new InvalidOperationException($"error al eliminar tarea {e}");
            }
            
        }

        public async Task<TareaDTO> ObtenerTareaPorId(int id)
        {
            try
            {
                _logger.LogInformation($"obteniendo tarea por id");
                var result = await context.Tareas.FirstOrDefaultAsync(x => x.Id == id);

                return mapper.Map<TareaDTO>(result);
            }
            catch (Exception e)
            {
                _logger.LogError($"error obteniendo tarea {e}");
                throw new InvalidOperationException($"error al obtener tarea {e}");
            }

        }

        public async Task<List<TareaDTO>> ObtenerTareas()
        {
            try
            {
                _logger.LogInformation($"obteniendo tareas");

                var result = await context.Tareas
                    .AsNoTracking()
                    .OrderBy(t => t.Nombre)
                    .ToListAsync();

                _logger.LogInformation($"tareas obtenidas {result.Count}");

                return mapper.Map<List<TareaDTO>>(result);
            }
            catch (Exception e)
            {
                _logger.LogError($"error obteniendo tareas {e}");
                throw new InvalidOperationException($"error al obtener tareas {e}");
            }
        }

        public async Task<List<TareaDTO>> ObtenerTareasPaginadas(PaginacionDTO paginacion, HttpContext httpContext)
        {
            try
            {

                _logger.LogInformation($"obteniendo tareas paginadas");

                var query = context.Tareas.AsQueryable();
                await httpContext.InsertarParametrosPaginacionEnCabecera(query);

                return await query
                    .OrderBy(x => x.Nombre)
                    .Paginar(paginacion)
                    .ProjectTo<TareaDTO>(mapper.ConfigurationProvider)
                    .ToListAsync();

            }
            catch (Exception e)
            {
                _logger.LogError($"error obteniendo tareas {e}");
                throw new InvalidOperationException($"error al obtener tareas {e}");
            }

        }
    }
}
