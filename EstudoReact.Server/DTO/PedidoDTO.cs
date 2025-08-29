namespace EstudoReact.Server.DTO
{
    public class PedidoDTO
    {
        public PedidoDTO()
        {
            ItemPedidoDTOs = new List<ItemPedidoDTO>();
        }
        public int Id { get; set; }
        public DateTime DataInclusao { get; set; }
        public string Comprador { get; set; }
        public int IdComprador { get; set; }
        public List<ItemPedidoDTO> ItemPedidoDTOs { get; set; }
    }
}
