namespace EstudoReact.Server.DTO
{
    public class PedidoDTO
    {
        public PedidoDTO()
        {
            Itens = new List<ItemPedidoDTO>();
        }
        public int Id { get; set; }
        public DateTime DataInclusao { get; set; }
        public string? Comprador { get; set; }
        public int IdComprador { get; set; }
        public List<ItemPedidoDTO> Itens { get; set; }
    }
}
