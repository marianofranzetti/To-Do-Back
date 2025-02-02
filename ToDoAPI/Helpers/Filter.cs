using ToDoAPI.Constantes;
using ToDoAPI.DTOs;
using ToDoAPI.Entidades;
using System.Globalization;

namespace ToDoAPI.Helpers
{
    public static class Filter
    {
        public static IQueryable<Tarea> ObtenerFiltroFinal(IQueryable<Tarea> tareas, TareasFiltrarDTO tareasFiltrarDTO)
        {
            if (!string.IsNullOrWhiteSpace(tareasFiltrarDTO.nombre))
            {
                tareas = tareas.Where(p => p.Nombre.Contains(tareasFiltrarDTO.nombre));
            }

            if (!string.IsNullOrWhiteSpace(tareasFiltrarDTO.descripcion))
            {
                tareas = tareas.Where(p => p.Descripcion.Contains(tareasFiltrarDTO.descripcion));
            }

            if (!string.IsNullOrWhiteSpace(tareasFiltrarDTO.fechaLimite) && tareasFiltrarDTO.fechaLimite != "null")
            {
                // Convertir el string a DateTimeOffset (respeta la zona horaria)
                DateTimeOffset fechaOffset = DateTimeOffset.ParseExact(tareasFiltrarDTO.fechaLimite, Fecha.FormatoFecha, CultureInfo.InvariantCulture);

                // Convertirlo a DateTime en UTC o local según necesidad
                DateTime fechaFinal = fechaOffset.UtcDateTime;

                tareas = tareas.Where(p => p.FechaLimite >= fechaFinal);
            }

            return tareas;
        }
    }
}
