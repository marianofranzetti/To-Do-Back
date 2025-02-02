using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;
using ToDoAPI.Constantes;
using ToDoAPI.DTOs;
using ToDoAPI.Entidades;
using ToDoAPI.Helpers;
using ToDoAPI.Servicios;
using ToDoAPI.Utilidades;
using System.Globalization;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ToDoAPI.Controllers
{
    [Route("api/tareas")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TareasController : ControllerBase
    {
        private readonly IOutputCacheStore outputCacheStore;
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private const string cacheTag = "tareas";
        private readonly ITareaServicio servicio;

        public TareasController(IOutputCacheStore outputCacheStore, ApplicationDbContext context,
            IMapper mapper, ITareaServicio servicio)
        {
            this.outputCacheStore = outputCacheStore;
            this.context = context;
            this.mapper = mapper;
            this.servicio = servicio;
        }

        [HttpGet]
        [OutputCache(Tags = [cacheTag])]
        public async Task<List<TareaDTO>> Get([FromQuery] PaginacionDTO paginacion)
        {
            return await servicio.ObtenerTareasPaginadas(paginacion, HttpContext);
        }

        [HttpGet("all")]
        [OutputCache(Tags = [cacheTag])]
        [AllowAnonymous]
        public async Task<List<TareaDTO>> Get()
        {
            return await servicio.ObtenerTareas();
        }

        [HttpGet("filter")]
        [AllowAnonymous]
        public async Task<ActionResult<List<TareaDTO>>> Filtrar([FromQuery] TareasFiltrarDTO tareasFiltrarDTO)
        {
            var tareasQueryable = Filter.ObtenerFiltroFinal(context.Tareas.AsQueryable(), tareasFiltrarDTO);

            await HttpContext.InsertarParametrosPaginacionEnCabecera(tareasQueryable);

            return await tareasQueryable.Paginar(tareasFiltrarDTO.Paginacion)
                .ProjectTo<TareaDTO>(mapper.ConfigurationProvider).ToListAsync();

        }

        [HttpGet("{id:int}", Name = "ObtenerTareaPorId")]
        [OutputCache(Tags = [cacheTag])]
        public async Task<ActionResult<TareaDTO>> Get(int id)
        {
            return await servicio.ObtenerTareaPorId(id);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TareaCreacionDTO TareaCreacionDTO)
        {
            var result = await servicio.CrearTarea(TareaCreacionDTO);
            return CreatedAtRoute("ObtenerTareaPorId", new { id = result.Id }, result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] TareaCreacionDTO tareaCreacionDTO)
        {
            await servicio.ActualizarTarea(id, tareaCreacionDTO);
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await servicio.eliminarTarea(id);
            return Ok();
        }
    }
}
