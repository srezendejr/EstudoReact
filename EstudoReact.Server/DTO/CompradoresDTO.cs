namespace EstudoReact.Server.DTO
{
    public class CompradoresDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string? CidadeNome { get; set; }
        public int IdCidade { get; set; }
        public string Documento { get; set; }
        public int IdEstado { get; set; }
        public string? EstadoNome { get; set; }
    }
}
