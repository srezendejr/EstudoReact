namespace EstudoReact.Server.DTO
{
    public class ItemPedidoDTO
    {
        public int Item {  get; set; }
        public int IdPedido { get; set; }
        public int IdProduto { get; set; }
        public string? Produto {  get; set; }
        public decimal Valor {  get; set; }
        public string? Moeda {  get; set; }
    }
}
