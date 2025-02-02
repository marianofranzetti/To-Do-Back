namespace ToDoAPI.DTOs
{
    public class TareaCreacionDTO
    {
        public required string Nombre { get; set; }
        public required string Descripcion { get; set; }
        public DateTime FechaLimite { get; set; }
    }
}
