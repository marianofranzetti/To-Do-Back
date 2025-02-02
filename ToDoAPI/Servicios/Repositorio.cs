using AutoMapper.QueryableExtensions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ToDoAPI.DTOs;
using System.Linq.Expressions;

namespace ToDoAPI.Servicios
{
    public class Repository<TEntidad> : IRepository<TEntidad> where TEntidad : class
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper _mapper;

        public Repository(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            _mapper = mapper;
        }

        public async Task<List<TEntidad>> ObtenerTodosAsync()
        {
            return await context.Set<TEntidad>().ToListAsync();
        }

        public async Task<TEntidad> ObtenerPorIdAsync(int id)
        {
            return await context.Set<TEntidad>().FindAsync(id);
        }

        public async Task CrearAsync(TEntidad entidad)
        {
            await context.Set<TEntidad>().AddAsync(entidad);
            await context.SaveChangesAsync();
        }

        public async Task ActualizarAsync(TEntidad entidad)
        {
            context.Set<TEntidad>().Update(entidad);
            await context.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            var entidad = await ObtenerPorIdAsync(id);
            if (entidad != null)
            {
                context.Set<TEntidad>().Remove(entidad);
                await context.SaveChangesAsync();
            }
        }

        public async Task<List<TDTO>> ObtenerConPaginacionAsync<TDTO>(PaginacionDTO paginacion, Expression<Func<TEntidad, object>> ordenarPor)
        {
            var queryable = context.Set<TEntidad>().AsQueryable();

            // Aplicar ordenación
            queryable = queryable.OrderBy(ordenarPor);

            // Aplicar paginación
            queryable = queryable.Skip((paginacion.Pagina - 1) * paginacion.RecordsPorPagina)
                                 .Take(paginacion.RecordsPorPagina);

            // Convertir a DTO
            var result = await queryable.ProjectTo<TDTO>(_mapper.ConfigurationProvider).ToListAsync();
            return result;
        }

        public async Task<List<TDTO>> Get<TEntidad, TDTO>(Expression<Func<TEntidad, object>> ordenarPor) where TEntidad : class
        {
            return await context.Set<TEntidad>()
                .OrderBy(ordenarPor)
                .ProjectTo<TDTO>(_mapper.ConfigurationProvider).ToListAsync();
        }
    }

}
