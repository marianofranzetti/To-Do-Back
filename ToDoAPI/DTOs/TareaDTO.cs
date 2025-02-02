using ToDoAPI.Entidades;
using System.ComponentModel.DataAnnotations;

namespace ToDoAPI.DTOs
{
    public class TareaDTO : IId
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public required string Descripcion { get; set; }
        public DateTime FechaLimite { get; set; }

    }
}
