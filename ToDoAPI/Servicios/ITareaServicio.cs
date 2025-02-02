using ToDoAPI.DTOs;
using ToDoAPI.Entidades;

namespace ToDoAPI.Servicios
{
    public interface ITareaServicio
    {
        public Task<List<TareaDTO>> ObtenerTareas();
        public Task<TareaDTO> ObtenerTareaPorId(int id);
        public Task<TareaDTO> CrearTarea(TareaCreacionDTO creacionDTO);
        public Task ActualizarTarea(int id, TareaCreacionDTO creacionDTO);
        public Task eliminarTarea(int id);
        public Task<List<TareaDTO>> ObtenerTareasPaginadas(PaginacionDTO paginacion, HttpContext httpContext);
    }
}
