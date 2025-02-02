namespace ToDoAPI.DTOs
{
    public class TareasFiltrarDTO
    {
        public int Pagina { get; set; }
        public int RecordsPorPagina { get; set; }

        internal PaginacionDTO Paginacion
        {
            get
            {
                return new PaginacionDTO { Pagina = Pagina, RecordsPorPagina = RecordsPorPagina };
            }
        }

        public string? nombre { get; set; }
        public string? descripcion { get; set; }
        public string? fechaLimite { get; set; }

    }
}
