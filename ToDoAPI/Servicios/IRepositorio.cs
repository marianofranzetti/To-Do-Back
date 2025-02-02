using ToDoAPI.DTOs;
using System.Linq.Expressions;

namespace ToDoAPI.Servicios
{
    public interface IRepository<TEntidad> where TEntidad : class
    {
        Task<List<TEntidad>> ObtenerTodosAsync();
        Task<TEntidad> ObtenerPorIdAsync(int id);
        Task CrearAsync(TEntidad entidad);
        Task ActualizarAsync(TEntidad entidad);
        Task EliminarAsync(int id);
        Task<List<TDTO>> ObtenerConPaginacionAsync<TDTO>(PaginacionDTO paginacion,
        Expression<Func<TEntidad, object>> ordenarPor);


    }

}
