using System.ComponentModel.DataAnnotations;

namespace ToDoAPI.Entidades
{
    public class Tarea : IId
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public required string Nombre { get; set; }

        [Required]
        [StringLength(500)]
        public required string Descripcion { get; set; }

        [Required] 
        public DateTime FechaLimite { get; set; }
    }
}
